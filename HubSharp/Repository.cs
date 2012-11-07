using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace HubSharp.Core
{
	public class Repository : GitHubObject
	{
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
		/// Gets or sets a value indicating whether this <see cref="HubSharp.Core.Repository"/> is fork.
		/// </summary>
		[JsonProperty("fork")]
		public bool Fork { get; set; }

		/// <summary>
		/// Gets or sets the number of forks.
		/// </summary>
		[JsonProperty("forks")]
		public int Forks { get; set; }

		/// <summary>
		/// Gets or sets the fork count.
		/// </summary>
		[JsonProperty("forks_count")]
		public int ForkCount { get; set; }
		
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
		public int OpenIssues { get; set; }

		/// <summary>
		/// Gets or sets the owner.
		/// </summary>
		[JsonProperty("owner")]
		public User Owner { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="HubSharp.Core.Repository"/> is private.
		/// </summary>
		[JsonProperty("private")]
		public bool Private { get; set; }

		/// <summary>
		/// Gets or sets the push date.
		/// </summary>
		[JsonProperty("pushed_at")]
		public DateTime? PushedAt { get; set; }

		/// <summary>
		/// Gets or sets the size.
		/// </summary>
		[JsonProperty("size")]
		public int Size { get; set; }
		
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
		public int Watchers { get; set; }

		/// <summary>
		/// Gets or sets the watchers count.
		/// </summary>
		[JsonProperty("watchers_count")]
		public int WatchersCount { get; set; }
		
		public IEnumerable<Label> GetLabels ()
		{
			return Label.List (this);
		}
		
		public Label GetLabel (String name)
		{
			return Label.Get (this, name);
		}
		
		public IEnumerable<Milestone> GetMilestones ()
		{
			return Milestone.List (this);
		}
		
		public Milestone GetMilestone (int id)
		{
			return Milestone.Get (this, id);
		}
		
		internal static IEnumerable<Repository> List (User owner, RepositoryType type = RepositoryType.All)
		{
			try {
				String path;
				switch (owner.Type) {
				case UserType.User:
					path = String.Format ("/users/{0}/repos", owner.Login);
					break;
				case UserType.Organization:
					path = String.Format ("/orgs/{0}/repos", owner.Login);
					break;
				default:
					throw new NotSupportedException ("Type: " + owner.Type);
				}

				// Set the parameters
				IDictionary<String, String> parameters = new Dictionary<String, String> () {
					{ "type", EnumExtensions.GetMemberValue(type) }
				};
				Tuple<WebHeaderCollection, String> result = owner.Requester.RequestAndCheck (WebRequestMethods.Http.Get, path, parameters, null);
				IList<Repository> list = JsonConvert.DeserializeObject<List<Repository>> (result.Item2);

				return list;
			} catch (Exception) {
				return null;
			}
		}

		internal static Repository Get (User owner, String name)
		{
			try {
				String path = String.Format ("/repos/{0}/{1}", owner.Login, name);
				Tuple<WebHeaderCollection, String> result = owner.Requester.RequestAndCheck (WebRequestMethods.Http.Get, path, null, null);
				return GitHubObject.Create<Repository> (result.Item2, owner.Requester);
			} catch (Exception) {
				return null;
			}
		}
	}
}
