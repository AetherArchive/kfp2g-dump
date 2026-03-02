using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A5 RID: 421
public class IconCharaCtrl : MonoBehaviour
{
	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x06001C22 RID: 7202 RVA: 0x00164364 File Offset: 0x00162564
	// (set) Token: 0x06001C23 RID: 7203 RVA: 0x0016436C File Offset: 0x0016256C
	public CharaPackData charaPackData { get; private set; }

	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x06001C24 RID: 7204 RVA: 0x00164378 File Offset: 0x00162578
	private Color defaultTextColor
	{
		get
		{
			if (this._defaultTextColor == null)
			{
				this._defaultTextColor = new Color?(Color.white);
				if (this.textLevel != null)
				{
					this._defaultTextColor = new Color?(this.textLevel.m_Text.color);
				}
			}
			return this._defaultTextColor.Value;
		}
	}

	// Token: 0x06001C25 RID: 7205 RVA: 0x001643D6 File Offset: 0x001625D6
	private void Awake()
	{
		Color defaultTextColor = this.defaultTextColor;
	}

	// Token: 0x06001C26 RID: 7206 RVA: 0x001643E0 File Offset: 0x001625E0
	public void IsEnableMask(bool isEnable)
	{
		if (this.imgBase != null)
		{
			this.imgBase.m_Image.color = (isEnable ? IconCharaCtrl.MaskColor : Color.white);
		}
		if (this.texChara != null)
		{
			this.texChara.m_RawImage.color = (isEnable ? IconCharaCtrl.MaskColor : Color.white);
		}
		if (this.imgAttribute != null)
		{
			this.imgAttribute.m_Image.color = (isEnable ? IconCharaCtrl.MaskColor : Color.white);
		}
		if (this.imgSubAttribute != null)
		{
			this.imgSubAttribute.m_Image.color = (isEnable ? IconCharaCtrl.MaskColor : Color.white);
		}
		if (this.imgMarkGood != null)
		{
			this.imgMarkGood.m_Image.color = (isEnable ? IconCharaCtrl.MaskColor : Color.white);
		}
		if (this.imgMarkBad != null)
		{
			this.imgMarkBad.m_Image.color = (isEnable ? IconCharaCtrl.MaskColor : Color.white);
		}
		if (this.imgStar != null)
		{
			foreach (PguiImageCtrl pguiImageCtrl in this.imgStar)
			{
				if (!(pguiImageCtrl == null))
				{
					pguiImageCtrl.m_Image.color = (isEnable ? IconCharaCtrl.MaskColor : Color.white);
				}
			}
		}
		if (this.textLevel != null)
		{
			this.textLevel.m_Text.color = (isEnable ? IconCharaCtrl.MaskColor : this.defaultTextColor);
		}
		if (this.frame != null)
		{
			this.frame.m_Image.color = (isEnable ? IconCharaCtrl.MaskColor : Color.white);
		}
		if (this.frameKiseki != null)
		{
			this.frameKiseki.m_Image.color = (isEnable ? IconCharaCtrl.MaskColor : Color.white);
		}
		if (this.imgWakeup != null)
		{
			this.imgWakeup.GetComponent<PguiImageCtrl>().m_Image.color = (isEnable ? IconCharaCtrl.MaskColor : Color.white);
		}
		if (this.imgMarkAccessory != null)
		{
			this.imgMarkAccessory.m_Image.color = (isEnable ? IconCharaCtrl.MaskColor : Color.white);
		}
	}

	// Token: 0x06001C27 RID: 7207 RVA: 0x0016464C File Offset: 0x0016284C
	private void SetupTextUserRank(HelperPackData hpd, SortFilterDefine.SortType sortType)
	{
		if (hpd != null && this.textLevel != null && this.textLevel != null && sortType == SortFilterDefine.SortType.USER_RANK)
		{
			this.textLevel.text = "Rank." + hpd.level.ToString();
		}
	}

	// Token: 0x06001C28 RID: 7208 RVA: 0x001646A0 File Offset: 0x001628A0
	private void SetupHelperSortTypeNew(HelperPackData hpd, SortFilterDefine.SortType sortType, bool isSendView)
	{
		if (hpd != null && this.textLevel != null && this.isHelper && sortType == SortFilterDefine.SortType.NEW)
		{
			this.textLevel.text = (isSendView ? hpd.sendFollowTime.ToString("yyyy/MM/dd") : hpd.receiveFollowTime.ToString("yyyy/MM/dd"));
		}
	}

	// Token: 0x06001C29 RID: 7209 RVA: 0x001646FC File Offset: 0x001628FC
	public void SetupHelper(HelperPackData hpd, SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL, bool isEnableMask = false, CharaWindowCtrl.DetailParamSetting detailParam = null, bool isSendView = true)
	{
		this.isHelper = true;
		if (hpd != null)
		{
			this.SetupHelperSortTypeNew(hpd, sortType, isSendView);
			this.Setup(hpd.FavoriteChara, sortType, isEnableMask, detailParam, 0, -1, 0);
			this.SetupTextUserRank(hpd, sortType);
		}
		else
		{
			this.Setup(null, sortType, isEnableMask, detailParam, 0, -1, 0);
		}
		this.isHelper = false;
	}

