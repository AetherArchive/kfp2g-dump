using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Mst;

public class CharaPackData
{
	public int id { get; private set; }

	public CharaDynamicData dynamicData { get; private set; }

	public CharaStaticData staticData { get; private set; }

	public int equipClothImageId
	{
		get
		{
			return DataManager.DmChara.ColothId2ImageId(this.dynamicData.equipClothesId);
		}
	}

	public bool equipLongSkirt
	{
		get
		{
			return DataManager.DmChara.ClothLongSkirt(this.dynamicData.equipClothesId);
		}
	}

	public CharaClothStatic.PlayMotionType equipPlayMotion
	{
		get
		{
			return DataManager.DmChara.ClothPlayMotion(this.dynamicData.equipClothesId);
		}
	}

	public CharaPackData()
	{
	}

	public CharaPackData(CharaDynamicData chDynamicData)
	{
		this.id = chDynamicData.id;
		this.dynamicData = chDynamicData;
		this.staticData = DataManager.DmChara.GetCharaStaticData(this.id);
	}

	public static int CalcLimitLevel(int charaId, int levelRank, int lvLimId)
	{
		if (charaId == 0)
		{
			return 0;
		}
		CharaStaticData mstChara = DataManager.DmChara.GetCharaStaticData(charaId);
		MstCharaRankData mstCharaRankData = DataManager.DmServerMst.mstCharaRankDataList.Find((MstCharaRankData item) => item.rankId == mstChara.baseData.gradeTableId && item.rank == levelRank);
		if (mstCharaRankData == null)
		{
			return 0;
		}
		int num = mstCharaRankData.levelLimit;
		if (6 <= mstCharaRankData.rank)
		{
			DataManagerChara.LevelLimitData levelLimitData = DataManager.DmChara.GetLevelLimitData(lvLimId);
			if (levelLimitData != null)
			{
				num += levelLimitData.IncreaseMaxLevel;
			}
		}
		return num;
	}

	public bool IsInvalid()
	{
		return this.id == 0;
	}

	public static CharaPackData MakeDummy(int charaId)
	{
		CharaDynamicData charaDynamicData = new CharaDynamicData
		{
			id = charaId,
			level = 10,
			limitLevel = 20,
			exp = 123L,
			kizunaLevel = 2,
			kizunaExp = 34L,
			promoteNum = 0,
			promoteFlag = new List<bool> { true, false, false, false, false, false },
			rank = DataManager.DmChara.GetCharaStaticData(charaId).baseData.rankLow,
			PhotoFrameTotalStep = 1,
			artsLevel = 3
		};
		charaDynamicData.limitLevel = CharaPackData.CalcLimitLevel(charaDynamicData.id, charaDynamicData.rank, charaDynamicData.levelLimitId);
		return new CharaPackData(charaDynamicData);
	}

	public static CharaPackData MakeInvalid()
	{
		return new CharaPackData
		{
			id = 0
		};
	}

	public static CharaPackData MakeInitial(int charaId)
	{
		CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(charaId);
		CharaDynamicData charaDynamicData = new CharaDynamicData
		{
			id = charaId,
			level = 1,
			limitLevel = 20,
			exp = 0L,
			kizunaLevel = 1,
			kizunaExp = 0L,
			promoteNum = 0,
			promoteFlag = new List<bool> { false, false, false, false, false, false },
			rank = ((charaStaticData != null) ? charaStaticData.baseData.rankLow : 1),
			PhotoFrameTotalStep = 1,
			artsLevel = 1
		};
		charaDynamicData.limitLevel = CharaPackData.CalcLimitLevel(charaDynamicData.id, charaDynamicData.rank, charaDynamicData.levelLimitId);
		return new CharaPackData(charaDynamicData);
	}

