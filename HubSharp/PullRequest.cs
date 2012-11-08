using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HubSharp.Core
{
	[JsonObject(MemberSerialization.OptIn)]
	public class PullRequest : GitHubObject
	{
		public PullRequest ()
		{
		}
	}
}
