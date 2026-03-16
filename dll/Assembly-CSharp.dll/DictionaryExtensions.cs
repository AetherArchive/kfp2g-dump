using System;
using System.Collections.Generic;

public static class DictionaryExtensions
{
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
