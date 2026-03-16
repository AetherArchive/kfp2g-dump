using System;
using System.Collections.Generic;
using UnityEngine;

public class UserOptionData
{
	public Vector2Int DefaultScreen { get; set; } = Vector2Int.zero;

	public int DisplayDirection { get; set; }

	public int Quality { get; set; }

	public int FrameRate { get; set; }

	public bool PushNotifyStaminaMax { get; set; }

	public bool PushNotifyPvpStaminaMax { get; set; }

	public bool PushNotifyGenkiZero { get; set; }

	public bool PushNotifyInfomation { get; set; }

	public int VolumeBGM
	{
		get
		{
			return this.volumeBGM;
		}
		set
		{
			this.volumeBGM = Mathf.Clamp(value, 0, 9);
		}
	}

	public int VolumeSE
	{
		get
		{
			return this.volumeSE;
		}
		set
		{
			this.volumeSE = Mathf.Clamp(value, 0, 9);
		}
	}

	public int VolumeVOICE
	{
		get
		{
			return this.volumeVOICE;
		}
		set
		{
			this.volumeVOICE = Mathf.Clamp(value, 0, 9);
		}
	}

	public float[] VolumeList
	{
		get
		{
			return UserOptionData.GetVolumeList(this.volumeBGM, this.volumeSE, this.volumeVOICE);
		}
		private set
		{
		}
	}

	public static float[] GetVolumeList(int bgm, int se, int voice)
	{
		return new float[]
		{
			(float)bgm / 9f,
			(float)se / 9f,
			(float)voice / 9f
		};
	}

	public bool secondScenarioSkip { get; set; }

	public int ScenarioSpeed { get; set; }

	public int autoSpeed { get; set; }

	public bool ViewPhotoAffect { get; set; }

	public bool ViewClothesAffect { get; set; }

	public int MissionProgressNotify { get; set; }

	public List<bool> battleAutoFlag { get; set; }

	public List<bool> battleSpeedFlag { get; set; }

	public List<int> LastPlayQuestOneIdList
	{
		get
		{
			if (this._lastPlayQuestOneIdList == null)
			{
				return new List<int>();
			}
			if (DataManager.DmQuest.QuestStaticData == null)
			{
				return new List<int>();
			}
			List<int> list = new List<int>();
			foreach (int num in this._lastPlayQuestOneIdList)
			{
				int num2 = 0;
				if (DataManager.DmQuest.QuestStaticData.oneDataMap.ContainsKey(num))
				{
					QuestStaticQuestOne questStaticQuestOne = DataManager.DmQuest.QuestStaticData.oneDataMap[num];
					if (DataManager.DmQuest.QuestStaticData.groupDataMap.ContainsKey(questStaticQuestOne.questGroupId))
					{
						QuestStaticQuestGroup questStaticQuestGroup = DataManager.DmQuest.QuestStaticData.groupDataMap[questStaticQuestOne.questGroupId];
						if (questStaticQuestGroup.startDatetime <= TimeManager.Now && TimeManager.Now < questStaticQuestGroup.endDatetime)
						{
							num2 = questStaticQuestOne.questId;
						}
					}
				}
				list.Add(num2);
			}
			this._lastPlayQuestOneIdList = list;
			return this._lastPlayQuestOneIdList;
		}
		set
		{
			this._lastPlayQuestOneIdList = value;
		}
	}

	public int photoIconSizeAllView { get; set; }

	public int photoIconSizeGrow { get; set; }

	public int photoIconSizeSelectMaterial { get; set; }

	public int photoIconSizeSell { get; set; }

	public int photoIconSizePartyEdit { get; set; }

	public int photoIconSizeAsistant { get; set; }

	public int photoIconSizePhotoAlbum { get; set; }

	public int CurrentQuestParty { get; set; }

	public int CurrentPvpParty { get; set; }

	public int CurrentSpPvpParty { get; set; }

	public int StayFriendsId { get; set; }

	public bool Pvp3x { get; set; }

	public bool LoginBonusFriends { get; set; }