	public static CharaPackData MakeShopCharaData(int charaId, int charaStatusId)
	{
		ShopData.ItemOne.CharaStatusData charaStatusData = new ShopData.ItemOne.CharaStatusData(DataManager.DmShop.GetCharaStatusData(charaStatusId));
		DataManager.DmChara.GetCharaStaticData(charaId);
		long needExpFromFirstLevel = DataManager.DmChara.GetNeedExpFromFirstLevel(charaId, charaStatusData.level);
		long needKizunaExpFromFirstLevel = DataManager.DmChara.GetNeedKizunaExpFromFirstLevel(charaId, charaStatusData.kizunaLevel);
		CharaDynamicData charaDynamicData = new CharaDynamicData
		{
			id = charaId,
			level = charaStatusData.level,
			limitLevel = charaStatusData.level,
			exp = needExpFromFirstLevel,
			kizunaLevel = charaStatusData.kizunaLevel,
			kizunaExp = needKizunaExpFromFirstLevel,
			promoteNum = charaStatusData.promoteNum,
			promoteFlag = new List<bool> { false, false, false, false, false, false },
			rank = charaStatusData.rank,
			PhotoFrameTotalStep = charaStatusData.photoFrameLevel,
			artsLevel = charaStatusData.artsLevel,
			charaStatusId = charaStatusId,
			OwnerType = CharaDynamicData.CharaOwnerType.SHOP
		};
		charaDynamicData.levelLimitId = DataManager.DmChara.GetLevelLimitOverCount(charaDynamicData.limitLevel);
		charaDynamicData.limitLevel = CharaPackData.CalcLimitLevel(charaDynamicData.id, charaDynamicData.rank, charaDynamicData.levelLimitId);
		return new CharaPackData(charaDynamicData);
	}

	public static CharaPackData MakeUpgradeUserCharaFromShopData(int charaId, int friendsStatusId)
	{
		if (DataManager.DmChara.GetUserCharaData(charaId) == null)
		{
			return CharaPackData.MakeShopCharaData(charaId, friendsStatusId);
		}
		ShopData.ItemOne.CharaStatusData charaStatusData = new ShopData.ItemOne.CharaStatusData(DataManager.DmShop.GetCharaStatusData(friendsStatusId));
		CharaDynamicData dynamicData = DataManager.DmChara.GetUserCharaData(charaId).dynamicData;
		long needExpFromFirstLevel = DataManager.DmChara.GetNeedExpFromFirstLevel(charaId, charaStatusData.level);
		long needKizunaExpFromFirstLevel = DataManager.DmChara.GetNeedKizunaExpFromFirstLevel(charaId, charaStatusData.kizunaLevel);
		CharaDynamicData charaDynamicData = new CharaDynamicData();
		charaDynamicData.id = charaId;
		charaDynamicData.level = ((dynamicData.level >= charaStatusData.level) ? dynamicData.level : charaStatusData.level);
		charaDynamicData.limitLevel = ((dynamicData.limitLevel >= charaStatusData.level) ? dynamicData.limitLevel : charaStatusData.level);
		charaDynamicData.exp = ((dynamicData.exp >= needExpFromFirstLevel) ? dynamicData.exp : needExpFromFirstLevel);
		charaDynamicData.kizunaLevel = ((dynamicData.kizunaLevel >= charaStatusData.kizunaLevel) ? dynamicData.kizunaLevel : charaStatusData.kizunaLevel);
		charaDynamicData.kizunaExp = ((dynamicData.kizunaExp >= needKizunaExpFromFirstLevel) ? dynamicData.kizunaExp : needKizunaExpFromFirstLevel);
		charaDynamicData.promoteNum = ((dynamicData.promoteNum >= charaStatusData.promoteNum) ? dynamicData.promoteNum : charaStatusData.promoteNum);
		List<bool> list2;
		if (dynamicData.promoteNum < charaStatusData.promoteNum)
		{
			List<bool> list = new List<bool>();
			list.Add(false);
			list.Add(false);
			list.Add(false);
			list.Add(false);
			list.Add(false);
			list2 = list;
			list.Add(false);
		}
		else
		{
			list2 = dynamicData.promoteFlag;
		}
		charaDynamicData.promoteFlag = list2;
		charaDynamicData.rank = ((dynamicData.rank >= charaStatusData.rank) ? dynamicData.rank : charaStatusData.rank);
		charaDynamicData.PhotoFrameTotalStep = ((dynamicData.PhotoFrameTotalStep >= charaStatusData.photoFrameLevel) ? dynamicData.PhotoFrameTotalStep : charaStatusData.photoFrameLevel);
		charaDynamicData.artsLevel = ((dynamicData.artsLevel >= charaStatusData.artsLevel) ? dynamicData.artsLevel : charaStatusData.artsLevel);
		charaDynamicData.charaStatusId = friendsStatusId;
		charaDynamicData.OwnerType = CharaDynamicData.CharaOwnerType.SHOP;
		CharaDynamicData charaDynamicData2 = charaDynamicData;
		charaDynamicData2.levelLimitId = DataManager.DmChara.GetLevelLimitOverCount(charaDynamicData2.limitLevel);
		charaDynamicData2.limitLevel = CharaPackData.CalcLimitLevel(charaDynamicData2.id, charaDynamicData2.rank, charaDynamicData2.levelLimitId);
		charaDynamicData2.kizunaLimitOverNum = DataManager.DmChara.GetKizunaLevelLimitOverCount(charaDynamicData2.kizunaLevel);
		return new CharaPackData(charaDynamicData2);
	}

