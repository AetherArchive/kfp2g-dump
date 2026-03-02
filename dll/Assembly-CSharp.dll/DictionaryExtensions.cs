using System;
using System.Collections.Generic;

// Token: 0x020000FB RID: 251
public static class DictionaryExtensions
{
	// Token: 0x06000C18 RID: 3096 RVA: 0x00047F54 File Offset: 0x00046154
	public static TValue TryGetValueEx<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue defaultValue)
	{
		if (source == null)
		{
			return defaultValue;
		}
		if (!source.ContainsKey(key))
		{
			return defaultValue;
		}
		return source[key];
	}
}
