using System;
using UnityEngine;

// Token: 0x0200010F RID: 271
public static class ScenarioDefine
{
	// Token: 0x06000D05 RID: 3333 RVA: 0x000516E7 File Offset: 0x0004F8E7
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

	// Token: 0x06000D06 RID: 3334 RVA: 0x00051724 File Offset: 0x0004F924
	public static Color GetIntroColor(int i, string s)
	{
		Color color = Color.white;
		if (!ColorUtility.TryParseHtmlString(s, out color))
		{
			color = ((i >= 0 && i < ScenarioDefine.introColor.Length) ? ScenarioDefine.introColor[i] : Color.white);
		}
		return color;
	}

	// Token: 0x04000A63 RID: 2659
	public static readonly string LOAD_PATH = "Scenario/ScenarioData/";

	// Token: 0x04000A64 RID: 2660
	public static readonly string[] PREFAB_NAME_PREFIX = new string[] { "Scenario_M_", "Scenario_C_", "Scenario_E_", "Scenario_A_", "Scenario_L_" };

	// Token: 0x04000A65 RID: 2661
	public static readonly string[] DATA_FOLDER = new string[] { "Main", "Char", "Event", "Another", "Login" };

	// Token: 0x04000A66 RID: 2662
	public static readonly string[] OrderSerifNames = new string[] { "Normal", "Needle", "Cloud" };

	// Token: 0x04000A67 RID: 2663
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

	// Token: 0x04000A68 RID: 2664
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

	// Token: 0x04000A69 RID: 2665
	public static readonly string[] BgmName = new string[]
	{
		"", "prd_bgm0001", "prd_bgm0002", "prd_bgm0003", "prd_bgm0004", "prd_bgm0005", "prd_bgm0006", "prd_bgm0007", "prd_bgm0008", "prd_bgm0009",
		"prd_bgm0010", "prd_bgm0011", "prd_bgm0012", "prd_bgm0013", "prd_bgm0014", "prd_bgm0015", "prd_bgm0016", "prd_bgm0017", "prd_bgm0018", "prd_bgm0019",
		"prd_bgm0020", "prd_bgm0021", "prd_bgm0022", "prd_bgm0023", "prd_bgm0024", "prd_bgm0025", "prd_bgm0026", "prd_bgm0027", "prd_bgm0028", "prd_bgm0029",
		"prd_bgm0030"
	};

	// Token: 0x04000A6A RID: 2666
	public static readonly int[] BgmSpeed = new int[] { 500, 1000, 2000 };

	// Token: 0x04000A6B RID: 2667
	public static readonly string[] SeName = new string[]
	{
		"", "prd_se_scenario_cellien_walk", "prd_se_scenario_luckybeast_walk", "prd_se_scenario_monitor_on", "prd_se_scenario_open", "prd_se_scenario_push_grass", "prd_se_scenario_run", "prd_se_scenario_run_grass", "prd_se_scenario_shock", "prd_se_scenario_slash",
		"prd_se_scenario_strike", "prd_se_scenario_walk", "prd_se_scenario_walk_grass", "prd_se_scenario_note", "prd_se_scenario_kirakira", "prd_se_scenario_sweat", "prd_se_scenario_flower", "prd_se_scenario_badmood", "prd_se_scenario_surprisedmark", "prd_se_scenario_questionmark",
		"prd_se_scenario_introduction", "prd_se_scenario_scene_change", "prd_se_scenario_signature_phrase", "prd_se_scenario_handy_goods", "prd_se_scenario_waves", "prd_se_scenario_seagull", "prd_se_scenario_rustling", "prd_se_scenario_photo_auth", "prd_se_scenario_shutter", "prd_se_scenario_sandstar",
		"prd_se_scenario_shine", "prd_se_scenario_seismic", "prd_se_scenario_bell_regret", "prd_se_scenario_quijada", "prd_se_scenario_remind_start", "prd_se_scenario_remind_end", "prd_se_scenario_flicky_walk", "prd_se_scenario_flicky_run", "prd_se_scenario_water_up", "prd_se_scenario_Indulge",
		"prd_se_scenario_rain", "prd_se_scenario_hugeobject_landing", "prd_se_scenario_thunder", "prd_se_scenario_night_loop", "prd_se_scenario_waves_loop", "prd_se_scenario_wind_loop", "prd_se_scenario_wing", "prd_se_scenario_dig", "prd_se_scenario_drip_loop", "prd_se_scenario_bird_cry",
		"prd_se_scenario_bird_cry_loop", "prd_se_scenario_sand_walk", "prd_se_scenario_sand_run"
	};

