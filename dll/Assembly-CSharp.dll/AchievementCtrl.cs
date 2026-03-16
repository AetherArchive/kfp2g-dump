using System;
using UnityEngine;

public class AchievementCtrl : MonoBehaviour
{
	private Transform TouchPanel
	{
		get
		{
			if (null == this.touchPanel)
			{
				this.touchPanel = base.gameObject.transform.Find("TouchPanel");
			}
			return this.touchPanel;
		}
	}

	public void Setup(int id, bool isOtherPlayer = false, bool isRemove = false)
	{
		this.currentSelect = base.transform.Find("CurrentSelect").gameObject;
		this.currentSelect.SetActive(false);
		this.achievementId = id;
		GameObject gameObject = base.transform.Find("Defined").gameObject;
		GameObject gameObject2 = base.transform.Find("Undefined").gameObject;
		GameObject gameObject3 = base.transform.Find("NoSetting").gameObject;
		GameObject gameObject4 = base.transform.Find("Remove").gameObject;
		gameObject.SetActive(false);
		gameObject2.SetActive(false);
		gameObject3.SetActive(false);
		gameObject4.SetActive(false);
		this.AddOnLongClickListener(delegate(AchievementCtrl x)
		{
			this.OpenInfo();
		});
		this.AddOnLongClickEndListener(delegate(AchievementCtrl x)
		{
			this.CloseInfo();
		});
		this.TouchPanel.gameObject.SetActive(true);
		this.isAllowedFlavor = isOtherPlayer || DataManager.DmAchievement.GetHaveAchievementData(this.achievementId) != null;
		if (isRemove)
		{
			gameObject4.SetActive(true);
			return;
		}
		if (isOtherPlayer)
		{
			DataManagerAchievement.AchievementStaticData achievementData = DataManager.DmAchievement.GetAchievementData(this.achievementId);
			if (achievementData != null)
			{
				gameObject.SetActive(true);
				this.defined = new AchievementCtrl.Defined(gameObject.transform, this.MinCharSize, this.MaxCharSize);
				this.defined.Setup(achievementData.achievementBg, achievementData.achievementFrame, achievementData.achievementIcon, achievementData.achievementName, false);
				return;
			}
			gameObject3.SetActive(true);
			return;
		}
		else
		{
			DataManagerAchievement.AchievementData haveAchievementData = DataManager.DmAchievement.GetHaveAchievementData(this.achievementId);
			if (haveAchievementData != null)
			{
				gameObject.SetActive(true);
				this.defined = new AchievementCtrl.Defined(gameObject.transform, this.MinCharSize, this.MaxCharSize);
				this.defined.Setup(haveAchievementData.staticData.achievementBg, haveAchievementData.staticData.achievementFrame, haveAchievementData.staticData.achievementIcon, haveAchievementData.staticData.achievementName, haveAchievementData.dynamicData.isNewFlag);
				return;
			}
			if (DataManager.DmAchievement.GetAchievementData(this.achievementId) != null)
			{
				gameObject2.SetActive(true);
				new AchievementCtrl.Undefined(gameObject2.transform).Setup();
				return;
			}
			gameObject3.SetActive(true);
			return;
		}
	}

	public void HideBadge(int id)
	{
		if (DataManager.DmAchievement.GetHaveAchievementData(id) == null)
		{
			return;
		}
		this.defined.HideNewBadge();
	}

	public void ChangeShowSelect(bool show)
	{
		this.currentSelect.SetActive(show);
	}

	public void SetScale(float newScal)
	{
		base.gameObject.transform.localScale = new Vector3(newScal, newScal);
	}

	public int GetAchievementId()
	{
		return this.achievementId;
	}

	public void InvalidTouchPanel()
	{
		base.GetComponent<PguiCollider>().enabled = false;
		this.TouchPanel.gameObject.SetActive(false);
	}

	private void OpenInfo()
	{
		CanvasManager.HdlAchievementDetailCtrl.Open(this);
	}

	private void CloseInfo()
	{
		CanvasManager.HdlAchievementDetailCtrl.Close();
	}

	public void AddOnClickListener(AchievementCtrl.OnClick callback)
	{
		this.callbackCL = callback;
	}

	public void OnPointerClick()
	{
		if (this.callbackCL != null)
		{
			this.callbackCL(this);
		}
	}

