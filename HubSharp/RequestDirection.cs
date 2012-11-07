using System.Runtime.Serialization;

namespace HubSharp.Core
{
	public enum RequestDirection
	{
		[EnumMember(Value = "none")]
		None = 0,
		[EnumMember(Value = "asc")]
		Ascending,
		[EnumMember(Value = "desc")]
		Descending,
	}
}
