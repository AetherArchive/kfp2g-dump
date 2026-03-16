using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PguiTabCtrl : PguiBehaviour
{
	public bool ActEnable
	{
		get
		{
			return this.m_ActEnable;
		}
	}

	private Sprite mainCurrentImage
	{
		get
		{
			if (!this.m_ChgMainSub)
			{
				return this.m_CurrentImage;
			}
			return this.m_SubCurrentImage;
		}
	}

	private Sprite mainDcurrentImage
	{
		get
		{
			if (!this.m_ChgMainSub)
			{
				return this.m_DcurrentImage;
			}
			return this.m_SubDcurrentImage;
		}
	}

	private Sprite subCurrentImage
	{
		get
		{
			if (!this.m_ChgMainSub)
			{
				return this.m_SubCurrentImage;
			}
			return this.m_CurrentImage;
		}
	}

	private Sprite subDcurrentImage
	{
		get
		{
			if (!this.m_ChgMainSub)
			{
				return this.m_SubDcurrentImage;
			}
			return this.m_DcurrentImage;
		}
	}

	private void Awake()
	{
		if (this.m_BaseImage != null)
		{
			this.m_ClildText.AddRange(this.m_BaseImage.GetComponentsInChildren<PguiTextCtrl>());
		}
		if (this.m_Button != null)
		{
			this.m_Button.transition = Selectable.Transition.None;
			this.m_Button.navigation = new Navigation
			{
				mode = Navigation.Mode.None
			};
		}
	}

	public void TabAction(bool tabCurrent, bool tabEnable)
	{
		if (this.m_DisableImage != null)
		{
			this.m_DisableImage.gameObject.SetActive(!tabEnable);
			this.m_Collider.enabled = tabEnable;
		}
		this.m_BaseImage.sprite = (tabCurrent ? this.mainCurrentImage : this.mainDcurrentImage);
		if (this.m_SubBaseImage != null)
		{
			this.m_SubBaseImage.sprite = (tabCurrent ? this.subCurrentImage : this.subDcurrentImage);
		}
	}

	public void SetActEnable(bool e)
	{
		if (this.m_DisableImage != null)
		{
			this.m_DisableImage.gameObject.SetActive(!e);
		}
		this.m_BaseImage.color = (e ? Color.white : PguiTabCtrl.DisableColor);
		if (this.m_SubBaseImage != null)
		{
			this.m_SubBaseImage.color = (e ? Color.white : PguiTabCtrl.DisableColor);
		}
		for (int i = 0; i < this.m_ClildText.Count; i++)
		{
			this.m_ClildText[i].textGrayScale = (e ? 1f : PguiTabCtrl.DisableColor.r);
		}
		this.m_ActEnable = e;
		this.m_Collider.enabled = e;
	}

	public void SetChgMainSub(bool sw)
	{
		this.m_ChgMainSub = sw;
	}

	public Button m_Button;

	public PguiCollider m_Collider;

	public Sprite m_CurrentImage;

	public Sprite m_DcurrentImage;

	public Image m_BaseImage;

	public Image m_DisableImage;

	public Sprite m_SubCurrentImage;

	public Sprite m_SubDcurrentImage;

	public Image m_SubBaseImage;

	public List<PguiTextCtrl> m_ClildText = new List<PguiTextCtrl>();

	private bool m_ActEnable = true;

	private static readonly Color DisableColor = new Color(0.7f, 0.7f, 0.7f, 1f);

	private bool m_ChgMainSub;
}