	public GrowItemData GetNextItemByRankup(int nowRank = 0)
	{
		if (nowRank == 0)
		{
			nowRank = this.dynamicData.rank;
		}
		int nextRank = nowRank + 1;
		if (nextRank > this.staticData.baseData.rankHigh)
		{
			return null;
		}
		MstCharaRankData mstCharaRankData = DataManager.DmServerMst.mstCharaRankDataList.Find((MstCharaRankData item) => item.rankId == this.staticData.baseData.gradeTableId && item.rank == nextRank);
		if (mstCharaRankData == null)
		{
			return null;
		}
		return new GrowItemData
		{
			item = new ItemData(this.staticData.baseData.rankItemId, mstCharaRankData.useCostFragmentNum),
			needGold = mstCharaRankData.useCostCoinNum
		};
	}

	public GrowItemData GetNextItemByReleasePhotoFrame()
	{
		int nextStep = this.dynamicData.PhotoFrameTotalStep;
		DataManagerServerMst.StaticPpData.PpDataOne ppDataOne = DataManager.DmServerMst.StaticCharaPpDataMap[this.staticData.baseData.photoFrameTableId].PpDataList.Find((DataManagerServerMst.StaticPpData.PpDataOne x) => nextStep == x.Index);
		if (ppDataOne == null)
		{
			return null;
		}
		return new GrowItemData
		{
			item = new ItemData(this.staticData.baseData.ppItemId, ppDataOne.CostNum),
			needGold = 0
		};
	}

	public GrowItemList GetNextItemByArtsUp(int nowArtsLv = 0)
	{
		if (nowArtsLv == 0)
		{
			nowArtsLv = this.dynamicData.artsLevel;
		}
		MstCharaArtsData mstCharaArtsData = DataManager.DmServerMst.mstCharaArtsDataList.Find((MstCharaArtsData item) => item.artsId == this.staticData.baseData.atrsTableId && item.artsLevel == nowArtsLv + 1);
		if (mstCharaArtsData == null)
		{
			return null;
		}
		GrowItemList growItemList = new GrowItemList
		{
			needGold = mstCharaArtsData.useCostCoinNum
		};
		foreach (ItemInput itemInput in new List<ItemInput>
		{
			new ItemInput(mstCharaArtsData.strengItemId00, mstCharaArtsData.strengItemNum00),
			new ItemInput(mstCharaArtsData.strengItemId01, mstCharaArtsData.strengItemNum01),
			new ItemInput(mstCharaArtsData.strengItemId02, mstCharaArtsData.strengItemNum02),
			new ItemInput(mstCharaArtsData.strengItemId03, mstCharaArtsData.strengItemNum03),
			new ItemInput(mstCharaArtsData.strengItemId04, mstCharaArtsData.strengItemNum04),
			new ItemInput(mstCharaArtsData.strengItemId05, mstCharaArtsData.strengItemNum05),
			new ItemInput(mstCharaArtsData.strengItemId06, mstCharaArtsData.strengItemNum06),
			new ItemInput(mstCharaArtsData.strengItemId07, mstCharaArtsData.strengItemNum07)
		})
		{
			if (itemInput.itemId != 0)
			{
				growItemList.itemList.Add(new ItemData(itemInput.itemId, itemInput.num));
			}
		}
		return growItemList;
	}

