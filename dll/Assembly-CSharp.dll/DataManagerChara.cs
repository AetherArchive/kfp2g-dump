using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SGNFW.Ab;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using UnityEngine;

public class DataManagerChara
{
	public DataManagerChara(DataManager p)
	{
		this.parentData = p;
	}

	public bool ShopUpdateRequired { get; set; }

	public bool CharaMissionUpdateRequired { get; set; }

	public static int CharaId2ModelId(int charaId)
	{
		if (Singleton<DataManager>.Instance != null && DataManager.DmChara.charaId2modelId != null && DataManager.DmChara.charaId2modelId.ContainsKey(charaId))
		{
			return DataManager.DmChara.charaId2modelId[charaId];
		}
		return charaId;
	}

	public List<CharaStaticData> CharaStaticDataList
	{
		get
		{
			List<CharaStaticData> list = new List<CharaStaticData>(this.charaStaticMap.Values);
			list.Sort((CharaStaticData a, CharaStaticData b) => a.baseData.id.CompareTo(b.baseData.id));
			return list;
		}
	}

	private List<DataManagerChara.FilterData> FilterDataListMiracle { get; set; }

	private List<DataManagerChara.FilterData> FilterDataListCharacteristic { get; set; }

	public CharaStaticData GetCharaStaticData(int charaId)
	{
		if (charaId < 0)
		{
			return null;
		}
		if (!this.charaStaticMap.ContainsKey(charaId))
		{
			Verbose<PrjLog>.LogError("Error : DataManagerChara.GetCharaStaticData : 定義されていないID[" + charaId.ToString() + "]を参照しようとしました", null);
			return null;
		}
		return this.charaStaticMap[charaId];
	}

	public CharaStaticBase GetCharaStaticBase(int charaId)
	{
		if (charaId < 0)
		{
			return null;
		}
		if (!this.charaStaticBaseMap.ContainsKey(charaId))
		{
			Verbose<PrjLog>.LogError("Error : DataManagerChara.GetCharaStaticBase : 定義されていないID[" + charaId.ToString() + "]を参照しようとしました", null);
			return null;
		}
		return this.charaStaticBaseMap[charaId];
	}

	public List<DataManagerChara.FilterData> GetFriendFilterDataList(bool filterMiracle)
	{
		List<DataManagerChara.FilterData> list = new List<DataManagerChara.FilterData>();
		DateTime now = TimeManager.Now;
		List<DataManagerChara.FilterData> list2 = new List<DataManagerChara.FilterData>();
		if (filterMiracle)
		{
			list2 = this.FilterDataListMiracle;
		}
		else
		{
			list2 = this.FilterDataListCharacteristic;
		}
		if (list2 == null)
		{
			return null;
		}
		foreach (DataManagerChara.FilterData filterData in list2)
		{
			if (filterData.StartDatetime < now)
			{
				list.Add(filterData);
			}
		}
		return list;
	}

	public static string GetNPCName(int charaId)
	{
		Dictionary<int, MstNpcData> dictionary = DataManager.DmChara.npcMap;
		if (dictionary.ContainsKey(charaId))
		{
			return PrjUtil.MakeMessage(dictionary[charaId].name);
		}
		Verbose<PrjLog>.LogError("Error : DataManagerChara.GetNPCName : 定義されていないID[" + charaId.ToString() + "]を参照しようとしました", null);
		return null;
	}

	public MstCharaEffectData GetCharaEffectData(string bodyModelName)
	{
		if (this.mstCharaEffectDataList != null)
		{
			foreach (MstCharaEffectData mstCharaEffectData in this.mstCharaEffectDataList)
			{
				if (bodyModelName.IndexOf(mstCharaEffectData.searchTarget) > 0)
				{
					return mstCharaEffectData;
				}
			}
		}
		return null;
	}

	public bool CheckSameChara(int charaIdA, int charaIdB)
	{
		return this.GetSameCharaList(charaIdA, true).Contains(charaIdB);
	}

	public bool CheckSameChara(int charaIdA, List<int> charaIdBList)
	{
		HashSet<int> list = this.GetSameCharaList(charaIdA, true);
		return charaIdBList.Exists((int item) => list.Contains(item));
	}

	public HashSet<int> GetSameCharaList(int charaId, bool isInMine)
	{
		HashSet<int> hashSet = new HashSet<int>();
		CharaStaticData charaStaticData = this.charaStaticMap.TryGetValueEx(charaId, null);
		if (charaStaticData != null)
		{
			if (charaStaticData.baseData.OriginalId > 0)
			{
				hashSet.Add(charaStaticData.baseData.OriginalId);
			}
			hashSet.UnionWith(charaStaticData.baseData.SynonymIdSet);
		}
		HashSet<int> hashSet2 = this.groupCharaMapByChara.TryGetValueEx(charaId, null);
		if (hashSet2 != null)
		{
			hashSet.UnionWith(hashSet2);
		}
		if (isInMine)
		{
			hashSet.Add(charaId);
		}
		else
		{
			hashSet.Remove(charaId);
		}
		return hashSet;
	}

	public MasterStaticSkill GetMasterSkillStaticData(int id)
	{
		if (!this.masterSkillStaticMap.ContainsKey(id))
		{
			return this.masterSkillStaticMap[80001];
		}
		return this.masterSkillStaticMap[id];
	}

	public TacticsStaticSkill GetTacticsSkillStaticData(int id)
	{
		if (this.tacticsSkillStaticMap.ContainsKey(id))
		{
			return this.tacticsSkillStaticMap[id];
		}
		if (this.tacticsSkillStaticMap.Values.Count > 0)
		{
			return this.tacticsSkillStaticMap.Values.ToArray<TacticsStaticSkill>()[Random.Range(0, this.tacticsSkillStaticMap.Values.Count)];
		}
		return new TacticsStaticSkill
		{
			id = id,
			type = TacticsStaticSkill.Type.INVALID,
			skillName = "",
			skillInfo = ""
		};
	}

	public List<TacticsStaticSkill> GetTacticsSkillStaticData()
	{
		return new List<TacticsStaticSkill>(this.tacticsSkillStaticMap.Values);
	}

	public TacticsParam.Tactics GetTacticsParamData(TacticsParam.Tactics.Type typ)
	{
		if (this.tacticsParam == null || this.tacticsParam.tacticsParam == null || this.tacticsParam.tacticsParam.Count < 1)
		{
			return new TacticsParam.Tactics
			{
				type = typ,
				tacticsName = "",
				paramInfo = "",
				param = new List<int> { 1 }
			};
		}
		TacticsParam.Tactics tactics = this.tacticsParam.tacticsParam.Find((TacticsParam.Tactics itm) => itm.type == typ);
		if (tactics == null)
		{
			tactics = this.tacticsParam.tacticsParam[Random.Range(0, this.tacticsParam.tacticsParam.Count)];
		}
		return tactics;
	}

	public List<TacticsParam.Tactics> GetTacticsParamData()
	{
		if (!(this.tacticsParam == null) && this.tacticsParam.tacticsParam != null)
		{
			return this.tacticsParam.tacticsParam;
		}
		return new List<TacticsParam.Tactics>();
	}

	public EnemyStaticData GetEnemyStaticData(int enemyId)
	{
		if (!this.enemyStaticMap.ContainsKey(enemyId))
		{
			Verbose<PrjLog>.LogError("Error : DataManagerChara.GetEnemyStaticData : 定義されていないID[" + enemyId.ToString() + "]を生成しようとしました：プランナに連絡してください", null);
			return null;
		}
		return this.enemyStaticMap[enemyId];
	}

	public Dictionary<int, EnemyStaticData> GetEnemyStaticMap()
	{
		return this.enemyStaticMap;
	}

	public CharaClothStatic GetCharaClothesStaticData(int clothesId)
	{
		if (!this.charaClothesStaticMap.ContainsKey(clothesId))
		{
			Verbose<PrjLog>.LogError("Error : DataManagerChara.GetCharaClothesStaticData : 定義されていないID[" + clothesId.ToString() + "]を生成しようとしました：プランナに連絡してください", null);
			return null;
		}
		return this.charaClothesStaticMap[clothesId];
	}

	public CharaContactStatic GetCharaContactStaticData(int contactId)
	{
		if (!this.charaContactStaticMap.ContainsKey(contactId))
		{
			Verbose<PrjLog>.LogError("Error : DataManagerChara.GetCharaContactStaticData : 定義されていないID[" + contactId.ToString() + "]を生成しようとしました：プランナに連絡してください", null);
			return null;
		}
		return this.charaContactStaticMap[contactId];
	}

	public List<CharaContactStatic> GetContactByChara(int charaId)
	{
		return new List<CharaContactStatic>(this.charaContactStaticMapByChara.TryGetValueEx(charaId, new List<CharaContactStatic>()));
	}

	public List<CharaVoiceCombi> GetCharaVoiceCombiAllList()
	{
		return this.charaVoiceCombiList;
	}

	public List<CharaVoiceCombi> GetCharaVoiceCombiList(CharaVoiceCombi.SitType type, int firstCharaId)
	{
		return this.charaVoiceCombiList.FindAll((CharaVoiceCombi item) => item.firstCharaId == firstCharaId && item.situationType == type);
	}

	public List<DataManagerChara.BonusCharaData> GetBonusCharaDataList()
	{
		return this.GetBonusCharaDataListInternal(0, TimeManager.Now, true);
	}

	public List<DataManagerChara.BonusCharaData> GetBonusCharaDataList(int evID)
	{
		return this.GetBonusCharaDataListInternal(evID, TimeManager.Now, false);
	}

	public List<DataManagerChara.BonusCharaData> GetBonusCharaDataList(int evID, DateTime dateTime)
	{
		return this.GetBonusCharaDataListInternal(evID, dateTime, false);
	}

	private List<DataManagerChara.BonusCharaData> GetBonusCharaDataListInternal(int evID, DateTime dateTime, bool returnAll = false)
	{
		List<DataManagerChara.BonusCharaData> list;
		if (returnAll)
		{
			list = this.bonusCharaDataList.FindAll((DataManagerChara.BonusCharaData x) => x.startDatetime < dateTime && dateTime < x.endDatetime);
		}
		else if (evID == 0)
		{
			list = this.bonusCharaDataList.FindAll((DataManagerChara.BonusCharaData x) => x.eventId == 0 && x.startDatetime < dateTime && dateTime < x.endDatetime);
		}
		else
		{
			list = this.bonusCharaDataList.FindAll((DataManagerChara.BonusCharaData x) => (x.eventId == 0 || evID == x.eventId) && x.startDatetime < dateTime && dateTime < x.endDatetime);
		}
		return list;
	}

	public int ColothId2ImageId(int clothId)
	{
		if (clothId == 0)
		{
			return 0;
		}
		CharaClothStatic charaClothesStaticData = this.GetCharaClothesStaticData(clothId);
		if (charaClothesStaticData == null)
		{
			return 0;
		}
		return charaClothesStaticData.ImageId;
	}

	public bool ClothLongSkirt(int clothId)
	{
		if (clothId == 0)
		{
			return false;
		}
		CharaClothStatic charaClothesStaticData = this.GetCharaClothesStaticData(clothId);
		return charaClothesStaticData != null && charaClothesStaticData.LongSkirt;
	}

	public CharaClothStatic.PlayMotionType ClothPlayMotion(int clothId)
	{
		if (clothId == 0)
		{
			return CharaClothStatic.PlayMotionType.Default;
		}
		CharaClothStatic charaClothesStaticData = this.GetCharaClothesStaticData(clothId);
		if (charaClothesStaticData != null)
		{
			return charaClothesStaticData.PlayMotion;
		}
		return CharaClothStatic.PlayMotionType.Default;
	}

	public CharaPackData GetUserCharaData(int charaId)
	{
		if (this.userCharaMap.ContainsKey(charaId))
		{
			return this.userCharaMap[charaId];
		}
		return null;
	}

	public Dictionary<int, CharaPackData> GetUserCharaMap()
	{
		return this.userCharaMap;
	}

	public Dictionary<int, int> UserAllCharaKemoStatusList { get; private set; } = new Dictionary<int, int>();

