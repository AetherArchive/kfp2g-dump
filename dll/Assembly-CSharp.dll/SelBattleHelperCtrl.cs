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

public class SelBattleHelperCtrl : MonoBehaviour
{
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

	public bool IsPlayingAnim()
	{
		return this.guiData.FriendSelectAnim.ExIsPlaying();
	}

	public void PlayStartAnim(bool start = true)
	{
		this.guiData.FriendSelectAnim.ExPlayAnimation(start ? SimpleAnimation.ExPguiStatus.START : SimpleAnimation.ExPguiStatus.END, null);
	}

	public void UpdateCampaign(QuestOnePackData questData = null)
	{
		this.guiData.ResetCampaignInfoCategory();
		if (questData == null)
		{
			questData = DataManager.DmQuest.GetQuestOnePackData(this.selectQuestOneId);
		}
		this.guiData.UpdateCampaignInfoCategory(questData.questChapter.category, questData.questChapter.chapterId);
	}

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

	private void SwitchCurrentTab(int index)
	{
		this.currentAttrIndex = index;
		this.guiData.ResizeScrollView(this.helperList[this.currentAttrIndex].Count);
	}

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

	private void OnStartItemFriend(int index, GameObject go)
	{
		go.GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonFriendSelect), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.friendChipList.Add(new SelBattleHelperCtrl.GUI.FriendChip(go.transform));
		go.AddComponent<PguiDataHolder>();
	}

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

	private void OnStartItemDropItem(int index, GameObject go)
	{
		for (int i = 0; i < SelBattleHelperCtrl.GUI.ITEM_INFO_DROP_H; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, go.transform);
			gameObject.name = i.ToString();
			gameObject.GetComponent<IconItemCtrl>();
		}
	}

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

	private void OnClickButtonFriendSelect(PguiButtonCtrl buttuon)
	{
		HelperPackData helperPackData = this.helperList[this.currentAttrIndex].Find((HelperPackData item) => item.friendId == buttuon.GetComponent<PguiDataHolder>().id);
		DataManager.DmHelper.UpdateLatestHelper(helperPackData, this.currentAttrIndex);
		this.callback(helperPackData, this.currentAttrIndex);
	}

	private void OnClickButtonHelperChange(PguiButtonCtrl button)
	{
		if (!button.ActEnable)
		{
			return;
		}
		Singleton<SceneManager>.Instance.StartCoroutine(this.OnClickButtonHelperChangeCoroutine());
	}

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

	private bool OnSelectTab(int index)
	{
		this.SwitchCurrentTab(index);
		return true;
	}

	public void OnClickHelperSetting(PguiButtonCtrl button)
	{
		this.onClickHelperSetting(SceneManager.SceneName.SceneProfile);
	}

	private SelBattleHelperCtrl.GUI guiData;

	private List<List<HelperPackData>> helperList;

	private SelBattleHelperCtrl.OnFriendSelect callback;

	private bool friendSelectInfoFlag;

	private int currentAttrIndex;

	private List<SelBattleHelperCtrl.DispItem> dispItemIdList = new List<SelBattleHelperCtrl.DispItem>();

	private bool isTutorial;

	private int selectQuestOneId;

	private CharaDef.AttributeMask enemyAttribute;

	private readonly float HELPER_CHANEGE_BTN_COOL_TIME = 1f;

	private float helperButtonGage;

	private bool isOnlyNPC;

	public UnityAction<SceneManager.SceneName> onClickHelperSetting;

	public delegate void OnFriendSelect(HelperPackData helper, int attrIndex);

	private class DispItem
	{
		public int id;

		public bool gentei;

		public List<int> photo;
	}

	public class GUI
	{
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

		public void UpdateCampaignInfoCategory(QuestStaticChapter.Category category, int chapterId)
		{
			List<string> list = new List<string>(QuestUtil.GetCampaignMessageList(category, chapterId));
			List<string> list2 = new List<string>(QuestUtil.GetCampaignTimeList(category, chapterId));
			this.campaignInfo.DispCampaign(list, list2);
		}

		public void ResetCampaignInfoCategory()
		{
			this.campaignInfo.ResetCampaign();
		}

		public void ResizeScrollView(int count)
		{
			this.Txt_None.SetActive(count <= 0);
			this.ScrollView1.Resize(count, 0);
		}

		public static readonly int ITEM_INFO_DROP_H = 3;

		public GameObject baseObj;

		public PguiButtonCtrl Btn_InfoSwitch;

		public GameObject AtrInfo;

		public PguiImageCtrl Mark_Night;

		public PguiTextCtrl Txt_ChapterName;

		public PguiTextCtrl Num_Quest;

		public PguiTextCtrl Txt_QuestName;

		public PguiTextCtrl Num_Stamina;

		public GameObject Stamina1;

		public GameObject Stamina2;

		public PguiTextCtrl Num_Difficulty;

		public PguiTextCtrl Txt_TerrainInfo;

		public PguiTextCtrl Num_Wave;

		public PguiTextCtrl Txt_ButtonInfo;

		public SimpleAnimation FriendSelectAnim;

		public ReuseScroll ScrollView1;

		public ReuseScroll ScrollView2;

		public PguiTabGroupCtrl TabGroup;

		public List<PguiTabCtrl> Btn_Filter_List;

		public PguiTabCtrl Btn_Filter_all;

		public PguiTabCtrl Btn_Filter_funny;

		public PguiTabCtrl Btn_Filter_friendly;

		public PguiTabCtrl Btn_Filter_relax;

		public PguiTabCtrl Btn_Filter_lovely;

		public PguiTabCtrl Btn_Filter_active;

		public PguiTabCtrl Btn_Filter_mypace;

		public GameObject questInfoObj;

		public GameObject itemInfoObj;

		public List<SelBattleHelperCtrl.GUI.FriendChip> friendChipList = new List<SelBattleHelperCtrl.GUI.FriendChip>();

		public List<SelBattleHelperCtrl.GUI.EnemyInfo> enemyInfoList = new List<SelBattleHelperCtrl.GUI.EnemyInfo>();

		public QuestUtil.CampaignInfo campaignInfo;

		public PguiButtonCtrl Btn_HelperChange;

		public PguiImageCtrl Btn_HelperChange_Gage;

		public PguiImageCtrl Btn_HelperChange_Circle;

		public PguiImageCtrl Mark_Hard;

		public PguiImageCtrl Mark_NotContinue;

		public PguiImageCtrl Mark_NotDhole;

		public PguiImageCtrl Mark_NotLeader;

		public GameObject Txt_None;

		public class FriendChip
		{
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

			public GameObject baseObj;

			public PguiButtonCtrl Quest_ListBar_Friend;

			public PguiReplaceSpriteCtrl Mark_Friend;

			public PguiTextCtrl Txt_FriendName;

			public PguiTextCtrl Num_Rank;

			public PguiTextCtrl Num_FriendPont;

			public PguiTextCtrl Txt_LastLogin;

			public PguiImageCtrl Mark_NPC;

			public PguiRawImageCtrl bonusIconImage;

			public PguiTextCtrl bonusText;

			public IconCharaCtrl iconCharaCtrl;

			public List<IconPhotoCtrl> iconPhotoList = new List<IconPhotoCtrl>();

			public InfoPhotoItemEffectCtrl Info_PhotoItemEffect;

			public PhotoPocketLevelCtrl Mark_PhotoPocketLevel;

			public AchievementCtrl Achievement;

			public GameObject NormalDecoration;

			public GameObject OwnDecoration;

			public PguiButtonCtrl HelperSetting;
		}

		public class EnemyInfo
		{
			public EnemyInfo(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Attr_Circle = baseTr.GetComponent<Image>();
				this.Attr_CircleCtrl = baseTr.GetComponent<uGUITweenScale>();
				this.Attr_ColorCtrl = baseTr.Find("Icon").GetComponent<PguiColorCtrl>();
				this.Attr_Icon = baseTr.Find("Icon").GetComponent<Image>();
				this.baseScl = baseTr.localScale;
			}

			public void Setup(bool flag)
			{
				this.baseObj.transform.localScale = this.baseScl;
				this.Attr_Circle.enabled = flag;
				this.Attr_CircleCtrl.enabled = flag;
				this.Attr_ColorCtrl.InitForce();
				this.Attr_Icon.color = this.Attr_ColorCtrl.GetGameObjectById(flag ? "ACTIVE" : "NORMAL");
			}

			public GameObject baseObj;

			public Image Attr_Circle;

			public uGUITweenScale Attr_CircleCtrl;

			public PguiColorCtrl Attr_ColorCtrl;

			public Image Attr_Icon;

			private Vector3 baseScl;

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
