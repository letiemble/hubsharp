using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HubSharp.Core
{
	[JsonObject(MemberSerialization.OptIn)]
	public class NamedEntity : GitHubObject
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HubSharp.Core.NamedEntity"/> class.
		/// </summary>
		public NamedEntity ()
		{
		}

		/// <summary>
		/// Gets or sets the avatar URL.
		/// </summary>
		[JsonProperty("avatar_url")]
		public String AvatarUrl { get; set; }
		
		/// <summary>
		/// Gets or sets the billing email.
		/// </summary>
		[JsonProperty("billing_email")]
		public String BillingEmail { get; set; }
		
		/// <summary>
		/// Gets or sets the bio.
		/// </summary>
		[JsonProperty("bio")]
		public String Bio { get; set; }

		/// <summary>
		/// Gets or sets the blog.
		/// </summary>
		[JsonProperty("blog")]
		public String Blog { get; set; }

		/// <summary>
		/// Gets or sets the collaborators.
		/// </summary>
		[JsonProperty("collaborators")]
		public int? Collaborators { get; set; }
		
		/// <summary>
		/// Gets or sets the company.
		/// </summary>
		[JsonProperty("company")]
		public String Company { get; set; }
		
		/// <summary>
		/// Gets or sets the disk usage.
		/// </summary>
		[JsonProperty("disk_usage")]
		public long? DiskUsage { get; set; }
		
		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		[JsonProperty("email")]
		public String Email { get; set; }

		/// <summary>
		/// Gets or sets the followers.
		/// </summary>
		[JsonProperty("followers")]
		public int? Followers { get; set; }
		
		/// <summary>
		/// Gets or sets the following.
		/// </summary>
		[JsonProperty("following")]
		public int? Following { get; set; }
		
		/// <summary>
		/// Gets or sets the gravatar identifier.
		/// </summary>
		[JsonProperty("gravatar_id")]
		public String GravatarId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="HubSharp.Core.User"/> is hireable.
		/// </summary>
		[JsonProperty("hireable")]
		public bool? Hireable { get; set; }
		
		/// <summary>
		/// Gets or sets the HTML URL.
		/// </summary>
		[JsonProperty("html_url")]
		public String HtmlUrl { get; set; }
		
		/// <summary>
		/// Gets or sets the location.
		/// </summary>
		[JsonProperty("location")]
		public String Location { get; set; }
		
		/// <summary>
		/// Gets or sets the login.
		/// </summary>
		[JsonProperty("login")]
		public String Login { get; set; }
		
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		[JsonProperty("name")]
		public String Name { get; set; }

		/// <summary>
		/// Gets or sets the owned private repositories.
		/// </summary>
		[JsonProperty("owned_private_repos")]
		public int? OwnedPrivateRepositories { get; set; }
		
		/// <summary>
		/// Gets or sets the plan.
		/// </summary>
		[JsonProperty("plan")]
		public NamedEntityPlan Plan { get; set; }
		
		/// <summary>
		/// Gets or sets the private gists.
		/// </summary>
		[JsonProperty("private_gists")]
		public int? PrivateGists { get; set; }
		
		/// <summary>
		/// Gets or sets the public gists.
		/// </summary>
		[JsonProperty("public_gists")]
		public int? PublicGists { get; set; }
		
		/// <summary>
		/// Gets or sets the public repositories.
		/// </summary>
		[JsonProperty("public_repos")]
		public int? PublicRepositories { get; set; }
		
		/// <summary>
		/// Gets or sets the total private repositories.
		/// </summary>
		[JsonProperty("total_private_repos")]
		public int? TotalPrivateRepositories { get; set; }

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		[JsonProperty("type")]
		[JsonConverter(typeof(StringEnumConverter))]
		public NamedEntityType Type { get; set; }
		
		/// <summary>
		/// Gets the repositories.
		/// </summary>
		public IEnumerable<Repository> GetRepositories (RepositoryType type = RepositoryType.All)
		{
			return Repository.List (this, type);
		}
		
		/// <summary>
		/// Gets the repository.
		/// </summary>
		public Repository GetRepository (String name)
		{
			return Repository.Get (this, name);
		}
	}
}
