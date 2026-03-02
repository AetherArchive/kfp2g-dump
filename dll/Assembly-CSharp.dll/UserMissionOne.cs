using System;
using ExtendedEnum;
using SGNFW.HttpRequest.Protocol;

// Token: 0x02000094 RID: 148
public class UserMissionOne
{
	// Token: 0x1700011A RID: 282
	// (get) Token: 0x060005E1 RID: 1505 RVA: 0x00028548 File Offset: 0x00026748
	// (set) Token: 0x060005E2 RID: 1506 RVA: 0x00028550 File Offset: 0x00026750
	public string missionContents { get; set; }

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x060005E3 RID: 1507 RVA: 0x00028559 File Offset: 0x00026759
	public bool isClear
	{
		get
		{
			return this.denominator <= this.numerator;
		}
	}

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x060005E4 RID: 1508 RVA: 0x0002856C File Offset: 0x0002676C
	// (set) Token: 0x060005E5 RID: 1509 RVA: 0x00028574 File Offset: 0x00026774
	public bool Received { get; set; }

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x060005E6 RID: 1510 RVA: 0x0002857D File Offset: 0x0002677D
	// (set) Token: 0x060005E7 RID: 1511 RVA: 0x00028585 File Offset: 0x00026785
	public bool IsSpecial { get; set; }

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x060005E8 RID: 1512 RVA: 0x0002858E File Offset: 0x0002678E
	// (set) Token: 0x060005E9 RID: 1513 RVA: 0x00028596 File Offset: 0x00026796
	public SceneManager.SceneName transitionScene { get; private set; }

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x060005EA RID: 1514 RVA: 0x0002859F File Offset: 0x0002679F
	// (set) Token: 0x060005EB RID: 1515 RVA: 0x000285A7 File Offset: 0x000267A7
	public int transitionId { get; private set; }

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x060005EC RID: 1516 RVA: 0x000285B0 File Offset: 0x000267B0
	// (set) Token: 0x060005ED RID: 1517 RVA: 0x000285B8 File Offset: 0x000267B8
	public int relType { get; private set; }

