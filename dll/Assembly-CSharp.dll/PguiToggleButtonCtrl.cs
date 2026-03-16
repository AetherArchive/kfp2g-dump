using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PguiToggleButtonCtrl : PguiBehaviour
{
	public bool ActEnable
	{
		get
		{
			return this.m_ActEnable;
		}
	}

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

	public void SetToggleIndex(int toggleIndex)
	{
		if (toggleIndex < this.m_ToggleImage.Count)
		{
			this.m_ToggleIndex = toggleIndex;
			this.m_BaseImage.sprite = this.m_ToggleImage[this.m_ToggleIndex];
		}
	}

	public int GetToggleIndex()
	{
		return this.m_ToggleIndex;
	}

	public void AddOnClickListener(PguiToggleButtonCtrl.OnClick callback)
	{
		this.m_Button.onClick.AddListener(delegate
		{
			this.OnClickInternal(callback);
		});
	}

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

	public void AllRemoveOnClickListener()
	{
		this.m_Button.onClick.RemoveAllListeners();
	}

	public Button m_Button;

	public PguiCollider m_Collider;

	public Image m_BaseImage;

	public Image m_DisableImage;

	public List<PguiImageCtrl> m_ClildImage = new List<PguiImageCtrl>();

	public List<PguiTextCtrl> m_ClildText = new List<PguiTextCtrl>();

	public List<Sprite> m_ToggleImage = new List<Sprite>();

	private int m_ToggleIndex;

	private bool m_ActEnable = true;

	private static readonly Color DisableColor = new Color(0.7f, 0.7f, 0.7f, 1f);

	public delegate bool OnClick(PguiToggleButtonCtrl pbc, int toggleIndex);
}
