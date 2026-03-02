using System;
using System.Collections.Generic;

namespace SGNFW.Common
{
	// Token: 0x02000259 RID: 601
	public static class ListExtensions
	{
		// Token: 0x060025B0 RID: 9648 RVA: 0x001A0243 File Offset: 0x0019E443
		public static bool AddSafely<TValue>(this IList<TValue> self, TValue value)
		{
			if (value != null && !self.Contains(value))
			{
				self.Add(value);
				return true;
			}
			return false;
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x001A0260 File Offset: 0x0019E460
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
