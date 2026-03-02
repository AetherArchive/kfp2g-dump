using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E5 RID: 485
public class PguiToggleButtonCtrl : PguiBehaviour
{
	// Token: 0x17000451 RID: 1105
	// (get) Token: 0x06002066 RID: 8294 RVA: 0x0018B280 File Offset: 0x00189480
	public bool ActEnable
	{
		get
		{
			return this.m_ActEnable;
		}
	}

	// Token: 0x06002067 RID: 8295 RVA: 0x0018B288 File Offset: 0x00189488
	private void Awake()
	{
		this.m_ClildImage.AddRange(this.m_BaseImage.GetComponentsInChildren<PguiImageCtrl>());
		this.m_ClildText.AddRange(this.m_BaseImage.GetComponentsInChildren<PguiTextCtrl>());
		if (this.m_Button != null)
		{
			this.m_Button.navigation = new Navigation
			{
				mode = Navigation.Mode.None
			};
		}
	}

	// Token: 0x06002068 RID: 8296 RVA: 0x0018B2EC File Offset: 0x001894EC
	public void SetActEnable(bool e)
	{
		if (this.m_DisableImage != null)
		{
			this.m_DisableImage.gameObject.SetActive(!e);
		}
		this.m_BaseImage.color = (e ? Color.white : PguiToggleButtonCtrl.DisableColor);
		for (int i = 0; i < this.m_ClildImage.Count; i++)
		{
			this.m_ClildImage[i].m_Image.color = (e ? Color.white : PguiToggleButtonCtrl.DisableColor);
		}
		for (int j = 0; j < this.m_ClildText.Count; j++)
		{
			this.m_ClildText[j].textGrayScale = (e ? 1f : PguiToggleButtonCtrl.DisableColor.r);
		}
		this.m_ActEnable = e;
		this.m_Collider.enabled = e;
	}

	// Token: 0x06002069 RID: 8297 RVA: 0x0018B3BE File Offset: 0x001895BE
	public void SetToggleIndex(int toggleIndex)
	{
		if (toggleIndex < this.m_ToggleImage.Count)
		{
			this.m_ToggleIndex = toggleIndex;
			this.m_BaseImage.sprite = this.m_ToggleImage[this.m_ToggleIndex];
		}
	}

	// Token: 0x0600206A RID: 8298 RVA: 0x0018B3F1 File Offset: 0x001895F1
	public int GetToggleIndex()
	{
		return this.m_ToggleIndex;
	}

	// Token: 0x0600206B RID: 8299 RVA: 0x0018B3FC File Offset: 0x001895FC
	public void AddOnClickListener(PguiToggleButtonCtrl.OnClick callback)
	{
		this.m_Button.onClick.AddListener(delegate
		{
			this.OnClickInternal(callback);
		});
	}

	// Token: 0x0600206C RID: 8300 RVA: 0x0018B43C File Offset: 0x0018963C
	private void OnClickInternal(PguiToggleButtonCtrl.OnClick callback)
	{
		if (callback != null && callback(this, this.m_ToggleIndex))
		{
			this.m_ToggleIndex++;
			if (this.m_ToggleIndex >= this.m_ToggleImage.Count)
			{
				this.m_ToggleIndex = 0;
			}
			this.m_BaseImage.sprite = this.m_ToggleImage[this.m_ToggleIndex];
			SoundManager.Play("prd_se_click", false, false);
		}
	}

	// Token: 0x0600206D RID: 8301 RVA: 0x0018B4AC File Offset: 0x001896AC
	public void AllRemoveOnClickListener()
	{
		this.m_Button.onClick.RemoveAllListeners();
	}

	// Token: 0x04001785 RID: 6021
	public Button m_Button;

	// Token: 0x04001786 RID: 6022
	public PguiCollider m_Collider;

	// Token: 0x04001787 RID: 6023
	public Image m_BaseImage;

	// Token: 0x04001788 RID: 6024
	public Image m_DisableImage;

	// Token: 0x04001789 RID: 6025
	public List<PguiImageCtrl> m_ClildImage = new List<PguiImageCtrl>();

	// Token: 0x0400178A RID: 6026
	public List<PguiTextCtrl> m_ClildText = new List<PguiTextCtrl>();

	// Token: 0x0400178B RID: 6027
	public List<Sprite> m_ToggleImage = new List<Sprite>();

	// Token: 0x0400178C RID: 6028
	private int m_ToggleIndex;

	// Token: 0x0400178D RID: 6029
	private bool m_ActEnable = true;

	// Token: 0x0400178E RID: 6030
	private static readonly Color DisableColor = new Color(0.7f, 0.7f, 0.7f, 1f);

	// Token: 0x02001029 RID: 4137
	// (Invoke) Token: 0x0600520C RID: 21004
	public delegate bool OnClick(PguiToggleButtonCtrl pbc, int toggleIndex);
}
