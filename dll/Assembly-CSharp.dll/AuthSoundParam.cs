using System;

// Token: 0x02000042 RID: 66
public class AuthSoundParam : AbstractAuthParam
{
	// Token: 0x17000023 RID: 35
	// (get) Token: 0x06000154 RID: 340 RVA: 0x0000AE81 File Offset: 0x00009081
	// (set) Token: 0x06000155 RID: 341 RVA: 0x0000AE89 File Offset: 0x00009089
	public string strParam { get; set; }

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x06000156 RID: 342 RVA: 0x0000AE92 File Offset: 0x00009092
	// (set) Token: 0x06000157 RID: 343 RVA: 0x0000AE9A File Offset: 0x0000909A
	public string effectName { get; set; }

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x06000158 RID: 344 RVA: 0x0000AEA3 File Offset: 0x000090A3
	// (set) Token: 0x06000159 RID: 345 RVA: 0x0000AEAB File Offset: 0x000090AB
	public string ctrlParam { get; set; }

	// Token: 0x0600015A RID: 346 RVA: 0x0000AEB4 File Offset: 0x000090B4
	public AuthSoundParam(string strParam)
	{
		this.Parse(strParam);
	}

	// Token: 0x0600015B RID: 347 RVA: 0x0000AEC4 File Offset: 0x000090C4
	private void Parse(string strParam)
	{
		string[] array = strParam.Split(this.SPLIT_STR, StringSplitOptions.None);
		this.effectName = array[0];
		this.ctrlParam = null;
		if (2 < array.Length)
		{
			this.ctrlParam = array[2];
		}
		string text = array[3].Substring(1);
		string text2 = array[4].Substring(1);
		this.startTime = (float)int.Parse(text) * 0.033333335f;
		this.endTime = (float)int.Parse(text2) * 0.033333335f;
		this.endTime -= this.startTime;
	}

	// Token: 0x040001B9 RID: 441
	public float startTime;

	// Token: 0x040001BA RID: 442
	public float endTime;
}
