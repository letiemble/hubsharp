using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HubSharp.Core
{
	[JsonObject(MemberSerialization.OptIn)]
	public class IssueComment : GitHubObject
	{
		public IssueComment ()
		{
		}
		
		/// <summary>
		/// Gets or sets the body.
		/// </summary>
		[JsonProperty("body")]
		public String Body { get; set; }

		/// <summary>
		/// Gets or sets the assignee.
		/// </summary>
		[JsonProperty("user")]
		public User User { get; set; }

		internal static IssueComment Create (Issue owner, IssueComment.IssueCommentSurrogate issueComment)
		{
			// Set the url
			String url = String.Format ("{0}/comments", owner.Url);
			return CreateObject<IssueComment> (owner, url, issueComment);
		}

		internal static IEnumerable<IssueComment> List (Issue owner)
		{
			// Set the url
			String url = String.Format ("{0}/comments", owner.Url);
			return GetList<IssueComment> (owner, url);
		}
		
		internal static IssueComment Get (Issue owner, int id)
		{
			// Set the url
			String url = String.Format ("{0}/comments/{1}", owner.Url, id);
			return GetObject<IssueComment> (owner, url);
		}

		/// <summary>
		/// This class is used to generate the required JSON for issue comment creation/modification.
		/// </summary>
		[JsonObject(MemberSerialization.OptIn)]
		internal class IssueCommentSurrogate : GitHubObject
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="HubSharp.Core.IssueComment+IssueCommentSurrogate"/> class.
			/// </summary>
			public IssueCommentSurrogate ()
			{
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="HubSharp.Core.IssueComment+IssueCommentSurrogate"/> class.
			/// </summary>
			public IssueCommentSurrogate (IssueComment issueComment)
			{
				this.Body = issueComment.Body;
			}
			
			/// <summary>
			/// Gets or sets the body.
			/// </summary>
			[JsonProperty("body")]
			public String Body { get; set; }
		}
	}
}