	public void AddOnLongClickListener(AchievementCtrl.OnLongClick callback)
	{
		this.callbackLCL = callback;
	}

	public void OnLongPress()
	{
		if (this.callbackLCL != null && this.isAllowedFlavor)
		{
			this.callbackLCL(this);
		}
	}

	public void AddOnLongClickEndListener(AchievementCtrl.OnReleaseClick callback)
	{
		this.callbackRL = callback;
	}

	public void OnLongPressEnd()
	{
		if (this.callbackRL != null && this.isAllowedFlavor)
		{
			this.callbackRL(this);
		}
	}

	private int achievementId;

	[SerializeField]
	private int MinCharSize;

	[SerializeField]
	private int MaxCharSize;

	private AchievementCtrl.Defined defined;

	private GameObject currentSelect;

	private AchievementCtrl.OnClick callbackCL;

	private AchievementCtrl.OnLongClick callbackLCL;

	private AchievementCtrl.OnReleaseClick callbackRL;

	private Transform touchPanel;

	private bool isAllowedFlavor;

	public class Undefined
	{
		public Undefined(Transform undefinedTransform)
		{
			this.undefinedBg = undefinedTransform.Find("Bg").GetComponent<PguiImageCtrl>();
		}

		public void Setup()
		{
		}

		private PguiImageCtrl undefinedBg;
	}

	public class Defined
	{
		public Defined(Transform baseTransform, int minCharSize, int maxCharSize)
		{
			this.bg = baseTransform.Find("Bg").GetComponent<PguiRawImageCtrl>();
			this.icon = baseTransform.Find("Icon").GetComponent<PguiRawImageCtrl>();
			this.frame = baseTransform.Find("Frame").GetComponent<PguiRawImageCtrl>();
			this.name = baseTransform.Find("Text").GetComponent<PguiTextCtrl>();
			this.newBadge = baseTransform.Find("Badge").GetComponent<PguiImageCtrl>();
			this.minCharSize = minCharSize;
			this.maxCharSize = maxCharSize;
		}

		public void Setup(string bgName, string frameName, string iconName, string name, bool isNew)
		{
			this.bg.SetRawImage("Texture2D/Achievement/" + bgName, true, false, null);
			this.frame.gameObject.SetActive(!string.IsNullOrWhiteSpace(frameName));
			this.icon.gameObject.SetActive(!string.IsNullOrWhiteSpace(iconName));
			if (this.frame.gameObject.activeSelf)
			{
				this.frame.SetRawImage("Texture2D/Achievement/" + frameName, true, false, null);
			}
			if (this.icon.gameObject.activeSelf)
			{
				this.icon.SetRawImage("Texture2D/" + iconName, true, false, null);
			}
			this.SetTextSize(name.Length);
			this.name.text = name;
			this.newBadge.gameObject.SetActive(isNew);
		}

		public void HideNewBadge()
		{
			PguiImageCtrl pguiImageCtrl = this.newBadge;
			if (pguiImageCtrl == null)
			{
				return;
			}
			pguiImageCtrl.gameObject.SetActive(false);
		}

		private void SetTextSize(int textLength)
		{
			int textSize = this.GetTextSize(textLength);
			this.name.SetFontSize(textSize);
		}

		private int GetTextSize(int characterCount)
		{
			if (characterCount > this.DEFAULT_TEXT_MAX_COUNT)
			{
				return this.minCharSize;
			}
			return this.maxCharSize;
		}

		public readonly int DEFAULT_TEXT_MAX_COUNT = 15;

		private PguiRawImageCtrl bg;

		private PguiRawImageCtrl icon;

		private PguiRawImageCtrl frame;

		private PguiTextCtrl name;

		private PguiImageCtrl newBadge;

		private int minCharSize;

		private int maxCharSize;
	}

	public class Remove
	{
		private Remove(Transform baseTr)
		{
			this.bg = baseTr.Find("Bg").GetComponent<PguiImageCtrl>();
		}

		private PguiImageCtrl bg;
	}

	public delegate void OnClick(AchievementCtrl Ac);

	public delegate void OnLongClick(AchievementCtrl Ac);

	public delegate void OnReleaseClick(AchievementCtrl Ac);
}
