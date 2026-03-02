using System;

// Token: 0x02000048 RID: 72
public class CharaMotionDefine
{
	// Token: 0x04000265 RID: 613
	public static readonly CharaMotionDefine.ActKey[] ViewerIndex = new CharaMotionDefine.ActKey[]
	{
		CharaMotionDefine.ActKey.IDLING,
		CharaMotionDefine.ActKey.STAND_BY_ST,
		CharaMotionDefine.ActKey.STAND_BY_LP,
		CharaMotionDefine.ActKey.STAND_BY_EN,
		CharaMotionDefine.ActKey.ACCENT,
		CharaMotionDefine.ActKey.ATTACK1,
		CharaMotionDefine.ActKey.ATTACK2,
		CharaMotionDefine.ActKey.ATTACK3,
		CharaMotionDefine.ActKey.ATTACK4,
		CharaMotionDefine.ActKey.ATTACK5,
		CharaMotionDefine.ActKey.ARTS_END,
		CharaMotionDefine.ActKey.DAMAGE_L,
		CharaMotionDefine.ActKey.DAMAGE_S,
		CharaMotionDefine.ActKey.DAMAGE_BOSS_L,
		CharaMotionDefine.ActKey.DAMAGE_BOSS_S,
		CharaMotionDefine.ActKey.ABNORMAL_ST,
		CharaMotionDefine.ActKey.ABNORMAL_LP,
		CharaMotionDefine.ActKey.ABNORMAL_EN,
		CharaMotionDefine.ActKey.COVER_ST,
		CharaMotionDefine.ActKey.COVER_LP,
		CharaMotionDefine.ActKey.DEATH_ST,
		CharaMotionDefine.ActKey.DEATH_LP,
		CharaMotionDefine.ActKey.DEATH_EN,
		CharaMotionDefine.ActKey.LAUGH,
		CharaMotionDefine.ActKey.LAUGH_SHORT,
		CharaMotionDefine.ActKey.LAUGH_WAT,
		CharaMotionDefine.ActKey.ENTRY,
		CharaMotionDefine.ActKey.ENTRY_LP,
		CharaMotionDefine.ActKey.ENTRY_EN,
		CharaMotionDefine.ActKey.WIN_ST,
		CharaMotionDefine.ActKey.WIN_LP,
		CharaMotionDefine.ActKey.FRONT_STEP,
		CharaMotionDefine.ActKey.BACK_STEP,
		CharaMotionDefine.ActKey.RUN,
		CharaMotionDefine.ActKey.RUNNING,
		CharaMotionDefine.ActKey.BIRD_IN,
		CharaMotionDefine.ActKey.BIRD_OUT,
		CharaMotionDefine.ActKey.JUMP_IN,
		CharaMotionDefine.ActKey.JUMP_OUT,
		CharaMotionDefine.ActKey.SCENARIO_STAND_BY,
		CharaMotionDefine.ActKey.SCENARIO_ENTRY,
		CharaMotionDefine.ActKey.SCENARIO_ATTACK,
		CharaMotionDefine.ActKey.VOICE_CREDIT,
		CharaMotionDefine.ActKey.POSITIVE,
		CharaMotionDefine.ActKey.DENIAL,
		CharaMotionDefine.ActKey.JOY,
		CharaMotionDefine.ActKey.ANGER,
		CharaMotionDefine.ActKey.SORROW,
		CharaMotionDefine.ActKey.SURPRISE,
		CharaMotionDefine.ActKey.WEAPON_TAKE,
		CharaMotionDefine.ActKey.WEAPON_STAND_BY,
		CharaMotionDefine.ActKey.WEAPON_PUT,
		CharaMotionDefine.ActKey.WEAPON_ATTACK,
		CharaMotionDefine.ActKey.SING_ST,
		CharaMotionDefine.ActKey.SING_LP,
		CharaMotionDefine.ActKey.SING_EN,
		CharaMotionDefine.ActKey.SPECIAL,
		CharaMotionDefine.ActKey.GACHA_ST,
		CharaMotionDefine.ActKey.GACHA_LP,
		CharaMotionDefine.ActKey.H_ITEM_UP_ST,
		CharaMotionDefine.ActKey.H_ITEM_UP_LP,
		CharaMotionDefine.ActKey.H_ITEM_UP_EN,
		CharaMotionDefine.ActKey.H_ACT_SEE_ST,
		CharaMotionDefine.ActKey.H_ACT_SEE_LP,
		CharaMotionDefine.ActKey.H_ACT_SEE_EN,
		CharaMotionDefine.ActKey.H_ITEM_STR,
		CharaMotionDefine.ActKey.H_ITEM_TAP,
		CharaMotionDefine.ActKey.H_ITEM_BED_LP,
		CharaMotionDefine.ActKey.H_ITEM_CHR_LP,
		CharaMotionDefine.ActKey.GROWTH_FRIENDS,
		CharaMotionDefine.ActKey.S_WAIT1,
		CharaMotionDefine.ActKey.S_WAIT2,
		CharaMotionDefine.ActKey.S_WAIT3,
		CharaMotionDefine.ActKey.S_WAKEUP,
		CharaMotionDefine.ActKey.S_SMILE,
		CharaMotionDefine.ActKey.S_FACETOUCH,
		CharaMotionDefine.ActKey.S_BODY,
		CharaMotionDefine.ActKey.S_IN,
		CharaMotionDefine.ActKey.COS_CHANGE1,
		CharaMotionDefine.ActKey.COS_CHANGE2,
		CharaMotionDefine.ActKey.COS_CHANGE3,
		CharaMotionDefine.ActKey.PVP_ENTRY,
		CharaMotionDefine.ActKey.PVP_DAMAGE,
		CharaMotionDefine.ActKey.PVP_DEATH_ST,
		CharaMotionDefine.ActKey.PVP_DEATH_LP,
		CharaMotionDefine.ActKey.PVP_DEATH_EN,
		CharaMotionDefine.ActKey.PVP_CLAP,
		CharaMotionDefine.ActKey.PVP_RESULT_STAND,
		CharaMotionDefine.ActKey.PVP_RESULT_LIE,
		CharaMotionDefine.ActKey.PVP_RESULT_SIT,
		CharaMotionDefine.ActKey.PVP_RESULT_RUN,
		CharaMotionDefine.ActKey.LOGIN_BONUS_ENTRY,
		CharaMotionDefine.ActKey.H_HOM_WAIT0,
		CharaMotionDefine.ActKey.H_HOM_MOV_ST,
		CharaMotionDefine.ActKey.H_HOM_MOV_LP,
		CharaMotionDefine.ActKey.H_HOM_MOV_EN,
		CharaMotionDefine.ActKey.H_HOM_LEFT_HALF_TURN,
		CharaMotionDefine.ActKey.H_HOM_RIGHT_HALF_TURN,
		CharaMotionDefine.ActKey.H_HOM_LEFT_FULL_TURN,
		CharaMotionDefine.ActKey.H_HOM_RIGHT_FULL_TURN,
		CharaMotionDefine.ActKey.H_HOM_YAW,
		CharaMotionDefine.ActKey.H_HOM_FLY_WAIT0,
		CharaMotionDefine.ActKey.H_HOM_FLY_MOV_ST,
		CharaMotionDefine.ActKey.H_HOM_FLY_MOV_LP,
		CharaMotionDefine.ActKey.H_HOM_FLY_MOV_EN,
		CharaMotionDefine.ActKey.H_HOM_FLY_LEFT_HALF_TURN,
		CharaMotionDefine.ActKey.H_HOM_FLY_RIGHT_HALF_TURN,
		CharaMotionDefine.ActKey.H_HOM_FLY_LEFT_FULL_TURN,
		CharaMotionDefine.ActKey.H_HOM_FLY_RIGHT_FULL_TURN,
		CharaMotionDefine.ActKey.SHOP_IN,
		CharaMotionDefine.ActKey.SHOP_BUY,
		CharaMotionDefine.ActKey.SHOP_IDLING,
		CharaMotionDefine.ActKey.EXP_GROWTH_ST,
		CharaMotionDefine.ActKey.EXP_GROWTH_LP,
		CharaMotionDefine.ActKey.EXP_GROWTH_EN,
		CharaMotionDefine.ActKey.FRIENDSHIP_STROKE_REACTION,
		CharaMotionDefine.ActKey.H_ACT_TREE_SLP_LP,
		CharaMotionDefine.ActKey.PIC_BALL_KICK,
		CharaMotionDefine.ActKey.PIC_SITTING_DOWN,
		CharaMotionDefine.ActKey.PIC_HANAICHIMONME,
		CharaMotionDefine.ActKey.PIC_CATCHBALL_WAIT,
		CharaMotionDefine.ActKey.PIC_CATCHBALL_THROW,
		CharaMotionDefine.ActKey.PIC_CATCHBALL_CALL,
		CharaMotionDefine.ActKey.PIC_BADMINTON_WAIT,
		CharaMotionDefine.ActKey.PIC_BADMINTON_HIT,
		CharaMotionDefine.ActKey.PIC_BADMINTON_CALL,
		CharaMotionDefine.ActKey.PIC_KENKENPA,
		CharaMotionDefine.ActKey.PIC_BALLOON_WALK,
		CharaMotionDefine.ActKey.PIC_BALLOON_LOOK,
		CharaMotionDefine.ActKey.PIC_CATCHBALL_WAIT_NOHAND,
		CharaMotionDefine.ActKey.PIC_CATCHBALL_THROW_NOHAND,
		CharaMotionDefine.ActKey.PIC_CATCHBALL_CALL_NOHAND,
		CharaMotionDefine.ActKey.PIC_BADMINTON_WAIT_NOHAND,
		CharaMotionDefine.ActKey.PIC_BADMINTON_HIT_NOHAND,
		CharaMotionDefine.ActKey.PIC_BADMINTON_CALL_NOHAND,
		CharaMotionDefine.ActKey.PIC_KENKENPA_LP,
		CharaMotionDefine.ActKey.PIC_KENKENPA_ED,
		CharaMotionDefine.ActKey.PIC_KENKENPA_JOY,
		CharaMotionDefine.ActKey.PIC_HANETSUKI_WAIT,
		CharaMotionDefine.ActKey.PIC_HANETSUKI_HIT_1,
		CharaMotionDefine.ActKey.PIC_HANETSUKI_HIT_2,
		CharaMotionDefine.ActKey.PIC_HANETSUKI_HIT_3,
		CharaMotionDefine.ActKey.PIC_HANETSUKI_CALL,
		CharaMotionDefine.ActKey.PIC_HANETSUKI_WAIT_NOHAND,
		CharaMotionDefine.ActKey.PIC_HANETSUKI_HIT_NOHAND,
		CharaMotionDefine.ActKey.PIC_HANETSUKI_CALL_NOHAND,
		CharaMotionDefine.ActKey.PIC_TRAIN_WAIT,
		CharaMotionDefine.ActKey.PIC_TRAIN_CALL,
		CharaMotionDefine.ActKey.PIC_TRAIN_WALK_ST,
		CharaMotionDefine.ActKey.PIC_TRAIN_WALK_LP_1,
		CharaMotionDefine.ActKey.PIC_TRAIN_WALK_LP_2,
		CharaMotionDefine.ActKey.PIC_TRAIN_WALK_EN,
		CharaMotionDefine.ActKey.PIC_SNOWMAN_1_ST,
		CharaMotionDefine.ActKey.PIC_SNOWMAN_1_LP,
		CharaMotionDefine.ActKey.PIC_SNOWMAN_1_ED,
		CharaMotionDefine.ActKey.PIC_SNOWMAN_2_ST,
		CharaMotionDefine.ActKey.PIC_SNOWMAN_2_LP,
		CharaMotionDefine.ActKey.PIC_SNOWMAN_2_ED,
		CharaMotionDefine.ActKey.PIC_SNOWROLING_ST,
		CharaMotionDefine.ActKey.PIC_SNOWROLING_LP,
		CharaMotionDefine.ActKey.PIC_SNOWROLING_EN,
		CharaMotionDefine.ActKey.PIC_HARDING_ST,
		CharaMotionDefine.ActKey.PIC_HARDING_LP,
		CharaMotionDefine.ActKey.PIC_HARDING_EN,
		CharaMotionDefine.ActKey.MYR_CHAT_1MOT,
		CharaMotionDefine.ActKey.MYR_CHAT_2MOT,
		CharaMotionDefine.ActKey.MYR_CHAT_3MOT,
		CharaMotionDefine.ActKey.MYR_CHAT_4MOT,
		CharaMotionDefine.ActKey.MYR_TEA_1MOT,
		CharaMotionDefine.ActKey.MYR_TEA_2MOT,
		CharaMotionDefine.ActKey.MYR_TEA_3MOT,
		CharaMotionDefine.ActKey.MYR_TEA_4MOT,
		CharaMotionDefine.ActKey.MYR_PLAY_1MOT,
		CharaMotionDefine.ActKey.MYR_PLAY_2MOT,
		CharaMotionDefine.ActKey.MYR_PLAY_3MOT,
		CharaMotionDefine.ActKey.MYR_PLAY_4MOT,
		CharaMotionDefine.ActKey.MYR_CURRY_1,
		CharaMotionDefine.ActKey.MYR_CURRY_2,
		CharaMotionDefine.ActKey.MYR_CURRY_3,
		CharaMotionDefine.ActKey.MYR_CURRY_4,
		CharaMotionDefine.ActKey.MYR_TALK_1MOT_NOHAND,
		CharaMotionDefine.ActKey.MYR_TALK_2MOT_NOHAND,
		CharaMotionDefine.ActKey.MYR_TALK_3MOT_NOHAND,
		CharaMotionDefine.ActKey.MYR_TALK_4MOT_NOHAND,
		CharaMotionDefine.ActKey.MYR_TENT_TALK,
		CharaMotionDefine.ActKey.MYR_DRW_ST,
		CharaMotionDefine.ActKey.MYR_DRW_LP_A,
		CharaMotionDefine.ActKey.MYR_DRW_LP_B,
		CharaMotionDefine.ActKey.MYR_DRW_EN,
		CharaMotionDefine.ActKey.MYR_DRW_SEE_ST,
		CharaMotionDefine.ActKey.MYR_DRW_SEE_LP,
		CharaMotionDefine.ActKey.MYR_DRW_SEE_EN,
		CharaMotionDefine.ActKey.MYR_DRW_SEE_NOHAND_ST,
		CharaMotionDefine.ActKey.MYR_DRW_SEE_NOHAND_LP,
		CharaMotionDefine.ActKey.MYR_DRW_SEE_NOHAND_EN,
		CharaMotionDefine.ActKey.MYR_DRW_JOY,
		CharaMotionDefine.ActKey.MYR_COS_ST,
		CharaMotionDefine.ActKey.MYR_COS_EN,
		CharaMotionDefine.ActKey.MYR_COS_NOHAND_ST,
		CharaMotionDefine.ActKey.MYR_COS_NOHAND_EN,
		CharaMotionDefine.ActKey.MYR_COS_SEE_ST,
		CharaMotionDefine.ActKey.MYR_COS_SEE_LP,
		CharaMotionDefine.ActKey.MYR_COS_SEE_EN,
		CharaMotionDefine.ActKey.MYR_COS_SEE_NOHAND_ST,
		CharaMotionDefine.ActKey.MYR_COS_SEE_NOHAND_LP,
		CharaMotionDefine.ActKey.MYR_COS_SEE_NOHAND_EN,
		CharaMotionDefine.ActKey.MYR_GAME_1MOT_ST,
		CharaMotionDefine.ActKey.MYR_GAME_1MOT_LP_A,
		CharaMotionDefine.ActKey.MYR_GAME_1MOT_LP_B,
		CharaMotionDefine.ActKey.MYR_GAME_1MOT_END,
		CharaMotionDefine.ActKey.MYR_GAME_2MOT_ST,
		CharaMotionDefine.ActKey.MYR_GAME_2MOT_LP_A,
		CharaMotionDefine.ActKey.MYR_GAME_2MOT_LP_B,
		CharaMotionDefine.ActKey.MYR_GAME_2MOT_END,
		CharaMotionDefine.ActKey.MYR_GAME_NOHAND_ST,
		CharaMotionDefine.ActKey.MYR_GAME_NOHAND_LP,
		CharaMotionDefine.ActKey.MYR_GAME_NOHAND_EN,
		CharaMotionDefine.ActKey.MYR_GAME_EMO_JOY,
		CharaMotionDefine.ActKey.MYR_GAME_EMO_SAD,
		CharaMotionDefine.ActKey.MYR_STAND_BY,
		CharaMotionDefine.ActKey.MYR_SLEEP_1,
		CharaMotionDefine.ActKey.MYR_SLEEP_2,
		CharaMotionDefine.ActKey.MYR_SLEEP_3,
		CharaMotionDefine.ActKey.MYR_SLEEP_4,
		CharaMotionDefine.ActKey.MYR_PLAYSEE_1,
		CharaMotionDefine.ActKey.MYR_PLAYSEE_2,
		CharaMotionDefine.ActKey.MYR_PLAYSEE_3,
		CharaMotionDefine.ActKey.MYR_WANAGE_THROW_1,
		CharaMotionDefine.ActKey.MYR_WANAGE_THROW_2,
		CharaMotionDefine.ActKey.MYR_WANAGE_THROW_3,
		CharaMotionDefine.ActKey.MYR_BALL_THROW_1,
		CharaMotionDefine.ActKey.MYR_BALL_THROW_2,
		CharaMotionDefine.ActKey.MYR_BALL_THROW_3,
		CharaMotionDefine.ActKey.MYR_BOOK_READ_1,
		CharaMotionDefine.ActKey.MYR_BOOK_READ_2,
		CharaMotionDefine.ActKey.MYR_KITCHEN_TEA,
		CharaMotionDefine.ActKey.MYR_KITCHEN_FOOD,
		CharaMotionDefine.ActKey.MYR_KITCHEN_GREENTEA,
		CharaMotionDefine.ActKey.MYR_NOTE_1,
		CharaMotionDefine.ActKey.MYR_NOTE_2,
		CharaMotionDefine.ActKey.MYR_BANNER_1,
		CharaMotionDefine.ActKey.MYR_BANNER_2,
		CharaMotionDefine.ActKey.MYR_KAKIZOME_1,
		CharaMotionDefine.ActKey.MYR_KAKIZOME_2,
		CharaMotionDefine.ActKey.MYR_GARDEN_WATER,
		CharaMotionDefine.ActKey.MYR_GARDEN_SEE,
		CharaMotionDefine.ActKey.MYR_GARDEN_SEE_NOHAND,
		CharaMotionDefine.ActKey.MYR_XYLOPHONE_PLAY_DUO,
		CharaMotionDefine.ActKey.MYR_XYLOPHONE_SEE_DUO,
		CharaMotionDefine.ActKey.MYR_XYLOPHONE_PLAY_SOLO,
		CharaMotionDefine.ActKey.MYR_COOK,
		CharaMotionDefine.ActKey.MYR_JPNTEA_1,
		CharaMotionDefine.ActKey.MYR_JPNTEA_2,
		CharaMotionDefine.ActKey.MYR_SWEETS_1,
		CharaMotionDefine.ActKey.MYR_SWEETS_2,
		CharaMotionDefine.ActKey.MYR_CAR_1,
		CharaMotionDefine.ActKey.MYR_CAR_SEE,
		CharaMotionDefine.ActKey.MYR_CAR_SEE_NOHAND,
		CharaMotionDefine.ActKey.MYR_SAND_PLAY_1,
		CharaMotionDefine.ActKey.MYR_SAND_PLAY_2,
		CharaMotionDefine.ActKey.MYR_SAND_PLAY_3,
		CharaMotionDefine.ActKey.MYR_SAND_PLAY_4,
		CharaMotionDefine.ActKey.MYR_KAKIZOME_1,
		CharaMotionDefine.ActKey.MYR_KAKIZOME_2,
		CharaMotionDefine.ActKey.MYR_PLUSH_SEE,
		CharaMotionDefine.ActKey.FRIENDSHIP_SEE_ST,
		CharaMotionDefine.ActKey.FRIENDSHIP_SEE_LP,
		CharaMotionDefine.ActKey.FRIENDSHIP_SEE_EN,
		CharaMotionDefine.ActKey.PVP_ENTRY_LONG,
		CharaMotionDefine.ActKey.PVP_DEATH_ST_LONG,
		CharaMotionDefine.ActKey.PVP_DEATH_LP_LONG,
		CharaMotionDefine.ActKey.PVP_DEATH_EN_LONG,
		CharaMotionDefine.ActKey.PVP_RESULT_SIT_LONG,
		CharaMotionDefine.ActKey.PIC_SITTING_DOWN_LONG,
		CharaMotionDefine.ActKey.HOME_CONTACT_001,
		CharaMotionDefine.ActKey.HOME_CONTACT_002,
		CharaMotionDefine.ActKey.HOME_CONTACT_003,
		CharaMotionDefine.ActKey.HOME_CONTACT_004,
		CharaMotionDefine.ActKey.HOME_CONTACT_005,
		CharaMotionDefine.ActKey.HOME_CONTACT_006,
		CharaMotionDefine.ActKey.HOME_CONTACT_007,
		CharaMotionDefine.ActKey.HOME_CONTACT_008,
		CharaMotionDefine.ActKey.HOME_CONTACT_009,
		CharaMotionDefine.ActKey.HOME_CONTACT_010,
		CharaMotionDefine.ActKey.HOME_CONTACT_011,
		CharaMotionDefine.ActKey.HOME_CONTACT_012,
		CharaMotionDefine.ActKey.HOME_CONTACT_013,
		CharaMotionDefine.ActKey.HOME_CONTACT_014,
		CharaMotionDefine.ActKey.HOME_CONTACT_015,
		CharaMotionDefine.ActKey.HOME_CONTACT_016,
		CharaMotionDefine.ActKey.HOME_CONTACT_017,
		CharaMotionDefine.ActKey.HOME_CONTACT_018,
		CharaMotionDefine.ActKey.HOME_CONTACT_019,
		CharaMotionDefine.ActKey.HOME_CONTACT_020,
		CharaMotionDefine.ActKey.HOME_CONTACT_021,
		CharaMotionDefine.ActKey.HOME_CONTACT_022,
		CharaMotionDefine.ActKey.HOME_CONTACT_023,
		CharaMotionDefine.ActKey.HOME_CONTACT_024,
		CharaMotionDefine.ActKey.HOME_CONTACT_025,
		CharaMotionDefine.ActKey.HOME_CONTACT_026,
		CharaMotionDefine.ActKey.HOME_CONTACT_027,
		CharaMotionDefine.ActKey.HOME_CONTACT_028,
		CharaMotionDefine.ActKey.HOME_CONTACT_029,
		CharaMotionDefine.ActKey.HOME_CONTACT_030,
		CharaMotionDefine.ActKey.HOME_CONTACT_031,
		CharaMotionDefine.ActKey.HOME_CONTACT_032,
		CharaMotionDefine.ActKey.HOME_CONTACT_033,
		CharaMotionDefine.ActKey.HOME_CONTACT_034,
		CharaMotionDefine.ActKey.HOME_CONTACT_035,
		CharaMotionDefine.ActKey.HOME_CONTACT_036,
		CharaMotionDefine.ActKey.HOME_CONTACT_037,
		CharaMotionDefine.ActKey.HOME_CONTACT_038,
		CharaMotionDefine.ActKey.HOME_CONTACT_039,
		CharaMotionDefine.ActKey.HOME_CONTACT_040,
		CharaMotionDefine.ActKey.HOME_CONTACT_041,
		CharaMotionDefine.ActKey.HOME_CONTACT_042,
		CharaMotionDefine.ActKey.HOME_CONTACT_043,
		CharaMotionDefine.ActKey.HOME_CONTACT_044,
		CharaMotionDefine.ActKey.HOME_CONTACT_045,
		CharaMotionDefine.ActKey.HOME_CONTACT_046,
		CharaMotionDefine.ActKey.HOME_CONTACT_047,
		CharaMotionDefine.ActKey.HOME_CONTACT_048,
		CharaMotionDefine.ActKey.HOME_CONTACT_049,
		CharaMotionDefine.ActKey.HOME_CONTACT_050,
		CharaMotionDefine.ActKey.HOME_CONTACT_051,
		CharaMotionDefine.ActKey.HOME_CONTACT_052,
		CharaMotionDefine.ActKey.HOME_CONTACT_053,
		CharaMotionDefine.ActKey.HOME_CONTACT_054,
		CharaMotionDefine.ActKey.HOME_CONTACT_055,
		CharaMotionDefine.ActKey.HOME_CONTACT_056,
		CharaMotionDefine.ActKey.HOME_CONTACT_057,
		CharaMotionDefine.ActKey.HOME_CONTACT_058,
		CharaMotionDefine.ActKey.HOME_CONTACT_059,
		CharaMotionDefine.ActKey.HOME_CONTACT_060,
		CharaMotionDefine.ActKey.HOME_CONTACT_061,
		CharaMotionDefine.ActKey.HOME_CONTACT_062,
		CharaMotionDefine.ActKey.HOME_CONTACT_063,
		CharaMotionDefine.ActKey.HOME_CONTACT_064,
		CharaMotionDefine.ActKey.HOME_CONTACT_065,
		CharaMotionDefine.ActKey.HOME_CONTACT_066,
		CharaMotionDefine.ActKey.HOME_CONTACT_067,
		CharaMotionDefine.ActKey.HOME_CONTACT_068,
		CharaMotionDefine.ActKey.HOME_CONTACT_069,
		CharaMotionDefine.ActKey.HOME_CONTACT_070,
		CharaMotionDefine.ActKey.HOME_CONTACT_071,
		CharaMotionDefine.ActKey.HOME_CONTACT_072,
		CharaMotionDefine.ActKey.HOME_CONTACT_073,
		CharaMotionDefine.ActKey.HOME_CONTACT_074,
		CharaMotionDefine.ActKey.HOME_CONTACT_075,
		CharaMotionDefine.ActKey.HOME_CONTACT_076,
		CharaMotionDefine.ActKey.HOME_CONTACT_077,
		CharaMotionDefine.ActKey.HOME_CONTACT_078,
		CharaMotionDefine.ActKey.HOME_CONTACT_079,
		CharaMotionDefine.ActKey.HOME_CONTACT_080,
		CharaMotionDefine.ActKey.HOME_CONTACT_081,
		CharaMotionDefine.ActKey.HOME_CONTACT_082,
		CharaMotionDefine.ActKey.HOME_CONTACT_083,
		CharaMotionDefine.ActKey.HOME_CONTACT_084,
		CharaMotionDefine.ActKey.HOME_CONTACT_085,
		CharaMotionDefine.ActKey.HOME_CONTACT_086,
		CharaMotionDefine.ActKey.HOME_CONTACT_087,
		CharaMotionDefine.ActKey.HOME_CONTACT_088,
		CharaMotionDefine.ActKey.HOME_CONTACT_089,
		CharaMotionDefine.ActKey.HOME_CONTACT_090,
		CharaMotionDefine.ActKey.HOME_CONTACT_091,
		CharaMotionDefine.ActKey.HOME_CONTACT_092,
		CharaMotionDefine.ActKey.HOME_CONTACT_093,
		CharaMotionDefine.ActKey.HOME_CONTACT_094,
		CharaMotionDefine.ActKey.HOME_CONTACT_095,
		CharaMotionDefine.ActKey.HOME_CONTACT_096,
		CharaMotionDefine.ActKey.HOME_CONTACT_097,
		CharaMotionDefine.ActKey.HOME_CONTACT_098,
		CharaMotionDefine.ActKey.HOME_CONTACT_099,
		CharaMotionDefine.ActKey.HOME_CONTACT_100
	};

