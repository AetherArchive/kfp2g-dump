using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001DF RID: 479
public class PguiScrollText : PguiBehaviour
{
	// Token: 0x0600202E RID: 8238 RVA: 0x0018A42C File Offset: 0x0018862C
	public void SetText(string text)
	{
		this.m_PguiText.text = text;
		this.RefreshRect();
	}

	// Token: 0x0600202F RID: 8239 RVA: 0x0018A440 File Offset: 0x00188640
	private void RefreshRect()
	{
		Text text = this.m_PguiText.m_Text;
		text.rectTransform.sizeDelta = new Vector2(0f, text.preferredHeight);
		text.rectTransform.sizeDelta = new Vector2(0f, text.preferredHeight);
		this.m_Scrollbar.value = 1f;
	}

	// Token: 0x06002030 RID: 8240 RVA: 0x0018A4A0 File Offset: 0x001886A0
	public void ResetScrollContent()
	{
		ScrollRect component = base.GetComponent<ScrollRect>();
		if (component != null)
		{
			component.content.anchoredPosition = new Vector2(0f, 0f);
			component.velocity = new Vector2(0f, 0f);
		}
	}

	// Token: 0x04001758 RID: 5976
	public PguiTextCtrl m_PguiText;

	// Token: 0x04001759 RID: 5977
	public Scrollbar m_Scrollbar;
}
