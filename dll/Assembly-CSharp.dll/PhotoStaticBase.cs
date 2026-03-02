using System;
using SGNFW.Mst;

// Token: 0x020000D3 RID: 211
public class PhotoStaticBase
{
	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x06000963 RID: 2403 RVA: 0x0003A9C0 File Offset: 0x00038BC0
	// (set) Token: 0x06000964 RID: 2404 RVA: 0x0003A9C8 File Offset: 0x00038BC8
	public int id { get; private set; }

	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x06000965 RID: 2405 RVA: 0x0003A9D1 File Offset: 0x00038BD1
	// (set) Token: 0x06000966 RID: 2406 RVA: 0x0003A9D9 File Offset: 0x00038BD9
	public string photoName { get; private set; }

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x06000967 RID: 2407 RVA: 0x0003A9E2 File Offset: 0x00038BE2
	// (set) Token: 0x06000968 RID: 2408 RVA: 0x0003A9EA File Offset: 0x00038BEA
	public string Reading { get; private set; }

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x06000969 RID: 2409 RVA: 0x0003A9F3 File Offset: 0x00038BF3
	// (set) Token: 0x0600096A RID: 2410 RVA: 0x0003A9FB File Offset: 0x00038BFB
	public string illustrator { get; private set; }

	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x0600096B RID: 2411 RVA: 0x0003AA04 File Offset: 0x00038C04
	// (set) Token: 0x0600096C RID: 2412 RVA: 0x0003AA0C File Offset: 0x00038C0C
	public string illustratorafter { get; private set; }

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x0600096D RID: 2413 RVA: 0x0003AA15 File Offset: 0x00038C15
	// (set) Token: 0x0600096E RID: 2414 RVA: 0x0003AA1D File Offset: 0x00038C1D
	public string flavorText { get; private set; }

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x0600096F RID: 2415 RVA: 0x0003AA26 File Offset: 0x00038C26
	// (set) Token: 0x06000970 RID: 2416 RVA: 0x0003AA2E File Offset: 0x00038C2E
	public string flavorTextAfter { get; private set; }

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x06000971 RID: 2417 RVA: 0x0003AA37 File Offset: 0x00038C37
	// (set) Token: 0x06000972 RID: 2418 RVA: 0x0003AA3F File Offset: 0x00038C3F
	public ItemDef.Rarity rarity { get; private set; }

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x06000973 RID: 2419 RVA: 0x0003AA48 File Offset: 0x00038C48
	// (set) Token: 0x06000974 RID: 2420 RVA: 0x0003AA50 File Offset: 0x00038C50
	public int levelTableId { get; private set; }

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x06000975 RID: 2421 RVA: 0x0003AA59 File Offset: 0x00038C59
	// (set) Token: 0x06000976 RID: 2422 RVA: 0x0003AA61 File Offset: 0x00038C61
	public bool imageChangeFlg { get; private set; }

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x06000977 RID: 2423 RVA: 0x0003AA6A File Offset: 0x00038C6A
	// (set) Token: 0x06000978 RID: 2424 RVA: 0x0003AA72 File Offset: 0x00038C72
	public PhotoDef.Type type { get; private set; }

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x06000979 RID: 2425 RVA: 0x0003AA7B File Offset: 0x00038C7B
	// (set) Token: 0x0600097A RID: 2426 RVA: 0x0003AA83 File Offset: 0x00038C83
	public bool kizunaPhotoFlg { get; private set; }

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x0600097B RID: 2427 RVA: 0x0003AA8C File Offset: 0x00038C8C
	// (set) Token: 0x0600097C RID: 2428 RVA: 0x0003AA94 File Offset: 0x00038C94
	public bool forbiddenDiscardFlg { get; private set; }

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x0600097D RID: 2429 RVA: 0x0003AA9D File Offset: 0x00038C9D
	// (set) Token: 0x0600097E RID: 2430 RVA: 0x0003AAA5 File Offset: 0x00038CA5
	public PhotoDef.ExpPhotoType expPhotoType { get; private set; }

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x0600097F RID: 2431 RVA: 0x0003AAAE File Offset: 0x00038CAE
	public bool isForbiddenGrowBase
	{
		get
		{
			return this.type == PhotoDef.Type.OTHER;
		}
	}

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x06000980 RID: 2432 RVA: 0x0003AAB9 File Offset: 0x00038CB9
	public bool isForbiddenEquip
	{
		get
		{
			return this.type == PhotoDef.Type.OTHER;
		}
	}

	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x06000981 RID: 2433 RVA: 0x0003AAC4 File Offset: 0x00038CC4
	// (set) Token: 0x06000982 RID: 2434 RVA: 0x0003AACC File Offset: 0x00038CCC
	public bool isForbiddenUseLimitOverPhoto { get; private set; }

	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x06000983 RID: 2435 RVA: 0x0003AAD5 File Offset: 0x00038CD5
	// (set) Token: 0x06000984 RID: 2436 RVA: 0x0003AADD File Offset: 0x00038CDD
	public PhotoDef.AlbumCategory albumCategory { get; private set; }

	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x06000985 RID: 2437 RVA: 0x0003AAE6 File Offset: 0x00038CE6
	// (set) Token: 0x06000986 RID: 2438 RVA: 0x0003AAEE File Offset: 0x00038CEE
	public string InfoGetText { get; private set; }

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x06000987 RID: 2439 RVA: 0x0003AAF7 File Offset: 0x00038CF7
	// (set) Token: 0x06000988 RID: 2440 RVA: 0x0003AAFF File Offset: 0x00038CFF
	public int hpParamLv1 { get; private set; }

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06000989 RID: 2441 RVA: 0x0003AB08 File Offset: 0x00038D08
	// (set) Token: 0x0600098A RID: 2442 RVA: 0x0003AB10 File Offset: 0x00038D10
	public int hpParamLvMiddle { get; private set; }

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x0600098B RID: 2443 RVA: 0x0003AB19 File Offset: 0x00038D19
	// (set) Token: 0x0600098C RID: 2444 RVA: 0x0003AB21 File Offset: 0x00038D21
	public int hpLvMiddleNum { get; private set; }

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x0600098D RID: 2445 RVA: 0x0003AB2A File Offset: 0x00038D2A
	// (set) Token: 0x0600098E RID: 2446 RVA: 0x0003AB32 File Offset: 0x00038D32
	public int hpParamLvMax { get; private set; }

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x0600098F RID: 2447 RVA: 0x0003AB3B File Offset: 0x00038D3B
	// (set) Token: 0x06000990 RID: 2448 RVA: 0x0003AB43 File Offset: 0x00038D43
	public int atkParamLv1 { get; private set; }

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x06000991 RID: 2449 RVA: 0x0003AB4C File Offset: 0x00038D4C
	// (set) Token: 0x06000992 RID: 2450 RVA: 0x0003AB54 File Offset: 0x00038D54
	public int atkParamLvMiddle { get; private set; }

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x06000993 RID: 2451 RVA: 0x0003AB5D File Offset: 0x00038D5D
	// (set) Token: 0x06000994 RID: 2452 RVA: 0x0003AB65 File Offset: 0x00038D65
	public int atkLvMiddleNum { get; private set; }

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x06000995 RID: 2453 RVA: 0x0003AB6E File Offset: 0x00038D6E
	// (set) Token: 0x06000996 RID: 2454 RVA: 0x0003AB76 File Offset: 0x00038D76
	public int atkParamLvMax { get; private set; }

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x06000997 RID: 2455 RVA: 0x0003AB7F File Offset: 0x00038D7F
	// (set) Token: 0x06000998 RID: 2456 RVA: 0x0003AB87 File Offset: 0x00038D87
	public int defParamLv1 { get; private set; }

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x06000999 RID: 2457 RVA: 0x0003AB90 File Offset: 0x00038D90
	// (set) Token: 0x0600099A RID: 2458 RVA: 0x0003AB98 File Offset: 0x00038D98
	public int defParamLvMiddle { get; private set; }

