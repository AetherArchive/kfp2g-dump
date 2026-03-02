using System;
using UnityEngine;

// Token: 0x0200018E RID: 398
public class AchievementCtrl : MonoBehaviour
{
	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x06001A93 RID: 6803 RVA: 0x00157461 File Offset: 0x00155661
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

	// Token: 0x06001A94 RID: 6804 RVA: 0x00157494 File Offset: 0x00155694
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

	// Token: 0x06001A95 RID: 6805 RVA: 0x001576C3 File Offset: 0x001558C3
	public void HideBadge(int id)
	{
		if (DataManager.DmAchievement.GetHaveAchievementData(id) == null)
		{
			return;
		}
		this.defined.HideNewBadge();
	}

	// Token: 0x06001A96 RID: 6806 RVA: 0x001576DE File Offset: 0x001558DE
	public void ChangeShowSelect(bool show)
	{
		this.currentSelect.SetActive(show);
	}

	// Token: 0x06001A97 RID: 6807 RVA: 0x001576EC File Offset: 0x001558EC
	public void SetScale(float newScal)
	{
		base.gameObject.transform.localScale = new Vector3(newScal, newScal);
	}

	// Token: 0x06001A98 RID: 6808 RVA: 0x00157705 File Offset: 0x00155905
	public int GetAchievementId()
	{
		return this.achievementId;
	}

	// Token: 0x06001A99 RID: 6809 RVA: 0x0015770D File Offset: 0x0015590D
	public void InvalidTouchPanel()
	{
		base.GetComponent<PguiCollider>().enabled = false;
		this.TouchPanel.gameObject.SetActive(false);
	}

	// Token: 0x06001A9A RID: 6810 RVA: 0x0015772C File Offset: 0x0015592C
	private void OpenInfo()
	{
		CanvasManager.HdlAchievementDetailCtrl.Open(this);
	}

	// Token: 0x06001A9B RID: 6811 RVA: 0x00157739 File Offset: 0x00155939
	private void CloseInfo()
	{
		CanvasManager.HdlAchievementDetailCtrl.Close();
	}

	// Token: 0x06001A9C RID: 6812 RVA: 0x00157745 File Offset: 0x00155945
	public void AddOnClickListener(AchievementCtrl.OnClick callback)
	{
		this.callbackCL = callback;
	}

	// Token: 0x06001A9D RID: 6813 RVA: 0x0015774E File Offset: 0x0015594E
	public void OnPointerClick()
	{
		if (this.callbackCL != null)
		{
			this.callbackCL(this);
		}
	}

	// Token: 0x06001A9E RID: 6814 RVA: 0x00157764 File Offset: 0x00155964
	public void AddOnLongClickListener(AchievementCtrl.OnLongClick callback)
	{
		this.callbackLCL = callback;
	}

	// Token: 0x06001A9F RID: 6815 RVA: 0x0015776D File Offset: 0x0015596D
	public void OnLongPress()
	{
		if (this.callbackLCL != null && this.isAllowedFlavor)
		{
			this.callbackLCL(this);
		}
	}

	// Token: 0x06001AA0 RID: 6816 RVA: 0x0015778B File Offset: 0x0015598B
	public void AddOnLongClickEndListener(AchievementCtrl.OnReleaseClick callback)
	{
		this.callbackRL = callback;
	}

	// Token: 0x06001AA1 RID: 6817 RVA: 0x00157794 File Offset: 0x00155994
	public void OnLongPressEnd()
	{
		if (this.callbackRL != null && this.isAllowedFlavor)
		{
			this.callbackRL(this);
		}
	}

	// Token: 0x0400143F RID: 5183
	private int achievementId;

	// Token: 0x04001440 RID: 5184
	[SerializeField]
	private int MinCharSize;

	// Token: 0x04001441 RID: 5185
	[SerializeField]
	private int MaxCharSize;

	// Token: 0x04001442 RID: 5186
	private AchievementCtrl.Defined defined;

	// Token: 0x04001443 RID: 5187
	private GameObject currentSelect;

	// Token: 0x04001444 RID: 5188
	private AchievementCtrl.OnClick callbackCL;

