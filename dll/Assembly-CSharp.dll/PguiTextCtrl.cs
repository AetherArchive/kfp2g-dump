using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E4 RID: 484
public class PguiTextCtrl : PguiBehaviour
{
	// Token: 0x06002058 RID: 8280 RVA: 0x0018ADF8 File Offset: 0x00188FF8
	public static void SetOverflowTypeOptions(ref Text txt, PguiTextCtrl.OverflowTypeOptions overflowType, int fontSize)
	{
		switch (overflowType)
		{
		case PguiTextCtrl.OverflowTypeOptions.ResizeFreely:
			txt.horizontalOverflow = HorizontalWrapMode.Overflow;
			txt.verticalOverflow = VerticalWrapMode.Overflow;
			return;
		case PguiTextCtrl.OverflowTypeOptions.ShrinkContents:
			txt.horizontalOverflow = HorizontalWrapMode.Wrap;
			txt.verticalOverflow = VerticalWrapMode.Overflow;
			txt.resizeTextForBestFit = true;
			txt.resizeTextMaxSize = fontSize;
			return;
		case PguiTextCtrl.OverflowTypeOptions.Clamp:
			txt.horizontalOverflow = HorizontalWrapMode.Wrap;
			txt.verticalOverflow = VerticalWrapMode.Overflow;
			txt.resizeTextForBestFit = false;
			break;
		case PguiTextCtrl.OverflowTypeOptions.Custom:
			break;
		default:
			return;
		}
	}

	// Token: 0x06002059 RID: 8281 RVA: 0x0018AE66 File Offset: 0x00189066
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x0600205A RID: 8282 RVA: 0x0018AE70 File Offset: 0x00189070
	public void Init()
	{
		if (!this.isInit)
		{
			this.isInit = true;
			this.CheckParam();
			this.m_DefaultColor = this.m_Text.color;
			if (this.m_DefaultText == null)
			{
				this.m_DefaultText = this.m_OriginalText;
			}
			PguiOutline[] components = base.GetComponents<PguiOutline>();
			for (int i = 0; i < components.Length; i++)
			{
				this.m_OutlineList.Add(components[i]);
				this.m_DefaultOutlineColor.Add(components[i].effectColor);
			}
		}
	}

	// Token: 0x0600205B RID: 8283 RVA: 0x0018AEF0 File Offset: 0x001890F0
	private void SetParam(string oldStr, string newStr)
	{
		int i = this.m_DefaultParam.IndexOf(oldStr);
		while (i >= this.m_ChangeParam.Count)
		{
			this.m_ChangeParam.Add(this.m_DefaultParam[this.m_ChangeParam.Count]);
		}
		this.m_ChangeParam[i] = newStr;
	}

	// Token: 0x0600205C RID: 8284 RVA: 0x0018AF48 File Offset: 0x00189148
	private void CheckParam()
	{
		if (this.m_Text == null)
		{
			this.m_Text = base.GetComponent<Text>();
		}
		if (string.IsNullOrEmpty(this.m_OriginalText))
		{
			this.m_OriginalText = this.m_Text.text;
		}
		this.m_DefaultParam = new List<string>();
		List<string> list = new List<string>();
		int num = 0;
		for (;;)
		{
			string text = "Param" + (num + 1).ToString("D2");
			if (this.m_OriginalText.IndexOf(text) < 0)
			{
				break;
			}
			this.m_DefaultParam.Add(text);
			if (num < this.m_ChangeParam.Count)
			{
				text = this.m_ChangeParam[num];
			}
			list.Add(text);
			num++;
		}
		this.m_ChangeParam = list;
	}

	// Token: 0x1700044F RID: 1103
	// (set) Token: 0x0600205D RID: 8285 RVA: 0x0018B008 File Offset: 0x00189208
	public float textGrayScale
	{
		set
		{
			if (!this.isInit)
			{
				this.Init();
			}
			this.m_Text.color = new Color(this.m_DefaultColor.r * value, this.m_DefaultColor.g * value, this.m_DefaultColor.b * value, this.m_DefaultColor.a);
			for (int i = 0; i < this.m_OutlineList.Count; i++)
			{
				this.m_OutlineList[i].effectColor = new Color(this.m_DefaultOutlineColor[i].r * value, this.m_DefaultOutlineColor[i].g * value, this.m_DefaultOutlineColor[i].b * value, this.m_DefaultOutlineColor[i].a);
			}
		}
	}

