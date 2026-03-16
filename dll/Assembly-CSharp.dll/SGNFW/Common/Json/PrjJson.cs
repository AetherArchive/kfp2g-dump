using System;
using System.Collections.Generic;
using LitJson;

namespace SGNFW.Common.Json
{
	public class PrjJson
	{
		public static T FromJson<T>(string json)
		{
			return JsonMapper.ToObject<T>(json);
		}

		public static List<T> FromJsonList<T>(string json)
		{
			return JsonMapper.ToObject<JsonList<T>>("{ \"list\":" + json + "}").list;
		}

		public static string ToJson(object obj)
		{
			return JsonMapper.ToJson(obj);
		}
	}
}
