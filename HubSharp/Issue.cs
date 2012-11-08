using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HubSharp.Core
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Issue : GitHubObject
	{
		public Issue ()
		{
		}

		/// <summary>
		/// Gets or sets the assignee.
		/// </summary>
		[JsonProperty("assignee")]
		public User Assignee { get; set; }

		/// <summary>
		/// Gets or sets the body.
		/// </summary>
		[JsonProperty("body")]
		public String Body { get; set; }

		/// <summary>
		/// Gets or sets the closing date.
		/// </summary>
		[JsonProperty("closed_at")]
		public DateTime? ClosedAt { get; set; }

		/// <summary>
		/// Gets or sets the comments.
		/// </summary>
		[JsonProperty("comments")]
		public int? Comments { get; set; }

		/// <summary>
		/// Gets or sets the HTML URL.
		/// </summary>
		[JsonProperty("html_url")]
		public String HtmlUrl { get; set; }

		/// <summary>
		/// Gets or sets the labels.
		/// </summary>
		[JsonProperty("labels")]
		public Label[] Labels { get; set; }

		/// <summary>
		/// Gets or sets the milestone.
		/// </summary>
		[JsonProperty("milestone")]
		public Milestone Milestone { get; set; }

		/// <summary>
		/// Gets or sets the number.
		/// </summary>
		[JsonProperty("number")]
		public int? Number { get; set; }

		/// <summary>
		/// Gets or sets the pull request.
		/// </summary>
		[JsonProperty("pull_request")]
		public PullRequest PullRequest { get; set; }
		
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

		/// <summary>
		/// Gets or sets the user.
		/// </summary>
		[JsonProperty("user")]
		public User User { get; set; }

		/// <summary>
		/// Gets the surrogate object.
		/// </summary>
		protected override GitHubObject Surrogate {
			get {
				return new Issue.IssueSurrogate (this);
			}
		}

		#region ----- Issue Comments -----
		
		public IEnumerable<IssueComment> GetIssueComments ()
		{
			return IssueComment.List (this);
		}
		
		public IssueComment GetIssueComment (int id)
		{
			return IssueComment.Get (this, id);
		}
		
		public IssueComment CreateIssueComment (String body)
		{
			IssueComment.IssueCommentSurrogate obj = new IssueComment.IssueCommentSurrogate() {
				Body = body,
			};
			return IssueComment.Create(this, obj);
		}
		
		#endregion

		internal static Issue Create (Repository owner, Issue.IssueSurrogate issue)
		{
			String url = String.Format ("{0}/issues", owner.Url);
			return CreateObject<Issue> (owner, url, issue);
		}

		internal static IEnumerable<Issue> List (User owner, ItemState state = ItemState.Open)
		{
			// Set the path
			String path = String.Format ("/users/{0}/issues", owner.Login);
			return List (owner, path, state);
		}
		
		internal static IEnumerable<Issue> List (Organization owner, ItemState state = ItemState.Open)
		{
			// Set the path
			String path = String.Format ("/orgs/{0}/issues", owner.Login);
			return List (owner, path, state);
		}
		
		internal static IEnumerable<Issue> List (Repository owner, ItemState state = ItemState.Open)
		{
			// Set the url
			String url = String.Format ("{0}/issues", owner.Url);
			return List (owner, url, state);
		}

		internal static Issue Get (User owner, int id)
		{
			// Set the path
			String path = String.Format ("/users/{0}/issues/{1}", owner.Login, id);
			return GetObject<Issue> (owner, path);
		}
		
		internal static Issue Get (Organization owner, int id)
		{
			// Set the path
			String path = String.Format ("/orgs/{0}/issues/{1}", owner.Login, id);
			return GetObject<Issue> (owner, path);
		}
		
		internal static Issue Get (Repository owner, int id)
		{
			// Set the url
			String url = String.Format ("{0}/issues/{1}", owner.Url, id);
			return GetObject<Issue> (owner, url);
		}
		
		private static IEnumerable<Issue> List (GitHubObject owner, String urlOrPath, ItemState state = ItemState.Open)
		{
			// Set the parameters
			IDictionary<String, String> parameters = new Dictionary<String, String> () {
				{ "state", EnumExtensions.GetMemberValue(state) }
			};
			
			return GetList<Issue> (owner, urlOrPath, parameters);
		}
		
		/// <summary>
		/// This class is used to generate the required JSON for issue creation/modification.
		/// </summary>
		[JsonObject(MemberSerialization.OptIn)]
		internal class IssueSurrogate : GitHubObject
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="HubSharp.Core.Issue+IssueSurrogate"/> class.
			/// </summary>
			public IssueSurrogate ()
			{
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="HubSharp.Core.Issue+IssueSurrogate"/> class.
			/// </summary>
			public IssueSurrogate (Issue issue)
			{
				this.Assignee = issue.Assignee.Login;
				this.Body = issue.Body;
				this.Labels = issue.Labels.Select (l => l.Name).ToArray ();
				this.Milestone = (issue.Milestone != null) ? issue.Milestone.Number : null;
				this.State = issue.State;
				this.Title = issue.Title;
			}
			
			/// <summary>
			/// Gets or sets the assignee.
			/// </summary>
			[JsonProperty("assignee")]
			public String Assignee { get; set; }
			
			/// <summary>
			/// Gets or sets the body.
			/// </summary>
			[JsonProperty("body")]
			public String Body { get; set; }

			/// <summary>
			/// Gets or sets the labels.
			/// </summary>
			[JsonProperty("labels")]
			public String[] Labels { get; set; }

			/// <summary>
			/// Gets or sets the milestone.
			/// </summary>
			[JsonProperty("milestone")]
			public int? Milestone { get; set; }

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
		}
	}
}
