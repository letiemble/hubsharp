using Newtonsoft.Json;

namespace HubSharp.Core
{
	[JsonObject(MemberSerialization.OptIn)]
	public class User : NamedEntity
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HubSharp.Core.User"/> class.
		/// </summary>
		public User ()
		{
		}
	}
}
