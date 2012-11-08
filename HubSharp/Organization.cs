using Newtonsoft.Json;

namespace HubSharp.Core
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Organization : NamedEntity
	{
		public Organization ()
		{
		}
	}
}
