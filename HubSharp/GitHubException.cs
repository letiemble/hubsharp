using System;

namespace HubSharp.Core
{
	public class GitHubException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HubSharp.Core.GitHubException"/> class.
		/// </summary>
		public GitHubException ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HubSharp.Core.GitHubException"/> class.
		/// </summary>
		public GitHubException (String message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HubSharp.Core.GitHubException"/> class.
		/// </summary>
		public GitHubException (String message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
