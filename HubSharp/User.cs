using Newtonsoft.Json;

namespace HubSharp.Core
{
	[JsonObject(MemberSerialization.OptIn)]
	public class User : NamedEntity
	{
		public User ()
		{
		}
	}
}
