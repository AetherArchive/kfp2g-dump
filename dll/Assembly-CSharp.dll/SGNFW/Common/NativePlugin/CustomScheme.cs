using System;
using UnityEngine;

namespace SGNFW.Common.NativePlugin
{
	// Token: 0x0200026B RID: 619
	public static class CustomScheme
	{
		// Token: 0x06002623 RID: 9763 RVA: 0x001A127F File Offset: 0x0019F47F
		public static bool AnalyzeCustomScheme()
		{
			return CustomScheme.analyzeCustomScheme();
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x001A1286 File Offset: 0x0019F486
		public static bool IsReady()
		{
			return CustomScheme.isReady();
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x001A1290 File Offset: 0x0019F490
		public static string[] GetURLSchemeList()
		{
			string urlschemeList = CustomScheme.getURLSchemeList();
			if (!string.IsNullOrEmpty(urlschemeList))
			{
				return urlschemeList.Split(new string[] { CustomScheme.customSchemeDelimiter_ }, StringSplitOptions.RemoveEmptyEntries);
			}
			return null;
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x001A12C4 File Offset: 0x0019F4C4
		public static void SetURLSchemeList(string[] urlStrings)
		{
			string text = "";
			if (urlStrings != null && urlStrings.Length != 0)
			{
				text = string.Join(CustomScheme.customSchemeDelimiter_, urlStrings);
			}
			CustomScheme.setURLSchemeList(text);
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x001A12F0 File Offset: 0x0019F4F0
		public static void EraseURLScheme(string url_scheme)
		{
			string[] urlschemeList = CustomScheme.GetURLSchemeList();
			if (urlschemeList != null && urlschemeList.Length != 0)
			{
				CustomScheme.SetURLSchemeList(Array.FindAll<string>(urlschemeList, (string s) => s != url_scheme));
			}
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x001A1330 File Offset: 0x0019F530
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

		// Token: 0x06002629 RID: 9769 RVA: 0x001A138C File Offset: 0x0019F58C
		public static int GetURLSchemeNum()
		{
			string[] urlschemeList = CustomScheme.GetURLSchemeList();
			if (urlschemeList == null)
			{
				return 0;
			}
			return urlschemeList.Length;
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x001A13A7 File Offset: 0x0019F5A7
		private static bool analyzeCustomScheme()
		{
			return false;
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x001A13AA File Offset: 0x0019F5AA
		private static bool isReady()
		{
			return false;
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x001A13AD File Offset: 0x0019F5AD
		private static string getURLSchemeList()
		{
			return PlayerPrefs.GetString(CustomScheme.prefsKeyCustomScheme_, "");
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x001A13BE File Offset: 0x0019F5BE
		private static void setURLSchemeList(string urlString)
		{
			PlayerPrefs.SetString(CustomScheme.prefsKeyCustomScheme_, urlString);
		}

		// Token: 0x04001C44 RID: 7236
		private static string prefsKeyCustomScheme_ = "SgnfwCustomScheme";

		// Token: 0x04001C45 RID: 7237
		private static string customSchemeDelimiter_ = ";";
	}
}