	// Token: 0x060005EE RID: 1518 RVA: 0x000285C1 File Offset: 0x000267C1
	public ItemData GetRewardItemData()
	{
		return new ItemData(this.rewardItemId, this.rewardItenNum);
	}

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x060005EF RID: 1519 RVA: 0x000285D4 File Offset: 0x000267D4
	public AcceptMission AcceptMission
	{
		get
		{
			return new AcceptMission
			{
				mission_id = this.missionId,
				mission_datetime = this.keyServerTime
			};
		}
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x000285F3 File Offset: 0x000267F3
	public bool CompareAcceptMission(AcceptMission acceptMission)
	{
		return this.missionId == acceptMission.mission_id && this.keyServerTime == acceptMission.mission_datetime;
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x00028614 File Offset: 0x00026814
	public UserMissionOne(DataManagerMission.StaticMissionData mst, Mission mission, bool noClearForce)
	{
		this.missionId = mst.MissionId;
		this.missionContents = mst.MissionContents;
		this.denominator = mst.Denominator;
		this.sortNum = mst.SortNum;
		this.alwaysDispFlg = mst.AlwaysDispFlg;
		this.rewardItemId = mst.RewardItemId;
		this.rewardItenNum = mst.RewardItemNum;
		this.needAcceptMissionId = mst.NeedMissionId;
		this.keyServerTime = mission.mission_datetime;
		this.transitionScene = mst.TransitionScene;
		this.transitionId = mst.TransitionId;
		this.relType = mst.RelType;
		if (!noClearForce)
		{
			this.numerator = mission.mission_status;
			this.Received = 1 == mission.accept_flg;
		}
		else
		{
			this.numerator = 0;
			this.Received = false;
		}
		this.IsSpecial = mst.IsSpecial;
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x000286F1 File Offset: 0x000268F1
	public void Update(Mission mission)
	{
		this.numerator = mission.mission_status;
		this.keyServerTime = mission.mission_datetime;
	}

	// Token: 0x040005BB RID: 1467
	public int missionId;

	// Token: 0x040005BD RID: 1469
	public int sortNum;

	// Token: 0x040005BE RID: 1470
	public int numerator;

	// Token: 0x040005BF RID: 1471
	public int denominator;

	// Token: 0x040005C0 RID: 1472
	public int rewardItemId;

	// Token: 0x040005C1 RID: 1473
	public int rewardItenNum;

	// Token: 0x040005C3 RID: 1475
	public int needAcceptMissionId;

	// Token: 0x040005C4 RID: 1476
	public bool alwaysDispFlg;

	// Token: 0x040005C6 RID: 1478
	public long keyServerTime;

	// Token: 0x02000703 RID: 1795
	public enum DefaultTransition
	{
		// Token: 0x04003193 RID: 12691
		[EnumKeyValue(301, SceneManager.SceneName.SceneProfile)]
		PLAYER_COMMENT_CHANGE_COUNT = 301,
		// Token: 0x04003194 RID: 12692
		[EnumKeyValue(302, SceneManager.SceneName.SceneProfile)]
		ASSISTANT_SET_COUNT,
		// Token: 0x04003195 RID: 12693
		[EnumKeyValue(303, SceneManager.SceneName.SceneAccountTransfer)]
		TRANSFER_PASSWORD_SET,
		// Token: 0x04003196 RID: 12694
		[EnumKeyValue(304, SceneManager.SceneName.SceneFriend)]
		FOLLOW_NUM,
		// Token: 0x04003197 RID: 12695
		[EnumKeyValue(306, SceneManager.SceneName.SceneProfile)]
		HELPER_SET_COUNT = 306,
		// Token: 0x04003198 RID: 12696
		[EnumKeyValue(400, SceneManager.SceneName.SceneQuest)]
		QUEST_CLEAR_COUNT = 400,
		// Token: 0x04003199 RID: 12697
		[EnumKeyValue(401, SceneManager.SceneName.SceneQuest)]
		QUEST_CLEAR,
		// Token: 0x0400319A RID: 12698
		[EnumKeyValue(402, SceneManager.SceneName.SceneQuest)]
		QUEST_CLEAR_WITH_CLOTHES,
		// Token: 0x0400319B RID: 12699
		[EnumKeyValue(403, SceneManager.SceneName.SceneQuest)]
		QUEST_CLEAR_TEAM_ATTRIBUTE,
		// Token: 0x0400319C RID: 12700
		[EnumKeyValue(404, SceneManager.SceneName.SceneQuest)]
		QUEST_CLEAR_COUNT_IN_STAGE,
		// Token: 0x0400319D RID: 12701
		[EnumKeyValue(405, SceneManager.SceneName.SceneQuest)]
		QUEST_WITHDRAWAL_COUNT,
		// Token: 0x0400319E RID: 12702
		[EnumKeyValue(600, SceneManager.SceneName.SceneQuest)]
		PVP_CHALLENGE_COUNT = 600,
		// Token: 0x0400319F RID: 12703
		[EnumKeyValue(601, SceneManager.SceneName.SceneQuest)]
		PVP_RANK,
		// Token: 0x040031A0 RID: 12704
		[EnumKeyValue(602, SceneManager.SceneName.SceneQuest)]
		PVP_WIN_COUNT,
		// Token: 0x040031A1 RID: 12705
		[EnumKeyValue(800, SceneManager.SceneName.ScenePicnic)]
		PICNIC_GET_NUM = 800,
		// Token: 0x040031A2 RID: 12706
		[EnumKeyValue(900, SceneManager.SceneName.SceneGacha)]
		GACHA_COUNT = 900,
		// Token: 0x040031A3 RID: 12707
		[EnumKeyValue(1000, SceneManager.SceneName.SceneGacha)]
		CHARA_GET_NUM = 1000,
		// Token: 0x040031A4 RID: 12708
		[EnumKeyValue(1001, SceneManager.SceneName.SceneCharaEdit)]
		CHARA_BEAST_COUNT,
		// Token: 0x040031A5 RID: 12709
		[EnumKeyValue(1002, SceneManager.SceneName.SceneCharaEdit)]
		CHARA_ARTS_COUNT,
		// Token: 0x040031A6 RID: 12710
		[EnumKeyValue(1003, SceneManager.SceneName.SceneCharaEdit)]
		CHARA_PP_COUNT,
		// Token: 0x040031A7 RID: 12711
		[EnumKeyValue(1004, SceneManager.SceneName.SceneCharaEdit)]
		CHARA_STRENG_COUNT,
		// Token: 0x040031A8 RID: 12712
		[EnumKeyValue(1006, SceneManager.SceneName.SceneCharaEdit)]
		CHARA_RANK_COUNT = 1006,
		// Token: 0x040031A9 RID: 12713
		[EnumKeyValue(1101, SceneManager.SceneName.SceneCharaEdit)]
		PHOTO_STRENG_COUNT = 1101,
		// Token: 0x040031AA RID: 12714
		[EnumKeyValue(1200, SceneManager.SceneName.SceneGacha)]
		FURNITURE_GET_NUM = 1200,
		// Token: 0x040031AB RID: 12715
		[EnumKeyValue(1201, SceneManager.SceneName.SceneHome)]
		FURNITURE_SET_COUNT,
		// Token: 0x040031AC RID: 12716
		[EnumKeyValue(1301, SceneManager.SceneName.SceneCharaEdit)]
		CLOTH_SET_COUNT = 1301,
		// Token: 0x040031AD RID: 12717
		[EnumKeyValue(1400, SceneManager.SceneName.SceneQuest)]
		ITEM_GET_NUM = 1400,
		// Token: 0x040031AE RID: 12718
		[EnumKeyValue(1500, SceneManager.SceneName.SceneShop)]
		SHOP_BUY_COUNT = 1500,
		// Token: 0x040031AF RID: 12719
		[EnumKeyValue(1600, SceneManager.SceneName.SceneCharaEdit)]
		DECK_EDIT_COUNT = 1600,
		// Token: 0x040031B0 RID: 12720
		[EnumKeyValue(1800, SceneManager.SceneName.SceneTreeHouse)]
		MASTER_ROOM_SEND_STAMP_COUNT = 1800,
		// Token: 0x040031B1 RID: 12721
		[EnumKeyValue(1900, SceneManager.SceneName.SceneKemoBoard)]
		KEMOBOARD_OPEN_COUNT = 1900,
		// Token: 0x040031B2 RID: 12722
		[EnumKeyValue(2000, SceneManager.SceneName.SceneCharaEdit)]
		KEMOSTATUS = 2000,
		// Token: 0x040031B3 RID: 12723
		[EnumKeyValue(2100, SceneManager.SceneName.SceneAchievement)]
		ACHIEVEMENT_GET_NUM = 2100
	}
}
