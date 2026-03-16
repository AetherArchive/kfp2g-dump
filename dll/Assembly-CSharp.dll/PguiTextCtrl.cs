using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

public class PguiTextCtrl : PguiBehaviour
{
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

	private void Awake()
	{
		this.Init();
	}

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

	private void SetParam(string oldStr, string newStr)
	{
		int i = this.m_DefaultParam.IndexOf(oldStr);
		while (i >= this.m_ChangeParam.Count)
		{
			this.m_ChangeParam.Add(this.m_DefaultParam[this.m_ChangeParam.Count]);
		}
		this.m_ChangeParam[i] = newStr;
	}

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

	public void SetTextDefaultColor(Color color)
	{
		this.m_DefaultColor = color;
	}

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

	public void SetFontSize(int fontSize)
	{
		base.GetComponent<Text>().fontSize = fontSize;
	}

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

	public Text m_Text;

	public TextHyphenation m_TextHyphenation;

	private List<PguiOutline> m_OutlineList = new List<PguiOutline>();

	private string m_DefaultText;

	private Color m_DefaultColor;

	private List<Color> m_DefaultOutlineColor = new List<Color>();

	public PguiTextCtrl.OverflowTypeOptions m_OverflowType;

	private List<string> m_DefaultParam = new List<string>();

	public bool customize;

	[SerializeField]
	[TextArea(3, 10)]
	public string m_OriginalText = "";

	public List<string> m_ChangeParam = new List<string>();

	private bool isInit;

	public enum OverflowTypeOptions
	{
		ResizeFreely,
		ShrinkContents,
		Clamp,
		Custom
	}
}
