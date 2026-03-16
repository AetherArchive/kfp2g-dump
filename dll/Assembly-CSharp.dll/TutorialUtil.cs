using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Common;
using SGNFW.HttpRequest.Protocol;
using UnityEngine;

public class TutorialUtil
{
	public static void RequestNextSequence(TutorialUtil.Sequence nowSeq)
	{
		SceneManager.SceneName sceneName = SceneManager.SceneName.None;
		object obj = null;
		TutorialUtil.Sequence sequence = nowSeq + 1;
		if (sequence == TutorialUtil.Sequence.CHARA_GROW)
		{
			Singleton<DataManager>.Instance.DisableServerRequestByTutorial = false;
			DataManager.DmUserInfo.RequestActionUpdateTutorialStep((int)sequence);
			Singleton<DataManager>.Instance.DisableServerRequestByTutorial = true;
			sequence++;
		}
		TutorialUtil.UpdateDataManagerByDirect(sequence);
		switch (sequence)
		{
		case TutorialUtil.Sequence.USER_SETTING:
			sceneName = SceneManager.SceneName.SceneTutorialFirst;
			obj = new SceneTutorialFirst.Args
			{
				tutorialSequence = sequence
			};
			break;
		case TutorialUtil.Sequence.SCENARIO_00:
			sceneName = SceneManager.SceneName.SceneScenario;
			obj = new SceneScenario.Args
			{
				tutorialSequence = sequence,
				questId = 11,
				storyType = 1,
				scenarioName = DataManager.DmQuest.QuestStaticData.oneDataMap[11].scenarioBeforeId
			};
			break;
		case TutorialUtil.Sequence.BATTLE_00:
			sceneName = SceneManager.SceneName.SceneBattle;
			obj = new SceneBattleArgs
			{
				hash_id = 1L,
				tutorialSequence = sequence,
				questOneId = 11,
				selectDeckId = 1
			};
			break;
		case TutorialUtil.Sequence.SCENARIO_01:
			sceneName = SceneManager.SceneName.SceneScenario;
			obj = new SceneScenario.Args
			{
				tutorialSequence = sequence,
				questId = 11,
				storyType = 2,
				scenarioName = DataManager.DmQuest.QuestStaticData.oneDataMap[11].scenarioAfterId
			};
			break;
		case TutorialUtil.Sequence.SCENARIO_02:
			sceneName = SceneManager.SceneName.SceneScenario;
			obj = new SceneScenario.Args
			{
				tutorialSequence = sequence,
				questId = 12,
				storyType = 1,
				scenarioName = DataManager.DmQuest.QuestStaticData.oneDataMap[12].scenarioBeforeId
			};
			break;
		case TutorialUtil.Sequence.BATTLE_01:
			sceneName = SceneManager.SceneName.SceneBattle;
			obj = new SceneBattleArgs
			{
				hash_id = 2L,
				tutorialSequence = sequence,
				questOneId = 12,
				selectDeckId = 1
			};
			break;
		case TutorialUtil.Sequence.SCENARIO_03:
			sceneName = SceneManager.SceneName.SceneScenario;
			obj = new SceneScenario.Args
			{
				tutorialSequence = sequence,
				questId = 12,
				storyType = 2,
				scenarioName = DataManager.DmQuest.QuestStaticData.oneDataMap[12].scenarioAfterId
			};
			break;
		case TutorialUtil.Sequence.GACHA_PLAY:
			sceneName = SceneManager.SceneName.SceneGacha;
			obj = new SceneGacha.OpenParam
			{
				tutorialSequence = sequence
			};
			break;
		case TutorialUtil.Sequence.SCENARIO_04:
			sceneName = SceneManager.SceneName.SceneScenario;
			obj = new SceneScenario.Args
			{
				tutorialSequence = sequence,
				questId = 13,
				storyType = 1,
				scenarioName = DataManager.DmQuest.QuestStaticData.oneDataMap[13].scenarioBeforeId
			};
			break;
		case TutorialUtil.Sequence.PARTY_EDIT:
			sceneName = SceneManager.SceneName.SceneCharaEdit;
			obj = new SceneCharaEdit.Args
			{
				tutorialSequence = sequence
			};
			break;
		case TutorialUtil.Sequence.HELPER_SELECT_00:
			sceneName = SceneManager.SceneName.SceneBattleSelector;
			obj = new SceneBattleSelector.Args
			{
				tutorialSequence = sequence,
				selectQuestOneId = 13
			};
			break;
		case TutorialUtil.Sequence.BATTLE_02:
		{
			sceneName = SceneManager.SceneName.SceneBattle;
			SceneBattleArgs sceneBattleArgs = new SceneBattleArgs();
			sceneBattleArgs.hash_id = 3L;
			sceneBattleArgs.tutorialSequence = sequence;
			sceneBattleArgs.questOneId = 13;
			sceneBattleArgs.selectDeckId = 1;
			sceneBattleArgs.helper = new Helper
			{
				friend_id = TutorialUtil.SelectHelperCharaId
			};
			List<ItemInput> list = DataManager.DmChara.GetCharaStaticData(1).promoteList[0].promoteOneList.ConvertAll<ItemInput>((CharaPromoteOne input) => new ItemInput
			{
				itemId = input.promoteUseItemId,
				num = input.promoteUseItemNum
			});
			sceneBattleArgs.dropItemList = new List<DrewItem>
			{
				new DrewItem
				{
					item_id = list[0].itemId,
					item_num = 1
				}
			};
			obj = sceneBattleArgs;
			break;
		}
		case TutorialUtil.Sequence.SCENARIO_05:
			sceneName = SceneManager.SceneName.SceneScenario;
			obj = new SceneScenario.Args
			{
				tutorialSequence = sequence,
				questId = 13,
				storyType = 2,
				scenarioName = DataManager.DmQuest.QuestStaticData.oneDataMap[13].scenarioAfterId
			};
			break;
		case TutorialUtil.Sequence.CHARA_GROW:
			sceneName = SceneManager.SceneName.SceneCharaEdit;
			obj = new SceneCharaEdit.Args
			{
				tutorialSequence = sequence
			};
			break;
		case TutorialUtil.Sequence.HELPER_SELECT_01:
			sceneName = SceneManager.SceneName.SceneBattleSelector;
			obj = new SceneBattleSelector.Args
			{
				tutorialSequence = sequence,
				selectQuestOneId = 14
			};
			break;
		case TutorialUtil.Sequence.SCENARIO_06:
			sceneName = SceneManager.SceneName.SceneScenario;
			obj = new SceneScenario.Args
			{
				tutorialSequence = sequence,
				questId = 14,
				storyType = 1,
				scenarioName = DataManager.DmQuest.QuestStaticData.oneDataMap[14].scenarioBeforeId
			};
			break;
		case TutorialUtil.Sequence.BATTLE_03:
			sceneName = SceneManager.SceneName.SceneBattle;
			obj = new SceneBattleArgs
			{
				hash_id = 4L,
				tutorialSequence = sequence,
				questOneId = 14,
				selectDeckId = 1,
				helper = new Helper
				{
					friend_id = TutorialUtil.SelectHelperCharaId
				}
			};
			break;
		case TutorialUtil.Sequence.SCENARIO_07:
			sceneName = SceneManager.SceneName.SceneScenario;
			obj = new SceneScenario.Args
			{
				tutorialSequence = sequence,
				questId = 14,
				storyType = 2,
				scenarioName = DataManager.DmQuest.QuestStaticData.oneDataMap[14].scenarioAfterId
			};
			break;
		case TutorialUtil.Sequence.DATA_RESET:
			if (PlayerPrefs.HasKey(TutorialUtil.tutorialRestartKey))
			{
				PlayerPrefs.DeleteKey(TutorialUtil.tutorialRestartKey);
			}
			sceneName = SceneManager.SceneName.SceneTutorialEnd;
			obj = null;
			break;
		case TutorialUtil.Sequence.OPENING_MOVIE:
			sceneName = SceneManager.SceneName.SceneOpening;
			obj = null;
			break;
		case TutorialUtil.Sequence.QUEST_GUIDE:
			sceneName = SceneManager.SceneName.SceneHome;
			obj = new SceneHome.Args
			{
				tutorialSequence = sequence,
				charaPackData = null
			};
			break;
		}
		Singleton<DataManager>.Instance.DisableServerRequestByTutorial = false;
		DataManager.DmUserInfo.RequestActionUpdateTutorialStep((int)sequence);
		Singleton<DataManager>.Instance.DisableServerRequestByTutorial = sequence <= TutorialUtil.Sequence.DATA_RESET;
		PrjUtil.SendAppsFlyerLtvIdByTutorial(nowSeq);
		if (sceneName != SceneManager.SceneName.None)
		{
			Singleton<SceneManager>.Instance.SetNextScene(sceneName, obj);
		}
	}

