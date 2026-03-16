using System;
using System.Collections.Generic;

namespace SGNFW.Common
{
	public static class ListExtensions
	{
		public static bool AddSafely<TValue>(this IList<TValue> self, TValue value)
		{
			if (value != null && !self.Contains(value))
			{
				self.Add(value);
				return true;
			}
			return false;
		}

		public static void AddRangeSafely<TValue>(this IList<TValue> self, IEnumerable<TValue> collection)
		{
			if (collection != null)
			{
				foreach (TValue tvalue in collection)
				{
					self.AddSafely(tvalue);
				}
			}
		}
	}
}
