using System;
using System.Collections.Generic;

namespace SGNFW.Common
{
	// Token: 0x0200025C RID: 604
	public static class StringExtensions
	{
		// Token: 0x060025C5 RID: 9669 RVA: 0x001A05F8 File Offset: 0x0019E7F8
		public static int ToInt(this string self, int defaultValue = 0)
		{
			int num = 0;
			if (int.TryParse(self, out num))
			{
				return num;
			}
			return defaultValue;
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x001A0614 File Offset: 0x0019E814
		public static float ToFloat(this string self, float defaultValue = 0f)
		{
			float num = 0f;
			if (float.TryParse(self, out num))
			{
				return num;
			}
			return defaultValue;
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x001A0634 File Offset: 0x0019E834
		public static bool IsMatchAny(this string self, params string[] patterns)
		{
			foreach (string text in patterns)
			{
				if (self == text)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x001A0664 File Offset: 0x0019E864
		public static bool IsMatchAny(this string self, List<string> patterns)
		{
			foreach (string text in patterns)
			{
				if (self == text)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x001A06BC File Offset: 0x0019E8BC
		public static bool ContainsAny(this string self, params string[] patterns)
		{
			foreach (string text in patterns)
			{
				if (self.Contains(text))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x001A06EC File Offset: 0x0019E8EC
		public static bool ContainsAny(this string self, List<string> patterns)
		{
			foreach (string text in patterns)
			{
				if (self.Contains(text))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x001A0744 File Offset: 0x0019E944
		public static bool StartsWithAny(this string self, params string[] patterns)
		{
			foreach (string text in patterns)
			{
				if (self.StartsWith(text))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x001A0774 File Offset: 0x0019E974
		public static bool StartsWithAny(this string self, List<string> patterns)
		{
			foreach (string text in patterns)
			{
				if (self.StartsWith(text))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x001A07CC File Offset: 0x0019E9CC
		public static bool EndsWithAny(this string self, params string[] patterns)
		{
			foreach (string text in patterns)
			{
				if (self.EndsWith(text))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x001A07FC File Offset: 0x0019E9FC
		public static bool EndsWithAny(this string self, List<string> patterns)
		{
			foreach (string text in patterns)
			{
				if (self.EndsWith(text))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x001A0854 File Offset: 0x0019EA54
		public static bool ContainsAll(this string self, params string[] patterns)
		{
			foreach (string text in patterns)
			{
				if (!self.Contains(text))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x001A0884 File Offset: 0x0019EA84
		public static bool ContainsAll(this string self, List<string> patterns)
		{
			foreach (string text in patterns)
			{
				if (!self.Contains(text))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x001A08DC File Offset: 0x0019EADC
		public static bool IsNullOrEmpty(this string self)
		{
			return string.IsNullOrEmpty(self);
		}
	}
}
