using Newtonsoft.Json;

namespace HubSharp.Core
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Organization : NamedEntity
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HubSharp.Core.Organization"/> class.
		/// </summary>
		public Organization ()
		{
		}
	}
}