	// Token: 0x04001445 RID: 5189
	private AchievementCtrl.OnLongClick callbackLCL;

	// Token: 0x04001446 RID: 5190
	private AchievementCtrl.OnReleaseClick callbackRL;

	// Token: 0x04001447 RID: 5191
	private Transform touchPanel;

	// Token: 0x04001448 RID: 5192
	private bool isAllowedFlavor;

	// Token: 0x02000E7B RID: 3707
	public class Undefined
	{
		// Token: 0x06004CD1 RID: 19665 RVA: 0x0022F7DC File Offset: 0x0022D9DC
		public Undefined(Transform undefinedTransform)
		{
			this.undefinedBg = undefinedTransform.Find("Bg").GetComponent<PguiImageCtrl>();
		}

		// Token: 0x06004CD2 RID: 19666 RVA: 0x0022F7FA File Offset: 0x0022D9FA
		public void Setup()
		{
		}

		// Token: 0x0400534E RID: 21326
		private PguiImageCtrl undefinedBg;
	}

	// Token: 0x02000E7C RID: 3708
	public class Defined
	{
		// Token: 0x06004CD3 RID: 19667 RVA: 0x0022F7FC File Offset: 0x0022D9FC
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

		// Token: 0x06004CD4 RID: 19668 RVA: 0x0022F894 File Offset: 0x0022DA94
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

		// Token: 0x06004CD5 RID: 19669 RVA: 0x0022F96E File Offset: 0x0022DB6E
		public void HideNewBadge()
		{
			PguiImageCtrl pguiImageCtrl = this.newBadge;
			if (pguiImageCtrl == null)
			{
				return;
			}
			pguiImageCtrl.gameObject.SetActive(false);
		}

		// Token: 0x06004CD6 RID: 19670 RVA: 0x0022F988 File Offset: 0x0022DB88
		private void SetTextSize(int textLength)
		{
			int textSize = this.GetTextSize(textLength);
			this.name.SetFontSize(textSize);
		}

		// Token: 0x06004CD7 RID: 19671 RVA: 0x0022F9A9 File Offset: 0x0022DBA9
		private int GetTextSize(int characterCount)
		{
			if (characterCount > this.DEFAULT_TEXT_MAX_COUNT)
			{
				return this.minCharSize;
			}
			return this.maxCharSize;
		}

		// Token: 0x0400534F RID: 21327
		public readonly int DEFAULT_TEXT_MAX_COUNT = 15;

		// Token: 0x04005350 RID: 21328
		private PguiRawImageCtrl bg;

		// Token: 0x04005351 RID: 21329
		private PguiRawImageCtrl icon;

		// Token: 0x04005352 RID: 21330
		private PguiRawImageCtrl frame;

		// Token: 0x04005353 RID: 21331
		private PguiTextCtrl name;

		// Token: 0x04005354 RID: 21332
		private PguiImageCtrl newBadge;

		// Token: 0x04005355 RID: 21333
		private int minCharSize;

		// Token: 0x04005356 RID: 21334
		private int maxCharSize;
	}

	// Token: 0x02000E7D RID: 3709
	public class Remove
	{
		// Token: 0x06004CD8 RID: 19672 RVA: 0x0022F9C1 File Offset: 0x0022DBC1
		private Remove(Transform baseTr)
		{
			this.bg = baseTr.Find("Bg").GetComponent<PguiImageCtrl>();
		}

		// Token: 0x04005357 RID: 21335
		private PguiImageCtrl bg;
	}

	// Token: 0x02000E7E RID: 3710
	// (Invoke) Token: 0x06004CDA RID: 19674
	public delegate void OnClick(AchievementCtrl Ac);

	// Token: 0x02000E7F RID: 3711
	// (Invoke) Token: 0x06004CDE RID: 19678
	public delegate void OnLongClick(AchievementCtrl Ac);

	// Token: 0x02000E80 RID: 3712
	// (Invoke) Token: 0x06004CE2 RID: 19682
	public delegate void OnReleaseClick(AchievementCtrl Ac);
}
