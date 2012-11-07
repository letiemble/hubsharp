using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace HubSharp.Core
{
	public static class EnumExtensions
	{
		public static String GetMemberValue<T>(T enumValue) 
		{
			Type enumType = typeof(T);
			String name = enumValue.ToString();
			foreach(FieldInfo info in enumType.GetFields()) {
				if (info.Name != name) {
					continue;
				}
				EnumMemberAttribute attribute = Attribute.GetCustomAttribute(info, typeof(EnumMemberAttribute)) as EnumMemberAttribute;
				if (attribute == null) {
					break;
				}
				return attribute.Value ?? name;
			}
			return name;
		}
	}
}
