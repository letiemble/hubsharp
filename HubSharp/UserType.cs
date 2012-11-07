using System.Runtime.Serialization;

namespace HubSharp.Core
{
	public enum UserType
	{
		/// <summary>
		/// No type.
		/// </summary>
		[EnumMember(Value="None")]
		None = 0,

		/// <summary>
		/// A user.
		/// </summary>
		[EnumMember(Value="User")]
		User,

		/// <summary>
		/// An organization.
		/// </summary>
		[EnumMember(Value="Organization")]
		Organization
	}
}