	public bool IsReleaseNanairoAbility()
	{
		MstCharaNanairoAbilityReleaseData mstCharaNanairoAbilityReleaseData = DataManager.DmServerMst.mstNanairoReleaseData.Find((MstCharaNanairoAbilityReleaseData item) => item.charaId == this.staticData.baseData.id);
		return mstCharaNanairoAbilityReleaseData != null && TimeManager.Now >= new DateTime(PrjUtil.ConvertTimeToTicks(mstCharaNanairoAbilityReleaseData.startTime));
	}

	public GrowItemList GetReleaseItemByNanairoAbilityRelease()
	{
		if (this.dynamicData.nanairoAbilityReleaseFlag)
		{
			return null;
		}
		MstCharaNanairoAbilityReleaseData mstCharaNanairoAbilityReleaseData = DataManager.DmServerMst.mstNanairoReleaseData.Find((MstCharaNanairoAbilityReleaseData item) => item.charaId == this.staticData.baseData.id);
		if (mstCharaNanairoAbilityReleaseData == null)
		{
			return null;
		}
		GrowItemList growItemList = new GrowItemList
		{
			needGold = 0
		};
		foreach (ItemInput itemInput in new List<ItemInput>
		{
			new ItemInput(mstCharaNanairoAbilityReleaseData.itemId00, mstCharaNanairoAbilityReleaseData.itemNum00),
			new ItemInput(mstCharaNanairoAbilityReleaseData.itemId01, mstCharaNanairoAbilityReleaseData.itemNum01),
			new ItemInput(mstCharaNanairoAbilityReleaseData.itemId02, mstCharaNanairoAbilityReleaseData.itemNum02),
			new ItemInput(mstCharaNanairoAbilityReleaseData.itemId03, mstCharaNanairoAbilityReleaseData.itemNum03)
		})
		{
			if (itemInput.itemId != 0)
			{
				growItemList.itemList.Add(new ItemData(itemInput.itemId, itemInput.num));
			}
		}
		return growItemList;
	}

	public List<DataManagerServerMst.CharaLevelItem> LevelItemUseOrderList(bool isKizuna = false)
	{
		CharaDef.AttributeType charaAttribute = this.staticData.baseData.attribute;
		List<DataManagerServerMst.CharaLevelItem> list = DataManager.DmServerMst.charaLevelItemDataList;
		List<DataManagerServerMst.CharaLevelItem> list2;
		if (!isKizuna)
		{
			list2 = list.FindAll((DataManagerServerMst.CharaLevelItem item) => item.isKizuna == 0);
		}
		else
		{
			list2 = list.FindAll((DataManagerServerMst.CharaLevelItem item) => item.isKizuna == 1);
		}
		list = list2;
		List<DataManagerServerMst.CharaLevelItem> list3 = new List<DataManagerServerMst.CharaLevelItem>();
		List<DataManagerServerMst.CharaLevelItem> list4 = list.FindAll((DataManagerServerMst.CharaLevelItem item) => charaAttribute == item.attribute);
		list4.Sort((DataManagerServerMst.CharaLevelItem a, DataManagerServerMst.CharaLevelItem b) => b.attributeExp.CompareTo(a.attributeExp));
		List<DataManagerServerMst.CharaLevelItem> list5 = list.FindAll((DataManagerServerMst.CharaLevelItem item) => item.attribute == CharaDef.AttributeType.ALL);
		list5.Sort((DataManagerServerMst.CharaLevelItem a, DataManagerServerMst.CharaLevelItem b) => b.attributeExp.CompareTo(a.attributeExp));
		List<DataManagerServerMst.CharaLevelItem> list6 = new List<DataManagerServerMst.CharaLevelItem>();
		foreach (object obj in Enum.GetValues(typeof(CharaDef.AttributeType)))
		{
			CharaDef.AttributeType attributeType = (CharaDef.AttributeType)obj;
			int num = (int)(charaAttribute + (int)attributeType);
			CharaDef.AttributeType calcIndex = (CharaDef.AttributeType)(num % 7);
			if (charaAttribute <= calcIndex)
			{
				calcIndex = (calcIndex + 1) % (CharaDef.AttributeType)7;
			}
			if (calcIndex != CharaDef.AttributeType.ALL && charaAttribute != calcIndex)
			{
				List<DataManagerServerMst.CharaLevelItem> list7 = list.FindAll((DataManagerServerMst.CharaLevelItem item) => calcIndex == item.attribute);
				list7.Sort((DataManagerServerMst.CharaLevelItem a, DataManagerServerMst.CharaLevelItem b) => b.attributeExp.CompareTo(a.attributeExp));
				list6.AddRange(list7);
			}
		}
		list.FindAll((DataManagerServerMst.CharaLevelItem item) => item.attribute == CharaDef.AttributeType.ALL);
		PrjUtil.InsertionSort<DataManagerServerMst.CharaLevelItem>(ref list6, (DataManagerServerMst.CharaLevelItem a, DataManagerServerMst.CharaLevelItem b) => b.attributeExp.CompareTo(a.attributeExp));
		list3.AddRange(list4);
		list3.AddRange(list5);
		list3.AddRange(list6);
		return list3;
	}