	public bool IsNeedUpdateByUserAllCharaKemoStatusList { get; set; }

	public int UserAllCharaKemoStatus
	{
		get
		{
			return this.UserAllCharaKemoStatusList.Sum<KeyValuePair<int, int>>((KeyValuePair<int, int> item) => item.Value);
		}
	}

	public MasterSkillPackData GetUserMasterSkillData(int id)
	{
		if (this.userMasterSkillMap.ContainsKey(id))
		{
			return this.userMasterSkillMap[id];
		}
		return null;
	}

	public Dictionary<int, MasterSkillPackData> GetUserMasterSkillMap()
	{
		return this.userMasterSkillMap;
	}

	public List<MstCharaKizunaBuffData> GetMstKizunaBuffList()
	{
		return this.mstCharaKizunaBuffList;
	}

	public CharaKizunaQualified GetUserKizunaQualified()
	{
		return this.userCharaKizunaQualified;
	}

	public PrjUtil.ParamPreset GetActiveKizunaBuff()
	{
		PrjUtil.ParamPreset paramPreset = new PrjUtil.ParamPreset();
		if (this.userCharaKizunaQualified != null)
		{
			List<MstCharaKizunaBuffData> list = DataManager.DmChara.GetMstKizunaBuffList().FindAll((MstCharaKizunaBuffData buff) => buff.requiredCount <= this.userCharaKizunaQualified.qualified_count);
			if (list != null)
			{
				PrjUtil.ParamPreset paramPreset2 = paramPreset;
				paramPreset2.hp += list.Sum<MstCharaKizunaBuffData>((MstCharaKizunaBuffData buff) => buff.hpBonus);
				PrjUtil.ParamPreset paramPreset3 = paramPreset;
				paramPreset3.atk += list.Sum<MstCharaKizunaBuffData>((MstCharaKizunaBuffData buff) => buff.atkBonus);
				PrjUtil.ParamPreset paramPreset4 = paramPreset;
				paramPreset4.def += list.Sum<MstCharaKizunaBuffData>((MstCharaKizunaBuffData buff) => buff.defBonus);
				PrjUtil.ParamPreset paramPreset5 = paramPreset;
				paramPreset5.avoid += list.Sum<MstCharaKizunaBuffData>((MstCharaKizunaBuffData buff) => buff.avoidBonus);
				PrjUtil.ParamPreset paramPreset6 = paramPreset;
				paramPreset6.beatDamageRatio += list.Sum<MstCharaKizunaBuffData>((MstCharaKizunaBuffData buff) => buff.beatBonus);
				PrjUtil.ParamPreset paramPreset7 = paramPreset;
				paramPreset7.actionDamageRatio += list.Sum<MstCharaKizunaBuffData>((MstCharaKizunaBuffData buff) => buff.actionBonus);
				PrjUtil.ParamPreset paramPreset8 = paramPreset;
				paramPreset8.tryDamageRatio += list.Sum<MstCharaKizunaBuffData>((MstCharaKizunaBuffData buff) => buff.tryBonus);
			}
		}
		return paramPreset;
	}

	public PrjUtil.ParamPreset GetActiveKizunaBuffByQualifiedCount(int count)
	{
		PrjUtil.ParamPreset paramPreset = new PrjUtil.ParamPreset();
		List<MstCharaKizunaBuffData> list = DataManager.DmChara.GetMstKizunaBuffList().FindAll((MstCharaKizunaBuffData buff) => buff.requiredCount <= count);
		if (list != null)
		{
			PrjUtil.ParamPreset paramPreset2 = paramPreset;
			paramPreset2.hp += list.Sum<MstCharaKizunaBuffData>((MstCharaKizunaBuffData buff) => buff.hpBonus);
			PrjUtil.ParamPreset paramPreset3 = paramPreset;
			paramPreset3.atk += list.Sum<MstCharaKizunaBuffData>((MstCharaKizunaBuffData buff) => buff.atkBonus);
			PrjUtil.ParamPreset paramPreset4 = paramPreset;
			paramPreset4.def += list.Sum<MstCharaKizunaBuffData>((MstCharaKizunaBuffData buff) => buff.defBonus);
			PrjUtil.ParamPreset paramPreset5 = paramPreset;
			paramPreset5.avoid += list.Sum<MstCharaKizunaBuffData>((MstCharaKizunaBuffData buff) => buff.avoidBonus);
			PrjUtil.ParamPreset paramPreset6 = paramPreset;
			paramPreset6.beatDamageRatio += list.Sum<MstCharaKizunaBuffData>((MstCharaKizunaBuffData buff) => buff.beatBonus);
			PrjUtil.ParamPreset paramPreset7 = paramPreset;
			paramPreset7.actionDamageRatio += list.Sum<MstCharaKizunaBuffData>((MstCharaKizunaBuffData buff) => buff.actionBonus);
			PrjUtil.ParamPreset paramPreset8 = paramPreset;
			paramPreset8.tryDamageRatio += list.Sum<MstCharaKizunaBuffData>((MstCharaKizunaBuffData buff) => buff.tryBonus);
		}
		return paramPreset;
	}

	public DataManagerChara.KiznaRewardData GetKizunaRewardData(int kizunaLv, int charaId)
	{
		DataManagerChara.KiznaRewardData kiznaRewardData = new DataManagerChara.KiznaRewardData
		{
			level = kizunaLv
		};
		MstKizunaRewardData mstKizunaRewardData = this.mstKizunaRewardList.Find((MstKizunaRewardData x) => x.level == kizunaLv);
		MstCharaKizunaRewardData mstCharaKizunaRewardData = this.mstCharaKizunaRewardList.Find((MstCharaKizunaRewardData x) => x.charaId == charaId && x.kizunaLevel == kizunaLv);
		if (mstKizunaRewardData != null)
		{
			kiznaRewardData.artsMax = mstKizunaRewardData.artsMax;
			kiznaRewardData.charaquest = mstKizunaRewardData.charaquest;
			int itemId = mstKizunaRewardData.itemId;
			if (mstCharaKizunaRewardData != null)
			{
				kiznaRewardData.itemId = mstCharaKizunaRewardData.rewardItemId;
				kiznaRewardData.itemNum = mstCharaKizunaRewardData.rewardItemNum;
			}
			if (kiznaRewardData.itemId != 0)
			{
				DataManagerAchievement.AchievementStaticData achievementData = DataManager.DmAchievement.GetAchievementData(kiznaRewardData.itemId);
				if (achievementData != null && TimeManager.Now < achievementData.startTime)
				{
					return null;
				}
			}
		}
		return kiznaRewardData;
	}

	public DataManagerChara.LevelLimitData GetLevelLimitData(int limitId)
	{
		return this.levelLimitDataList.Find((DataManagerChara.LevelLimitData x) => x.levelLimitId == limitId && x.isStartTime);
	}

	public int GetLevelLimitDataListCount()
	{
		return this.levelLimitDataList.Count;
	}

	public int GetLevelLimitOverCount(int maxLevel)
	{
		int num = 0;
		if (90 >= maxLevel)
		{
			return num;
		}
		foreach (DataManagerChara.LevelLimitData levelLimitData in this.levelLimitDataList)
		{
			num = levelLimitData.levelLimitId;
			if (levelLimitData.maxLevel >= maxLevel)
			{
				break;
			}
		}
		return num;
	}

	public int GetKizunaLevelLimitOverCount(int maxLevel)
	{
		int num = 0;
		if (6 >= maxLevel)
		{
			return num;
		}
		return maxLevel - 6;
	}

	public DataManagerChara.LevelLimitRisingStatus GetLimitRisingStatus(int patternId, int charaId)
	{
		int num = this.levelLimitRisingStatusList.Min<DataManagerChara.LevelLimitRisingStatus>((DataManagerChara.LevelLimitRisingStatus x) => x.patternId);
		int num2 = this.levelLimitRisingStatusList.Max<DataManagerChara.LevelLimitRisingStatus>((DataManagerChara.LevelLimitRisingStatus x) => x.patternId);
		if (patternId < num || patternId > num2)
		{
			patternId = 1;
		}
		return this.levelLimitRisingStatusList.Find((DataManagerChara.LevelLimitRisingStatus data) => data.patternId == patternId);
	}

	public void UpdateUserDataByServer(List<Chara> haveCharaList)
	{
		bool flag = false;
		foreach (Chara chara in haveCharaList)
		{
			int chara_id = chara.chara_id;
			if (!this.userCharaMap.ContainsKey(chara_id))
			{
				CharaDynamicData charaDynamicData = new CharaDynamicData();
				charaDynamicData.id = chara_id;
				charaDynamicData.OwnerType = CharaDynamicData.CharaOwnerType.User;
				this.userCharaMap.Add(chara_id, new CharaPackData(charaDynamicData));
				flag = true;
			}
			this.userCharaMap[chara_id].dynamicData.UpdateByServer(chara);
		}
		if (flag)
		{
			this.CharaMissionUpdateRequired = true;
			List<ShopData> shopDataList = DataManager.DmShop.GetShopDataList(true, true, ShopData.TabCategory.ALL);
			if (0 < shopDataList.Count)
			{
				this.ShopUpdateRequired = true;
			}
		}
	}

	public void UpdateUserDataByServer(List<Item> userItemList)
	{
		foreach (Item item in userItemList)
		{
			if (ItemDef.Id2Kind(item.item_id) == ItemDef.Kind.MASTER_SKILL && !this.userMasterSkillMap.ContainsKey(item.item_id))
			{
				this.userMasterSkillMap.Add(item.item_id, MasterSkillPackData.MakeDummy(item.item_id));
			}
		}
	}

	public void UpdateUserCharasClothesData()
	{
		foreach (CharaPackData charaPackData in this.userCharaMap.Values)
		{
			this.UpdateUserCharaClothesData(charaPackData);
		}
	}

	private void UpdateUserCharaClothesData(CharaPackData haveChara)
	{
		int id = haveChara.id;
		List<CharaClothStatic> clothListByChara = this.GetClothListByChara(id);
		List<int> list = new List<int>();
		foreach (CharaClothStatic charaClothStatic in clothListByChara)
		{
			if (0 < DataManager.DmItem.GetUserItemData(charaClothStatic.GetId()).num)
			{
				list.Add(charaClothStatic.GetId());
			}
		}
		haveChara.dynamicData.haveClothesIdList = new List<int>(list);
	}

	public void UpdateUserCharasContactData()
	{
		foreach (CharaPackData charaPackData in this.userCharaMap.Values)
		{
			this.UpdateUserCharaContactData(charaPackData);
		}
	}

	private void UpdateUserCharaContactData(CharaPackData haveChara)
	{
		int id = haveChara.id;
		List<CharaContactStatic> list = this.charaContactStaticMapByChara.TryGetValueEx(id, new List<CharaContactStatic>());
		haveChara.dynamicData.haveContactItemIdList = new HashSet<int>();
		foreach (CharaContactStatic charaContactStatic in list)
		{
			if (0 < DataManager.DmItem.GetUserItemData(charaContactStatic.GetId()).num)
			{
				haveChara.dynamicData.haveContactItemIdList.Add(charaContactStatic.GetId());
			}
			else if (charaContactStatic.IsDefaultItem)
			{
				haveChara.dynamicData.haveContactItemIdList.Add(charaContactStatic.GetId());
			}
		}
	}