	// Token: 0x06001C2A RID: 7210 RVA: 0x00164750 File Offset: 0x00162950
	public void SetupHelper(CharaPackData cpd, HelperPackData hpd, SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL, bool isEnableMask = false, CharaWindowCtrl.DetailParamSetting detaiParam = null, bool isSendView = true)
	{
		this.isHelper = true;
		if (hpd != null || cpd != null)
		{
			this.SetupHelperSortTypeNew(hpd, sortType, isSendView);
			this.Setup(cpd, sortType, isEnableMask, detaiParam, 0, -1, 0);
			this.SetupTextUserRank(hpd, sortType);
		}
		else
		{
			this.Setup(null, sortType, isEnableMask, detaiParam, 0, -1, 0);
		}
		this.isHelper = false;
	}

	// Token: 0x06001C2B RID: 7211 RVA: 0x001647A3 File Offset: 0x001629A3
	public void SetupPrm(IconCharaCtrl.SetupParam param)
	{
		this.Setup(param.cpd, param.sortType, param.isEnableMask, param.detaiParam, param.eventId, param.iconId, 0);
	}

	// Token: 0x06001C2C RID: 7212 RVA: 0x001647D0 File Offset: 0x001629D0
	public void Setup(CharaPackData cpd, SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL, bool isEnableMask = false, CharaWindowCtrl.DetailParamSetting detailParam = null, int eventId = 0, int iconId = -1, int bonusQuest = 0)
	{
		IconCharaCtrl.<>c__DisplayClass55_0 CS$<>8__locals1 = new IconCharaCtrl.<>c__DisplayClass55_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.detailParam = detailParam;
		this.charaPackData = cpd;
		this.detailParam = CS$<>8__locals1.detailParam;
		if (this.charaPackData == null || this.charaPackData.IsInvalid())
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
		if (this.frame != null)
		{
			this.frame.gameObject.SetActive(!this.charaPackData.IsEnableSpAbility);
		}
		if (this.frameKiseki != null)
		{
			this.frameKiseki.gameObject.SetActive(this.charaPackData.IsEnableSpAbility);
		}
		bool flag = false;
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(eventId);
		if (eventData != null && eventData.IsEnableEvent)
		{
			flag = eventData.eventCategory == DataManagerEvent.Category.Growth;
			if (flag && (this.detailParam != null || bonusQuest > 0))
			{
				QuestOnePackData qopd = DataManager.DmQuest.GetQuestOnePackData((this.detailParam == null) ? bonusQuest : this.detailParam.selectQuestOneId);
				if (qopd == null || !eventData.GrowthQuestGroupList.Exists((int item) => item == qopd.questGroup.questGroupId) || !qopd.questGroup.questOneList.Exists((QuestStaticQuestOne item) => item.questId == qopd.questOne.questId))
				{
					flag = false;
				}
			}
		}
		bool flag2 = CS$<>8__locals1.detailParam != null && CharaWindowCtrl.DetailParamSetting.Preset.GACHA_RESULT == CS$<>8__locals1.detailParam.UIPreset;
		if (this.imgBase != null)
		{
			this.imgBase.gameObject.SetActive(this.imgBaseV2 == null || this.charaPackData.staticData.baseData.OriginalId <= 0);
		}
		if (this.imgBaseV2 != null)
		{
			this.imgBaseV2.gameObject.SetActive(this.imgBase == null || this.charaPackData.staticData.baseData.OriginalId > 0);
			int num = 0;
			if (this.charaPackData.staticData.baseData.OriginalId > 0)
			{
				if (this.charaPackData.staticData.baseData.attribute == CharaDef.AttributeType.RED)
				{
					num = 1;
				}
				else if (this.charaPackData.staticData.baseData.attribute == CharaDef.AttributeType.GREEN)
				{
					num = 2;
				}
				else if (this.charaPackData.staticData.baseData.attribute == CharaDef.AttributeType.BLUE)
				{
					num = 3;
				}
				else if (this.charaPackData.staticData.baseData.attribute == CharaDef.AttributeType.PINK)
				{
					num = 4;
				}
				else if (this.charaPackData.staticData.baseData.attribute == CharaDef.AttributeType.LIME)
				{
					num = 5;
				}
				else if (this.charaPackData.staticData.baseData.attribute == CharaDef.AttributeType.AQUA)
				{
					num = 6;
				}
			}
			this.imgBaseV2.Replace(num);
		}
		if (this.texChara != null)
		{
			string text = this.charaPackData.dynamicData.IconName;
			if (flag2)
			{
				iconId = 0;
			}
			if (0 <= iconId && iconId <= 4)
			{
				string text2 = "Texture2D/Icon_Chara/Chara/icon_chara_";
				string text3 = this.charaPackData.staticData.baseData.assetId.ToString("0000");
				string text4 = ((1 < iconId) ? ("_" + iconId.ToString("00")) : "");
				text = text2 + text3 + text4;
			}
			this.texChara.SetRawImage(text, true, false, null);
			this.texChara.SetRaycastTarget(CS$<>8__locals1.detailParam != null || this.callbackCL != null);
		}
		if (this.imgAttribute != null)
		{
			this.imgAttribute.SetImageByName(IconCharaCtrl.Attribute2IconName(this.charaPackData.staticData.baseData.attribute));
		}
		if (this.imgSubAttribute != null)
		{
			if (this.charaPackData.staticData.baseData.subAttribute <= CharaDef.AttributeType.ALL)
			{
				this.imgSubAttribute.gameObject.SetActive(false);
			}
			else
			{
				this.imgSubAttribute.gameObject.SetActive(true);
				this.imgSubAttribute.SetImageByName(IconCharaCtrl.SubAttribute2IconName(this.charaPackData.staticData.baseData.subAttribute));
			}
		}
		if (this.imgMarkGood != null)
		{
			this.imgMarkGood.gameObject.SetActive(false);
		}
		if (this.imgMarkBad != null)
		{
			this.imgMarkBad.gameObject.SetActive(false);
		}
		if (this.imgAttrBar != null && this.colorAttrBar != null)
		{
			int attribute = (int)this.charaPackData.staticData.baseData.attribute;
			string text5 = attribute.ToString();
			this.imgAttrBar.m_Image.color = this.colorAttrBar.GetGameObjectById(text5);
		}
		if (this.textLevel != null)
		{
			this.textLevel.gameObject.SetActive(true);
			if (!this.isHelper || sortType != SortFilterDefine.SortType.NEW)
			{
				this.textLevel.text = CS$<>8__locals1.<Setup>g__GetSortNameFunc|0(sortType);
			}
		}
		if (this.textLevelStory != null)
		{
			this.textLevelStory.gameObject.SetActive(false);
			this.textLevelStory.text = CS$<>8__locals1.<Setup>g__GetSortNameFunc|0(sortType);
		}
		if (this.textCharaNameStoryWName != null)
		{
			Transform transform = this.textCharaNameStoryWName.transform.Find("Num_Lv_Story");
			transform.gameObject.SetActive(true);
			transform.GetComponent<PguiTextCtrl>().text = CS$<>8__locals1.<Setup>g__GetSortNameFunc|0(sortType);
		}
		if (this.imgWakeup != null)
		{
			if (flag2 || this.charaPackData.dynamicData.promoteNum <= 0)
			{
				this.imgWakeup.gameObject.SetActive(false);
			}
			else
			{
				this.imgWakeup.gameObject.SetActive(true);
				this.imgWakeup.Replace(this.charaPackData.dynamicData.promoteNum);
			}
		}
		if (this.imgStar != null)
		{
			int num2 = (flag2 ? this.charaPackData.staticData.baseData.rankLow : this.charaPackData.dynamicData.rank);
			for (int i = 0; i < this.imgStar.Count; i++)
			{
				string text6 = ((i < num2) ? "icon_star" : "icon_star_blank");
				DataManagerChara.LevelLimitData levelLimitData = DataManager.DmChara.GetLevelLimitData(this.charaPackData.dynamicData.levelLimitId);
				if (levelLimitData != null && !flag2)
				{
					if (this.charaPackData.dynamicData.level == levelLimitData.maxLevel)
					{
						text6 = levelLimitData.compImageName;
					}
					else
					{
						text6 = levelLimitData.openImageName;
					}
				}
				if (this.type == IconCharaCtrl.Type.ICON)
				{
					text6 += "_s";
				}
				else
				{
					text6 += "_m";
				}
				if (this.imgStar[i] != null)
				{
					this.imgStar[i].SetImageByName(text6);
					this.imgStar[i].gameObject.SetActive(i < this.charaPackData.staticData.baseData.rankHigh);
				}
			}
		}
		if (this.textCharaName != null)
		{
			this.textCharaName.gameObject.SetActive(true);
			this.textCharaName.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				this.charaPackData.staticData.baseData.NickName,
				this.charaPackData.staticData.GetName()
			});
		}
		if (this.textCharaNameStory != null)
		{
			this.textCharaNameStory.gameObject.SetActive(false);
			this.textCharaNameStory.text = this.charaPackData.staticData.baseData.charaName;
		}
		if (this.textCharaNameStoryWName != null)
		{
			this.textCharaNameStoryWName.gameObject.SetActive(false);
			this.textCharaNameStoryWName.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				this.charaPackData.staticData.baseData.NickName,
				this.charaPackData.staticData.GetName()
			});
		}
		if (this.textEponymName != null)
		{
			this.textEponymName.text = ((this.type == IconCharaCtrl.Type.ICON_TOP) ? "" : this.charaPackData.staticData.baseData.eponymName);
		}
		if (null != this.currentFrame)
		{
			this.currentFrame.SetActive(false);
		}
		if (this.storyInfo != null)
		{
			foreach (IconCharaCtrl.StoryInfo storyInfo in this.storyInfo)
			{
				if (storyInfo.icon != null)
				{
					storyInfo.icon.gameObject.SetActive(false);
				}
				if (storyInfo.textAct != null)
				{
					storyInfo.textAct.gameObject.SetActive(false);
				}
				if (storyInfo.textNor != null)
				{
					storyInfo.textNor.gameObject.SetActive(false);
				}
			}
		}
		if (this.aeNew)
		{
			this.aeNew.SetActive(false);
		}
		this.DispMarkExpUp(false);
		this.DispEventGrowPickUp(false);
		if (this.imgMarkExpUp != null && flag)
		{
			DataManagerEvent.EventData.Bonus bonus = eventData.GrowthCharaList.Find((DataManagerEvent.EventData.Bonus item) => item.Id == CS$<>8__locals1.<>4__this.charaPackData.id);
			if (bonus == null)
			{
				bonus = ((eventData.SelectGrowthCharaData.Id == this.charaPackData.id) ? new DataManagerEvent.EventData.Bonus(eventData.SelectGrowthCharaData.Id, eventData.SelectGrowthCharaData.Ratio) : null);
			}
			this.DispMarkExpUp(bonus != null);
		}
		if (this.type == IconCharaCtrl.Type.ICON_TOP)
		{
			this.DispDisable(isEnableMask);
		}
		else
		{
			this.IsEnableMask(isEnableMask);
		}
		int num3 = eventId;
		if (num3 <= 0)
		{
			num3 = ((((bonusQuest > 0) ? DataManager.DmQuest.GetQuestOnePackData(bonusQuest) : null) == null) ? (-1) : QuestUtil.GetEventId(bonusQuest, false));
		}
		List<DataManagerChara.BonusCharaData> list = ((num3 < 0) ? DataManager.DmChara.GetBonusCharaDataList() : DataManager.DmChara.GetBonusCharaDataList(num3)).FindAll((DataManagerChara.BonusCharaData itm) => itm.charaId == CS$<>8__locals1.<>4__this.charaPackData.id);
		bool flag3;
		if (list.Count > 0)
		{
			if (list.Find((DataManagerChara.BonusCharaData itm) => itm.hpBonusRatio != 0) == null)
			{
				if (list.Find((DataManagerChara.BonusCharaData itm) => itm.strBonusRatio != 0) == null)
				{
					if (list.Find((DataManagerChara.BonusCharaData itm) => itm.defBonusRatio != 0) == null)
					{
						flag3 = list.Find((DataManagerChara.BonusCharaData itm) => itm.kizunaBonusRatio != 0) != null;
						goto IL_0AFF;
					}
				}
			}
			flag3 = true;
		}
		else
		{
			flag3 = false;
		}
		IL_0AFF:
		bool flag4 = flag3;
		bool flag5;
		if (list.Find((DataManagerChara.BonusCharaData itm) => itm.increaseItemId01 != 0) == null)
		{
			flag5 = list.Find((DataManagerChara.BonusCharaData itm) => itm.increaseItemId02 != 0) != null;
		}
		else
		{
			flag5 = true;
		}
		bool flag6 = flag5;
		this.DispMarkEvent(flag4, list.Find((DataManagerChara.BonusCharaData itm) => itm.pickUpFlg) != null, flag6);
		if (this.imgMarkPhotoPocketLevel != null)
		{
			this.imgMarkPhotoPocketLevel.Setup(new PhotoPocketLevelCtrl.SetupParam
			{
				charaPackData = this.charaPackData
			});
			this.DispPhotoPocketLevel(false);
		}
		this.DispMarkAccessory(!AccessoryUtil.IsInvalid(this.charaPackData.dynamicData.accessory));
		this.DispFavorite(cpd);
	}

	// Token: 0x06001C2D RID: 7213 RVA: 0x001653D0 File Offset: 0x001635D0
	public void DispRanking()
	{
		this.DispLevel(false);
		this.DispRarity(false);
		this.DispAttribute(false);
		this.DispAttributeMark((CharaDef.AttributeMask)0);
		this.DispWakeUp(false);
		this.DispMarkEvent(false, false, false);
	}

	// Token: 0x06001C2E RID: 7214 RVA: 0x001653FE File Offset: 0x001635FE
	public void DispCurrentFrame(bool isDisp)
	{
		if (this.currentFrame != null)
		{
			this.currentFrame.SetActive(isDisp);
		}
	}

	// Token: 0x06001C2F RID: 7215 RVA: 0x0016541A File Offset: 0x0016361A
	public void DispEventGrowPickUp(bool flag)
	{
		if (this.imgEventGrowPickUp != null)
		{
			this.imgEventGrowPickUp.gameObject.SetActive(flag);
		}
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x0016543B File Offset: 0x0016363B
	public void DispMarkExpUp(bool flag)
	{
		if (this.imgMarkExpUp != null)
		{
			this.imgMarkExpUp.gameObject.SetActive(flag);
		}
	}

	// Token: 0x06001C31 RID: 7217 RVA: 0x0016545C File Offset: 0x0016365C
	public bool IsDispMarkExpUp()
	{
		return this.imgMarkExpUp != null && this.imgMarkExpUp.gameObject.activeSelf;
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x0016547E File Offset: 0x0016367E
	public void DispDisable(bool flag)
	{
		if (this.imgDisable != null)
		{
			this.imgDisable.SetActive(flag);
		}
	}

	// Token: 0x06001C33 RID: 7219 RVA: 0x0016549A File Offset: 0x0016369A
	public void DispLevel(bool flag)
	{
		if (this.textLevel != null)
		{
			this.textLevel.gameObject.SetActive(flag);
		}
	}

	// Token: 0x06001C34 RID: 7220 RVA: 0x001654BB File Offset: 0x001636BB
	public void DispAttribute(bool flag)
	{
		if (this.imgAttribute != null)
		{
			this.imgAttribute.gameObject.SetActive(flag);
		}
		if (this.imgSubAttribute != null)
		{
			this.imgSubAttribute.gameObject.SetActive(flag);
		}
	}

	// Token: 0x06001C35 RID: 7221 RVA: 0x001654FC File Offset: 0x001636FC
	public void DispAttributeMark(CharaDef.AttributeMask attr)
	{
		CharaDef.AttributeMask attributeMask = ((this.charaPackData == null || this.charaPackData.IsInvalid() || this.charaPackData.staticData == null) ? ((CharaDef.AttributeMask)0) : this.charaPackData.staticData.baseData.attributeMask);
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < SceneBattle.attrMatch.GetLength(0); i++)
		{
			if ((attributeMask & SceneBattle.attrMatch[i, 0]) != (CharaDef.AttributeMask)0)
			{
				if ((attr & SceneBattle.attrMatch[i, 1]) != (CharaDef.AttributeMask)0)
				{
					flag = true;
				}
				if ((attr & SceneBattle.attrMatch[i, 2]) != (CharaDef.AttributeMask)0)
				{
					flag2 = true;
				}
			}
		}
		if (this.imgMarkGood != null)
		{
			this.imgMarkGood.gameObject.SetActive(flag);
		}
		if (this.imgMarkBad != null)
		{
			this.imgMarkBad.gameObject.SetActive(flag2);
		}
	}

	// Token: 0x06001C36 RID: 7222 RVA: 0x001655D0 File Offset: 0x001637D0
	public void DispMarkEvent(bool norm, bool pick, bool drop)
	{
		if (this.imgMarkEvent != null)
		{
			this.imgMarkEvent.gameObject.SetActive(norm);
			if (this.imgMarkEvent.gameObject.activeSelf)
			{
				PguiReplaceSpriteCtrl component = this.imgMarkEvent.GetComponent<PguiReplaceSpriteCtrl>();
				if (component != null)
				{
					component.Replace(pick ? 2 : 1);
				}
				uGUITweenScale component2 = this.imgMarkEvent.GetComponent<uGUITweenScale>();
				if (component2 != null)
				{
					component2.enabled = pick;
				}
			}
		}
		if (this.imgMarkEventDropBonus != null)
		{
			this.imgMarkEventDropBonus.gameObject.SetActive(drop);
		}
	}

	// Token: 0x06001C37 RID: 7223 RVA: 0x0016566B File Offset: 0x0016386B
	public void DispWakeUp(bool flag)
	{
		if (this.imgWakeup != null)
		{
			this.imgWakeup.gameObject.SetActive(flag);
		}
	}

	// Token: 0x06001C38 RID: 7224 RVA: 0x0016568C File Offset: 0x0016388C
	public void DispBookmark(bool flag)
	{
		if (this.imgBookmark != null)
		{
			this.imgBookmark.gameObject.SetActive(flag);
		}
	}

	// Token: 0x06001C39 RID: 7225 RVA: 0x001656B0 File Offset: 0x001638B0
	public void SetWakeUp(int promoteNum)
	{
		if (this.imgWakeup != null)
		{
			if (promoteNum <= 0)
			{
				this.imgWakeup.gameObject.SetActive(false);
				return;
			}
			this.imgWakeup.gameObject.SetActive(true);
			this.imgWakeup.Replace(promoteNum);
		}
	}

	// Token: 0x06001C3A RID: 7226 RVA: 0x00165700 File Offset: 0x00163900
	public void SetupStoryInfo(CharaPackData chara, bool isHave)
	{
		if (this.storyInfo == null)
		{
			return;
		}
		if (this.textLevel != null)
		{
			this.textLevel.gameObject.SetActive(false);
		}
		if (this.textCharaName != null)
		{
			this.textCharaName.gameObject.SetActive(false);
		}
		if (this.textLevelStory != null)
		{
			this.textLevelStory.gameObject.SetActive(DataManager.DmChara.GetUserCharaData(chara.id) != null);
		}
		if (string.IsNullOrEmpty(chara.staticData.baseData.NickName))
		{
			if (this.textCharaNameStory != null)
			{
				this.textCharaNameStory.gameObject.SetActive(true);
			}
			if (this.textCharaNameStoryWName != null)
			{
				this.textCharaNameStoryWName.gameObject.SetActive(false);
			}
		}
		else
		{
			if (this.textCharaNameStory != null)
			{
				this.textCharaNameStory.gameObject.SetActive(false);
			}
			if (this.textCharaNameStoryWName != null)
			{
				this.textCharaNameStoryWName.gameObject.SetActive(true);
				this.textCharaNameStoryWName.transform.Find("Num_Lv_Story").gameObject.SetActive(isHave);
			}
		}
		Dictionary<int, QuestDynamicQuestOne> oneDataMap = DataManager.DmQuest.QuestDynamicData.oneDataMap;
		int num = 0;
		QuestStaticMap questStaticMap = DataManager.DmQuest.QuestStaticData.mapDataList.Find((QuestStaticMap item) => item.questCharaId == chara.id);
		if (questStaticMap != null)
		{
			foreach (QuestStaticQuestGroup questStaticQuestGroup in questStaticMap.questGroupList)
			{
				if (num < this.storyInfo.Count)
				{
					QuestStaticQuestGroup questStaticQuestGroup2 = questStaticMap.questGroupList[num];
					IconCharaCtrl.StoryInfo storyInfo = this.storyInfo[num];
					PguiReplaceSpriteCtrl component = storyInfo.icon.GetComponent<PguiReplaceSpriteCtrl>();
					component.InitForce();
					storyInfo.icon.gameObject.SetActive(true);
					int num2 = 0;
					bool flag = false;
					foreach (QuestStaticQuestOne questStaticQuestOne in questStaticQuestGroup.questOneList)
					{
						flag = false;
						CharaPackData charaPackData = DataManager.DmChara.GetUserCharaData(questStaticQuestGroup2.charaId);
						if (charaPackData == null)
						{
							charaPackData = CharaPackData.MakeInitial(questStaticQuestGroup2.charaId);
						}
						bool flag2 = questStaticQuestGroup2.targetCharaKizunaLevel == 0 || charaPackData.dynamicData.kizunaLevel >= questStaticQuestGroup2.targetCharaKizunaLevel;
						if (num == 3)
						{
							if (chara.dynamicData.artsLevel >= questStaticQuestGroup.targetCharaArtsLevel)
							{
								flag = true;
							}
						}
						else if (flag2)
						{
							flag = true;
						}
						if (!oneDataMap.ContainsKey(questStaticQuestOne.questId))
						{
							num2 = -1;
							break;
						}
						QuestDynamicQuestOne questDynamicQuestOne = oneDataMap[questStaticQuestOne.questId];
						if (questDynamicQuestOne.status == QuestOneStatus.COMPLETE || questDynamicQuestOne.status == QuestOneStatus.CLEAR)
						{
							num2++;
						}
						if (flag && questDynamicQuestOne.status == QuestOneStatus.NEW)
						{
							if (this.aeNew != null)
							{
								this.aeNew.SetActive(true);
								break;
							}
							break;
						}
					}
					if (num2 >= questStaticQuestGroup.questOneList.Count)
					{
						storyInfo.icon.SetImageByName(component.GetSpriteById(0).name);
						storyInfo.textNor.gameObject.SetActive(false);
						storyInfo.textAct.gameObject.SetActive(true);
						storyInfo.textAct.text = (num + 1).ToString();
					}
					else if (num2 < 0 || !flag)
					{
						storyInfo.icon.SetImageByName(component.GetSpriteById(1).name);
						storyInfo.textAct.gameObject.SetActive(false);
						storyInfo.textNor.gameObject.SetActive(false);
					}
					else
					{
						storyInfo.icon.SetImageByName(component.GetSpriteById(2).name);
						storyInfo.textAct.gameObject.SetActive(false);
						storyInfo.textNor.gameObject.SetActive(true);
						storyInfo.textNor.text = (num + 1).ToString();
					}
				}
				num++;
			}
		}
	}

	// Token: 0x06001C3B RID: 7227 RVA: 0x00165B70 File Offset: 0x00163D70
	public void SetupLevel(int lv)
	{
		if (this.charaPackData != null && this.textLevel != null)
		{
			this.textLevel.text = PrjUtil.MakeMessage("Lv.") + lv.ToString() + PrjUtil.MakeMessage("/") + this.charaPackData.dynamicData.limitLevel.ToString();
		}
	}

	// Token: 0x06001C3C RID: 7228 RVA: 0x00165BD4 File Offset: 0x00163DD4
	public static string Attribute2IconName(CharaDef.AttributeType attribute)
	{
		switch (attribute)
		{
		case CharaDef.AttributeType.RED:
			return "icon_atr_r";
		case CharaDef.AttributeType.GREEN:
			return "icon_atr_g";
		case CharaDef.AttributeType.BLUE:
			return "icon_atr_b";
		case CharaDef.AttributeType.PINK:
			return "icon_atr_r2";
		case CharaDef.AttributeType.LIME:
			return "icon_atr_g2";
		case CharaDef.AttributeType.AQUA:
			return "icon_atr_b2";
		default:
			return "";
		}
	}

	// Token: 0x06001C3D RID: 7229 RVA: 0x00165C2C File Offset: 0x00163E2C
	public static string SubAttribute2IconName(CharaDef.AttributeType attribute)
	{
		switch (attribute)
		{
		case CharaDef.AttributeType.RED:
			return "icon_sub_atr_r";
		case CharaDef.AttributeType.GREEN:
			return "icon_sub_atr_g";
		case CharaDef.AttributeType.BLUE:
			return "icon_sub_atr_b";
		case CharaDef.AttributeType.PINK:
			return "icon_sub_atr_r2";
		case CharaDef.AttributeType.LIME:
			return "icon_sub_atr_g2";
		case CharaDef.AttributeType.AQUA:
			return "icon_sub_atr_b2";
		default:
			return "";
		}
	}

	// Token: 0x06001C3E RID: 7230 RVA: 0x00165C84 File Offset: 0x00163E84
	public void DispRarity(bool flag)
	{
		if (this.imgStar != null)
		{
			for (int i = 0; i < this.imgStar.Count; i++)
			{
				if (!(this.imgStar[i] == null))
				{
					this.imgStar[i].gameObject.SetActive(flag);
					string text = ((this.charaPackData != null && i < this.charaPackData.dynamicData.rank) ? "icon_star" : "icon_star_blank");
					if (this.type == IconCharaCtrl.Type.ICON)
					{
						text += "_s";
					}
					else
					{
						text += "_m";
					}
					this.imgStar[i].SetImageByName(text);
				}
			}
		}
	}

	// Token: 0x06001C3F RID: 7231 RVA: 0x00165D40 File Offset: 0x00163F40
	public void DispPhotoPocketLevel(bool flag)
	{
		if (this.imgMarkPhotoPocketLevel != null)
		{
			this.imgMarkPhotoPocketLevel.SetActive(flag);
		}
	}

	// Token: 0x06001C40 RID: 7232 RVA: 0x00165D5C File Offset: 0x00163F5C
	public void DispMarkAccessory(bool flag)
	{
		if (this.imgMarkAccessory != null)
		{
			this.imgMarkAccessory.gameObject.SetActive(flag);
		}
	}

	// Token: 0x06001C41 RID: 7233 RVA: 0x00165D80 File Offset: 0x00163F80
	public void DispFavorite(CharaPackData cpd)
	{
		if (this.imgFavoriteFlag == null)
		{
			return;
		}
		bool favoriteFlag = cpd.dynamicData.favoriteFlag;
		this.imgFavoriteFlag.gameObject.SetActive(favoriteFlag);
	}

	// Token: 0x06001C42 RID: 7234 RVA: 0x00165DB9 File Offset: 0x00163FB9
	public void DispMarkBan(bool flag)
	{
		if (this.objMarkBan != null)
		{
			this.objMarkBan.SetActive(flag);
		}
	}

	// Token: 0x06001C43 RID: 7235 RVA: 0x00165DD5 File Offset: 0x00163FD5
	public void AddOnClickListener(IconCharaCtrl.OnClick callback)
	{
		this.callbackCL = callback;
		if (this.texChara != null)
		{
			this.texChara.SetRaycastTarget(this.detailParam != null || this.callbackCL != null);
		}
	}

	// Token: 0x06001C44 RID: 7236 RVA: 0x00165E0B File Offset: 0x0016400B
	public void OnPointerClick()
	{
		if (this.callbackCL != null)
		{
			this.callbackCL(this);
		}
	}

	// Token: 0x06001C45 RID: 7237 RVA: 0x00165E24 File Offset: 0x00164024
	public void OnLongPress()
	{
		if (this.charaPackData != null && !this.charaPackData.IsInvalid() && this.detailParam != null)
		{
			if (this.charaPackData.dynamicData.OwnerType == CharaDynamicData.CharaOwnerType.SHOP)
			{
				CharaPackData charaPackData = CharaPackData.MakeUpgradeUserCharaFromShopData(this.charaPackData.staticData.GetId(), this.charaPackData.dynamicData.charaStatusId);
				CanvasManager.HdlCharaWindowCtrl.Open(charaPackData, this.detailParam, null);
				return;
			}
			CanvasManager.HdlCharaWindowCtrl.Open(this.charaPackData, this.detailParam, null);
		}
	}

	// Token: 0x040014F0 RID: 5360
	private static readonly Color MaskColor = new Color(0.6f, 0.6f, 0.6f, 1f);

	// Token: 0x040014F1 RID: 5361
	[SerializeField]
	private IconCharaCtrl.Type type;

	// Token: 0x040014F2 RID: 5362
	[SerializeField]
	private PguiImageCtrl imgBase;

	// Token: 0x040014F3 RID: 5363
	[SerializeField]
	private PguiReplaceSpriteCtrl imgBaseV2;

	// Token: 0x040014F4 RID: 5364
	[SerializeField]
	private PguiRawImageCtrl texChara;

	// Token: 0x040014F5 RID: 5365
	[SerializeField]
	private PguiImageCtrl imgAttribute;

	// Token: 0x040014F6 RID: 5366
	[SerializeField]
	private PguiImageCtrl imgSubAttribute;

	// Token: 0x040014F7 RID: 5367
	[SerializeField]
	private PguiImageCtrl imgMarkGood;

	// Token: 0x040014F8 RID: 5368
	[SerializeField]
	private PguiImageCtrl imgMarkBad;

	// Token: 0x040014F9 RID: 5369
	[SerializeField]
	private PguiTextCtrl textLevel;

	// Token: 0x040014FA RID: 5370
	[SerializeField]
	private PguiTextCtrl textLevelStory;

	// Token: 0x040014FB RID: 5371
	[SerializeField]
	private PguiReplaceSpriteCtrl imgWakeup;

	// Token: 0x040014FC RID: 5372
	[SerializeField]
	private List<PguiImageCtrl> imgStar;

	// Token: 0x040014FD RID: 5373
	[SerializeField]
	private PguiImageCtrl frame;

	// Token: 0x040014FE RID: 5374
	[SerializeField]
	private PguiImageCtrl frameKiseki;

	// Token: 0x040014FF RID: 5375
	[SerializeField]
	private PguiTextCtrl textCharaName;

	// Token: 0x04001500 RID: 5376
	[SerializeField]
	private PguiTextCtrl textCharaNameStory;

	// Token: 0x04001501 RID: 5377
	[SerializeField]
	private PguiTextCtrl textCharaNameStoryWName;

	// Token: 0x04001502 RID: 5378
	[SerializeField]
	private PguiTextCtrl textEponymName;

	// Token: 0x04001503 RID: 5379
	[SerializeField]
	private GameObject currentFrame;

	// Token: 0x04001504 RID: 5380
	[SerializeField]
	private List<IconCharaCtrl.StoryInfo> storyInfo;

	// Token: 0x04001505 RID: 5381
	[SerializeField]
	private GameObject aeNew;

	// Token: 0x04001506 RID: 5382
	[SerializeField]
	private PguiColorCtrl colorAttrBar;

	// Token: 0x04001507 RID: 5383
	[SerializeField]
	private PguiImageCtrl imgAttrBar;

	// Token: 0x04001508 RID: 5384
	[SerializeField]
	private PguiImageCtrl imgEventGrowPickUp;

	// Token: 0x04001509 RID: 5385
	[SerializeField]
	private PguiImageCtrl imgMarkExpUp;

	// Token: 0x0400150A RID: 5386
	[SerializeField]
	private GameObject imgDisable;

	// Token: 0x0400150B RID: 5387
	[SerializeField]
	private PguiImageCtrl imgMarkEvent;

	// Token: 0x0400150C RID: 5388
	[SerializeField]
	private PhotoPocketLevelCtrl imgMarkPhotoPocketLevel;

	// Token: 0x0400150D RID: 5389
	[SerializeField]
	private PguiImageCtrl imgMarkAccessory;

	// Token: 0x0400150E RID: 5390
	[SerializeField]
	private PguiImageCtrl imgFavoriteFlag;

	// Token: 0x0400150F RID: 5391
	[SerializeField]
	private PguiImageCtrl imgMarkEventDropBonus;

	// Token: 0x04001510 RID: 5392
	[SerializeField]
	private GameObject imgBookmark;

	// Token: 0x04001511 RID: 5393
	[SerializeField]
	private GameObject objMarkBan;

	// Token: 0x04001513 RID: 5395
	private CharaWindowCtrl.DetailParamSetting detailParam;

	// Token: 0x04001514 RID: 5396
	private IconCharaCtrl.OnClick callbackCL;

	// Token: 0x04001515 RID: 5397
	private Color? _defaultTextColor;

	// Token: 0x04001516 RID: 5398
	private bool isHelper;

	// Token: 0x02000EFE RID: 3838
	[Serializable]
	public class StoryInfo
	{
		// Token: 0x04005580 RID: 21888
		public PguiImageCtrl icon;

		// Token: 0x04005581 RID: 21889
		public PguiTextCtrl textAct;

		// Token: 0x04005582 RID: 21890
		public PguiTextCtrl textNor;
	}

	// Token: 0x02000EFF RID: 3839
	public enum Type
	{
		// Token: 0x04005584 RID: 21892
		CARD,
		// Token: 0x04005585 RID: 21893
		ICON,
		// Token: 0x04005586 RID: 21894
		ICON_MIDDLE,
		// Token: 0x04005587 RID: 21895
		ICON_TOP
	}

	// Token: 0x02000F00 RID: 3840
	// (Invoke) Token: 0x06004E58 RID: 20056
	public delegate void OnClick(IconCharaCtrl ipc);

	// Token: 0x02000F01 RID: 3841
	public class SetupParam
	{
		// Token: 0x06004E5B RID: 20059 RVA: 0x00236700 File Offset: 0x00234900
		public SetupParam()
		{
			this.cpd = null;
			this.sortType = SortFilterDefine.SortType.LEVEL;
			this.isEnableMask = false;
			this.detaiParam = null;
			this.eventId = 0;
			this.iconId = -1;
		}

		// Token: 0x04005588 RID: 21896
		public CharaPackData cpd;

		// Token: 0x04005589 RID: 21897
		public SortFilterDefine.SortType sortType;

		// Token: 0x0400558A RID: 21898
		public bool isEnableMask;

		// Token: 0x0400558B RID: 21899
		public CharaWindowCtrl.DetailParamSetting detaiParam;

		// Token: 0x0400558C RID: 21900
		public int eventId;

		// Token: 0x0400558D RID: 21901
		public int iconId;
	}
}