	// Token: 0x04000266 RID: 614
	public static readonly string[] MotionPersonalityName = new string[]
	{
		"未設定", "タイプA", "タイプB", "タイプC", "タイプD", "タイプE", "タイプF", "タイプG", "タイプH", "タイプI",
		"タイプJ", "タイプK"
	};

	// Token: 0x020005F4 RID: 1524
	public enum ActKey
	{
		// Token: 0x04002AC0 RID: 10944
		INVALID,
		// Token: 0x04002AC1 RID: 10945
		IDLING,
		// Token: 0x04002AC2 RID: 10946
		STAND_BY,
		// Token: 0x04002AC3 RID: 10947
		ACCENT,
		// Token: 0x04002AC4 RID: 10948
		ATTACK1,
		// Token: 0x04002AC5 RID: 10949
		ATTACK2,
		// Token: 0x04002AC6 RID: 10950
		ATTACK3,
		// Token: 0x04002AC7 RID: 10951
		ATTACK4,
		// Token: 0x04002AC8 RID: 10952
		ATTACK5,
		// Token: 0x04002AC9 RID: 10953
		ARTS_END,
		// Token: 0x04002ACA RID: 10954
		DAMAGE_L,
		// Token: 0x04002ACB RID: 10955
		DAMAGE_S,
		// Token: 0x04002ACC RID: 10956
		ABNORMAL_LP,
		// Token: 0x04002ACD RID: 10957
		DEATH_ST,
		// Token: 0x04002ACE RID: 10958
		ENTRY,
		// Token: 0x04002ACF RID: 10959
		WIN_ST,
		// Token: 0x04002AD0 RID: 10960
		FRONT_STEP,
		// Token: 0x04002AD1 RID: 10961
		BACK_STEP,
		// Token: 0x04002AD2 RID: 10962
		RUN,
		// Token: 0x04002AD3 RID: 10963
		SCENARIO_STAND_BY,
		// Token: 0x04002AD4 RID: 10964
		VOICE_CREDIT,
		// Token: 0x04002AD5 RID: 10965
		POSITIVE,
		// Token: 0x04002AD6 RID: 10966
		DENIAL,
		// Token: 0x04002AD7 RID: 10967
		JOY,
		// Token: 0x04002AD8 RID: 10968
		ANGER,
		// Token: 0x04002AD9 RID: 10969
		SORROW,
		// Token: 0x04002ADA RID: 10970
		SURPRISE,
		// Token: 0x04002ADB RID: 10971
		WEAPON_STAND_BY,
		// Token: 0x04002ADC RID: 10972
		WEAPON_TAKE,
		// Token: 0x04002ADD RID: 10973
		WEAPON_PUT,
		// Token: 0x04002ADE RID: 10974
		SPECIAL,
		// Token: 0x04002ADF RID: 10975
		GACHA_ST,
		// Token: 0x04002AE0 RID: 10976
		ABNORMAL_ST,
		// Token: 0x04002AE1 RID: 10977
		ABNORMAL_EN,
		// Token: 0x04002AE2 RID: 10978
		DEATH_LP,
		// Token: 0x04002AE3 RID: 10979
		DEATH_EN,
		// Token: 0x04002AE4 RID: 10980
		WIN_LP,
		// Token: 0x04002AE5 RID: 10981
		GACHA_LP,
		// Token: 0x04002AE6 RID: 10982
		STAND_BY_ST,
		// Token: 0x04002AE7 RID: 10983
		STAND_BY_LP,
		// Token: 0x04002AE8 RID: 10984
		STAND_BY_EN,
		// Token: 0x04002AE9 RID: 10985
		ENTRY_LP,
		// Token: 0x04002AEA RID: 10986
		ENTRY_EN,
		// Token: 0x04002AEB RID: 10987
		H_ITEM_UP_ST,
		// Token: 0x04002AEC RID: 10988
		H_ITEM_UP_LP,
		// Token: 0x04002AED RID: 10989
		H_ITEM_UP_EN,
		// Token: 0x04002AEE RID: 10990
		H_ACT_SEE_ST,
		// Token: 0x04002AEF RID: 10991
		H_ACT_SEE_LP,
		// Token: 0x04002AF0 RID: 10992
		H_ACT_SEE_EN,
		// Token: 0x04002AF1 RID: 10993
		H_ITEM_STR,
		// Token: 0x04002AF2 RID: 10994
		H_ITEM_TAP,
		// Token: 0x04002AF3 RID: 10995
		BIRD_IN,
		// Token: 0x04002AF4 RID: 10996
		BIRD_OUT,
		// Token: 0x04002AF5 RID: 10997
		H_ITEM_BED_LP,
		// Token: 0x04002AF6 RID: 10998
		H_ITEM_CHR_LP,
		// Token: 0x04002AF7 RID: 10999
		S_WAIT1,
		// Token: 0x04002AF8 RID: 11000
		S_WAIT2,
		// Token: 0x04002AF9 RID: 11001
		S_WAIT3,
		// Token: 0x04002AFA RID: 11002
		S_WAKEUP,
		// Token: 0x04002AFB RID: 11003
		S_SMILE,
		// Token: 0x04002AFC RID: 11004
		S_FACETOUCH,
		// Token: 0x04002AFD RID: 11005
		S_BODY,
		// Token: 0x04002AFE RID: 11006
		S_IN,
		// Token: 0x04002AFF RID: 11007
		COS_CHANGE1,
		// Token: 0x04002B00 RID: 11008
		COS_CHANGE2,
		// Token: 0x04002B01 RID: 11009
		COS_CHANGE3,
		// Token: 0x04002B02 RID: 11010
		GROWTH_FRIENDS,
		// Token: 0x04002B03 RID: 11011
		ARTS_CHARGE_LP,
		// Token: 0x04002B04 RID: 11012
		ARTS_CHARGE_EN,
		// Token: 0x04002B05 RID: 11013
		HIGH_TOUCH_A,
		// Token: 0x04002B06 RID: 11014
		HIGH_TOUCH_B,
		// Token: 0x04002B07 RID: 11015
		EXERCISE_A,
		// Token: 0x04002B08 RID: 11016
		EXERCISE_B,
		// Token: 0x04002B09 RID: 11017
		EXERCISE_C,
		// Token: 0x04002B0A RID: 11018
		EXERCISE_D,
		// Token: 0x04002B0B RID: 11019
		PVP_ENTRY,
		// Token: 0x04002B0C RID: 11020
		PVP_DAMAGE,
		// Token: 0x04002B0D RID: 11021
		PVP_DEATH_ST,
		// Token: 0x04002B0E RID: 11022
		PVP_DEATH_LP,
		// Token: 0x04002B0F RID: 11023
		PVP_DEATH_EN,
		// Token: 0x04002B10 RID: 11024
		PVP_CLAP,
		// Token: 0x04002B11 RID: 11025
		LOGIN_BONUS_ENTRY,
		// Token: 0x04002B12 RID: 11026
		H_HOM_WAIT0,
		// Token: 0x04002B13 RID: 11027
		H_HOM_MOV_ST,
		// Token: 0x04002B14 RID: 11028
		H_HOM_MOV_LP,
		// Token: 0x04002B15 RID: 11029
		H_HOM_MOV_EN,
		// Token: 0x04002B16 RID: 11030
		H_HOM_LEFT_HALF_TURN,
		// Token: 0x04002B17 RID: 11031
		H_HOM_RIGHT_HALF_TURN,
		// Token: 0x04002B18 RID: 11032
		H_HOM_LEFT_FULL_TURN,
		// Token: 0x04002B19 RID: 11033
		H_HOM_RIGHT_FULL_TURN,
		// Token: 0x04002B1A RID: 11034
		H_HOM_YAW,
		// Token: 0x04002B1B RID: 11035
		H_HOM_FLY_WAIT0,
		// Token: 0x04002B1C RID: 11036
		H_HOM_FLY_MOV_ST,
		// Token: 0x04002B1D RID: 11037
		H_HOM_FLY_MOV_LP,
		// Token: 0x04002B1E RID: 11038
		H_HOM_FLY_MOV_EN,
		// Token: 0x04002B1F RID: 11039
		H_HOM_FLY_LEFT_HALF_TURN,
		// Token: 0x04002B20 RID: 11040
		H_HOM_FLY_RIGHT_HALF_TURN,
		// Token: 0x04002B21 RID: 11041
		H_HOM_FLY_LEFT_FULL_TURN,
		// Token: 0x04002B22 RID: 11042
		H_HOM_FLY_RIGHT_FULL_TURN,
		// Token: 0x04002B23 RID: 11043
		SHOP_IN,
		// Token: 0x04002B24 RID: 11044
		SHOP_BUY,
		// Token: 0x04002B25 RID: 11045
		SHOP_IDLING,
		// Token: 0x04002B26 RID: 11046
		EXP_GROWTH_ST,
		// Token: 0x04002B27 RID: 11047
		EXP_GROWTH_LP,
		// Token: 0x04002B28 RID: 11048
		EXP_GROWTH_EN,
		// Token: 0x04002B29 RID: 11049
		FRIENDSHIP_STROKE_REACTION,
		// Token: 0x04002B2A RID: 11050
		H_ACT_TREE_SLP_LP,
		// Token: 0x04002B2B RID: 11051
		PIC_BALL_KICK,
		// Token: 0x04002B2C RID: 11052
		PIC_SITTING_DOWN,
		// Token: 0x04002B2D RID: 11053
		PIC_HANAICHIMONME,
		// Token: 0x04002B2E RID: 11054
		SING_ST,
		// Token: 0x04002B2F RID: 11055
		SING_LP,
		// Token: 0x04002B30 RID: 11056
		SING_EN,
		// Token: 0x04002B31 RID: 11057
		PVP_RESULT_STAND,
		// Token: 0x04002B32 RID: 11058
		PVP_RESULT_LIE,
		// Token: 0x04002B33 RID: 11059
		PVP_RESULT_SIT,
		// Token: 0x04002B34 RID: 11060
		PVP_RESULT_RUN,
		// Token: 0x04002B35 RID: 11061
		FRIENDSHIP_SEE_ST,
		// Token: 0x04002B36 RID: 11062
		FRIENDSHIP_SEE_LP,
		// Token: 0x04002B37 RID: 11063
		FRIENDSHIP_SEE_EN,
		// Token: 0x04002B38 RID: 11064
		PVP_ENTRY_LONG,
		// Token: 0x04002B39 RID: 11065
		PVP_DEATH_ST_LONG,
		// Token: 0x04002B3A RID: 11066
		PVP_DEATH_LP_LONG,
		// Token: 0x04002B3B RID: 11067
		PVP_DEATH_EN_LONG,
		// Token: 0x04002B3C RID: 11068
		PVP_RESULT_SIT_LONG,
		// Token: 0x04002B3D RID: 11069
		PIC_SITTING_DOWN_LONG,
		// Token: 0x04002B3E RID: 11070
		RUNNING,
		// Token: 0x04002B3F RID: 11071
		HOME_CONTACT_001,
		// Token: 0x04002B40 RID: 11072
		HOME_CONTACT_002,
		// Token: 0x04002B41 RID: 11073
		HOME_CONTACT_003,
		// Token: 0x04002B42 RID: 11074
		HOME_CONTACT_004,
		// Token: 0x04002B43 RID: 11075
		HOME_CONTACT_005,
		// Token: 0x04002B44 RID: 11076
		HOME_CONTACT_006,
		// Token: 0x04002B45 RID: 11077
		HOME_CONTACT_007,
		// Token: 0x04002B46 RID: 11078
		HOME_CONTACT_008,
		// Token: 0x04002B47 RID: 11079
		HOME_CONTACT_009,
		// Token: 0x04002B48 RID: 11080
		HOME_CONTACT_010,
		// Token: 0x04002B49 RID: 11081
		HOME_CONTACT_011,
		// Token: 0x04002B4A RID: 11082
		HOME_CONTACT_012,
		// Token: 0x04002B4B RID: 11083
		HOME_CONTACT_013,
		// Token: 0x04002B4C RID: 11084
		HOME_CONTACT_014,
		// Token: 0x04002B4D RID: 11085
		HOME_CONTACT_015,
		// Token: 0x04002B4E RID: 11086
		HOME_CONTACT_016,
		// Token: 0x04002B4F RID: 11087
		HOME_CONTACT_017,
		// Token: 0x04002B50 RID: 11088
		HOME_CONTACT_018,
		// Token: 0x04002B51 RID: 11089
		HOME_CONTACT_019,
		// Token: 0x04002B52 RID: 11090
		HOME_CONTACT_020,
		// Token: 0x04002B53 RID: 11091
		HOME_CONTACT_021,
		// Token: 0x04002B54 RID: 11092
		HOME_CONTACT_022,
		// Token: 0x04002B55 RID: 11093
		HOME_CONTACT_023,
		// Token: 0x04002B56 RID: 11094
		HOME_CONTACT_024,
		// Token: 0x04002B57 RID: 11095
		HOME_CONTACT_025,
		// Token: 0x04002B58 RID: 11096
		HOME_CONTACT_026,
		// Token: 0x04002B59 RID: 11097
		HOME_CONTACT_027,
		// Token: 0x04002B5A RID: 11098
		HOME_CONTACT_028,
		// Token: 0x04002B5B RID: 11099
		HOME_CONTACT_029,
		// Token: 0x04002B5C RID: 11100
		HOME_CONTACT_030,
		// Token: 0x04002B5D RID: 11101
		HOME_CONTACT_031,
		// Token: 0x04002B5E RID: 11102
		HOME_CONTACT_032,
		// Token: 0x04002B5F RID: 11103
		HOME_CONTACT_033,
		// Token: 0x04002B60 RID: 11104
		HOME_CONTACT_034,
		// Token: 0x04002B61 RID: 11105
		HOME_CONTACT_035,
		// Token: 0x04002B62 RID: 11106
		HOME_CONTACT_036,
		// Token: 0x04002B63 RID: 11107
		HOME_CONTACT_037,
		// Token: 0x04002B64 RID: 11108
		HOME_CONTACT_038,
		// Token: 0x04002B65 RID: 11109
		HOME_CONTACT_039,
		// Token: 0x04002B66 RID: 11110
		HOME_CONTACT_040,
		// Token: 0x04002B67 RID: 11111
		HOME_CONTACT_041,
		// Token: 0x04002B68 RID: 11112
		HOME_CONTACT_042,
		// Token: 0x04002B69 RID: 11113
		HOME_CONTACT_043,
		// Token: 0x04002B6A RID: 11114
		HOME_CONTACT_044,
		// Token: 0x04002B6B RID: 11115
		HOME_CONTACT_045,
		// Token: 0x04002B6C RID: 11116
		HOME_CONTACT_046,
		// Token: 0x04002B6D RID: 11117
		HOME_CONTACT_047,
		// Token: 0x04002B6E RID: 11118
		HOME_CONTACT_048,
		// Token: 0x04002B6F RID: 11119
		HOME_CONTACT_049,
		// Token: 0x04002B70 RID: 11120
		HOME_CONTACT_050,
		// Token: 0x04002B71 RID: 11121
		HOME_CONTACT_051,
		// Token: 0x04002B72 RID: 11122
		HOME_CONTACT_052,
		// Token: 0x04002B73 RID: 11123
		HOME_CONTACT_053,
		// Token: 0x04002B74 RID: 11124
		HOME_CONTACT_054,
		// Token: 0x04002B75 RID: 11125
		HOME_CONTACT_055,
		// Token: 0x04002B76 RID: 11126
		HOME_CONTACT_056,
		// Token: 0x04002B77 RID: 11127
		HOME_CONTACT_057,
		// Token: 0x04002B78 RID: 11128
		HOME_CONTACT_058,
		// Token: 0x04002B79 RID: 11129
		HOME_CONTACT_059,
		// Token: 0x04002B7A RID: 11130
		HOME_CONTACT_060,
		// Token: 0x04002B7B RID: 11131
		HOME_CONTACT_061,
		// Token: 0x04002B7C RID: 11132
		HOME_CONTACT_062,
		// Token: 0x04002B7D RID: 11133
		HOME_CONTACT_063,
		// Token: 0x04002B7E RID: 11134
		HOME_CONTACT_064,
		// Token: 0x04002B7F RID: 11135
		HOME_CONTACT_065,
		// Token: 0x04002B80 RID: 11136
		HOME_CONTACT_066,
		// Token: 0x04002B81 RID: 11137
		HOME_CONTACT_067,
		// Token: 0x04002B82 RID: 11138
		HOME_CONTACT_068,
		// Token: 0x04002B83 RID: 11139
		HOME_CONTACT_069,
		// Token: 0x04002B84 RID: 11140
		HOME_CONTACT_070,
		// Token: 0x04002B85 RID: 11141
		HOME_CONTACT_071,
		// Token: 0x04002B86 RID: 11142
		HOME_CONTACT_072,
		// Token: 0x04002B87 RID: 11143
		HOME_CONTACT_073,
		// Token: 0x04002B88 RID: 11144
		HOME_CONTACT_074,
		// Token: 0x04002B89 RID: 11145
		HOME_CONTACT_075,
		// Token: 0x04002B8A RID: 11146
		HOME_CONTACT_076,
		// Token: 0x04002B8B RID: 11147
		HOME_CONTACT_077,
		// Token: 0x04002B8C RID: 11148
		HOME_CONTACT_078,
		// Token: 0x04002B8D RID: 11149
		HOME_CONTACT_079,
		// Token: 0x04002B8E RID: 11150
		HOME_CONTACT_080,
		// Token: 0x04002B8F RID: 11151
		HOME_CONTACT_081,
		// Token: 0x04002B90 RID: 11152
		HOME_CONTACT_082,
		// Token: 0x04002B91 RID: 11153
		HOME_CONTACT_083,
		// Token: 0x04002B92 RID: 11154
		HOME_CONTACT_084,
		// Token: 0x04002B93 RID: 11155
		HOME_CONTACT_085,
		// Token: 0x04002B94 RID: 11156
		HOME_CONTACT_086,
		// Token: 0x04002B95 RID: 11157
		HOME_CONTACT_087,
		// Token: 0x04002B96 RID: 11158
		HOME_CONTACT_088,
		// Token: 0x04002B97 RID: 11159
		HOME_CONTACT_089,
		// Token: 0x04002B98 RID: 11160
		HOME_CONTACT_090,
		// Token: 0x04002B99 RID: 11161
		HOME_CONTACT_091,
		// Token: 0x04002B9A RID: 11162
		HOME_CONTACT_092,
		// Token: 0x04002B9B RID: 11163
		HOME_CONTACT_093,
		// Token: 0x04002B9C RID: 11164
		HOME_CONTACT_094,
		// Token: 0x04002B9D RID: 11165
		HOME_CONTACT_095,
		// Token: 0x04002B9E RID: 11166
		HOME_CONTACT_096,
		// Token: 0x04002B9F RID: 11167
		HOME_CONTACT_097,
		// Token: 0x04002BA0 RID: 11168
		HOME_CONTACT_098,
		// Token: 0x04002BA1 RID: 11169
		HOME_CONTACT_099,
		// Token: 0x04002BA2 RID: 11170
		HOME_CONTACT_100,
		// Token: 0x04002BA3 RID: 11171
		PIC_CATCHBALL_WAIT,
		// Token: 0x04002BA4 RID: 11172
		PIC_CATCHBALL_THROW,
		// Token: 0x04002BA5 RID: 11173
		PIC_CATCHBALL_CALL,
		// Token: 0x04002BA6 RID: 11174
		PIC_BADMINTON_WAIT,
		// Token: 0x04002BA7 RID: 11175
		PIC_BADMINTON_HIT,
		// Token: 0x04002BA8 RID: 11176
		PIC_BADMINTON_CALL,
		// Token: 0x04002BA9 RID: 11177
		PIC_KENKENPA,
		// Token: 0x04002BAA RID: 11178
		PIC_BALLOON_WALK,
		// Token: 0x04002BAB RID: 11179
		PIC_BALLOON_LOOK,
		// Token: 0x04002BAC RID: 11180
		PIC_CATCHBALL_WAIT_NOHAND,
		// Token: 0x04002BAD RID: 11181
		PIC_CATCHBALL_THROW_NOHAND,
		// Token: 0x04002BAE RID: 11182
		PIC_CATCHBALL_CALL_NOHAND,
		// Token: 0x04002BAF RID: 11183
		PIC_BADMINTON_WAIT_NOHAND,
		// Token: 0x04002BB0 RID: 11184
		PIC_BADMINTON_HIT_NOHAND,
		// Token: 0x04002BB1 RID: 11185
		PIC_BADMINTON_CALL_NOHAND,
		// Token: 0x04002BB2 RID: 11186
		SCENARIO_ATTACK,
		// Token: 0x04002BB3 RID: 11187
		WEAPON_ATTACK,
		// Token: 0x04002BB4 RID: 11188
		SCENARIO_ENTRY,
		// Token: 0x04002BB5 RID: 11189
		COVER_ST,
		// Token: 0x04002BB6 RID: 11190
		COVER_LP,
		// Token: 0x04002BB7 RID: 11191
		DAMAGE_BOSS_L,
		// Token: 0x04002BB8 RID: 11192
		DAMAGE_BOSS_S,
		// Token: 0x04002BB9 RID: 11193
		JUMP_IN,
		// Token: 0x04002BBA RID: 11194
		JUMP_OUT,
		// Token: 0x04002BBB RID: 11195
		MYR_CHAT_1MOT,
		// Token: 0x04002BBC RID: 11196
		MYR_CHAT_2MOT,
		// Token: 0x04002BBD RID: 11197
		MYR_CHAT_3MOT,
		// Token: 0x04002BBE RID: 11198
		MYR_CHAT_4MOT,
		// Token: 0x04002BBF RID: 11199
		MYR_TEA_1MOT,
		// Token: 0x04002BC0 RID: 11200
		MYR_TEA_2MOT,
		// Token: 0x04002BC1 RID: 11201
		MYR_TEA_3MOT,
		// Token: 0x04002BC2 RID: 11202
		MYR_TEA_4MOT,
		// Token: 0x04002BC3 RID: 11203
		MYR_PLAY_1MOT,
		// Token: 0x04002BC4 RID: 11204
		MYR_PLAY_2MOT,
		// Token: 0x04002BC5 RID: 11205
		MYR_PLAY_3MOT,
		// Token: 0x04002BC6 RID: 11206
		MYR_PLAY_4MOT,
		// Token: 0x04002BC7 RID: 11207
		MYR_TALK_1MOT_NOHAND,
		// Token: 0x04002BC8 RID: 11208
		MYR_TALK_2MOT_NOHAND,
		// Token: 0x04002BC9 RID: 11209
		MYR_TALK_3MOT_NOHAND,
		// Token: 0x04002BCA RID: 11210
		MYR_TALK_4MOT_NOHAND,
		// Token: 0x04002BCB RID: 11211
		MYR_DRW_ST,
		// Token: 0x04002BCC RID: 11212
		MYR_DRW_LP_A,
		// Token: 0x04002BCD RID: 11213
		MYR_DRW_LP_B,
		// Token: 0x04002BCE RID: 11214
		MYR_DRW_EN,
		// Token: 0x04002BCF RID: 11215
		MYR_DRW_SEE_ST,
		// Token: 0x04002BD0 RID: 11216
		MYR_DRW_SEE_LP,
		// Token: 0x04002BD1 RID: 11217
		MYR_DRW_SEE_EN,
		// Token: 0x04002BD2 RID: 11218
		MYR_DRW_SEE_NOHAND_ST,
		// Token: 0x04002BD3 RID: 11219
		MYR_DRW_SEE_NOHAND_LP,
		// Token: 0x04002BD4 RID: 11220
		MYR_DRW_SEE_NOHAND_EN,
		// Token: 0x04002BD5 RID: 11221
		MYR_DRW_JOY,
		// Token: 0x04002BD6 RID: 11222
		MYR_COS_ST,
		// Token: 0x04002BD7 RID: 11223
		MYR_COS_EN,
		// Token: 0x04002BD8 RID: 11224
		MYR_COS_NOHAND_ST,
		// Token: 0x04002BD9 RID: 11225
		MYR_COS_NOHAND_EN,
		// Token: 0x04002BDA RID: 11226
		MYR_COS_SEE_ST,
		// Token: 0x04002BDB RID: 11227
		MYR_COS_SEE_LP,
		// Token: 0x04002BDC RID: 11228
		MYR_COS_SEE_EN,
		// Token: 0x04002BDD RID: 11229
		MYR_COS_SEE_NOHAND_ST,
		// Token: 0x04002BDE RID: 11230
		MYR_COS_SEE_NOHAND_LP,
		// Token: 0x04002BDF RID: 11231
		MYR_COS_SEE_NOHAND_EN,
		// Token: 0x04002BE0 RID: 11232
		MYR_GAME_1MOT_ST,
		// Token: 0x04002BE1 RID: 11233
		MYR_GAME_1MOT_LP_A,
		// Token: 0x04002BE2 RID: 11234
		MYR_GAME_1MOT_LP_B,
		// Token: 0x04002BE3 RID: 11235
		MYR_GAME_1MOT_END,
		// Token: 0x04002BE4 RID: 11236
		MYR_GAME_2MOT_ST,
		// Token: 0x04002BE5 RID: 11237
		MYR_GAME_2MOT_LP_A,
		// Token: 0x04002BE6 RID: 11238
		MYR_GAME_2MOT_LP_B,
		// Token: 0x04002BE7 RID: 11239
		MYR_GAME_2MOT_END,
		// Token: 0x04002BE8 RID: 11240
		MYR_GAME_NOHAND_ST,
		// Token: 0x04002BE9 RID: 11241
		MYR_GAME_NOHAND_LP,
		// Token: 0x04002BEA RID: 11242
		MYR_GAME_NOHAND_EN,
		// Token: 0x04002BEB RID: 11243
		MYR_GAME_EMO_JOY,
		// Token: 0x04002BEC RID: 11244
		MYR_GAME_EMO_SAD,
		// Token: 0x04002BED RID: 11245
		MYR_STAND_BY,
		// Token: 0x04002BEE RID: 11246
		MYR_SLEEP_1,
		// Token: 0x04002BEF RID: 11247
		MYR_SLEEP_2,
		// Token: 0x04002BF0 RID: 11248
		MYR_SLEEP_3,
		// Token: 0x04002BF1 RID: 11249
		MYR_SLEEP_4,
		// Token: 0x04002BF2 RID: 11250
		MYR_PLAYSEE_1,
		// Token: 0x04002BF3 RID: 11251
		MYR_PLAYSEE_2,
		// Token: 0x04002BF4 RID: 11252
		MYR_PLAYSEE_3,
		// Token: 0x04002BF5 RID: 11253
		MYR_WANAGE_THROW_1,
		// Token: 0x04002BF6 RID: 11254
		MYR_WANAGE_THROW_2,
		// Token: 0x04002BF7 RID: 11255
		MYR_WANAGE_THROW_3,
		// Token: 0x04002BF8 RID: 11256
		MYR_BALL_THROW_1,
		// Token: 0x04002BF9 RID: 11257
		MYR_BALL_THROW_2,
		// Token: 0x04002BFA RID: 11258
		MYR_BALL_THROW_3,
		// Token: 0x04002BFB RID: 11259
		MYR_BOOK_READ_1,
		// Token: 0x04002BFC RID: 11260
		MYR_BOOK_READ_2,
		// Token: 0x04002BFD RID: 11261
		MYR_KITCHEN_TEA,
		// Token: 0x04002BFE RID: 11262
		MYR_KITCHEN_FOOD,
		// Token: 0x04002BFF RID: 11263
		PIC_KENKENPA_LP,
		// Token: 0x04002C00 RID: 11264
		PIC_KENKENPA_ED,
		// Token: 0x04002C01 RID: 11265
		PIC_KENKENPA_JOY,
		// Token: 0x04002C02 RID: 11266
		PIC_HANETSUKI_WAIT,
		// Token: 0x04002C03 RID: 11267
		PIC_HANETSUKI_HIT_1,
		// Token: 0x04002C04 RID: 11268
		PIC_HANETSUKI_HIT_2,
		// Token: 0x04002C05 RID: 11269
		PIC_HANETSUKI_HIT_3,
		// Token: 0x04002C06 RID: 11270
		PIC_HANETSUKI_CALL,
		// Token: 0x04002C07 RID: 11271
		PIC_HANETSUKI_WAIT_NOHAND,
		// Token: 0x04002C08 RID: 11272
		PIC_HANETSUKI_HIT_NOHAND,
		// Token: 0x04002C09 RID: 11273
		PIC_HANETSUKI_CALL_NOHAND,
		// Token: 0x04002C0A RID: 11274
		PIC_TRAIN_WAIT,
		// Token: 0x04002C0B RID: 11275
		PIC_TRAIN_CALL,
		// Token: 0x04002C0C RID: 11276
		PIC_TRAIN_WALK_ST,
		// Token: 0x04002C0D RID: 11277
		PIC_TRAIN_WALK_LP_1,
		// Token: 0x04002C0E RID: 11278
		PIC_TRAIN_WALK_LP_2,
		// Token: 0x04002C0F RID: 11279
		PIC_TRAIN_WALK_EN,
		// Token: 0x04002C10 RID: 11280
		PIC_SNOWMAN_1_ST,
		// Token: 0x04002C11 RID: 11281
		PIC_SNOWMAN_1_LP,
		// Token: 0x04002C12 RID: 11282
		PIC_SNOWMAN_1_ED,
		// Token: 0x04002C13 RID: 11283
		PIC_SNOWMAN_2_ST,
		// Token: 0x04002C14 RID: 11284
		PIC_SNOWMAN_2_LP,
		// Token: 0x04002C15 RID: 11285
		PIC_SNOWMAN_2_ED,
		// Token: 0x04002C16 RID: 11286
		PIC_SNOWROLING_ST,
		// Token: 0x04002C17 RID: 11287
		PIC_SNOWROLING_LP,
		// Token: 0x04002C18 RID: 11288
		PIC_SNOWROLING_EN,
		// Token: 0x04002C19 RID: 11289
		PIC_HARDING_ST,
		// Token: 0x04002C1A RID: 11290
		PIC_HARDING_LP,
		// Token: 0x04002C1B RID: 11291
		PIC_HARDING_EN,
		// Token: 0x04002C1C RID: 11292
		LAUGH,
		// Token: 0x04002C1D RID: 11293
		LAUGH_WAT,
		// Token: 0x04002C1E RID: 11294
		MYR_NOTE_1,
		// Token: 0x04002C1F RID: 11295
		MYR_NOTE_2,
		// Token: 0x04002C20 RID: 11296
		MYR_BANNER_1,
		// Token: 0x04002C21 RID: 11297
		MYR_BANNER_2,
		// Token: 0x04002C22 RID: 11298
		MYR_GARDEN_WATER,
		// Token: 0x04002C23 RID: 11299
		MYR_GARDEN_SEE,
		// Token: 0x04002C24 RID: 11300
		MYR_GARDEN_SEE_NOHAND,
		// Token: 0x04002C25 RID: 11301
		MYR_XYLOPHONE_PLAY_DUO,
		// Token: 0x04002C26 RID: 11302
		MYR_XYLOPHONE_SEE_DUO,
		// Token: 0x04002C27 RID: 11303
		MYR_XYLOPHONE_PLAY_SOLO,
		// Token: 0x04002C28 RID: 11304
		MYR_CURRY_1,
		// Token: 0x04002C29 RID: 11305
		MYR_CURRY_2,
		// Token: 0x04002C2A RID: 11306
		MYR_CURRY_3,
		// Token: 0x04002C2B RID: 11307
		MYR_CURRY_4,
		// Token: 0x04002C2C RID: 11308
		MYR_KITCHEN_GREENTEA,
		// Token: 0x04002C2D RID: 11309
		MYR_TENT_TALK,
		// Token: 0x04002C2E RID: 11310
		LAUGH_SHORT,
		// Token: 0x04002C2F RID: 11311
		MYR_COOK,
		// Token: 0x04002C30 RID: 11312
		MYR_JPNTEA_1,
		// Token: 0x04002C31 RID: 11313
		MYR_JPNTEA_2,
		// Token: 0x04002C32 RID: 11314
		MYR_SWEETS_1,
		// Token: 0x04002C33 RID: 11315
		MYR_SWEETS_2,
		// Token: 0x04002C34 RID: 11316
		MYR_CAR_1,
		// Token: 0x04002C35 RID: 11317
		MYR_CAR_SEE,
		// Token: 0x04002C36 RID: 11318
		MYR_CAR_SEE_NOHAND,
		// Token: 0x04002C37 RID: 11319
		MYR_SAND_PLAY_1,
		// Token: 0x04002C38 RID: 11320
		MYR_SAND_PLAY_2,
		// Token: 0x04002C39 RID: 11321
		MYR_SAND_PLAY_3,
		// Token: 0x04002C3A RID: 11322
		MYR_SAND_PLAY_4,
		// Token: 0x04002C3B RID: 11323
		MYR_KAKIZOME_1,
		// Token: 0x04002C3C RID: 11324
		MYR_KAKIZOME_2,
		// Token: 0x04002C3D RID: 11325
		MYR_PLUSH_SEE,
		// Token: 0x04002C3E RID: 11326
		ARRAY_MAX
	}

