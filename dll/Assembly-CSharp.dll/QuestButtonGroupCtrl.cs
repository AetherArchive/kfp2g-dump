using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.UI;

public class QuestButtonGroupCtrl : MonoBehaviour
{
	public void Init(QuestButtonGroupCtrl.QuestButtonCallback callback)
	{
		this.questButtonCallback = callback;
		this.parentRect = base.transform.parent.GetComponent<RectTransform>();
		int group_NUM_MAX = QuestButtonGroupCtrl.GROUP_NUM_MAX;
		int battle_NUM_MAX = QuestButtonGroupCtrl.BATTLE_NUM_MAX;
		for (int i = 0; i < group_NUM_MAX; i++)
		{
			QuestButtonGroupCtrl.GuiQuestGroup guiQuestGroup = new QuestButtonGroupCtrl.GuiQuestGroup(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneQuest/GUI/Prefab/Quest_ListBar_Chapter"), base.transform).transform);
			guiQuestGroup.groupRt.gameObject.SetActive(false);
			guiQuestGroup.button.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickQuestGroupButton), PguiButtonCtrl.SoundType.DEFAULT);
			guiQuestGroup.button.gameObject.SetActive(false);
			for (int j = 0; j < battle_NUM_MAX; j++)
			{
				GameObject gameObject = new GameObject("child" + j.ToString(), new Type[] { typeof(LayoutElement) });
				gameObject.transform.SetParent(guiQuestGroup.groupRt.transform, true);
				gameObject.transform.localScale = Vector3.one;
				QuestButtonGroupCtrl.GuiQuest guiQuest = new QuestButtonGroupCtrl.GuiQuest(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneQuest/GUI/Prefab/Quest_ListBar_Quest"), gameObject.transform).transform);
				guiQuest.buttonMain.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickQuestButton), PguiButtonCtrl.SoundType.DEFAULT);
				guiQuestGroup.questGui.Add(guiQuest);
			}
			this.questGroupList.Add(guiQuestGroup);
		}
	}

	private void OnClickQuestButton(PguiButtonCtrl ptbc)
	{
		int num = int.Parse(ptbc.name);
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(num);
		int num2 = DataManager.DmQuest.CalcRestPlayNumByQuestOneId(num);
		if (!ptbc.ActEnable)
		{
			string text = QuestUtil.WindowWord01;
			if (DataManager.DmEvent.isRaidByQuestOneId(num))
			{
				text = "今回の残りクリア回数が0になりました。";
			}
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			return;
		}
		if (num2 == 0 && questOnePackData.questDynamicOne.todayRecoveryNum < questOnePackData.questOne.RecoveryMaxNum)
		{
			CanvasManager.HdlSelQuestCountRecoveryWindowCtrl.Setup(num, delegate
			{
				this.Setup(new QuestButtonGroupCtrl.SetupParam
				{
					selectData = this.selectData,
					callback = this.updateChapterChara,
					callbackText = this.updateChapterCharaText,
					offsetPosY = this.setupParam.offsetPosY
				});
			});
			return;
		}
		QuestButtonGroupCtrl.QuestButtonCallback questButtonCallback = this.questButtonCallback;
		if (questButtonCallback == null)
		{
			return;
		}
		questButtonCallback(int.Parse(ptbc.name));
	}

	private int GetAttrSpriteId(CharaDef.AttributeMask attr)
	{
		int num = 0;
		if (attr == CharaDef.AttributeMask.RED)
		{
			num = 4;
		}
		else if (attr == CharaDef.AttributeMask.GREEN)
		{
			num = 2;
		}
		else if (attr == CharaDef.AttributeMask.BLUE)
		{
			num = 0;
		}
		else if (attr == CharaDef.AttributeMask.PINK)
		{
			num = 5;
		}
		else if (attr == CharaDef.AttributeMask.LIME)
		{
			num = 3;
		}
		else if (attr == CharaDef.AttributeMask.AQUA)
		{
			num = 1;
		}
		return num;
	}

	private IEnumerator UpdateGrid()
	{
		yield return new WaitForEndOfFrame();
		QuestStaticMap questStaticMap = DataManager.DmQuest.QuestStaticData.mapDataMap[this.selectData.mapId];
		for (int i = 0; i < this.questGroupList.Count; i++)
		{
			if (i < questStaticMap.questGroupList.Count)
			{
				this.questGroupList[i].textChapterNum.transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = false;
				this.questGroupList[i].textChapterNum.transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = true;
			}
		}
		yield break;
	}

	private bool TwoOrMoreQuestGroup { get; set; }

	public void Setup(QuestButtonGroupCtrl.SetupParam param)
	{
		this.setupParam = param;
		if (!this.setupParam.selectData.Equals(this.selectData))
		{
			this.currentButton = null;
		}
		this.selectData = new QuestUtil.SelectData(this.setupParam.selectData);
		this.updateChapterChara = this.setupParam.callback;
		this.updateChapterCharaText = this.setupParam.callbackText;
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(this.selectData.chapterId);
		if (playableMapIdList.Count > 0)
		{
			int num = playableMapIdList.Find((int item) => item == this.selectData.mapId);
			if (num <= 0)
			{
				this.selectData.mapId = playableMapIdList[playableMapIdList.Count - 1];
			}
			else
			{
				this.selectData.mapId = num;
			}
		}
		else
		{
			Verbose<PrjLog>.LogError(this.selectData, null);
			this.selectData.mapId = 0;
		}
		if (DataManager.DmQuest.QuestStaticData.mapDataMap.ContainsKey(this.selectData.mapId))
		{
			QuestStaticMap questStaticMap = DataManager.DmQuest.QuestStaticData.mapDataMap[this.selectData.mapId];
			int chapterId = questStaticMap.chapterId;
			QuestStaticChapter questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataMap[chapterId];
			List<QuestStaticQuestGroup> list = new List<QuestStaticQuestGroup>(questStaticMap.questGroupList);
			list.Sort((QuestStaticQuestGroup a, QuestStaticQuestGroup b) => a.dispPriority - b.dispPriority);
			this.TwoOrMoreQuestGroup = list.Count > 1;
			foreach (QuestButtonGroupCtrl.GuiQuestGroup guiQuestGroup in this.questGroupList)
			{
				guiQuestGroup.button.gameObject.SetActive(false);
				guiQuestGroup.imageYajiOpen.gameObject.SetActive(this.TwoOrMoreQuestGroup);
			}
			this.ResetQuestGroupButton();
			int num2 = 0;
			for (int i = 0; i < this.questGroupList.Count; i++)
			{
				if (i < questStaticMap.questGroupList.Count)
				{
					QuestStaticQuestGroup groupData = list[i];
					List<QuestStaticQuestOne> list2 = groupData.questOneList.FindAll((QuestStaticQuestOne item) => DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(item.questId));
					list2.Sort((QuestStaticQuestOne a, QuestStaticQuestOne b) => a.dispPriority - b.dispPriority);
					string text = PrjUtil.MakeMessage("");
					this.questGroupList[num2].button.name = groupData.questGroupId.ToString();
					this.questGroupList[num2].dispPriority = groupData.dispPriority;
					bool flag = true;
					int num3 = DataManager.DmQuest.CalcRestPlayNumByQuestGroupId(groupData.questGroupId);
					string text2 = "本日残り";
					if (DataManager.DmEvent.isRaidByMapId(groupData.mapId))
					{
						text2 = "今回残り";
					}
					this.questGroupList[num2].Num_Count.text = ((num3 >= 0) ? (PrjUtil.MakeMessage(text2) + num3.ToString() + PrjUtil.MakeMessage("回")) : "");
					this.questGroupList[num2].Campaign.gameObject.SetActive(false);
					bool flag2 = list2.Count > 0;
					if (this.selectData.questCategory == QuestStaticChapter.Category.CHARA)
					{
						int num4 = 4;
						if (list[num2].questOneList.Count > 0)
						{
							QuestStaticQuestOne questStaticQuestOne = list[num2].questOneList[list[num2].questOneList.Count - 1];
						}
						CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(groupData.charaId);
						bool flag3 = false;
						bool flag4 = true;
						if (userCharaData != null)
						{
							flag4 = groupData.targetCharaKizunaLevel == 0 || userCharaData.dynamicData.kizunaLevel >= groupData.targetCharaKizunaLevel;
						}
						if (num2 == 0)
						{
							if (userCharaData != null && userCharaData.dynamicData.artsLevel >= groupData.targetCharaArtsLevel && flag2)
							{
								flag3 = true;
							}
						}
						else if (flag4 && flag2)
						{
							flag3 = true;
						}
						flag = flag3;
						this.questGroupList[num2].button.gameObject.SetActive(true);
						this.questGroupList[num2].button.SetActEnable(flag3, !flag3, false);
						this.questGroupList[num2].markLockCtrl.SetActive(!flag3);
						string text3 = ((userCharaData == null && num2 == 2) ? "フレンズ加入 ＆ " : "");
						if (num2 == 0)
						{
							text = PrjUtil.MakeMessage("けものミラクルLv") + PrjUtil.MakeMessage(groupData.targetCharaArtsLevel.ToString()) + PrjUtil.MakeMessage(" ＆ ");
						}
						else
						{
							text = text3 + PrjUtil.MakeMessage("なかよしLv") + PrjUtil.MakeMessage((num4 - num2).ToString()) + PrjUtil.MakeMessage(" ＆ ");
						}
						text = text + PrjUtil.MakeMessage((num4 - num2 - 1).ToString()) + PrjUtil.MakeMessage("話クリアで解放");
					}
					else if (this.selectData.questCategory == QuestStaticChapter.Category.EVENT)
					{
						Func<int> GetRelQuestIdFunc = delegate
						{
							int num12 = 0;
							List<QuestStaticQuestOne> list7 = new List<QuestStaticQuestOne>(groupData.questOneList);
							list7.Sort((QuestStaticQuestOne a, QuestStaticQuestOne b) => a.questId - b.questId);
							if (DataManager.DmQuest.QuestStaticData.oneDataMap.ContainsKey(list7[0].relQuestId))
							{
								num12 = DataManager.DmQuest.QuestStaticData.oneDataMap[list7[0].relQuestId].questId;
							}
							return num12;
						};
						Func<bool> func = delegate
						{
							QuestDynamicQuestOne questDynamicQuestOne2 = null;
							int num13 = GetRelQuestIdFunc();
							if (num13 == 0)
							{
								return true;
							}
							if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(num13))
							{
								questDynamicQuestOne2 = DataManager.DmQuest.QuestDynamicData.oneDataMap[num13];
							}
							return questDynamicQuestOne2 != null && (questDynamicQuestOne2.status == QuestOneStatus.CLEAR || questDynamicQuestOne2.status == QuestOneStatus.COMPLETE);
						};
						if (list2.Count <= 0 && !this.IsEventCoopDifficult(groupData) && !func())
						{
							goto IL_15B9;
						}
						bool flag5 = groupData.startDatetime <= SceneQuest.TimeStampInScene;
						this.questGroupList[num2].button.gameObject.SetActive(true);
						this.questGroupList[num2].button.SetActEnable(flag5 && num3 != 0, !flag5 || num3 == 0, false);
						this.questGroupList[num2].markLockCtrl.SetActive(!flag5 || num3 == 0);
						if (!flag5)
						{
							text = groupData.startDatetime.ToString("M/d HH:mm 以降");
						}
						flag = flag5;
						DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.selectData.eventId);
						if (eventData != null)
						{
							if (eventData.eventCategory == DataManagerEvent.Category.Coop)
							{
								DataManagerEvent.CoopData lastCoopInfo = DataManager.DmEvent.LastCoopInfo;
								if (lastCoopInfo.MapInfoMap.ContainsKey(this.selectData.mapId))
								{
									DataManagerEvent.CoopData.MapInfo mapInfo = lastCoopInfo.MapInfoMap[this.selectData.mapId];
									if (groupData.QuestGroupCategory == QuestStaticQuestGroup.GroupCategory.CoopDifficult)
									{
										this.questGroupList[num2].button.gameObject.SetActive(mapInfo.IsHardQuestOpen);
										bool flag6 = func();
										this.questGroupList[num2].button.SetActEnable(flag6, !flag6, false);
										this.questGroupList[num2].markLockCtrl.SetActive(!flag6);
										if (!flag6)
										{
											QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(GetRelQuestIdFunc());
											if (questOnePackData != null)
											{
												text = questOnePackData.questChapter.chapterName + questOnePackData.questGroup.titleName + "クリアで解放";
											}
										}
										flag = flag6;
									}
									else if (groupData.QuestGroupCategory == QuestStaticQuestGroup.GroupCategory.CoopNormal)
									{
										this.questGroupList[num2].button.gameObject.SetActive(!mapInfo.IsHardQuestOpen);
									}
								}
							}
							else if (eventData.eventCategory == DataManagerEvent.Category.Scenario && groupData.HideUnreleasedBelt)
							{
								this.questGroupList[num2].button.gameObject.SetActive(flag5);
							}
						}
					}
					else if (this.selectData.questCategory == QuestStaticChapter.Category.GROW)
					{
						this.questGroupList[num2].button.gameObject.SetActive(true);
						this.questGroupList[num2].button.SetActEnable(num3 != 0, num3 == 0, false);
						this.questGroupList[num2].markLockCtrl.SetActive(num3 == 0);
						this.questGroupList[num2].Campaign.gameObject.SetActive(QuestUtil.CheckCampaignQuestGroup(groupData.questGroupId));
					}
					else
					{
						this.questGroupList[num2].button.gameObject.SetActive(true);
						this.questGroupList[num2].button.SetActEnable(num3 != 0, num3 == 0, false);
						this.questGroupList[num2].markLockCtrl.SetActive(num3 == 0);
					}
					this.questGroupList[num2].textChapterNum.transform.parent.gameObject.SetActive(true);
					this.questGroupList[num2].textChapterNum.text = questStaticChapter.chapterName + groupData.titleName;
					PguiGradientCtrl component = this.questGroupList[num2].textChapterNum.GetComponent<PguiGradientCtrl>();
					if (component != null)
					{
						foreach (Outline outline in this.questGroupList[num2].textChapterNum.GetComponents<Outline>())
						{
							if (this.selectData.questCategory == QuestStaticChapter.Category.SIDE_STORY)
							{
								outline.effectColor = component.GetOutlineById("Arai");
							}
							else
							{
								outline.effectColor = component.GetOutlineById(QuestUtil.IsHardMode(this.selectData) ? "Hard" : "Normal");
							}
						}
					}
					this.questGroupList[num2].textChapterName.text = "「" + groupData.storyName + "」";
					PguiGradientCtrl component2 = this.questGroupList[num2].textChapterName.GetComponent<PguiGradientCtrl>();
					if (component2 != null)
					{
						foreach (Outline outline2 in this.questGroupList[num2].textChapterName.GetComponents<Outline>())
						{
							if (this.selectData.questCategory == QuestStaticChapter.Category.SIDE_STORY)
							{
								outline2.effectColor = component2.GetOutlineById("Arai");
							}
							else
							{
								outline2.effectColor = component2.GetOutlineById(QuestUtil.IsHardMode(this.selectData) ? "Hard" : "Normal");
							}
						}
					}
					List<DataManagerQuest.QuestSealedCharaData> list3 = DataManager.DmQuest.QuestSealedCharaDatas.FindAll((DataManagerQuest.QuestSealedCharaData item) => item.target == groupData.questGroupId);
					this.questGroupList[num2].quesSealedInfo.gameObject.SetActive(groupData.limitGroupFlag && list3.Count != 0);
					this.questGroupList[num2].quesSealedInfo.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickQuestSealedInfoButton), PguiButtonCtrl.SoundType.DEFAULT);
					for (int k = 0; k < this.questGroupList[num2].IconAll.Count; k++)
					{
						this.questGroupList[num2].IconAll[k].gameObject.SetActive(false);
					}
					if (this.selectData.questCategory == QuestStaticChapter.Category.GROW && list2.Count > 0)
					{
						List<CharaDef.AttributeMask> list4 = new List<CharaDef.AttributeMask>();
						CharaDef.AttributeMask attrMask = list2[0].attrMask;
						if ((attrMask & CharaDef.AttributeMask.RED) == CharaDef.AttributeMask.RED)
						{
							list4.Add(CharaDef.AttributeMask.RED);
						}
						if ((attrMask & CharaDef.AttributeMask.GREEN) == CharaDef.AttributeMask.GREEN)
						{
							list4.Add(CharaDef.AttributeMask.GREEN);
						}
						if ((attrMask & CharaDef.AttributeMask.BLUE) == CharaDef.AttributeMask.BLUE)
						{
							list4.Add(CharaDef.AttributeMask.BLUE);
						}
						if ((attrMask & CharaDef.AttributeMask.PINK) == CharaDef.AttributeMask.PINK)
						{
							list4.Add(CharaDef.AttributeMask.PINK);
						}
						if ((attrMask & CharaDef.AttributeMask.LIME) == CharaDef.AttributeMask.LIME)
						{
							list4.Add(CharaDef.AttributeMask.LIME);
						}
						if ((attrMask & CharaDef.AttributeMask.AQUA) == CharaDef.AttributeMask.AQUA)
						{
							list4.Add(CharaDef.AttributeMask.AQUA);
						}
						for (int l = 0; l < list4.Count; l++)
						{
							PguiImageCtrl pguiImageCtrl = this.questGroupList[num2].IconAll[l];
							pguiImageCtrl.gameObject.SetActive(true);
							PguiReplaceSpriteCtrl component3 = pguiImageCtrl.GetComponent<PguiReplaceSpriteCtrl>();
							component3.InitForce();
							int attrSpriteId = this.GetAttrSpriteId(list4[l]);
							pguiImageCtrl.SetImageByName(component3.GetSpriteById(attrSpriteId).name);
						}
					}
					this.questGroupList[num2].Num_Time.gameObject.SetActive(groupData.dispLimitTime);
					this.questGroupList[num2].Num_Time.text = TimeManager.MakeTimeResidueText(TimeManager.Now, groupData.limitTime, false, true).ToString();
					this.questGroupList[num2].Num_Count.gameObject.SetActive(num3 >= 0);
					this.questGroupList[num2].markLockCtrl.SetText(text);
					this.questGroupList[num2].Mark_EventGrow.gameObject.SetActive(QuestUtil.EnableEventGrowthExpUpFromGroupQuestId(groupData.questGroupId));
					bool flag7 = false;
					int num5 = 0;
					for (int m = 0; m < this.questGroupList[num2].questGui.Count; m++)
					{
						if (m < list2.Count)
						{
							QuestStaticQuestOne questStaticQuestOne2 = list2[m];
							QuestDynamicQuestOne questDynamicQuestOne = null;
							if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questStaticQuestOne2.questId))
							{
								questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataMap[questStaticQuestOne2.questId];
							}
							QuestOneStatus questOneStatus = ((questDynamicQuestOne != null) ? questDynamicQuestOne.status : QuestOneStatus.NEW);
							BattleMissionPack battleMissionPack = DataManager.DmQuest.GetBattleMissionPack(questStaticQuestOne2.questId);
							this.questGroupList[num2].questGui[m].Mark_EventGrow.gameObject.SetActive(QuestUtil.EnableEventGrowthExpUpFromQuesOneId(questStaticQuestOne2.questId));
							this.questGroupList[num2].questGui[m].buttonMain.name = questStaticQuestOne2.questId.ToString();
							this.questGroupList[num2].questGui[m].goBaseObj.SetActive(true);
							this.questGroupList[num2].questGui[m].goMissionInfo.SetActive(false);
							for (int n = 0; n < this.questGroupList[num2].questGui[m].goMissionFlag.Length; n++)
							{
								bool flag8 = battleMissionPack.staticData.mission[n].type > BattleMissionType.INVALID;
								this.questGroupList[num2].questGui[m].goMissionFlag[n].gameObject.SetActive(flag8);
								this.questGroupList[num2].questGui[m].textmissionName[n].gameObject.SetActive(flag8);
								if (flag8)
								{
									bool flag9 = battleMissionPack.clearFlag[n];
									this.questGroupList[num2].questGui[m].goMissionFlag[n].SetImageByName(flag9 ? "questbar_missionmemori_act" : "questbar_missionmemori_nor");
									this.questGroupList[num2].questGui[m].textmissionName[n].text = PrjUtil.MakeMessage(battleMissionPack.staticData.mission[n].text);
								}
							}
							if (flag && questOneStatus == QuestOneStatus.NEW)
							{
								flag7 = true;
							}
							this.questGroupList[num2].questGui[m].imageOption.InitForce();
							this.questGroupList[num2].questGui[m].imageOption.gameObject.SetActive(questOneStatus == QuestOneStatus.NEW);
							if (questOneStatus == QuestOneStatus.NEW)
							{
								PguiAECtrl componentInChildren = this.questGroupList[num2].questGui[m].imageOption.GetComponentInChildren<PguiAECtrl>();
								if (componentInChildren != null)
								{
									componentInChildren.PauseAnimation(PguiAECtrl.AmimeType.MAX, 1f, null);
								}
							}
							int num6;
							if (this.selectData.questCategory == QuestStaticChapter.Category.SIDE_STORY)
							{
								num6 = 3;
							}
							else
							{
								num6 = (QuestUtil.IsHardMode(this.selectData) ? 2 : 1);
							}
							this.questGroupList[num2].questGui[m].BaseImage.Replace(num6);
							if (questOneStatus == QuestOneStatus.COMPLETE)
							{
								num5++;
							}
							for (int num7 = 0; num7 < this.questGroupList[num2].questGui[m].IconAll.Count; num7++)
							{
								this.questGroupList[num2].questGui[m].IconAll[num7].gameObject.SetActive(false);
							}
							if (this.selectData.questCategory == QuestStaticChapter.Category.GROW)
							{
								List<CharaDef.AttributeMask> list5 = new List<CharaDef.AttributeMask>();
								CharaDef.AttributeMask attrMask2 = questStaticQuestOne2.attrMask;
								if ((attrMask2 & CharaDef.AttributeMask.RED) == CharaDef.AttributeMask.RED)
								{
									list5.Add(CharaDef.AttributeMask.RED);
								}
								if ((attrMask2 & CharaDef.AttributeMask.GREEN) == CharaDef.AttributeMask.GREEN)
								{
									list5.Add(CharaDef.AttributeMask.GREEN);
								}
								if ((attrMask2 & CharaDef.AttributeMask.BLUE) == CharaDef.AttributeMask.BLUE)
								{
									list5.Add(CharaDef.AttributeMask.BLUE);
								}
								if ((attrMask2 & CharaDef.AttributeMask.PINK) == CharaDef.AttributeMask.PINK)
								{
									list5.Add(CharaDef.AttributeMask.PINK);
								}
								if ((attrMask2 & CharaDef.AttributeMask.LIME) == CharaDef.AttributeMask.LIME)
								{
									list5.Add(CharaDef.AttributeMask.LIME);
								}
								if ((attrMask2 & CharaDef.AttributeMask.AQUA) == CharaDef.AttributeMask.AQUA)
								{
									list5.Add(CharaDef.AttributeMask.AQUA);
								}
								for (int num8 = 0; num8 < list5.Count; num8++)
								{
									PguiImageCtrl pguiImageCtrl2 = this.questGroupList[num2].questGui[m].IconAll[num8];
									pguiImageCtrl2.gameObject.SetActive(true);
									PguiReplaceSpriteCtrl component4 = pguiImageCtrl2.GetComponent<PguiReplaceSpriteCtrl>();
									component4.InitForce();
									int attrSpriteId2 = this.GetAttrSpriteId(list5[num8]);
									pguiImageCtrl2.SetImageByName(component4.GetSpriteById(attrSpriteId2).name);
								}
							}
							int num9 = DataManager.DmQuest.CalcRestPlayNumByQuestOneId(questStaticQuestOne2.questId);
							ItemInput recoveryKeyItem = questStaticQuestOne2.RecoveryKeyItem;
							QuestOnePackData questOnePackData2 = DataManager.DmQuest.GetQuestOnePackData(questStaticQuestOne2.questId);
							bool flag10 = num9 == 0 && (recoveryKeyItem == null || questOnePackData2.questDynamicOne.todayRecoveryNum >= questStaticQuestOne2.RecoveryMaxNum);
							this.questGroupList[num2].questGui[m].questListBarCmnInfo.Setup(new QuestUtil.QuestListBarCmnInfo.SetupParam
							{
								qsqo = questStaticQuestOne2,
								selectData = this.selectData,
								questOneStatus = questOneStatus,
								restGroupNum = num3,
								enableChangeColor = true
							});
							this.questGroupList[num2].questGui[m].questRuleInfo.Setup(questStaticQuestOne2.ruleId);
							this.questGroupList[num2].questGui[m].buttonMain.ReloadChildObject();
							if (num9 < 0)
							{
								this.questGroupList[num2].questGui[m].buttonMain.SetActEnable(true, false, false);
							}
							else
							{
								this.questGroupList[num2].questGui[m].buttonMain.SetActEnable(!flag10, num9 == 0, false);
							}
							this.questGroupList[num2].questGui[m].questRuleInfo.buttonRuleInfo.gameObject.SetActive(questStaticQuestOne2.ruleId != 0);
						}
						else
						{
							this.questGroupList[num2].questGui[m].goBaseObj.SetActive(false);
						}
						this.questGroupList[num2].questGui[m].questListBarCmnInfo.SetActiveQuestInfo(true);
						this.questGroupList[num2].questGui[m].goMissionInfo.SetActive(true);
						this.questGroupList[num2].questGui[m].BaseImageSA.ExPlayAnimation(this.infoModeMain ? SimpleAnimation.ExPguiStatus.END : SimpleAnimation.ExPguiStatus.START, delegate
						{
						});
					}
					this.questGroupList[num2].Mark_New.gameObject.SetActive(flag7);
					if (this.selectData.questCategory == QuestStaticChapter.Category.SIDE_STORY)
					{
						this.questGroupList[num2].BaseImage.Replace(3);
					}
					else
					{
						this.questGroupList[num2].BaseImage.Replace(QuestUtil.IsHardMode(this.selectData) ? 2 : 1);
					}
					this.questGroupList[num2].Num_MissionComp.text = string.Format("{0}/{1}", num5, groupData.questOneList.Count);
					this.questGroupList[num2].Mark_Complete_Fnt.gameObject.SetActive(num5 == groupData.questOneList.Count);
					num2++;
				}
				IL_15B9:;
			}
			List<QuestButtonGroupCtrl.GuiQuestGroup> list6 = this.questGroupList.FindAll((QuestButtonGroupCtrl.GuiQuestGroup item) => item.button.ActEnable);
			if (list6 != null)
			{
				list6.Sort((QuestButtonGroupCtrl.GuiQuestGroup a, QuestButtonGroupCtrl.GuiQuestGroup b) => a.dispPriority - b.dispPriority);
				QuestButtonGroupCtrl.GuiQuestGroup guiQuestGroup2 = list6.Find((QuestButtonGroupCtrl.GuiQuestGroup item) => item == this.currentButton);
				if (guiQuestGroup2 == null)
				{
					QuestButtonGroupCtrl.GuiQuestGroup guiQuestGroup3 = list6.Find((QuestButtonGroupCtrl.GuiQuestGroup item) => item.button.gameObject.activeSelf);
					if (guiQuestGroup3 != null)
					{
						this.InternalQuestGroupButton(guiQuestGroup3.button, false);
					}
				}
				else
				{
					this.currentButton = null;
					this.InternalQuestGroupButton(guiQuestGroup2.button, false);
				}
				if (list.Count > 0 && DataManager.DmEvent.isRaidBonusMapId(this.selectData.mapId))
				{
					this.currentGroupId = list[0].questGroupId;
				}
			}
			else if (this.updateChapterChara != null && list.Count > 0)
			{
				this.currentGroupId = list[0].questGroupId;
			}
			(base.transform.parent as RectTransform).anchoredPosition = Vector2.zero;
			QuestButtonGroupCtrl.UpdateChapterChara updateChapterChara = this.updateChapterChara;
			if (updateChapterChara != null)
			{
				updateChapterChara(this.currentGroupId);
			}
			Singleton<SceneManager>.Instance.StartCoroutine(this.UpdateGrid());
			for (int num10 = 0; num10 < this.questGroupList.Count; num10++)
			{
				for (int num11 = 0; num11 < this.questGroupList[num10].questGui.Count; num11++)
				{
					this.CheckFinishedAnim(this.questGroupList[num10].questGui[num11].BaseImageSA);
				}
			}
			return;
		}
		foreach (QuestButtonGroupCtrl.GuiQuestGroup guiQuestGroup4 in this.questGroupList)
		{
			guiQuestGroup4.button.gameObject.SetActive(false);
			guiQuestGroup4.imageYajiOpen.gameObject.SetActive(this.TwoOrMoreQuestGroup);
		}
		QuestButtonGroupCtrl.UpdateChapterChara updateChapterChara2 = this.updateChapterChara;
		if (updateChapterChara2 == null)
		{
			return;
		}
		updateChapterChara2(0);
	}

	public void SwitchInfo()
	{
		this.infoModeMain = !this.infoModeMain;
		for (int i = 0; i < this.questGroupList.Count; i++)
		{
			for (int j = 0; j < this.questGroupList[i].questGui.Count; j++)
			{
				this.questGroupList[i].questGui[j].questListBarCmnInfo.SetActiveQuestInfo(true);
				this.questGroupList[i].questGui[j].goMissionInfo.SetActive(true);
				this.questGroupList[i].questGui[j].BaseImageSA.ExPlayAnimation(this.infoModeMain ? SimpleAnimation.ExPguiStatus.END : SimpleAnimation.ExPguiStatus.START, delegate
				{
				});
				PguiDataHolder component = this.questGroupList[i].questGui[j].BaseImageSA.GetComponent<PguiDataHolder>();
				if (component != null)
				{
					component.id = 1;
				}
			}
		}
	}

	private void InternalQuestGroupButton(PguiButtonCtrl ptbc, bool isClickButton)
	{
		QuestButtonGroupCtrl.GuiQuestGroup guiQuestGroup = this.questGroupList.Find((QuestButtonGroupCtrl.GuiQuestGroup item) => item.button == ptbc);
		if (!ptbc.ActEnable)
		{
			int num = this.questGroupList.FindIndex((QuestButtonGroupCtrl.GuiQuestGroup item) => item.button == ptbc);
			QuestStaticMap questStaticMap = DataManager.DmQuest.QuestStaticData.mapDataMap[this.selectData.mapId];
			int chapterId = questStaticMap.chapterId;
			QuestStaticChapter questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataMap[chapterId];
			List<QuestStaticQuestGroup> list = new List<QuestStaticQuestGroup>(questStaticMap.questGroupList);
			list.Sort((QuestStaticQuestGroup a, QuestStaticQuestGroup b) => a.dispPriority - b.dispPriority);
			if (this.selectData.questCategory == QuestStaticChapter.Category.CHARA)
			{
				QuestStaticQuestGroup questStaticQuestGroup = list[num];
				string[] array = new string[] { " ＆ " };
				string[] array2 = guiQuestGroup.markLockCtrl.GetText().Split(array, StringSplitOptions.None);
				string text = array2[array2.Length - 1].Replace("で解放", "");
				int num2 = num + 1;
				if (num2 >= list.Count)
				{
					num2 = list.Count - 1;
				}
				QuestStaticQuestGroup questStaticQuestGroup2 = list[num2];
				int num3 = 0;
				foreach (QuestStaticQuestOne questStaticQuestOne in questStaticQuestGroup2.questOneList)
				{
					if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questStaticQuestOne.questId))
					{
						QuestDynamicQuestOne questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataMap[questStaticQuestOne.questId];
						if (questDynamicQuestOne.status == QuestOneStatus.CLEAR || questDynamicQuestOne.status == QuestOneStatus.COMPLETE)
						{
							num3++;
						}
					}
				}
				int num4 = ((questStaticQuestGroup.targetCharaId == 0) ? questStaticQuestGroup.charaId : questStaticQuestGroup.targetCharaId);
				CharaPackData charaPackData = DataManager.DmChara.GetUserCharaData(num4);
				if (charaPackData == null)
				{
					charaPackData = CharaPackData.MakeInitial(num4);
				}
				List<CmnReleaseConditionWindowCtrl.SetupParam> list3;
				if (array2.Length != 2)
				{
					List<CmnReleaseConditionWindowCtrl.SetupParam> list2 = new List<CmnReleaseConditionWindowCtrl.SetupParam>();
					list2.Add(new CmnReleaseConditionWindowCtrl.SetupParam
					{
						text = array2[0],
						enableClear = false
					});
					list2.Add(new CmnReleaseConditionWindowCtrl.SetupParam
					{
						text = array2[1],
						enableClear = ((num == 0) ? (charaPackData.dynamicData.artsLevel >= questStaticQuestGroup.targetCharaArtsLevel) : (charaPackData.dynamicData.kizunaLevel >= questStaticQuestGroup.targetCharaKizunaLevel))
					});
					list3 = list2;
					list2.Add(new CmnReleaseConditionWindowCtrl.SetupParam
					{
						text = text,
						enableClear = (num3 >= questStaticQuestGroup2.questOneList.Count)
					});
				}
				else
				{
					List<CmnReleaseConditionWindowCtrl.SetupParam> list4 = new List<CmnReleaseConditionWindowCtrl.SetupParam>();
					list4.Add(new CmnReleaseConditionWindowCtrl.SetupParam
					{
						text = array2[0],
						enableClear = ((num == 0) ? (charaPackData.dynamicData.artsLevel >= questStaticQuestGroup.targetCharaArtsLevel) : (charaPackData.dynamicData.kizunaLevel >= questStaticQuestGroup.targetCharaKizunaLevel))
					});
					list3 = list4;
					list4.Add(new CmnReleaseConditionWindowCtrl.SetupParam
					{
						text = text,
						enableClear = (num3 >= questStaticQuestGroup2.questOneList.Count)
					});
				}
				List<CmnReleaseConditionWindowCtrl.SetupParam> list5 = list3;
				CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), list5);
				return;
			}
			if (this.IsEventCoopDifficult(list[num]))
			{
				CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
				{
					new CmnReleaseConditionWindowCtrl.SetupParam
					{
						text = guiQuestGroup.markLockCtrl.GetText(),
						enableClear = false
					}
				});
				return;
			}
			if (isClickButton)
			{
				int num5 = 0;
				QuestStaticQuestGroup questStaticQuestGroup3 = (int.TryParse(guiQuestGroup.button.name, out num5) ? DataManager.DmQuest.QuestStaticData.groupDataMap[num5] : list[num]);
				if (questStaticQuestGroup3.startDatetime > SceneQuest.TimeStampInScene)
				{
					CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
					{
						new CmnReleaseConditionWindowCtrl.SetupParam
						{
							text = questStaticQuestGroup3.startDatetime.ToString("M/d HH:mm 以降"),
							enableClear = (questStaticQuestGroup3.startDatetime <= SceneQuest.TimeStampInScene)
						}
					});
					return;
				}
				string text2 = ((!DataManager.DmEvent.isRaidByMapId(questStaticQuestGroup3.mapId)) ? QuestUtil.WindowWord01 : QuestUtil.WindowWord02);
				CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), text2, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				return;
			}
		}
		else
		{
			if (this.currentButton != null && this.TwoOrMoreQuestGroup)
			{
				this.currentButton.imageYajiOpen.SetImageByName("btn_yaji_nor");
				this.currentButton.childAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
				{
				});
				for (int i = 0; i < this.questGroupList.Count; i++)
				{
					for (int j = 0; j < this.questGroupList[i].questGui.Count; j++)
					{
						this.questGroupList[i].questGui[j].BaseSA.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
					}
				}
			}
			if (this.currentButton != guiQuestGroup)
			{
				guiQuestGroup.childAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
				{
				});
				for (int k = 0; k < this.questGroupList.Count; k++)
				{
					for (int l = 0; l < this.questGroupList[k].questGui.Count; l++)
					{
						this.questGroupList[k].questGui[l].BaseSA.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
						this.CheckFinishedAnim(this.questGroupList[k].questGui[l].BaseImageSA);
					}
				}
				this.currentButton = guiQuestGroup;
				this.currentButton.imageYajiOpen.SetImageByName("btn_yaji_act");
			}
			else if (this.TwoOrMoreQuestGroup)
			{
				this.currentButton = null;
			}
			this.currentGroupId = int.Parse(ptbc.name);
			QuestButtonGroupCtrl.UpdateChapterChara updateChapterChara = this.updateChapterChara;
			if (updateChapterChara == null)
			{
				return;
			}
			updateChapterChara(this.currentGroupId);
		}
	}

	private void CheckFinishedAnim(SimpleAnimation sa)
	{
		PguiDataHolder pdh = sa.GetComponent<PguiDataHolder>();
		if (pdh != null && (pdh.id == 1 || pdh.id == 0))
		{
			sa.ExInit();
			sa.ExPlayAnimation(this.infoModeMain ? SimpleAnimation.ExPguiStatus.END : SimpleAnimation.ExPguiStatus.START, delegate
			{
				pdh.id = 2;
			});
		}
	}

	private void OnClickQuestGroupButton(PguiButtonCtrl ptbc)
	{
		this.InternalQuestGroupButton(ptbc, true);
	}

	private void OnClickQuestSealedInfoButton(PguiButtonCtrl ptbc)
	{
		QuestUtil.OpenQuestSealedInfo(this.currentGroupId).MoveNext();
	}

	private void ResetQuestGroupButton()
	{
		foreach (QuestButtonGroupCtrl.GuiQuestGroup guiQuestGroup in this.questGroupList)
		{
			guiQuestGroup.imageYajiOpen.SetImageByName("btn_yaji_nor");
			guiQuestGroup.childAnim.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END);
		}
	}

	private bool IsEventCoopDifficult(QuestStaticQuestGroup groupData)
	{
		bool flag = false;
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.selectData.eventId);
		if (eventData != null && eventData.eventCategory == DataManagerEvent.Category.Coop)
		{
			DataManagerEvent.CoopData lastCoopInfo = DataManager.DmEvent.LastCoopInfo;
			if (lastCoopInfo.MapInfoMap.ContainsKey(this.selectData.mapId))
			{
				DataManagerEvent.CoopData.MapInfo mapInfo = lastCoopInfo.MapInfoMap[this.selectData.mapId];
				if (groupData.QuestGroupCategory == QuestStaticQuestGroup.GroupCategory.CoopDifficult)
				{
					flag = true;
				}
			}
		}
		return flag;
	}

	private void Update()
	{
		float num = 0f;
		for (int i = 0; i < this.questGroupList.Count; i++)
		{
			float num2 = Mathf.Abs((this.questGroupList[i].button.gameObject.activeSelf ? (this.questGroupList[i].groupRt.sizeDelta.y * this.questGroupList[i].groupRt.localScale.y) : 0f) - this.questGroupList[i].button.GetComponent<RectTransform>().anchoredPosition.y);
			if (num < num2)
			{
				num = num2;
			}
			if (i < this.questGroupList.Count - 1)
			{
				RectTransform component = this.questGroupList[i + 1].button.GetComponent<RectTransform>();
				component.anchoredPosition = new Vector2(component.anchoredPosition.x, this.questGroupList[i].button.gameObject.activeSelf ? (-100f - num2) : num2);
			}
			else
			{
				this.parentRect.sizeDelta = new Vector2(this.parentRect.sizeDelta.x, num + 150f + this.setupParam.offsetPosY);
			}
		}
	}

	public RectTransform GetButtonRectTransform()
	{
		return this.questGroupList[0].questGui[0].buttonMain.transform as RectTransform;
	}

	private QuestButtonGroupCtrl.QuestButtonCallback questButtonCallback;

	private QuestButtonGroupCtrl.UpdateChapterChara updateChapterChara;

	private QuestButtonGroupCtrl.UpdateChapterChara updateChapterCharaText;

	private static readonly int BATTLE_NUM_MAX = 20;

	private static readonly int GROUP_NUM_MAX = 10;

	public bool isDebug;

	private List<QuestButtonGroupCtrl.GuiQuestGroup> questGroupList = new List<QuestButtonGroupCtrl.GuiQuestGroup>();

	private RectTransform parentRect;

	private QuestUtil.SelectData selectData;

	private int currentGroupId;

	private QuestButtonGroupCtrl.SetupParam setupParam = new QuestButtonGroupCtrl.SetupParam();

	private bool infoModeMain = true;

	private QuestButtonGroupCtrl.GuiQuestGroup currentButton;

	public delegate void QuestButtonCallback(int questId);

	public delegate void UpdateChapterChara(int groupQuestId);

	private class GuiQuestGroup
	{
		public GuiQuestGroup(Transform baseTr)
		{
			this.button = baseTr.GetComponent<PguiButtonCtrl>();
			this.childAnim = baseTr.Find("All/ChildGroup").GetComponent<SimpleAnimation>();
			this.groupRt = baseTr.Find("All/ChildGroup/ChildPanel").GetComponent<RectTransform>();
			this.textChapterName = baseTr.Find("All/BaseImage/Txt_TitleGrid/Txt_ChapterName").GetComponent<PguiTextCtrl>();
			this.textChapterNum = baseTr.Find("All/BaseImage/Txt_TitleGrid/Num_Chapter").GetComponent<PguiTextCtrl>();
			this.imageYajiOpen = baseTr.Find("All/BaseImage/Btn_Yaji_Open/BaseImage").GetComponent<PguiImageCtrl>();
			this.imageYajiOpen.SetImageByName("All/btn_yaji_nor");
			this.Num_Time = baseTr.Find("All/BaseImage/Num_Time").GetComponent<PguiTextCtrl>();
			this.Num_Count = baseTr.Find("All/BaseImage/Num_Count").GetComponent<PguiTextCtrl>();
			this.Mark_New = baseTr.Find("All/BaseImage/Mark_New").gameObject;
			this.BaseImage = baseTr.Find("All/BaseImage").GetComponent<PguiReplaceSpriteCtrl>();
			for (int i = 0; i < 6; i++)
			{
				this.IconAll.Add(baseTr.Find("All/BaseImage/IconAll/Icon" + (i + 1).ToString("D2")).GetComponent<PguiImageCtrl>());
			}
			Resources.Load("SceneQuest/GUI/Prefab/Quest_Memori_MissionComp");
			this.markLockCtrl = baseTr.Find("All/Mark_Lock").GetComponent<MarkLockCtrl>();
			this.Campaign = baseTr.Find("All/BaseImage/Campaign").GetComponent<PguiImageCtrl>();
			this.Mark_EventGrow = baseTr.Find("All/BaseImage/Mark_EventGrow").GetComponent<PguiImageCtrl>();
			this.Mark_EventGrow.gameObject.SetActive(false);
			Transform transform = baseTr.Find("All/BaseImage/MissionInfo");
			if (transform != null)
			{
				this.Mark_Complete_Fnt = transform.Find("Mark_Complete_Fnt").GetComponent<PguiImageCtrl>();
				this.Num_MissionComp = transform.Find("Mark_Mission/Num_MissionComp").GetComponent<PguiTextCtrl>();
			}
			this.quesSealedInfo = baseTr.Find("All/Btn_SealedInfo").GetComponent<PguiButtonCtrl>();
		}

		public PguiButtonCtrl button;

		public SimpleAnimation childAnim;

		public RectTransform groupRt;

		public PguiTextCtrl textChapterName;

		public PguiTextCtrl textChapterNum;

		public PguiImageCtrl imageYajiOpen;

		public PguiTextCtrl Num_Time;

		public PguiTextCtrl Num_Count;

		public GameObject Mark_New;

		public List<PguiImageCtrl> IconAll = new List<PguiImageCtrl>();

		public List<QuestButtonGroupCtrl.GuiQuest> questGui = new List<QuestButtonGroupCtrl.GuiQuest>();

		public PguiReplaceSpriteCtrl BaseImage;

		public MarkLockCtrl markLockCtrl;

		public PguiImageCtrl Campaign;

		public PguiImageCtrl Mark_EventGrow;

		public PguiImageCtrl Mark_Complete_Fnt;

		public PguiTextCtrl Num_MissionComp;

		public PguiButtonCtrl quesSealedInfo;

		public int dispPriority;
	}

	public class GuiQuest
	{
		public GuiQuest(Transform baseTr)
		{
			this.goBaseObj = baseTr.parent.gameObject;
			this.BaseImage = baseTr.Find("Btn/BaseImage").GetComponent<PguiReplaceSpriteCtrl>();
			this.buttonMain = baseTr.Find("Btn").GetComponent<PguiButtonCtrl>();
			this.imageOption = baseTr.Find("Btn/BaseImage/Mark_New").GetComponent<PguiNestPrefab>();
			this.textmissionName = new PguiTextCtrl[]
			{
				baseTr.Find("Btn/BaseImage/MissionInfo/Memori_Mission01/Txt_Mission01").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Btn/BaseImage/MissionInfo/Memori_Mission02/Txt_Mission02").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Btn/BaseImage/MissionInfo/Memori_Mission03/Txt_Mission03").GetComponent<PguiTextCtrl>()
			};
			this.goMissionFlag = new PguiImageCtrl[]
			{
				baseTr.Find("Btn/BaseImage/MissionInfo/Memori_Mission01").GetComponent<PguiImageCtrl>(),
				baseTr.Find("Btn/BaseImage/MissionInfo/Memori_Mission02").GetComponent<PguiImageCtrl>(),
				baseTr.Find("Btn/BaseImage/MissionInfo/Memori_Mission03").GetComponent<PguiImageCtrl>()
			};
			this.goMissionInfo = baseTr.Find("Btn/BaseImage/MissionInfo").gameObject;
			this.BaseImageSA = baseTr.Find("Btn/BaseImage").GetComponent<SimpleAnimation>();
			this.BaseSA = baseTr.GetComponent<SimpleAnimation>();
			for (int i = 0; i < 6; i++)
			{
				this.IconAll.Add(baseTr.Find("Btn/BaseImage/IconAll/Icon" + (i + 1).ToString("D2")).GetComponent<PguiImageCtrl>());
			}
			this.Mark_EventGrow = baseTr.Find("Btn/BaseImage/Mark_EventGrow").GetComponent<PguiImageCtrl>();
			this.Mark_EventGrow.gameObject.SetActive(false);
			this.questListBarCmnInfo = new QuestUtil.QuestListBarCmnInfo(this.BaseImage.transform.Find("QuestListBar_CmnInfo"));
			this.questRuleInfo = new QuestUtil.QuestRuleInfo(baseTr.Find("Btn/BaseImage/Btn_Question").GetComponent<PguiButtonCtrl>());
		}

		public GameObject goBaseObj;

		public PguiButtonCtrl buttonMain;

		public PguiTextCtrl[] textmissionName;

		public PguiImageCtrl[] goMissionFlag;

		public GameObject goMissionInfo;

		public PguiNestPrefab imageOption;

		public List<PguiImageCtrl> IconAll = new List<PguiImageCtrl>();

		public SimpleAnimation BaseImageSA;

		public PguiReplaceSpriteCtrl BaseImage;

		public SimpleAnimation BaseSA;

		public PguiImageCtrl Mark_EventGrow;

		public QuestUtil.QuestListBarCmnInfo questListBarCmnInfo;

		public QuestUtil.QuestRuleInfo questRuleInfo;
	}

	public class SetupParam
	{
		public QuestUtil.SelectData selectData;

		public QuestButtonGroupCtrl.UpdateChapterChara callback;

		public QuestButtonGroupCtrl.UpdateChapterChara callbackText;

		public float offsetPosY;
	}
}
