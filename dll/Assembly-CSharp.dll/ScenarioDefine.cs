using System;
using UnityEngine;

public static class ScenarioDefine
{
	public static bool loopSE(ScenarioDefine.SE_TYPE typ)
	{
		switch (typ)
		{
		case ScenarioDefine.SE_TYPE.RAIN:
		case ScenarioDefine.SE_TYPE.NIGHT_LOOP:
		case ScenarioDefine.SE_TYPE.WAVES_LOOP:
		case ScenarioDefine.SE_TYPE.WIND_LOOP:
		case ScenarioDefine.SE_TYPE.DRIP_LOOP:
		case ScenarioDefine.SE_TYPE.BIRD_CRY_LOOP:
			return true;
		}
		return false;
	}

	public static Color GetIntroColor(int i, string s)
	{
		Color color = Color.white;
		if (!ColorUtility.TryParseHtmlString(s, out color))
		{
			color = ((i >= 0 && i < ScenarioDefine.introColor.Length) ? ScenarioDefine.introColor[i] : Color.white);
		}
		return color;
	}

	public static readonly string LOAD_PATH = "Scenario/ScenarioData/";

	public static readonly string[] PREFAB_NAME_PREFIX = new string[] { "Scenario_M_", "Scenario_C_", "Scenario_E_", "Scenario_A_", "Scenario_L_" };

	public static readonly string[] DATA_FOLDER = new string[] { "Main", "Char", "Event", "Another", "Login" };

	public static readonly string[] OrderSerifNames = new string[] { "Normal", "Needle", "Cloud" };

	public static ScenarioDefine.ContrastVignette cv_recollection = new ScenarioDefine.ContrastVignette
	{
		sharpness = 58f,
		darkness = 25f,
		contrast = 0f,
		redCoeff = 0.5f,
		greenCoeff = 0.5f,
		blueCoeff = 0.5f,
		edge = 0f,
		redAmbient = 1f,
		greenAmbient = 1f,
		blueAmbient = 1f
	};

	public static ScenarioDefine.ContrastVignette cv_dirtcloud = new ScenarioDefine.ContrastVignette
	{
		sharpness = -30f,
		darkness = 50f,
		contrast = 0f,
		redCoeff = 0.5f,
		greenCoeff = 0.5f,
		blueCoeff = 0.5f,
		edge = 0f,
		redAmbient = 0.6f,
		greenAmbient = 0.45f,
		blueAmbient = 0.3f
	};

	public static readonly string[] BgmName = new string[]
	{
		"", "prd_bgm0001", "prd_bgm0002", "prd_bgm0003", "prd_bgm0004", "prd_bgm0005", "prd_bgm0006", "prd_bgm0007", "prd_bgm0008", "prd_bgm0009",
		"prd_bgm0010", "prd_bgm0011", "prd_bgm0012", "prd_bgm0013", "prd_bgm0014", "prd_bgm0015", "prd_bgm0016", "prd_bgm0017", "prd_bgm0018", "prd_bgm0019",
		"prd_bgm0020", "prd_bgm0021", "prd_bgm0022", "prd_bgm0023", "prd_bgm0024", "prd_bgm0025", "prd_bgm0026", "prd_bgm0027", "prd_bgm0028", "prd_bgm0029",
		"prd_bgm0030"
	};

	public static readonly int[] BgmSpeed = new int[] { 500, 1000, 2000 };

	public static readonly string[] SeName = new string[]
	{
		"", "prd_se_scenario_cellien_walk", "prd_se_scenario_luckybeast_walk", "prd_se_scenario_monitor_on", "prd_se_scenario_open", "prd_se_scenario_push_grass", "prd_se_scenario_run", "prd_se_scenario_run_grass", "prd_se_scenario_shock", "prd_se_scenario_slash",
		"prd_se_scenario_strike", "prd_se_scenario_walk", "prd_se_scenario_walk_grass", "prd_se_scenario_note", "prd_se_scenario_kirakira", "prd_se_scenario_sweat", "prd_se_scenario_flower", "prd_se_scenario_badmood", "prd_se_scenario_surprisedmark", "prd_se_scenario_questionmark",
		"prd_se_scenario_introduction", "prd_se_scenario_scene_change", "prd_se_scenario_signature_phrase", "prd_se_scenario_handy_goods", "prd_se_scenario_waves", "prd_se_scenario_seagull", "prd_se_scenario_rustling", "prd_se_scenario_photo_auth", "prd_se_scenario_shutter", "prd_se_scenario_sandstar",
		"prd_se_scenario_shine", "prd_se_scenario_seismic", "prd_se_scenario_bell_regret", "prd_se_scenario_quijada", "prd_se_scenario_remind_start", "prd_se_scenario_remind_end", "prd_se_scenario_flicky_walk", "prd_se_scenario_flicky_run", "prd_se_scenario_water_up", "prd_se_scenario_Indulge",
		"prd_se_scenario_rain", "prd_se_scenario_hugeobject_landing", "prd_se_scenario_thunder", "prd_se_scenario_night_loop", "prd_se_scenario_waves_loop", "prd_se_scenario_wind_loop", "prd_se_scenario_wing", "prd_se_scenario_dig", "prd_se_scenario_drip_loop", "prd_se_scenario_bird_cry",
		"prd_se_scenario_bird_cry_loop", "prd_se_scenario_sand_walk", "prd_se_scenario_sand_run"
	};

