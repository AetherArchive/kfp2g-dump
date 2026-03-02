using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SGNFW.Common.Json;

namespace SGNFW.Text
{
	// Token: 0x02000243 RID: 579
	public class Manager
	{
		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06002470 RID: 9328 RVA: 0x0019C7E3 File Offset: 0x0019A9E3
		public static Manager Instance
		{
			get
			{
				return Manager.instance_;
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06002471 RID: 9329 RVA: 0x0019C7EA File Offset: 0x0019A9EA
		public static bool IsSetup
		{
			get
			{
				return 0 < Manager.instance_.texts_.Count;
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06002472 RID: 9330 RVA: 0x0019C7FE File Offset: 0x0019A9FE
		// (set) Token: 0x06002473 RID: 9331 RVA: 0x0019C80A File Offset: 0x0019AA0A
		public static string CustomTag
		{
			get
			{
				return Manager.instance_.customTag_;
			}
			set
			{
				Manager.instance_.customTag_ = value;
				Manager.instance_.createRegex();
			}
		}

		// Token: 0x17000560 RID: 1376
		// (set) Token: 0x06002474 RID: 9332 RVA: 0x0019C821 File Offset: 0x0019AA21
		public static Manager.OnCustomKeywordDelegate OnCustomKeyword
		{
			set
			{
				Manager.instance_.onCustomKeyword = value;
			}
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x0019C830 File Offset: 0x0019AA30
		public Manager(string customTag = "")
		{
			this.regex_ = new Regex("\\[(arg|key|sys)\\s+([a-zA-Z0-9_=]+)\\s*\\]");
			this.customTag_ = customTag;
			this.createRegex();
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x0019C881 File Offset: 0x0019AA81
		public static void Initialize(string customTag = "")
		{
			if (Manager.instance_ == null)
			{
				Manager.instance_ = new Manager(customTag);
			}
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x0019C895 File Offset: 0x0019AA95
		public static void Terminate()
		{
			Manager.instance_ = null;
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x0019C89D File Offset: 0x0019AA9D
		public static string GetText(string key, params object[] args)
		{
			return Manager.Instance.replaceTag(Manager.Instance.getText(key), args);
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x0019C8B5 File Offset: 0x0019AAB5
		public static bool Exists(string key)
		{
			return Manager.Instance.texts_.ContainsKey(key);
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x0019C8C7 File Offset: 0x0019AAC7
		public static string Process(string text)
		{
			return Manager.Instance.replaceTag(text, Array.Empty<object>());
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x0019C8DC File Offset: 0x0019AADC
		public static void AddJson(string jsonText)
		{
			foreach (KeyValuePair<string, Data> keyValuePair in Data.CreateFromJsonText(jsonText).Dict)
			{
				Manager.Instance.texts_[keyValuePair.Key] = keyValuePair.Value.String;
			}
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x0019C950 File Offset: 0x0019AB50
		public static void SetJson(string jsonData)
		{
			Manager.Clear();
			Manager.AddJson(jsonData);
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x0019C95D File Offset: 0x0019AB5D
		public static void Clear()
		{
			Manager.Instance.texts_.Clear();
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x0019C96E File Offset: 0x0019AB6E
		public static void ClearSystem()
		{
			Manager.Instance.systemTexts_.Clear();
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x0019C97F File Offset: 0x0019AB7F
		public static void SetSystemData(string key, string text)
		{
			Manager.Instance.systemTexts_[key] = text;
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x0019C992 File Offset: 0x0019AB92
		public static void RemoveSystemData(string key)
		{
			if (Manager.Instance.systemTexts_.ContainsKey(key))
			{
				Manager.Instance.systemTexts_.Remove(key);
			}
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x0019C9B7 File Offset: 0x0019ABB7
		public static string GetSystemData(string key)
		{
			if (Manager.Instance.systemTexts_.ContainsKey(key))
			{
				return Manager.Instance.systemTexts_[key];
			}
			return null;
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x0019C9E0 File Offset: 0x0019ABE0
		private void createRegex()
		{
			this.stringBuilder_.Length = 0;
			if (string.IsNullOrEmpty(this.customTag_))
			{
				this.regexCustom_ = null;
				return;
			}
			string text = this.stringBuilder_.AppendFormat("\\[({0})\\s+([a-zA-Z0-9_=]+)\\s*\\]", this.customTag_).ToString();
			this.regexCustom_ = new Regex(text);
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x0019CA38 File Offset: 0x0019AC38
		private string getText(string key)
		{
			string text;
			if (!this.texts_.TryGetValue(key, out text))
			{
				return "[未設定]";
			}
			return text;
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x0019CA5C File Offset: 0x0019AC5C
		private string getSystemText(string key)
		{
			string text;
			if (!this.systemTexts_.TryGetValue(key, out text))
			{
				return "[未設定]";
			}
			return text;
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x0019CA80 File Offset: 0x0019AC80
		private bool tryGetParamString(int index, out string str)
		{
			if (this.params_ == null || index < 0 || this.params_.Length <= index)
			{
				str = string.Empty;
				return false;
			}
			str = this.params_[index].ToString();
			return true;
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x0019CAB4 File Offset: 0x0019ACB4
		private string regexMatchEvaluator(Match match)
		{
			string value = match.Groups[1].Value;
			int num;
			string text;
			if (!(value == "arg"))
			{
				if (value == "key")
				{
					return this.getText(match.Groups[2].Value);
				}
				if (value == "sys")
				{
					return this.getSystemText(match.Groups[2].Value);
				}
			}
			else if (int.TryParse(match.Groups[2].Value, out num) && this.tryGetParamString(num, out text))
			{
				return text;
			}
			return match.Value;
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x0019CB5C File Offset: 0x0019AD5C
		private string replaceTag(string text, params object[] args)
		{
			if (text == null)
			{
				return string.Empty;
			}
			this.stringBuilder_.Length = 0;
			text = this.stringBuilder_.AppendFormat(text, args).ToString();
			this.params_ = args;
			try
			{
				text = this.regex_.Replace(text, new MatchEvaluator(this.regexMatchEvaluator));
				if (this.regexCustom_ != null && this.onCustomKeyword != null)
				{
					Match match = this.regexCustom_.Match(text);
					while (match.Success)
					{
						text = this.onCustomKeyword(text, match.Index, match.Length, match.Groups[1].Value, match.Groups[2].Value);
						match = this.regexCustom_.Match(text, match.Index + 1);
					}
				}
			}
			catch
			{
			}
			this.params_ = null;
			return text;
		}

		// Token: 0x04001B36 RID: 6966
		private const string Pattern = "\\[(arg|key|sys)\\s+([a-zA-Z0-9_=]+)\\s*\\]";

		// Token: 0x04001B37 RID: 6967
		private const string PatternTemplate = "\\[({0})\\s+([a-zA-Z0-9_=]+)\\s*\\]";

		// Token: 0x04001B38 RID: 6968
		private static Manager instance_;

		// Token: 0x04001B39 RID: 6969
		public Manager.OnCustomKeywordDelegate onCustomKeyword;

		// Token: 0x04001B3A RID: 6970
		private Regex regex_;

		// Token: 0x04001B3B RID: 6971
		private Regex regexCustom_;

		// Token: 0x04001B3C RID: 6972
		private Dictionary<string, string> texts_ = new Dictionary<string, string>();

		// Token: 0x04001B3D RID: 6973
		private Dictionary<string, string> systemTexts_ = new Dictionary<string, string>();

		// Token: 0x04001B3E RID: 6974
		private StringBuilder stringBuilder_ = new StringBuilder();

		// Token: 0x04001B3F RID: 6975
		private object[] params_;

		// Token: 0x04001B40 RID: 6976
		private string customTag_;

		// Token: 0x0200107E RID: 4222
		// (Invoke) Token: 0x0600531B RID: 21275
		public delegate string OnCustomKeywordDelegate(string body, int index, int length, string tag, string key);
	}
}
