using System;
using System.Collections.Generic;
using AEAuth3;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PguiButtonCtrl : PguiBehaviour
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
		this.Init();
	}

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

	public void SetActEnable(bool isEnable, bool buttonEnable = false, bool disableColorChange = false)
	{
		this.InternalActEnable(isEnable, buttonEnable, disableColorChange, null);
	}

	public void SetActEnable(bool isEnable, List<string> ignoreDisableColorChangeList, bool buttonEnable = false)
	{
		this.InternalActEnable(isEnable, buttonEnable, false, ignoreDisableColorChangeList);
	}

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

	public void SetSoundType(PguiButtonCtrl.SoundType _soundType)
	{
		this.soundType = _soundType;
	}

	public void AddOnClickListener(PguiButtonCtrl.OnClick callback, PguiButtonCtrl.SoundType soundType = PguiButtonCtrl.SoundType.DEFAULT)
	{
		this.callback = callback;
		this.soundType = soundType;
	}

	public PguiButtonCtrl.OnClick GetOnClickListener()
	{
		return this.callback;
	}

	public PguiButtonCtrl.SoundType GetSoundType()
	{
		return this.soundType;
	}

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

	private void Update()
	{
		this.ActionAndroidBackKeyTarget();
	}

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

	private void ActionAndroidBackKeyTarget()
	{
	}

	public Button m_Button;

	public PguiCollider m_Collider;

	public Image m_BaseImage;

	public GameObject m_DisableMaskObj;

	private List<PguiImageCtrl> m_ChildImage = new List<PguiImageCtrl>();

	private List<PguiTextCtrl> m_ChildText = new List<PguiTextCtrl>();

	private List<PguiRawImageCtrl> m_ChildRawImage = new List<PguiRawImageCtrl>();

	private List<AEImage> m_ChildAEImage = new List<AEImage>();

	private bool m_ActEnable = true;

	private PguiButtonCtrl.SoundType soundType = PguiButtonCtrl.SoundType.DEFAULT;

	private static readonly Color DisableColor = new Color(0.7f, 0.7f, 0.7f, 1f);

	private PguiButtonCtrl.OnClick callback;

	public bool androidBackKeyTarget;

	private Camera canvasCamera;

	private bool isSetupAndroidBackKeyTarget;

	private bool isInit;

	public enum SoundType
	{
		INVALID,
		DEFAULT,
		DECIDE,
		DECIDE_LOW,
		CANCEL,
		MENU_SLIDE
	}

	public delegate void OnClick(PguiButtonCtrl pbc);
}