	// Token: 0x04000A6C RID: 2668
	private static Color custom01 = new Color(0.5176471f, 0.26666668f, 1f, 1f);

	// Token: 0x04000A6D RID: 2669
	private static Color testBg = new Color(0.6745098f, 0.5019608f, 0.7490196f, 1f);

	// Token: 0x04000A6E RID: 2670
	private static Color testText = new Color(0.30588236f, 0.039215688f, 0.41960785f, 1f);

	// Token: 0x04000A6F RID: 2671
	private static Color testNo = new Color(0.9490196f, 0.83137256f, 1f, 1f);

	// Token: 0x04000A70 RID: 2672
	private static Color testDoll = new Color(0.9019608f, 0.64705884f, 0.49019608f, 1f);

	// Token: 0x04000A71 RID: 2673
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

	// Token: 0x02000850 RID: 2128
	public enum EPISODE_TYPE
	{
		// Token: 0x04003774 RID: 14196
		MAIN,
		// Token: 0x04003775 RID: 14197
		CHAR,
		// Token: 0x04003776 RID: 14198
		EVENT,
		// Token: 0x04003777 RID: 14199
		ANOTHER,
		// Token: 0x04003778 RID: 14200
		LOGIN
	}

	// Token: 0x02000851 RID: 2129
	public enum TYPE
	{
		// Token: 0x0400377A RID: 14202
		INITIALISE,
		// Token: 0x0400377B RID: 14203
		TITLE,
		// Token: 0x0400377C RID: 14204
		SERIF,
		// Token: 0x0400377D RID: 14205
		SERIF_MIRAI,
		// Token: 0x0400377E RID: 14206
		SPECIAL_AUTH,
		// Token: 0x0400377F RID: 14207
		BACKGROUND,
		// Token: 0x04003780 RID: 14208
		BGM,
		// Token: 0x04003781 RID: 14209
		SE,
		// Token: 0x04003782 RID: 14210
		SELECT,
		// Token: 0x04003783 RID: 14211
		JUMP,
		// Token: 0x04003784 RID: 14212
		LABEL,
		// Token: 0x04003785 RID: 14213
		CHARA_CTRL,
		// Token: 0x04003786 RID: 14214
		EFFECT,
		// Token: 0x04003787 RID: 14215
		ALL_FADE,
		// Token: 0x04003788 RID: 14216
		INTRODUCE,
		// Token: 0x04003789 RID: 14217
		SERIF_OFF,
		// Token: 0x0400378A RID: 14218
		WAIT,
		// Token: 0x0400378B RID: 14219
		WINDOW,
		// Token: 0x0400378C RID: 14220
		NARRATION,
		// Token: 0x0400378D RID: 14221
		PHOTO,
		// Token: 0x0400378E RID: 14222
		BG_EFFECT,
		// Token: 0x0400378F RID: 14223
		SPECIAL_MOVIE
	}

