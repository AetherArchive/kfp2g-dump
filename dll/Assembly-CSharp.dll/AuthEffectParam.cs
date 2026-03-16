using System;

public class AuthEffectParam : AbstractAuthParam
{
	public string strParam { get; set; }

	public string locaterName { get; set; }

	public string effectName { get; set; }

	public string index { get; set; }

	public string ctrlParam { get; set; }

	public AuthEffectParam(string strParam)
	{
		this.Parse(strParam);
	}

	private void Parse(string strParam)
	{
		this.strParam = strParam;
		string[] array = strParam.Split(this.SPLIT_STR, StringSplitOptions.None);
		this.effectName = array[0];
		this.index = array[1];
		this.locaterName = this.effectName + "__" + this.index;
		if (2 < array.Length)
		{
			this.ctrlParam = array[2];
		}
		else
		{
			this.ctrlParam = "EFNONE";
		}
		if (!this.ctrlParam.Equals("EFNONE"))
		{
			this.locaterName = this.locaterName + "__" + this.ctrlParam;
		}
		this.startTime = 0f;
		if (3 < array.Length)
		{
			string text = AuthEffectParam.Substring(array[3], 1);
			this.startTime = (float)AuthEffectParam.ParseInt(text) * 0.033333335f;
		}
		this.endTime = 0f;
		if (4 < array.Length)
		{
			string text2 = AuthEffectParam.Substring(array[4], 1);
			this.endTime = (float)AuthEffectParam.ParseInt(text2) * 0.033333335f;
		}
	}

	public static int ParseInt(string str)
	{
		int num = 0;
		try
		{
			num = int.Parse(str);
		}
		catch (FormatException)
		{
		}
		return num;
	}

	public static string Substring(string str, int index)
	{
		string text = "";
		if (str == null)
		{
			return text;
		}
		if (str.Length < index)
		{
			return text;
		}
		return str.Substring(index);
	}

	public const string AUTH_EF_PREFIX = "Ef_";

	public const string AUTH_EF_CTRL_INVALID = "EFNONE";

	public float startTime;

	public float endTime;
}
