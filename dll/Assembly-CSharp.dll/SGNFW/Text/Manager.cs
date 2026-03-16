using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SGNFW.Common.Json;

namespace SGNFW.Text
{
	public class Manager
	{
		public static Manager Instance
		{
			get
			{
				return Manager.instance_;
			}
		}

		public static bool IsSetup
		{
			get
			{
				return 0 < Manager.instance_.texts_.Count;
			}
		}

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

		public static Manager.OnCustomKeywordDelegate OnCustomKeyword
		{
			set
			{
				Manager.instance_.onCustomKeyword = value;
			}
		}

		public Manager(string customTag = "")
		{
			this.regex_ = new Regex("\\[(arg|key|sys)\\s+([a-zA-Z0-9_=]+)\\s*\\]");
			this.customTag_ = customTag;
			this.createRegex();
		}

		public static void Initialize(string customTag = "")
		{
			if (Manager.instance_ == null)
			{
				Manager.instance_ = new Manager(customTag);
			}
		}

		public static void Terminate()
		{
			Manager.instance_ = null;
		}

		public static string GetText(string key, params object[] args)
		{
			return Manager.Instance.replaceTag(Manager.Instance.getText(key), args);
		}

		public static bool Exists(string key)
		{
			return Manager.Instance.texts_.ContainsKey(key);
		}

		public static string Process(string text)
		{
			return Manager.Instance.replaceTag(text, Array.Empty<object>());
		}

		public static void AddJson(string jsonText)
		{
			foreach (KeyValuePair<string, Data> keyValuePair in Data.CreateFromJsonText(jsonText).Dict)
			{
				Manager.Instance.texts_[keyValuePair.Key] = keyValuePair.Value.String;
			}
		}

		public static void SetJson(string jsonData)
		{
			Manager.Clear();
			Manager.AddJson(jsonData);
		}

		public static void Clear()
		{
			Manager.Instance.texts_.Clear();
		}

		public static void ClearSystem()
		{
			Manager.Instance.systemTexts_.Clear();
		}

		public static void SetSystemData(string key, string text)
		{
			Manager.Instance.systemTexts_[key] = text;
		}

		public static void RemoveSystemData(string key)
		{
			if (Manager.Instance.systemTexts_.ContainsKey(key))
			{
				Manager.Instance.systemTexts_.Remove(key);
			}
		}

		public static string GetSystemData(string key)
		{
			if (Manager.Instance.systemTexts_.ContainsKey(key))
			{
				return Manager.Instance.systemTexts_[key];
			}
			return null;
		}

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

		private string getText(string key)
		{
			string text;
			if (!this.texts_.TryGetValue(key, out text))
			{
				return "[未設定]";
			}
			return text;
		}

		private string getSystemText(string key)
		{
			string text;
			if (!this.systemTexts_.TryGetValue(key, out text))
			{
				return "[未設定]";
			}
			return text;
		}

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

		private const string Pattern = "\\[(arg|key|sys)\\s+([a-zA-Z0-9_=]+)\\s*\\]";

		private const string PatternTemplate = "\\[({0})\\s+([a-zA-Z0-9_=]+)\\s*\\]";

		private static Manager instance_;

		public Manager.OnCustomKeywordDelegate onCustomKeyword;

		private Regex regex_;

		private Regex regexCustom_;

		private Dictionary<string, string> texts_ = new Dictionary<string, string>();

		private Dictionary<string, string> systemTexts_ = new Dictionary<string, string>();

		private StringBuilder stringBuilder_ = new StringBuilder();

		private object[] params_;

		private string customTag_;

		public delegate string OnCustomKeywordDelegate(string body, int index, int length, string tag, string key);
	}
}
