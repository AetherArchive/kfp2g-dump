using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x0200004C RID: 76
[Serializable]
public class CharaDynamicData
{
	// Token: 0x17000047 RID: 71
	// (get) Token: 0x06000212 RID: 530 RVA: 0x00013561 File Offset: 0x00011761
	// (set) Token: 0x06000213 RID: 531 RVA: 0x00013569 File Offset: 0x00011769
	public CharaDynamicData.CharaOwnerType OwnerType { get; set; }

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x06000214 RID: 532 RVA: 0x00013572 File Offset: 0x00011772
	public int limitLevelRankMax
	{
		get
		{
			return CharaPackData.CalcLimitLevel(this.id, this.rank, 0);
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x06000215 RID: 533 RVA: 0x00013586 File Offset: 0x00011786
	public long haveNextExp
	{
		get
		{
			return CharaDynamicData.ConvertExp(this.id, this.exp);
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x06000216 RID: 534 RVA: 0x00013599 File Offset: 0x00011799
	public long haveNextKizunaExp
	{
		get
		{
			return CharaDynamicData.ConvertKizunaExp(this.id, this.kizunaExp);
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x06000217 RID: 535 RVA: 0x000135AC File Offset: 0x000117AC
	public int KizunaLimitLevel
	{
		get
		{
			int maxKizunaLevel = DataManager.DmChara.GetCharaStaticData(this.id).baseData.maxKizunaLevel;
			if (this.kizunaLevel >= maxKizunaLevel)
			{
				return maxKizunaLevel + this.kizunaLimitOverNum;
			}
			return maxKizunaLevel;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x06000218 RID: 536 RVA: 0x000135E7 File Offset: 0x000117E7
	// (set) Token: 0x06000219 RID: 537 RVA: 0x000135F0 File Offset: 0x000117F0
	public int PhotoFrameTotalStep
	{
		get
		{
			return this.innerPhotoFrameTotalStep;
		}
		set
		{
			this.innerPhotoFrameTotalStep = value;
			CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(this.id);
			this.innerPhotoPocketList = new List<CharaDynamicData.PPParam>
			{
				new CharaDynamicData.PPParam
				{
					Flag = (0 < this.PhotoFrameTotalStep),
					Step = this.PhotoFrameTotalStep / 4 + ((0 < this.PhotoFrameTotalStep % 4) ? 1 : 0),
					Hp = 100,
					Atk = 100,
					Def = 100
				},
				new CharaDynamicData.PPParam
				{
					Flag = (1 < this.PhotoFrameTotalStep),
					Step = this.PhotoFrameTotalStep / 4 + ((1 < this.PhotoFrameTotalStep % 4) ? 1 : 0),
					Hp = 100,
					Atk = 100,
					Def = 100
				},
				new CharaDynamicData.PPParam
				{
					Flag = (2 < this.PhotoFrameTotalStep),
					Step = this.PhotoFrameTotalStep / 4 + ((2 < this.PhotoFrameTotalStep % 4) ? 1 : 0),
					Hp = 100,
					Atk = 100,
					Def = 100
				},
				new CharaDynamicData.PPParam
				{
					Flag = (3 < this.PhotoFrameTotalStep),
					Step = this.PhotoFrameTotalStep / 4 + ((3 < this.PhotoFrameTotalStep % 4) ? 1 : 0),
					Hp = 100,
					Atk = 100,
					Def = 100
				}
			};
			int ppId = charaStaticData.baseData.ppId;
			foreach (DataManagerServerMst.StaticPpData.PpDataOne ppDataOne in DataManager.DmServerMst.StaticCharaPpDataMap[ppId].PpDataList)
			{
				if (this.PhotoFrameTotalStep < ppDataOne.Step)
				{
					break;
				}
				this.innerPhotoPocketList[ppDataOne.DispIndex].Hp = ppDataOne.HpRate;
				this.innerPhotoPocketList[ppDataOne.DispIndex].Atk = ppDataOne.AtkRate;
				this.innerPhotoPocketList[ppDataOne.DispIndex].Def = ppDataOne.DefRate;
			}
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x0600021A RID: 538 RVA: 0x0001381C File Offset: 0x00011A1C
	// (set) Token: 0x0600021B RID: 539 RVA: 0x00013824 File Offset: 0x00011A24
	private int innerPhotoFrameTotalStep { get; set; }

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x0600021C RID: 540 RVA: 0x0001382D File Offset: 0x00011A2D
	public List<CharaDynamicData.PPParam> PhotoPocket
	{
		get
		{
			if (this.innerPhotoPocketList == null)
			{
				this.PhotoFrameTotalStep = 1;
			}
			return this.innerPhotoPocketList;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x0600021D RID: 541 RVA: 0x00013844 File Offset: 0x00011A44
	// (set) Token: 0x0600021E RID: 542 RVA: 0x0001384C File Offset: 0x00011A4C
	private List<CharaDynamicData.PPParam> innerPhotoPocketList { get; set; }

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x0600021F RID: 543 RVA: 0x00013855 File Offset: 0x00011A55
	public bool miracleAvailable
	{
		get
		{
			return 0 < this.artsLevel;
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x06000220 RID: 544 RVA: 0x00013860 File Offset: 0x00011A60
	public bool isArtsMaxEnable
	{
		get
		{
			return this.clearScenarioNum >= 4;
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x06000221 RID: 545 RVA: 0x0001386E File Offset: 0x00011A6E
	public int limitMiracleLevel
	{
		get
		{
			return Math.Min(5, this.kizunaLevel);
		}
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x06000222 RID: 546 RVA: 0x0001387C File Offset: 0x00011A7C
	public string IconName
	{
		get
		{
			int num = DataManagerChara.CharaId2ModelId(this.id);
			string text = "Texture2D/Icon_Chara/Chara/icon_chara_";
			string text2 = num.ToString("0000");
			string text3 = ((1 < this.selectFaceIconId) ? ("_" + this.selectFaceIconId.ToString("00")) : "");
			return text + text2 + text3;
		}
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x06000223 RID: 547 RVA: 0x000138DC File Offset: 0x00011ADC
	public string MiniIconName
	{
		get
		{
			int num = DataManagerChara.CharaId2ModelId(this.id);
			string text = "Texture2D/Icon_Chara/Chara_Mini/icon_chara_mini_";
			string text2 = num.ToString("0000");
			string text3 = ((1 < this.selectFaceIconId) ? ("_" + this.selectFaceIconId.ToString("00")) : "");
			return text + text2 + text3;
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x06000225 RID: 549 RVA: 0x00013957 File Offset: 0x00011B57
	private CharaStaticData StaticData
	{
		get
		{
			return DataManager.DmChara.GetCharaStaticData(this.id);
		}
	}

	// Token: 0x06000226 RID: 550 RVA: 0x0001396C File Offset: 0x00011B6C
	public void UpdateByServer(Chara srvChara)
	{
		this.id = srvChara.chara_id;
		this.level = srvChara.level;
		this.levelLimitId = srvChara.limit_over_num;
		this.rank = srvChara.rank;
		this.limitLevel = CharaPackData.CalcLimitLevel(this.id, this.rank, this.levelLimitId);
		this.exp = CharaDynamicData.ConvertExp(this.id, srvChara.exp);
		this.kizunaLevel = srvChara.kizuna_level;
		this.kizunaExp = CharaDynamicData.ConvertKizunaExp(this.id, srvChara.kizuna_exp);
		this.kizunaLimitOverNum = srvChara.kizuna_limit_over_num;
		this.promoteNum = srvChara.promote_num;
		this.selectFaceIconId = srvChara.select_faceicon_id;
		this.promoteFlag = new List<bool>
		{
			srvChara.promote_flag00 != 0,
			srvChara.promote_flag01 != 0,
			srvChara.promote_flag02 != 0,
			srvChara.promote_flag03 != 0,
			srvChara.promote_flag04 != 0,
			srvChara.promote_flag05 != 0
		};
		this.equipClothesId = srvChara.equip_clothes_id;
		this.artsLevel = srvChara.arts_level;
		if (this.accessory == null)
		{
			this.accessory = ((srvChara.accessory == null) ? null : new DataManagerCharaAccessory.Accessory(srvChara.accessory));
		}
		this.dispAccessoryEffect = srvChara.accessory_effect == 0;
		this.favoriteFlag = srvChara.favorite_flag == 1;
		this.nanairoAbilityReleaseFlag = srvChara.nanairo_ability_release_flag == 1;
		this.PhotoFrameTotalStep = srvChara.photo_frame_step;
		this.haveClothesIdList = new List<int>();
		if (srvChara.clothes_list != null && 0 < srvChara.clothes_list.Count)
		{
			this.haveClothesIdList.AddRange(srvChara.clothes_list);
		}
		this.clearScenarioNum = srvChara.scenario_status;
		this.insertTime = new DateTime(PrjUtil.ConvertTimeToTicks(srvChara.insert_time));
	}

	// Token: 0x06000227 RID: 551 RVA: 0x00013B5C File Offset: 0x00011D5C
	public void UpdateByServer(FollowsChara srvFollowChara)
	{
		this.id = srvFollowChara.chara_id;
		this.levelLimitId = srvFollowChara.limit_over_num;
		this.level = srvFollowChara.level;
		this.rank = srvFollowChara.rank;
		this.limitLevel = CharaPackData.CalcLimitLevel(this.id, this.rank, this.levelLimitId);
		this.promoteNum = srvFollowChara.promote_num;
		this.selectFaceIconId = srvFollowChara.select_faceicon_id;
		this.promoteFlag = new List<bool> { false, false, false, false, false, false };
		this.PhotoFrameTotalStep = 0;
	}

	// Token: 0x06000228 RID: 552 RVA: 0x00013C0C File Offset: 0x00011E0C
	public bool CheckCharaRankMaxConversion(bool justIsFalse)
	{
		int rankItemId = this.StaticData.baseData.rankItemId;
		int num = DataManager.DmItem.GetUserItemData(rankItemId).num;
		int num2 = 0;
		foreach (MstCharaRankData mstCharaRankData in DataManager.DmServerMst.mstCharaRankDataList)
		{
			if (this.rank < mstCharaRankData.rank)
			{
				num2 += mstCharaRankData.useCostFragmentNum;
			}
		}
		bool flag;
		if (justIsFalse)
		{
			flag = num > num2;
		}
		else
		{
			flag = num >= num2;
		}
		return flag;
	}

	// Token: 0x06000229 RID: 553 RVA: 0x00013CB0 File Offset: 0x00011EB0
	private static long ConvertExp(int charaId, long exp)
	{
		int levelTableId = DataManager.DmChara.GetCharaStaticData(charaId).baseData.levelTableId;
		long num = 0L;
		foreach (GameLevelInfo gameLevelInfo in DataManager.DmServerMst.gameLevelInfoList)
		{
			if (!gameLevelInfo.charaLevelExp.ContainsKey(levelTableId))
			{
				break;
			}
			if (exp < num + gameLevelInfo.charaLevelExp[levelTableId])
			{
				break;
			}
			num += gameLevelInfo.charaLevelExp[levelTableId];
		}
		return exp - num;
	}

	// Token: 0x0600022A RID: 554 RVA: 0x00013D50 File Offset: 0x00011F50
	private static long ConvertKizunaExp(int charaId, long kizunaExp)
	{
		int kizunaTableId = DataManager.DmChara.GetCharaStaticData(charaId).baseData.kizunaTableId;
		long num = 0L;
		foreach (GameLevelInfo gameLevelInfo in DataManager.DmServerMst.gameLevelInfoList)
		{
			if (!gameLevelInfo.kizunaLevelExp.ContainsKey(kizunaTableId))
			{
				break;
			}
			if (kizunaExp < num + gameLevelInfo.kizunaLevelExp[kizunaTableId].LevelExp)
			{
				break;
			}
			num += gameLevelInfo.kizunaLevelExp[kizunaTableId].LevelExp;
		}
		return kizunaExp - num;
	}

	// Token: 0x04000272 RID: 626
	public int id;

	// Token: 0x04000273 RID: 627
	public int level;

	// Token: 0x04000274 RID: 628
	public int levelLimitId;

	// Token: 0x04000275 RID: 629
	public int limitLevel;

	// Token: 0x04000276 RID: 630
	public long exp;

	// Token: 0x04000277 RID: 631
	public int kizunaLevel;

	// Token: 0x04000278 RID: 632
	public long kizunaExp;

	// Token: 0x04000279 RID: 633
	public int kizunaLimitOverNum;

	// Token: 0x0400027A RID: 634
	public int rank;

	// Token: 0x0400027B RID: 635
	public int promoteNum;

	// Token: 0x0400027C RID: 636
	public List<bool> promoteFlag;

	// Token: 0x0400027D RID: 637
	public int artsLevel;

	// Token: 0x0400027E RID: 638
	public bool accessoryOpen;

	// Token: 0x0400027F RID: 639
	public DataManagerCharaAccessory.Accessory accessory;

	// Token: 0x04000280 RID: 640
	public bool dispAccessoryEffect;

	// Token: 0x04000281 RID: 641
	public bool favoriteFlag;

	// Token: 0x04000282 RID: 642
	public bool nanairoAbilityReleaseFlag;

	// Token: 0x04000285 RID: 645
	public int equipClothesId;

	// Token: 0x04000286 RID: 646
	public List<int> haveClothesIdList = new List<int>();

	// Token: 0x04000287 RID: 647
	public int selectFaceIconId;

	// Token: 0x04000288 RID: 648
	public DateTime insertTime;

	// Token: 0x04000289 RID: 649
	public int clearScenarioNum;

	// Token: 0x0400028A RID: 650
	public const int ArtsMaxEnableClearScenarioNum = 4;

	// Token: 0x0400028B RID: 651
	public const int ArtsMaxScenarioOpenArtsLevel = 5;

	// Token: 0x0400028C RID: 652
	public HashSet<int> haveContactItemIdList = new HashSet<int>();

	// Token: 0x0400028D RID: 653
	public int charaStatusId;

	// Token: 0x020005FF RID: 1535
	public class PPParam
	{
		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06002FD6 RID: 12246 RVA: 0x001B99CD File Offset: 0x001B7BCD
		// (set) Token: 0x06002FD7 RID: 12247 RVA: 0x001B99D5 File Offset: 0x001B7BD5
		public bool Flag { get; set; }

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06002FD8 RID: 12248 RVA: 0x001B99DE File Offset: 0x001B7BDE
		// (set) Token: 0x06002FD9 RID: 12249 RVA: 0x001B99E6 File Offset: 0x001B7BE6
		public int Step { get; set; }

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06002FDA RID: 12250 RVA: 0x001B99EF File Offset: 0x001B7BEF
		// (set) Token: 0x06002FDB RID: 12251 RVA: 0x001B99F7 File Offset: 0x001B7BF7
		public int Hp { get; set; }

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06002FDC RID: 12252 RVA: 0x001B9A00 File Offset: 0x001B7C00
		// (set) Token: 0x06002FDD RID: 12253 RVA: 0x001B9A08 File Offset: 0x001B7C08
		public int Atk { get; set; }

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06002FDE RID: 12254 RVA: 0x001B9A11 File Offset: 0x001B7C11
		// (set) Token: 0x06002FDF RID: 12255 RVA: 0x001B9A19 File Offset: 0x001B7C19
		public int Def { get; set; }

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06002FE0 RID: 12256 RVA: 0x001B9A24 File Offset: 0x001B7C24
		public string HpRatio2String
		{
			get
			{
				return ((float)this.Hp / 100f).ToString("F2");
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06002FE1 RID: 12257 RVA: 0x001B9A4C File Offset: 0x001B7C4C
		public string AtkRatio2String
		{
			get
			{
				return ((float)this.Atk / 100f).ToString("F2");
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06002FE2 RID: 12258 RVA: 0x001B9A74 File Offset: 0x001B7C74
		public string DefRatio2String
		{
			get
			{
				return ((float)this.Def / 100f).ToString("F2");
			}
		}

		// Token: 0x06002FE3 RID: 12259 RVA: 0x001B9A9B File Offset: 0x001B7C9B
		public PrjUtil.ParamPreset CalcPhotoParam(PhotoPackData ppd)
		{
			return this.CalcPhotoParam(PrjUtil.CalcParamByPhoto(ppd.staticData, ppd.dynamicData.level));
		}

		// Token: 0x06002FE4 RID: 12260 RVA: 0x001B9ABC File Offset: 0x001B7CBC
		public PrjUtil.ParamPreset CalcPhotoParam(PrjUtil.ParamPreset photoParam)
		{
			return new PrjUtil.ParamPreset
			{
				hp = photoParam.hp * this.Hp / 100 + ((0 < photoParam.hp * this.Hp % 100) ? 1 : 0),
				atk = photoParam.atk * this.Atk / 100 + ((0 < photoParam.atk * this.Atk % 100) ? 1 : 0),
				def = photoParam.def * this.Def / 100 + ((0 < photoParam.def * this.Def % 100) ? 1 : 0)
			};
		}

		// Token: 0x06002FE5 RID: 12261 RVA: 0x001B9B58 File Offset: 0x001B7D58
		public PrjUtil.ParamPreset DiffCalcPhotoParam(PhotoPackData ppd)
		{
			return this.DiffCalcPhotoParam(PrjUtil.CalcParamByPhoto(ppd.staticData, ppd.dynamicData.level));
		}

		// Token: 0x06002FE6 RID: 12262 RVA: 0x001B9B78 File Offset: 0x001B7D78
		public PrjUtil.ParamPreset DiffCalcPhotoParam(PrjUtil.ParamPreset photoParam)
		{
			PrjUtil.ParamPreset paramPreset = this.CalcPhotoParam(photoParam);
			return new PrjUtil.ParamPreset
			{
				hp = paramPreset.hp - photoParam.hp,
				atk = paramPreset.atk - photoParam.atk,
				def = paramPreset.def - photoParam.def
			};
		}
	}

	// Token: 0x02000600 RID: 1536
	public enum CharaOwnerType
	{
		// Token: 0x04002C71 RID: 11377
		Undefined,
		// Token: 0x04002C72 RID: 11378
		User,
		// Token: 0x04002C73 RID: 11379
		Helper,
		// Token: 0x04002C74 RID: 11380
		SHOP
	}
}
