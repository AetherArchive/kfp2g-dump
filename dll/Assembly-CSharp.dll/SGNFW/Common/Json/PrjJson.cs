using System;
using System.Collections.Generic;
using LitJson;

namespace SGNFW.Common.Json
{
	// Token: 0x02000275 RID: 629
	public class PrjJson
	{
		// Token: 0x0600269F RID: 9887 RVA: 0x001A2BEB File Offset: 0x001A0DEB
		public static T FromJson<T>(string json)
		{
			return JsonMapper.ToObject<T>(json);
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x001A2BF3 File Offset: 0x001A0DF3
		public static List<T> FromJsonList<T>(string json)
		{
			return JsonMapper.ToObject<JsonList<T>>("{ \"list\":" + json + "}").list;
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x001A2C0F File Offset: 0x001A0E0F
		public static string ToJson(object obj)
		{
			return JsonMapper.ToJson(obj);
		}
	}
}