	// Token: 0x02000852 RID: 2130
	public enum CHARA_MOVE
	{
		// Token: 0x04003791 RID: 14225
		NONE,
		// Token: 0x04003792 RID: 14226
		DISAPPEAR_FADE_OUT,
		// Token: 0x04003793 RID: 14227
		DISAPPEAR_TO_RIGHT,
		// Token: 0x04003794 RID: 14228
		DISAPPEAR_TO_LEFT,
		// Token: 0x04003795 RID: 14229
		DISAPPEAR_TO_UPWARD,
		// Token: 0x04003796 RID: 14230
		DISAPPEAR_TO_DOWNWARD,
		// Token: 0x04003797 RID: 14231
		APPEAR_FADE_IN,
		// Token: 0x04003798 RID: 14232
		APPEAR_FROM_RIGHT,
		// Token: 0x04003799 RID: 14233
		APPEAR_FROM_LEFT,
		// Token: 0x0400379A RID: 14234
		APPEAR_FROM_UPWARD,
		// Token: 0x0400379B RID: 14235
		APPEAR_FROM_DOWNWARD,
		// Token: 0x0400379C RID: 14236
		DISAPPEAR_JUMP_OUT,
		// Token: 0x0400379D RID: 14237
		APPEAR_JUMP_IN,
		// Token: 0x0400379E RID: 14238
		APPEAR_FAST_RIGHT,
		// Token: 0x0400379F RID: 14239
		APPEAR_FAST_LEFT,
		// Token: 0x040037A0 RID: 14240
		DISAPPEAR_FAST_RIGHT,
		// Token: 0x040037A1 RID: 14241
		DISAPPEAR_FAST_LEFT
	}

	// Token: 0x02000853 RID: 2131
	public enum CHARA_POSITION
	{
		// Token: 0x040037A3 RID: 14243
		FRONT_5_1,
		// Token: 0x040037A4 RID: 14244
		FRONT_5_2,
		// Token: 0x040037A5 RID: 14245
		FRONT_5_3,
		// Token: 0x040037A6 RID: 14246
		FRONT_5_4,
		// Token: 0x040037A7 RID: 14247
		FRONT_5_5,
		// Token: 0x040037A8 RID: 14248
		INTRODUCE,
		// Token: 0x040037A9 RID: 14249
		BACK_5_1,
		// Token: 0x040037AA RID: 14250
		BACK_5_2,
		// Token: 0x040037AB RID: 14251
		BACK_5_3,
		// Token: 0x040037AC RID: 14252
		BACK_5_4,
		// Token: 0x040037AD RID: 14253
		BACK_5_5,
		// Token: 0x040037AE RID: 14254
		OUTSIDE_5_1,
		// Token: 0x040037AF RID: 14255
		OUTSIDE_5_2,
		// Token: 0x040037B0 RID: 14256
		OUTSIDE_5_4,
		// Token: 0x040037B1 RID: 14257
		OUTSIDE_5_5,
		// Token: 0x040037B2 RID: 14258
		BETWEEN_4_1,
		// Token: 0x040037B3 RID: 14259
		BETWEEN_4_2,
		// Token: 0x040037B4 RID: 14260
		BETWEEN_4_3,
		// Token: 0x040037B5 RID: 14261
		BETWEEN_4_4,
		// Token: 0x040037B6 RID: 14262
		PIP_4_1,
		// Token: 0x040037B7 RID: 14263
		PIP_4_2,
		// Token: 0x040037B8 RID: 14264
		PIP_4_3,
		// Token: 0x040037B9 RID: 14265
		PIP_4_4,
		// Token: 0x040037BA RID: 14266
		HIDDEN_LEFT,
		// Token: 0x040037BB RID: 14267
		HIDDEN_RIGHT,
		// Token: 0x040037BC RID: 14268
		RESERVE_1,
		// Token: 0x040037BD RID: 14269
		RESERVE_2,
		// Token: 0x040037BE RID: 14270
		RESERVE_3,
		// Token: 0x040037BF RID: 14271
		RESERVE_4,
		// Token: 0x040037C0 RID: 14272
		RESERVE_5
	}

