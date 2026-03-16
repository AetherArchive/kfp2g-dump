using System;
using System.Collections.Generic;

namespace SGNFW.Common
{
	public static class StringExtensions
	{
		public static int ToInt(this string self, int defaultValue = 0)
		{
			int num = 0;
			if (int.TryParse(self, out num))
			{
				return num;
			}
			return defaultValue;
		}

		public static float ToFloat(this string self, float defaultValue = 0f)
		{
			float num = 0f;
			if (float.TryParse(self, out num))
			{
				return num;
			}
			return defaultValue;
		}

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

		public static bool IsNullOrEmpty(this string self)
		{
			return string.IsNullOrEmpty(self);
		}
	}
}
