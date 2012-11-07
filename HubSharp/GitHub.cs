using System;
using System.Net;

namespace HubSharp.Core
{
	public class GitHub
	{
		public const string DefaultBaseUrl = "https://api.github.com";
		public const int DefaultTimeout = 10;

		private Requester request;

		/// <summary>
		/// Initializes a new instance of the <see cref="HubSharp.Core.GitHub"/> class.
		/// </summary>
		public GitHub (String loginOrToken, String password, String baseUrl = DefaultBaseUrl, int timeout = DefaultTimeout)
		{
			this.request = new Requester (loginOrToken, password, baseUrl, timeout);
		}

		/// <summary>
		/// Gets the user.
		/// </summary>
		public User GetUser (String login)
		{
			Tuple<WebHeaderCollection, String> result = this.request.RequestAndCheck ("GET", "/users/" + login, null, null);
			return GitHubObject.Create<User> (result.Item2, this.request);
		}

		/// <summary>
		/// Gets the organization.
		/// </summary>
		public Organization GetOrganization (String login)
		{
			Tuple<WebHeaderCollection, String> result = this.request.RequestAndCheck ("GET", "/orgs/" + login, null, null);
			return GitHubObject.Create<Organization> (result.Item2, this.request);
		}
	}
}