	// Token: 0x02000854 RID: 2132
	public enum MOTION_FADE
	{
		// Token: 0x040037C2 RID: 14274
		NORM_NORM,
		// Token: 0x040037C3 RID: 14275
		OFF_NORM,
		// Token: 0x040037C4 RID: 14276
		FIRST_NORM,
		// Token: 0x040037C5 RID: 14277
		SLOW_NORM,
		// Token: 0x040037C6 RID: 14278
		NORM_OFF,
		// Token: 0x040037C7 RID: 14279
		OFF_OFF,
		// Token: 0x040037C8 RID: 14280
		FIRST_OFF,
		// Token: 0x040037C9 RID: 14281
		SLOW_OFF,
		// Token: 0x040037CA RID: 14282
		NORM_FIRST,
		// Token: 0x040037CB RID: 14283
		OFF_FIRST,
		// Token: 0x040037CC RID: 14284
		FIRST_FIRST,
		// Token: 0x040037CD RID: 14285
		SLOW_FIRST,
		// Token: 0x040037CE RID: 14286
		NORM_SLOW,
		// Token: 0x040037CF RID: 14287
		OFF_SLOW,
		// Token: 0x040037D0 RID: 14288
		FIRST_SLOW,
		// Token: 0x040037D1 RID: 14289
		SLOW_SLOW
	}

	// Token: 0x02000855 RID: 2133
	public enum SERIF_TYPE
	{
		// Token: 0x040037D3 RID: 14291
		NORMAL,
		// Token: 0x040037D4 RID: 14292
		NEEDLE,
		// Token: 0x040037D5 RID: 14293
		CLOUD
	}

	// Token: 0x02000856 RID: 2134
	public enum TEXT_EFFECT_TYPE
	{
		// Token: 0x040037D7 RID: 14295
		NORMAL,
		// Token: 0x040037D8 RID: 14296
		SHAKE,
		// Token: 0x040037D9 RID: 14297
		SHAKE2
	}

	// Token: 0x02000857 RID: 2135
	public enum TEXT_SIZE_TYPE
	{
		// Token: 0x040037DB RID: 14299
		SIZE100,
		// Token: 0x040037DC RID: 14300
		SIZE125,
		// Token: 0x040037DD RID: 14301
		SIZE150,
		// Token: 0x040037DE RID: 14302
		SIZE175,
		// Token: 0x040037DF RID: 14303
		SIZE200
	}

	// Token: 0x02000858 RID: 2136
	public enum BG_EFFECT_TYPE
	{
		// Token: 0x040037E1 RID: 14305
		NORMAL,
		// Token: 0x040037E2 RID: 14306
		QUAKE,
		// Token: 0x040037E3 RID: 14307
		SHAKE_ONCE,
		// Token: 0x040037E4 RID: 14308
		ZOOM_NONE,
		// Token: 0x040037E5 RID: 14309
		ZOOM1,
		// Token: 0x040037E6 RID: 14310
		ZOOM2,
		// Token: 0x040037E7 RID: 14311
		ZOOM3,
		// Token: 0x040037E8 RID: 14312
		ZOOM4,
		// Token: 0x040037E9 RID: 14313
		ZOOM5,
		// Token: 0x040037EA RID: 14314
		ZOOM6,
		// Token: 0x040037EB RID: 14315
		ZOOM7,
		// Token: 0x040037EC RID: 14316
		ZOOM8,
		// Token: 0x040037ED RID: 14317
		ZOOM9
	}

