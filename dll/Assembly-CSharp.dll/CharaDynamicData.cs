using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

[Serializable]
public class CharaDynamicData
{
	public CharaDynamicData.CharaOwnerType OwnerType { get; set; }

	public int limitLevelRankMax
	{
		get
		{
			return CharaPackData.CalcLimitLevel(this.id, this.rank, 0);
		}
	}

	public long haveNextExp
	{
		get
		{
			return CharaDynamicData.ConvertExp(this.id, this.exp);
		}
	}

	public long haveNextKizunaExp
	{
		get
		{
			return CharaDynamicData.ConvertKizunaExp(this.id, this.kizunaExp);
		}
	}

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

	private int innerPhotoFrameTotalStep { get; set; }

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

	private List<CharaDynamicData.PPParam> innerPhotoPocketList { get; set; }

	public bool miracleAvailable
	{
		get
		{
			return 0 < this.artsLevel;
		}
	}

	public bool isArtsMaxEnable
	{
		get
		{
			return this.clearScenarioNum >= 4;
		}
	}

	public int limitMiracleLevel
	{
		get
		{
			return Math.Min(5, this.kizunaLevel);
		}
	}

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

	private CharaStaticData StaticData
	{
		get
		{
			return DataManager.DmChara.GetCharaStaticData(this.id);
		}
	}

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

	public int id;

	public int level;

	public int levelLimitId;

	public int limitLevel;

	public long exp;

	public int kizunaLevel;

	public long kizunaExp;

	public int kizunaLimitOverNum;

	public int rank;

	public int promoteNum;

	public List<bool> promoteFlag;

	public int artsLevel;

	public bool accessoryOpen;

	public DataManagerCharaAccessory.Accessory accessory;

	public bool dispAccessoryEffect;

	public bool favoriteFlag;

	public bool nanairoAbilityReleaseFlag;

	public int equipClothesId;

	public List<int> haveClothesIdList = new List<int>();

	public int selectFaceIconId;

	public DateTime insertTime;

	public int clearScenarioNum;

	public const int ArtsMaxEnableClearScenarioNum = 4;

	public const int ArtsMaxScenarioOpenArtsLevel = 5;

	public HashSet<int> haveContactItemIdList = new HashSet<int>();

	public int charaStatusId;

	public class PPParam
	{
		public bool Flag { get; set; }

		public int Step { get; set; }

		public int Hp { get; set; }

		public int Atk { get; set; }

		public int Def { get; set; }

		public string HpRatio2String
		{
			get
			{
				return ((float)this.Hp / 100f).ToString("F2");
			}
		}

		public string AtkRatio2String
		{
			get
			{
				return ((float)this.Atk / 100f).ToString("F2");
			}
		}

		public string DefRatio2String
		{
			get
			{
				return ((float)this.Def / 100f).ToString("F2");
			}
		}

		public PrjUtil.ParamPreset CalcPhotoParam(PhotoPackData ppd)
		{
			return this.CalcPhotoParam(PrjUtil.CalcParamByPhoto(ppd.staticData, ppd.dynamicData.level));
		}

		public PrjUtil.ParamPreset CalcPhotoParam(PrjUtil.ParamPreset photoParam)
		{
			return new PrjUtil.ParamPreset
			{
				hp = photoParam.hp * this.Hp / 100 + ((0 < photoParam.hp * this.Hp % 100) ? 1 : 0),
				atk = photoParam.atk * this.Atk / 100 + ((0 < photoParam.atk * this.Atk % 100) ? 1 : 0),
				def = photoParam.def * this.Def / 100 + ((0 < photoParam.def * this.Def % 100) ? 1 : 0)
			};
		}

		public PrjUtil.ParamPreset DiffCalcPhotoParam(PhotoPackData ppd)
		{
			return this.DiffCalcPhotoParam(PrjUtil.CalcParamByPhoto(ppd.staticData, ppd.dynamicData.level));
		}

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

	public enum CharaOwnerType
	{
		Undefined,
		User,
		Helper,
		SHOP
	}
}