	// Token: 0x020005F5 RID: 1525
	public enum MotionPersonalityType
	{
		// Token: 0x04002C40 RID: 11328
		INVALID,
		// Token: 0x04002C41 RID: 11329
		TYPE_A,
		// Token: 0x04002C42 RID: 11330
		TYPE_B,
		// Token: 0x04002C43 RID: 11331
		TYPE_C,
		// Token: 0x04002C44 RID: 11332
		TYPE_D,
		// Token: 0x04002C45 RID: 11333
		TYPE_E,
		// Token: 0x04002C46 RID: 11334
		TYPE_F,
		// Token: 0x04002C47 RID: 11335
		TYPE_G,
		// Token: 0x04002C48 RID: 11336
		TYPE_H,
		// Token: 0x04002C49 RID: 11337
		TYPE_I,
		// Token: 0x04002C4A RID: 11338
		TYPE_J,
		// Token: 0x04002C4B RID: 11339
		TYPE_K
	}

	// Token: 0x020005F6 RID: 1526
	public enum MotionSettingFilterType
	{
		// Token: 0x04002C4D RID: 11341
		NONE,
		// Token: 0x04002C4E RID: 11342
		CONSIDERATION,
		// Token: 0x04002C4F RID: 11343
		UNIQUE,
		// Token: 0x04002C50 RID: 11344
		IF_EXIST_UNIQUE,
		// Token: 0x04002C51 RID: 11345
		DONT,
		// Token: 0x04002C52 RID: 11346
		DO,
		// Token: 0x04002C53 RID: 11347
		ATTENTION,
		// Token: 0x04002C54 RID: 11348
		ONLY_FLY_FRIENDS,
		// Token: 0x04002C55 RID: 11349
		IF_NECESSARY
	}
}