	public int LastPlaySpPvpSeasonId { get; set; }

	public int CurrentSpPvpDifficultyTab { get; set; }

	public bool SpPvp3x { get; set; }

	public void SetupDefault()
	{
		if (Vector2Int.zero == this.DefaultScreen)
		{
			this.DefaultScreen = new Vector2Int(SceneManager.screenSize.width, SceneManager.screenSize.height);
		}
		this.DisplayDirection = 0;
		this.Quality = 2;
		this.FrameRate = 0;
		this.PushNotifyStaminaMax = true;
		this.PushNotifyPvpStaminaMax = true;
		this.PushNotifyGenkiZero = true;
		this.PushNotifyInfomation = true;
		this.VolumeBGM = 6;
		this.VolumeSE = 6;
		this.VolumeVOICE = 6;
		this.ScenarioSpeed = 0;
		this.secondScenarioSkip = true;
		this.autoSpeed = 0;
		this.ViewPhotoAffect = true;
		this.ViewClothesAffect = true;
		this.MissionProgressNotify = 0;
		this.battleAutoFlag = new List<bool>();
		this.battleSpeedFlag = new List<bool>();
		this.LastPlayQuestOneIdList = new List<int>();
		for (int i = 0; i < Enum.GetNames(typeof(QuestStaticChapter.Category)).Length; i++)
		{
			this.battleAutoFlag.Add(false);
			this.battleSpeedFlag.Add(false);
			this.LastPlayQuestOneIdList.Add(0);
		}
		this.photoIconSizeAllView = 3;
		this.photoIconSizeGrow = 3;
		this.photoIconSizeSelectMaterial = 3;
		this.photoIconSizeSell = 3;
		this.photoIconSizePartyEdit = 3;
		this.photoIconSizeAsistant = 3;
		this.photoIconSizePhotoAlbum = 3;
		this.CurrentQuestParty = 1;
		this.CurrentPvpParty = 101;
		this.CurrentSpPvpParty = 101;
		this.StayFriendsId = 0;
		this.Pvp3x = false;
		this.LoginBonusFriends = false;
		this.LastPlayQuestOneIdByMainScenario = 0;
		this.LastPlaySpPvpSeasonId = 0;
		this.CurrentSpPvpDifficultyTab = 1;
		this.SpPvp3x = false;
		SceneManager.SetOption(new int[] { this.DisplayDirection, this.Quality, this.VolumeBGM, this.VolumeSE, this.VolumeVOICE });
	}

