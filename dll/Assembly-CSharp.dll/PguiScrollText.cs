using System;
using UnityEngine;
using UnityEngine.UI;

public class PguiScrollText : PguiBehaviour
{
	public void SetText(string text)
	{
		this.m_PguiText.text = text;
		this.RefreshRect();
	}

	private void RefreshRect()
	{
		Text text = this.m_PguiText.m_Text;
		text.rectTransform.sizeDelta = new Vector2(0f, text.preferredHeight);
		text.rectTransform.sizeDelta = new Vector2(0f, text.preferredHeight);
		this.m_Scrollbar.value = 1f;
	}

	public void ResetScrollContent()
	{
		ScrollRect component = base.GetComponent<ScrollRect>();
		if (component != null)
		{
			component.content.anchoredPosition = new Vector2(0f, 0f);
			component.velocity = new Vector2(0f, 0f);
		}
	}

	public PguiTextCtrl m_PguiText;

	public Scrollbar m_Scrollbar;
}
