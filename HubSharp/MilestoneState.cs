using System.Runtime.Serialization;

namespace HubSharp.Core
{
	public enum MilestoneState
	{
		[EnumMember(Value = "none")]
		None = 0,
		[EnumMember(Value = "open")]
		Open,
		[EnumMember(Value = "close")]
		Closed,
	}
}
