using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C0 RID: 192
public class UserOptionData
{
	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06000872 RID: 2162 RVA: 0x00036FF9 File Offset: 0x000351F9
	// (set) Token: 0x06000873 RID: 2163 RVA: 0x00037001 File Offset: 0x00035201
	public Vector2Int DefaultScreen { get; set; } = Vector2Int.zero;

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x06000874 RID: 2164 RVA: 0x0003700A File Offset: 0x0003520A
	// (set) Token: 0x06000875 RID: 2165 RVA: 0x00037012 File Offset: 0x00035212
	public int DisplayDirection { get; set; }

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x06000876 RID: 2166 RVA: 0x0003701B File Offset: 0x0003521B
	// (set) Token: 0x06000877 RID: 2167 RVA: 0x00037023 File Offset: 0x00035223
	public int Quality { get; set; }

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x06000878 RID: 2168 RVA: 0x0003702C File Offset: 0x0003522C
	// (set) Token: 0x06000879 RID: 2169 RVA: 0x00037034 File Offset: 0x00035234
	public int FrameRate { get; set; }

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x0600087A RID: 2170 RVA: 0x0003703D File Offset: 0x0003523D
	// (set) Token: 0x0600087B RID: 2171 RVA: 0x00037045 File Offset: 0x00035245
	public bool PushNotifyStaminaMax { get; set; }

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x0600087C RID: 2172 RVA: 0x0003704E File Offset: 0x0003524E
	// (set) Token: 0x0600087D RID: 2173 RVA: 0x00037056 File Offset: 0x00035256
	public bool PushNotifyPvpStaminaMax { get; set; }

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x0600087E RID: 2174 RVA: 0x0003705F File Offset: 0x0003525F
	// (set) Token: 0x0600087F RID: 2175 RVA: 0x00037067 File Offset: 0x00035267
	public bool PushNotifyGenkiZero { get; set; }

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06000880 RID: 2176 RVA: 0x00037070 File Offset: 0x00035270
	// (set) Token: 0x06000881 RID: 2177 RVA: 0x00037078 File Offset: 0x00035278
	public bool PushNotifyInfomation { get; set; }

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06000882 RID: 2178 RVA: 0x00037081 File Offset: 0x00035281
	// (set) Token: 0x06000883 RID: 2179 RVA: 0x00037089 File Offset: 0x00035289
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

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06000884 RID: 2180 RVA: 0x0003709A File Offset: 0x0003529A
	// (set) Token: 0x06000885 RID: 2181 RVA: 0x000370A2 File Offset: 0x000352A2
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

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06000886 RID: 2182 RVA: 0x000370B3 File Offset: 0x000352B3
	// (set) Token: 0x06000887 RID: 2183 RVA: 0x000370BB File Offset: 0x000352BB
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

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06000888 RID: 2184 RVA: 0x000370CC File Offset: 0x000352CC
	// (set) Token: 0x06000889 RID: 2185 RVA: 0x000370E5 File Offset: 0x000352E5
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