	private static Color custom01 = new Color(0.5176471f, 0.26666668f, 1f, 1f);

	private static Color testBg = new Color(0.6745098f, 0.5019608f, 0.7490196f, 1f);

	private static Color testText = new Color(0.30588236f, 0.039215688f, 0.41960785f, 1f);

	private static Color testNo = new Color(0.9490196f, 0.83137256f, 1f, 1f);

	private static Color testDoll = new Color(0.9019608f, 0.64705884f, 0.49019608f, 1f);

	private static Color[] introColor = new Color[]
	{
		Color.white,
		Color.red,
		Color.green,
		ScenarioDefine.custom01,
		ScenarioDefine.testBg,
		ScenarioDefine.testText,
		ScenarioDefine.testNo,
		ScenarioDefine.testDoll
	};

	public enum EPISODE_TYPE
	{
		MAIN,
		CHAR,
		EVENT,
		ANOTHER,
		LOGIN
	}

	public enum TYPE
	{
		INITIALISE,
		TITLE,
		SERIF,
		SERIF_MIRAI,
		SPECIAL_AUTH,
		BACKGROUND,
		BGM,
		SE,
		SELECT,
		JUMP,
		LABEL,
		CHARA_CTRL,
		EFFECT,
		ALL_FADE,
		INTRODUCE,
		SERIF_OFF,
		WAIT,
		WINDOW,
		NARRATION,
		PHOTO,
		BG_EFFECT,
		SPECIAL_MOVIE
	}

	public enum CHARA_MOVE
	{
		NONE,
		DISAPPEAR_FADE_OUT,
		DISAPPEAR_TO_RIGHT,
		DISAPPEAR_TO_LEFT,
		DISAPPEAR_TO_UPWARD,
		DISAPPEAR_TO_DOWNWARD,
		APPEAR_FADE_IN,
		APPEAR_FROM_RIGHT,
		APPEAR_FROM_LEFT,
		APPEAR_FROM_UPWARD,
		APPEAR_FROM_DOWNWARD,
		DISAPPEAR_JUMP_OUT,
		APPEAR_JUMP_IN,
		APPEAR_FAST_RIGHT,
		APPEAR_FAST_LEFT,
		DISAPPEAR_FAST_RIGHT,
		DISAPPEAR_FAST_LEFT
	}

	public enum CHARA_POSITION
	{
		FRONT_5_1,
		FRONT_5_2,
		FRONT_5_3,
		FRONT_5_4,
		FRONT_5_5,
		INTRODUCE,
		BACK_5_1,
		BACK_5_2,
		BACK_5_3,
		BACK_5_4,
		BACK_5_5,
		OUTSIDE_5_1,
		OUTSIDE_5_2,
		OUTSIDE_5_4,
		OUTSIDE_5_5,
		BETWEEN_4_1,
		BETWEEN_4_2,
		BETWEEN_4_3,
		BETWEEN_4_4,
		PIP_4_1,
		PIP_4_2,
		PIP_4_3,
		PIP_4_4,
		HIDDEN_LEFT,
		HIDDEN_RIGHT,
		RESERVE_1,
		RESERVE_2,
		RESERVE_3,
		RESERVE_4,
		RESERVE_5
	}

	public enum MOTION_FADE
	{
		NORM_NORM,
		OFF_NORM,
		FIRST_NORM,
		SLOW_NORM,
		NORM_OFF,
		OFF_OFF,
		FIRST_OFF,
		SLOW_OFF,
		NORM_FIRST,
		OFF_FIRST,
		FIRST_FIRST,
		SLOW_FIRST,
		NORM_SLOW,
		OFF_SLOW,
		FIRST_SLOW,
		SLOW_SLOW
	}

	public enum SERIF_TYPE
	{
		NORMAL,
		NEEDLE,
		CLOUD
	}

	public enum TEXT_EFFECT_TYPE
	{
		NORMAL,
		SHAKE,
		SHAKE2
	}

