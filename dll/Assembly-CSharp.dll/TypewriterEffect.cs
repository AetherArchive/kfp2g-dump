using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000117 RID: 279
public class TypewriterEffect : MonoBehaviour
{
	// Token: 0x06000D6D RID: 3437 RVA: 0x000593CC File Offset: 0x000575CC
	private void Start()
	{
		this.mTestText.text = "";
		this.mTestText.supportRichText = true;
		this.fntSiz = this.mTestText.fontSize;
		this.posY = base.transform.localPosition.y;
	}

	// Token: 0x06000D6E RID: 3438 RVA: 0x0005941C File Offset: 0x0005761C
	private void Update()
	{
		if (this.dispText.Count <= 0)
		{
			this.dispText.Add("");
			int num = this.currentText.IndexOf("[");
			int num2 = this.currentText.IndexOf(":");
			int num3 = this.currentText.IndexOf("]");
			if (num >= 0 && num2 - num > 1 && num3 - num2 > 1)
			{
				this.rubyText.Add("");
			}
			this.mTestText.lineSpacing = ((this.rubyText.Count > 0) ? 0.45f : 0.85f);
			this.mTestText.fontSize = this.fntSiz * (this.fsiz * 25 + 100) / 100;
			Vector3 localPosition = base.transform.localPosition;
			localPosition.y = this.posY;
			if (this.rubyText.Count > 0)
			{
				localPosition.y += (float)this.fsiz * 4f + 12f;
			}
			base.transform.localPosition = localPosition;
		}
		this.timer += TimeManager.DeltaTime;
		while (this.timer >= this.speed && 0 < this.currentText.Length)
		{
			while (0 < this.currentText.Length)
			{
				if (this.currentText[0] == '\\')
				{
					List<string> list = this.dispText;
					int num4 = this.dispText.Count - 1;
					list[num4] += this.currentText.Substring(0, 2);
					if (this.rubyText.Count > 0)
					{
						list = this.rubyText;
						num4 = this.rubyText.Count - 1;
						list[num4] += "‐";
					}
				}
				else
				{
					if (this.currentText.Length >= 3)
					{
						if (this.currentText.IndexOf("</") == 0)
						{
							int num5 = this.currentText.IndexOf(">");
							if (num5 > 0)
							{
								string text = this.currentText.Substring(2, 1);
								if (this.rich.ContainsKey(text))
								{
									this.rich.Remove(text);
									List<string> list = this.dispText;
									int num4 = this.dispText.Count - 1;
									list[num4] += this.currentText.Substring(0, num5 + 1);
									this.currentText = this.currentText.Substring(num5 + 1);
									if (this.ruby > 0)
									{
										this.ruby -= num5 + 1;
										continue;
									}
									continue;
								}
							}
						}
						else if (this.currentText[0] == '<')
						{
							int num6 = this.currentText.IndexOf(">");
							if (num6 > 0)
							{
								string text2 = this.currentText.Substring(1, 1);
								if (!this.rich.ContainsKey(text2))
								{
									int num7 = this.currentText.IndexOf("</" + text2);
									if (num7 > num6)
									{
										int num8 = this.currentText.Substring(num7).IndexOf(">");
										if (num8 > 0)
										{
											this.rich.Add(text2, new KeyValuePair<int, string>(this.rich.Count, this.currentText.Substring(num7, num8 + 1)));
											List<string> list = this.dispText;
											int num4 = this.dispText.Count - 1;
											list[num4] += this.currentText.Substring(0, num6 + 1);
											this.currentText = this.currentText.Substring(num6 + 1);
											if (this.ruby > 0)
											{
												this.ruby -= num6 + 1;
												continue;
											}
											continue;
										}
									}
								}
							}
						}
						else if (this.rubyText.Count > 0 && this.currentText[0] == '[')
						{
							int num9 = this.currentText.IndexOf(":");
							int num10 = this.currentText.IndexOf("]") - num9;
							if (num9 > 1 && num10 > 1)
							{
								num9--;
								num10--;
								this.ruby = num9;
								this.currentText = this.currentText.Substring(1);
								continue;
							}
						}
						else if (this.currentText[0] == ':')
						{
							int num11 = this.currentText.IndexOf("]");
							if (this.ruby > 0 && num11 > 1)
							{
								int num12 = this.rubyText.Count - 1;
								int num13 = this.ruby * 2 - num11 + 1;
								int num14 = num13 / 2;
								num13 %= 2;
								int num15 = num14;
								this.ruby = 0;
								if (num13 < 0)
								{
									num15 = 0;
									num13 = -num13;
									num14--;
									int num16 = this.rubyText[num12].Length + num14;
									if (num16 < 0)
									{
										num14 += num16;
										num16 = 0;
										num15 = num13;
										num13 = 0;
									}
									this.rubyText[num12] = this.rubyText[num12].Substring(0, num16);
									this.ruby = num14;
									num14 = 0;
								}
								List<string> list;
								int num4;
								if (num14 > 0)
								{
									list = this.rubyText;
									num4 = num12;
									list[num4] += new string('‐', num14);
								}
								if (num13 > 0)
								{
									list = this.rubyText;
									num4 = num12;
									list[num4] += new string('-', num13);
								}
								list = this.rubyText;
								num4 = num12;
								list[num4] = list[num4] + "</color>" + this.currentText.Substring(1, num11 - 1) + "<color=#00000000>";
								if (num15 > 0)
								{
									list = this.rubyText;
									num4 = num12;
									list[num4] += new string('‐', num15);
								}
								if (num13 > 0)
								{
									list = this.rubyText;
									num4 = num12;
									list[num4] += new string('-', num13);
								}
								this.currentText = this.currentText.Substring(num11 + 1);
								continue;
							}
						}
					}
					if (this.currentText[0] == '\r')
					{
						this.currentText = this.currentText.Substring(1);
					}
					else
					{
						if (this.currentText[0] == '\n')
						{
							this.currentText = this.currentText.Substring(1);
							this.dispText.Add("");
							if (this.rubyText.Count > 0)
							{
								this.rubyText.Add("");
							}
							this.ruby = 0;
							break;
						}
						List<string> list = this.dispText;
						int num4 = this.dispText.Count - 1;
						list[num4] += this.currentText.Substring(0, 1);
						this.currentText = this.currentText.Substring(1);
						if (this.rubyText.Count > 0)
						{
							for (int i = 0; i < 2; i++)
							{
								if (this.ruby < 0)
								{
									this.ruby++;
								}
								else if (this.ruby == 0)
								{
									list = this.rubyText;
									num4 = this.rubyText.Count - 1;
									list[num4] += "‐";
								}
							}
							break;
						}
						break;
					}
				}
			}
			this.timer -= this.speed;
		}
		string text3 = "";
		string text4 = ((int)(this.mTestText.color.a * 255f)).ToString("X2");
		bool flag = false;
		for (int j = 0; j < this.dispText.Count; j++)
		{
			if (j > 0)
			{
				text3 += "\n";
			}
			if (j < this.rubyText.Count)
			{
				text3 = string.Concat(new string[]
				{
					text3,
					"<size=",
					(this.mTestText.fontSize / 2).ToString(),
					"><color=#00000000>",
					this.rubyText[j],
					"</color></size>\n"
				});
			}
			string text5 = this.dispText[j];
			while (text5.Length > 0)
			{
				if (flag)
				{
					int num17 = text5.IndexOf(">");
					if (num17 < 0)
					{
						text3 += text5;
						text5 = "";
					}
					else
					{
						if (num17 > 0)
						{
							text3 += text5.Substring(0, num17);
						}
						text3 = text3 + text4 + text5.Substring(num17, 1);
						text5 = text5.Substring(num17 + 1);
						flag = false;
					}
				}
				else
				{
					int num18 = text5.IndexOf("<color=");
					if (num18 < 0)
					{
						text3 += text5;
						text5 = "";
					}
					else
					{
						text3 += text5.Substring(0, num18 + 7);
						text5 = text5.Substring(num18 + 7);
						flag = true;
					}
				}
			}
		}
		List<KeyValuePair<int, string>> list2 = new List<KeyValuePair<int, string>>(this.rich.Values);
		list2.Sort((KeyValuePair<int, string> a, KeyValuePair<int, string> b) => b.Key.CompareTo(a.Key));
		foreach (KeyValuePair<int, string> keyValuePair in list2)
		{
			text3 += keyValuePair.Value;
		}
		this.mTestText.text = text3;
	}

