using System;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;

namespace HubSharp.Core
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Label : GitHubObject
	{
		public Label ()
		{
		}
		
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		[JsonProperty("name")]
		public String Name { get; set; }
		
		/// <summary>
		/// Gets or sets the color.
		/// </summary>
		[JsonProperty("color")]
		public String Color { get; set; }

		internal static IEnumerable<Label> List (Repository owner)
		{
			try {
				String url = String.Format ("{0}/labels", owner.Url);
				Tuple<WebHeaderCollection, String> result = owner.Requester.RequestAndCheck (WebRequestMethods.Http.Get, url, null, null);
				IList<Label> list = JsonConvert.DeserializeObject<List<Label>> (result.Item2);
				return list;
			} catch (Exception) {
				return null;
			}
		}
		
		internal static Label Get (Repository owner, String name)
		{
			try {
				String url = String.Format ("{0}/labels/{1}", owner.Url, name);
				Tuple<WebHeaderCollection, String> result = owner.Requester.RequestAndCheck (WebRequestMethods.Http.Get, url, null, null);
				return GitHubObject.Create<Label> (result.Item2, owner.Requester);
			} catch (Exception) {
				return null;
			}
		}
	}
}