	// Token: 0x0600088A RID: 2186 RVA: 0x000370E7 File Offset: 0x000352E7
	public static float[] GetVolumeList(int bgm, int se, int voice)
	{
		return new float[]
		{
			(float)bgm / 9f,
			(float)se / 9f,
			(float)voice / 9f
		};
	}

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x0600088B RID: 2187 RVA: 0x00037110 File Offset: 0x00035310
	// (set) Token: 0x0600088C RID: 2188 RVA: 0x00037118 File Offset: 0x00035318
	public bool secondScenarioSkip { get; set; }

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x0600088D RID: 2189 RVA: 0x00037121 File Offset: 0x00035321
	// (set) Token: 0x0600088E RID: 2190 RVA: 0x00037129 File Offset: 0x00035329
	public int ScenarioSpeed { get; set; }

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x0600088F RID: 2191 RVA: 0x00037132 File Offset: 0x00035332
	// (set) Token: 0x06000890 RID: 2192 RVA: 0x0003713A File Offset: 0x0003533A
	public int autoSpeed { get; set; }

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06000891 RID: 2193 RVA: 0x00037143 File Offset: 0x00035343
	// (set) Token: 0x06000892 RID: 2194 RVA: 0x0003714B File Offset: 0x0003534B
	public bool ViewPhotoAffect { get; set; }

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06000893 RID: 2195 RVA: 0x00037154 File Offset: 0x00035354
	// (set) Token: 0x06000894 RID: 2196 RVA: 0x0003715C File Offset: 0x0003535C
	public bool ViewClothesAffect { get; set; }

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x06000895 RID: 2197 RVA: 0x00037165 File Offset: 0x00035365
	// (set) Token: 0x06000896 RID: 2198 RVA: 0x0003716D File Offset: 0x0003536D
	public int MissionProgressNotify { get; set; }

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x06000897 RID: 2199 RVA: 0x00037176 File Offset: 0x00035376
	// (set) Token: 0x06000898 RID: 2200 RVA: 0x0003717E File Offset: 0x0003537E
	public List<bool> battleAutoFlag { get; set; }

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x06000899 RID: 2201 RVA: 0x00037187 File Offset: 0x00035387
	// (set) Token: 0x0600089A RID: 2202 RVA: 0x0003718F File Offset: 0x0003538F
	public List<bool> battleSpeedFlag { get; set; }

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x0600089B RID: 2203 RVA: 0x00037198 File Offset: 0x00035398
	// (set) Token: 0x0600089C RID: 2204 RVA: 0x000372BC File Offset: 0x000354BC
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

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x0600089D RID: 2205 RVA: 0x000372C5 File Offset: 0x000354C5
	// (set) Token: 0x0600089E RID: 2206 RVA: 0x000372CD File Offset: 0x000354CD
	public int photoIconSizeAllView { get; set; }

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x0600089F RID: 2207 RVA: 0x000372D6 File Offset: 0x000354D6
	// (set) Token: 0x060008A0 RID: 2208 RVA: 0x000372DE File Offset: 0x000354DE
	public int photoIconSizeGrow { get; set; }

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x060008A1 RID: 2209 RVA: 0x000372E7 File Offset: 0x000354E7
	// (set) Token: 0x060008A2 RID: 2210 RVA: 0x000372EF File Offset: 0x000354EF
	public int photoIconSizeSelectMaterial { get; set; }

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x060008A3 RID: 2211 RVA: 0x000372F8 File Offset: 0x000354F8
	// (set) Token: 0x060008A4 RID: 2212 RVA: 0x00037300 File Offset: 0x00035500
	public int photoIconSizeSell { get; set; }

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x060008A5 RID: 2213 RVA: 0x00037309 File Offset: 0x00035509
	// (set) Token: 0x060008A6 RID: 2214 RVA: 0x00037311 File Offset: 0x00035511
	public int photoIconSizePartyEdit { get; set; }

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x060008A7 RID: 2215 RVA: 0x0003731A File Offset: 0x0003551A
	// (set) Token: 0x060008A8 RID: 2216 RVA: 0x00037322 File Offset: 0x00035522
	public int photoIconSizeAsistant { get; set; }

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x060008A9 RID: 2217 RVA: 0x0003732B File Offset: 0x0003552B
	// (set) Token: 0x060008AA RID: 2218 RVA: 0x00037333 File Offset: 0x00035533
	public int photoIconSizePhotoAlbum { get; set; }

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x060008AB RID: 2219 RVA: 0x0003733C File Offset: 0x0003553C
	// (set) Token: 0x060008AC RID: 2220 RVA: 0x00037344 File Offset: 0x00035544
	public int CurrentQuestParty { get; set; }

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x060008AD RID: 2221 RVA: 0x0003734D File Offset: 0x0003554D
	// (set) Token: 0x060008AE RID: 2222 RVA: 0x00037355 File Offset: 0x00035555
	public int CurrentPvpParty { get; set; }

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x060008AF RID: 2223 RVA: 0x0003735E File Offset: 0x0003555E
	// (set) Token: 0x060008B0 RID: 2224 RVA: 0x00037366 File Offset: 0x00035566
	public int CurrentSpPvpParty { get; set; }

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x060008B1 RID: 2225 RVA: 0x0003736F File Offset: 0x0003556F
	// (set) Token: 0x060008B2 RID: 2226 RVA: 0x00037377 File Offset: 0x00035577
	public int StayFriendsId { get; set; }

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x060008B3 RID: 2227 RVA: 0x00037380 File Offset: 0x00035580
	// (set) Token: 0x060008B4 RID: 2228 RVA: 0x00037388 File Offset: 0x00035588
	public bool Pvp3x { get; set; }

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x060008B5 RID: 2229 RVA: 0x00037391 File Offset: 0x00035591
	// (set) Token: 0x060008B6 RID: 2230 RVA: 0x00037399 File Offset: 0x00035599
	public bool LoginBonusFriends { get; set; }

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x060008B7 RID: 2231 RVA: 0x000373A2 File Offset: 0x000355A2
	// (set) Token: 0x060008B8 RID: 2232 RVA: 0x000373AA File Offset: 0x000355AA
	public int LastPlaySpPvpSeasonId { get; set; }

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x060008B9 RID: 2233 RVA: 0x000373B3 File Offset: 0x000355B3
	// (set) Token: 0x060008BA RID: 2234 RVA: 0x000373BB File Offset: 0x000355BB
	public int CurrentSpPvpDifficultyTab { get; set; }

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x060008BB RID: 2235 RVA: 0x000373C4 File Offset: 0x000355C4
	// (set) Token: 0x060008BC RID: 2236 RVA: 0x000373CC File Offset: 0x000355CC
	public bool SpPvp3x { get; set; }

