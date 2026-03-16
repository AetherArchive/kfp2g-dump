using System;

public class AuthSoundParam : AbstractAuthParam
{
	public string strParam { get; set; }

	public string effectName { get; set; }

	public string ctrlParam { get; set; }

	public AuthSoundParam(string strParam)
	{
		this.Parse(strParam);
	}

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

	public float startTime;

	public float endTime;
}