	public List<int> CalcEatLevelItem(int afterLv = 0, int haveCoin = 0, bool isKizuna = false)
	{
		List<int> list = new List<int>();
		bool flag = false;
		List<DataManagerServerMst.CharaLevelItem> list2 = this.LevelItemUseOrderList(isKizuna);
		long num;
		if (isKizuna)
		{
			num = DataManager.DmChara.GetKizunaExpForNextLevel(this.id, this.dynamicData.kizunaLevel) - this.dynamicData.kizunaExp;
		}
		else
		{
			num = ((afterLv == 0) ? (DataManager.DmChara.GetExpByNextLevel(this.id, this.dynamicData.level) - this.dynamicData.exp) : DataManager.DmChara.GetNeedExpByRangeLevel(this.id, afterLv));
		}
		if (haveCoin == 0)
		{
			haveCoin = DataManager.DmItem.GetUserItemData(30101).num;
		}
		long num2 = 0L;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		foreach (DataManagerServerMst.CharaLevelItem charaLevelItem in list2)
		{
			ItemData userItemData = DataManager.DmItem.GetUserItemData(charaLevelItem.itemId);
			int i = 0;
			while (i < userItemData.num)
			{
				long num6 = ((this.staticData.baseData.attribute == charaLevelItem.attribute) ? charaLevelItem.attributeExp : charaLevelItem.exp);
				if (flag)
				{
					if (num5 > charaLevelItem.strengCoinNum && num <= num2 + num6)
					{
						num4 = charaLevelItem.itemId;
						num5 = charaLevelItem.strengCoinNum;
						break;
					}
					break;
				}
				else
				{
					if (haveCoin < num3 + charaLevelItem.strengCoinNum)
					{
						break;
					}
					if (num > 0L && num <= num2 + num6)
					{
						num4 = charaLevelItem.itemId;
						num5 = charaLevelItem.strengCoinNum;
						flag = true;
						break;
					}
					num3 += charaLevelItem.strengCoinNum;
					num2 += num6;
					list.Add(charaLevelItem.itemId);
					i++;
				}
			}
		}
		if (flag)
		{
			list.Add(num4);
		}
		if (!flag)
		{
			return null;
		}
		return list;
	}

