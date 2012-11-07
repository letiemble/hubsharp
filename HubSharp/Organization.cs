using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace HubSharp.Core
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Organization : User
	{
		public Organization ()
		{
		}
	}
}