	public void UpdateByServerData(List<int> optionList)
	{
		if (optionList != null && optionList.Count > 0)
		{
			if (optionList.FindAll((int item) => item != 0).Count > 0)
			{
				if (optionList.Count < 59)
				{
					this.SetupDefault();
					return;
				}
				if (Vector2Int.zero == this.DefaultScreen)
				{
					this.DefaultScreen = new Vector2Int(SceneManager.screenSize.width, SceneManager.screenSize.height);
				}
				this.DisplayDirection = optionList[0];
				this.Quality = optionList[1];
				this.PushNotifyStaminaMax = optionList[2] != 0;
				this.PushNotifyPvpStaminaMax = optionList[3] != 0;
				this.PushNotifyGenkiZero = optionList[4] != 0;
				this.PushNotifyInfomation = optionList[5] != 0;
				this.VolumeBGM = optionList[6];
				this.VolumeSE = optionList[7];
				this.VolumeVOICE = optionList[8];
				this.ScenarioSpeed = optionList[9];
				this.secondScenarioSkip = optionList[10] != 0;
				this.autoSpeed = optionList[11];
				this.ViewPhotoAffect = optionList[12] != 0;
				this.ViewClothesAffect = optionList[13] != 0;
				this.MissionProgressNotify = optionList[14];
				this.battleAutoFlag = new List<bool>
				{
					false,
					optionList[15] != 0,
					optionList[16] != 0,
					optionList[17] != 0,
					optionList[18] != 0,
					optionList[19] != 0,
					optionList[25] != 0,
					false,
					optionList[42] != 0,
					optionList[15] != 0,
					false,
					optionList[50] != 0,
					optionList[15] != 0,
					optionList[15] != 0
				};
				this.battleSpeedFlag = new List<bool>
				{
					false,
					optionList[20] != 0,
					optionList[21] != 0,
					optionList[22] != 0,
					optionList[23] != 0,
					optionList[24] != 0,
					optionList[26] != 0,
					false,
					optionList[43] != 0,
					optionList[20] != 0,
					false,
					optionList[51] != 0,
					optionList[20] != 0,
					optionList[20] != 0
				};
				this.LastPlayQuestOneIdList = new List<int>
				{
					0,
					optionList[27],
					optionList[28],
					optionList[29],
					optionList[30],
					optionList[31],
					optionList[32],
					0,
					0,
					optionList[47],
					0,
					optionList[52],
					optionList[55],
					optionList[57]
				};
				this.photoIconSizeAllView = optionList[33];
				this.photoIconSizeGrow = optionList[34];
				this.photoIconSizeSelectMaterial = optionList[35];
				this.photoIconSizeSell = optionList[36];
				this.photoIconSizePartyEdit = optionList[37];
				this.photoIconSizeAsistant = optionList[38];
				this.photoIconSizePhotoAlbum = optionList[46];
				this.CurrentQuestParty = optionList[39];
				this.CurrentPvpParty = optionList[40];
				this.CurrentSpPvpParty = optionList[48];
				if (this.CurrentQuestParty < 1)
				{
					this.CurrentQuestParty = 1;
				}
				if (this.CurrentPvpParty < 101)
				{
					this.CurrentPvpParty = 101;
				}
				this.StayFriendsId = optionList[41];
				this.Pvp3x = optionList[44] != 0;
				this.LoginBonusFriends = optionList[45] != 0;
				this.LastPlayQuestOneIdByMainScenario = optionList[49];
				this.LastPlaySpPvpSeasonId = optionList[53];
				this.CurrentSpPvpDifficultyTab = optionList[54];
				this.SpPvp3x = optionList[56] != 0;
				this.FrameRate = optionList[58];
				SceneManager.SetOption(new int[] { this.DisplayDirection, this.Quality, this.VolumeBGM, this.VolumeSE, this.VolumeVOICE });
				return;
			}
		}
		this.SetupDefault();
	}

	public List<int> CreateByServerData()
	{
		return new List<int>
		{
			this.DisplayDirection,
			this.Quality,
			this.PushNotifyStaminaMax ? 1 : 0,
			this.PushNotifyPvpStaminaMax ? 1 : 0,
			this.PushNotifyGenkiZero ? 1 : 0,
			this.PushNotifyInfomation ? 1 : 0,
			this.VolumeBGM,
			this.VolumeSE,
			this.VolumeVOICE,
			this.ScenarioSpeed,
			this.secondScenarioSkip ? 1 : 0,
			this.autoSpeed,
			this.ViewPhotoAffect ? 1 : 0,
			this.ViewClothesAffect ? 1 : 0,
			this.MissionProgressNotify,
			this.battleAutoFlag[1] ? 1 : 0,
			this.battleAutoFlag[2] ? 1 : 0,
			this.battleAutoFlag[3] ? 1 : 0,
			this.battleAutoFlag[4] ? 1 : 0,
			this.battleAutoFlag[5] ? 1 : 0,
			this.battleSpeedFlag[1] ? 1 : 0,
			this.battleSpeedFlag[2] ? 1 : 0,
			this.battleSpeedFlag[3] ? 1 : 0,
			this.battleSpeedFlag[4] ? 1 : 0,
			this.battleSpeedFlag[5] ? 1 : 0,
			this.battleAutoFlag[6] ? 1 : 0,
			this.battleSpeedFlag[6] ? 1 : 0,
			this.LastPlayQuestOneIdList[1],
			this.LastPlayQuestOneIdList[2],
			this.LastPlayQuestOneIdList[3],
			this.LastPlayQuestOneIdList[4],
			this.LastPlayQuestOneIdList[5],
			this.LastPlayQuestOneIdList[6],
			this.photoIconSizeAllView,
			this.photoIconSizeGrow,
			this.photoIconSizeSelectMaterial,
			this.photoIconSizeSell,
			this.photoIconSizePartyEdit,
			this.photoIconSizeAsistant,
			this.CurrentQuestParty,
			this.CurrentPvpParty,
			this.StayFriendsId,
			this.battleAutoFlag[8] ? 1 : 0,
			this.battleSpeedFlag[8] ? 1 : 0,
			this.Pvp3x ? 1 : 0,
			this.LoginBonusFriends ? 1 : 0,
			this.photoIconSizePhotoAlbum,
			this.LastPlayQuestOneIdList[9],
			this.CurrentSpPvpParty,
			this.LastPlayQuestOneIdByMainScenario,
			this.battleAutoFlag[11] ? 1 : 0,
			this.battleSpeedFlag[11] ? 1 : 0,
			this.LastPlayQuestOneIdList[11],
			this.LastPlaySpPvpSeasonId,
			this.CurrentSpPvpDifficultyTab,
			this.LastPlayQuestOneIdList[12],
			this.SpPvp3x ? 1 : 0,
			this.LastPlayQuestOneIdList[13],
			this.FrameRate
		};
	}

