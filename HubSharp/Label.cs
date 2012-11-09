using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HubSharp.Core
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Label : GitHubObject
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HubSharp.Core.Label"/> class.
		/// </summary>
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

		internal static Label Create (Repository owner, Label label)
		{
			String url = String.Format ("{0}/labels", owner.Url);
			return CreateObject<Label> (owner, url, label);
		}

		internal static IEnumerable<Label> List (Repository owner)
		{
			// Set the url
			String url = String.Format ("{0}/labels", owner.Url);
			return GetList<Label> (owner, url);
		}
		
		internal static Label Get (Repository owner, String name)
		{
			// Set the url
			String url = String.Format ("{0}/labels/{1}", owner.Url, name);
			return GetObject<Label> (owner, url);
		}
	}
}
