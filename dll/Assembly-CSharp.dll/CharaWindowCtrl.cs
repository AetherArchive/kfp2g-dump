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
using UnityStandardAssets.ImageEffects;

public class CharaWindowCtrl : MonoBehaviour
{
	public void Init()
	{
		if (this.guiData != null)
		{
			return;
		}
		this.guiData = new CharaWindowCtrl.GUI(base.transform);
		this.guiData.Btn_Close.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Yaji_Left.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Yaji_Right.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Grow.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_View.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Scenario.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_DressUp.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.TabGroup.Setup(0, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectDetailTab));
		this.OnSelectDetailTab(0);
		ReuseScroll info_Status_ScrollView = this.guiData.Info_Status_ScrollView;
		info_Status_ScrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(info_Status_ScrollView.onStartItem, new Action<int, GameObject>(this.OnStartInfoStatus));
		ReuseScroll info_Status_ScrollView2 = this.guiData.Info_Status_ScrollView;
		info_Status_ScrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(info_Status_ScrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateInfoStatus));
		this.guiData.Info_Status_ScrollView.Setup(3, 0);
		this.guiData.Info_Status_Btn_Guide_L.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Info_Status_Btn_Guide_R.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		FixedScrollRect fixedScrollRect = this.guiData.Info_Status_ScrollView.RefScrollRect as FixedScrollRect;
		if (fixedScrollRect != null)
		{
			fixedScrollRect.onScrollStopped = delegate(int index)
			{
				this.BonusCharaDataListIndex = index;
			};
		}
		this.guiData.All.transform.localScale = new Vector3(0f, 0f, 0f);
		this.guiData.All.GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.guiData.All.SetActive(false);
		this.renderTextureChara = null;
	}

	private bool OnSelectDetailTab(int index)
	{
		this.guiData.infoPicture.SetActive(index == 3);
		this.guiData.infoProfile.SetActive(index == 2);
		this.guiData.infoSkill.SetActive(index == 1);
		this.guiData.infoStatus.SetActive(index == 0);
		return true;
	}

	private void OnClickButton(PguiButtonCtrl pbc)
	{
		if (!this.FinishedOpen())
		{
			return;
		}
		if (CanvasManager.HdlDressUpWipeCtrl.IsActive())
		{
			return;
		}
		Func<bool, SceneManager.SceneName, object> func = delegate(bool isQuest, SceneManager.SceneName name)
		{
			SceneManager.SceneName currentSceneName = Singleton<SceneManager>.Instance.CurrentSceneName;
			if (currentSceneName <= SceneManager.SceneName.SceneQuest)
			{
				if (currentSceneName == SceneManager.SceneName.SceneBattleSelector)
				{
					return new SceneBattleSelector.Args
					{
						detailCharaId = this.currentCharaPackData.id,
						recordCameSceneName = name,
						menuBackSceneArgs = Singleton<SceneManager>.Instance.CurrentScene
					};
				}
				if (currentSceneName == SceneManager.SceneName.SceneQuest)
				{
					return new SceneQuest.Args
					{
						category = QuestStaticChapter.Category.CHARA,
						selectCharaId = this.currentCharaPackData.id
					};
				}
			}
			else
			{
				if (currentSceneName == SceneManager.SceneName.SceneCharaEdit)
				{
					SceneCharaEdit sceneCharaEdit = (SceneCharaEdit)Singleton<SceneManager>.Instance.CurrentScene;
					return new SceneCharaEdit.Args
					{
						growCharaId = ((this.openParam.UIPreset == CharaWindowCtrl.DetailParamSetting.Preset.MINE_EASY_NO_GROW) ? this.currentCharaPackData.id : 0),
						detailCharaId = ((this.openParam.UIPreset != CharaWindowCtrl.DetailParamSetting.Preset.MINE_EASY_NO_GROW) ? this.currentCharaPackData.id : 0),
						openDetailWindow = isQuest,
						menuBackRequestMode = sceneCharaEdit.getCurrentMode()
					};
				}
				if (currentSceneName == SceneManager.SceneName.ScenePvpDeck)
				{
					return new ScenePvpDeck.Args
					{
						pvpSeasonId = this.openParam.pvpSeasonId,
						openCharaWindow = true
					};
				}
				if (currentSceneName == SceneManager.SceneName.SceneTraining)
				{
					return new SceneTraining.Args
					{
						deck = true,
						openCharaWindow = true
					};
				}
			}
			return null;
		};
		if (pbc == this.guiData.Btn_Yaji_Left || pbc == this.guiData.Btn_Yaji_Right)
		{
			if (this.renderTextureChara != null && !this.renderTextureChara.FinishedSetup)
			{
				return;
			}
			if (this.dispCharaPackList == null || !this.dispCharaPackList.Contains(this.currentCharaPackData))
			{
				return;
			}
			this.CharaChangeIcon();
			int num = this.dispCharaPackList.IndexOf(this.currentCharaPackData);
			num += ((pbc == this.guiData.Btn_Yaji_Left) ? (-1) : 1);
			num = (num + this.dispCharaPackList.Count) % this.dispCharaPackList.Count;
			this.SetupButton(this.dispCharaPackList[num]);
			this.ChangeChara(this.dispCharaPackList[num]);
			this.guiData.markLockCtrl.StartAE();
			this.guiData.All.GetComponent<SimpleAnimation>().ExPlayAnimation("CHANGE", null);
			return;
		}
		else if (pbc == this.guiData.Btn_Close)
		{
			this.Close();
			CharaWindowCtrl.OnClick onClick = this.callback;
			if (onClick == null)
			{
				return;
			}
			onClick();
			return;
		}
		else
		{
			if (!(pbc == this.guiData.Btn_Grow))
			{
				if (pbc == this.guiData.Btn_View)
				{
					this.Close();
					this.openParam.openCB = null;
					this.openParam.openPrevCB = null;
					SceneHome.Args args = new SceneHome.Args
					{
						charaPackData = this.currentCharaPackData,
						sceneName = ((Singleton<SceneManager>.Instance != null) ? Singleton<SceneManager>.Instance.CurrentSceneName : SceneManager.SceneName.None),
						menuBackSceneArgs = func(true, (Singleton<SceneManager>.Instance != null) ? Singleton<SceneManager>.Instance.CurrentSceneName : SceneManager.SceneName.None)
					};
					if (Singleton<SceneManager>.Instance != null)
					{
						Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneHome, args);
						return;
					}
				}
				else
				{
					if (pbc == this.guiData.Btn_Scenario)
					{
						if (this.guiData.markLockCtrl.IsActive())
						{
							QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.CHARA));
							if (questOnePackData != null)
							{
								CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
								{
									new CmnReleaseConditionWindowCtrl.SetupParam
									{
										text = string.Concat(new string[]
										{
											SceneQuest.GetMainStoryName(questOnePackData.questChapter.category, false),
											" ",
											questOnePackData.questChapter.chapterName,
											questOnePackData.questGroup.titleName,
											PrjUtil.MakeMessage("クリア")
										}),
										enableClear = false
									}
								});
								return;
							}
							return;
						}
						else
						{
							this.Close();
							this.openParam.openCB = null;
							this.openParam.openPrevCB = null;
							int num2 = DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.CHARA).Find((int item) => DataManager.DmQuest.QuestStaticData.mapDataMap[item].questCharaId == this.currentCharaPackData.id);
							if (num2 <= 0)
							{
								return;
							}
							using (List<QuestStaticQuestGroup>.Enumerator enumerator = DataManager.DmQuest.QuestStaticData.mapDataMap[num2].questGroupList.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									QuestStaticQuestGroup questStaticQuestGroup = enumerator.Current;
									using (List<QuestStaticQuestOne>.Enumerator enumerator2 = questStaticQuestGroup.questOneList.GetEnumerator())
									{
										if (enumerator2.MoveNext())
										{
											QuestStaticQuestOne questStaticQuestOne = enumerator2.Current;
											SceneQuest.Args args2 = new SceneQuest.Args
											{
												selectQuestOneId = questStaticQuestOne.questId,
												jumpQuest = true,
												menuBackSceneName = Singleton<SceneManager>.Instance.CurrentSceneName,
												menuBackSceneArgs = func(true, SceneManager.SceneName.SceneQuest)
											};
											CanvasManager.HdlCmnMenu.MoveSceneByMenu(SceneManager.SceneName.SceneQuest, args2);
											break;
										}
									}
								}
								return;
							}
						}
					}
					if (pbc == this.guiData.Btn_DressUp)
					{
						CanvasManager.HdlDressUpWipeCtrl.Play(delegate
						{
							this.Close();
							DressUpWindowCtrl.OpenParameter.Preset preset = ((this.openParam.UIPreset == CharaWindowCtrl.DetailParamSetting.Preset.MINE_EASY_NO_GROW) ? DressUpWindowCtrl.OpenParameter.Preset.MINE_EASY_NO_GROW : DressUpWindowCtrl.OpenParameter.Preset.DEFAULT);
							if (!this.openParam.DispCameraBtn)
							{
								preset = DressUpWindowCtrl.OpenParameter.Preset.NO_VIEW;
							}
							CanvasManager.HdlDressUpWindowCtrl.Open(this.currentCharaPackData, new DressUpWindowCtrl.OpenParameter(preset, this.openParam.cpdList));
						});
						return;
					}
					if (pbc == this.guiData.Info_Status_Btn_Guide_L || pbc == this.guiData.Info_Status_Btn_Guide_R)
					{
						int num3 = 1;
						int num4 = -1;
						this.BonusCharaDataListIndex += ((pbc == this.guiData.Info_Status_Btn_Guide_L) ? num3 : num4);
						this.BonusCharaDataListIndex = (this.BonusCharaDataListIndex + this.GetScrollCount()) % this.GetScrollCount();
						this.guiData.Info_Status_ScrollView.ForceFocus(this.BonusCharaDataListIndex);
					}
				}
				return;
			}
			this.Close();
			if (Singleton<SceneManager>.Instance.CurrentSceneName != SceneManager.SceneName.SceneCharaEdit || this.openParam.UIPreset != CharaWindowCtrl.DetailParamSetting.Preset.MINE_EASY_NO_GROW)
			{
				this.openParam.openCB = null;
				this.openParam.openPrevCB = null;
				SceneCharaEdit.Args args3 = new SceneCharaEdit.Args
				{
					growCharaId = this.currentCharaPackData.id,
					growTab = 0,
					menuBackSceneArgs = func(true, SceneManager.SceneName.SceneCharaEdit),
					menuBackSceneName = Singleton<SceneManager>.Instance.CurrentSceneName
				};
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneCharaEdit, args3);
				return;
			}
			CharaWindowCtrl.OnClick onClick2 = this.callback;
			if (onClick2 == null)
			{
				return;
			}
			onClick2();
			return;
		}
	}

	private void UpdateInfoStatusNumPage(Transform tr, int page)
	{
		if (tr != null)
		{
			tr.GetComponent<PguiTextCtrl>().text = (this.bonusCharaData.isSpecial ? string.Format("{0}/{1}", page % 2 + 1, this.GetScrollCount() / 2) : string.Format("{0}/{1}", page + 1, this.GetScrollCount()));
		}
	}

	private void OnStartInfoStatus(int index, GameObject go)
	{
		for (int i = 0; i < 1; i++)
		{
			this.guiData.guiInfoStatusMap[go.name] = new CharaWindowCtrl.GUIInfoStatus(go.transform);
		}
	}

	private void OnUpdateInfoStatus(int index, GameObject go)
	{
		bool flag = this.maxDispChangeChara;
		bool flag2 = this.shopDispChangeChara;
		int num = int.Parse(go.name);
		if (num < this.GetScrollCount() && this.guiData.guiInfoStatusMap.ContainsKey(go.name))
		{
			DataManagerChara.BonusCharaData bonusCharaData = this.bonusCharaData.list[num];
			CharaWindowCtrl.GUIInfoStatus guiinfoStatus = this.guiData.guiInfoStatusMap[go.name];
			this.guiData.Txt_GachaInfo.gameObject.SetActive(flag || flag2);
			if (flag)
			{
				this.guiData.Txt_GachaInfo.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
				{
					"※表示されているステータスは\n\u3000「Lv.」「なかよしLv.」「野生解放」「けものミラクル」が強化された状態です\n\u3000（入手時はLv.1および未強化の状態のものとなります)",
					(this.bonusCharaData.list.Count > 1) ? "※ステータスアップ効果は対象のイベントでのみ有効です" : ""
				});
			}
			else if (flag2)
			{
				this.guiData.Txt_GachaInfo.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[] { "※表示されているステータスはしょうたい後のステータスになります\n※既にしょうたい済みのフレンズの場合は各強化段階を比較して高いほうで上書きされます\n※フレンズ強化以外に起因する上乗せステータスは上書きの対象に含みません", "" });
			}
			PrjUtil.ParamPreset paramPreset = (flag ? PrjUtil.CalcShowWindowParamByChara(this.currentCharaPackData.staticData, new List<DataManagerChara.BonusCharaData> { bonusCharaData }) : PrjUtil.CalcParamByChara(this.currentCharaPackData.dynamicData, null, new List<DataManagerChara.BonusCharaData> { bonusCharaData }, null));
			float num2 = (float)paramPreset.avoid / 10f;
			List<string> list = new List<string>
			{
				paramPreset.totalParam.ToString(),
				flag ? string.Format("{0}/{1}", this.currentCharaPackData.staticData.maxPromoteNum, this.currentCharaPackData.staticData.maxPromoteNum) : string.Format("{0}/{1}", this.currentCharaPackData.dynamicData.promoteNum, this.currentCharaPackData.staticData.maxPromoteNum),
				paramPreset.hp.ToString(),
				paramPreset.atk.ToString(),
				paramPreset.def.ToString(),
				num2.ToString("F1") + "％",
				this.currentCharaPackData.staticData.baseData.plasmPoint.ToString()
			};
			for (int i = 0; i < guiinfoStatus.StatusTextList.Count; i++)
			{
				guiinfoStatus.StatusTextList[i].text = list[i];
			}
			guiinfoStatus.StatusTextList[0].m_Text.color = guiinfoStatus.StatusTextList[0].GetComponent<PguiColorCtrl>().GetGameObjectById(this.IsNormalCharaParamExcludeDropRatio(num) ? "NORMAL" : "EVENT");
			guiinfoStatus.StatusTextList[2].m_Text.color = guiinfoStatus.StatusTextList[2].GetComponent<PguiColorCtrl>().GetGameObjectById((bonusCharaData.hpBonusRatio == 0) ? "NORMAL" : "EVENT");
			guiinfoStatus.StatusTextList[3].m_Text.color = guiinfoStatus.StatusTextList[3].GetComponent<PguiColorCtrl>().GetGameObjectById((bonusCharaData.strBonusRatio == 0) ? "NORMAL" : "EVENT");
			guiinfoStatus.StatusTextList[4].m_Text.color = guiinfoStatus.StatusTextList[4].GetComponent<PguiColorCtrl>().GetGameObjectById((bonusCharaData.defBonusRatio == 0) ? "NORMAL" : "EVENT");
			int num3 = CharaPackData.CalcLimitLevel(this.currentCharaPackData.staticData.GetId(), this.currentCharaPackData.staticData.baseData.rankLow, 0);
			int num4 = ((this.currentCharaPackData.dynamicData != null) ? (this.currentCharaPackData.staticData.baseData.maxKizunaLevel + this.currentCharaPackData.dynamicData.kizunaLimitOverNum) : this.currentCharaPackData.staticData.baseData.maxKizunaLevel);
			long num5 = ((this.currentCharaPackData.dynamicData.level >= this.currentCharaPackData.dynamicData.limitLevel) ? 0L : (DataManager.DmChara.GetExpByNextLevel(this.currentCharaPackData.id, this.currentCharaPackData.dynamicData.level) - this.currentCharaPackData.dynamicData.exp));
			long num6 = ((this.currentCharaPackData.dynamicData.kizunaLevel >= this.currentCharaPackData.dynamicData.KizunaLimitLevel) ? 0L : (DataManager.DmChara.GetKizunaExpForNextLevel(this.currentCharaPackData.id, this.currentCharaPackData.dynamicData.kizunaLevel) - this.currentCharaPackData.dynamicData.kizunaExp));
			guiinfoStatus.StatusTextLv.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				flag ? num3.ToString() : this.currentCharaPackData.dynamicData.level.ToString(),
				flag ? num3.ToString() : this.currentCharaPackData.dynamicData.limitLevel.ToString()
			});
			guiinfoStatus.StatusTextLvExp.ReplaceTextByDefault("Param01", num5.ToString());
			guiinfoStatus.LvGage.m_Image.fillAmount = ((this.currentCharaPackData.dynamicData.level >= this.currentCharaPackData.dynamicData.limitLevel || flag) ? 1f : ((float)this.currentCharaPackData.dynamicData.exp / (float)DataManager.DmChara.GetExpByNextLevel(this.currentCharaPackData.dynamicData.id, this.currentCharaPackData.dynamicData.level)));
			guiinfoStatus.StatusTextKizunaLv.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				flag ? string.Format("{0}", this.currentCharaPackData.staticData.baseData.maxKizunaLevel) : string.Format("{0}", this.currentCharaPackData.dynamicData.kizunaLevel),
				flag ? string.Format("{0}", this.currentCharaPackData.staticData.baseData.maxKizunaLevel) : string.Format("{0}", num4)
			});
			guiinfoStatus.StatusTextKizunaLvExp.ReplaceTextByDefault("Param01", num6.ToString());
			guiinfoStatus.KizunaLvGage.m_Image.fillAmount = ((this.currentCharaPackData.dynamicData.kizunaLevel >= num4 || flag) ? 1f : ((float)this.currentCharaPackData.dynamicData.kizunaExp / (float)DataManager.DmChara.GetKizunaExpForNextLevel(this.currentCharaPackData.dynamicData.id, this.currentCharaPackData.dynamicData.kizunaLevel)));
			guiinfoStatus.StatusTextLvExp.transform.parent.gameObject.SetActive(!flag && num5 != 0L);
			guiinfoStatus.StatusTextKizunaLvExp.transform.parent.gameObject.SetActive(!flag && num6 != 0L);
			for (int j = 0; j < this.currentCharaPackData.staticData.orderCardList.Count; j++)
			{
				guiinfoStatus.guiOrderCardList[j].Setup(this.currentCharaPackData.staticData.orderCardList[j], this.currentCharaPackData.staticData.baseData);
			}
			float num7 = (float)paramPreset.actionDamageRatio / 10f;
			float num8 = (float)paramPreset.tryDamageRatio / 10f;
			float num9 = (float)paramPreset.beatDamageRatio / 10f;
			guiinfoStatus.Info_Flag_Action.text = PrjUtil.MakeMessage("＋") + num7.ToString("F1") + PrjUtil.MakeMessage("％");
			guiinfoStatus.Info_Flag_Try.text = PrjUtil.MakeMessage("＋") + num8.ToString("F1") + PrjUtil.MakeMessage("％");
			guiinfoStatus.Info_Flag_Beat.text = PrjUtil.MakeMessage("＋") + num9.ToString("F1") + PrjUtil.MakeMessage("％");
			bool flag3 = this.GetScrollCount() > 1;
			guiinfoStatus.Info_StatusKind_Event.SetActive(flag3 && !this.IsNormalChara(num));
			this.UpdateInfoStatusNumPage(guiinfoStatus.Info_StatusKind_Event.transform.Find("Num_Page"), num);
			guiinfoStatus.Info_StatusKind_Normal.SetActive(flag3 && this.IsNormalChara(num));
			this.UpdateInfoStatusNumPage(guiinfoStatus.Info_StatusKind_Normal.transform.Find("Num_Page"), num);
			if (guiinfoStatus.Info_StatusKind_Event.activeSelf)
			{
				DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(bonusCharaData.eventId);
				string text = "";
				string text2 = "ステータスアップ";
				if (bonusCharaData.eventId == 0)
				{
					text = TimeManager.MakeTimeResidueText(TimeManager.Now, bonusCharaData.endDatetime, true, true) + " まで";
				}
				else if (eventData != null)
				{
					bool flag4 = bonusCharaData.hpBonusRatio == 0 && bonusCharaData.strBonusRatio == 0 && bonusCharaData.defBonusRatio == 0 && bonusCharaData.kizunaBonusRatio == 0;
					text = eventData.eventName;
					if (flag4 && (bonusCharaData.increaseItemId01 != 0 || bonusCharaData.increaseItemId02 != 0))
					{
						text2 = "獲得量アップ";
					}
				}
				guiinfoStatus.Info_StatusKind_Event.transform.Find("Txt_Event").GetComponent<PguiTextCtrl>().ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[] { text2, text });
			}
			DataManagerKemoBoard.KemoBoardBonusParam kemoBoardBonusParam = ((this.openParam.DispKemoBordInfo && !flag) ? DataManager.DmKemoBoard.KemoBoardBonusParamMap[this.currentCharaPackData.staticData.baseData.attribute] : new DataManagerKemoBoard.KemoBoardBonusParam(this.currentCharaPackData.staticData.baseData.attribute));
			PrjUtil.ParamPreset paramPreset2 = ((this.openParam.DispKemoBordInfo && !flag) ? DataManager.DmChara.GetActiveKizunaBuff() : new PrjUtil.ParamPreset());
			PguiTextCtrl pguiTextCtrl = guiinfoStatus.TxtBoardPlusList[0];
			int num10 = kemoBoardBonusParam.KemoStatus + paramPreset2.totalParam;
			pguiTextCtrl.gameObject.SetActive(num10 != 0);
			pguiTextCtrl.text = string.Format("+{0}", num10);
			PguiTextCtrl pguiTextCtrl2 = guiinfoStatus.TxtBonusRatioList[0];
			int kizunaBonusRatio = bonusCharaData.kizunaBonusRatio;
			pguiTextCtrl2.gameObject.SetActive(kizunaBonusRatio != 0);
			pguiTextCtrl2.text = "x" + (((float)kizunaBonusRatio + CharaWindowCtrl.r1000) / CharaWindowCtrl.r1000).ToString("##.##");
			PguiTextCtrl pguiTextCtrl3 = guiinfoStatus.TxtBonusRatioList[1];
			int hpBonusRatio = bonusCharaData.hpBonusRatio;
			pguiTextCtrl3.gameObject.SetActive(hpBonusRatio != 0);
			pguiTextCtrl3.text = "x" + (((float)hpBonusRatio + CharaWindowCtrl.r1000) / CharaWindowCtrl.r1000).ToString("##.##");
			PguiTextCtrl pguiTextCtrl4 = guiinfoStatus.TxtBoardPlusList[1];
			int num11 = kemoBoardBonusParam.Hp + paramPreset2.hp;
			pguiTextCtrl4.gameObject.SetActive(num11 != 0);
			pguiTextCtrl4.text = string.Format("+{0}", num11);
			PguiTextCtrl pguiTextCtrl5 = guiinfoStatus.TxtBonusRatioList[2];
			int strBonusRatio = bonusCharaData.strBonusRatio;
			pguiTextCtrl5.gameObject.SetActive(strBonusRatio != 0);
			pguiTextCtrl5.text = "x" + (((float)strBonusRatio + CharaWindowCtrl.r1000) / CharaWindowCtrl.r1000).ToString("##.##");
			PguiTextCtrl pguiTextCtrl6 = guiinfoStatus.TxtBoardPlusList[2];
			int num12 = kemoBoardBonusParam.Attack + paramPreset2.atk;
			pguiTextCtrl6.gameObject.SetActive(num12 != 0);
			pguiTextCtrl6.text = string.Format("+{0}", num12);
			PguiTextCtrl pguiTextCtrl7 = guiinfoStatus.TxtBonusRatioList[3];
			int defBonusRatio = bonusCharaData.defBonusRatio;
			pguiTextCtrl7.gameObject.SetActive(defBonusRatio != 0);
			pguiTextCtrl7.text = "x" + (((float)defBonusRatio + CharaWindowCtrl.r1000) / CharaWindowCtrl.r1000).ToString("##.##");
			PguiTextCtrl pguiTextCtrl8 = guiinfoStatus.TxtBoardPlusList[3];
			int num13 = kemoBoardBonusParam.Difence + paramPreset2.def;
			pguiTextCtrl8.gameObject.SetActive(num13 != 0);
			pguiTextCtrl8.text = string.Format("+{0}", num13);
			PguiTextCtrl pguiTextCtrl9 = guiinfoStatus.TxtBoardPlusList[4];
			int num14 = kemoBoardBonusParam.Avoid + paramPreset2.avoid;
			pguiTextCtrl9.gameObject.SetActive(num14 != 0);
			string text3 = ((float)num14 / 10f).ToString("F1");
			pguiTextCtrl9.text = "+" + text3 + "%";
			PguiTextCtrl pguiTextCtrl10 = guiinfoStatus.TxtBoardPlusList[5];
			int num15 = kemoBoardBonusParam.BeatDamage + paramPreset2.beatDamageRatio;
			pguiTextCtrl10.gameObject.SetActive(num15 != 0);
			string text4 = ((float)num15 / 10f).ToString("F1");
			pguiTextCtrl10.text = "+" + text4 + "%";
			PguiTextCtrl pguiTextCtrl11 = guiinfoStatus.TxtBoardPlusList[6];
			int num16 = kemoBoardBonusParam.TryDamage + paramPreset2.tryDamageRatio;
			pguiTextCtrl11.gameObject.SetActive(num16 != 0);
			string text5 = ((float)num16 / 10f).ToString("F1");
			pguiTextCtrl11.text = "+" + text5 + "%";
			PguiTextCtrl pguiTextCtrl12 = guiinfoStatus.TxtBoardPlusList[7];
			int num17 = kemoBoardBonusParam.ActionDamage + paramPreset2.actionDamageRatio;
			pguiTextCtrl12.gameObject.SetActive(num17 != 0);
			string text6 = ((float)num17 / 10f).ToString("F1");
			pguiTextCtrl12.text = "+" + text6 + "%";
			List<DataManagerChara.BonusCharaData> list2;
			if (bonusCharaData.increaseItemId01 == 0 && bonusCharaData.increaseItemId02 == 0)
			{
				list2 = new List<DataManagerChara.BonusCharaData>();
			}
			else
			{
				(list2 = new List<DataManagerChara.BonusCharaData>()).Add(bonusCharaData);
			}
			List<DataManagerChara.BonusCharaData> list3 = list2;
			List<int> list4 = new List<int>();
			list4.Add(this.currentCharaPackData.dynamicData.PhotoPocket.FindAll((CharaDynamicData.PPParam itm) => itm.Flag).Count<CharaDynamicData.PPParam>());
			List<int> list5 = list4;
			guiinfoStatus.InfoPhotoItemEffectCtrl.Setup(DataManager.DmChara.CalcDropBonus(new List<DataManagerPhoto.CalcDropBonusResult>(), list3, list5));
			return;
		}
		go.SetActive(false);
	}

	private void OnStartSkill(int index, GameObject go)
	{
	}

	private void UpdateSkill(int index, GameObject go, bool notOwnedChara)
	{
		if (this.currentCharaPackData != null)
		{
			bool flag = new List<CharaPackData>(DataManager.DmChara.GetUserCharaMap().Values).Contains(this.currentCharaPackData);
			bool maxDisp = (!flag && notOwnedChara) || this.openParam.DispMaxInfo;
			go.SetActive(true);
			if (index < Enum.GetValues(typeof(CharaUtil.GUISkillInfo.Type)).Length)
			{
				CharaUtil.GUISkillInfo guiskillInfo = this.guiData.guiSkillInfoList[index];
				guiskillInfo.Setup(new CharaUtil.GUISkillInfo.SetupParam
				{
					charaPackData = this.currentCharaPackData,
					maxDisp = maxDisp,
					type = (CharaUtil.GUISkillInfo.Type)index
				});
				if (index == 0)
				{
					int num = guiskillInfo.Txt_Info.text.Count<char>((char s) => s == '\n') + 1;
					if (num > CharaWindowCtrl.SKILL_UI_DEFAULT_LINE_NUM)
					{
						guiskillInfo.baseObj.GetComponent<RectTransform>().sizeDelta = new Vector2(guiskillInfo.baseObj.GetComponent<RectTransform>().sizeDelta.x, CharaWindowCtrl.KEMONO_MIRACLE_DEFAULT_Y + CharaWindowCtrl.SKILL_UI_TEXT_SIZE * (float)(num - CharaWindowCtrl.SKILL_UI_DEFAULT_LINE_NUM));
						return;
					}
					guiskillInfo.baseObj.GetComponent<RectTransform>().sizeDelta = new Vector2(guiskillInfo.baseObj.GetComponent<RectTransform>().sizeDelta.x, CharaWindowCtrl.KEMONO_MIRACLE_DEFAULT_Y);
					return;
				}
				else if (index == 1)
				{
					int num = guiskillInfo.Txt_Info.text.Count<char>((char s) => s == '\n') + 1;
					if (num > CharaWindowCtrl.SKILL_UI_DEFAULT_LINE_NUM)
					{
						guiskillInfo.baseObj.GetComponent<RectTransform>().sizeDelta = new Vector2(guiskillInfo.baseObj.GetComponent<RectTransform>().sizeDelta.x, CharaWindowCtrl.SPECIALY_ATTACK_DEFAULT_Y + CharaWindowCtrl.SKILL_UI_TEXT_SIZE * (float)(num - CharaWindowCtrl.SKILL_UI_DEFAULT_LINE_NUM));
						return;
					}
					guiskillInfo.baseObj.GetComponent<RectTransform>().sizeDelta = new Vector2(guiskillInfo.baseObj.GetComponent<RectTransform>().sizeDelta.x, CharaWindowCtrl.SPECIALY_ATTACK_DEFAULT_Y);
					return;
				}
				else if (index != 2)
				{
					if (index == 3)
					{
						if (guiskillInfo.Disable != null)
						{
							guiskillInfo.markLockCtrl.Setup(new MarkLockCtrl.SetupParam
							{
								updateConditionCallback = () => (maxDisp ? this.currentCharaPackData.staticData.maxPromoteNum : this.currentCharaPackData.dynamicData.promoteNum) > 0,
								releaseFlag = true,
								tagetObject = guiskillInfo.Disable,
								text = CharaWindowCtrl.WILD_RELEASE_MARK_LOCK_STR
							}, true);
							if (guiskillInfo.Disable.activeSelf)
							{
								PrjUtil.AddTouchEventTrigger(guiskillInfo.Disable.transform.GetComponent<RectTransform>(), delegate(Transform trans)
								{
									CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("「とくせい」解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
									{
										new CmnReleaseConditionWindowCtrl.SetupParam
										{
											text = CharaWindowCtrl.WILD_RELEASE_MARK_LOCK_STR,
											enableClear = false
										}
									});
								});
								return;
							}
							PrjUtil.RemoveTouchEventTrigger(guiskillInfo.Disable);
							return;
						}
					}
					else if (index == 4 && this.currentCharaPackData.IsHaveSpAbility)
					{
						if (guiskillInfo.Disable != null)
						{
							string str2 = "フォトポケ" + this.currentCharaPackData.staticData.baseData.spAbilityRelPp.ToString() + "段階達成";
							guiskillInfo.markLockCtrl.Setup(new MarkLockCtrl.SetupParam
							{
								updateConditionCallback = () => this.currentCharaPackData.IsEnableSpAbility,
								releaseFlag = true,
								tagetObject = guiskillInfo.Disable,
								text = str2
							}, true);
							if (guiskillInfo.Disable.activeSelf)
							{
								PrjUtil.AddTouchEventTrigger(guiskillInfo.Disable.transform.GetComponent<RectTransform>(), delegate(Transform trans)
								{
									CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("「キセキとくせい」解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
									{
										new CmnReleaseConditionWindowCtrl.SetupParam
										{
											text = str2,
											enableClear = false
										}
									});
								});
								return;
							}
							PrjUtil.RemoveTouchEventTrigger(guiskillInfo.Disable);
							return;
						}
					}
					else
					{
						if (index != 5 || !this.currentCharaPackData.IsHaveNanairoAbility)
						{
							go.SetActive(false);
							return;
						}
						if (guiskillInfo.Disable != null)
						{
							string str = "フレンズ成長画面で解放";
							guiskillInfo.markLockCtrl.Setup(new MarkLockCtrl.SetupParam
							{
								updateConditionCallback = () => this.currentCharaPackData.IsEnableNanairoAbility,
								releaseFlag = true,
								tagetObject = guiskillInfo.Disable,
								text = str
							}, true);
							if (guiskillInfo.Disable.activeSelf)
							{
								PrjUtil.AddTouchEventTrigger(guiskillInfo.Disable.transform.GetComponent<RectTransform>(), delegate(Transform trans)
								{
									CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("「なないろとくせい」解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
									{
										new CmnReleaseConditionWindowCtrl.SetupParam
										{
											text = str,
											enableClear = false
										}
									});
								});
								return;
							}
							PrjUtil.RemoveTouchEventTrigger(guiskillInfo.Disable);
							return;
						}
					}
				}
			}
			else
			{
				go.SetActive(false);
			}
		}
	}

	private void Update()
	{
		if (Singleton<SceneManager>.Instance.GetCanvasCamera(SceneManager.CanvasType.SYSTEM).GetComponent<Blur>().enabled && this.guiData.Null_BlurSpread != null)
		{
			Singleton<SceneManager>.Instance.GetCanvasCamera(SceneManager.CanvasType.SYSTEM).GetComponent<Blur>().blurSpread = this.guiData.Null_BlurSpread.transform.localScale.x;
		}
		if (this.iEOpen != null && !this.iEOpen.MoveNext())
		{
			this.iEOpen = null;
			this.guiData.markLockCtrl.StartAE();
		}
	}

	private int GetScrollCount()
	{
		return this.bonusCharaData.list.Count;
	}

	private bool IsNormalChara(int index)
	{
		return index >= this.GetScrollCount() || (this.IsNormalCharaParam(index) && this.bonusCharaData.list[index].kizunaBonusRatio == 0);
	}

	private bool IsNormalCharaParam(int index)
	{
		return index >= this.GetScrollCount() || (this.bonusCharaData.list[index].hpBonusRatio == 0 && this.bonusCharaData.list[index].strBonusRatio == 0 && this.bonusCharaData.list[index].defBonusRatio == 0 && this.bonusCharaData.list[index].increaseItemId01 == 0 && this.bonusCharaData.list[index].increaseItemId02 == 0);
	}

	private bool IsNormalCharaParamExcludeDropRatio(int index)
	{
		return index >= this.GetScrollCount() || (this.bonusCharaData.list[index].hpBonusRatio == 0 && this.bonusCharaData.list[index].strBonusRatio == 0 && this.bonusCharaData.list[index].defBonusRatio == 0);
	}

	private void ChangeChara(CharaPackData cpd)
	{
		this.currentCharaPackData = cpd;
		this.currenEquipClothId = cpd.dynamicData.equipClothesId;
		bool flag = cpd.dynamicData.OwnerType == CharaDynamicData.CharaOwnerType.SHOP;
		bool flag2 = cpd.dynamicData == null || cpd.dynamicData.OwnerType == CharaDynamicData.CharaOwnerType.Undefined;
		bool flag3 = flag2 || this.openParam.DispMaxInfo;
		this.maxDispChangeChara = flag3;
		this.shopDispChangeChara = flag;
		int num = this.openParam.selectEventId;
		if (num <= 0)
		{
			num = ((((this.openParam.selectQuestOneId > 0) ? DataManager.DmQuest.GetQuestOnePackData(this.openParam.selectQuestOneId) : null) == null) ? (-1) : QuestUtil.GetEventId(this.openParam.selectQuestOneId, false));
		}
		if (this.openParam.pvpSeasonId > 0)
		{
			this.bonusCharaData.list = ((num < 0) ? new List<DataManagerChara.BonusCharaData>() : new List<DataManagerChara.BonusCharaData>(DataManager.DmChara.GetBonusCharaDataList(num)).FindAll((DataManagerChara.BonusCharaData itm) => itm.charaId == this.currentCharaPackData.id));
		}
		else
		{
			this.bonusCharaData.list = ((num < 0) ? new List<DataManagerChara.BonusCharaData>(DataManager.DmChara.GetBonusCharaDataList()) : new List<DataManagerChara.BonusCharaData>(DataManager.DmChara.GetBonusCharaDataList(num)).FindAll((DataManagerChara.BonusCharaData itm) => itm.charaId == this.currentCharaPackData.id));
		}
		this.bonusCharaData.isSpecial = false;
		this.bonusCharaData.list.RemoveAll((DataManagerChara.BonusCharaData item) => item.charaId != this.currentCharaPackData.id);
		if ((this.openParam.deckCategory == UserDeckData.Category.PVP && this.openParam.selectEventId <= 0) || this.openParam.deckCategory == UserDeckData.Category.TRAINING)
		{
			this.bonusCharaData.list.Clear();
		}
		this.BonusCharaDataListIndex = 0;
		if (num < 0)
		{
			this.bonusCharaData.list.Add(new DataManagerChara.BonusCharaData(new MstEventBonusCharaData
			{
				charaId = this.currentCharaPackData.id
			}));
		}
		else if (this.bonusCharaData.list.Count == 0)
		{
			this.bonusCharaData.list.Add(new DataManagerChara.BonusCharaData(new MstEventBonusCharaData
			{
				charaId = this.currentCharaPackData.id
			}));
		}
		if (this.bonusCharaData.list.Count == 2)
		{
			this.bonusCharaData.list.Add(this.bonusCharaData.list[0]);
			this.bonusCharaData.list.Add(new DataManagerChara.BonusCharaData(new MstEventBonusCharaData
			{
				charaId = this.currentCharaPackData.id
			}));
			this.bonusCharaData.isSpecial = true;
		}
		else
		{
			this.bonusCharaData.list.Sort((DataManagerChara.BonusCharaData a, DataManagerChara.BonusCharaData b) => b.eventId - a.eventId);
		}
		this.userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.CHARA));
		string text = "";
		if (questOnePackData != null)
		{
			string mainStoryName = SceneQuest.GetMainStoryName(questOnePackData.questChapter.category, true);
			text = mainStoryName + ((mainStoryName != "") ? "\n" : "") + questOnePackData.questChapter.chapterName + questOnePackData.questGroup.titleName + PrjUtil.MakeMessage("クリア");
		}
		MarkLockCtrl markLockCtrl = this.guiData.markLockCtrl;
		MarkLockCtrl.SetupParam setupParam = new MarkLockCtrl.SetupParam();
		setupParam.updateConditionCallback = () => QuestUtil.ClearConditionGrayOutButton(QuestStaticChapter.Category.CHARA);
		setupParam.releaseFlag = this.userFlagData.ReleaseModeFlag.FriendsStory == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
		setupParam.tagetObject = this.guiData.Btn_Scenario.gameObject;
		setupParam.text = text;
		setupParam.updateUserFlagDataCallback = delegate
		{
			this.userFlagData.ReleaseModeFlag.FriendsStory = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
			DataManager.DmGameStatus.RequestActionUpdateUserFlag(this.userFlagData);
		};
		markLockCtrl.Setup(setupParam, false);
		foreach (CharaWindowCtrl.GUI.Name name in this.guiData.names)
		{
			name.baseObj.SetActive(false);
		}
		CharaWindowCtrl.GUI.Name name2 = this.guiData.names[0];
		if (!string.IsNullOrEmpty(this.currentCharaPackData.staticData.baseData.NickName))
		{
			name2 = this.guiData.names[1];
		}
		name2.Setup(this.currentCharaPackData, flag3);
		if (flag)
		{
			name2.ShopInfo.SetActive(true);
		}
		else
		{
			name2.ShopInfo.SetActive(false);
		}
		this.guiData.Info_Status_ScrollView.Clear();
		this.guiData.Info_Status_ScrollView.ReuseItemNum = this.GetScrollCount();
		this.guiData.Info_Status_ScrollView.Setup(this.GetScrollCount(), 0);
		bool flag4 = this.GetScrollCount() > 1;
		this.guiData.Info_Status_ScrollView.GetComponent<FixedScrollRect>().enabled = flag4;
		this.guiData.Info_Status_Btn_Guide_L.gameObject.SetActive(flag4);
		this.guiData.Info_Status_Btn_Guide_R.gameObject.SetActive(flag4);
		this.guiData.Info_Status_ScrollView.Resize(this.GetScrollCount(), 0);
		this.guiData.Info_Status_ScrollView.ForceFocus(this.BonusCharaDataListIndex);
		for (int m = 0; m < this.guiData.guiSkillInfoList.Count; m++)
		{
			this.UpdateSkill(m, this.guiData.guiSkillInfoList[m].baseObj, flag2);
		}
		List<string> list = new List<string>
		{
			this.currentCharaPackData.staticData.baseData.flavorText,
			this.currentCharaPackData.staticData.baseData.castName
		};
		for (int j = 0; j < this.guiData.ProfileTextList.Count; j++)
		{
			this.guiData.ProfileTextList[j].text = list[j];
		}
		int selectFaceIconId = this.currentCharaPackData.dynamicData.selectFaceIconId;
		this.selectIconIndex = ((selectFaceIconId > 1) ? (selectFaceIconId - 1) : 0);
		int idx;
		int idx2;
		for (idx = 0; idx < this.guiData.guiIconCharaList.Count; idx = idx2 + 1)
		{
			CharaWindowCtrl.GUIIconChara guiiconChara = this.guiData.guiIconCharaList[idx];
			guiiconChara.Icon_Chara.SetupPrm(new IconCharaCtrl.SetupParam
			{
				cpd = this.currentCharaPackData,
				sortType = SortFilterDefine.SortType.INVALID,
				iconId = idx + 1,
				isEnableMask = (idx != 0 && this.currentCharaPackData.dynamicData.promoteNum < idx + 1)
			});
			guiiconChara.Icon_Chara.DispRarity(false);
			guiiconChara.Icon_Chara.DispAttribute(false);
			guiiconChara.Icon_Chara.DispAttributeMark((CharaDef.AttributeMask)0);
			guiiconChara.Icon_Chara.DispWakeUp(false);
			guiiconChara.Icon_Chara.DispMarkEvent(false, false, false);
			guiiconChara.Icon_Chara.DispMarkAccessory(false);
			int i = idx;
			guiiconChara.Current.gameObject.SetActive(idx == this.selectIconIndex && !flag3 && !flag);
			if (this.openParam.UIPreset != CharaWindowCtrl.DetailParamSetting.Preset.OTHER && this.openParam.UIPreset != CharaWindowCtrl.DetailParamSetting.Preset.OTHER_WITH_KEMOBOARD && !flag3 && !flag)
			{
				guiiconChara.Mark_Lock.gameObject.SetActive(true);
				MarkLockCtrl mark_Lock = guiiconChara.Mark_Lock;
				MarkLockCtrl.SetupParam setupParam2 = new MarkLockCtrl.SetupParam();
				setupParam2.updateConditionCallback = () => idx == 0 || this.currentCharaPackData.dynamicData.promoteNum >= idx + 1;
				setupParam2.releaseFlag = true;
				setupParam2.tagetObject = guiiconChara.Mark_Lock.gameObject;
				setupParam2.text = "野生解放\n" + (i + 1).ToString() + "段階目達成";
				setupParam2.updateUserFlagDataCallback = delegate
				{
				};
				mark_Lock.Setup(setupParam2, false);
				if (guiiconChara.Mark_Lock.gameObject.activeSelf)
				{
					PrjUtil.RemoveTouchEventTrigger(guiiconChara.Icon_Chara.gameObject);
					PrjUtil.AddTouchEventTrigger(guiiconChara.Icon_Chara.gameObject.transform.GetComponent<RectTransform>(), delegate(Transform trans)
					{
						CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("「アイコン」解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
						{
							new CmnReleaseConditionWindowCtrl.SetupParam
							{
								text = "野生解放" + (i + 1).ToString() + "段階目達成",
								enableClear = false
							}
						});
					});
				}
				else
				{
					PrjUtil.RemoveTouchEventTrigger(guiiconChara.Icon_Chara.gameObject);
					PrjUtil.AddTouchEventTrigger(guiiconChara.Icon_Chara.gameObject.transform.GetComponent<RectTransform>(), delegate(Transform trans)
					{
						this.selectIconIndex = i;
						for (int l = 0; l < this.guiData.guiIconCharaList.Count; l++)
						{
							this.guiData.guiIconCharaList[l].Current.SetActive(l == this.selectIconIndex);
						}
					});
				}
			}
			else
			{
				guiiconChara.Mark_Lock.gameObject.SetActive(false);
				bool flag5 = idx != 0 && this.currentCharaPackData.dynamicData.promoteNum < idx + 1;
				guiiconChara.Icon_Chara.IsEnableMask(flag5 && !flag3 && !flag);
				PrjUtil.RemoveTouchEventTrigger(guiiconChara.Icon_Chara.gameObject);
			}
			idx2 = idx;
		}
		CharaStaticBase baseData = this.currentCharaPackData.staticData.baseData;
		if (!AssetManager.IsExsistAssetData(baseData.animalImagePath))
		{
			this.guiData.Texture_Animal.SetRawImage("Texture2D/AnimalPicture/animalpicture_default", true, false, null);
		}
		else
		{
			this.guiData.Texture_Animal.SetRawImage(baseData.animalImagePath, true, false, null);
		}
		this.guiData.Txt_CopyLight.text = baseData.animalImageProvider;
		this.guiData.Txt_Tabitat.text = baseData.animalTabitat;
		this.guiData.Txt_DistributionArea.text = baseData.animalDistributionArea;
		this.guiData.Box04.SetActive(CharaDef.IsExistRedListId(baseData.AnimalRedListType));
		this.guiData.Txt_Current.text = CharaDef.GetRedListWording(baseData.AnimalRedListType);
		this.guiData.Txt_Info.text = CharaDef.GetRedListInformationSource(baseData.AnimalRedListType);
		List<string> iconArrangement = CharaDef.GetIconArrangement(baseData.AnimalRedListType);
		for (int k = 0; k < this.guiData.RedListImgColor.Count; k++)
		{
			this.guiData.RedListImg[k].m_Image.color = this.guiData.RedListImgColor[k].GetGameObjectById("NORMAL");
			this.guiData.RedListTxt[k].m_Text.color = this.guiData.RedListTxtColor[k].GetGameObjectById("NORMAL");
			this.guiData.RedListTxt[k].text = ((k < iconArrangement.Count) ? iconArrangement[k] : "");
		}
		int num2;
		switch (CharaDef.GetRedListLevel(baseData.AnimalRedListType))
		{
		case CharaDef.RedList.Level.L1:
			num2 = 0;
			break;
		case CharaDef.RedList.Level.L2:
			num2 = 1;
			break;
		case CharaDef.RedList.Level.L3:
			num2 = 2;
			break;
		case CharaDef.RedList.Level.L4:
			num2 = 3;
			break;
		case CharaDef.RedList.Level.L5:
			num2 = 4;
			break;
		case CharaDef.RedList.Level.L6:
			num2 = 5;
			break;
		case CharaDef.RedList.Level.L7:
			num2 = 6;
			break;
		default:
			num2 = -1;
			break;
		}
		if (0 <= num2 && num2 < this.guiData.RedListImgColor.Count)
		{
			this.guiData.RedListImg[num2].m_Image.color = this.guiData.RedListImgColor[num2].GetGameObjectById("ACTIVE");
			this.guiData.RedListTxt[num2].m_Text.color = this.guiData.RedListTxtColor[num2].GetGameObjectById("ACTIVE");
		}
		this.guiData.Txt_FlavorText.text = baseData.animalFlavorText;
		bool flag6 = this.guiData.Txt_FlavorText.AlignRectToText(this.guiData.Txt_FlavorText.transform.parent.GetComponent<RectTransform>());
		if (!flag6)
		{
			this.guiData.Rect_FlavorView.verticalNormalizedPosition = 1f;
		}
		this.guiData.Rect_FlavorView.vertical = flag6;
		if (this.renderTextureChara == null)
		{
			this.renderTextureChara = Object.Instantiate<GameObject>((GameObject)Resources.Load("RenderTextureChara/Prefab/RenderTextureCharaCtrl"), this.guiData.renderTexture.transform).GetComponent<RenderTextureChara>();
			this.renderTextureChara.fieldOfView = 30f;
		}
		this.renderTextureChara.Setup(this.currentCharaPackData, 2, CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true, null, false, null, 0f, null, false);
		this.guiData.AEImage_KisekiEff.gameObject.SetActive(this.currentCharaPackData.IsEnableSpAbility);
		this.guiData.AEImage_KisekiEff.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
		{
			this.guiData.AEImage_KisekiEff.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
		});
	}

	private IEnumerator IEOpen()
	{
		this.guiData.TabGroup.Setup(0, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectDetailTab));
		this.OnSelectDetailTab(0);
		ScrollRect component = this.guiData.ScrollView.GetComponent<ScrollRect>();
		if (component != null)
		{
			component.content.anchoredPosition = new Vector2(0f, 0f);
			component.velocity = new Vector2(0f, 0f);
			component.scrollSensitivity = ScrollParamDefine.CharaWindow;
		}
		this.guiData.AEImage_WindowClose.gameObject.SetActive(false);
		this.guiData.All.transform.localScale = new Vector3(0f, 0f, 0f);
		this.guiData.All.GetComponent<Animator>().cullingMode = AnimatorCullingMode.AlwaysAnimate;
		this.guiData.All.SetActive(true);
		this.guiData.Info_Status_ScrollView.transform.Find("Viewport/Content").GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
		yield return null;
		while (this.renderTextureChara != null && !this.renderTextureChara.FinishedSetup)
		{
			yield return null;
		}
		SoundManager.Play("prd_se_friends_profile_open", false, false);
		this.guiData.All.GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		bool next = false;
		Singleton<SceneManager>.Instance.GetCanvasCamera(SceneManager.CanvasType.SYSTEM).GetComponent<Blur>().enabled = true;
		this.guiData.AEImage_WindowOpen.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
		{
			this.guiData.AEImage_WindowOpen.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			next = true;
		});
		yield return null;
		this.guiData.All.transform.localScale = new Vector3(1f, 1f, 1f);
		while (!next)
		{
			yield return null;
		}
		yield break;
	}

	private void SetupButton(CharaPackData cpd)
	{
		bool flag = new List<CharaPackData>(DataManager.DmChara.GetUserCharaMap().Values).Contains(cpd);
		this.guiData.Btn_Yaji_Left.gameObject.SetActive(this.openParam.DispChangeBtn && flag && this.dispCharaPackList != null);
		this.guiData.Btn_Yaji_Right.gameObject.SetActive(this.openParam.DispChangeBtn && flag && this.dispCharaPackList != null);
		this.guiData.Btn_Grow.gameObject.SetActive(this.openParam.DispGrowBtn && flag);
		this.guiData.Btn_Scenario.gameObject.SetActive(this.openParam.DispScenarioBtn && flag);
		this.guiData.Btn_View.gameObject.SetActive(this.openParam.DispCameraBtn && flag);
		this.guiData.Btn_DressUp.gameObject.SetActive(this.openParam.DispClothBtn && flag);
	}

	public void Open(CharaPackData cpd, CharaWindowCtrl.DetailParamSetting param, CharaWindowCtrl.OnClick clickCloseBtnCB = null)
	{
		this.openParam = param;
		this.currentCharaPackData = cpd;
		this.dispCharaPackList = param.cpdList;
		if (this.dispCharaPackList != null && this.dispCharaPackList.Find((CharaPackData x) => x.id == cpd.id) == null)
		{
			this.dispCharaPackList.Add(cpd);
		}
		base.gameObject.SetActive(true);
		this.SetupButton(this.currentCharaPackData);
		this.ChangeChara(this.currentCharaPackData);
		UnityAction openCB = this.openParam.openCB;
		if (openCB != null)
		{
			openCB();
		}
		this.iEOpen = this.IEOpen();
		this.callback = clickCloseBtnCB;
	}

	public void OpenPrev()
	{
		base.gameObject.SetActive(true);
		this.ChangeChara(this.currentCharaPackData);
		UnityAction openPrevCB = this.openParam.openPrevCB;
		if (openPrevCB != null)
		{
			openPrevCB();
		}
		this.iEOpen = this.IEOpen();
	}

	public bool FinishedOpen()
	{
		return this.iEOpen == null;
	}

	private void Close()
	{
		this.CharaChangeIcon();
		Singleton<SceneManager>.Instance.GetCanvasCamera(SceneManager.CanvasType.SYSTEM).GetComponent<Blur>().enabled = false;
		this.guiData.All.GetComponent<SimpleAnimation>().ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.guiData.All.SetActive(false);
		this.guiData.AEImage_WindowOpen.PlayAnimation(PguiAECtrl.AmimeType.END, null);
		this.guiData.AEImage_WindowClose.gameObject.SetActive(true);
		this.guiData.AEImage_WindowClose.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
		{
			base.gameObject.SetActive(false);
			if (this.renderTextureChara != null)
			{
				Object.Destroy(this.renderTextureChara.gameObject);
			}
			this.renderTextureChara = null;
		});
	}

	public bool IsActive()
	{
		return base.gameObject.activeSelf;
	}

	private void CharaChangeIcon()
	{
		int num = ((this.selectIconIndex >= 1) ? (this.selectIconIndex + 1) : 0);
		if (num != this.currentCharaPackData.dynamicData.selectFaceIconId)
		{
			this.currentCharaPackData.dynamicData.selectFaceIconId = num;
			DataManager.DmChara.RequestActoinCharaChangeIcon(this.currentCharaPackData.id, num);
		}
	}

	private static readonly string WILD_RELEASE_MARK_LOCK_STR = PrjUtil.MakeMessage("野生解放１段階目達成");

	private static readonly float SPECIALY_ATTACK_DEFAULT_Y = 123f;

	private static readonly float KEMONO_MIRACLE_DEFAULT_Y = 123f;

	private static readonly float SKILL_UI_TEXT_SIZE = 24f;

	private static readonly int SKILL_UI_DEFAULT_LINE_NUM = 3;

	private CharaWindowCtrl.OnClick callback;

	private CharaWindowCtrl.GUI guiData;

	private CharaPackData currentCharaPackData;

	private List<CharaPackData> dispCharaPackList;

	private List<CharaClothStatic> haveClothes;

	private RenderTextureChara renderTextureChara;

	private int currenEquipClothId;

	private DataManagerGameStatus.UserFlagData userFlagData;

	private int selectIconIndex;

	private bool maxDispChangeChara;

	private bool shopDispChangeChara;

	private int BonusCharaDataListIndex;

	private CharaWindowCtrl.BonusCharaData bonusCharaData = new CharaWindowCtrl.BonusCharaData();

	private static readonly float r1000 = 1000f;

	private IEnumerator iEOpen;

	private CharaWindowCtrl.DetailParamSetting openParam = new CharaWindowCtrl.DetailParamSetting();

	public delegate void OnClick();

	private class BonusCharaData
	{
		public BonusCharaData()
		{
			this.list = new List<DataManagerChara.BonusCharaData>();
			this.isSpecial = false;
		}

		public List<DataManagerChara.BonusCharaData> list;

		public bool isSpecial;
	}

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.All = baseTr.Find("All").gameObject;
			this.Btn_View = baseTr.Find("All/Btn_View").GetComponent<PguiButtonCtrl>();
			this.Btn_Yaji_Left = baseTr.Find("All/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
			this.Btn_Yaji_Right = baseTr.Find("All/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
			this.Btn_Close = baseTr.Find("All/Btn_Close").GetComponent<PguiButtonCtrl>();
			this.Btn_Close.androidBackKeyTarget = true;
			this.Btn_Scenario = baseTr.Find("All/Btn_Scenario").GetComponent<PguiButtonCtrl>();
			this.Btn_DressUp = baseTr.Find("All/Btn_DressUp").GetComponent<PguiButtonCtrl>();
			this.Btn_Grow = baseTr.Find("All/Btn_Grow").GetComponent<PguiButtonCtrl>();
			this.names = new List<CharaWindowCtrl.GUI.Name>
			{
				new CharaWindowCtrl.GUI.Name(baseTr.Find("All/Name")),
				new CharaWindowCtrl.GUI.Name(baseTr.Find("All/Name_WName"))
			};
			this.Texture_Animal = baseTr.Find("All/RightBase/Info_Picture/Box01/Texture_Animal").GetComponent<PguiRawImageCtrl>();
			this.Txt_CopyLight = baseTr.Find("All/RightBase/Info_Picture/Box01/Txt_CopyLight").GetComponent<PguiTextCtrl>();
			this.Txt_Tabitat = baseTr.Find("All/RightBase/Info_Picture/Box02/Txt_Info").GetComponent<PguiTextCtrl>();
			this.Txt_DistributionArea = baseTr.Find("All/RightBase/Info_Picture/Box03/Txt_Info").GetComponent<PguiTextCtrl>();
			this.Txt_Current = baseTr.Find("All/RightBase/Info_Picture/Box04/Txt_Current").GetComponent<PguiTextCtrl>();
			this.Txt_Info = baseTr.Find("All/RightBase/Info_Picture/Box04/Txt_Info").GetComponent<PguiTextCtrl>();
			this.RedListImgColor = new List<PguiColorCtrl>
			{
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_LC/Base_Color").GetComponent<PguiColorCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_NT/Base_Color").GetComponent<PguiColorCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_VU/Base_Color").GetComponent<PguiColorCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_EN/Base_Color").GetComponent<PguiColorCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_CR/Base_Color").GetComponent<PguiColorCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_EW/Base_Color").GetComponent<PguiColorCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_EX/Base_Color").GetComponent<PguiColorCtrl>()
			};
			this.RedListImg = new List<PguiImageCtrl>
			{
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_LC/Base_Color").GetComponent<PguiImageCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_NT/Base_Color").GetComponent<PguiImageCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_VU/Base_Color").GetComponent<PguiImageCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_EN/Base_Color").GetComponent<PguiImageCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_CR/Base_Color").GetComponent<PguiImageCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_EW/Base_Color").GetComponent<PguiImageCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_EX/Base_Color").GetComponent<PguiImageCtrl>()
			};
			this.RedListTxtColor = new List<PguiColorCtrl>
			{
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_LC/Txt").GetComponent<PguiColorCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_NT/Txt").GetComponent<PguiColorCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_VU/Txt").GetComponent<PguiColorCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_EN/Txt").GetComponent<PguiColorCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_CR/Txt").GetComponent<PguiColorCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_EW/Txt").GetComponent<PguiColorCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_EX/Txt").GetComponent<PguiColorCtrl>()
			};
			this.RedListTxt = new List<PguiTextCtrl>
			{
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_LC/Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_NT/Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_VU/Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_EN/Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_CR/Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_EW/Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("All/RightBase/Info_Picture/Box04/RedList/Icon_RedList_EX/Txt").GetComponent<PguiTextCtrl>()
			};
			this.Rect_FlavorView = baseTr.Find("All/RightBase/Info_Picture/Box05/TextScrollView").GetComponent<ScrollRect>();
			this.Txt_FlavorText = baseTr.Find("All/RightBase/Info_Picture/Box05/TextScrollView/Viewport/Content/Txt_Info").GetComponent<PguiTextCtrl>();
			this.GUI_CharaInfo = baseTr.GetComponent<SimpleAnimation>();
			this.ScrollView = baseTr.Find("All/RightBase/Info_Skill/ScrollView").gameObject;
			this.guiSkillInfoList = new List<CharaUtil.GUISkillInfo>
			{
				new CharaUtil.GUISkillInfo(this.ScrollView.transform.Find("Viewport/Content/CharaInfo_List_Skill_01")),
				new CharaUtil.GUISkillInfo(this.ScrollView.transform.Find("Viewport/Content/CharaInfo_List_Skill_02")),
				new CharaUtil.GUISkillInfo(this.ScrollView.transform.Find("Viewport/Content/CharaInfo_List_Skill_03")),
				new CharaUtil.GUISkillInfo(this.ScrollView.transform.Find("Viewport/Content/CharaInfo_List_Skill_04")),
				new CharaUtil.GUISkillInfo(this.ScrollView.transform.Find("Viewport/Content/CharaInfo_List_Skill_05_kiseki")),
				new CharaUtil.GUISkillInfo(this.ScrollView.transform.Find("Viewport/Content/CharaInfo_List_Skill_06_nanairo"))
			};
			if (this.guiSkillInfoList[2].Disable != null)
			{
				this.guiSkillInfoList[2].Disable.SetActive(false);
			}
			this.TabGroup = baseTr.Find("All/RightBase/TabGroup").GetComponent<PguiTabGroupCtrl>();
			this.AEImage_WindowOpen = baseTr.Find("AEImage_WindowOpen").GetComponent<PguiAECtrl>();
			this.infoSkill = baseTr.Find("All/RightBase/Info_Skill").gameObject;
			this.infoProfile = baseTr.Find("All/RightBase/Info_Profile").gameObject;
			this.infoStatus = baseTr.Find("All/RightBase/Info_Status").gameObject;
			this.infoPicture = baseTr.Find("All/RightBase/Info_Picture").gameObject;
			this.Box04 = baseTr.Find("All/RightBase/Info_Picture/Box04").gameObject;
			this.Info_Status_ScrollView = this.infoStatus.transform.Find("ScrollView").GetComponent<ReuseScroll>();
			this.Info_Status_Btn_Guide_L = this.infoStatus.transform.Find("Btn_Guide_L").GetComponent<PguiButtonCtrl>();
			this.Info_Status_Btn_Guide_R = this.infoStatus.transform.Find("Btn_Guide_R").GetComponent<PguiButtonCtrl>();
			this.guiIconCharaList = new List<CharaWindowCtrl.GUIIconChara>();
			for (int i = 0; i < 4; i++)
			{
				string text = "IconSetting/Icon_Chara" + (i + 1).ToString("D2");
				Transform transform = this.infoProfile.transform.Find(text);
				this.guiIconCharaList.Add(new CharaWindowCtrl.GUIIconChara(transform));
			}
			this.ProfileTextList = new List<PguiTextCtrl>
			{
				baseTr.Find("All/RightBase/Info_Profile/Base/Txt_Profile").GetComponent<PguiTextCtrl>(),
				baseTr.Find("All/RightBase/Info_Profile/CVInfo/Txt_Name").GetComponent<PguiTextCtrl>()
			};
			this.renderTexture = baseTr.Find("All/Mask/RenderChara").gameObject;
			this.Null_BlurSpread = baseTr.Find("All/Null_BlurSpread").gameObject;
			this.Txt_GachaInfo = baseTr.Find("All/Txt_GachaInfo").GetComponent<PguiTextCtrl>();
			this.markLockCtrl = this.Btn_Scenario.transform.Find("Mark_Lock").GetComponent<MarkLockCtrl>();
			this.AEImage_WindowClose = baseTr.Find("AEImage_WindowClose").GetComponent<PguiAECtrl>();
			this.AEImage_WindowClose.gameObject.SetActive(false);
			this.AEImage_KisekiEff = baseTr.Find("All/AEImage_KisekiEff").GetComponent<PguiAECtrl>();
			this.AEImage_KisekiEff.gameObject.SetActive(false);
		}

		public static readonly int RED_LIST_NUM = 7;

		public GameObject baseObj;

		public GameObject All;

		public PguiButtonCtrl Btn_View;

		public PguiButtonCtrl Btn_Yaji_Left;

		public PguiButtonCtrl Btn_Yaji_Right;

		public PguiButtonCtrl Btn_Close;

		public PguiButtonCtrl Btn_Scenario;

		public PguiButtonCtrl Btn_DressUp;

		public PguiButtonCtrl Btn_Grow;

		public List<CharaWindowCtrl.GUI.Name> names;

		public PguiRawImageCtrl Texture_Animal;

		public PguiTextCtrl Txt_CopyLight;

		public PguiTextCtrl Txt_Tabitat;

		public PguiTextCtrl Txt_DistributionArea;

		public PguiTextCtrl Txt_Current;

		public PguiTextCtrl Txt_Info;

		public List<PguiColorCtrl> RedListImgColor;

		public List<PguiImageCtrl> RedListImg;

		public List<PguiColorCtrl> RedListTxtColor;

		public List<PguiTextCtrl> RedListTxt;

		public ScrollRect Rect_FlavorView;

		public PguiTextCtrl Txt_FlavorText;

		public SimpleAnimation GUI_CharaInfo;

		public GameObject ScrollView;

		public PguiTabGroupCtrl TabGroup;

		public PguiAECtrl AEImage_WindowOpen;

		public GameObject infoSkill;

		public GameObject infoProfile;

		public GameObject infoStatus;

		public GameObject infoPicture;

		public List<PguiTextCtrl> ProfileTextList;

		public List<CharaWindowCtrl.GUIIconChara> guiIconCharaList;

		public GameObject renderTexture;

		public GameObject Null_BlurSpread;

		public List<CharaUtil.GUISkillInfo> guiSkillInfoList;

		public PguiTextCtrl Txt_GachaInfo;

		public MarkLockCtrl markLockCtrl;

		public PguiAECtrl AEImage_WindowClose;

		public GameObject Box04;

		public ReuseScroll Info_Status_ScrollView;

		public Dictionary<string, CharaWindowCtrl.GUIInfoStatus> guiInfoStatusMap = new Dictionary<string, CharaWindowCtrl.GUIInfoStatus>();

		public PguiButtonCtrl Info_Status_Btn_Guide_L;

		public PguiButtonCtrl Info_Status_Btn_Guide_R;

		public PguiAECtrl AEImage_KisekiEff;

		public enum NameType
		{
			Normal,
			NickName
		}

		public class Name
		{
			public Name(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Detail = baseTr.Find("Detail").GetComponent<RectTransform>();
				this.Ornament = baseTr.Find("Detail/Ornament").GetComponent<RectTransform>();
				this.Icon_Atr = baseTr.Find("Detail/Ornament/Attribute/Icon_Atr").GetComponent<PguiImageCtrl>();
				this.Icon_SubAtr = baseTr.Find("Detail/Ornament/Attribute/Icon_SubAtr").GetComponent<PguiImageCtrl>();
				this.Txt_Kind = baseTr.Find("Detail/Txt_Kind").GetComponent<PguiTextCtrl>();
				this.Txt_Name_EG = baseTr.Find("Detail/NameDetail/Txt_Name_EG").GetComponent<PguiTextCtrl>();
				this.Txt_Name = baseTr.Find("Detail/NameDetail/Txt_Name").GetComponent<PguiTextCtrl>();
				this.Star_All_Obj = baseTr.Find("Detail/Ornament/StarAll").gameObject;
				this.Star_All_Init_Pos = this.Star_All_Obj.transform.localPosition;
				this.StarAll = new List<GameObject>
				{
					baseTr.Find("Detail/Ornament/StarAll/Icon_Star01").gameObject,
					baseTr.Find("Detail/Ornament/StarAll/Icon_Star02").gameObject,
					baseTr.Find("Detail/Ornament/StarAll/Icon_Star03").gameObject,
					baseTr.Find("Detail/Ornament/StarAll/Icon_Star04").gameObject,
					baseTr.Find("Detail/Ornament/StarAll/Icon_Star05").gameObject,
					baseTr.Find("Detail/Ornament/StarAll/Icon_Star06").gameObject
				};
				if (baseTr.Find("ShopInfo") != null)
				{
					this.ShopInfo = baseTr.Find("ShopInfo").gameObject;
					this.ShopInfo.SetActive(false);
				}
			}

			public void Setup(CharaPackData cpd, bool maxDisp)
			{
				this.baseObj.SetActive(true);
				this.Icon_Atr.SetImageByName(IconCharaCtrl.Attribute2IconName(cpd.staticData.baseData.attribute));
				if (cpd.staticData.baseData.subAttribute <= CharaDef.AttributeType.ALL)
				{
					this.Icon_SubAtr.gameObject.SetActive(false);
					this.Star_All_Obj.transform.localPosition = this.Star_All_Init_Pos - new Vector3(30f, 0f, 0f);
				}
				else
				{
					this.Star_All_Obj.transform.localPosition = this.Star_All_Init_Pos;
					this.Icon_SubAtr.gameObject.SetActive(true);
					this.Icon_SubAtr.SetImageByName(IconCharaCtrl.SubAttribute2IconName(cpd.staticData.baseData.subAttribute));
				}
				if (string.IsNullOrEmpty(cpd.staticData.baseData.NickName))
				{
					this.Txt_Name.text = cpd.staticData.baseData.charaName;
				}
				else
				{
					this.Txt_Name.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
					{
						cpd.staticData.baseData.NickName,
						cpd.staticData.GetName()
					});
				}
				this.Txt_Kind.text = cpd.staticData.baseData.eponymName;
				this.Txt_Name_EG.text = cpd.staticData.baseData.charaNameEng;
				if (!maxDisp)
				{
					int rank = cpd.dynamicData.rank;
				}
				else
				{
					int rankLow = cpd.staticData.baseData.rankLow;
				}
				for (int i = 0; i < this.StarAll.Count; i++)
				{
					string text = ((i < cpd.dynamicData.rank) ? "icon_star" : "icon_star_blank");
					DataManagerChara.LevelLimitData levelLimitData = DataManager.DmChara.GetLevelLimitData(cpd.dynamicData.levelLimitId);
					if (levelLimitData != null)
					{
						if (cpd.dynamicData.level == levelLimitData.maxLevel)
						{
							text = levelLimitData.compImageName;
						}
						else
						{
							text = levelLimitData.openImageName;
						}
					}
					text += "_m";
					this.StarAll[i].GetComponent<PguiImageCtrl>().SetImageByName(text);
					this.StarAll[i].SetActive(i < cpd.staticData.baseData.rankHigh);
				}
				this.Detail.GetComponent<ContentSizeFitter>().SetLayoutVertical();
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.Detail);
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.Ornament);
			}

			public GameObject baseObj;

			public PguiImageCtrl Icon_Atr;

			public GameObject Star_All_Obj;

			public Vector3 Star_All_Init_Pos;

			public PguiImageCtrl Icon_SubAtr;

			public PguiTextCtrl Txt_Kind;

			public PguiTextCtrl Txt_Name_EG;

			public PguiTextCtrl Txt_Name;

			public List<GameObject> StarAll;

			public GameObject ShopInfo;

			public RectTransform Detail;

			public RectTransform Ornament;
		}
	}

	public class GUIInfoStatus
	{
		public GUIInfoStatus(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.StatusTextList = new List<PguiTextCtrl>
			{
				baseTr.Find("Info_Status/Box01/Parameter/Info_Total/Num").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Info_Status/Box01/Parameter/Info_Yasei/Num").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Info_Status/Box01/Parameter/Info_HP/Num").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Info_Status/Box01/Parameter/Info_Atack/Num").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Info_Status/Box01/Parameter/Info_Guard/Num").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Info_Status/Box01/Parameter/Info_ArtsLv/Num").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Info_Status/Box01/Parameter/Info_Kemono/Num").GetComponent<PguiTextCtrl>()
			};
			this.StatusTextLv = baseTr.Find("Info_Status/Box01/Base_Lv/Lv/Gage_Lv/Num_Lv").GetComponent<PguiTextCtrl>();
			this.StatusTextLvExp = baseTr.Find("Info_Status/Box01/Base_Lv/Lv/Exp/Num_Exp").GetComponent<PguiTextCtrl>();
			this.LvGage = baseTr.Find("Info_Status/Box01/Base_Lv/Lv/Gage_Lv/Img_Gage").GetComponent<PguiImageCtrl>();
			this.StatusTextKizunaLv = baseTr.Find("Info_Status/Box01/Base_Lv/Heart/Gage_HeartLv/Num_Lv").GetComponent<PguiTextCtrl>();
			this.StatusTextKizunaLvExp = baseTr.Find("Info_Status/Box01/Base_Lv/Heart/Exp/Num_Exp").GetComponent<PguiTextCtrl>();
			this.KizunaLvGage = baseTr.Find("Info_Status/Box01/Base_Lv/Heart/Gage_HeartLv/Img_HeartGage").GetComponent<PguiImageCtrl>();
			this.Box02 = baseTr.Find("Info_Status/Box02").gameObject;
			this.Info_Flag_Beat = baseTr.Find("Info_Status/Box02/Base_Flag/Info_Flag_Beat/Num").GetComponent<PguiTextCtrl>();
			this.Info_Flag_Try = baseTr.Find("Info_Status/Box02/Base_Flag/Info_Flag_Try/Num").GetComponent<PguiTextCtrl>();
			this.Info_Flag_Action = baseTr.Find("Info_Status/Box02/Base_Flag/Info_Flag_Action/Num").GetComponent<PguiTextCtrl>();
			GameObject[] array = new GameObject[]
			{
				AssetManager.GetAssetData("SelCmn/GUI/Prefab/CharaInfo_OrderCard_Tameru") as GameObject,
				AssetManager.GetAssetData("SelCmn/GUI/Prefab/CharaInfo_OrderCard_Tokui") as GameObject,
				AssetManager.GetAssetData("SelCmn/GUI/Prefab/CharaInfo_OrderCard_Yasei") as GameObject,
				AssetManager.GetAssetData("SelCmn/GUI/Prefab/CharaInfo_OrderCard_Special") as GameObject
			};
			for (int i = 0; i < 5; i++)
			{
				Transform transform = this.Box02.transform.Find("Grid");
				for (int j = 0; j < array.Length; j++)
				{
					Object.Instantiate<GameObject>(array[j], transform).name = array[j].name + i.ToString();
				}
				this.guiOrderCardList.Add(new CharaWindowCtrl.GUIOrderCard(transform, i, true));
			}
			this.TxtBonusRatioList = new List<PguiTextCtrl>
			{
				baseTr.Find("Info_Status/Box01/Base_Lv/Heart/Gage_HeartLv/Num_EventEffect").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Info_Status/Box01/Parameter/Info_HP/Num_EventEffect").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Info_Status/Box01/Parameter/Info_Atack/Num_EventEffect").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Info_Status/Box01/Parameter/Info_Guard/Num_EventEffect").GetComponent<PguiTextCtrl>()
			};
			this.Info_StatusKind_Event = baseTr.Find("Info_StatusKind_Event").gameObject;
			this.Info_StatusKind_Normal = baseTr.Find("Info_StatusKind_Normal").gameObject;
			this.TxtBoardPlusList = new List<PguiTextCtrl>
			{
				baseTr.Find("Info_Status/Box01/Parameter/Info_Total/Num_BoardPlus").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Info_Status/Box01/Parameter/Info_HP/Num_BoardPlus").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Info_Status/Box01/Parameter/Info_Atack/Num_BoardPlus").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Info_Status/Box01/Parameter/Info_Guard/Num_BoardPlus").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Info_Status/Box01/Parameter/Info_ArtsLv/Num_BoardPlus").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Info_Status/Box02/Base_Flag/Info_Flag_Beat/Num_BoardPlus").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Info_Status/Box02/Base_Flag/Info_Flag_Try/Num_BoardPlus").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Info_Status/Box02/Base_Flag/Info_Flag_Action/Num_BoardPlus").GetComponent<PguiTextCtrl>()
			};
			this.InfoPhotoItemEffectCtrl = baseTr.Find("Info_Status/Box01/EventBonus/Info_PhotoItemEffect").GetComponent<InfoPhotoItemEffectCtrl>();
		}

		public GameObject baseObj;

		public List<PguiTextCtrl> StatusTextList;

		public PguiTextCtrl StatusTextLv;

		public PguiTextCtrl StatusTextLvExp;

		public PguiTextCtrl StatusTextKizunaLv;

		public PguiTextCtrl StatusTextKizunaLvExp;

		public PguiImageCtrl KizunaLvGage;

		public PguiImageCtrl LvGage;

		public GameObject Box02;

		public PguiTextCtrl Info_Flag_Beat;

		public PguiTextCtrl Info_Flag_Try;

		public PguiTextCtrl Info_Flag_Action;

		public List<CharaWindowCtrl.GUIOrderCard> guiOrderCardList = new List<CharaWindowCtrl.GUIOrderCard>();

		public List<PguiTextCtrl> TxtBonusRatioList;

		public GameObject Info_StatusKind_Event;

		public GameObject Info_StatusKind_Normal;

		public List<PguiTextCtrl> TxtBoardPlusList;

		public InfoPhotoItemEffectCtrl InfoPhotoItemEffectCtrl;
	}

	public class GUIOrderCard
	{
		public GUIOrderCard(Transform baseTr, int index, bool isSuffixIndex = true)
		{
			string text = (isSuffixIndex ? index.ToString() : "");
			this.BaseTameru = baseTr.Find("CharaInfo_OrderCard_Tameru" + text).gameObject;
			this.BaseTokui = baseTr.Find("CharaInfo_OrderCard_Tokui" + text).gameObject;
			this.BaseYasei = baseTr.Find("CharaInfo_OrderCard_Yasei" + text).gameObject;
			this.BaseSpecial = baseTr.Find("CharaInfo_OrderCard_Special" + text).gameObject;
			this.YaseiMark = baseTr.Find("CharaInfo_OrderCard_Yasei" + text + "/Base/Mark").GetComponent<PguiImageCtrl>();
			this.YaseiMarkSP = baseTr.Find("CharaInfo_OrderCard_Special" + text + "/Base/Mark").GetComponent<PguiImageCtrl>();
			this.TameruTxt = baseTr.Find("CharaInfo_OrderCard_Tameru" + text + "/Base/Txt").GetComponent<PguiTextCtrl>();
			this.TameruTxtSP = baseTr.Find("CharaInfo_OrderCard_Special" + text + "/Base/Txt").GetComponent<PguiTextCtrl>();
		}

		public void Setup(CharaOrderCard coc, CharaStaticBase baseData)
		{
			if (coc == null)
			{
				this.BaseTameru.SetActive(false);
				this.BaseTokui.SetActive(false);
				this.BaseYasei.SetActive(false);
				this.BaseSpecial.SetActive(false);
				return;
			}
			this.BaseTameru.SetActive(coc.type == CharaDef.OrderCardType.ACTION);
			this.BaseTokui.SetActive(coc.type == CharaDef.OrderCardType.BEAT);
			this.BaseYasei.SetActive(coc.type == CharaDef.OrderCardType.TRY);
			this.BaseSpecial.SetActive(coc.type == CharaDef.OrderCardType.SPECIAL);
			bool flag = baseData.isSpecialFlagSupported && coc.type == CharaDef.OrderCardType.SPECIAL;
			int num = (flag ? baseData.orderCardSPValueMP : coc.param);
			int num2 = (flag ? baseData.orderCardSPValuePlasm : coc.param);
			if (coc.type == CharaDef.OrderCardType.ACTION || flag)
			{
				(flag ? this.TameruTxtSP : this.TameruTxt).text = num.ToString();
			}
			if (coc.type == CharaDef.OrderCardType.TRY || flag)
			{
				PguiImageCtrl pguiImageCtrl = (flag ? this.YaseiMarkSP : this.YaseiMark);
				if (num2 <= 20)
				{
					pguiImageCtrl.SetImageByName("mark_trycondition_01");
					return;
				}
				if (num2 <= 30)
				{
					pguiImageCtrl.SetImageByName("mark_trycondition_02");
					return;
				}
				pguiImageCtrl.SetImageByName("mark_trycondition_03");
			}
		}

		public GameObject BaseTameru;

		public GameObject BaseTokui;

		public GameObject BaseYasei;

		public GameObject BaseSpecial;

		public PguiImageCtrl YaseiMark;

		public PguiImageCtrl YaseiMarkSP;

		public PguiTextCtrl TameruTxt;

		public PguiTextCtrl TameruTxtSP;
	}

	public class GUIIconChara
	{
		public GUIIconChara(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Chara"), baseTr.Find("Icon_CharaSet/Icon_Chara"));
			this.Icon_Chara = gameObject.GetComponent<IconCharaCtrl>();
			this.Current = baseTr.Find("Icon_CharaSet/Current").gameObject;
			this.Remove = baseTr.Find("Icon_CharaSet/Remove").GetComponent<PguiImageCtrl>();
			this.Remove.gameObject.SetActive(false);
			this.selected = baseTr.Find("Icon_CharaSet/Fnt_Selected").gameObject;
			this.selected.SetActive(false);
			this.disable = baseTr.Find("Icon_CharaSet/Txt_Disable").gameObject;
			this.disable.SetActive(false);
			this.Txt_LockInfo = baseTr.Find("Mark_Lock/Txt_LockInfo").GetComponent<PguiTextCtrl>();
			this.AEImage_Eff_Change = baseTr.Find("Icon_CharaSet/AEImage_Eff_Change").GetComponent<PguiAECtrl>();
			this.Mark_Lock = baseTr.Find("Mark_Lock").GetComponent<MarkLockCtrl>();
		}

		public GameObject baseObj;

		public IconCharaCtrl Icon_Chara;

		public GameObject Current;

		public PguiImageCtrl Remove;

		public GameObject selected;

		public GameObject disable;

		public PguiTextCtrl Txt_LockInfo;

		public PguiAECtrl AEImage_Eff_Change;

		public MarkLockCtrl Mark_Lock;
	}

	public class DetailParamSetting
	{
		public bool DispChangeBtn
		{
			get
			{
				bool flag = false;
				CharaWindowCtrl.DetailParamSetting.Preset uipreset = this.UIPreset;
				if (uipreset - CharaWindowCtrl.DetailParamSetting.Preset.OTHER > 1 && uipreset - CharaWindowCtrl.DetailParamSetting.Preset.NO_VIEW > 1 && uipreset - CharaWindowCtrl.DetailParamSetting.Preset.OTHER_WITH_KEMOBOARD > 1)
				{
					flag = true;
				}
				return flag;
			}
		}

		public bool DispClothBtn
		{
			get
			{
				bool flag = false;
				CharaWindowCtrl.DetailParamSetting.Preset uipreset = this.UIPreset;
				if (uipreset - CharaWindowCtrl.DetailParamSetting.Preset.OTHER > 1 && uipreset != CharaWindowCtrl.DetailParamSetting.Preset.GACHA_RESULT && uipreset - CharaWindowCtrl.DetailParamSetting.Preset.OTHER_WITH_KEMOBOARD > 1)
				{
					flag = true;
				}
				return flag;
			}
		}

		public bool DispGrowBtn
		{
			get
			{
				bool flag = false;
				CharaWindowCtrl.DetailParamSetting.Preset uipreset = this.UIPreset;
				if (uipreset - CharaWindowCtrl.DetailParamSetting.Preset.OTHER > 1 && uipreset != CharaWindowCtrl.DetailParamSetting.Preset.GACHA_RESULT && uipreset - CharaWindowCtrl.DetailParamSetting.Preset.OTHER_WITH_KEMOBOARD > 1)
				{
					flag = true;
				}
				return flag;
			}
		}

		public bool DispScenarioBtn
		{
			get
			{
				bool flag = false;
				CharaWindowCtrl.DetailParamSetting.Preset uipreset = this.UIPreset;
				if (uipreset - CharaWindowCtrl.DetailParamSetting.Preset.OTHER > 1 && uipreset != CharaWindowCtrl.DetailParamSetting.Preset.GACHA_RESULT && uipreset - CharaWindowCtrl.DetailParamSetting.Preset.OTHER_WITH_KEMOBOARD > 1)
				{
					flag = true;
				}
				return flag;
			}
		}

		public bool DispCameraBtn
		{
			get
			{
				bool flag = false;
				CharaWindowCtrl.DetailParamSetting.Preset uipreset = this.UIPreset;
				if (uipreset - CharaWindowCtrl.DetailParamSetting.Preset.OTHER > 1 && uipreset - CharaWindowCtrl.DetailParamSetting.Preset.NO_VIEW > 1 && uipreset - CharaWindowCtrl.DetailParamSetting.Preset.OTHER_WITH_KEMOBOARD > 1)
				{
					flag = true;
				}
				return flag;
			}
		}

		public bool DispMaxInfo
		{
			get
			{
				bool flag = false;
				if (this.UIPreset == CharaWindowCtrl.DetailParamSetting.Preset.DISPLAY)
				{
					flag = true;
				}
				return flag;
			}
		}

		public bool DispKemoBordInfo
		{
			get
			{
				bool flag = false;
				CharaWindowCtrl.DetailParamSetting.Preset uipreset = this.UIPreset;
				if (uipreset - CharaWindowCtrl.DetailParamSetting.Preset.OTHER > 1 && uipreset != CharaWindowCtrl.DetailParamSetting.Preset.MINE_EASY_NO_KEMOBOARD && uipreset != CharaWindowCtrl.DetailParamSetting.Preset.SHOP_DETAIL)
				{
					flag = true;
				}
				return flag;
			}
		}

		public DetailParamSetting()
		{
		}

		public DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset preset, List<CharaPackData> dispList = null)
		{
			this.UIPreset = preset;
			this.cpdList = dispList;
		}

		public List<CharaPackData> cpdList;

		public UnityAction openCB;

		public UnityAction openPrevCB;

		public CharaWindowCtrl.DetailParamSetting.Preset UIPreset;

		public int pvpSeasonId;

		public int selectEventId;

		public int selectQuestOneId;

		public UserDeckData.Category deckCategory;

		public enum Preset
		{
			MINE_EASY,
			MINE_DETAIL,
			OTHER,
			DISPLAY,
			HOME,
			HOME_LIST,
			HOME_DRESS,
			MINE_EASY_NO_GROW,
			MINE_EASY_NO_SCENARIO,
			NO_VIEW,
			GACHA_RESULT,
			MINE_EASY_NO_KEMOBOARD,
			OTHER_WITH_KEMOBOARD,
			SHOP_DETAIL
		}
	}
}
