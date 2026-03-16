using System;
using UnityEngine;

namespace SGNFW.Common.NativePlugin
{
	public static class CustomScheme
	{
		public static bool AnalyzeCustomScheme()
		{
			return CustomScheme.analyzeCustomScheme();
		}

		public static bool IsReady()
		{
			return CustomScheme.isReady();
		}

		public static string[] GetURLSchemeList()
		{
			string urlschemeList = CustomScheme.getURLSchemeList();
			if (!string.IsNullOrEmpty(urlschemeList))
			{
				return urlschemeList.Split(new string[] { CustomScheme.customSchemeDelimiter_ }, StringSplitOptions.RemoveEmptyEntries);
			}
			return null;
		}

		public static void SetURLSchemeList(string[] urlStrings)
		{
			string text = "";
			if (urlStrings != null && urlStrings.Length != 0)
			{
				text = string.Join(CustomScheme.customSchemeDelimiter_, urlStrings);
			}
			CustomScheme.setURLSchemeList(text);
		}

		public static void EraseURLScheme(string url_scheme)
		{
			string[] urlschemeList = CustomScheme.GetURLSchemeList();
			if (urlschemeList != null && urlschemeList.Length != 0)
			{
				CustomScheme.SetURLSchemeList(Array.FindAll<string>(urlschemeList, (string s) => s != url_scheme));
			}
		}

		public static string FindURLScheme(string customScheme)
		{
			string text = null;
			string[] urlschemeList = CustomScheme.GetURLSchemeList();
			if (urlschemeList != null && urlschemeList.Length != 0)
			{
				foreach (string text2 in urlschemeList)
				{
					int num = text2.IndexOf("://");
					if (num >= 0 && text2.Substring(0, num) == customScheme)
					{
						text = text2;
						break;
					}
				}
			}
			return text;
		}

		public static int GetURLSchemeNum()
		{
			string[] urlschemeList = CustomScheme.GetURLSchemeList();
			if (urlschemeList == null)
			{
				return 0;
			}
			return urlschemeList.Length;
		}

		private static bool analyzeCustomScheme()
		{
			return false;
		}

		private static bool isReady()
		{
			return false;
		}

		private static string getURLSchemeList()
		{
			return PlayerPrefs.GetString(CustomScheme.prefsKeyCustomScheme_, "");
		}

		private static void setURLSchemeList(string urlString)
		{
			PlayerPrefs.SetString(CustomScheme.prefsKeyCustomScheme_, urlString);
		}

		private static string prefsKeyCustomScheme_ = "SgnfwCustomScheme";

		private static string customSchemeDelimiter_ = ";";
	}
}