	public bool EnhanceInfoLv
	{
		get
		{
			if (this.dynamicData.limitLevel <= this.dynamicData.level)
			{
				if (6 <= this.dynamicData.rank)
				{
					DataManagerChara.LevelLimitData levelLimitData = DataManager.DmChara.GetLevelLimitData(this.dynamicData.levelLimitId + 1);
					if (levelLimitData != null)
					{
						return levelLimitData.needItemNum01 <= DataManager.DmItem.GetUserItemData(levelLimitData.needItemId01).num && (levelLimitData.needItemId02 == 0 || levelLimitData.needItemNum02 <= DataManager.DmItem.GetUserItemData(levelLimitData.needItemId02).num) && levelLimitData.needGoldNum <= DataManager.DmItem.GetUserItemData(30101).num;
					}
				}
				return false;
			}
			List<int> list = this.CalcEatLevelItem(0, 0, false);
			return list != null && list.Count != 0;
		}
	}

	public bool EnhanceInfoPromote
	{
		get
		{
			if (this.staticData.maxPromoteNum <= this.dynamicData.promoteNum)
			{
				return false;
			}
			CharaPromotePreset charaPromotePreset = this.staticData.promoteList.Find((CharaPromotePreset x) => x.PromoteNum == this.dynamicData.promoteNum);
			int num = 0;
			foreach (CharaPromoteOne charaPromoteOne in charaPromotePreset.promoteOneList)
			{
				if (!this.dynamicData.promoteFlag[num])
				{
					if (charaPromoteOne.promoteUseItemNum > DataManager.DmItem.GetUserItemData(charaPromoteOne.promoteUseItemId).num)
					{
						num++;
					}
					else
					{
						if (charaPromoteOne.costGoldNum <= DataManager.DmItem.GetUserItemData(30101).num)
						{
							return true;
						}
						num++;
					}
				}
				else
				{
					num++;
				}
			}
			return false;
		}
	}

	public bool EnhanceInfoRank
	{
		get
		{
			if (this.staticData.baseData.rankHigh > this.dynamicData.rank)
			{
				if (this.GetNextItemByRankup(0).item.num > DataManager.DmItem.GetUserItemData(this.staticData.baseData.rankItemId).num)
				{
					return false;
				}
				if (this.GetNextItemByRankup(0).needGold > DataManager.DmItem.GetUserItemData(30101).num)
				{
					return false;
				}
			}
			else
			{
				if (!this.dynamicData.CheckCharaRankMaxConversion(true))
				{
					return false;
				}
				if (DataManager.DmItem.GetUserItemData(this.staticData.baseData.rankItemId).num == 0)
				{
					return false;
				}
			}
			return true;
		}
	}

	public bool EnhanceInfoMiracle
	{
		get
		{
			if (this.dynamicData.limitMiracleLevel <= this.dynamicData.artsLevel)
			{
				return false;
			}
			GrowItemList nextItemByArtsUp = this.GetNextItemByArtsUp(0);
			foreach (ItemData itemData in nextItemByArtsUp.itemList)
			{
				if (itemData.num > DataManager.DmItem.GetUserItemData(itemData.id).num)
				{
					return false;
				}
			}
			return nextItemByArtsUp.needGold <= DataManager.DmItem.GetUserItemData(30101).num;
		}
	}

	public bool EnhanceInfoPhotoPocket
	{
		get
		{
			if (this.dynamicData.PhotoPocket.Count != this.dynamicData.PhotoPocket.FindAll((CharaDynamicData.PPParam x) => x.Flag).Count)
			{
				if (this.GetNextItemByReleasePhotoFrame().item.num > DataManager.DmItem.GetUserItemData(this.staticData.baseData.ppItemId).num)
				{
					return false;
				}
				if (this.GetNextItemByReleasePhotoFrame().needGold > DataManager.DmItem.GetUserItemData(30101).num)
				{
					return false;
				}
			}
			else if (DataManager.DmItem.GetUserItemData(this.staticData.baseData.ppItemId).num == 0)
			{
				return false;
			}
			return true;
		}
	}

	public bool CanBeEnhancedPhotoPocket
	{
		get
		{
			return this.dynamicData.PhotoFrameTotalStep < DataManager.DmServerMst.StaticCharaPpDataMap[this.staticData.baseData.photoFrameTableId].PpStepMax && this.GetNextItemByReleasePhotoFrame().item.num <= DataManager.DmItem.GetUserItemData(this.staticData.baseData.ppItemId).num && this.GetNextItemByReleasePhotoFrame().needGold <= DataManager.DmItem.GetUserItemData(30101).num;
		}
	}

