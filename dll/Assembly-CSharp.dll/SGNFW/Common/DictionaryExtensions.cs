using System;
using System.Collections.Generic;

namespace SGNFW.Common
{
	public static class DictionaryExtensions
	{
		public static bool AddSafely<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue value)
		{
			if (!self.ContainsKey(key))
			{
				self.Add(key, value);
				return true;
			}
			return false;
		}

		public static TValue GetSafely<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue defaultValue = default(TValue))
		{
			if (self.ContainsKey(key))
			{
				return self[key];
			}
			return defaultValue;
		}
	}
}