	// Token: 0x060008BD RID: 2237 RVA: 0x000373D8 File Offset: 0x000355D8
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

	// Token: 0x060008BE RID: 2238 RVA: 0x00037598 File Offset: 0x00035798
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

	// Token: 0x060008BF RID: 2239 RVA: 0x00037AD0 File Offset: 0x00035CD0
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

	// Token: 0x060008C0 RID: 2240 RVA: 0x00037EE4 File Offset: 0x000360E4
	public void SetDisplayQuality()
	{
		UserOptionData.SetDisplayQuality(this.Quality, this.DefaultScreen);
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x00037EF7 File Offset: 0x000360F7
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

	// Token: 0x060008C2 RID: 2242 RVA: 0x00037F35 File Offset: 0x00036135
	public void SetFrameRate()
	{
		Application.targetFrameRate = ((this.FrameRate == 0) ? 30 : 60);
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x00037F4A File Offset: 0x0003614A
	private static void SetDisplayResolution(bool isFull, Vector2Int siz)
	{
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x00037F4C File Offset: 0x0003614C
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

	// Token: 0x04000726 RID: 1830
	private const int VolumeMax = 9;

	// Token: 0x04000727 RID: 1831
	private int volumeBGM;

	// Token: 0x04000728 RID: 1832
	private int volumeSE;

	// Token: 0x04000729 RID: 1833
	private int volumeVOICE;

	// Token: 0x04000732 RID: 1842
	private List<int> _lastPlayQuestOneIdList;

	// Token: 0x04000733 RID: 1843
	public int LastPlayQuestOneIdByMainScenario;

	// Token: 0x020007B5 RID: 1973
	public enum ServerDataIndex
	{
		// Token: 0x04003433 RID: 13363
		DISPLAY_DIRECTION,
		// Token: 0x04003434 RID: 13364
		QUALITY,
		// Token: 0x04003435 RID: 13365
		PUSH_NOTIFY_STAMINA_MAX,
		// Token: 0x04003436 RID: 13366
		PUSH_NOTIFY_PVP_STAMINA_MAX,
		// Token: 0x04003437 RID: 13367
		PUSH_NOTIFY_GENKI_ZERO,
		// Token: 0x04003438 RID: 13368
		PUSH_NOTIFY_INFORMATION,
		// Token: 0x04003439 RID: 13369
		VOLUME_BGM,
		// Token: 0x0400343A RID: 13370
		VOLUME_SE,
		// Token: 0x0400343B RID: 13371
		VOLUME_VOICE,
		// Token: 0x0400343C RID: 13372
		SCENARIO_SPEED,
		// Token: 0x0400343D RID: 13373
		SECOND_SCENARIO_SKIP,
		// Token: 0x0400343E RID: 13374
		AUTO_SPEED,
		// Token: 0x0400343F RID: 13375
		VIEW_PHOTO_AFFECT,
		// Token: 0x04003440 RID: 13376
		VIEW_CLOTHES_AFFECT,
		// Token: 0x04003441 RID: 13377
		VIEW_MISSION_AFFECT,
		// Token: 0x04003442 RID: 13378
		BATTLE_AUTO_STORY,
		// Token: 0x04003443 RID: 13379
		BATTLE_AUTO_GROW,
		// Token: 0x04003444 RID: 13380
		BATTLE_AUTO_CHARA,
		// Token: 0x04003445 RID: 13381
		BATTLE_AUTO_PVP,
		// Token: 0x04003446 RID: 13382
		BATTLE_AUTO_EVENT,
		// Token: 0x04003447 RID: 13383
		BATTLE_SPEED_STORRY,
		// Token: 0x04003448 RID: 13384
		BATTLE_SPEED_GROW,
		// Token: 0x04003449 RID: 13385
		BATTLE_SPEED_CHARA,
		// Token: 0x0400344A RID: 13386
		BATTLE_SPEED_PVP,
		// Token: 0x0400344B RID: 13387
		BATTLE_SPEED_EVENT,
		// Token: 0x0400344C RID: 13388
		BATTLE_AUTO_SIDE_STORY,
		// Token: 0x0400344D RID: 13389
		BATTLE_SPEED_SIDE_STORY,
		// Token: 0x0400344E RID: 13390
		LAST_PLAY_QUEST_ONE_STORY,
		// Token: 0x0400344F RID: 13391
		LAST_PLAY_QUEST_ONE_GROW,
		// Token: 0x04003450 RID: 13392
		LAST_PLAY_QUEST_ONE_CHARA,
		// Token: 0x04003451 RID: 13393
		LAST_PLAY_QUEST_ONE_PVP,
		// Token: 0x04003452 RID: 13394
		LAST_PLAY_QUEST_ONE_EVENT,
		// Token: 0x04003453 RID: 13395
		LAST_PLAY_QUEST_ONE_SIDE_STORY,
		// Token: 0x04003454 RID: 13396
		PHOTO_ICON_SIZE_ALLVIEW,
		// Token: 0x04003455 RID: 13397
		PHOTO_ICON_SIZE_GROW,
		// Token: 0x04003456 RID: 13398
		PHOTO_ICON_SIZE_SELECT_MATERIAL,
		// Token: 0x04003457 RID: 13399
		PHOTO_ICON_SIZE_SELL,
		// Token: 0x04003458 RID: 13400
		PHOTO_ICON_SIZE_PARTY_EDIT,
		// Token: 0x04003459 RID: 13401
		PHOTO_ICON_SIZE_ASSISTANT,
		// Token: 0x0400345A RID: 13402
		CURRENT_QUEST_PARTY,
		// Token: 0x0400345B RID: 13403
		CURRENT_PVP_PARTY,
		// Token: 0x0400345C RID: 13404
		STAY_FRIENDS,
		// Token: 0x0400345D RID: 13405
		BATTLE_AUTO_TRAINING,
		// Token: 0x0400345E RID: 13406
		BATTLE_SPEED_TRAINING,
		// Token: 0x0400345F RID: 13407
		PVP_3X,
		// Token: 0x04003460 RID: 13408
		LOGIN_BONUS_FRIENDS,
		// Token: 0x04003461 RID: 13409
		PHOTO_ICON_SIZE_PHOTOALUBUM,
		// Token: 0x04003462 RID: 13410
		LAST_PLAY_QUEST_ONE_CELLVAL,
		// Token: 0x04003463 RID: 13411
		CURRENT_SP_PVP_PARTY,
		// Token: 0x04003464 RID: 13412
		LAST_PLAY_QUEST_ONE_MAIN_SCENARIO,
		// Token: 0x04003465 RID: 13413
		BATTLE_AUTO_ETCETERA,
		// Token: 0x04003466 RID: 13414
		BATTLE_SPEED_ETCETERA,
		// Token: 0x04003467 RID: 13415
		LAST_PLAY_QUEST_ONE_ETCETERA,
		// Token: 0x04003468 RID: 13416
		LAST_PLAY_SP_PVP_SEASON_ID,
		// Token: 0x04003469 RID: 13417
		CURRENT_SP_PVP_DIFFICULTY_TAB,
		// Token: 0x0400346A RID: 13418
		LAST_PLAY_QUEST_ONE_STORY2,
		// Token: 0x0400346B RID: 13419
		SP_PVP_3X,
		// Token: 0x0400346C RID: 13420
		LAST_PLAY_QUEST_ONE_STORY3,
		// Token: 0x0400346D RID: 13421
		FRAME_RATE,
		// Token: 0x0400346E RID: 13422
		MAX
	}
}
