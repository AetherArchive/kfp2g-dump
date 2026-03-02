using System;
using System.Collections.Generic;

namespace SGNFW.Common
{
	// Token: 0x02000256 RID: 598
	public static class DictionaryExtensions
	{
		// Token: 0x06002563 RID: 9571 RVA: 0x0019F541 File Offset: 0x0019D741
		public static bool AddSafely<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue value)
		{
			if (!self.ContainsKey(key))
			{
				self.Add(key, value);
				return true;
			}
			return false;
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x0019F557 File Offset: 0x0019D757
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