	// Token: 0x02000859 RID: 2137
	public enum EFFECT_TYPE
	{
		// Token: 0x040037EF RID: 14319
		NONE,
		// Token: 0x040037F0 RID: 14320
		FOCUS,
		// Token: 0x040037F1 RID: 14321
		RECOLLECTION,
		// Token: 0x040037F2 RID: 14322
		FLASH,
		// Token: 0x040037F3 RID: 14323
		SANDSTAR,
		// Token: 0x040037F4 RID: 14324
		SHINE,
		// Token: 0x040037F5 RID: 14325
		RAIN,
		// Token: 0x040037F6 RID: 14326
		DIRT_CLOUD,
		// Token: 0x040037F7 RID: 14327
		STEAM,
		// Token: 0x040037F8 RID: 14328
		SNOW,
		// Token: 0x040037F9 RID: 14329
		DARKNESS,
		// Token: 0x040037FA RID: 14330
		SPEEDLINE_UP,
		// Token: 0x040037FB RID: 14331
		SPEEDLINE_DOWN,
		// Token: 0x040037FC RID: 14332
		SPEEDLINE_LEFT,
		// Token: 0x040037FD RID: 14333
		SPEEDLINE_RIGHT
	}

	// Token: 0x0200085A RID: 2138
	public class ContrastVignette
	{
		// Token: 0x040037FE RID: 14334
		public float sharpness = 32f;

		// Token: 0x040037FF RID: 14335
		public float darkness = 28f;

		// Token: 0x04003800 RID: 14336
		public float contrast = 20f;

		// Token: 0x04003801 RID: 14337
		public float redCoeff = 0.5f;

		// Token: 0x04003802 RID: 14338
		public float greenCoeff = 0.5f;

		// Token: 0x04003803 RID: 14339
		public float blueCoeff = 0.5f;

		// Token: 0x04003804 RID: 14340
		public float edge;

		// Token: 0x04003805 RID: 14341
		public float redAmbient;

		// Token: 0x04003806 RID: 14342
		public float greenAmbient;

		// Token: 0x04003807 RID: 14343
		public float blueAmbient;
	}

	// Token: 0x0200085B RID: 2139
	public enum LABEL_TYPE
	{
		// Token: 0x04003809 RID: 14345
		LABEL_A,
		// Token: 0x0400380A RID: 14346
		LABEL_B,
		// Token: 0x0400380B RID: 14347
		LABEL_C,
		// Token: 0x0400380C RID: 14348
		LABEL_D,
		// Token: 0x0400380D RID: 14349
		LABEL_E,
		// Token: 0x0400380E RID: 14350
		LABEL_F,
		// Token: 0x0400380F RID: 14351
		LABEL_G,
		// Token: 0x04003810 RID: 14352
		LABEL_H,
		// Token: 0x04003811 RID: 14353
		LABEL_I,
		// Token: 0x04003812 RID: 14354
		LABEL_J,
		// Token: 0x04003813 RID: 14355
		LABEL_K,
		// Token: 0x04003814 RID: 14356
		LABEL_L,
		// Token: 0x04003815 RID: 14357
		LABEL_M,
		// Token: 0x04003816 RID: 14358
		LABEL_N,
		// Token: 0x04003817 RID: 14359
		LABEL_O
	}

	// Token: 0x0200085C RID: 2140
	public enum FADE_LAYER_TYPE
	{
		// Token: 0x04003819 RID: 14361
		OVER,
		// Token: 0x0400381A RID: 14362
		UNDER
	}

	// Token: 0x0200085D RID: 2141
	public enum FADE_TYPE
	{
		// Token: 0x0400381C RID: 14364
		NONE,
		// Token: 0x0400381D RID: 14365
		KEMONO_FADE,
		// Token: 0x0400381E RID: 14366
		BLACK_IN,
		// Token: 0x0400381F RID: 14367
		BLACK_OUT,
		// Token: 0x04003820 RID: 14368
		WHITE_IN,
		// Token: 0x04003821 RID: 14369
		WHITE_OUT,
		// Token: 0x04003822 RID: 14370
		RED_IN,
		// Token: 0x04003823 RID: 14371
		RED_OUT,
		// Token: 0x04003824 RID: 14372
		SOILSMOKE,
		// Token: 0x04003825 RID: 14373
		SMOKE
	}

