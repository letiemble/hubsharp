using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HubSharp.Core
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Milestone : GitHubObject
	{
		public Milestone ()
		{
		}

		/// <summary>
		/// Gets or sets the closed issues.
		/// </summary>
		[JsonProperty("closed_issues")]
		public int? ClosedIssues { get; set; }

		/// <summary>
		/// Gets or sets the creator.
		/// </summary>
		[JsonProperty("creator")]
		public User Creator { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		[JsonProperty("description")]
		public String Description { get; set; }

		/// <summary>
		/// Gets or sets the due on.
		/// </summary>
		[JsonProperty("due_on")]
		public DateTime? DueOn { get; set; }

		/// <summary>
		/// Gets or sets the number.
		/// </summary>
		[JsonProperty("number")]
		public int? Number { get; set; }

		/// <summary>
		/// Gets or sets the open issues.
		/// </summary>
		[JsonProperty("open_issues")]
		public int? OpenIssues { get; set; }

		/// <summary>
		/// Gets or sets the state.
		/// </summary>
		[JsonProperty("state")]
		[JsonConverter(typeof(StringEnumConverter))]
		public ItemState? State { get; set; }

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		[JsonProperty("title")]
		public String Title { get; set; }
		
		internal static Milestone Create (Repository owner, Milestone milestone)
		{
			String url = String.Format ("{0}/milestones", owner.Url);
			return CreateObject<Milestone> (owner, url, milestone);
		}

		internal static IEnumerable<Milestone> List (Repository owner, ItemState state = ItemState.Open)
		{
			// Set the url
			String url = String.Format ("{0}/milestones", owner.Url);

			// Set the parameters
			IDictionary<String, String> parameters = new Dictionary<String, String> () {
				{ "state", EnumExtensions.GetMemberValue(state) }
			};

			return GetList<Milestone> (owner, url, parameters);
		}
		
		internal static Milestone Get (Repository owner, int number)
		{
			// Set the url
			String url = String.Format ("{0}/milestones/{1}", owner.Url, number);
			return GetObject<Milestone> (owner, url);
		}
	}
}
