using System.Runtime.Serialization;

namespace HubSharp.Core
{
	public enum ItemState
	{
		[EnumMember(Value = "none")]
		None = 0,
		[EnumMember(Value = "open")]
		Open,
		[EnumMember(Value = "closed")]
		Closed,
	}
}