	public void SetDisplayQuality()
	{
		UserOptionData.SetDisplayQuality(this.Quality, this.DefaultScreen);
	}

	public static void SetDisplayQuality(int qrty, Vector2Int siz)
	{
		switch (qrty)
		{
		case 0:
			UserOptionData.SetDisplayResolution(false, siz);
			QualitySettings.antiAliasing = 1;
			return;
		case 1:
			UserOptionData.SetDisplayResolution(true, siz);
			QualitySettings.antiAliasing = 1;
			return;
		case 2:
			UserOptionData.SetDisplayResolution(true, siz);
			QualitySettings.antiAliasing = 2;
			return;
		default:
			return;
		}
	}

	public void SetFrameRate()
	{
		Application.targetFrameRate = ((this.FrameRate == 0) ? 30 : 60);
	}

	private static void SetDisplayResolution(bool isFull, Vector2Int siz)
	{
	}

	public UserOptionData Clone()
	{
		return new UserOptionData
		{
			DefaultScreen = this.DefaultScreen,
			DisplayDirection = this.DisplayDirection,
			Quality = this.Quality,
			FrameRate = this.FrameRate,
			PushNotifyStaminaMax = this.PushNotifyStaminaMax,
			PushNotifyPvpStaminaMax = this.PushNotifyPvpStaminaMax,
			PushNotifyGenkiZero = this.PushNotifyGenkiZero,
			PushNotifyInfomation = this.PushNotifyInfomation,
			volumeBGM = this.volumeBGM,
			volumeSE = this.volumeSE,
			volumeVOICE = this.volumeVOICE,
			ScenarioSpeed = this.ScenarioSpeed,
			secondScenarioSkip = this.secondScenarioSkip,
			autoSpeed = this.autoSpeed,
			ViewPhotoAffect = this.ViewPhotoAffect,
			ViewClothesAffect = this.ViewClothesAffect,
			MissionProgressNotify = this.MissionProgressNotify,
			battleAutoFlag = new List<bool>(this.battleAutoFlag),
			battleSpeedFlag = new List<bool>(this.battleSpeedFlag),
			LastPlayQuestOneIdList = new List<int>(this.LastPlayQuestOneIdList),
			photoIconSizeAllView = this.photoIconSizeAllView,
			photoIconSizeGrow = this.photoIconSizeGrow,
			photoIconSizeSelectMaterial = this.photoIconSizeSelectMaterial,
			photoIconSizeSell = this.photoIconSizeSell,
			photoIconSizePartyEdit = this.photoIconSizePartyEdit,
			photoIconSizeAsistant = this.photoIconSizeAsistant,
			photoIconSizePhotoAlbum = this.photoIconSizePhotoAlbum,
			CurrentQuestParty = this.CurrentQuestParty,
			CurrentPvpParty = this.CurrentPvpParty,
			CurrentSpPvpParty = this.CurrentSpPvpParty,
			StayFriendsId = this.StayFriendsId,
			Pvp3x = this.Pvp3x,
			LoginBonusFriends = this.LoginBonusFriends,
			LastPlayQuestOneIdByMainScenario = this.LastPlayQuestOneIdByMainScenario,
			LastPlaySpPvpSeasonId = this.LastPlaySpPvpSeasonId,
			CurrentSpPvpDifficultyTab = this.CurrentSpPvpDifficultyTab,
			SpPvp3x = this.SpPvp3x
		};
	}

