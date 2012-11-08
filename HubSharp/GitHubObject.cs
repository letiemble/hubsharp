using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Reflection;

namespace HubSharp.Core
{
	public abstract class GitHubObject : IRequesterProvider
	{
		private static readonly Regex LINK_PATTERN = new Regex ("<(?<url>.+)>; rel=\"(?<direction>.+)\"");
		private const String DIRECTION_FIRST = "first";
		private const String DIRECTION_PREVIOUS = "prev";
		private const String DIRECTION_NEXT = "next";
		private const String DIRECTION_LAST = "last";

		/// <summary>
		/// Initializes a new instance of the <see cref="HubSharp.Core.GitHubObject"/> class.
		/// </summary>
		protected GitHubObject ()
		{
		}

		/// <summary>
		/// Gets or sets the creation date.
		/// </summary>
		[JsonProperty("created_at")]
		public DateTime? CreateAt { get; set; }

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		[JsonProperty("id")]
		public long? Id { get; set; }

		/// <summary>
		/// Gets or sets the update date.
		/// </summary>
		/// <value>
		/// The updated at.
		/// </value>
		[JsonProperty("updated_at")]
		public DateTime? UpdatedAt { get; set; }

		/// <summary>
		/// Gets or sets the API URL.
		/// </summary>
		/// <value>
		/// The URL.
		/// </value>
		[JsonProperty("url")]
		public String Url { get; set; }

		public virtual Requester Requester { get; set; }

		public bool Completed { get; set; }

