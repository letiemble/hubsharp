using System.Runtime.Serialization;

namespace HubSharp.Core
{
	public enum RepositoryType
	{
		[EnumMember(Value = "none")]
		None = 0,
		[EnumMember(Value = "all")]
		All,
		[EnumMember(Value = "owner")]
		Owner,
		[EnumMember(Value = "public")]
		Public,
		[EnumMember(Value = "private")]
		Private,
		[EnumMember(Value = "member")]
		Member,
	}
}