	private const int VolumeMax = 9;

	private int volumeBGM;

	private int volumeSE;

	private int volumeVOICE;

	private List<int> _lastPlayQuestOneIdList;

	public int LastPlayQuestOneIdByMainScenario;

	public enum ServerDataIndex
	{
		DISPLAY_DIRECTION,
		QUALITY,
		PUSH_NOTIFY_STAMINA_MAX,
		PUSH_NOTIFY_PVP_STAMINA_MAX,
		PUSH_NOTIFY_GENKI_ZERO,
		PUSH_NOTIFY_INFORMATION,
		VOLUME_BGM,
		VOLUME_SE,
		VOLUME_VOICE,
		SCENARIO_SPEED,
		SECOND_SCENARIO_SKIP,
		AUTO_SPEED,
		VIEW_PHOTO_AFFECT,
		VIEW_CLOTHES_AFFECT,
		VIEW_MISSION_AFFECT,
		BATTLE_AUTO_STORY,
		BATTLE_AUTO_GROW,
		BATTLE_AUTO_CHARA,
		BATTLE_AUTO_PVP,
		BATTLE_AUTO_EVENT,
		BATTLE_SPEED_STORRY,
		BATTLE_SPEED_GROW,
		BATTLE_SPEED_CHARA,
		BATTLE_SPEED_PVP,
		BATTLE_SPEED_EVENT,
		BATTLE_AUTO_SIDE_STORY,
		BATTLE_SPEED_SIDE_STORY,
		LAST_PLAY_QUEST_ONE_STORY,
		LAST_PLAY_QUEST_ONE_GROW,
		LAST_PLAY_QUEST_ONE_CHARA,
		LAST_PLAY_QUEST_ONE_PVP,
		LAST_PLAY_QUEST_ONE_EVENT,
		LAST_PLAY_QUEST_ONE_SIDE_STORY,
		PHOTO_ICON_SIZE_ALLVIEW,
		PHOTO_ICON_SIZE_GROW,
		PHOTO_ICON_SIZE_SELECT_MATERIAL,
		PHOTO_ICON_SIZE_SELL,
		PHOTO_ICON_SIZE_PARTY_EDIT,
		PHOTO_ICON_SIZE_ASSISTANT,
		CURRENT_QUEST_PARTY,
		CURRENT_PVP_PARTY,
		STAY_FRIENDS,
		BATTLE_AUTO_TRAINING,
		BATTLE_SPEED_TRAINING,
		PVP_3X,
		LOGIN_BONUS_FRIENDS,
		PHOTO_ICON_SIZE_PHOTOALUBUM,
		LAST_PLAY_QUEST_ONE_CELLVAL,
		CURRENT_SP_PVP_PARTY,
		LAST_PLAY_QUEST_ONE_MAIN_SCENARIO,
		BATTLE_AUTO_ETCETERA,
		BATTLE_SPEED_ETCETERA,
		LAST_PLAY_QUEST_ONE_ETCETERA,
		LAST_PLAY_SP_PVP_SEASON_ID,
		CURRENT_SP_PVP_DIFFICULTY_TAB,
		LAST_PLAY_QUEST_ONE_STORY2,
		SP_PVP_3X,
		LAST_PLAY_QUEST_ONE_STORY3,
		FRAME_RATE,
		MAX
	}
}
