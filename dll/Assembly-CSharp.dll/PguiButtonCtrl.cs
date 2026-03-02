using System;
using System.Collections.Generic;
using AEAuth3;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001CC RID: 460
public class PguiButtonCtrl : PguiBehaviour
{
	// Token: 0x17000435 RID: 1077
	// (get) Token: 0x06001F65 RID: 8037 RVA: 0x00183B61 File Offset: 0x00181D61
	public bool ActEnable
	{
		get
		{
			return this.m_ActEnable;
		}
	}

	// Token: 0x06001F66 RID: 8038 RVA: 0x00183B69 File Offset: 0x00181D69
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x06001F67 RID: 8039 RVA: 0x00183B74 File Offset: 0x00181D74
	private void EvacuateOriginalColor()
	{
		if (this.m_BaseImage != null)
		{
			if (this.m_BaseImage.gameObject.GetComponent<PguiDataHolder>() == null)
			{
				this.m_BaseImage.gameObject.AddComponent<PguiDataHolder>().color = this.m_BaseImage.color;
			}
			foreach (PguiImageCtrl pguiImageCtrl in this.m_ChildImage)
			{
				if (pguiImageCtrl.m_Image.gameObject.GetComponent<PguiDataHolder>() == null)
				{
					pguiImageCtrl.gameObject.AddComponent<PguiDataHolder>().color = pguiImageCtrl.m_Image.color;
				}
			}
			foreach (PguiRawImageCtrl pguiRawImageCtrl in this.m_ChildRawImage)
			{
				if (pguiRawImageCtrl.m_RawImage.gameObject.GetComponent<PguiDataHolder>() == null)
				{
					pguiRawImageCtrl.gameObject.AddComponent<PguiDataHolder>().color = pguiRawImageCtrl.m_RawImage.color;
				}
			}
			foreach (AEImage aeimage in this.m_ChildAEImage)
			{
				if (aeimage.gameObject.GetComponent<PguiDataHolder>() == null)
				{
					aeimage.gameObject.AddComponent<PguiDataHolder>().color = aeimage.color;
				}
			}
		}
	}

	// Token: 0x06001F68 RID: 8040 RVA: 0x00183D14 File Offset: 0x00181F14
	public void RefreshOriginalColor()
	{
		if (this.m_BaseImage != null)
		{
			PguiDataHolder component = this.m_BaseImage.gameObject.GetComponent<PguiDataHolder>();
			if (component != null)
			{
				component.color = this.m_BaseImage.color;
			}
			foreach (PguiImageCtrl pguiImageCtrl in this.m_ChildImage)
			{
				PguiDataHolder component2 = pguiImageCtrl.m_Image.gameObject.GetComponent<PguiDataHolder>();
				if (component2 != null)
				{
					component2.color = pguiImageCtrl.m_Image.color;
				}
			}
			foreach (PguiRawImageCtrl pguiRawImageCtrl in this.m_ChildRawImage)
			{
				PguiDataHolder component3 = pguiRawImageCtrl.m_RawImage.gameObject.GetComponent<PguiDataHolder>();
				if (component3 != null)
				{
					component3.color = pguiRawImageCtrl.m_RawImage.color;
				}
			}
			foreach (AEImage aeimage in this.m_ChildAEImage)
			{
				PguiDataHolder component4 = aeimage.gameObject.GetComponent<PguiDataHolder>();
				if (component4 != null)
				{
					component4.color = aeimage.color;
				}
			}
		}
	}

	// Token: 0x06001F69 RID: 8041 RVA: 0x00183E98 File Offset: 0x00182098
	public void Init()
	{
		if (!this.isInit)
		{
			this.isInit = true;
			this.ReloadChildObject();
			if (this.m_Button != null)
			{
				this.m_Button.onClick.AddListener(new UnityAction(this.OnClickInternal));
				this.m_Button.navigation = new Navigation
				{
					mode = Navigation.Mode.None
				};
			}
			if (this.m_DisableMaskObj != null)
			{
				this.m_DisableMaskObj.SetActive(this.m_ActEnable);
			}
		}
	}

	// Token: 0x06001F6A RID: 8042 RVA: 0x00183F20 File Offset: 0x00182120
	public void ReloadChildObject()
	{
		if (this.m_BaseImage != null)
		{
			this.m_ChildImage = new List<PguiImageCtrl>(this.m_BaseImage.GetComponentsInChildren<PguiImageCtrl>());
			this.m_ChildText = new List<PguiTextCtrl>(this.m_BaseImage.GetComponentsInChildren<PguiTextCtrl>());
			this.m_ChildRawImage = new List<PguiRawImageCtrl>(this.m_BaseImage.GetComponentsInChildren<PguiRawImageCtrl>());
			this.m_ChildAEImage = new List<AEImage>(this.m_BaseImage.GetComponentsInChildren<AEImage>());
		}
		this.EvacuateOriginalColor();
	}

