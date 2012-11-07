using System;
using Newtonsoft.Json;

namespace HubSharp.Core
{
	[JsonObject(MemberSerialization.OptIn)]
	public class UserPlan
	{
		/// <summary>
		/// Gets or sets the collaborators.
		/// </summary>
		/// <value>
		/// The collaborators.
		/// </value>
		[JsonProperty("collaborators")]
		public int Collaborators { get; set; }

		/// <summary>
		/// Gets or sets the private repositories.
		/// </summary>
		/// <value>
		/// The private repositories.
		/// </value>
		[JsonProperty("private_repos")]
		public int PrivateRepositories { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[JsonProperty("name")]
		public String Name { get; set; }

		/// <summary>
		/// Gets or sets the available space.
		/// </summary>
		/// <value>
		/// The space.
		/// </value>
		[JsonProperty("space")]
		public long Space { get; set; }
	}
}
