using System;

// Token: 0x0200003E RID: 62
public class AuthEffectParam : AbstractAuthParam
{
	// Token: 0x1700000C RID: 12
	// (get) Token: 0x060000E1 RID: 225 RVA: 0x00006DCB File Offset: 0x00004FCB
	// (set) Token: 0x060000E2 RID: 226 RVA: 0x00006DD3 File Offset: 0x00004FD3
	public string strParam { get; set; }

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x060000E3 RID: 227 RVA: 0x00006DDC File Offset: 0x00004FDC
	// (set) Token: 0x060000E4 RID: 228 RVA: 0x00006DE4 File Offset: 0x00004FE4
	public string locaterName { get; set; }

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x060000E5 RID: 229 RVA: 0x00006DED File Offset: 0x00004FED
	// (set) Token: 0x060000E6 RID: 230 RVA: 0x00006DF5 File Offset: 0x00004FF5
	public string effectName { get; set; }

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x060000E7 RID: 231 RVA: 0x00006DFE File Offset: 0x00004FFE
	// (set) Token: 0x060000E8 RID: 232 RVA: 0x00006E06 File Offset: 0x00005006
	public string index { get; set; }

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x060000E9 RID: 233 RVA: 0x00006E0F File Offset: 0x0000500F
	// (set) Token: 0x060000EA RID: 234 RVA: 0x00006E17 File Offset: 0x00005017
	public string ctrlParam { get; set; }

	// Token: 0x060000EB RID: 235 RVA: 0x00006E20 File Offset: 0x00005020
	public AuthEffectParam(string strParam)
	{
		this.Parse(strParam);
	}

	// Token: 0x060000EC RID: 236 RVA: 0x00006E30 File Offset: 0x00005030
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

	// Token: 0x060000ED RID: 237 RVA: 0x00006F28 File Offset: 0x00005128
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

	// Token: 0x060000EE RID: 238 RVA: 0x00006F54 File Offset: 0x00005154
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

	// Token: 0x04000152 RID: 338
	public const string AUTH_EF_PREFIX = "Ef_";

	// Token: 0x04000153 RID: 339
	public const string AUTH_EF_CTRL_INVALID = "EFNONE";

	// Token: 0x04000159 RID: 345
	public float startTime;

	// Token: 0x0400015A RID: 346
	public float endTime;
}