	// Token: 0x06001F6B RID: 8043 RVA: 0x00183F99 File Offset: 0x00182199
	public void SetActEnable(bool isEnable, bool buttonEnable = false, bool disableColorChange = false)
	{
		this.InternalActEnable(isEnable, buttonEnable, disableColorChange, null);
	}

	// Token: 0x06001F6C RID: 8044 RVA: 0x00183FA5 File Offset: 0x001821A5
	public void SetActEnable(bool isEnable, List<string> ignoreDisableColorChangeList, bool buttonEnable = false)
	{
		this.InternalActEnable(isEnable, buttonEnable, false, ignoreDisableColorChangeList);
	}

	// Token: 0x06001F6D RID: 8045 RVA: 0x00183FB4 File Offset: 0x001821B4
	private void InternalActEnable(bool isEnable, bool buttonEnable, bool disableColorChange, List<string> ignoreDisableColorChangeList)
	{
		if (!this.isInit)
		{
			this.Init();
		}
		if (this.m_DisableMaskObj != null)
		{
			this.m_DisableMaskObj.SetActive(!isEnable);
		}
		Func<string, bool> func = (string str) => ignoreDisableColorChangeList == null || (ignoreDisableColorChangeList != null && !ignoreDisableColorChangeList.Exists((string item) => item.Equals(str)));
		if (!disableColorChange)
		{
			if (this.m_BaseImage != null && func(this.m_BaseImage.name))
			{
				this.m_BaseImage.color = (isEnable ? this.m_BaseImage.GetComponent<PguiDataHolder>().color : (PguiButtonCtrl.DisableColor * this.m_BaseImage.GetComponent<PguiDataHolder>().color));
			}
			foreach (PguiImageCtrl pguiImageCtrl in this.m_ChildImage)
			{
				if (func(pguiImageCtrl.name))
				{
					pguiImageCtrl.m_Image.color = (isEnable ? pguiImageCtrl.GetComponent<PguiDataHolder>().color : (PguiButtonCtrl.DisableColor * pguiImageCtrl.GetComponent<PguiDataHolder>().color));
				}
			}
			foreach (PguiTextCtrl pguiTextCtrl in this.m_ChildText)
			{
				if (func(pguiTextCtrl.name))
				{
					pguiTextCtrl.textGrayScale = (isEnable ? 1f : PguiButtonCtrl.DisableColor.r);
				}
			}
			foreach (PguiRawImageCtrl pguiRawImageCtrl in this.m_ChildRawImage)
			{
				if (func(pguiRawImageCtrl.name))
				{
					pguiRawImageCtrl.m_RawImage.color = (isEnable ? pguiRawImageCtrl.GetComponent<PguiDataHolder>().color : (PguiButtonCtrl.DisableColor * pguiRawImageCtrl.GetComponent<PguiDataHolder>().color));
				}
			}
			foreach (AEImage aeimage in this.m_ChildAEImage)
			{
				if (func(aeimage.name))
				{
					aeimage.color = (isEnable ? aeimage.GetComponent<PguiDataHolder>().color : (PguiButtonCtrl.DisableColor * aeimage.GetComponent<PguiDataHolder>().color));
				}
			}
		}
		this.m_ActEnable = isEnable;
		if (buttonEnable)
		{
			this.m_Collider.enabled = buttonEnable;
			return;
		}
		this.m_Collider.enabled = isEnable;
	}

	// Token: 0x06001F6E RID: 8046 RVA: 0x00184268 File Offset: 0x00182468
	public void SetSoundType(PguiButtonCtrl.SoundType _soundType)
	{
		this.soundType = _soundType;
	}

	// Token: 0x06001F6F RID: 8047 RVA: 0x00184271 File Offset: 0x00182471
	public void AddOnClickListener(PguiButtonCtrl.OnClick callback, PguiButtonCtrl.SoundType soundType = PguiButtonCtrl.SoundType.DEFAULT)
	{
		this.callback = callback;
		this.soundType = soundType;
	}

	// Token: 0x06001F70 RID: 8048 RVA: 0x00184281 File Offset: 0x00182481
	public PguiButtonCtrl.OnClick GetOnClickListener()
	{
		return this.callback;
	}