	public void UpdateSumUserAllCharaKemoStatus(List<int> updateCharaIdList)
	{
		if (updateCharaIdList == null)
		{
			this.UserAllCharaKemoStatusList.Clear();
			using (Dictionary<int, CharaPackData>.Enumerator enumerator = this.userCharaMap.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, CharaPackData> keyValuePair = enumerator.Current;
					this.UserAllCharaKemoStatusList[keyValuePair.Key] = PrjUtil.CalcParamByCharaWithKemoBoard(keyValuePair.Value).totalParam;
				}
				goto IL_00B5;
			}
		}
		foreach (int num in updateCharaIdList)
		{
			CharaPackData charaPackData = this.userCharaMap.TryGetValueEx(num, null);
			if (charaPackData != null)
			{
				this.UserAllCharaKemoStatusList[num] = PrjUtil.CalcParamByCharaWithKemoBoard(charaPackData).totalParam;
			}
		}
		IL_00B5:
		this.IsNeedUpdateByUserAllCharaKemoStatusList = true;
	}

	public void UpdateUserDataByTutorial(List<CharaPackData> charaList)
	{
		this.userCharaMap = new Dictionary<int, CharaPackData>();
		foreach (CharaPackData charaPackData in charaList)
		{
			this.userCharaMap.Add(charaPackData.id, charaPackData);
		}
	}

	public void UpdateUserDataByTutorial(List<MasterSkillPackData> skillList)
	{
		this.userMasterSkillMap = new Dictionary<int, MasterSkillPackData>();
		foreach (MasterSkillPackData masterSkillPackData in skillList)
		{
			this.userMasterSkillMap.Add(masterSkillPackData.id, masterSkillPackData);
		}
	}

	public void UpdateKizunaQualifiedByServer(CharaKizunaQualified qualified)
	{
		this.userCharaKizunaQualified = qualified;
	}

	public void UpdateUserDataByDebug(int charaId)
	{
		if (!this.userCharaMap.ContainsKey(charaId))
		{
			this.userCharaMap.Add(charaId, CharaPackData.MakeInitial(charaId));
		}
	}

	public IEnumerator InitializeMstData(MstManager mstManager)
	{
		long nowServerTime = PrjUtil.ConvertTicksToTime(TimeManager.Now.Ticks);
		this.loadAssetParameterList = AssetManager.LoadAssetDataByCategory(AssetManager.ASSET_CATEGORY_PARAMETER, AssetManager.OWNER.DataManagerChara, 0, null);
		foreach (string assetName in this.loadAssetParameterList)
		{
			while (!AssetManager.IsLoadFinishAssetData(AssetManager.PREFIX_PATH_CHARA_PARAM + assetName))
			{
				yield return null;
			}
			assetName = null;
		}
		List<string>.Enumerator enumerator = default(List<string>.Enumerator);
		List<MstCharaData> mst = mstManager.GetMst<List<MstCharaData>>(MstType.CHARA_DATA);
		List<MstCharaLimitlevelData> mst2 = mstManager.GetMst<List<MstCharaLimitlevelData>>(MstType.CHARA_LIMITLEVEL_DATA);
		List<MstLimitLevelRaisingStatus> mst3 = mstManager.GetMst<List<MstLimitLevelRaisingStatus>>(MstType.LIMITLEVEL_RISING_STATUS);
		List<MstCharaClothesData> mst4 = mstManager.GetMst<List<MstCharaClothesData>>(MstType.CHARA_CLOTHES_DATA);
		List<MstCharaMotionItemData> mst5 = mstManager.GetMst<List<MstCharaMotionItemData>>(MstType.CHARA_MOTION_ITEM_DATA);
		List<MstCharaPromoteData> mst6 = mstManager.GetMst<List<MstCharaPromoteData>>(MstType.CHARA_PROMOTE_DATA);
		List<MstCharaPromotePresetData> mst7 = mstManager.GetMst<List<MstCharaPromotePresetData>>(MstType.CHARA_PROMOTE_PRESET_DATA);
		List<MstCharaVoiceComboData> list = new List<MstCharaVoiceComboData>(mstManager.GetMst<List<MstCharaVoiceComboData>>(MstType.CHARA_VOICE_COMBO_DATA));
		this.charaVoiceCombiList = list.ConvertAll<CharaVoiceCombi>((MstCharaVoiceComboData item) => new CharaVoiceCombi(item));
		List<MstEventBonusCharaData> mst8 = mstManager.GetMst<List<MstEventBonusCharaData>>(MstType.EVENT_BONUS_CHARA_DATA);
		List<MstCharaGroupData> mst9 = mstManager.GetMst<List<MstCharaGroupData>>(MstType.CHARA_GROUP_DATA);
		List<MstCharaFilterData> mst10 = mstManager.GetMst<List<MstCharaFilterData>>(MstType.CHARA_FILTER_DATA);
		this.mstKizunaRewardList = mstManager.GetMst<List<MstKizunaRewardData>>(MstType.KIZUNA_REWARD_DATA);
		this.mstCharaKizunaRewardList = mstManager.GetMst<List<MstCharaKizunaRewardData>>(MstType.CHARA_KIZUNA_REWARD_DATA);
		List<MstEnemyCharaData> mst11 = mstManager.GetMst<List<MstEnemyCharaData>>(MstType.ENEMY_CHARA_DATA);
		this.mstCharaKizunaBuffList = mstManager.GetMst<List<MstCharaKizunaBuffData>>(MstType.CHARA_KIZUNA_BUFF_DATA);
		this.mstCharaEffectDataList = mstManager.GetMst<List<MstCharaEffectData>>(MstType.CHARA_EFFECT_DATA);
		foreach (MstNpcData mstNpcData in new List<MstNpcData>(mstManager.GetMst<List<MstNpcData>>(MstType.NPC_DATA)))
		{
			this.npcMap.Add(mstNpcData.id, mstNpcData);
		}
		this.levelLimitDataList = new List<DataManagerChara.LevelLimitData>();
		foreach (MstCharaLimitlevelData mstCharaLimitlevelData in mst2)
		{
			DataManagerChara.LevelLimitData levelLimitData = new DataManagerChara.LevelLimitData
			{
				levelLimitId = mstCharaLimitlevelData.limitId,
				needItemId01 = mstCharaLimitlevelData.needItemId01,
				needItemNum01 = mstCharaLimitlevelData.needItemNum01,
				needItemId02 = mstCharaLimitlevelData.needItemId02,
				needItemNum02 = mstCharaLimitlevelData.needItemNum02,
				needGoldNum = mstCharaLimitlevelData.needGoldNum,
				maxLevel = mstCharaLimitlevelData.maxLevel,
				openImageName = mstCharaLimitlevelData.openImageName,
				compImageName = mstCharaLimitlevelData.compImageName,
				isStartTime = (TimeManager.Now.Ticks > PrjUtil.ConvertTimeToTicks(mstCharaLimitlevelData.startTime))
			};
			this.levelLimitDataList.Add(levelLimitData);
		}
		this.levelLimitRisingStatusList = new List<DataManagerChara.LevelLimitRisingStatus>();
		foreach (MstLimitLevelRaisingStatus mstLimitLevelRaisingStatus in mst3)
		{
			DataManagerChara.LevelLimitRisingStatus levelLimitRisingStatus = new DataManagerChara.LevelLimitRisingStatus
			{
				patternId = mstLimitLevelRaisingStatus.patternId,
				addHp = mstLimitLevelRaisingStatus.hp,
				addAtk = mstLimitLevelRaisingStatus.atk,
				addDef = mstLimitLevelRaisingStatus.def
			};
			this.levelLimitRisingStatusList.Add(levelLimitRisingStatus);
		}
		Dictionary<int, CharaPromoteOne> dictionary = new Dictionary<int, CharaPromoteOne>();
		foreach (MstCharaPromoteData mstCharaPromoteData in mst6)
		{
			CharaPromoteOne charaPromoteOne = new CharaPromoteOne
			{
				promoteId = mstCharaPromoteData.promoteId,
				promoteHp = mstCharaPromoteData.promoteHp,
				promoteAtk = mstCharaPromoteData.promoteAtk,
				promoteDef = mstCharaPromoteData.promoteDef,
				promoteAvoid = mstCharaPromoteData.promoteAvoid,
				promoteActionDamageRatio = mstCharaPromoteData.promoteActionDamageRatio,
				promoteTryDamageRatio = mstCharaPromoteData.promoteTryDamageRatio,
				promoteBeatDamageRatio = mstCharaPromoteData.promoteBeatDamageRatio,
				promoteUseItemId = mstCharaPromoteData.promoteUseItemId00,
				promoteUseItemNum = mstCharaPromoteData.promoteUseItemNum00,
				costGoldNum = mstCharaPromoteData.promoteCostItemNum
			};
			dictionary.Add(charaPromoteOne.promoteId, charaPromoteOne);
		}
		List<ItemStaticBase> list2 = new List<ItemStaticBase>();
		this.charaStaticMap = new Dictionary<int, CharaStaticData>();
		this.charaId2modelId = new Dictionary<int, int>();
		foreach (MstCharaData mstCharaData in mst)
		{
			int id = mstCharaData.id;
			if (!string.IsNullOrEmpty(mstCharaData.modelFileName))
			{
				int.TryParse(mstCharaData.modelFileName, out id);
			}
			int id2 = mstCharaData.id;
			if (!string.IsNullOrEmpty(mstCharaData.paramFileName))
			{
				int.TryParse(mstCharaData.paramFileName, out id2);
			}
			CharaStaticData csd = new CharaStaticData();
			CharaStaticAlphaBase charaStaticAlphaBase = AssetManager.GetAssetData("Charas/Parameter/AlphaBase/ParamAlphaBase_" + id2.ToString("0000")) as CharaStaticAlphaBase;
			if (!(charaStaticAlphaBase == null))
			{
				csd.baseData = new CharaStaticBase(charaStaticAlphaBase, mstCharaData, id);
				this.charaStaticBaseMap.Add(mstCharaData.id, csd.baseData);
				string text = "Charas/Parameter/Arts/ParamArts_" + id2.ToString("0000");
				CharaStaticAction charaStaticAction = AssetManager.GetAssetData(text) as CharaStaticAction;
				if (charaStaticAction == null)
				{
					Verbose<PrjLog>.LogError("Error : DataManagerChara.InitializeStatic : Asset Dataが存在してません：" + text + "：プランナに連絡してください", null);
				}
				csd.artsData = charaStaticAction;
				string text2 = "Charas/Parameter/NormalAttack/ParamNormalAttack_" + id2.ToString("0000");
				CharaStaticAction charaStaticAction2 = AssetManager.GetAssetData(text2) as CharaStaticAction;
				if (charaStaticAction2 == null)
				{
					Verbose<PrjLog>.LogError("Error : DataManagerChara.InitializeStatic : Asset Dataが存在してません：" + text2 + "：プランナに連絡してください", null);
				}
				csd.normalAttackData = charaStaticAction2;
				CharaStaticAction charaStaticAction3 = AssetManager.GetAssetData("Charas/Parameter/SpecialAttack/ParamSpecialAttack_" + id2.ToString("0000") + "_1") as CharaStaticAction;
				charaStaticAction3 == null;
				csd.specialFlagAttackData = charaStaticAction3;
				string text3 = "Charas/Parameter/SpecialAttack/ParamSpecialAttack_" + id2.ToString("0000");
				CharaStaticAction charaStaticAction4 = AssetManager.GetAssetData(text3) as CharaStaticAction;
				if (charaStaticAction4 == null)
				{
					Verbose<PrjLog>.LogError("Error : DataManagerChara.InitializeStatic : Asset Dataが存在してません：" + text3 + "：プランナに連絡してください", null);
				}
				csd.specialAttackData = charaStaticAction4;
				string text4 = "Charas/Parameter/WaitAction/ParamWaitAction_" + id2.ToString("0000");
				CharaStaticWaitSkill charaStaticWaitSkill = AssetManager.GetAssetData(text4) as CharaStaticWaitSkill;
				if (charaStaticWaitSkill == null)
				{
					Verbose<PrjLog>.LogError("Error : DataManagerChara.InitializeStatic : Asset Dataが存在してません：" + text4 + "：プランナに連絡してください", null);
				}
				csd.waitActionData = charaStaticWaitSkill;
				string text5 = "Charas/Parameter/Ability/ParamAbility_" + id2.ToString("0000");
				CharaStaticAbility charaStaticAbility = AssetManager.GetAssetData(text5) as CharaStaticAbility;
				if (charaStaticAbility == null)
				{
					Verbose<PrjLog>.LogError("Error : DataManagerChara.InitializeStatic : Asset Dataが存在してません：" + text5 + "：プランナに連絡してください", null);
				}
				csd.abilityData.Add(charaStaticAbility);
				CharaStaticAbility charaStaticAbility2 = AssetManager.GetAssetData("Charas/Parameter/Ability/ParamAbility_" + id2.ToString("0000") + "_1") as CharaStaticAbility;
				charaStaticAbility2 == null;
				csd.spAbilityData = charaStaticAbility2;
				CharaStaticAbility charaStaticAbility3 = AssetManager.GetAssetData("Charas/Parameter/Ability/ParamAbility_" + id2.ToString("0000") + "_2") as CharaStaticAbility;
				charaStaticAbility3 == null;
				csd.nanairoAbilityData = charaStaticAbility3;
				csd.orderCardList = new List<CharaOrderCard>
				{
					new CharaOrderCard(csd.baseData.orderCardType00, csd.baseData.orderCardValue00, csd.baseData.orderCardSPValueMP, csd.baseData.orderCardSPValuePlasm),
					new CharaOrderCard(csd.baseData.orderCardType01, csd.baseData.orderCardValue01, csd.baseData.orderCardSPValueMP, csd.baseData.orderCardSPValuePlasm),
					new CharaOrderCard(csd.baseData.orderCardType02, csd.baseData.orderCardValue02, csd.baseData.orderCardSPValueMP, csd.baseData.orderCardSPValuePlasm),
					new CharaOrderCard(csd.baseData.orderCardType03, csd.baseData.orderCardValue03, csd.baseData.orderCardSPValueMP, csd.baseData.orderCardSPValuePlasm),
					new CharaOrderCard(csd.baseData.orderCardType04, csd.baseData.orderCardValue04, csd.baseData.orderCardSPValueMP, csd.baseData.orderCardSPValuePlasm)
				};
				csd.promoteList = new List<CharaPromotePreset>();
				List<MstCharaPromotePresetData> list3 = mst7.FindAll((MstCharaPromotePresetData item) => item.promotePresetId == csd.baseData.promotePresetId);
				int num = 0;
				int j;
				int i;
				for (i = 0; i < 100; i = j + 1)
				{
					MstCharaPromotePresetData mstCharaPromotePresetData = list3.Find((MstCharaPromotePresetData item) => item.promoteStep == i);
					if (mstCharaPromotePresetData == null)
					{
						break;
					}
					if (mstCharaPromotePresetData.promoteStepDatetime <= nowServerTime)
					{
						num++;
					}
					CharaPromotePreset charaPromotePreset = new CharaPromotePreset
					{
						PromoteNum = i
					};
					charaPromotePreset.promoteOneList = new List<CharaPromoteOne>
					{
						dictionary[mstCharaPromotePresetData.promoteId00],
						dictionary[mstCharaPromotePresetData.promoteId01],
						dictionary[mstCharaPromotePresetData.promoteId02],
						dictionary[mstCharaPromotePresetData.promoteId03],
						dictionary[mstCharaPromotePresetData.promoteId04],
						dictionary[mstCharaPromotePresetData.promoteId05]
					};
					csd.promoteList.Add(charaPromotePreset);
					j = i;
				}
				csd.SetMaxPromoteNumInternal(num);
				DataManager.DmItem.AddMstDataByItemCharaMap(mstCharaData.rankItemId, id);
				DataManager.DmItem.AddMstDataByItemCharaMap(mstCharaData.ppItemId, id);
				DataManager.DmItem.AddMstDataByItemCharaMap(mstCharaData.promoteItemId, id);
				foreach (CharaPromotePreset charaPromotePreset2 in csd.promoteList)
				{
					foreach (CharaPromoteOne charaPromoteOne2 in charaPromotePreset2.promoteOneList)
					{
						if (ItemDef.Id2Kind(charaPromoteOne2.promoteUseItemId) == ItemDef.Kind.PROMOTE_EXT)
						{
							DataManager.DmItem.AddMstDataByItemCharaMap(charaPromoteOne2.promoteUseItemId, id);
						}
					}
				}
				this.charaId2modelId.Add(mstCharaData.id, id);
				list2.Add(csd);
				this.charaStaticMap.Add(mstCharaData.id, csd);
			}
		}
		foreach (KeyValuePair<int, CharaStaticData> keyValuePair in this.charaStaticMap)
		{
			if (keyValuePair.Value.baseData.OriginalId != 0 && this.charaStaticMap.ContainsKey(keyValuePair.Value.baseData.OriginalId))
			{
				this.charaStaticMap[keyValuePair.Value.baseData.OriginalId].baseData.SynonymIdSet.Add(keyValuePair.Value.baseData.id);
			}
		}
		foreach (KeyValuePair<int, CharaStaticData> keyValuePair2 in this.charaStaticMap)
		{
			if (keyValuePair2.Value.baseData.OriginalId != 0 && this.charaStaticMap.ContainsKey(keyValuePair2.Value.baseData.OriginalId))
			{
				foreach (int num2 in this.charaStaticMap[keyValuePair2.Value.baseData.OriginalId].baseData.SynonymIdSet)
				{
					if (keyValuePair2.Value.baseData.id != num2)
					{
						keyValuePair2.Value.baseData.SynonymIdSet.Add(num2);
					}
				}
			}
		}
		new Dictionary<int, HashSet<int>>();
		foreach (MstCharaGroupData mstCharaGroupData in mst9)
		{
			if (!this.groupCharaMapByChara.ContainsKey(mstCharaGroupData.charaId))
			{
				this.groupCharaMapByChara[mstCharaGroupData.charaId] = new HashSet<int>();
			}
			if (!this.groupCharaMapByChara.ContainsKey(mstCharaGroupData.groupcharaId))
			{
				this.groupCharaMapByChara[mstCharaGroupData.groupcharaId] = new HashSet<int>();
			}
			this.groupCharaMapByChara[mstCharaGroupData.charaId].Add(mstCharaGroupData.groupcharaId);
			this.groupCharaMapByChara[mstCharaGroupData.groupcharaId].Add(mstCharaGroupData.charaId);
		}
		DataManager.DmItem.AddMstDataByItem(list2);
		this.InitializeStaticClothes(mst4);
		this.InitializeStaticContact(mst5);
		this.InitializeStaticMasterSkill(mstManager);
		this.InitializeStaticTacticsSkill();
		this.InitializeTacticsParam();
		this.bonusCharaDataList = new List<DataManagerChara.BonusCharaData>();
		foreach (MstEventBonusCharaData mstEventBonusCharaData in mst8)
		{
			this.bonusCharaDataList.Add(new DataManagerChara.BonusCharaData(mstEventBonusCharaData));
		}
		List<int> list4 = new List<int>();
		string text6 = "paramenemybase_";
		foreach (string text7 in this.loadAssetParameterList)
		{
			string[] allAssetNames = SGNFW.Ab.Manager.GetData(text7).asset.GetAllAssetNames();
			for (int j = 0; j < allAssetNames.Length; j++)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(allAssetNames[j]);
				if (fileNameWithoutExtension.StartsWith(text6))
				{
					string[] array = fileNameWithoutExtension.Split('_', StringSplitOptions.None);
					if (array.Length == 2)
					{
						list4.Add(int.Parse(array[1]));
					}
				}
			}
		}
		this.InitializeStaticEnemy(list4);
		using (Dictionary<int, EnemyStaticData>.Enumerator enumerator14 = this.enemyStaticMap.GetEnumerator())
		{
			while (enumerator14.MoveNext())
			{
				KeyValuePair<int, EnemyStaticData> enemy = enumerator14.Current;
				MstEnemyCharaData mstEnemyCharaData = mst11.Find((MstEnemyCharaData item) => item.enemyCharaId == enemy.Key);
				if (mstEnemyCharaData != null)
				{
					CharaDef.AttributeType attribute = enemy.Value.baseData.attribute;
					int attribute2 = mstEnemyCharaData.attribute;
				}
			}
		}
		this.FilterDataListMiracle = new List<DataManagerChara.FilterData>();
		using (List<MstCharaFilterData>.Enumerator enumerator15 = mst10.GetEnumerator())
		{
			while (enumerator15.MoveNext())
			{
				MstCharaFilterData charaFilterData2 = enumerator15.Current;
				if (!string.IsNullOrEmpty(charaFilterData2.labelTextMiracle) && charaFilterData2.hideFlagMiracle == 0)
				{
					DataManagerChara.FilterData filterData = this.FilterDataListMiracle.Find((DataManagerChara.FilterData x) => x.DisplayName1 == charaFilterData2.labelTextMiracle);
					if (filterData == null)
					{
						this.FilterDataListMiracle.Add(new DataManagerChara.FilterData(charaFilterData2));
					}
					else
					{
						filterData.AdditionalElement(charaFilterData2);
					}
				}
			}
		}
		this.FilterDataListMiracle.Sort((DataManagerChara.FilterData a, DataManagerChara.FilterData b) => a.Priority - b.Priority);
		this.FilterDataListCharacteristic = new List<DataManagerChara.FilterData>();
		using (List<MstCharaFilterData>.Enumerator enumerator15 = mst10.GetEnumerator())
		{
			while (enumerator15.MoveNext())
			{
				MstCharaFilterData charaFilterData = enumerator15.Current;
				if (!(string.Empty == charaFilterData.labelTextCharacteristic) && charaFilterData.hideFlagCharacteristic == 0)
				{
					DataManagerChara.FilterData filterData2 = this.FilterDataListCharacteristic.Find((DataManagerChara.FilterData x) => x.DisplayName2 == charaFilterData.labelTextCharacteristic);
					if (filterData2 == null)
					{
						this.FilterDataListCharacteristic.Add(new DataManagerChara.FilterData(charaFilterData));
					}
					else
					{
						filterData2.AdditionalElement(charaFilterData);
					}
				}
			}
		}
		this.FilterDataListCharacteristic.Sort((DataManagerChara.FilterData a, DataManagerChara.FilterData b) => a.Priority - b.Priority);
		yield break;
		yield break;
	}

	public void InitializeStaticClothes(List<MstCharaClothesData> mstClothesList)
	{
		List<ItemStaticBase> list = new List<ItemStaticBase>();
		List<CharaStaticData> list2 = new List<CharaStaticData>(this.charaStaticMap.Values);
		this.charaClothesStaticMap = new Dictionary<int, CharaClothStatic>();
		using (List<MstCharaClothesData>.Enumerator enumerator = mstClothesList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MstCharaClothesData mstClothes = enumerator.Current;
				CharaStaticData charaStaticData = list2.Find((CharaStaticData item) => item.baseData.clothesPresetId == mstClothes.clothesPresetId);
				CharaClothStatic charaClothStatic = new CharaClothStatic(mstClothes, (charaStaticData != null) ? charaStaticData.GetId() : 0);
				this.charaClothesStaticMap.Add(mstClothes.id, charaClothStatic);
				list.Add(charaClothStatic);
				DataManager.DmItem.AddMstDataByItemCharaMap(mstClothes.id, charaClothStatic.CharaId);
			}
		}
		DataManager.DmItem.AddMstDataByItem(list);
	}

	public void InitializeStaticContact(List<MstCharaMotionItemData> mstContactList)
	{
		List<ItemStaticBase> list = new List<ItemStaticBase>();
		new List<CharaStaticData>(this.charaStaticMap.Values);
		this.charaContactStaticMap = new Dictionary<int, CharaContactStatic>();
		this.charaContactStaticMapByChara = new Dictionary<int, List<CharaContactStatic>>();
		foreach (MstCharaMotionItemData mstCharaMotionItemData in mstContactList)
		{
			CharaContactStatic charaContactStatic = new CharaContactStatic(mstCharaMotionItemData);
			this.charaContactStaticMap.Add(mstCharaMotionItemData.id, charaContactStatic);
			CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(mstCharaMotionItemData.charaId);
			if (charaStaticData != null)
			{
				List<int> list2 = new List<int> { charaStaticData.baseData.id };
				if (charaStaticData.baseData.OriginalId > 0)
				{
					list2.Add(charaStaticData.baseData.OriginalId);
				}
				list2.AddRange(charaStaticData.baseData.SynonymIdSet);
				foreach (int num in list2)
				{
					if (!this.charaContactStaticMapByChara.ContainsKey(num))
					{
						this.charaContactStaticMapByChara.Add(num, new List<CharaContactStatic>());
					}
					this.charaContactStaticMapByChara[num].Add(charaContactStatic);
				}
			}
			list.Add(charaContactStatic);
			DataManager.DmItem.AddMstDataByItemCharaMap(mstCharaMotionItemData.id, charaContactStatic.CharaId);
		}
		using (Dictionary<int, List<CharaContactStatic>>.ValueCollection.Enumerator enumerator3 = this.charaContactStaticMapByChara.Values.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				enumerator3.Current.Sort((CharaContactStatic a, CharaContactStatic b) => a.SortPriority.CompareTo(b.SortPriority));
			}
		}
		DataManager.DmItem.AddMstDataByItem(list);
	}

	public void InitializeStaticEnemy(List<int> idList)
	{
		this.enemyStaticMap = new Dictionary<int, EnemyStaticData>();
		Dictionary<int, EnemyStaticBase> dictionary = new Dictionary<int, EnemyStaticBase>();
		Dictionary<int, List<EnemyStaticBase>> dictionary2 = new Dictionary<int, List<EnemyStaticBase>>();
		Dictionary<string, CharaStaticAction> dictionary3 = new Dictionary<string, CharaStaticAction>();
		Dictionary<string, CharaStaticAction> dictionary4 = new Dictionary<string, CharaStaticAction>();
		foreach (int num in idList)
		{
			EnemyStaticData enemyStaticData = new EnemyStaticData();
			int num2 = num;
			if (!dictionary.ContainsKey(num2))
			{
				EnemyStaticBase enemyStaticBase = AssetManager.GetAssetData("Charas/Parameter/EnemyBase/ParamEnemyBase_" + num2.ToString("00000")) as EnemyStaticBase;
				if (enemyStaticBase == null)
				{
					continue;
				}
				enemyStaticBase.id = num;
				if (enemyStaticBase.abilityFileName != null && enemyStaticBase.abilityFileName != string.Empty)
				{
					string text = "Charas/Parameter/Ability/" + enemyStaticBase.abilityFileName;
					enemyStaticBase.abilityData = AssetManager.GetAssetData(text) as CharaStaticAbility;
					if (enemyStaticBase.abilityData == null)
					{
						Verbose<PrjLog>.LogError("Error : DataManagerChara.InitializeStaticEnemy : Asset Dataが存在してません：" + text + "：プランナに連絡してください", null);
					}
				}
				dictionary.Add(num2, enemyStaticBase);
			}
			enemyStaticData.baseData = dictionary[num2];
			int num3 = num;
			if (!dictionary2.ContainsKey(num3))
			{
				List<EnemyStaticBase> list = new List<EnemyStaticBase>();
				string text2 = "Charas/Parameter/EnemyBase/ParamEnemyBase_" + num3.ToString("00000") + "_";
				int num4 = 1;
				for (;;)
				{
					EnemyStaticBase enemyStaticBase2 = AssetManager.GetAssetData(text2 + num4.ToString()) as EnemyStaticBase;
					if (enemyStaticBase2 == null)
					{
						break;
					}
					enemyStaticBase2.id = num;
					if (enemyStaticBase2.abilityFileName != null && enemyStaticBase2.abilityFileName != string.Empty)
					{
						string text3 = "Charas/Parameter/Ability/" + enemyStaticBase2.abilityFileName;
						enemyStaticBase2.abilityData = AssetManager.GetAssetData(text3) as CharaStaticAbility;
						if (enemyStaticBase2.abilityData == null)
						{
							Verbose<PrjLog>.LogError("Error : DataManagerChara.InitializeStaticEnemy : Asset Dataが存在してません：" + text3 + "：プランナに連絡してください", null);
						}
					}
					list.Add(enemyStaticBase2);
					num4++;
				}
				dictionary2.Add(num3, list);
			}
			enemyStaticData.partsData = dictionary2[num3];
			if (enemyStaticData.baseData.artsParamId != string.Empty)
			{
				string artsParamId = enemyStaticData.baseData.artsParamId;
				if (!dictionary3.ContainsKey(artsParamId))
				{
					string text4 = "Charas/Parameter/EnemyArts/" + artsParamId;
					CharaStaticAction charaStaticAction = AssetManager.GetAssetData(text4) as CharaStaticAction;
					if (charaStaticAction == null)
					{
						Verbose<PrjLog>.LogError("Error : DataManagerChara.InitializeStatic : Asset Dataが存在してません：" + text4 + "：プランナに連絡してください", null);
					}
					dictionary3.Add(artsParamId, charaStaticAction);
				}
				enemyStaticData.artsData = dictionary3[artsParamId];
			}
			enemyStaticData.normalAttackData = new EnemyAttackData
			{
				actionPattern = enemyStaticData.baseData.actionPattern
			};
			foreach (EnemyStaticBase.ActionParam actionParam in enemyStaticData.baseData.actionParamList)
			{
				string attackParamId = actionParam.attackParamId;
				if (!dictionary4.ContainsKey(attackParamId))
				{
					string text5 = "Charas/Parameter/EnemyAttack/" + attackParamId;
					CharaStaticAction charaStaticAction2 = AssetManager.GetAssetData(text5) as CharaStaticAction;
					if (charaStaticAction2 == null)
					{
						Verbose<PrjLog>.LogError("Error : DataManagerChara.InitializeStatic : Asset Dataが存在してません：" + text5 + "：プランナに連絡してください", null);
					}
					dictionary4.Add(attackParamId, charaStaticAction2);
				}
				EnemyAttackData.Param param = new EnemyAttackData.Param
				{
					point = actionParam.actionPoint,
					param = dictionary4[attackParamId],
					death = this.GetPartsList(actionParam.death),
					alive = this.GetPartsList(actionParam.alive)
				};
				enemyStaticData.normalAttackData.attackList.Add(param);
			}
			this.enemyStaticMap.Add(num, enemyStaticData);
			this.charaId2modelId.Add(num, (enemyStaticData.baseData.modelId == 0) ? num : enemyStaticData.baseData.modelId);
		}
	}

	private List<int> GetPartsList(string str)
	{
		List<int> list = new List<int>();
		if (!string.IsNullOrEmpty(str))
		{
			for (int i = 0; i < str.Length; i++)
			{
				int num = (int)(str[i] - '0');
				if (num > 0 && num <= 9)
				{
					list.Add(num);
				}
			}
		}
		return list;
	}

	private void InitializeStaticMasterSkill(MstManager mstManager)
	{
		List<MstItemCommon> mst = mstManager.GetMst<List<MstItemCommon>>(MstType.ITEM_COMMON);
		this.masterSkillStaticMap.Clear();
		foreach (MstItemCommon mstItemCommon in mst)
		{
			if (ItemDef.Id2Kind(mstItemCommon.id) == ItemDef.Kind.MASTER_SKILL)
			{
				int id = mstItemCommon.id;
				string text = "Charas/Parameter/MasterSkill/ParamMasterSkill_" + id.ToString("00000");
				MasterStaticSkill masterStaticSkill = AssetManager.GetAssetData(text) as MasterStaticSkill;
				if (masterStaticSkill == null)
				{
					Verbose<PrjLog>.LogError("Error : DataManagerChara.InitializeStaticMasterSkill : Asset Dataが存在してません：" + text + "：プランナに連絡してください", null);
				}
				else
				{
					masterStaticSkill.maxLevel = 0;
				}
				this.masterSkillStaticMap.Add(mstItemCommon.id, masterStaticSkill);
			}
		}
	}

	private void InitializeStaticTacticsSkill()
	{
		this.tacticsSkillStaticMap.Clear();
		int num = 1;
		for (;;)
		{
			TacticsStaticSkill tacticsStaticSkill = AssetManager.GetAssetData("Charas/Parameter/TacticsSkill/ParamTacticsSkill_" + num.ToString("00")) as TacticsStaticSkill;
			if (tacticsStaticSkill == null)
			{
				break;
			}
			this.tacticsSkillStaticMap.Add(num, tacticsStaticSkill);
			num++;
		}
	}

	private void InitializeTacticsParam()
	{
		this.tacticsParam = AssetManager.GetAssetData("Charas/Parameter/TacticsParam") as TacticsParam;
	}

	public long GetExpByNextLevel(int charaId, int nowLevel)
	{
		int num = nowLevel + 1 - 1;
		int levelTableId = this.GetCharaStaticData(charaId).baseData.levelTableId;
		long num2 = 0L;
		if (num < DataManager.DmServerMst.gameLevelInfoList.Count)
		{
			GameLevelInfo gameLevelInfo = DataManager.DmServerMst.gameLevelInfoList[num];
			num2 = (gameLevelInfo.charaLevelExp.ContainsKey(levelTableId) ? gameLevelInfo.charaLevelExp[levelTableId] : 0L);
		}
		return num2;
	}

	public long GetExpByNextKizunaLevel(int charaId, int nowLevel)
	{
		int num = nowLevel + 1 - 1;
		int kizunaTableId = this.GetCharaStaticData(charaId).baseData.kizunaTableId;
		long num2 = 0L;
		if (num < DataManager.DmServerMst.gameLevelInfoList.Count)
		{
			GameLevelInfo gameLevelInfo = DataManager.DmServerMst.gameLevelInfoList[num];
			num2 = (gameLevelInfo.kizunaLevelExp.ContainsKey(kizunaTableId) ? gameLevelInfo.kizunaLevelExp[kizunaTableId].LevelExp : 0L);
		}
		return num2;
	}

	public long GetNeedExpByLimitLevel(int charaId)
	{
		int limitLevel = DataManager.DmChara.GetUserCharaData(charaId).dynamicData.limitLevel;
		return this.GetNeedExpByRangeLevel(charaId, limitLevel);
	}

	public long GetNeedExpByRangeLevel(int charaId, int endLevel)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		int level = userCharaData.dynamicData.level;
		long exp = userCharaData.dynamicData.exp;
		int levelTableId = DataManager.DmChara.GetCharaStaticData(charaId).baseData.levelTableId;
		long num = 0L;
		if (level >= DataManager.DmServerMst.gameLevelInfoList.Count)
		{
			return 0L;
		}
		if (endLevel >= DataManager.DmServerMst.gameLevelInfoList.Count)
		{
			return 0L;
		}
		if (level >= endLevel)
		{
			return 0L;
		}
		for (int i = level + 1 - 1; i < endLevel; i++)
		{
			GameLevelInfo gameLevelInfo = DataManager.DmServerMst.gameLevelInfoList[i];
			num += gameLevelInfo.charaLevelExp[levelTableId];
		}
		return num - exp;
	}

	public long GetNeedExpFromFirstLevel(int charaId, int endLevel)
	{
		int num = endLevel - 1;
		int levelTableId = this.GetCharaStaticData(charaId).baseData.levelTableId;
		long num2 = 0L;
		for (int i = 0; i <= num; i++)
		{
			GameLevelInfo gameLevelInfo = DataManager.DmServerMst.gameLevelInfoList[i];
			num2 += (gameLevelInfo.charaLevelExp.ContainsKey(levelTableId) ? gameLevelInfo.charaLevelExp[levelTableId] : 0L);
		}
		return num2;
	}

	public long GetKizunaExpForNextLevel(int charaId, int nowLevel)
	{
		int num = nowLevel + 1 - 1;
		int kizunaTableId = this.GetCharaStaticData(charaId).baseData.kizunaTableId;
		long num2 = 0L;
		if (num < DataManager.DmServerMst.gameLevelInfoList.Count)
		{
			GameLevelInfo gameLevelInfo = DataManager.DmServerMst.gameLevelInfoList[num];
			if (gameLevelInfo.kizunaLevelExp.ContainsKey(kizunaTableId))
			{
				num2 = gameLevelInfo.kizunaLevelExp[kizunaTableId].LevelExp;
			}
		}
		return num2;
	}

	public long GetNeedKizunaExpFromFirstLevel(int charaId, int endLevel)
	{
		int num = endLevel - 1;
		int kizunaTableId = this.GetCharaStaticData(charaId).baseData.kizunaTableId;
		long num2 = 0L;
		for (int i = 0; i <= num; i++)
		{
			GameLevelInfo gameLevelInfo = DataManager.DmServerMst.gameLevelInfoList[i];
			num2 += (gameLevelInfo.kizunaLevelExp.ContainsKey(kizunaTableId) ? gameLevelInfo.kizunaLevelExp[kizunaTableId].LevelExp : 0L);
		}
		return num2;
	}

	public long GetNeedExpByKizunaLimitLevel(int charaId)
	{
		int kizunaLimitLevel = DataManager.DmChara.GetUserCharaData(charaId).dynamicData.KizunaLimitLevel;
		return this.GetNeedExpByRangeKizunaLevel(charaId, kizunaLimitLevel);
	}

	public long GetNeedExpByRangeKizunaLevel(int charaId, int endLevel)
	{
		CharaPackData userCharaData = this.GetUserCharaData(charaId);
		int kizunaLevel = userCharaData.dynamicData.kizunaLevel;
		long kizunaExp = userCharaData.dynamicData.kizunaExp;
		long num = 0L;
		int kizunaLevelId = userCharaData.staticData.baseData.kizunaLevelId;
		int count = DataManager.DmServerMst.gameLevelInfoList.Count;
		if (kizunaLevel >= count)
		{
			return 0L;
		}
		if (endLevel >= count)
		{
			return 0L;
		}
		if (kizunaLevel >= endLevel)
		{
			return 0L;
		}
		for (int i = kizunaLevel + 1 - 1; i < endLevel; i++)
		{
			GameLevelInfo gameLevelInfo = DataManager.DmServerMst.gameLevelInfoList[i];
			num += gameLevelInfo.kizunaLevelExp[kizunaLevelId].LevelExp;
		}
		return num - kizunaExp;
	}

	public List<CharaClothStatic> GetClothListByChara(int charaId)
	{
		return new List<CharaClothStatic>(this.charaClothesStaticMap.Values).FindAll((CharaClothStatic item) => item.CharaId == charaId);
	}

	public int GetRankupRewardItem(int charaId, int rank)
	{
		CharaClothStatic charaClothStatic = new List<CharaClothStatic>(this.charaClothesStaticMap.Values).Find((CharaClothStatic item) => item.CharaId == charaId && item.GetRank == rank);
		if (charaClothStatic == null)
		{
			return 0;
		}
		return charaClothStatic.GetId();
	}

	public List<ItemInput> RankUpResultKemoBoardItem { get; set; }

	public CharaGrowMultiRequest CharaGrowMultiRequest { get; set; }

	public DataManagerChara.CharaLevelupResult GetCharaLevelupResult()
	{
		return this.charaLevelupResult;
	}

	public DataManagerChara.CharaLevelupResult GetCharaKizunaLevelupResult()
	{
		return this.charaKizunaLevelupResult;
	}

	public DataManagerChara.SimulateAddExpResult SimulateAddExp(CharaPackData charaPack, List<ItemInput> addItemList)
	{
		DataManagerChara.SimulateAddExpResult simulateAddExpResult = new DataManagerChara.SimulateAddExpResult
		{
			level = charaPack.dynamicData.level,
			exp = charaPack.dynamicData.exp
		};
		if (addItemList != null)
		{
			foreach (ItemInput itemInput in addItemList)
			{
				ItemDef.AddCharaLevelExp addCharaLevelExpBase = ItemDef.GetAddCharaLevelExpBase(itemInput.itemId, charaPack.staticData.baseData.attribute);
				long num = addCharaLevelExpBase.addExp;
				num *= (long)itemInput.num;
				simulateAddExpResult.exp += num;
				simulateAddExpResult.costGold += addCharaLevelExpBase.needGold * itemInput.num;
			}
		}
		for (;;)
		{
			long expByNextLevel = this.GetExpByNextLevel(charaPack.id, simulateAddExpResult.level);
			if (expByNextLevel <= 0L || simulateAddExpResult.exp < expByNextLevel)
			{
				break;
			}
			simulateAddExpResult.exp -= expByNextLevel;
			simulateAddExpResult.level++;
		}
		simulateAddExpResult.maxExp = this.GetExpByNextLevel(charaPack.id, simulateAddExpResult.level);
		return simulateAddExpResult;
	}

	public DataManagerChara.SimulateAddExpResult SimulateAddKizunaExp(CharaPackData charaPack, List<ItemInput> addItemList)
	{
		DataManagerChara.SimulateAddExpResult simulateAddExpResult = new DataManagerChara.SimulateAddExpResult
		{
			level = charaPack.dynamicData.kizunaLevel,
			exp = charaPack.dynamicData.kizunaExp
		};
		if (addItemList != null)
		{
			foreach (ItemInput itemInput in addItemList)
			{
				ItemDef.AddCharaLevelExp addCharaLevelExpBase = ItemDef.GetAddCharaLevelExpBase(itemInput.itemId, charaPack.staticData.baseData.attribute);
				long num = addCharaLevelExpBase.addExp;
				num *= (long)itemInput.num;
				simulateAddExpResult.exp += num;
				simulateAddExpResult.costGold += addCharaLevelExpBase.needGold * itemInput.num;
			}
		}
		for (;;)
		{
			long expByNextKizunaLevel = this.GetExpByNextKizunaLevel(charaPack.id, simulateAddExpResult.level);
			if (expByNextKizunaLevel <= 0L || simulateAddExpResult.exp < expByNextKizunaLevel)
			{
				break;
			}
			simulateAddExpResult.exp -= expByNextKizunaLevel;
			simulateAddExpResult.level++;
		}
		simulateAddExpResult.maxExp = this.GetExpByNextKizunaLevel(charaPack.id, simulateAddExpResult.level);
		return simulateAddExpResult;
	}

	public void Destory()
	{
		foreach (string text in this.loadAssetParameterList)
		{
			AssetManager.UnloadAssetData(AssetManager.PREFIX_PATH_CHARA_PARAM + text, AssetManager.OWNER.DataManagerChara);
		}
	}

	public void RequestActionCharaGrowMulti()
	{
		this.parentData.ServerRequest(CharaGrowMultiCmd.Create(this.CharaGrowMultiRequest), new Action<Command>(this.CbCharaGrowMultiCmd));
	}

	public void RequestActionCharaLevelup(int charaId, List<ItemInput> useItemList)
	{
		this.charaLevelupResult = new DataManagerChara.CharaLevelupResult();
		CharaPackData userCharaData = this.GetUserCharaData(charaId);
		this.charaLevelupResult.befExp = userCharaData.dynamicData.exp;
		this.charaLevelupResult.befLevel = userCharaData.dynamicData.level;
		List<DataManagerServerMst.CharaLevelItem> list = userCharaData.LevelItemUseOrderList(false);
		List<UseItem> list2 = new List<UseItem>();
		using (List<DataManagerServerMst.CharaLevelItem>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DataManagerServerMst.CharaLevelItem levelItemUseOrder = enumerator.Current;
				ItemInput itemInput = useItemList.Find((ItemInput x) => x.itemId == levelItemUseOrder.itemId);
				if (itemInput != null)
				{
					list2.Add(new UseItem
					{
						use_item_id = itemInput.itemId,
						use_item_num = itemInput.num
					});
				}
			}
		}
		if (this.CharaGrowMultiRequest == null)
		{
			this.parentData.ServerRequest(CharaLevelUpCmd.Create(charaId, list2), new Action<Command>(this.CbCharaLevelUpCmd));
			return;
		}
		this.CharaGrowMultiRequest.chara_level_up_request = new CharaLevelUpRequest
		{
			chara_id = charaId,
			use_items = list2
		};
	}

	public void RequestActionCharaKizunaLevelup(int charaId, List<ItemInput> useItemList)
	{
		this.charaKizunaLevelupResult = new DataManagerChara.CharaLevelupResult();
		CharaPackData userCharaData = this.GetUserCharaData(charaId);
		this.charaKizunaLevelupResult.befExp = userCharaData.dynamicData.kizunaExp;
		this.charaKizunaLevelupResult.befLevel = userCharaData.dynamicData.kizunaLevel;
		List<DataManagerServerMst.CharaLevelItem> list = userCharaData.LevelItemUseOrderList(true);
		List<UseItem> list2 = new List<UseItem>();
		using (List<DataManagerServerMst.CharaLevelItem>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DataManagerServerMst.CharaLevelItem levelItemUseOrder = enumerator.Current;
				ItemInput itemInput = useItemList.Find((ItemInput x) => x.itemId == levelItemUseOrder.itemId);
				if (itemInput != null)
				{
					list2.Add(new UseItem
					{
						use_item_id = itemInput.itemId,
						use_item_num = itemInput.num
					});
				}
			}
		}
		this.parentData.ServerRequest(CharaKizunaLevelUpCmd.Create(charaId, list2), new Action<Command>(this.CbCharaKizunaLevelUpCmd));
	}

	public void RequestActionCharaPromote(int charaId, List<WildResult> promoteRequest, bool isPromoteStepUp)
	{
		if (this.CharaGrowMultiRequest == null)
		{
			this.parentData.ServerRequest(CharaWildRelCmd.Create(charaId, promoteRequest, isPromoteStepUp ? 1 : 0), new Action<Command>(this.CbCharaWildRelCmd));
			return;
		}
		this.CharaGrowMultiRequest.chara_wild_rel_request = new CharaWildRelRequest
		{
			chara_id = charaId,
			promote_request = promoteRequest,
			is_promoteup_action = (isPromoteStepUp ? 1 : 0)
		};
	}

	public void RequestActionCharaNanairo(int charaId)
	{
		if (this.CharaGrowMultiRequest == null)
		{
			this.parentData.ServerRequest(CharaNanairoAbilityReleaseCmd.Create(charaId), new Action<Command>(this.CbCharaNanairoReleaseCmd));
			return;
		}
		this.CharaGrowMultiRequest.chara_nanairo_ability_release_request = new CharaNanairoAbilityReleaseRequest
		{
			chara_id = charaId
		};
	}

	public void RequestActionCharaRankup(int charaId, int targetRank)
	{
		this.RankUpResultKemoBoardItem = new List<ItemInput>();
		if (this.CharaGrowMultiRequest == null)
		{
			this.parentData.ServerRequest(CharaRankUpCmd.Create(charaId, targetRank), new Action<Command>(this.CbCharaRankUpCmd));
			return;
		}
		this.CharaGrowMultiRequest.chara_rank_up_request = new CharaRankUpRequest
		{
			chara_id = charaId,
			target_rank = targetRank
		};
	}

	public void RequestActionCharaReleasePhotoFrame(int charaId, int targetPhotoPocketStep)
	{
		this.parentData.ServerRequest(CharaPpRelCmd.Create(charaId, targetPhotoPocketStep), new Action<Command>(this.CbCharaPpRelCmd));
	}

	public void RequestActionCharaArtsUp(int charaId, int targetLevel)
	{
		if (this.CharaGrowMultiRequest == null)
		{
			this.parentData.ServerRequest(CharaArtsUpCmd.Create(charaId, targetLevel), new Action<Command>(this.CbCharaArtsUpCmd));
			return;
		}
		this.CharaGrowMultiRequest.chara_arts_up_request = new CharaArtsUpRequest
		{
			chara_id = charaId,
			target_arts_level = targetLevel
		};
	}

	public void RequestActoinCharaChangeClothes(int charaId, int clothId)
	{
		this.parentData.ServerRequest(CharaChangeClothesCmd.Create(charaId, clothId), new Action<Command>(this.CbCharaChangeClothesCmd));
	}

	public void RequestActoinCharaChangeIcon(int charaId, int iconId)
	{
		this.parentData.ServerRequest(CharaSelectFaceIconCmd.Create(charaId, iconId), new Action<Command>(this.CbCharaChangeIconCmd));
	}

	public void RequestActoinCharaLimitLevelUp(int charaId, int targetLevelLimitId)
	{
		if (this.CharaGrowMultiRequest == null)
		{
			this.parentData.ServerRequest(CharaLimitLevelUpCmd.Create(charaId, targetLevelLimitId), new Action<Command>(this.CbCharaLimitLevelUpCmd));
			return;
		}
		this.CharaGrowMultiRequest.chara_limit_level_up_request = new CharaLimitLevelUpRequest
		{
			chara_id = charaId,
			target_level_limit_id = targetLevelLimitId
		};
	}

	public void RequestActoinCharaKizunaLimitLevelUp(int charaId)
	{
		if (this.CharaGrowMultiRequest == null)
		{
			this.parentData.ServerRequest(CharaKizunaLimitLevelUpCmd.Create(charaId), new Action<Command>(this.CbCharaKizunaLimitLevelUpCmd));
			return;
		}
		this.CharaGrowMultiRequest.chara_kizuna_limit_level_up_request = new CharaKizunaLimitLevelUpRequest
		{
			chara_id = charaId
		};
	}

	public void RequestActoinCharaAccessoryOpen(int charaId)
	{
		this.parentData.ServerRequest(CharaAccessoryOpenCmd.Create(charaId), new Action<Command>(this.CbCharaAccessoryOpenCmd));
	}

	public void RequestActoinCharaAccessoryEquip(int charaId, DataManagerCharaAccessory.Accessory acc)
	{
		long num = ((acc == null) ? 0L : acc.UniqId);
		List<EquipAccessory> list = new List<EquipAccessory>();
		list.Add(new EquipAccessory
		{
			chara_id = charaId,
			accessory_id = num
		});
		this.parentData.ServerRequest(CharaAccessoryEquipCmd.Create(list), new Action<Command>(this.CbCharaAccessoryEquipCmd));
	}

	public void RequestActoinCharaAccessoryEffectStatus(int charaId, bool effectDisp)
	{
		int num = (effectDisp ? 0 : 1);
		this.parentData.ServerRequest(CharaAccessoryEffectStatusCmd.Create(charaId, num), new Action<Command>(this.CharaAccessoryEffectStatus));
	}

	public void RequestActtionCharaFavoriteFlag(int charaId)
	{
		this.parentData.ServerRequest(CharaFavoriteFlagCmd.Create(charaId), new Action<Command>(this.CharaFavoriteFlag));
	}

	public void RequestUpdateTotalKemoStatus()
	{
		if (this.IsNeedUpdateByUserAllCharaKemoStatusList)
		{
			this.IsNeedUpdateByUserAllCharaKemoStatusList = false;
			this.parentData.ServerRequest(UpdateTotalKemoStatusCmd.Create(this.UserAllCharaKemoStatus), new Action<Command>(this.CbUpdateTotalKemoStatusCmd));
		}
	}

	public void RequestCharaTouchCount(int charaId, int count)
	{
		this.parentData.ServerRequest(CharaTouchCountCmd.Create(charaId, count), new Action<Command>(this.CbCharaTouchCmd));
	}

	private void CbCharaGrowMultiCmd(Command cmd)
	{
		CharaGrowMultiResponse charaGrowMultiResponse = cmd.response as CharaGrowMultiResponse;
		if (charaGrowMultiResponse.rankup_result != null)
		{
			this.UpdateRankUpResultKemoBoardItem(charaGrowMultiResponse.assets.update_item_list);
		}
		this.parentData.UpdateUserAssetByAssets(charaGrowMultiResponse.assets);
		if (charaGrowMultiResponse.level_result != null)
		{
			this.UpdateCharaLevelupResult(charaGrowMultiResponse.level_result);
		}
		this.CharaGrowMultiRequest = null;
	}

	private void CbCharaLevelUpCmd(Command cmd)
	{
		CharaLevelUpResponse charaLevelUpResponse = cmd.response as CharaLevelUpResponse;
		this.parentData.UpdateUserAssetByAssets(charaLevelUpResponse.assets);
		this.UpdateCharaLevelupResult(charaLevelUpResponse.level_result);
	}

	private void CbCharaKizunaLevelUpCmd(Command cmd)
	{
		CharaKizunaLevelUpResponse charaKizunaLevelUpResponse = cmd.response as CharaKizunaLevelUpResponse;
		this.parentData.UpdateUserAssetByAssets(charaKizunaLevelUpResponse.assets);
		this.UpdateCharaKizunaLevelupResult(charaKizunaLevelUpResponse.level_result);
	}

	private void UpdateCharaLevelupResult(LevelResult level_result)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(level_result.chara_id);
		this.charaLevelupResult.charaId = userCharaData.id;
		this.charaLevelupResult.exp = userCharaData.dynamicData.exp;
		this.charaLevelupResult.level = userCharaData.dynamicData.level;
		this.charaLevelupResult.returnItem = level_result.returnitem != 0;
		this.charaLevelupResult.successStatus = (DataManagerChara.CharaLevelupResult.Status)level_result.success_status;
	}

	private void UpdateCharaKizunaLevelupResult(KizunaLevelResult level_result)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(level_result.chara_id);
		this.charaKizunaLevelupResult.charaId = userCharaData.id;
		this.charaKizunaLevelupResult.exp = userCharaData.dynamicData.kizunaExp;
		this.charaKizunaLevelupResult.level = userCharaData.dynamicData.kizunaLevel;
		this.charaKizunaLevelupResult.returnItem = level_result.return_item != 0;
		this.charaKizunaLevelupResult.successStatus = (DataManagerChara.CharaLevelupResult.Status)level_result.success_status;
	}

	private void CbCharaWildRelCmd(Command cmd)
	{
		CharaWildRelResponse charaWildRelResponse = cmd.response as CharaWildRelResponse;
		this.parentData.UpdateUserAssetByAssets(charaWildRelResponse.assets);
	}

	private void CbCharaNanairoReleaseCmd(Command cmd)
	{
		CharaNanairoAbilityReleaseResponse charaNanairoAbilityReleaseResponse = cmd.response as CharaNanairoAbilityReleaseResponse;
		this.parentData.UpdateUserAssetByAssets(charaNanairoAbilityReleaseResponse.assets);
	}

	private void CbCharaRankUpCmd(Command cmd)
	{
		CharaRankUpResponse charaRankUpResponse = cmd.response as CharaRankUpResponse;
		this.UpdateRankUpResultKemoBoardItem(charaRankUpResponse.assets.update_item_list);
		this.parentData.UpdateUserAssetByAssets(charaRankUpResponse.assets);
	}

	private void UpdateRankUpResultKemoBoardItem(List<Item> update_item_list)
	{
		List<ItemInput> list = new List<ItemInput>();
		foreach (Item item in update_item_list)
		{
			ItemData userItemData = DataManager.DmItem.GetUserItemData(item.item_id);
			if (userItemData.staticData.GetKind() == ItemDef.Kind.KEMOBOARD)
			{
				list.Add(new ItemInput(userItemData.id, item.item_num - userItemData.num));
			}
		}
		this.RankUpResultKemoBoardItem = list;
	}

	private void CbCharaPpRelCmd(Command cmd)
	{
		CharaPpRelResponse charaPpRelResponse = cmd.response as CharaPpRelResponse;
		this.parentData.UpdateUserAssetByAssets(charaPpRelResponse.assets);
	}

	private void CbCharaArtsUpCmd(Command cmd)
	{
		CharaArtsUpResponse charaArtsUpResponse = cmd.response as CharaArtsUpResponse;
		this.parentData.UpdateUserAssetByAssets(charaArtsUpResponse.assets);
	}

	private void CbCharaChangeClothesCmd(Command cmd)
	{
		CharaChangeClothesResponse charaChangeClothesResponse = cmd.response as CharaChangeClothesResponse;
		this.parentData.UpdateUserAssetByAssets(charaChangeClothesResponse.assets);
	}

	private void CbCharaChangeIconCmd(Command cmd)
	{
		CharaSelectFaceIconResponse charaSelectFaceIconResponse = cmd.response as CharaSelectFaceIconResponse;
		this.parentData.UpdateUserAssetByAssets(charaSelectFaceIconResponse.assets);
	}

	private void CbCharaLimitLevelUpCmd(Command cmd)
	{
		CharaLimitLevelUpResponse charaLimitLevelUpResponse = cmd.response as CharaLimitLevelUpResponse;
		this.parentData.UpdateUserAssetByAssets(charaLimitLevelUpResponse.assets);
	}

	private void CbCharaKizunaLimitLevelUpCmd(Command cmd)
	{
		CharaKizunaLimitLevelUpResponse charaKizunaLimitLevelUpResponse = cmd.response as CharaKizunaLimitLevelUpResponse;
		this.parentData.UpdateUserAssetByAssets(charaKizunaLimitLevelUpResponse.assets);
	}

	private void CbCharaAccessoryOpenCmd(Command cmd)
	{
		CharaAccessoryOpenResponse charaAccessoryOpenResponse = cmd.response as CharaAccessoryOpenResponse;
		this.parentData.UpdateUserAssetByAssets(charaAccessoryOpenResponse.assets);
	}

	private void CbCharaAccessoryEquipCmd(Command cmd)
	{
		CharaAccessoryEquipResponse charaAccessoryEquipResponse = cmd.response as CharaAccessoryEquipResponse;
		this.parentData.UpdateUserAssetByAssets(charaAccessoryEquipResponse.assets);
	}

	private void CharaAccessoryEffectStatus(Command cmd)
	{
		CharaAccessoryEffectStatusResponse charaAccessoryEffectStatusResponse = cmd.response as CharaAccessoryEffectStatusResponse;
		this.parentData.UpdateUserAssetByAssets(charaAccessoryEffectStatusResponse.assets);
	}

	private void CharaFavoriteFlag(Command cmd)
	{
		CharaFavoriteFlagResponse charaFavoriteFlagResponse = cmd.response as CharaFavoriteFlagResponse;
		this.parentData.UpdateUserAssetByAssets(charaFavoriteFlagResponse.assets);
	}

	private void CbUpdateTotalKemoStatusCmd(Command cmd)
	{
		UpdateTotalKemoStatusResponse updateTotalKemoStatusResponse = cmd.response as UpdateTotalKemoStatusResponse;
		this.parentData.UpdateUserAssetByAssets(updateTotalKemoStatusResponse.assets);
	}

	private void CbCharaTouchCmd(Command cmd)
	{
		CharaTouchCountResponse charaTouchCountResponse = cmd.response as CharaTouchCountResponse;
		this.parentData.UpdateUserAssetByAssets(charaTouchCountResponse.assets);
	}

	public List<DataManagerPhoto.CalcDropBonusResult> CalcDropBonus(List<DataManagerPhoto.CalcDropBonusResult> result, List<DataManagerChara.BonusCharaData> dropBonusCharaList, List<int> pocketReleaseCountList)
	{
		return this.InternalCalcDropBonus(result, dropBonusCharaList, pocketReleaseCountList);
	}

	private List<DataManagerPhoto.CalcDropBonusResult> InternalCalcDropBonus(List<DataManagerPhoto.CalcDropBonusResult> result, List<DataManagerChara.BonusCharaData> dropBonusCharaList, List<int> pocketReleaseCountList)
	{
		for (int i = 0; i < dropBonusCharaList.Count; i++)
		{
			DataManagerChara.BonusCharaData bonusChara = dropBonusCharaList[i];
			int num = pocketReleaseCountList[i];
			if (bonusChara.increaseItemId01 != 0)
			{
				DataManagerPhoto.CalcDropBonusResult calcDropBonusResult = result.Find((DataManagerPhoto.CalcDropBonusResult item) => item.targetItemId == bonusChara.increaseItemId01);
				if (calcDropBonusResult == null)
				{
					calcDropBonusResult = new DataManagerPhoto.CalcDropBonusResult();
					calcDropBonusResult.targetItemId = bonusChara.increaseItemId01;
					calcDropBonusResult.targetItemBonusRatio = bonusChara.dropBonusRatio01;
					result.Add(calcDropBonusResult);
				}
				calcDropBonusResult.ratio += bonusChara.dropBonusRatio01 * num;
			}
			if (bonusChara.increaseItemId02 != 0)
			{
				DataManagerPhoto.CalcDropBonusResult calcDropBonusResult2 = result.Find((DataManagerPhoto.CalcDropBonusResult item) => item.targetItemId == bonusChara.increaseItemId02);
				if (calcDropBonusResult2 == null)
				{
					calcDropBonusResult2 = new DataManagerPhoto.CalcDropBonusResult();
					calcDropBonusResult2.targetItemId = bonusChara.increaseItemId02;
					calcDropBonusResult2.targetItemBonusRatio = bonusChara.dropBonusRatio02;
					result.Add(calcDropBonusResult2);
				}
				calcDropBonusResult2.ratio += bonusChara.dropBonusRatio02 * num;
			}
		}
		result.Sort((DataManagerPhoto.CalcDropBonusResult a, DataManagerPhoto.CalcDropBonusResult b) => b.targetItemId - a.targetItemId);
		return result;
	}

	private DataManager parentData;

	private Dictionary<int, CharaPackData> userCharaMap = new Dictionary<int, CharaPackData>();

	private Dictionary<int, MasterSkillPackData> userMasterSkillMap = new Dictionary<int, MasterSkillPackData>();

	private List<CharaVoiceCombi> charaVoiceCombiList = new List<CharaVoiceCombi>();

	private List<DataManagerChara.LevelLimitData> levelLimitDataList = new List<DataManagerChara.LevelLimitData>();

	private List<DataManagerChara.LevelLimitRisingStatus> levelLimitRisingStatusList = new List<DataManagerChara.LevelLimitRisingStatus>();

	private List<string> loadAssetParameterList = new List<string>();

	private List<DataManagerChara.BonusCharaData> bonusCharaDataList;

	private Dictionary<int, CharaStaticData> charaStaticMap = new Dictionary<int, CharaStaticData>();

	private Dictionary<int, CharaStaticBase> charaStaticBaseMap = new Dictionary<int, CharaStaticBase>();

	private Dictionary<int, CharaContactStatic> charaContactStaticMap = new Dictionary<int, CharaContactStatic>();

	private Dictionary<int, List<CharaContactStatic>> charaContactStaticMapByChara = new Dictionary<int, List<CharaContactStatic>>();

	private Dictionary<int, int> charaId2modelId;

	private Dictionary<int, MstNpcData> npcMap = new Dictionary<int, MstNpcData>();

	private List<MstCharaKizunaRewardData> mstCharaKizunaRewardList;

	private List<MstKizunaRewardData> mstKizunaRewardList;

	private List<MstCharaKizunaBuffData> mstCharaKizunaBuffList;

	private List<MstCharaEffectData> mstCharaEffectDataList;

	private Dictionary<int, HashSet<int>> groupCharaMapByChara = new Dictionary<int, HashSet<int>>();

	public CharaKizunaQualified userCharaKizunaQualified;

	private Dictionary<int, MasterStaticSkill> masterSkillStaticMap = new Dictionary<int, MasterStaticSkill>();

	private Dictionary<int, TacticsStaticSkill> tacticsSkillStaticMap = new Dictionary<int, TacticsStaticSkill>();

	private TacticsParam tacticsParam;

	private Dictionary<int, EnemyStaticData> enemyStaticMap = new Dictionary<int, EnemyStaticData>();

	private Dictionary<int, CharaClothStatic> charaClothesStaticMap = new Dictionary<int, CharaClothStatic>();

	private DataManagerChara.CharaLevelupResult charaLevelupResult;

	private DataManagerChara.CharaLevelupResult charaKizunaLevelupResult;

	public class CharaLevelupResult
	{
		public int charaId;

		public int level;

		public long exp;

		public int befLevel;

		public long befExp;

		public bool returnItem;

		public DataManagerChara.CharaLevelupResult.Status successStatus;

		public enum Status
		{
			NORMAL,
			SPECIAL_S,
			SPECIAL_L
		}
	}

	public class SimulateAddExpResult
	{
		public int level;

		public long exp;

		public long maxExp;

		public int costGold;
	}

	public class KiznaRewardData
	{
		public int level;

		public int artsMax;

		public int charaquest;

		public int itemId;

		public int itemNum;
	}

	public class LevelLimitData
	{
		public int IncreaseMaxLevel
		{
			get
			{
				if (this.maxLevel <= 90)
				{
					return 0;
				}
				return this.maxLevel - 90;
			}
		}

		public string NeedItemName01
		{
			get
			{
				ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(this.needItemId01);
				if (itemStaticBase == null)
				{
					return string.Empty;
				}
				return itemStaticBase.GetName();
			}
		}

		public string NeedItemName02
		{
			get
			{
				ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(this.needItemId02);
				if (itemStaticBase == null)
				{
					return string.Empty;
				}
				return itemStaticBase.GetName();
			}
		}

		public int levelLimitId;

		public int needItemId01;

		public int needItemNum01;

		public int needItemId02;

		public int needItemNum02;

		public int needGoldNum;

		public int maxLevel;

		public bool isStartTime;

		public string openImageName;

		public string compImageName;
	}

	public class LevelLimitRisingStatus
	{
		public int patternId;

		public int addHp;

		public int addAtk;

		public int addDef;
	}

	public class BonusCharaData
	{
		public BonusCharaData(MstEventBonusCharaData mstCharaBonus)
		{
			this.charaId = mstCharaBonus.charaId;
			this.hpBonusRatio = mstCharaBonus.hpBonusRatio;
			this.strBonusRatio = mstCharaBonus.strBonusRatio;
			this.defBonusRatio = mstCharaBonus.defBonusRatio;
			this.kizunaBonusRatio = mstCharaBonus.kizunaBonusRatio;
			this.pickUpFlg = 1 == mstCharaBonus.pickupFlag;
			this.eventId = mstCharaBonus.eventId;
			this.increaseItemId01 = mstCharaBonus.increaseItemId01;
			this.dropBonusRatio01 = mstCharaBonus.dropBonusRatio01;
			this.increaseItemId02 = mstCharaBonus.increaseItemId02;
			this.dropBonusRatio02 = mstCharaBonus.dropBonusRatio02;
			this.startDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mstCharaBonus.startTime));
			this.endDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mstCharaBonus.endTime));
		}

		public int charaId;

		public int hpBonusRatio;

		public int strBonusRatio;

		public int defBonusRatio;

		public int kizunaBonusRatio;

		public bool pickUpFlg;

		public int eventId;

		public int increaseItemId01;

		public int dropBonusRatio01;

		public int increaseItemId02;

		public int dropBonusRatio02;

		public DateTime startDatetime;

		public DateTime endDatetime;
	}

	public class FilterData
	{
		public SortFilterDefine.CharacteristicFilterCategory Category { get; private set; }

		public string CategoryName { get; private set; }

		public string DisplayName1 { get; private set; }

		public string DisplayName2 { get; private set; }

		public List<DataManagerChara.FilterData.FilterElementOne> ElementList { get; set; }

		public bool IsEnabled
		{
			get
			{
				if (this._isEnable)
				{
					return true;
				}
				using (List<DataManagerChara.FilterData.FilterElementOne>.Enumerator enumerator = this.ElementList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.FilteringStartDatetime <= TimeManager.Now)
						{
							this._isEnable = true;
							return true;
						}
					}
				}
				return false;
			}
		}

		public int Priority { get; private set; }

		public DateTime StartDatetime { get; private set; }

		public FilterData(MstCharaFilterData mst)
		{
			this.Category = (SortFilterDefine.CharacteristicFilterCategory)mst.category;
			this.CategoryName = mst.categoryName;
			this.DisplayName1 = mst.labelTextMiracle;
			this.DisplayName2 = mst.labelTextCharacteristic;
			this.Priority = mst.priority;
			this.StartDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.startTime));
			this.ElementList = new List<DataManagerChara.FilterData.FilterElementOne>();
			this.AdditionalElement(mst);
		}

		public void AdditionalElement(MstCharaFilterData mst)
		{
			if (this.Category != (SortFilterDefine.CharacteristicFilterCategory)mst.category)
			{
				return;
			}
			this.Priority = ((this.Priority < mst.priority) ? this.Priority : mst.priority);
			DateTime dateTime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.startTime));
			this.StartDatetime = ((dateTime < this.StartDatetime) ? dateTime : this.StartDatetime);
			this.ElementList.Add(new DataManagerChara.FilterData.FilterElementOne(mst));
		}

		private bool _isEnable;

		public class FilterElementOne
		{
			public string FilterName { get; private set; }

			public DateTime FilteringStartDatetime { get; set; }

			public SortFilterDefine.FilterElementType FilterType { get; private set; }

			public CharaDef.ConditionType Condition
			{
				get
				{
					return this._condition;
				}
			}

			public CharaDef.AbilityTraits Traits
			{
				get
				{
					return this._traits;
				}
			}

			public CharaDef.AbilityTraits2 Traits2
			{
				get
				{
					return this._traits2;
				}
			}

			public CharaDef.ActionTargetType TargetType
			{
				get
				{
					return this._targetType;
				}
			}

			public CharaDef.ActionBuffType BuffType
			{
				get
				{
					return this._buffType;
				}
			}

			public CharaDef.ActionAbnormalMask AbnormalMask
			{
				get
				{
					return this._abnormalMask;
				}
			}

			public CharaDef.ActionAbnormalMask2 AbnormalMask2
			{
				get
				{
					return this._abnormalMask2;
				}
			}

			public bool GutsListEnable { get; private set; }

			public FilterElementOne(MstCharaFilterData mst)
			{
				this.FilterName = mst.filterName;
				this.FilterType = SortFilterDefine.FilterElementType.Invalid;
				switch (mst.category)
				{
				case 1:
				{
					bool flag = Enum.TryParse<CharaDef.ConditionType>(this.FilterName, out this._condition);
					if (!flag)
					{
						this._condition = CharaDef.ConditionType.UPPER;
					}
					else
					{
						this.FilterType = SortFilterDefine.FilterElementType.Condition;
					}
					if (!flag)
					{
						CharaDef.AbilityTraits terrainAll = SortFilterDefine.TerrainAll;
						flag = Enum.TryParse<CharaDef.AbilityTraits>(this.FilterName, out this._traits);
						if (!flag)
						{
							this._traits = terrainAll;
						}
						else
						{
							this.FilterType = SortFilterDefine.FilterElementType.Terrain;
						}
					}
					if (!flag)
					{
						flag = Enum.TryParse<CharaDef.AbilityTraits2>(this.FilterName, out this._traits2);
						if (!flag)
						{
							this._traits2 = CharaDef.AbilityTraits2.without;
						}
						else
						{
							this.FilterType = SortFilterDefine.FilterElementType.Night;
						}
					}
					if (!flag)
					{
					}
					break;
				}
				case 2:
				{
					bool flag2 = Enum.TryParse<CharaDef.ActionTargetType>(this.FilterName, out this._targetType);
					if (!flag2)
					{
						this._targetType = CharaDef.ActionTargetType.INVALID;
					}
					else
					{
						this.FilterType = SortFilterDefine.FilterElementType.Target;
					}
					if (!flag2)
					{
					}
					break;
				}
				case 3:
				{
					bool flag3 = Enum.TryParse<CharaDef.ActionBuffType>(this.FilterName, out this._buffType);
					if (!flag3)
					{
						this._buffType = CharaDef.ActionBuffType.INVALID;
					}
					else
					{
						this.FilterType = SortFilterDefine.FilterElementType.Buff;
					}
					if (!flag3 && "GUTS_LIST" == this.FilterName)
					{
						this.GutsListEnable = true;
						flag3 = true;
						this.FilterType = SortFilterDefine.FilterElementType.Guts;
					}
					if (!flag3)
					{
					}
					break;
				}
				case 4:
				{
					bool flag4 = Enum.TryParse<CharaDef.ActionAbnormalMask>(this.FilterName, out this._abnormalMask);
					bool flag5 = Enum.TryParse<CharaDef.ActionAbnormalMask2>(this.FilterName, out this._abnormalMask2);
					if (!flag4 && !flag5)
					{
						this._abnormalMask = (CharaDef.ActionAbnormalMask)0;
					}
					else
					{
						this.FilterType = SortFilterDefine.FilterElementType.Abnormal;
					}
					if (!flag4)
					{
					}
					break;
				}
				}
				this.FilteringStartDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.startTime));
			}

			private CharaDef.ConditionType _condition;

			private CharaDef.AbilityTraits _traits;

			private CharaDef.AbilityTraits2 _traits2;

			private CharaDef.ActionTargetType _targetType;

			private CharaDef.ActionBuffType _buffType;

			private CharaDef.ActionAbnormalMask _abnormalMask;

			private CharaDef.ActionAbnormalMask2 _abnormalMask2;
		}
	}
}
