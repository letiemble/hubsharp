using System;
using System.Collections.Generic;
using System.Net;
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
		
		[JsonProperty("closed_issues")]
		public int ClosedIssues { get; set; }

		[JsonProperty("creator")]
		public User Creator { get; set; }

		[JsonProperty("description")]
		public String Description { get; set; }

		[JsonProperty("due_on")]
		public DateTime? DueOn { get; set; }
		
		[JsonProperty("number")]
		public int Number { get; set; }
		
		[JsonProperty("open_issues")]
		public int OpenIssues { get; set; }
		
		[JsonProperty("state")]
		[JsonConverter(typeof(StringEnumConverter))]
		public MilestoneState State { get; set; }
		
		[JsonProperty("title")]
		public String Title { get; set; }
		
		internal static IEnumerable<Milestone> List (Repository owner)
		{
			try {
				String url = String.Format ("{0}/milestones", owner.Url);
				Tuple<WebHeaderCollection, String> result = owner.Requester.RequestAndCheck (WebRequestMethods.Http.Get, url, null, null);
				IList<Milestone> list = JsonConvert.DeserializeObject<List<Milestone>> (result.Item2);
				return list;
			} catch (Exception) {
				return null;
			}
		}
		
		internal static Milestone Get (Repository owner, int number)
		{
			try {
				String url = String.Format ("{0}/milestones/{1}", owner.Url, number);
				Tuple<WebHeaderCollection, String> result = owner.Requester.RequestAndCheck (WebRequestMethods.Http.Get, url, null, null);
				return GitHubObject.Create<Milestone> (result.Item2, owner.Requester);
			} catch (Exception) {
				return null;
			}
		}
	}
}