	public enum TEXT_SIZE_TYPE
	{
		SIZE100,
		SIZE125,
		SIZE150,
		SIZE175,
		SIZE200
	}

	public enum BG_EFFECT_TYPE
	{
		NORMAL,
		QUAKE,
		SHAKE_ONCE,
		ZOOM_NONE,
		ZOOM1,
		ZOOM2,
		ZOOM3,
		ZOOM4,
		ZOOM5,
		ZOOM6,
		ZOOM7,
		ZOOM8,
		ZOOM9
	}

	public enum EFFECT_TYPE
	{
		NONE,
		FOCUS,
		RECOLLECTION,
		FLASH,
		SANDSTAR,
		SHINE,
		RAIN,
		DIRT_CLOUD,
		STEAM,
		SNOW,
		DARKNESS,
		SPEEDLINE_UP,
		SPEEDLINE_DOWN,
		SPEEDLINE_LEFT,
		SPEEDLINE_RIGHT
	}

	public class ContrastVignette
	{
		public float sharpness = 32f;

		public float darkness = 28f;

		public float contrast = 20f;

		public float redCoeff = 0.5f;

		public float greenCoeff = 0.5f;

		public float blueCoeff = 0.5f;

		public float edge;

		public float redAmbient;

		public float greenAmbient;

		public float blueAmbient;
	}

	public enum LABEL_TYPE
	{
		LABEL_A,
		LABEL_B,
		LABEL_C,
		LABEL_D,
		LABEL_E,
		LABEL_F,
		LABEL_G,
		LABEL_H,
		LABEL_I,
		LABEL_J,
		LABEL_K,
		LABEL_L,
		LABEL_M,
		LABEL_N,
		LABEL_O
	}

	public enum FADE_LAYER_TYPE
	{
		OVER,
		UNDER
	}

	public enum FADE_TYPE
	{
		NONE,
		KEMONO_FADE,
		BLACK_IN,
		BLACK_OUT,
		WHITE_IN,
		WHITE_OUT,
		RED_IN,
		RED_OUT,
		SOILSMOKE,
		SMOKE
	}

	public enum SPEED_TYPE
	{
		NORMAL,
		LATE,
		FAST
	}

	public enum BGM_TYPE
	{
		NONE,
		BGM_0001,
		BGM_0002,
		BGM_0003,
		BGM_0004,
		BGM_0005,
		BGM_0006,
		BGM_0007,
		BGM_0008,
		BGM_0009,
		BGM_0010,
		BGM_0011,
		BGM_0012,
		BGM_0013,
		BGM_0014,
		BGM_0015,
		BGM_0016,
		BGM_0017,
		BGM_0018,
		BGM_0019,
		BGM_0020,
		BGM_0021,
		BGM_0022,
		BGM_0023,
		BGM_0024,
		BGM_0025,
		BGM_0026,
		BGM_0027,
		BGM_0028,
		BGM_0029,
		BGM_0030
	}

	public enum BGM_FADE_TYPE
	{
		NONE,
		FADE_IN,
		FADE_OUT
	}

	public enum BGM_SPEED_TYPE
	{
		SHORT,
		MIDDLE,
		LONG
	}

	public enum SE_TYPE
	{
		NONE,
		CELLIEN_WALK,
		LUCKYBEAST_WALK,
		MONITOR_ON,
		OPEN,
		PUSH_GRASS,
		RUN,
		RUN_GRASS,
		SHOCK,
		SLASH,
		STRIKE,
		WALK,
		WALK_GRASS,
		NOTE,
		KIRAKIRA,
		SWEAT,
		FLOWER,
		BADMOOD,
		SURPRISED_MARK,
		QUESTION_MARK,
		INTRODUCE,
		KEMONO_FADE,
		EMPHASIS,
		HANDY_GOODS,
		WAVES,
		SEAGULL,
		RUSTLING,
		PHOTO,
		SHUTTER,
		SANDSTAR,
		SHINE,
		SEISMIC,
		BELL_REGRET,
		QUIJADA,
		REMIND_START,
		REMIND_END,
		FLICKY_WALK,
		FLICKY_RUN,
		WATER_UP,
		INDULGE,
		RAIN,
		LANDING,
		THUNDER,
		NIGHT_LOOP,
		WAVES_LOOP,
		WIND_LOOP,
		WING,
		DIG,
		DRIP_LOOP,
		BIRD_CRY,
		BIRD_CRY_LOOP,
		WALK_SAND,
		RUN_SAND
	}

	public enum INTRODUCE_COLOR
	{
		WHITE,
		RED,
		GREEN,
		TEST_COLOR,
		TEST_BG,
		TEST_TEXT,
		TEST_NO,
		TEST_DOLL
	}
}
