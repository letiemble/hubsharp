using System;

namespace HubSharp.Core
{
	public class GitHub : IRequesterProvider
	{
		public const string DefaultBaseUrl = "https://api.github.com";
		public const int DefaultTimeout = 10;

		/// <summary>
		/// Initializes a new instance of the <see cref="HubSharp.Core.GitHub"/> class.
		/// </summary>
		public GitHub (String loginOrToken, String password, String baseUrl = DefaultBaseUrl, int timeout = DefaultTimeout)
		{
			this.Requester = new Requester (loginOrToken, password, baseUrl, timeout);
		}

		public Requester Requester { get; private set; }
		
		/// <summary>
		/// Gets the user.
		/// </summary>
		public User GetUser (String login)
		{
			// Set the path
			String path = String.Format ("/users/{0}", login);
			return GitHubObject.GetObject<User>(this, path);
		}

		/// <summary>
		/// Gets the organization.
		/// </summary>
		public Organization GetOrganization (String login)
		{
			// Set the path
			String path = String.Format ("/orgs/{0}", login);
			return GitHubObject.GetObject<Organization>(this, path);
		}
	}
}