	// Token: 0x0200085E RID: 2142
	public enum SPEED_TYPE
	{
		// Token: 0x04003827 RID: 14375
		NORMAL,
		// Token: 0x04003828 RID: 14376
		LATE,
		// Token: 0x04003829 RID: 14377
		FAST
	}

	// Token: 0x0200085F RID: 2143
	public enum BGM_TYPE
	{
		// Token: 0x0400382B RID: 14379
		NONE,
		// Token: 0x0400382C RID: 14380
		BGM_0001,
		// Token: 0x0400382D RID: 14381
		BGM_0002,
		// Token: 0x0400382E RID: 14382
		BGM_0003,
		// Token: 0x0400382F RID: 14383
		BGM_0004,
		// Token: 0x04003830 RID: 14384
		BGM_0005,
		// Token: 0x04003831 RID: 14385
		BGM_0006,
		// Token: 0x04003832 RID: 14386
		BGM_0007,
		// Token: 0x04003833 RID: 14387
		BGM_0008,
		// Token: 0x04003834 RID: 14388
		BGM_0009,
		// Token: 0x04003835 RID: 14389
		BGM_0010,
		// Token: 0x04003836 RID: 14390
		BGM_0011,
		// Token: 0x04003837 RID: 14391
		BGM_0012,
		// Token: 0x04003838 RID: 14392
		BGM_0013,
		// Token: 0x04003839 RID: 14393
		BGM_0014,
		// Token: 0x0400383A RID: 14394
		BGM_0015,
		// Token: 0x0400383B RID: 14395
		BGM_0016,
		// Token: 0x0400383C RID: 14396
		BGM_0017,
		// Token: 0x0400383D RID: 14397
		BGM_0018,
		// Token: 0x0400383E RID: 14398
		BGM_0019,
		// Token: 0x0400383F RID: 14399
		BGM_0020,
		// Token: 0x04003840 RID: 14400
		BGM_0021,
		// Token: 0x04003841 RID: 14401
		BGM_0022,
		// Token: 0x04003842 RID: 14402
		BGM_0023,
		// Token: 0x04003843 RID: 14403
		BGM_0024,
		// Token: 0x04003844 RID: 14404
		BGM_0025,
		// Token: 0x04003845 RID: 14405
		BGM_0026,
		// Token: 0x04003846 RID: 14406
		BGM_0027,
		// Token: 0x04003847 RID: 14407
		BGM_0028,
		// Token: 0x04003848 RID: 14408
		BGM_0029,
		// Token: 0x04003849 RID: 14409
		BGM_0030
	}

	// Token: 0x02000860 RID: 2144
	public enum BGM_FADE_TYPE
	{
		// Token: 0x0400384B RID: 14411
		NONE,
		// Token: 0x0400384C RID: 14412
		FADE_IN,
		// Token: 0x0400384D RID: 14413
		FADE_OUT
	}

	// Token: 0x02000861 RID: 2145
	public enum BGM_SPEED_TYPE
	{
		// Token: 0x0400384F RID: 14415
		SHORT,
		// Token: 0x04003850 RID: 14416
		MIDDLE,
		// Token: 0x04003851 RID: 14417
		LONG
	}