	// Token: 0x0600205E RID: 8286 RVA: 0x0018B0DB File Offset: 0x001892DB
	public void SetTextDefaultColor(Color color)
	{
		this.m_DefaultColor = color;
	}

	// Token: 0x17000450 RID: 1104
	// (get) Token: 0x06002060 RID: 8288 RVA: 0x0018B10D File Offset: 0x0018930D
	// (set) Token: 0x0600205F RID: 8287 RVA: 0x0018B0E4 File Offset: 0x001892E4
	public string text
	{
		get
		{
			if (!(this.m_TextHyphenation != null))
			{
				return this.m_Text.text;
			}
			return this.m_TextHyphenation.Text;
		}
		set
		{
			if (this.m_TextHyphenation != null)
			{
				this.m_TextHyphenation.Text = value;
				return;
			}
			this.m_Text.text = value;
		}
	}

	// Token: 0x06002061 RID: 8289 RVA: 0x0018B134 File Offset: 0x00189334
	public void ReplaceTextByDefault(string oldValue, string newValue)
	{
		if (this.m_DefaultText == null)
		{
			this.CheckParam();
			this.m_DefaultText = this.m_OriginalText;
		}
		this.text = this.m_DefaultText.Replace(oldValue, newValue);
		this.SetParam(oldValue, newValue);
	}

	// Token: 0x06002062 RID: 8290 RVA: 0x0018B16C File Offset: 0x0018936C
	public void ReplaceTextByDefault(string[] oldValue, string[] newValue)
	{
		if (this.m_DefaultText == null)
		{
			this.CheckParam();
			this.m_DefaultText = this.m_OriginalText;
		}
		string text = this.m_DefaultText;
		for (int i = 0; i < oldValue.Length; i++)
		{
			text = text.Replace(oldValue[i], newValue[i]);
			this.SetParam(oldValue[i], newValue[i]);
		}
		this.text = text;
	}

	// Token: 0x06002063 RID: 8291 RVA: 0x0018B1C8 File Offset: 0x001893C8
	public void SetFontSize(int fontSize)
	{
		base.GetComponent<Text>().fontSize = fontSize;
	}

	// Token: 0x06002064 RID: 8292 RVA: 0x0018B1D8 File Offset: 0x001893D8
	public bool AlignRectToText(RectTransform contentsRect)
	{
		float preferredHeight = this.m_Text.preferredHeight;
		RectTransform component = this.m_Text.GetComponent<RectTransform>();
		float num = -(contentsRect.sizeDelta.y - preferredHeight);
		bool flag = num > 0f;
		if (flag)
		{
			component.sizeDelta = new Vector2(0f, num);
			return flag;
		}
		component.sizeDelta = new Vector2(-contentsRect.sizeDelta.x, num);
		return flag;
	}

	// Token: 0x04001779 RID: 6009
	public Text m_Text;

	// Token: 0x0400177A RID: 6010
	public TextHyphenation m_TextHyphenation;

	// Token: 0x0400177B RID: 6011
	private List<PguiOutline> m_OutlineList = new List<PguiOutline>();

	// Token: 0x0400177C RID: 6012
	private string m_DefaultText;

	// Token: 0x0400177D RID: 6013
	private Color m_DefaultColor;

	// Token: 0x0400177E RID: 6014
	private List<Color> m_DefaultOutlineColor = new List<Color>();

	// Token: 0x0400177F RID: 6015
	public PguiTextCtrl.OverflowTypeOptions m_OverflowType;

	// Token: 0x04001780 RID: 6016
	private List<string> m_DefaultParam = new List<string>();

	// Token: 0x04001781 RID: 6017
	public bool customize;

	// Token: 0x04001782 RID: 6018
	[SerializeField]
	[TextArea(3, 10)]
	public string m_OriginalText = "";

	// Token: 0x04001783 RID: 6019
	public List<string> m_ChangeParam = new List<string>();

	// Token: 0x04001784 RID: 6020
	private bool isInit;

	// Token: 0x02001028 RID: 4136
	public enum OverflowTypeOptions
	{
		// Token: 0x04005AB9 RID: 23225
		ResizeFreely,
		// Token: 0x04005ABA RID: 23226
		ShrinkContents,
		// Token: 0x04005ABB RID: 23227
		Clamp,
		// Token: 0x04005ABC RID: 23228
		Custom
	}
}