	// Token: 0x170001FD RID: 509
	// (get) Token: 0x0600099B RID: 2459 RVA: 0x0003ABA1 File Offset: 0x00038DA1
	// (set) Token: 0x0600099C RID: 2460 RVA: 0x0003ABA9 File Offset: 0x00038DA9
	public int defLvMiddleNum { get; private set; }

	// Token: 0x170001FE RID: 510
	// (get) Token: 0x0600099D RID: 2461 RVA: 0x0003ABB2 File Offset: 0x00038DB2
	// (set) Token: 0x0600099E RID: 2462 RVA: 0x0003ABBA File Offset: 0x00038DBA
	public int defParamLvMax { get; private set; }

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x0600099F RID: 2463 RVA: 0x0003ABC3 File Offset: 0x00038DC3
	// (set) Token: 0x060009A0 RID: 2464 RVA: 0x0003ABCB File Offset: 0x00038DCB
	public bool AbilityEffectChange { get; private set; }

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x060009A1 RID: 2465 RVA: 0x0003ABD4 File Offset: 0x00038DD4
	// (set) Token: 0x060009A2 RID: 2466 RVA: 0x0003ABDC File Offset: 0x00038DDC
	public DateTime StartDateTime { get; private set; }

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x060009A3 RID: 2467 RVA: 0x0003ABE5 File Offset: 0x00038DE5
	// (set) Token: 0x060009A4 RID: 2468 RVA: 0x0003ABED File Offset: 0x00038DED
	public DateTime qrDispStartTime { get; private set; }