	// Token: 0x02000862 RID: 2146
	public enum SE_TYPE
	{
		// Token: 0x04003853 RID: 14419
		NONE,
		// Token: 0x04003854 RID: 14420
		CELLIEN_WALK,
		// Token: 0x04003855 RID: 14421
		LUCKYBEAST_WALK,
		// Token: 0x04003856 RID: 14422
		MONITOR_ON,
		// Token: 0x04003857 RID: 14423
		OPEN,
		// Token: 0x04003858 RID: 14424
		PUSH_GRASS,
		// Token: 0x04003859 RID: 14425
		RUN,
		// Token: 0x0400385A RID: 14426
		RUN_GRASS,
		// Token: 0x0400385B RID: 14427
		SHOCK,
		// Token: 0x0400385C RID: 14428
		SLASH,
		// Token: 0x0400385D RID: 14429
		STRIKE,
		// Token: 0x0400385E RID: 14430
		WALK,
		// Token: 0x0400385F RID: 14431
		WALK_GRASS,
		// Token: 0x04003860 RID: 14432
		NOTE,
		// Token: 0x04003861 RID: 14433
		KIRAKIRA,
		// Token: 0x04003862 RID: 14434
		SWEAT,
		// Token: 0x04003863 RID: 14435
		FLOWER,
		// Token: 0x04003864 RID: 14436
		BADMOOD,
		// Token: 0x04003865 RID: 14437
		SURPRISED_MARK,
		// Token: 0x04003866 RID: 14438
		QUESTION_MARK,
		// Token: 0x04003867 RID: 14439
		INTRODUCE,
		// Token: 0x04003868 RID: 14440
		KEMONO_FADE,
		// Token: 0x04003869 RID: 14441
		EMPHASIS,
		// Token: 0x0400386A RID: 14442
		HANDY_GOODS,
		// Token: 0x0400386B RID: 14443
		WAVES,
		// Token: 0x0400386C RID: 14444
		SEAGULL,
		// Token: 0x0400386D RID: 14445
		RUSTLING,
		// Token: 0x0400386E RID: 14446
		PHOTO,
		// Token: 0x0400386F RID: 14447
		SHUTTER,
		// Token: 0x04003870 RID: 14448
		SANDSTAR,
		// Token: 0x04003871 RID: 14449
		SHINE,
		// Token: 0x04003872 RID: 14450
		SEISMIC,
		// Token: 0x04003873 RID: 14451
		BELL_REGRET,
		// Token: 0x04003874 RID: 14452
		QUIJADA,
		// Token: 0x04003875 RID: 14453
		REMIND_START,
		// Token: 0x04003876 RID: 14454
		REMIND_END,
		// Token: 0x04003877 RID: 14455
		FLICKY_WALK,
		// Token: 0x04003878 RID: 14456
		FLICKY_RUN,
		// Token: 0x04003879 RID: 14457
		WATER_UP,
		// Token: 0x0400387A RID: 14458
		INDULGE,
		// Token: 0x0400387B RID: 14459
		RAIN,
		// Token: 0x0400387C RID: 14460
		LANDING,
		// Token: 0x0400387D RID: 14461
		THUNDER,
		// Token: 0x0400387E RID: 14462
		NIGHT_LOOP,
		// Token: 0x0400387F RID: 14463
		WAVES_LOOP,
		// Token: 0x04003880 RID: 14464
		WIND_LOOP,
		// Token: 0x04003881 RID: 14465
		WING,
		// Token: 0x04003882 RID: 14466
		DIG,
		// Token: 0x04003883 RID: 14467
		DRIP_LOOP,
		// Token: 0x04003884 RID: 14468
		BIRD_CRY,
		// Token: 0x04003885 RID: 14469
		BIRD_CRY_LOOP,
		// Token: 0x04003886 RID: 14470
		WALK_SAND,
		// Token: 0x04003887 RID: 14471
		RUN_SAND
	}

	// Token: 0x02000863 RID: 2147
	public enum INTRODUCE_COLOR
	{
		// Token: 0x04003889 RID: 14473
		WHITE,
		// Token: 0x0400388A RID: 14474
		RED,
		// Token: 0x0400388B RID: 14475
		GREEN,
		// Token: 0x0400388C RID: 14476
		TEST_COLOR,
		// Token: 0x0400388D RID: 14477
		TEST_BG,
		// Token: 0x0400388E RID: 14478
		TEST_TEXT,
		// Token: 0x0400388F RID: 14479
		TEST_NO,
		// Token: 0x04003890 RID: 14480
		TEST_DOLL
	}
}