		/// <summary>
		/// Complete this instance if it contains only partial data.
		/// </summary>
		public void Complete ()
		{
			if (String.IsNullOrEmpty (this.Url)) {
				throw new NotSupportedException ("Cannot complete object without valid Url.");
			}
			if (this.Completed) {
				return;
			}

			// Perform a request
			var result = this.Requester.Request (Requester.Get, this.Url, null, null);
			if (result.Item1 != HttpStatusCode.OK) {
				return;
			}

			// Parse the result if any
			Type type = this.GetType ();
			GitHubObject complete = Deserialize (result.Item3, type, null, false);

			// Merge result into this instance
			PropertyInfo[] properties = type.GetProperties (BindingFlags.Public | BindingFlags.Instance);
			foreach (PropertyInfo property in properties) {
				// Only copy values from serialized property
				JsonPropertyAttribute attribute = Attribute.GetCustomAttribute (property, typeof(JsonPropertyAttribute)) as JsonPropertyAttribute;
				if (attribute == null) {
					continue;
				}

				// Ignore null value
				Object value = property.GetValue (complete, null);
				if (value == null) {
					continue;
				}

				// Copy value
				property.SetValue(this, value, null);
			}

			this.Completed = true;
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		public bool Update ()
		{
			if (String.IsNullOrEmpty (this.Url)) {
				throw new NotSupportedException ("Cannot update an object without an Url");
			}
			String data = this.Surrogate.ToJson ();
			var result = this.Requester.Request (Requester.Patch, this.Url, null, data);
			return (result.Item1 == HttpStatusCode.OK);
		}

		/// <summary>
		/// Delete this instance.
		/// </summary>
		public bool Delete ()
		{
			if (String.IsNullOrEmpty (this.Url)) {
				throw new NotSupportedException ("Cannot delete an object without an Url");
			}
			var result = this.Requester.Request (Requester.Delete, this.Url, null, null);
			return (result.Item1 == HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Convert this instance to its JSON representation.
		/// </summary>
		public String ToJson (bool ignoreNull = true)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings ();
			settings.NullValueHandling = ignoreNull ? NullValueHandling.Ignore : NullValueHandling.Include;
			return JsonConvert.SerializeObject (this, settings);
		}

		/// <summary>
		/// Gets the surrogate object.
		/// </summary>
		protected virtual GitHubObject Surrogate {
			get {
				return this;
			}
		}

		#region ----- Operation Methods -----

		internal static T CreateObject<T> (IRequesterProvider owner, String urlOrPath, GitHubObject obj) where T : GitHubObject
		{
			try {
				String data = obj.ToJson ();

				var result = owner.Requester.Request (Requester.Post, urlOrPath, null, data);
				if (result.Item1 < HttpStatusCode.BadRequest) {
					return Deserialize<T> (result.Item3, owner.Requester, true);
				}
			} catch (Exception) {
				// Do nothing
			}
			return default(T);
		}

		internal static T GetObject<T> (IRequesterProvider owner, String urlOrPath, IDictionary<String, String> parameters = null) where T : GitHubObject
		{
			try {
				var result = owner.Requester.Request (Requester.Get, urlOrPath, parameters, null);
				if (result.Item1 < HttpStatusCode.BadRequest) {
					return Deserialize<T> (result.Item3, owner.Requester, true);
				}
			} catch (Exception) {
				// Do nothing
			}
			return default(T);
		}

		internal static IEnumerable<T> GetList<T> (IRequesterProvider owner, String urlOrPath, IDictionary<String, String> parameters = null) where T : GitHubObject
		{
			try {
				List<T> allResult = new List<T> ();
				String currentUrlOrPath = urlOrPath;
				IDictionary<String, String> currentParameters = parameters;

				while (true) {
					var result = owner.Requester.Request (Requester.Get, currentUrlOrPath, currentParameters, null);
					if (result.Item1 >= HttpStatusCode.BadRequest) {
						break;
					}
					IEnumerable<T> pageResult = DeserializeList<T> (result.Item3, owner.Requester, true);
					allResult.AddRange (pageResult);

					IDictionary<String, String> links = ExtractLinks (result.Item2);
					if (links.Count == 0) {
						break;
					}
					if (!links.ContainsKey (DIRECTION_NEXT)) {
						break;
					}
					currentUrlOrPath = links [DIRECTION_NEXT];
					currentParameters = null;
				}

				return allResult;
			} catch (Exception) {
				return new T[0];
			}
		}

		private static IDictionary<String, String> ExtractLinks (WebHeaderCollection headers)
		{
			IDictionary<String, String> links = new Dictionary<String, String> ();
			String linkHeadersValue = headers ["Link"];
			if (linkHeadersValue != null) {
				String[] linkHeaders = linkHeadersValue.Split (',');
				foreach (var linkHeader in linkHeaders) {
					Match m = LINK_PATTERN.Match (linkHeader);
					if (!m.Success) {
						continue;
					}
					var dir = m.Groups ["direction"];
					var url = m.Groups ["url"];
					links.Add (dir.Value, url.Value);
				}
			}
			return links;
		}

		#endregion

		#region ----- Factory Methods -----

		internal static T Deserialize<T> (String data, Requester requester = null, bool completed = true) where T : GitHubObject
		{
			return (T)Deserialize (data, typeof(T), requester, completed);
		}

		internal static GitHubObject Deserialize (String data, Type type, Requester requester = null, bool completed = true)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings ();
			GitHubObject obj = (GitHubObject)JsonConvert.DeserializeObject (data, type, settings);
			obj.Requester = requester;
			obj.Completed = completed;
			return obj;
		}

		internal static IEnumerable<T> DeserializeList<T> (String data, Requester requester = null, bool completed = true) where T : GitHubObject
		{
			return DeserializeList (data, typeof(T), requester, completed).Cast<T> ();
		}

		internal static IEnumerable<GitHubObject> DeserializeList (String data, Type type, Requester requester = null, bool completed = true)
		{
			Type listType = typeof(List<>).MakeGenericType (new []{ type });
			JsonSerializerSettings settings = new JsonSerializerSettings ();
			IEnumerable<GitHubObject> list = (IEnumerable<GitHubObject>)JsonConvert.DeserializeObject (data, listType, settings);
			foreach (GitHubObject obj in list) {
				obj.Requester = requester;
				obj.Completed = completed;
			}
			return list;
		}

		#endregion
	}
}
