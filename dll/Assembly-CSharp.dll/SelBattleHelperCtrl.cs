using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Common;
using SGNFW.Mst;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001B7 RID: 439
public class SelBattleHelperCtrl : MonoBehaviour
{
	// Token: 0x06001DDF RID: 7647 RVA: 0x00172E9C File Offset: 0x0017109C
	public void Init(UnityAction<SceneManager.SceneName> callback)
	{
		GameObject gameObject = AssetManager.GetAssetData("SceneQuest/GUI/Prefab/GUI_Friend_Select") as GameObject;
		this.guiData = new SelBattleHelperCtrl.GUI(Object.Instantiate<GameObject>(gameObject.transform.Find("FriendSelect").gameObject, base.transform).transform);
		this.guiData.Btn_InfoSwitch.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		ReuseScroll scrollView = this.guiData.ScrollView1;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartItemFriend));
		ReuseScroll scrollView2 = this.guiData.ScrollView1;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItemFriend));
		this.guiData.ScrollView1.Setup(10, 0);
		ReuseScroll scrollView3 = this.guiData.ScrollView2;
		scrollView3.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView3.onStartItem, new Action<int, GameObject>(this.OnStartItemDropItem));
		ReuseScroll scrollView4 = this.guiData.ScrollView2;
		scrollView4.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView4.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItemDropItem));
		this.guiData.ScrollView2.Setup(10, 0);
		this.guiData.ScrollView2.Refresh();
		this.currentAttrIndex = 0;
		this.guiData.TabGroup.Setup(this.currentAttrIndex, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectTab));
		this.guiData.Btn_HelperChange.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonHelperChange), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_HelperChange_Gage.m_Image.fillAmount = 0f;
		this.guiData.Btn_HelperChange_Circle.m_Image.color = Color.white;
		this.helperButtonGage = 0f;
		this.onClickHelperSetting = callback;
	}

	// Token: 0x06001DE0 RID: 7648 RVA: 0x00173078 File Offset: 0x00171278
	public void Setup(int selectQuestOneId, SelBattleHelperCtrl.OnFriendSelect cb, bool isTutorial)
	{
		this.callback = cb;
		this.isTutorial = isTutorial;
		this.selectQuestOneId = selectQuestOneId;
		this.guiData.TabGroup.SelectTab(this.currentAttrIndex);
		this.UpdateHelperList();
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(selectQuestOneId);
		this.enemyAttribute = questOnePackData.questOne.ennemyAttrMask;
		for (int i = 0; i < SceneBattle.attrMatch.GetLength(0); i++)
		{
			bool flag = (this.enemyAttribute & SceneBattle.attrMatch[i, 1]) > (CharaDef.AttributeMask)0;
			bool flag2 = (this.enemyAttribute & SceneBattle.attrMatch[i, 2]) > (CharaDef.AttributeMask)0;
			if (i + 1 < this.guiData.Btn_Filter_List.Count)
			{
				Transform transform = this.guiData.Btn_Filter_List[i + 1].transform.Find("Mark_Good_Bad");
				transform.Find("Mark_Good").gameObject.SetActive(flag);
				transform.Find("Mark_Bad").gameObject.SetActive(flag2);
			}
		}
		Singleton<SceneManager>.Instance.StartCoroutine(this.UpdateFriendSelectDisp(questOnePackData));
	}

	// Token: 0x06001DE1 RID: 7649 RVA: 0x00173194 File Offset: 0x00171394
	private void UpdateHelperList()
	{
		QuestOnePackData qopd = DataManager.DmQuest.GetQuestOnePackData(this.selectQuestOneId);
		this.isOnlyNPC = false;
		this.helperList = new List<List<HelperPackData>>();
		List<DataManagerChara.BonusCharaData> bonusCharaData = ((qopd != null && qopd.questOne.questId > 0) ? DataManager.DmChara.GetBonusCharaDataList(QuestUtil.GetEventId(qopd.questOne.questId, false)) : new List<DataManagerChara.BonusCharaData>());
		int i;
		Predicate<HelperPackData> <>9__0;
		Comparison<HelperPackData> <>9__1;
		int j;
		for (i = 0; i < HelperPackData.HELP_ARRAY_SIZE; i = j + 1)
		{
			List<HelperPackData> list = new List<HelperPackData>(DataManager.DmHelper.GetRentalHelperList());
			List<HelperPackData> list2 = list;
			Predicate<HelperPackData> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = (HelperPackData item) => i >= item.HelperCharaSetList.Count || item.HelperCharaSetList[i].helpChara == null || item.HelperCharaSetList[i].helpChara.IsInvalid());
			}
			list2.RemoveAll(predicate);
			if (qopd != null)
			{
				List<HelperPackData> list3 = list;
				Comparison<HelperPackData> comparison;
				if ((comparison = <>9__1) == null)
				{
					comparison = (<>9__1 = (HelperPackData a, HelperPackData b) => SelBattleHelperCtrl.SortList(a, b, qopd, i, bonusCharaData));
				}
				list3.Sort(comparison);
				if (i == 0)
				{
					this.isOnlyNPC = true;
					using (List<HelperPackData>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							HelperPackData helperPack = enumerator.Current;
							if (!qopd.questOne.DummyHelperList.Exists((int item) => item == helperPack.friendId))
							{
								this.isOnlyNPC = false;
								break;
							}
						}
					}
				}
			}
			this.helperList.Add(list);
			j = i;
		}
		if (this.isOnlyNPC)
		{
			this.SetActiveHelperChangeButton(false, false);
		}
	}

	// Token: 0x06001DE2 RID: 7650 RVA: 0x0017334C File Offset: 0x0017154C
	private void Update()
	{
		if (this.helperButtonGage > 0f)
		{
			this.helperButtonGage = Mathf.Max(0f, this.helperButtonGage - Time.deltaTime / this.HELPER_CHANEGE_BTN_COOL_TIME);
		}
		if (!this.guiData.Btn_HelperChange.ActEnable && !this.isOnlyNPC)
		{
			this.guiData.Btn_HelperChange_Gage.m_Image.fillAmount = this.helperButtonGage;
			if (this.guiData.Btn_HelperChange_Gage.m_Image.fillAmount <= 0f)
			{
				this.SetActiveHelperChangeButton(true, true);
			}
		}
	}

	// Token: 0x06001DE3 RID: 7651 RVA: 0x001733E2 File Offset: 0x001715E2
	public bool IsPlayingAnim()
	{
		return this.guiData.FriendSelectAnim.ExIsPlaying();
	}

	// Token: 0x06001DE4 RID: 7652 RVA: 0x001733F4 File Offset: 0x001715F4
	public void PlayStartAnim(bool start = true)
	{
		this.guiData.FriendSelectAnim.ExPlayAnimation(start ? SimpleAnimation.ExPguiStatus.START : SimpleAnimation.ExPguiStatus.END, null);
	}

	// Token: 0x06001DE5 RID: 7653 RVA: 0x00173410 File Offset: 0x00171610
	public void UpdateCampaign(QuestOnePackData questData = null)
	{
		this.guiData.ResetCampaignInfoCategory();
		if (questData == null)
		{
			questData = DataManager.DmQuest.GetQuestOnePackData(this.selectQuestOneId);
		}
		this.guiData.UpdateCampaignInfoCategory(questData.questChapter.category, questData.questChapter.chapterId);
	}

	// Token: 0x06001DE6 RID: 7654 RVA: 0x00173460 File Offset: 0x00171660
	public static int SortList(HelperPackData a, HelperPackData b, QuestOnePackData selectQuestOnePackData, int index, List<DataManagerChara.BonusCharaData> bonusCharaData)
	{
		DateTime now = TimeManager.Now;
		if (index < 0 || a.HelperCharaSetList.Count <= index || b.HelperCharaSetList.Count <= index)
		{
			return 0;
		}
		if (a.HelperCharaSetList == null || b.HelperCharaSetList == null)
		{
			return 0;
		}
		List<DataManagerChara.BonusCharaData> list = new List<DataManagerChara.BonusCharaData>();
		List<DataManagerChara.BonusCharaData> list2 = new List<DataManagerChara.BonusCharaData>();
		CharaPackData charaA = a.HelperCharaSetList[index].helpChara;
		CharaPackData charaB = b.HelperCharaSetList[index].helpChara;
		int num = 0;
		int num2 = 0;
		DataManagerChara.BonusCharaData bonusCharaData2 = bonusCharaData.Find((DataManagerChara.BonusCharaData itm) => itm.charaId == charaA.id && (itm.increaseItemId01 != 0 || itm.increaseItemId02 != 0));
		if (bonusCharaData2 != null)
		{
			list.Add(bonusCharaData2);
			num = charaA.dynamicData.PhotoPocket.FindAll((CharaDynamicData.PPParam itm) => itm.Flag).Count<CharaDynamicData.PPParam>();
		}
		bonusCharaData2 = bonusCharaData.Find((DataManagerChara.BonusCharaData itm) => itm.charaId == charaB.id && (itm.increaseItemId01 != 0 || itm.increaseItemId02 != 0));
		if (bonusCharaData2 != null)
		{
			list2.Add(bonusCharaData2);
			num2 = charaB.dynamicData.PhotoPocket.FindAll((CharaDynamicData.PPParam itm) => itm.Flag).Count<CharaDynamicData.PPParam>();
		}
		List<DataManagerPhoto.CalcDropBonusResult> list3 = DataManager.DmPhoto.CalcPhotoBonus(a.HelperCharaSetList[index].helpPhotoList, now, a.HelperCharaSetList[index].helpChara);
		list3 = DataManager.DmChara.CalcDropBonus(list3, list, new List<int> { num });
		List<DataManagerPhoto.CalcDropBonusResult> list4 = DataManager.DmPhoto.CalcPhotoBonus(b.HelperCharaSetList[index].helpPhotoList, now, b.HelperCharaSetList[index].helpChara);
		list4 = DataManager.DmChara.CalcDropBonus(list4, list2, new List<int> { num2 });
		int num3 = 0;
		foreach (DataManagerPhoto.CalcDropBonusResult calcDropBonusResult in list3)
		{
			if (selectQuestOnePackData.questOne.DropItemList.Contains(calcDropBonusResult.targetItemId))
			{
				num3 += calcDropBonusResult.ratio;
			}
		}
		int num4 = 0;
		foreach (DataManagerPhoto.CalcDropBonusResult calcDropBonusResult2 in list4)
		{
			if (selectQuestOnePackData.questOne.DropItemList.Contains(calcDropBonusResult2.targetItemId))
			{
				num4 += calcDropBonusResult2.ratio;
			}
		}
		int num5 = num4 - num3;
		if (num5 == 0)
		{
			num5 = (selectQuestOnePackData.questOne.DummyHelperList.Exists((int item) => item == a.friendId) ? 0 : 1) - (selectQuestOnePackData.questOne.DummyHelperList.Exists((int item) => item == b.friendId) ? 0 : 1);
			if (num5 == 0)
			{
				int num6 = (a.isReceiveFollow ? 1 : 0);
				if (a.isSendFollow)
				{
					num6 += 2;
				}
				if (a.friendId == DataManager.DmUserInfo.friendId)
				{
					num6 += 3;
				}
				int num7 = (b.isReceiveFollow ? 1 : 0);
				if (b.isSendFollow)
				{
					num7 += 2;
				}
				if (b.friendId == DataManager.DmUserInfo.friendId)
				{
					num7 += 3;
				}
				num5 = num7 - num6;
				if (num5 == 0)
				{
					num5 = b.HelperCharaSetList[index].helpChara.dynamicData.level - a.HelperCharaSetList[index].helpChara.dynamicData.level;
					if (num5 == 0)
					{
						num5 = 1 - Random.Range(0, 2) * 2;
					}
				}
			}
		}
		return num5;
	}

	// Token: 0x06001DE7 RID: 7655 RVA: 0x00173884 File Offset: 0x00171A84
	private void SwitchCurrentTab(int index)
	{
		this.currentAttrIndex = index;
		this.guiData.ResizeScrollView(this.helperList[this.currentAttrIndex].Count);
	}

	// Token: 0x06001DE8 RID: 7656 RVA: 0x001738AE File Offset: 0x00171AAE
	private IEnumerator UpdateFriendSelectDisp(QuestOnePackData questData)
	{
		this.UpdateCampaign(questData);
		if (questData.questChapter.chapterName.Equals("") || questData.questGroup.Equals(""))
		{
			this.guiData.Num_Quest.transform.parent.gameObject.SetActive(false);
		}
		else
		{
			this.guiData.Num_Quest.transform.parent.gameObject.SetActive(true);
			this.guiData.Num_Quest.text = questData.questChapter.chapterName + questData.questGroup.titleName;
		}
		this.guiData.Txt_QuestName.text = questData.questOne.questName;
		this.guiData.Txt_ChapterName.text = questData.questGroup.storyName;
		if (questData.questOne.useItemId > 0)
		{
			this.guiData.Stamina1.SetActive(false);
			this.guiData.Stamina2.SetActive(true);
			ItemData userItemData = DataManager.DmItem.GetUserItemData(questData.questOne.useItemId);
			this.guiData.Stamina2.transform.Find("Icon_Item").GetComponent<PguiRawImageCtrl>().SetRawImage(userItemData.staticData.GetIconName(), true, false, null);
			this.guiData.Num_Stamina.m_Text.color = this.guiData.Num_Stamina.GetComponent<PguiColorCtrl>().GetGameObjectById("NORMAL");
			this.guiData.Num_Stamina.m_Text.supportRichText = true;
			string text = userItemData.num.ToString();
			if (userItemData.num < questData.questOne.useItemNum)
			{
				text = "<color=#FF0000>" + text + "</color>";
			}
			this.guiData.Num_Stamina.text = text + "/" + questData.questOne.useItemNum.ToString();
		}
		else
		{
			this.guiData.Stamina1.SetActive(true);
			this.guiData.Stamina2.SetActive(false);
			int num = -1;
			Predicate<DataManagerCampaign.CampaignTarget> <>9__0;
			foreach (DataManagerCampaign.CampaignQuestStaminaData campaignQuestStaminaData in DataManager.DmCampaign.PresentCampaignQuestStaminaDataList)
			{
				if (campaignQuestStaminaData.value >= 0)
				{
					List<DataManagerCampaign.CampaignTarget> campaignTargetList = campaignQuestStaminaData.campaignTargetList;
					Predicate<DataManagerCampaign.CampaignTarget> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = (DataManagerCampaign.CampaignTarget itm) => itm.TargetType == DataManagerCampaign.TARGET_TYPE.Chapter && itm.TargetId == questData.questChapter.chapterId);
					}
					if (campaignTargetList.Find(predicate) != null)
					{
						num = campaignQuestStaminaData.value;
						break;
					}
				}
			}
			this.guiData.Num_Stamina.m_Text.color = this.guiData.Num_Stamina.GetComponent<PguiColorCtrl>().GetGameObjectById((num < 0) ? "NORMAL" : "SPECIAL");
			if (num < 0)
			{
				num = 0;
			}
			if ((num = questData.questOne.stamina - num) < 0)
			{
				num = 0;
			}
			this.guiData.Num_Stamina.text = num.ToString();
		}
		this.guiData.Num_Difficulty.text = questData.questOne.difficulty.ToString();
		if (!string.IsNullOrEmpty(questData.questOne.stageName))
		{
			this.guiData.Txt_TerrainInfo.text = questData.questOne.stageName;
		}
		else
		{
			this.guiData.Txt_TerrainInfo.text = CharaDef.GetAbilityTraitsName(questData.questOne.traitsType);
		}
		if (this.guiData.Txt_TerrainInfo.text.Length >= 13)
		{
			this.guiData.Txt_TerrainInfo.m_Text.fontSize = 19;
		}
		else if (this.guiData.Txt_TerrainInfo.text.Length >= 11)
		{
			this.guiData.Txt_TerrainInfo.m_Text.fontSize = 22;
		}
		HashSet<int> hashSet = new HashSet<int>();
		foreach (QuestStaticWave.WaveStatic waveStatic in questData.questOne.waveData.waveList)
		{
			hashSet.Add(waveStatic.id);
		}
		this.guiData.Num_Wave.text = PrjUtil.MakeMessage(hashSet.Count.ToString());
		this.guiData.Mark_Night.gameObject.SetActive(questData.questOne.isNightTraits);
		this.guiData.Txt_ButtonInfo.text = (this.friendSelectInfoFlag ? PrjUtil.MakeMessage("クエスト情報") : PrjUtil.MakeMessage("獲得情報"));
		this.guiData.questInfoObj.SetActive(!this.friendSelectInfoFlag);
		this.guiData.itemInfoObj.SetActive(this.friendSelectInfoFlag);
		this.guiData.Mark_Hard.gameObject.SetActive(QuestUtil.IsHardMode(new QuestUtil.SelectData
		{
			chapterId = questData.questChapter.chapterId,
			questCategory = questData.questChapter.category
		}));
		this.guiData.Mark_NotContinue.gameObject.SetActive(questData.questOne.ContinueImpossible);
		this.guiData.Mark_NotDhole.gameObject.SetActive(questData.questOne.QuestCategory == QuestStaticQuestOne.QuestOneCategory.NoDhole);
		this.guiData.Mark_NotLeader.gameObject.SetActive(questData.questOne.QuestCategory == QuestStaticQuestOne.QuestOneCategory.NoPlayer);
		this.guiData.ResizeScrollView(this.helperList[this.currentAttrIndex].Count);
		this.dispItemIdList = new List<SelBattleHelperCtrl.DispItem>();
		DataManagerEvent.EventData eventDataCompareToChapterId = DataManager.DmEvent.GetEventDataCompareToChapterId(questData.questChapter.chapterId);
		if (eventDataCompareToChapterId == null || !eventDataCompareToChapterId.raidFlg)
		{
			List<MstQuestDrawItemData> mst = Singleton<MstManager>.Instance.GetMst<List<MstQuestDrawItemData>>(MstType.QUEST_DRAW_ITEM_DATA);
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			foreach (DataManagerPhoto.PhotoDropItemData photoDropItemData in questData.questOne.PhotoDropItemList)
			{
				if (photoDropItemData.PhotoId > 0)
				{
					foreach (MstQuestDrawItemData mstQuestDrawItemData in mst)
					{
						if (mstQuestDrawItemData.drawId == photoDropItemData.BonusDrawId && mstQuestDrawItemData.itemId > 0 && mstQuestDrawItemData.itemNum > 0)
						{
							if (!dictionary.ContainsKey(mstQuestDrawItemData.itemId))
							{
								dictionary.Add(mstQuestDrawItemData.itemId, new List<int>());
							}
							if (!dictionary[mstQuestDrawItemData.itemId].Contains(photoDropItemData.PhotoId))
							{
								dictionary[mstQuestDrawItemData.itemId].Add(photoDropItemData.PhotoId);
							}
						}
					}
				}
			}
			foreach (int num2 in dictionary.Keys)
			{
				this.dispItemIdList.Add(new SelBattleHelperCtrl.DispItem
				{
					id = num2,
					gentei = false,
					photo = dictionary[num2]
				});
			}
			foreach (int num3 in questData.questOne.QuestDropItemList)
			{
				this.dispItemIdList.Add(new SelBattleHelperCtrl.DispItem
				{
					id = num3,
					gentei = (eventDataCompareToChapterId == null || (eventDataCompareToChapterId != null && !eventDataCompareToChapterId.raidFlg)),
					photo = null
				});
			}
			using (HashSet<int>.Enumerator enumerator6 = questData.questOne.EnemyDropItemIdList.GetEnumerator())
			{
				while (enumerator6.MoveNext())
				{
					int num4 = enumerator6.Current;
					SelBattleHelperCtrl.DispItem dispItem = new SelBattleHelperCtrl.DispItem
					{
						id = num4,
						gentei = false,
						photo = null
					};
					this.dispItemIdList.Add(dispItem);
				}
				goto IL_0A90;
			}
		}
		HashSet<int> hashSet2 = new HashSet<int>(questData.questOne.EnemyDropDrawDataList.Select<MstQuestDrawItemData, int>((MstQuestDrawItemData data) => data.drawId));
		hashSet2.UnionWith(questData.questOne.QuestDropDrawDataList.Select<MstQuestQuestdropItemData, int>((MstQuestQuestdropItemData data) => data.bonusDrawId));
		List<int> convertDrawIdList = new List<int>();
		foreach (int num5 in hashSet2)
		{
			int mapId = questData.questMap.mapId;
			foreach (int num6 in DataManager.DmEvent.GetConvertDrawId(DataManager.DmEvent.LastCoopInfo.EventId, mapId, num5))
			{
				convertDrawIdList.Add((num6 == 0) ? num5 : num6);
			}
		}
		using (List<MstQuestDrawItemData>.Enumerator enumerator4 = DataManager.DmQuest.MstDrawItemDataList.FindAll((MstQuestDrawItemData data) => convertDrawIdList.Contains(data.drawId)).GetEnumerator())
		{
			while (enumerator4.MoveNext())
			{
				MstQuestDrawItemData drawItemData = enumerator4.Current;
				if (this.dispItemIdList.Find((SelBattleHelperCtrl.DispItem item) => item.id == drawItemData.itemId) == null)
				{
					this.dispItemIdList.Add(new SelBattleHelperCtrl.DispItem
					{
						id = drawItemData.itemId,
						gentei = false,
						photo = null
					});
				}
			}
		}
		IL_0A90:
		this.guiData.ScrollView2.Resize(this.dispItemIdList.Count / SelBattleHelperCtrl.GUI.ITEM_INFO_DROP_H + 1, 0);
		CharaDef.AttributeMask ennemyAttrMask = questData.questOne.ennemyAttrMask;
		for (int i = 0; i < this.guiData.enemyInfoList.Count; i++)
		{
			this.guiData.enemyInfoList[i].Setup((ennemyAttrMask & SelBattleHelperCtrl.GUI.EnemyInfo.attributeMaskList[i]) == SelBattleHelperCtrl.GUI.EnemyInfo.attributeMaskList[i]);
		}
		this.guiData.FriendSelectAnim.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		yield return null;
		this.PlayStartAnim(true);
		yield break;
	}

	// Token: 0x06001DE9 RID: 7657 RVA: 0x001738C4 File Offset: 0x00171AC4
	private void OnClickButton(PguiButtonCtrl button)
	{
		if (button == this.guiData.Btn_InfoSwitch)
		{
			this.friendSelectInfoFlag = !this.friendSelectInfoFlag;
			this.guiData.Txt_ButtonInfo.text = (this.friendSelectInfoFlag ? PrjUtil.MakeMessage("クエスト情報") : PrjUtil.MakeMessage("獲得情報"));
			this.guiData.questInfoObj.SetActive(!this.friendSelectInfoFlag);
			this.guiData.itemInfoObj.SetActive(this.friendSelectInfoFlag);
		}
	}

	// Token: 0x06001DEA RID: 7658 RVA: 0x00173950 File Offset: 0x00171B50
	private void OnStartItemFriend(int index, GameObject go)
	{
		go.GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonFriendSelect), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.friendChipList.Add(new SelBattleHelperCtrl.GUI.FriendChip(go.transform));
		go.AddComponent<PguiDataHolder>();
	}

	// Token: 0x06001DEB RID: 7659 RVA: 0x0017398C File Offset: 0x00171B8C
	private void OnUpdateItemFriend(int index, GameObject go)
	{
		int num = 0;
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(this.selectQuestOneId);
		if (questOnePackData != null)
		{
			foreach (DataManagerEvent.EventData eventData in DataManager.DmEvent.GetEventDataList())
			{
				if (eventData.IsEnableEvent && eventData.eventChapterId == questOnePackData.questChapter.chapterId)
				{
					num = eventData.eventId;
					break;
				}
			}
		}
		SelBattleHelperCtrl.GUI.FriendChip friendChip = this.guiData.friendChipList.Find((SelBattleHelperCtrl.GUI.FriendChip item) => item.baseObj == go);
		HelperPackData helperPack = this.helperList[this.currentAttrIndex][index];
		go.GetComponent<PguiDataHolder>().id = helperPack.friendId;
		bool flag = questOnePackData != null && questOnePackData.questOne.DummyHelperList.Exists((int item) => item == helperPack.friendId);
		friendChip.Txt_FriendName.text = helperPack.userName;
		friendChip.Num_Rank.ReplaceTextByDefault("Param01", helperPack.level.ToString());
		friendChip.Num_Rank.gameObject.SetActive(!flag);
		CharaWindowCtrl.DetailParamSetting detailParamSetting = null;
		if (!this.isTutorial)
		{
			detailParamSetting = new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.OTHER_WITH_KEMOBOARD, null)
			{
				selectQuestOneId = this.selectQuestOneId,
				selectEventId = num
			};
		}
		List<DataManagerChara.BonusCharaData> list = ((this.selectQuestOneId > 0) ? DataManager.DmChara.GetBonusCharaDataList(QuestUtil.GetEventId(this.selectQuestOneId, false)) : new List<DataManagerChara.BonusCharaData>());
		List<DataManagerChara.BonusCharaData> list2 = new List<DataManagerChara.BonusCharaData>();
		List<int> list3 = new List<int>();
		CharaPackData cpd = helperPack.HelperCharaSetList[this.currentAttrIndex].helpChara;
		DataManagerChara.BonusCharaData bonusCharaData = list.Find((DataManagerChara.BonusCharaData itm) => itm.charaId == cpd.dynamicData.id && (itm.increaseItemId01 != 0 || itm.increaseItemId02 != 0));
		if (bonusCharaData != null)
		{
			list2.Add(bonusCharaData);
			int num2 = cpd.dynamicData.PhotoPocket.FindAll((CharaDynamicData.PPParam itm) => itm.Flag).Count<CharaDynamicData.PPParam>();
			list3.Add(num2);
		}
		friendChip.iconCharaCtrl.Setup(cpd, SortFilterDefine.SortType.LEVEL, false, detailParamSetting, 0, -1, this.selectQuestOneId);
		friendChip.iconCharaCtrl.DispAttributeMark(this.enemyAttribute);
		friendChip.iconCharaCtrl.DispPhotoPocketLevel(true);
		if (questOnePackData.questChapter.category != QuestStaticChapter.Category.TRAINING && questOnePackData.questChapter.category != QuestStaticChapter.Category.PVP)
		{
			int currentDeck = DataManager.DmUserInfo.optionData.CurrentQuestParty;
			int num3 = DataManager.DmDeck.GetUserDeckList(UserDeckData.Category.NORMAL).Find((UserDeckData item) => item.id == currentDeck).charaIdList.First<int>((int item) => item != 0);
			CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(num3);
			List<CharaStaticData> list4 = new List<CharaStaticData>();
			list4.Add(charaStaticData);
			bool flag2 = QuestUtil.IsBanTarget(DataManager.DmChara.GetCharaStaticData(cpd.id), questOnePackData, list4);
			friendChip.iconCharaCtrl.DispMarkBan(flag2);
		}
		List<PhotoPackData> list5 = new List<PhotoPackData>();
		List<PhotoPackData> list6 = new List<PhotoPackData>();
		for (int i = 0; i < friendChip.iconPhotoList.Count; i++)
		{
			PhotoPackData photoPackData = helperPack.HelperCharaSetList[this.currentAttrIndex].helpPhotoList[i];
			friendChip.iconPhotoList[i].Setup(new IconPhotoCtrl.SetupParam
			{
				ppd = helperPack.HelperCharaSetList[this.currentAttrIndex].helpPhotoList[i],
				isEnableRaycast = false,
				playQuestOneId = this.selectQuestOneId,
				isHelper = true
			});
			bool flag3 = cpd.dynamicData.PhotoPocket[i].Flag && (photoPackData == null || !photoPackData.staticData.baseData.kizunaPhotoFlg || photoPackData.staticData.GetId() == cpd.staticData.baseData.kizunaPhotoId);
			friendChip.iconPhotoList[i].DispImgDisable(!flag3);
			if (flag3)
			{
				list6.Add(photoPackData);
				PhotoUtil.RefDropItemEffectPhotoList(ref list5, photoPackData, true);
			}
		}
		if (friendChip.Info_PhotoItemEffect != null)
		{
			friendChip.Info_PhotoItemEffect.Setup(QuestUtil.GetCalcDropBonusResultDeck(questOnePackData, list6, list2, list3), list5);
		}
		friendChip.Num_FriendPont.ReplaceTextByDefault("Param01", helperPack.addPoint.ToString());
		int num4 = 0;
		if (helperPack.isReceiveFollow && helperPack.isSendFollow)
		{
			num4 = 1;
		}
		else if (helperPack.isReceiveFollow)
		{
			num4 = 2;
		}
		else if (helperPack.isSendFollow)
		{
			num4 = 3;
		}
		friendChip.Mark_Friend.gameObject.SetActive(num4 > 0);
		friendChip.Mark_Friend.Replace(num4);
		friendChip.Txt_LastLogin.text = PrjUtil.MakeMessage("最終ログイン") + TimeManager.MakeTimeSpanText(helperPack.lastLoginTime, TimeManager.Now) + PrjUtil.MakeMessage("前");
		friendChip.Txt_LastLogin.gameObject.SetActive(!flag);
		friendChip.Mark_NPC.gameObject.SetActive(flag);
		friendChip.Mark_PhotoPocketLevel.Setup(new PhotoPocketLevelCtrl.SetupParam
		{
			charaPackData = cpd
		});
		friendChip.Mark_PhotoPocketLevel.SetActive(false);
		friendChip.Achievement.Setup(helperPack.achievementId, true, false);
		friendChip.Achievement.InvalidTouchPanel();
		friendChip.NormalDecoration.SetActive(helperPack.friendId != DataManager.DmUserInfo.friendId);
		friendChip.OwnDecoration.SetActive(helperPack.friendId == DataManager.DmUserInfo.friendId);
		friendChip.HelperSetting.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickHelperSetting), PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x06001DEC RID: 7660 RVA: 0x00173FC8 File Offset: 0x001721C8
	private void OnStartItemDropItem(int index, GameObject go)
	{
		for (int i = 0; i < SelBattleHelperCtrl.GUI.ITEM_INFO_DROP_H; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, go.transform);
			gameObject.name = i.ToString();
			gameObject.GetComponent<IconItemCtrl>();
		}
	}

	// Token: 0x06001DED RID: 7661 RVA: 0x00174010 File Offset: 0x00172210
	private void OnUpdateItemDropItem(int index, GameObject go)
	{
		for (int i = 0; i < SelBattleHelperCtrl.GUI.ITEM_INFO_DROP_H; i++)
		{
			int num = index * SelBattleHelperCtrl.GUI.ITEM_INFO_DROP_H + i;
			IconItemCtrl component = go.transform.Find(i.ToString()).GetComponent<IconItemCtrl>();
			if (num < this.dispItemIdList.Count && this.dispItemIdList[num].id > 0)
			{
				component.Setup(DataManager.DmItem.GetItemStaticBase(this.dispItemIdList[num].id), new IconItemCtrl.SetupParam
				{
					useInfo = true,
					useMaxDetail = true,
					gentei = this.dispItemIdList[num].gentei,
					photoLottery = this.dispItemIdList[num].photo,
					forceSetup = true
				});
			}
			else
			{
				component.Clear();
			}
		}
	}

	// Token: 0x06001DEE RID: 7662 RVA: 0x001740E8 File Offset: 0x001722E8
	private void OnClickButtonFriendSelect(PguiButtonCtrl buttuon)
	{
		HelperPackData helperPackData = this.helperList[this.currentAttrIndex].Find((HelperPackData item) => item.friendId == buttuon.GetComponent<PguiDataHolder>().id);
		DataManager.DmHelper.UpdateLatestHelper(helperPackData, this.currentAttrIndex);
		this.callback(helperPackData, this.currentAttrIndex);
	}

	// Token: 0x06001DEF RID: 7663 RVA: 0x00174148 File Offset: 0x00172348
	private void OnClickButtonHelperChange(PguiButtonCtrl button)
	{
		if (!button.ActEnable)
		{
			return;
		}
		Singleton<SceneManager>.Instance.StartCoroutine(this.OnClickButtonHelperChangeCoroutine());
	}

	// Token: 0x06001DF0 RID: 7664 RVA: 0x00174164 File Offset: 0x00172364
	public IEnumerator OnClickButtonHelperChangeCoroutine()
	{
		DataManager.DmHelper.RequestGetRentalHelper(this.selectQuestOneId, true);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.UpdateHelperList();
		this.guiData.ResizeScrollView(this.helperList[this.currentAttrIndex].Count);
		this.SetActiveHelperChangeButton(false, true);
		this.helperButtonGage = 1f;
		yield break;
	}

	// Token: 0x06001DF1 RID: 7665 RVA: 0x00174174 File Offset: 0x00172374
	private void SetActiveHelperChangeButton(bool isActEnable, bool isGauging)
	{
		if (isGauging)
		{
			this.guiData.Btn_HelperChange.SetActEnable(true, true, false);
		}
		this.guiData.Btn_HelperChange.SetActEnable(isActEnable, true, isGauging);
		if (isActEnable)
		{
			this.guiData.Btn_HelperChange.SetSoundType(PguiButtonCtrl.SoundType.DEFAULT);
			this.guiData.Btn_HelperChange_Gage.m_Image.fillAmount = 0f;
			this.guiData.Btn_HelperChange_Circle.m_Image.color = Color.white;
			return;
		}
		this.guiData.Btn_HelperChange.SetSoundType(PguiButtonCtrl.SoundType.CANCEL);
		if (isGauging)
		{
			this.guiData.Btn_HelperChange_Gage.m_Image.fillAmount = 1f;
		}
		this.guiData.Btn_HelperChange_Circle.m_Image.color = new Color(0.7f, 0.7f, 0.7f, 1f);
	}

	// Token: 0x06001DF2 RID: 7666 RVA: 0x0017424F File Offset: 0x0017244F
	private bool OnSelectTab(int index)
	{
		this.SwitchCurrentTab(index);
		return true;
	}

	// Token: 0x06001DF3 RID: 7667 RVA: 0x00174259 File Offset: 0x00172459
	public void OnClickHelperSetting(PguiButtonCtrl button)
	{
		this.onClickHelperSetting(SceneManager.SceneName.SceneProfile);
	}

	// Token: 0x040015F9 RID: 5625
	private SelBattleHelperCtrl.GUI guiData;

	// Token: 0x040015FA RID: 5626
	private List<List<HelperPackData>> helperList;

	// Token: 0x040015FB RID: 5627
	private SelBattleHelperCtrl.OnFriendSelect callback;

	// Token: 0x040015FC RID: 5628
	private bool friendSelectInfoFlag;

	// Token: 0x040015FD RID: 5629
	private int currentAttrIndex;

	// Token: 0x040015FE RID: 5630
	private List<SelBattleHelperCtrl.DispItem> dispItemIdList = new List<SelBattleHelperCtrl.DispItem>();

	// Token: 0x040015FF RID: 5631
	private bool isTutorial;

	// Token: 0x04001600 RID: 5632
	private int selectQuestOneId;

	// Token: 0x04001601 RID: 5633
	private CharaDef.AttributeMask enemyAttribute;

	// Token: 0x04001602 RID: 5634
	private readonly float HELPER_CHANEGE_BTN_COOL_TIME = 1f;

	// Token: 0x04001603 RID: 5635
	private float helperButtonGage;

	// Token: 0x04001604 RID: 5636
	private bool isOnlyNPC;

	// Token: 0x04001605 RID: 5637
	public UnityAction<SceneManager.SceneName> onClickHelperSetting;

	// Token: 0x02000F68 RID: 3944
	// (Invoke) Token: 0x06004F7D RID: 20349
	public delegate void OnFriendSelect(HelperPackData helper, int attrIndex);

	// Token: 0x02000F69 RID: 3945
	private class DispItem
	{
		// Token: 0x0400572E RID: 22318
		public int id;

		// Token: 0x0400572F RID: 22319
		public bool gentei;

		// Token: 0x04005730 RID: 22320
		public List<int> photo;
	}

	// Token: 0x02000F6A RID: 3946
	public class GUI
	{
		// Token: 0x06004F81 RID: 20353 RVA: 0x0023B368 File Offset: 0x00239568
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_InfoSwitch = baseTr.Find("Left/QuestInfoAll/Btn_InfoSwitch").GetComponent<PguiButtonCtrl>();
			this.AtrInfo = baseTr.Find("Left/QuestInfoAll/QuestInfo/AtrInfo").gameObject;
			this.Mark_Night = baseTr.Find("Left/QuestInfoAll/QuestInfo/Mark_Night").GetComponent<PguiImageCtrl>();
			this.Txt_ChapterName = baseTr.Find("Left/QuestInfoAll/Txt_ChapterName").GetComponent<PguiTextCtrl>();
			this.Num_Quest = baseTr.Find("Left/QuestInfoAll/ScenarioBase/Num_Quest").GetComponent<PguiTextCtrl>();
			this.Txt_QuestName = baseTr.Find("Left/QuestInfoAll/Txt_QuestName").GetComponent<PguiTextCtrl>();
			this.FriendSelectAnim = baseTr.GetComponent<SimpleAnimation>();
			this.ScrollView1 = baseTr.Find("Right/ScrollView").GetComponent<ReuseScroll>();
			this.ScrollView2 = baseTr.Find("Left/QuestInfoAll/ItemInfo/ScrollView").GetComponent<ReuseScroll>();
			this.Num_Stamina = baseTr.Find("Left/QuestInfoAll/QuestInfo/Contents01/Num_01").GetComponent<PguiTextCtrl>();
			this.Stamina1 = baseTr.Find("Left/QuestInfoAll/QuestInfo/Contents01/Base/Txt_01").gameObject;
			this.Stamina2 = baseTr.Find("Left/QuestInfoAll/QuestInfo/Contents01/Base/Txt_02").gameObject;
			this.Num_Difficulty = baseTr.Find("Left/QuestInfoAll/QuestInfo/Contents02/Num_02").GetComponent<PguiTextCtrl>();
			this.Txt_TerrainInfo = baseTr.Find("Left/QuestInfoAll/QuestInfo/Contents04/Txt_Info").GetComponent<PguiTextCtrl>();
			this.Num_Wave = baseTr.Find("Left/QuestInfoAll/QuestInfo/Contents05/Num").GetComponent<PguiTextCtrl>();
			this.Txt_ButtonInfo = baseTr.Find("Left/QuestInfoAll/Btn_InfoSwitch/BaseImage/Txt").GetComponent<PguiTextCtrl>();
			this.TabGroup = baseTr.Find("Right/TabGroup").GetComponent<PguiTabGroupCtrl>();
			this.Btn_Filter_all = baseTr.Find("Right/TabGroup/Btn_Filter_all").GetComponent<PguiTabCtrl>();
			this.Btn_Filter_funny = baseTr.Find("Right/TabGroup/Btn_Filter_funny").GetComponent<PguiTabCtrl>();
			this.Btn_Filter_friendly = baseTr.Find("Right/TabGroup/Btn_Filter_friendly").GetComponent<PguiTabCtrl>();
			this.Btn_Filter_relax = baseTr.Find("Right/TabGroup/Btn_Filter_relax").GetComponent<PguiTabCtrl>();
			this.Btn_Filter_lovely = baseTr.Find("Right/TabGroup/Btn_Filter_lovely").GetComponent<PguiTabCtrl>();
			this.Btn_Filter_active = baseTr.Find("Right/TabGroup/Btn_Filter_active").GetComponent<PguiTabCtrl>();
			this.Btn_Filter_mypace = baseTr.Find("Right/TabGroup/Btn_Filter_mypace").GetComponent<PguiTabCtrl>();
			this.Btn_Filter_List = new List<PguiTabCtrl> { this.Btn_Filter_all, this.Btn_Filter_funny, this.Btn_Filter_friendly, this.Btn_Filter_relax, this.Btn_Filter_lovely, this.Btn_Filter_active, this.Btn_Filter_mypace };
			this.enemyInfoList = new List<SelBattleHelperCtrl.GUI.EnemyInfo>
			{
				new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("Left/QuestInfoAll/QuestInfo/AtrInfo/AtrInfo01/Icon_Atr_R")),
				new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("Left/QuestInfoAll/QuestInfo/AtrInfo/AtrInfo01/Icon_Atr_G")),
				new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("Left/QuestInfoAll/QuestInfo/AtrInfo/AtrInfo01/Icon_Atr_B")),
				new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("Left/QuestInfoAll/QuestInfo/AtrInfo/AtrInfo02/Icon_Atr_R")),
				new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("Left/QuestInfoAll/QuestInfo/AtrInfo/AtrInfo02/Icon_Atr_G")),
				new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("Left/QuestInfoAll/QuestInfo/AtrInfo/AtrInfo02/Icon_Atr_B"))
			};
			this.questInfoObj = baseTr.Find("Left/QuestInfoAll/QuestInfo").gameObject;
			this.itemInfoObj = baseTr.Find("Left/QuestInfoAll/ItemInfo").gameObject;
			this.campaignInfo = new QuestUtil.CampaignInfo(baseTr.Find("Right/Campaign"));
			this.Btn_HelperChange = baseTr.Find("Right/BtnChange").GetComponent<PguiButtonCtrl>();
			this.Btn_HelperChange_Gage = baseTr.Find("Right/BtnChange/BaseImage/Gage").GetComponent<PguiImageCtrl>();
			this.Btn_HelperChange_Circle = baseTr.Find("Right/BtnChange/BaseImage/CircleImage").GetComponent<PguiImageCtrl>();
			this.Mark_Hard = baseTr.Find("Left/QuestInfoAll/Mark_Hard").GetComponent<PguiImageCtrl>();
			this.Mark_NotContinue = baseTr.Find("Left/QuestInfoAll/Mark_NotContinue").GetComponent<PguiImageCtrl>();
			this.Mark_NotDhole = baseTr.Find("Left/QuestInfoAll/Mark_NotDhole").GetComponent<PguiImageCtrl>();
			this.Mark_NotLeader = baseTr.Find("Left/QuestInfoAll/Mark_NotLeader").GetComponent<PguiImageCtrl>();
			this.Txt_None = baseTr.Find("Right/Txt_None").gameObject;
			this.Txt_None.SetActive(false);
		}

		// Token: 0x06004F82 RID: 20354 RVA: 0x0023B790 File Offset: 0x00239990
		public void UpdateCampaignInfoCategory(QuestStaticChapter.Category category, int chapterId)
		{
			List<string> list = new List<string>(QuestUtil.GetCampaignMessageList(category, chapterId));
			List<string> list2 = new List<string>(QuestUtil.GetCampaignTimeList(category, chapterId));
			this.campaignInfo.DispCampaign(list, list2);
		}

		// Token: 0x06004F83 RID: 20355 RVA: 0x0023B7C4 File Offset: 0x002399C4
		public void ResetCampaignInfoCategory()
		{
			this.campaignInfo.ResetCampaign();
		}

		// Token: 0x06004F84 RID: 20356 RVA: 0x0023B7D1 File Offset: 0x002399D1
		public void ResizeScrollView(int count)
		{
			this.Txt_None.SetActive(count <= 0);
			this.ScrollView1.Resize(count, 0);
		}

		// Token: 0x04005731 RID: 22321
		public static readonly int ITEM_INFO_DROP_H = 3;

		// Token: 0x04005732 RID: 22322
		public GameObject baseObj;

		// Token: 0x04005733 RID: 22323
		public PguiButtonCtrl Btn_InfoSwitch;

		// Token: 0x04005734 RID: 22324
		public GameObject AtrInfo;

		// Token: 0x04005735 RID: 22325
		public PguiImageCtrl Mark_Night;

		// Token: 0x04005736 RID: 22326
		public PguiTextCtrl Txt_ChapterName;

		// Token: 0x04005737 RID: 22327
		public PguiTextCtrl Num_Quest;

		// Token: 0x04005738 RID: 22328
		public PguiTextCtrl Txt_QuestName;

		// Token: 0x04005739 RID: 22329
		public PguiTextCtrl Num_Stamina;

		// Token: 0x0400573A RID: 22330
		public GameObject Stamina1;

		// Token: 0x0400573B RID: 22331
		public GameObject Stamina2;

		// Token: 0x0400573C RID: 22332
		public PguiTextCtrl Num_Difficulty;

		// Token: 0x0400573D RID: 22333
		public PguiTextCtrl Txt_TerrainInfo;

		// Token: 0x0400573E RID: 22334
		public PguiTextCtrl Num_Wave;

		// Token: 0x0400573F RID: 22335
		public PguiTextCtrl Txt_ButtonInfo;

		// Token: 0x04005740 RID: 22336
		public SimpleAnimation FriendSelectAnim;

		// Token: 0x04005741 RID: 22337
		public ReuseScroll ScrollView1;

		// Token: 0x04005742 RID: 22338
		public ReuseScroll ScrollView2;

		// Token: 0x04005743 RID: 22339
		public PguiTabGroupCtrl TabGroup;

		// Token: 0x04005744 RID: 22340
		public List<PguiTabCtrl> Btn_Filter_List;

		// Token: 0x04005745 RID: 22341
		public PguiTabCtrl Btn_Filter_all;

		// Token: 0x04005746 RID: 22342
		public PguiTabCtrl Btn_Filter_funny;

		// Token: 0x04005747 RID: 22343
		public PguiTabCtrl Btn_Filter_friendly;

		// Token: 0x04005748 RID: 22344
		public PguiTabCtrl Btn_Filter_relax;

		// Token: 0x04005749 RID: 22345
		public PguiTabCtrl Btn_Filter_lovely;

		// Token: 0x0400574A RID: 22346
		public PguiTabCtrl Btn_Filter_active;

		// Token: 0x0400574B RID: 22347
		public PguiTabCtrl Btn_Filter_mypace;

		// Token: 0x0400574C RID: 22348
		public GameObject questInfoObj;

		// Token: 0x0400574D RID: 22349
		public GameObject itemInfoObj;

		// Token: 0x0400574E RID: 22350
		public List<SelBattleHelperCtrl.GUI.FriendChip> friendChipList = new List<SelBattleHelperCtrl.GUI.FriendChip>();

		// Token: 0x0400574F RID: 22351
		public List<SelBattleHelperCtrl.GUI.EnemyInfo> enemyInfoList = new List<SelBattleHelperCtrl.GUI.EnemyInfo>();

		// Token: 0x04005750 RID: 22352
		public QuestUtil.CampaignInfo campaignInfo;

		// Token: 0x04005751 RID: 22353
		public PguiButtonCtrl Btn_HelperChange;

		// Token: 0x04005752 RID: 22354
		public PguiImageCtrl Btn_HelperChange_Gage;

		// Token: 0x04005753 RID: 22355
		public PguiImageCtrl Btn_HelperChange_Circle;

		// Token: 0x04005754 RID: 22356
		public PguiImageCtrl Mark_Hard;

		// Token: 0x04005755 RID: 22357
		public PguiImageCtrl Mark_NotContinue;

		// Token: 0x04005756 RID: 22358
		public PguiImageCtrl Mark_NotDhole;

		// Token: 0x04005757 RID: 22359
		public PguiImageCtrl Mark_NotLeader;

		// Token: 0x04005758 RID: 22360
		public GameObject Txt_None;

		// Token: 0x0200120A RID: 4618
		public class FriendChip
		{
			// Token: 0x0600579E RID: 22430 RVA: 0x00257B0C File Offset: 0x00255D0C
			public FriendChip(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Quest_ListBar_Friend = baseTr.GetComponent<PguiButtonCtrl>();
				this.Mark_Friend = baseTr.Find("BaseImage/Normal/Mark_Friend").GetComponent<PguiReplaceSpriteCtrl>();
				this.Txt_FriendName = baseTr.Find("BaseImage/Txt_FriendName").GetComponent<PguiTextCtrl>();
				this.Num_Rank = baseTr.Find("BaseImage/Num_Rank").GetComponent<PguiTextCtrl>();
				this.Num_FriendPont = baseTr.Find("BaseImage/Normal/FriendPoint/Num_FriendPont").GetComponent<PguiTextCtrl>();
				this.Txt_LastLogin = baseTr.Find("BaseImage/Normal/Txt_LastLogin").GetComponent<PguiTextCtrl>();
				this.Mark_NPC = baseTr.Find("BaseImage/Mark_NPC").GetComponent<PguiImageCtrl>();
				this.Mark_NPC.gameObject.SetActive(false);
				this.bonusIconImage = baseTr.Find("BaseImage/Mark_EffectPhoto/Icon_Item").GetComponent<PguiRawImageCtrl>();
				this.bonusText = baseTr.Find("BaseImage/Mark_EffectPhoto/Txt_Effect").GetComponent<PguiTextCtrl>();
				GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara, baseTr.Find("BaseImage/Icon_Chara"));
				this.iconCharaCtrl = gameObject.GetComponent<IconCharaCtrl>();
				for (int i = 0; i < 4; i++)
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Photo, baseTr.Find("BaseImage/Icon_Photo0" + (i + 1).ToString()));
					this.iconPhotoList.Add(gameObject2.GetComponent<IconPhotoCtrl>());
				}
				this.Info_PhotoItemEffect = baseTr.Find("BaseImage/Info_PhotoItemEffect").GetComponent<InfoPhotoItemEffectCtrl>();
				this.Mark_PhotoPocketLevel = baseTr.Find("BaseImage/Mark_PhotoPocketLevel").GetComponent<PhotoPocketLevelCtrl>();
				this.Achievement = baseTr.Find("BaseImage/Achievement").GetComponent<AchievementCtrl>();
				this.NormalDecoration = baseTr.Find("BaseImage/Normal").gameObject;
				this.OwnDecoration = baseTr.Find("BaseImage/Own").gameObject;
				this.HelperSetting = baseTr.Find("BaseImage/Own/Btn_Helper").GetComponent<PguiButtonCtrl>();
			}

			// Token: 0x040062B0 RID: 25264
			public GameObject baseObj;

			// Token: 0x040062B1 RID: 25265
			public PguiButtonCtrl Quest_ListBar_Friend;

			// Token: 0x040062B2 RID: 25266
			public PguiReplaceSpriteCtrl Mark_Friend;

			// Token: 0x040062B3 RID: 25267
			public PguiTextCtrl Txt_FriendName;

			// Token: 0x040062B4 RID: 25268
			public PguiTextCtrl Num_Rank;

			// Token: 0x040062B5 RID: 25269
			public PguiTextCtrl Num_FriendPont;

			// Token: 0x040062B6 RID: 25270
			public PguiTextCtrl Txt_LastLogin;

			// Token: 0x040062B7 RID: 25271
			public PguiImageCtrl Mark_NPC;

			// Token: 0x040062B8 RID: 25272
			public PguiRawImageCtrl bonusIconImage;

			// Token: 0x040062B9 RID: 25273
			public PguiTextCtrl bonusText;

			// Token: 0x040062BA RID: 25274
			public IconCharaCtrl iconCharaCtrl;

			// Token: 0x040062BB RID: 25275
			public List<IconPhotoCtrl> iconPhotoList = new List<IconPhotoCtrl>();

			// Token: 0x040062BC RID: 25276
			public InfoPhotoItemEffectCtrl Info_PhotoItemEffect;

			// Token: 0x040062BD RID: 25277
			public PhotoPocketLevelCtrl Mark_PhotoPocketLevel;

			// Token: 0x040062BE RID: 25278
			public AchievementCtrl Achievement;

			// Token: 0x040062BF RID: 25279
			public GameObject NormalDecoration;

			// Token: 0x040062C0 RID: 25280
			public GameObject OwnDecoration;

			// Token: 0x040062C1 RID: 25281
			public PguiButtonCtrl HelperSetting;
		}

		// Token: 0x0200120B RID: 4619
		public class EnemyInfo
		{
			// Token: 0x0600579F RID: 22431 RVA: 0x00257CF8 File Offset: 0x00255EF8
			public EnemyInfo(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Attr_Circle = baseTr.GetComponent<Image>();
				this.Attr_CircleCtrl = baseTr.GetComponent<uGUITweenScale>();
				this.Attr_ColorCtrl = baseTr.Find("Icon").GetComponent<PguiColorCtrl>();
				this.Attr_Icon = baseTr.Find("Icon").GetComponent<Image>();
				this.baseScl = baseTr.localScale;
			}

			// Token: 0x060057A0 RID: 22432 RVA: 0x00257D68 File Offset: 0x00255F68
			public void Setup(bool flag)
			{
				this.baseObj.transform.localScale = this.baseScl;
				this.Attr_Circle.enabled = flag;
				this.Attr_CircleCtrl.enabled = flag;
				this.Attr_ColorCtrl.InitForce();
				this.Attr_Icon.color = this.Attr_ColorCtrl.GetGameObjectById(flag ? "ACTIVE" : "NORMAL");
			}

			// Token: 0x040062C2 RID: 25282
			public GameObject baseObj;

			// Token: 0x040062C3 RID: 25283
			public Image Attr_Circle;

			// Token: 0x040062C4 RID: 25284
			public uGUITweenScale Attr_CircleCtrl;

			// Token: 0x040062C5 RID: 25285
			public PguiColorCtrl Attr_ColorCtrl;

			// Token: 0x040062C6 RID: 25286
			public Image Attr_Icon;

			// Token: 0x040062C7 RID: 25287
			private Vector3 baseScl;

			// Token: 0x040062C8 RID: 25288
			public static List<CharaDef.AttributeMask> attributeMaskList = new List<CharaDef.AttributeMask>
			{
				CharaDef.AttributeMask.RED,
				CharaDef.AttributeMask.GREEN,
				CharaDef.AttributeMask.BLUE,
				CharaDef.AttributeMask.PINK,
				CharaDef.AttributeMask.LIME,
				CharaDef.AttributeMask.AQUA
			};
		}
	}
}