	public bool EnhanceInfoKizunaLimit
	{
		get
		{
			List<int> list = this.CalcEatLevelItem(0, 0, true);
			CharaDynamicData dynamicData = this.dynamicData;
			int nowLimitLevel = this.dynamicData.KizunaLimitLevel;
			bool flag = nowLimitLevel == dynamicData.kizunaLevel;
			if (list != null && list.Count != 0 && !flag)
			{
				return true;
			}
			GameLevelInfo gameLevelInfo = DataManager.DmServerMst.gameLevelInfoList.Find((GameLevelInfo x) => x.level == nowLimitLevel + 1);
			if (gameLevelInfo.kizunaLevelExp.ContainsKey(this.staticData.baseData.kizunaLevelId))
			{
				GameLevelInfo.KizunaLevelData kizunaLevelData = gameLevelInfo.kizunaLevelExp[this.staticData.baseData.kizunaLevelId];
				return flag && kizunaLevelData.releaseItemNum <= DataManager.DmItem.GetUserItemData(kizunaLevelData.releaseItemId).num;
			}
			return false;
		}
	}

	public bool EnhanceInfoNanairo
	{
		get
		{
			if (!this.IsNanairoAbilityReleaseAvailable)
			{
				return false;
			}
			if (!this.IsHaveNanairoAbility || this.dynamicData.nanairoAbilityReleaseFlag)
			{
				return false;
			}
			GrowItemList releaseItemByNanairoAbilityRelease = this.GetReleaseItemByNanairoAbilityRelease();
			if (releaseItemByNanairoAbilityRelease == null || releaseItemByNanairoAbilityRelease.itemList == null)
			{
				return false;
			}
			foreach (ItemData itemData in releaseItemByNanairoAbilityRelease.itemList)
			{
				if (itemData.num > DataManager.DmItem.GetUserItemData(itemData.id).num)
				{
					return false;
				}
			}
			return releaseItemByNanairoAbilityRelease.needGold <= DataManager.DmItem.GetUserItemData(30101).num;
		}
	}

	public bool IsEnableSpAbility
	{
		get
		{
			return this.IsHaveSpAbility && this.staticData.baseData.spAbilityRelPp <= this.dynamicData.PhotoFrameTotalStep;
		}
	}

	public bool IsHaveSpAbility
	{
		get
		{
			return this.staticData.spAbilityData != null && this.staticData.baseData.spAbilityRelPp > 0;
		}
	}

	public bool IsEnableNanairoAbility
	{
		get
		{
			return this.IsHaveNanairoAbility && this.dynamicData.nanairoAbilityReleaseFlag;
		}
	}

	public bool IsHaveNanairoAbility
	{
		get
		{
			return this.staticData.nanairoAbilityData != null && this.IsReleaseNanairoAbility();
		}
	}

	public bool IsNanairoAbilityReleaseAvailable
	{
		get
		{
			return this.IsNanairoConditionLevelOk() && this.IsNanairoConditionPromoteOk() && this.IsNanairoConditionPPOk() && this.IsNanairoConditionArtsOk();
		}
	}

	public bool IsNanairoConditionLevelOk()
	{
		return this.dynamicData.level >= 90;
	}

	public bool IsNanairoConditionPromoteOk()
	{
		return this.dynamicData.promoteNum >= 4;
	}

	public bool IsNanairoConditionPPOk()
	{
		return this.dynamicData.PhotoPocket.Count<CharaDynamicData.PPParam>((CharaDynamicData.PPParam v) => v.Flag) >= 2;
	}

	public bool IsNanairoConditionArtsOk()
	{
		return this.dynamicData.artsLevel >= 3;
	}

	public const int NanairoConditionLevel = 90;

	public const int NanairoConditionPromoteNum = 4;

	public const int NanairoConditionArtsLv = 3;
}