	// Token: 0x1700032C RID: 812
	// (get) Token: 0x06000D6F RID: 3439 RVA: 0x00059E18 File Offset: 0x00058018
	public bool IsCompleteDisplayText
	{
		get
		{
			return 0 >= this.currentText.Length;
		}
	}

	// Token: 0x06000D70 RID: 3440 RVA: 0x00059E2C File Offset: 0x0005802C
	public void SetCurrentText(string text, int size, int spd)
	{
		this.currentText = text;
		this.dispText = new List<string>();
		this.rubyText = new List<string>();
		this.timer = 0f;
		this.rich = new Dictionary<string, KeyValuePair<int, string>>();
		this.ruby = 0;
		this.fsiz = size;
		this.speed = this.intervalForCharacterDisplay;
		if (spd == 1)
		{
			this.speed *= 0.5f;
		}
		else if (spd == 2)
		{
			this.speed = 0f;
		}
		this.mTestText.text = "";
	}

	// Token: 0x06000D71 RID: 3441 RVA: 0x00059EBD File Offset: 0x000580BD
	public bool ClickScreen()
	{
		bool isCompleteDisplayText = this.IsCompleteDisplayText;
		if (!isCompleteDisplayText)
		{
			this.timer = 99999f;
			this.Update();
		}
		return isCompleteDisplayText;
	}

	// Token: 0x04000B08 RID: 2824
	[SerializeField]
	private Text mTestText;

	// Token: 0x04000B09 RID: 2825
	[SerializeField]
	[Range(0.001f, 0.3f)]
	private float intervalForCharacterDisplay = 0.05f;

	// Token: 0x04000B0A RID: 2826
	private float posY;

	// Token: 0x04000B0B RID: 2827
	private int fntSiz;

	// Token: 0x04000B0C RID: 2828
	private string currentText = "";

	// Token: 0x04000B0D RID: 2829
	private List<string> dispText = new List<string>();

	// Token: 0x04000B0E RID: 2830
	private List<string> rubyText = new List<string>();

	// Token: 0x04000B0F RID: 2831
	private float timer;

	// Token: 0x04000B10 RID: 2832
	private Dictionary<string, KeyValuePair<int, string>> rich = new Dictionary<string, KeyValuePair<int, string>>();

	// Token: 0x04000B11 RID: 2833
	private int ruby;

	// Token: 0x04000B12 RID: 2834
	private int fsiz;

	// Token: 0x04000B13 RID: 2835
	private float speed;
}
