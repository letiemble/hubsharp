using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HubSharp.Core
{
	public class Repository : GitHubObject
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HubSharp.Core.Repository"/> class.
		/// </summary>
		public Repository ()
		{
		}

		/// <summary>
		/// Gets or sets the clone URL.
		/// </summary>
		[JsonProperty("clone_url")]
		public String CloneUrl { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		[JsonProperty("description")]
		public String Description { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="HubSharp.Core.Repository"/> is a fork.
		/// </summary>
		[JsonProperty("fork")]
		public bool? Fork { get; set; }

		/// <summary>
		/// Gets or sets the number of forks.
		/// </summary>
		[JsonProperty("forks")]
		public int? Forks { get; set; }

		/// <summary>
		/// Gets or sets the fork count.
		/// </summary>
		[JsonProperty("forks_count")]
		public int? ForkCount { get; set; }
		
		/// <summary>
		/// Gets or sets the full name.
		/// </summary>
		[JsonProperty("full_name")]
		public String FullName { get; set; }

		/// <summary>
		/// Gets or sets the GIT URL.
		/// </summary>
		[JsonProperty("git_url")]
		public String GitUrl { get; set; }

		/// <summary>
		/// Gets or sets the homepage.
		/// </summary>
		[JsonProperty("homepage")]
		public String Homepage { get; set; }

		/// <summary>
		/// Gets or sets the HTML URL.
		/// </summary>
		[JsonProperty("html_url")]
		public String HtmlUrl { get; set; }

		/// <summary>
		/// Gets or sets the language.
		/// </summary>
		[JsonProperty("language")]
		public String Language { get; set; }

		/// <summary>
		/// Gets or sets the master branch.
		/// </summary>
		[JsonProperty("master_branch")]
		public String MasterBranch { get; set; }
		
		/// <summary>
		/// Gets or sets the mirror URL.
		/// </summary>
		[JsonProperty("mirror_url")]
		public String MirrorUrl { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		[JsonProperty("name")]
		public String Name { get; set; }

		/// <summary>
		/// Gets or sets the open issues.
		/// </summary>
		[JsonProperty("open_issues")]
		public int? OpenIssues { get; set; }

		/// <summary>
		/// Gets or sets the owner.
		/// </summary>
		[JsonProperty("owner")]
		public User Owner { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="HubSharp.Core.Repository"/> is private.
		/// </summary>
		[JsonProperty("private")]
		public bool? Private { get; set; }

		/// <summary>
		/// Gets or sets the push date.
		/// </summary>
		[JsonProperty("pushed_at")]
		public DateTime? PushedAt { get; set; }

		/// <summary>
		/// Gets or sets the size.
		/// </summary>
		[JsonProperty("size")]
		public int? Size { get; set; }
		
		/// <summary>
		/// Gets or sets the SSH URL.
		/// </summary>
		[JsonProperty("ssh_url")]
		public String SshUrl { get; set; }

		/// <summary>
		/// Gets or sets the SVN URL.
		/// </summary>
		[JsonProperty("svn_url")]
		public String SvnUrl { get; set; }

		/// <summary>
		/// Gets or sets the number of watchers.
		/// </summary>
		[JsonProperty("watchers")]
		public int? Watchers { get; set; }

		/// <summary>
		/// Gets or sets the watchers count.
		/// </summary>
		[JsonProperty("watchers_count")]
		public int? WatchersCount { get; set; }

		public override Requester Requester {
			get {
				return base.Requester;
			}
			set {
				base.Requester = value;
				if (this.Owner != null) {
					this.Owner.Requester = value;
				}
			}
		}

		public IEnumerable<Label> GetLabels ()
		{
			return Label.List (this);
		}
		
		public Label GetLabel (String name)
		{
			return Label.Get (this, name);
		}

		#region ----- Labels -----

		public Label CreateLabel (String name, String color = null)
		{
			Label obj = new Label () {
				Name = name,
				Color = color
			};
			return Label.Create (this, obj);
		}

		public IEnumerable<Milestone> GetMilestones (ItemState state = ItemState.Open)
		{
			return Milestone.List (this, state);
		}

		#endregion
		
		#region ----- Milestones -----
		
		public Milestone GetMilestone (int id)
		{
			return Milestone.Get (this, id);
		}
		
		public Milestone CreateMilestone (String title, ItemState? state = null, String description = null, DateTime? dueOn = null)
		{
			Milestone obj = new Milestone (){
				Title = title,
				State = state,
				Description = description,
				DueOn = dueOn,
			};
			return Milestone.Create (this, obj);
		}

		#endregion
		
		#region ----- Issues -----

		public IEnumerable<Issue> GetIssues (ItemState state = ItemState.Open)
		{
			return Issue.List (this, state);
		}
		
		public Issue GetIssue (int id)
		{
			return Issue.Get (this, id);
		}
		
		public Issue CreateIssue (String title, String body = null, String assignee = null, int? milestone = null, String[] labels = null)
		{
			Issue.IssueSurrogate obj = new Issue.IssueSurrogate() {
				Title = title,
				Body = body,
				Assignee = assignee,
				Milestone = milestone,
				Labels = labels
			};
			return Issue.Create(this, obj);
		}
		
		#endregion

		#region ----- Repositories -----

		internal static IEnumerable<Repository> List (NamedEntity owner, RepositoryType type = RepositoryType.Public)
		{
			// Set the path
			String path;
			switch (owner.Type) {
			case NamedEntityType.User:
				path = String.Format ("/users/{0}/repos", owner.Login);
				break;
			case NamedEntityType.Organization:
				path = String.Format ("/orgs/{0}/repos", owner.Login);
				break;
			default:
				throw new NotSupportedException ("Type: " + owner.Type);
			}

			// Set the parameters
			IDictionary<String, String> parameters = new Dictionary<String, String> () {
				{ "type", EnumExtensions.GetMemberValue(type) }
			};

			return GetList<Repository> (owner, path, parameters);
		}

		internal static Repository Get (NamedEntity owner, String name)
		{
			// Set the path
			String path = String.Format ("/repos/{0}/{1}", owner.Login, name);

			return GetObject<Repository> (owner, path);
		}

		#endregion
	}
}