	public static void UpdateDataManagerByCharaGrow()
	{
		DataManager.DmChara.GetUserCharaData(1).dynamicData.promoteFlag[0] = true;
		CharaPromoteOne charaPromoteOne = DataManager.DmChara.GetCharaStaticData(1).promoteList[0].promoteOneList[0];
		List<ItemInput> list = new List<ItemInput>
		{
			new ItemInput
			{
				itemId = charaPromoteOne.promoteUseItemId,
				num = charaPromoteOne.promoteUseItemNum
			}
		};
		int costGoldNum = charaPromoteOne.costGoldNum;
		list.Add(new ItemInput(30101, costGoldNum));
		DataManager.DmItem.UpdateUserDataByDirect(list, 2);
	}

	private static void UpdateDataManagerByDirect(TutorialUtil.Sequence seq)
	{
		switch (seq)
		{
		case TutorialUtil.Sequence.BATTLE_00:
		{
			List<CharaPackData> list = new List<CharaPackData>
			{
				CharaPackData.MakeInitial(4),
				CharaPackData.MakeInitial(322),
				CharaPackData.MakeInitial(3)
			};
			DataManager.DmChara.UpdateUserDataByTutorial(list);
			List<MasterSkillPackData> list2 = new List<MasterSkillPackData> { MasterSkillPackData.MakeDummy(80001) };
			DataManager.DmChara.UpdateUserDataByTutorial(list2);
			DataManager.DmDeck.UpdateUserDataByTutorial(1, new List<int> { 0, 4, 322, 3, 0 }, false, 80001);
			return;
		}
		case TutorialUtil.Sequence.SCENARIO_01:
		case TutorialUtil.Sequence.SCENARIO_02:
		case TutorialUtil.Sequence.SCENARIO_03:
		case TutorialUtil.Sequence.SCENARIO_04:
		case TutorialUtil.Sequence.SCENARIO_05:
		case TutorialUtil.Sequence.SCENARIO_06:
			break;
		case TutorialUtil.Sequence.BATTLE_01:
		{
			List<CharaPackData> list3 = new List<CharaPackData>
			{
				CharaPackData.MakeInitial(22),
				CharaPackData.MakeInitial(1),
				CharaPackData.MakeInitial(21)
			};
			DataManager.DmChara.UpdateUserDataByTutorial(list3);
			List<MasterSkillPackData> list4 = new List<MasterSkillPackData> { MasterSkillPackData.MakeDummy(80001) };
			DataManager.DmChara.UpdateUserDataByTutorial(list4);
			DataManager.DmDeck.UpdateUserDataByTutorial(1, new List<int> { 0, 22, 1, 21, 0 }, false, 80001);
			return;
		}
		case TutorialUtil.Sequence.GACHA_PLAY:
		{
			List<CharaPackData> list5 = new List<CharaPackData>
			{
				CharaPackData.MakeInitial(22),
				CharaPackData.MakeInitial(1),
				CharaPackData.MakeInitial(TutorialUtil.TutorialGuestCharaId)
			};
			DataManager.DmChara.UpdateUserDataByTutorial(list5);
			List<MasterSkillPackData> list6 = new List<MasterSkillPackData> { MasterSkillPackData.MakeDummy(80001) };
			DataManager.DmChara.UpdateUserDataByTutorial(list6);
			DataManager.DmItem.UpdateUserDataByDirect(new List<ItemInput>
			{
				new ItemInput(30001, 0)
			}, 0);
			DataManagerGacha.GachaPackData gachaPackData = new DataManagerGacha.GachaPackData
			{
				gachaId = 9999,
				staticData = new DataManagerGacha.GachaStaticData
				{
					gachaId = 9999,
					gachaCategory = DataManagerGacha.Category.KiraKira,
					gachaName = "チュートリアル便",
					banner = "mxanxk9e4abvkbt4kIk9tDxdKW-dtLlDu8",
					labelTextureName = "mxanxk9e4yIkk9v4kIk9tDxdKCiSU7uDAN",
					typeDataList = new List<DataManagerGacha.GachaStaticTypeData>
					{
						new DataManagerGacha.GachaStaticTypeData(1, 30100, 0)
					},
					InfoDispIdList = new List<int> { TutorialUtil.TutorialGuestCharaId },
					startDatetime = TimeManager.Now,
					endDatetime = new DateTime(2100, 1, 1)
				},
				dynamicData = new DataManagerGacha.DynamicGachaData
				{
					gachaId = 9999,
					gachaTypeData = new List<DataManagerGacha.DynamicGachaTypeData>
					{
						new DataManagerGacha.DynamicGachaTypeData()
					}
				}
			};
			DataManager.DmGacha.SetGachaPackDataByDirect(gachaPackData);
			return;
		}
		case TutorialUtil.Sequence.PARTY_EDIT:
		{
			List<CharaPackData> list7 = new List<CharaPackData>
			{
				CharaPackData.MakeInitial(22),
				CharaPackData.MakeInitial(1),
				CharaPackData.MakeInitial(TutorialUtil.TutorialGuestCharaId)
			};
			DataManager.DmChara.UpdateUserDataByTutorial(list7);
			List<MasterSkillPackData> list8 = new List<MasterSkillPackData> { MasterSkillPackData.MakeDummy(80001) };
			DataManager.DmChara.UpdateUserDataByTutorial(list8);
			DataManager.DmDeck.UpdateUserDataByTutorial(1, new List<int> { 0, 22, 1, 0, -1 }, false, 80001);
			DataManager.DmPhoto.UpdateUserDataByTutorial(new List<int> { TutorialUtil.TutorialPhotoId });
			return;
		}
		case TutorialUtil.Sequence.HELPER_SELECT_00:
		case TutorialUtil.Sequence.BATTLE_02:
		{
			List<HelperPackData> rentalHelperList = DataManager.DmHelper.GetRentalHelperList();
			rentalHelperList.Clear();
			rentalHelperList.Add(HelperPackData.MakeTutorialDummy(4, "アライグマ"));
			rentalHelperList.Add(HelperPackData.MakeTutorialDummy(3, "フェネック"));
			if (TutorialUtil.SelectHelperCharaId != 4 && TutorialUtil.SelectHelperCharaId != 3 && PlayerPrefs.HasKey(TutorialUtil.tutorialRestartKey))
			{
				TutorialUtil.SelectHelperCharaId = PlayerPrefs.GetInt(TutorialUtil.tutorialRestartKey);
			}
			if (TutorialUtil.SelectHelperCharaId != 4 && TutorialUtil.SelectHelperCharaId != 3)
			{
				TutorialUtil.SelectHelperCharaId = 4;
			}
			List<CharaPackData> list9 = new List<CharaPackData>
			{
				CharaPackData.MakeInitial(TutorialUtil.SelectHelperCharaId),
				CharaPackData.MakeInitial(22),
				CharaPackData.MakeInitial(1),
				CharaPackData.MakeInitial(TutorialUtil.TutorialGuestCharaId)
			};
			DataManager.DmChara.UpdateUserDataByTutorial(list9);
			List<MasterSkillPackData> list10 = new List<MasterSkillPackData> { MasterSkillPackData.MakeDummy(80001) };
			DataManager.DmChara.UpdateUserDataByTutorial(list10);
			DataManager.DmPhoto.UpdateUserDataByTutorial(new List<int> { TutorialUtil.TutorialPhotoId });
			DataManager.DmDeck.UpdateUserDataByTutorial(1, new List<int>
			{
				0,
				22,
				1,
				TutorialUtil.TutorialGuestCharaId,
				-1
			}, true, 80001);
			return;
		}
		case TutorialUtil.Sequence.CHARA_GROW:
		{
			List<CharaPackData> list11 = new List<CharaPackData>
			{
				CharaPackData.MakeInitial(22),
				CharaPackData.MakeInitial(1),
				CharaPackData.MakeInitial(TutorialUtil.TutorialGuestCharaId)
			};
			DataManager.DmChara.UpdateUserDataByTutorial(list11);
			List<MasterSkillPackData> list12 = new List<MasterSkillPackData> { MasterSkillPackData.MakeDummy(80001) };
			DataManager.DmChara.UpdateUserDataByTutorial(list12);
			DataManager.DmPhoto.UpdateUserDataByTutorial(new List<int> { TutorialUtil.TutorialPhotoId });
			List<CharaPromoteOne> list13 = new List<CharaPromoteOne>(DataManager.DmChara.GetCharaStaticData(1).promoteList[0].promoteOneList);
			list13.RemoveRange(1, list13.Count - 1);
			List<ItemInput> list14 = list13.ConvertAll<ItemInput>((CharaPromoteOne input) => new ItemInput
			{
				itemId = input.promoteUseItemId,
				num = input.promoteUseItemNum
			});
			int num = list13.Sum<CharaPromoteOne>((CharaPromoteOne item) => item.costGoldNum);
			list14.Add(new ItemInput(30101, num));
			list14.Add(new ItemInput(30001, 5));
			DataManager.DmItem.ClearUserDataByDirect();
			DataManager.DmItem.UpdateUserDataByDirect(list14, 1);
			return;
		}
		case TutorialUtil.Sequence.HELPER_SELECT_01:
		case TutorialUtil.Sequence.BATTLE_03:
		{
			List<HelperPackData> rentalHelperList2 = DataManager.DmHelper.GetRentalHelperList();
			rentalHelperList2.Clear();
			rentalHelperList2.Add(HelperPackData.MakeTutorialDummy(6, "ひよっこ隊長"));
			rentalHelperList2.Add(HelperPackData.MakeTutorialDummy(7, "かけだし隊長"));
			if (TutorialUtil.SelectHelperCharaId != 6 && TutorialUtil.SelectHelperCharaId != 7 && PlayerPrefs.HasKey(TutorialUtil.tutorialRestartKey))
			{
				TutorialUtil.SelectHelperCharaId = PlayerPrefs.GetInt(TutorialUtil.tutorialRestartKey);
			}
			if (TutorialUtil.SelectHelperCharaId != 6 && TutorialUtil.SelectHelperCharaId != 7)
			{
				TutorialUtil.SelectHelperCharaId = 6;
			}
			List<ItemInput> list15 = new List<ItemInput>();
			list15.Add(new ItemInput(30001, 5));
			DataManager.DmItem.UpdateUserDataByDirect(list15, 0);
			List<CharaPackData> list16 = new List<CharaPackData>
			{
				CharaPackData.MakeInitial(TutorialUtil.SelectHelperCharaId),
				CharaPackData.MakeInitial(22),
				CharaPackData.MakeInitial(1),
				CharaPackData.MakeInitial(TutorialUtil.TutorialGuestCharaId)
			};
			DataManager.DmChara.UpdateUserDataByTutorial(list16);
			List<MasterSkillPackData> list17 = new List<MasterSkillPackData> { MasterSkillPackData.MakeDummy(80001) };
			DataManager.DmChara.UpdateUserDataByTutorial(list17);
			DataManager.DmPhoto.UpdateUserDataByTutorial(new List<int> { TutorialUtil.TutorialPhotoId });
			DataManager.DmDeck.UpdateUserDataByTutorial(1, new List<int>
			{
				0,
				22,
				1,
				TutorialUtil.TutorialGuestCharaId,
				-1
			}, true, 80001);
			break;
		}
		default:
			return;
		}
	}

	public static void SetHelper(int helper)
	{
		TutorialUtil.SelectHelperCharaId = helper;
		if (helper > 0)
		{
			PlayerPrefs.SetInt(TutorialUtil.tutorialRestartKey, TutorialUtil.SelectHelperCharaId);
		}
	}

	private static readonly string tutorialRestartKey = "tutorialRestart";

	private static int SelectHelperCharaId = -1;

	public static readonly int TutorialPhotoId = 7001;

	public static readonly int TutorialGuestCharaId = 29;

	public enum Sequence
	{
		INVALID,
		FIRST,
		USER_SETTING,
		SCENARIO_00,
		BATTLE_00,
		SCENARIO_01,
		SCENARIO_02,
		BATTLE_01,
		SCENARIO_03,
		GACHA_PLAY,
		SCENARIO_04,
		PARTY_EDIT,
		HELPER_SELECT_00,
		BATTLE_02,
		SCENARIO_05,
		CHARA_GROW,
		HELPER_SELECT_01,
		SCENARIO_06,
		BATTLE_03,
		SCENARIO_07,
		DATA_RESET,
		OPENING_MOVIE,
		QUEST_GUIDE,
		END
	}
}