	// Token: 0x06001F71 RID: 8049 RVA: 0x00184289 File Offset: 0x00182489
	public PguiButtonCtrl.SoundType GetSoundType()
	{
		return this.soundType;
	}

	// Token: 0x06001F72 RID: 8050 RVA: 0x00184294 File Offset: 0x00182494
	private void OnClickInternal()
	{
		if (this.callback != null)
		{
			this.callback(this);
		}
		switch (this.soundType)
		{
		case PguiButtonCtrl.SoundType.DEFAULT:
			SoundManager.Play("prd_se_click", false, false);
			return;
		case PguiButtonCtrl.SoundType.DECIDE:
			SoundManager.Play("prd_se_decide", false, false);
			return;
		case PguiButtonCtrl.SoundType.DECIDE_LOW:
			SoundManager.Play("prd_se_decide_low", false, false);
			return;
		case PguiButtonCtrl.SoundType.CANCEL:
			SoundManager.Play("prd_se_cancel", false, false);
			return;
		case PguiButtonCtrl.SoundType.MENU_SLIDE:
			SoundManager.Play("prd_se_menu_slide", false, false);
			return;
		default:
			return;
		}
	}

	// Token: 0x06001F73 RID: 8051 RVA: 0x0018431E File Offset: 0x0018251E
	private void Update()
	{
		this.ActionAndroidBackKeyTarget();
	}

	// Token: 0x06001F74 RID: 8052 RVA: 0x00184328 File Offset: 0x00182528
	private void SetupAndroidBackKeyTarget()
	{
		if (this.isSetupAndroidBackKeyTarget)
		{
			return;
		}
		RectTransform rectTransform = base.transform as RectTransform;
		this.canvasCamera = rectTransform.GetComponentInParent<Canvas>().worldCamera;
		this.isSetupAndroidBackKeyTarget = true;
	}

	// Token: 0x06001F75 RID: 8053 RVA: 0x00184362 File Offset: 0x00182562
	private void ActionAndroidBackKeyTarget()
	{
	}

	// Token: 0x040016CD RID: 5837
	public Button m_Button;

	// Token: 0x040016CE RID: 5838
	public PguiCollider m_Collider;

	// Token: 0x040016CF RID: 5839
	public Image m_BaseImage;

	// Token: 0x040016D0 RID: 5840
	public GameObject m_DisableMaskObj;

	// Token: 0x040016D1 RID: 5841
	private List<PguiImageCtrl> m_ChildImage = new List<PguiImageCtrl>();

	// Token: 0x040016D2 RID: 5842
	private List<PguiTextCtrl> m_ChildText = new List<PguiTextCtrl>();

	// Token: 0x040016D3 RID: 5843
	private List<PguiRawImageCtrl> m_ChildRawImage = new List<PguiRawImageCtrl>();

	// Token: 0x040016D4 RID: 5844
	private List<AEImage> m_ChildAEImage = new List<AEImage>();

	// Token: 0x040016D5 RID: 5845
	private bool m_ActEnable = true;

	// Token: 0x040016D6 RID: 5846
	private PguiButtonCtrl.SoundType soundType = PguiButtonCtrl.SoundType.DEFAULT;

	// Token: 0x040016D7 RID: 5847
	private static readonly Color DisableColor = new Color(0.7f, 0.7f, 0.7f, 1f);

	// Token: 0x040016D8 RID: 5848
	private PguiButtonCtrl.OnClick callback;

	// Token: 0x040016D9 RID: 5849
	public bool androidBackKeyTarget;

	// Token: 0x040016DA RID: 5850
	private Camera canvasCamera;

	// Token: 0x040016DB RID: 5851
	private bool isSetupAndroidBackKeyTarget;

	// Token: 0x040016DC RID: 5852
	private bool isInit;

	// Token: 0x02001009 RID: 4105
	public enum SoundType
	{
		// Token: 0x04005A05 RID: 23045
		INVALID,
		// Token: 0x04005A06 RID: 23046
		DEFAULT,
		// Token: 0x04005A07 RID: 23047
		DECIDE,
		// Token: 0x04005A08 RID: 23048
		DECIDE_LOW,
		// Token: 0x04005A09 RID: 23049
		CANCEL,
		// Token: 0x04005A0A RID: 23050
		MENU_SLIDE
	}

	// Token: 0x0200100A RID: 4106
	// (Invoke) Token: 0x060051B7 RID: 20919
	public delegate void OnClick(PguiButtonCtrl pbc);
}
