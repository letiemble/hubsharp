using System;
using Newtonsoft.Json;

namespace HubSharp.Core
{
	public class GitHubObject
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HubSharp.Core.GitHubObject"/> class.
		/// </summary>
		public GitHubObject ()
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
		public int Id { get; set; }

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

		public Requester Requester { get; set; }

		public String Serialize (bool ignoreNull = true)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings ();
			settings.NullValueHandling = ignoreNull ? NullValueHandling.Ignore : NullValueHandling.Include;
			return JsonConvert.SerializeObject (this, settings);
		}

		public static T Create<T> (String data, Requester requester = null) where T : GitHubObject
		{
			JsonSerializerSettings settings = new JsonSerializerSettings ();
			//settings.MissingMemberHandling = MissingMemberHandling.Error;
			T obj = JsonConvert.DeserializeObject<T> (data, settings);
			obj.Requester = requester;

			return obj;
		}
	}
}
