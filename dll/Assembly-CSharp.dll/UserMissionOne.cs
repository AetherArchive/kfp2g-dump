using System;
using ExtendedEnum;
using SGNFW.HttpRequest.Protocol;

public class UserMissionOne
{
	public string missionContents { get; set; }

	public bool isClear
	{
		get
		{
			return this.denominator <= this.numerator;
		}
	}

	public bool Received { get; set; }

	public bool IsSpecial { get; set; }

	public SceneManager.SceneName transitionScene { get; private set; }

	public int transitionId { get; private set; }

	public int relType { get; private set; }

	public ItemData GetRewardItemData()
	{
		return new ItemData(this.rewardItemId, this.rewardItenNum);
	}

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

	public bool CompareAcceptMission(AcceptMission acceptMission)
	{
		return this.missionId == acceptMission.mission_id && this.keyServerTime == acceptMission.mission_datetime;
	}

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

	public void Update(Mission mission)
	{
		this.numerator = mission.mission_status;
		this.keyServerTime = mission.mission_datetime;
	}

	public int missionId;

	public int sortNum;

	public int numerator;

	public int denominator;

	public int rewardItemId;

	public int rewardItenNum;

	public int needAcceptMissionId;

	public bool alwaysDispFlg;

	public long keyServerTime;

	public enum DefaultTransition
	{
		[EnumKeyValue(301, SceneManager.SceneName.SceneProfile)]
		PLAYER_COMMENT_CHANGE_COUNT = 301,
		[EnumKeyValue(302, SceneManager.SceneName.SceneProfile)]
		ASSISTANT_SET_COUNT,
		[EnumKeyValue(303, SceneManager.SceneName.SceneAccountTransfer)]
		TRANSFER_PASSWORD_SET,
		[EnumKeyValue(304, SceneManager.SceneName.SceneFriend)]
		FOLLOW_NUM,
		[EnumKeyValue(306, SceneManager.SceneName.SceneProfile)]
		HELPER_SET_COUNT = 306,
		[EnumKeyValue(400, SceneManager.SceneName.SceneQuest)]
		QUEST_CLEAR_COUNT = 400,
		[EnumKeyValue(401, SceneManager.SceneName.SceneQuest)]
		QUEST_CLEAR,
		[EnumKeyValue(402, SceneManager.SceneName.SceneQuest)]
		QUEST_CLEAR_WITH_CLOTHES,
		[EnumKeyValue(403, SceneManager.SceneName.SceneQuest)]
		QUEST_CLEAR_TEAM_ATTRIBUTE,
		[EnumKeyValue(404, SceneManager.SceneName.SceneQuest)]
		QUEST_CLEAR_COUNT_IN_STAGE,
		[EnumKeyValue(405, SceneManager.SceneName.SceneQuest)]
		QUEST_WITHDRAWAL_COUNT,
		[EnumKeyValue(600, SceneManager.SceneName.SceneQuest)]
		PVP_CHALLENGE_COUNT = 600,
		[EnumKeyValue(601, SceneManager.SceneName.SceneQuest)]
		PVP_RANK,
		[EnumKeyValue(602, SceneManager.SceneName.SceneQuest)]
		PVP_WIN_COUNT,
		[EnumKeyValue(800, SceneManager.SceneName.ScenePicnic)]
		PICNIC_GET_NUM = 800,
		[EnumKeyValue(900, SceneManager.SceneName.SceneGacha)]
		GACHA_COUNT = 900,
		[EnumKeyValue(1000, SceneManager.SceneName.SceneGacha)]
		CHARA_GET_NUM = 1000,
		[EnumKeyValue(1001, SceneManager.SceneName.SceneCharaEdit)]
		CHARA_BEAST_COUNT,
		[EnumKeyValue(1002, SceneManager.SceneName.SceneCharaEdit)]
		CHARA_ARTS_COUNT,
		[EnumKeyValue(1003, SceneManager.SceneName.SceneCharaEdit)]
		CHARA_PP_COUNT,
		[EnumKeyValue(1004, SceneManager.SceneName.SceneCharaEdit)]
		CHARA_STRENG_COUNT,
		[EnumKeyValue(1006, SceneManager.SceneName.SceneCharaEdit)]
		CHARA_RANK_COUNT = 1006,
		[EnumKeyValue(1101, SceneManager.SceneName.SceneCharaEdit)]
		PHOTO_STRENG_COUNT = 1101,
		[EnumKeyValue(1200, SceneManager.SceneName.SceneGacha)]
		FURNITURE_GET_NUM = 1200,
		[EnumKeyValue(1201, SceneManager.SceneName.SceneHome)]
		FURNITURE_SET_COUNT,
		[EnumKeyValue(1301, SceneManager.SceneName.SceneCharaEdit)]
		CLOTH_SET_COUNT = 1301,
		[EnumKeyValue(1400, SceneManager.SceneName.SceneQuest)]
		ITEM_GET_NUM = 1400,
		[EnumKeyValue(1500, SceneManager.SceneName.SceneShop)]
		SHOP_BUY_COUNT = 1500,
		[EnumKeyValue(1600, SceneManager.SceneName.SceneCharaEdit)]
		DECK_EDIT_COUNT = 1600,
		[EnumKeyValue(1800, SceneManager.SceneName.SceneTreeHouse)]
		MASTER_ROOM_SEND_STAMP_COUNT = 1800,
		[EnumKeyValue(1900, SceneManager.SceneName.SceneKemoBoard)]
		KEMOBOARD_OPEN_COUNT = 1900,
		[EnumKeyValue(2000, SceneManager.SceneName.SceneCharaEdit)]
		KEMOSTATUS = 2000,
		[EnumKeyValue(2100, SceneManager.SceneName.SceneAchievement)]
		ACHIEVEMENT_GET_NUM = 2100
	}
}