	// Token: 0x060009A5 RID: 2469 RVA: 0x0003ABF8 File Offset: 0x00038DF8
	public PhotoStaticBase(MstPhotoData mst)
	{
		this.id = mst.id;
		this.type = (PhotoDef.Type)mst.type;
		this.photoName = mst.name;
		this.Reading = mst.reading;
		this.illustrator = mst.illustratorName;
		this.illustratorafter = mst.illustratorNameAfter;
		this.flavorText = mst.flavorTextBefore;
		this.flavorTextAfter = mst.flavorTextAfter;
		this.rarity = (ItemDef.Rarity)mst.rarity;
		this.levelTableId = mst.levelTableId;
		this.expPhotoType = (PhotoDef.ExpPhotoType)mst.expPhotoFlg;
		this.forbiddenDiscardFlg = mst.noDestoryFlg != 0;
		this.isForbiddenUseLimitOverPhoto = mst.noUseLimitOverPhotoFlg != 0;
		this.imageChangeFlg = mst.imgFlg != 0;
		this.kizunaPhotoFlg = mst.kizunaPhotoFlg != 0;
		this.albumCategory = (PhotoDef.AlbumCategory)mst.albumCategory;
		this.InfoGetText = mst.infoGettext;
		this.hpParamLv1 = mst.hpParamLv1;
		this.hpParamLvMiddle = mst.hpParamLvMiddle;
		this.hpLvMiddleNum = mst.hpLvMiddleNum;
		this.hpParamLvMax = mst.hpParamLvMax;
		this.atkParamLv1 = mst.atkParamLv1;
		this.atkParamLvMiddle = mst.atkParamLvMiddle;
		this.atkLvMiddleNum = mst.atkLvMiddleNum;
		this.atkParamLvMax = mst.atkParamLvMax;
		this.defParamLv1 = mst.defParamLv1;
		this.defParamLvMiddle = mst.defParamLvMiddle;
		this.defLvMiddleNum = mst.defLvMiddleNum;
		this.defParamLvMax = mst.defParamLvMax;
		this.AbilityEffectChange = 1 == mst.abilityEffectChangeFlg;
		this.StartDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.startTime));
		this.qrDispStartTime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.qeDispDatetime));
	}

	// Token: 0x0400079F RID: 1951
	public const int MAX_LEVEL_RANK = 4;
}
