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

// Token: 0x02000071 RID: 113
public class DataManagerChara
{
	// Token: 0x06000345 RID: 837 RVA: 0x00018B7C File Offset: 0x00016D7C
	public DataManagerChara(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x06000346 RID: 838 RVA: 0x00018C51 File Offset: 0x00016E51
	// (set) Token: 0x06000347 RID: 839 RVA: 0x00018C59 File Offset: 0x00016E59
	public bool ShopUpdateRequired { get; set; }

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x06000348 RID: 840 RVA: 0x00018C62 File Offset: 0x00016E62
	// (set) Token: 0x06000349 RID: 841 RVA: 0x00018C6A File Offset: 0x00016E6A
	public bool CharaMissionUpdateRequired { get; set; }

	// Token: 0x0600034A RID: 842 RVA: 0x00018C73 File Offset: 0x00016E73
	public static int CharaId2ModelId(int charaId)
	{
		if (Singleton<DataManager>.Instance != null && DataManager.DmChara.charaId2modelId != null && DataManager.DmChara.charaId2modelId.ContainsKey(charaId))
		{
			return DataManager.DmChara.charaId2modelId[charaId];
		}
		return charaId;
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x0600034B RID: 843 RVA: 0x00018CB2 File Offset: 0x00016EB2
	public List<CharaStaticData> CharaStaticDataList
	{
		get
		{
			List<CharaStaticData> list = new List<CharaStaticData>(this.charaStaticMap.Values);
			list.Sort((CharaStaticData a, CharaStaticData b) => a.baseData.id.CompareTo(b.baseData.id));
			return list;
		}
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x0600034C RID: 844 RVA: 0x00018CE9 File Offset: 0x00016EE9
	// (set) Token: 0x0600034D RID: 845 RVA: 0x00018CF1 File Offset: 0x00016EF1
	private List<DataManagerChara.FilterData> FilterDataListMiracle { get; set; }

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x0600034E RID: 846 RVA: 0x00018CFA File Offset: 0x00016EFA
	// (set) Token: 0x0600034F RID: 847 RVA: 0x00018D02 File Offset: 0x00016F02
	private List<DataManagerChara.FilterData> FilterDataListCharacteristic { get; set; }

	// Token: 0x06000350 RID: 848 RVA: 0x00018D0B File Offset: 0x00016F0B
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

	// Token: 0x06000351 RID: 849 RVA: 0x00018D4B File Offset: 0x00016F4B
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

	// Token: 0x06000352 RID: 850 RVA: 0x00018D8C File Offset: 0x00016F8C
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

	// Token: 0x06000353 RID: 851 RVA: 0x00018E18 File Offset: 0x00017018
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

	// Token: 0x06000354 RID: 852 RVA: 0x00018E68 File Offset: 0x00017068
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

	// Token: 0x06000355 RID: 853 RVA: 0x00018ED4 File Offset: 0x000170D4
	public bool CheckSameChara(int charaIdA, int charaIdB)
	{
		return this.GetSameCharaList(charaIdA, true).Contains(charaIdB);
	}

	// Token: 0x06000356 RID: 854 RVA: 0x00018EE4 File Offset: 0x000170E4
	public bool CheckSameChara(int charaIdA, List<int> charaIdBList)
	{
		HashSet<int> list = this.GetSameCharaList(charaIdA, true);
		return charaIdBList.Exists((int item) => list.Contains(item));
	}

	// Token: 0x06000357 RID: 855 RVA: 0x00018F18 File Offset: 0x00017118
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

	// Token: 0x06000358 RID: 856 RVA: 0x00018F9B File Offset: 0x0001719B
	public MasterStaticSkill GetMasterSkillStaticData(int id)
	{
		if (!this.masterSkillStaticMap.ContainsKey(id))
		{
			return this.masterSkillStaticMap[80001];
		}
		return this.masterSkillStaticMap[id];
	}

	// Token: 0x06000359 RID: 857 RVA: 0x00018FC8 File Offset: 0x000171C8
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

	// Token: 0x0600035A RID: 858 RVA: 0x00019054 File Offset: 0x00017254
	public List<TacticsStaticSkill> GetTacticsSkillStaticData()
	{
		return new List<TacticsStaticSkill>(this.tacticsSkillStaticMap.Values);
	}

	// Token: 0x0600035B RID: 859 RVA: 0x00019068 File Offset: 0x00017268
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

	// Token: 0x0600035C RID: 860 RVA: 0x00019132 File Offset: 0x00017332
	public List<TacticsParam.Tactics> GetTacticsParamData()
	{
		if (!(this.tacticsParam == null) && this.tacticsParam.tacticsParam != null)
		{
			return this.tacticsParam.tacticsParam;
		}
		return new List<TacticsParam.Tactics>();
	}

	// Token: 0x0600035D RID: 861 RVA: 0x00019160 File Offset: 0x00017360
	public EnemyStaticData GetEnemyStaticData(int enemyId)
	{
		if (!this.enemyStaticMap.ContainsKey(enemyId))
		{
			Verbose<PrjLog>.LogError("Error : DataManagerChara.GetEnemyStaticData : 定義されていないID[" + enemyId.ToString() + "]を生成しようとしました：プランナに連絡してください", null);
			return null;
		}
		return this.enemyStaticMap[enemyId];
	}

	// Token: 0x0600035E RID: 862 RVA: 0x0001919A File Offset: 0x0001739A
	public Dictionary<int, EnemyStaticData> GetEnemyStaticMap()
	{
		return this.enemyStaticMap;
	}

	// Token: 0x0600035F RID: 863 RVA: 0x000191A2 File Offset: 0x000173A2
	public CharaClothStatic GetCharaClothesStaticData(int clothesId)
	{
		if (!this.charaClothesStaticMap.ContainsKey(clothesId))
		{
			Verbose<PrjLog>.LogError("Error : DataManagerChara.GetCharaClothesStaticData : 定義されていないID[" + clothesId.ToString() + "]を生成しようとしました：プランナに連絡してください", null);
			return null;
		}
		return this.charaClothesStaticMap[clothesId];
	}

	// Token: 0x06000360 RID: 864 RVA: 0x000191DC File Offset: 0x000173DC
	public CharaContactStatic GetCharaContactStaticData(int contactId)
	{
		if (!this.charaContactStaticMap.ContainsKey(contactId))
		{
			Verbose<PrjLog>.LogError("Error : DataManagerChara.GetCharaContactStaticData : 定義されていないID[" + contactId.ToString() + "]を生成しようとしました：プランナに連絡してください", null);
			return null;
		}
		return this.charaContactStaticMap[contactId];
	}

	// Token: 0x06000361 RID: 865 RVA: 0x00019216 File Offset: 0x00017416
	public List<CharaContactStatic> GetContactByChara(int charaId)
	{
		return new List<CharaContactStatic>(this.charaContactStaticMapByChara.TryGetValueEx(charaId, new List<CharaContactStatic>()));
	}

	// Token: 0x06000362 RID: 866 RVA: 0x0001922E File Offset: 0x0001742E
	public List<CharaVoiceCombi> GetCharaVoiceCombiAllList()
	{
		return this.charaVoiceCombiList;
	}

	// Token: 0x06000363 RID: 867 RVA: 0x00019238 File Offset: 0x00017438
	public List<CharaVoiceCombi> GetCharaVoiceCombiList(CharaVoiceCombi.SitType type, int firstCharaId)
	{
		return this.charaVoiceCombiList.FindAll((CharaVoiceCombi item) => item.firstCharaId == firstCharaId && item.situationType == type);
	}

	// Token: 0x06000364 RID: 868 RVA: 0x00019270 File Offset: 0x00017470
	public List<DataManagerChara.BonusCharaData> GetBonusCharaDataList()
	{
		return this.GetBonusCharaDataListInternal(0, TimeManager.Now, true);
	}

	// Token: 0x06000365 RID: 869 RVA: 0x0001927F File Offset: 0x0001747F
	public List<DataManagerChara.BonusCharaData> GetBonusCharaDataList(int evID)
	{
		return this.GetBonusCharaDataListInternal(evID, TimeManager.Now, false);
	}

	// Token: 0x06000366 RID: 870 RVA: 0x0001928E File Offset: 0x0001748E
	public List<DataManagerChara.BonusCharaData> GetBonusCharaDataList(int evID, DateTime dateTime)
	{
		return this.GetBonusCharaDataListInternal(evID, dateTime, false);
	}

	// Token: 0x06000367 RID: 871 RVA: 0x0001929C File Offset: 0x0001749C
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

	// Token: 0x06000368 RID: 872 RVA: 0x00019318 File Offset: 0x00017518
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

	// Token: 0x06000369 RID: 873 RVA: 0x00019340 File Offset: 0x00017540
	public bool ClothLongSkirt(int clothId)
	{
		if (clothId == 0)
		{
			return false;
		}
		CharaClothStatic charaClothesStaticData = this.GetCharaClothesStaticData(clothId);
		return charaClothesStaticData != null && charaClothesStaticData.LongSkirt;
	}

	// Token: 0x0600036A RID: 874 RVA: 0x00019368 File Offset: 0x00017568
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

	// Token: 0x0600036B RID: 875 RVA: 0x0001938D File Offset: 0x0001758D
	public CharaPackData GetUserCharaData(int charaId)
	{
		if (this.userCharaMap.ContainsKey(charaId))
		{
			return this.userCharaMap[charaId];
		}
		return null;
	}

	// Token: 0x0600036C RID: 876 RVA: 0x000193AB File Offset: 0x000175AB
	public Dictionary<int, CharaPackData> GetUserCharaMap()
	{
		return this.userCharaMap;
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x0600036D RID: 877 RVA: 0x000193B3 File Offset: 0x000175B3
	// (set) Token: 0x0600036E RID: 878 RVA: 0x000193BB File Offset: 0x000175BB
	public Dictionary<int, int> UserAllCharaKemoStatusList { get; private set; } = new Dictionary<int, int>();

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x0600036F RID: 879 RVA: 0x000193C4 File Offset: 0x000175C4
	// (set) Token: 0x06000370 RID: 880 RVA: 0x000193CC File Offset: 0x000175CC
	public bool IsNeedUpdateByUserAllCharaKemoStatusList { get; set; }

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x06000371 RID: 881 RVA: 0x000193D5 File Offset: 0x000175D5
	public int UserAllCharaKemoStatus
	{
		get
		{
			return this.UserAllCharaKemoStatusList.Sum<KeyValuePair<int, int>>((KeyValuePair<int, int> item) => item.Value);
		}
	}

	// Token: 0x06000372 RID: 882 RVA: 0x00019401 File Offset: 0x00017601
	public MasterSkillPackData GetUserMasterSkillData(int id)
	{
		if (this.userMasterSkillMap.ContainsKey(id))
		{
			return this.userMasterSkillMap[id];
		}
		return null;
	}

	// Token: 0x06000373 RID: 883 RVA: 0x0001941F File Offset: 0x0001761F
	public Dictionary<int, MasterSkillPackData> GetUserMasterSkillMap()
	{
		return this.userMasterSkillMap;
	}

	// Token: 0x06000374 RID: 884 RVA: 0x00019427 File Offset: 0x00017627
	public List<MstCharaKizunaBuffData> GetMstKizunaBuffList()
	{
		return this.mstCharaKizunaBuffList;
	}

	// Token: 0x06000375 RID: 885 RVA: 0x0001942F File Offset: 0x0001762F
	public CharaKizunaQualified GetUserKizunaQualified()
	{
		return this.userCharaKizunaQualified;
	}

	// Token: 0x06000376 RID: 886 RVA: 0x00019438 File Offset: 0x00017638
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

	// Token: 0x06000377 RID: 887 RVA: 0x000195D8 File Offset: 0x000177D8
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

	// Token: 0x06000378 RID: 888 RVA: 0x0001977C File Offset: 0x0001797C
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

	// Token: 0x06000379 RID: 889 RVA: 0x00019850 File Offset: 0x00017A50
	public DataManagerChara.LevelLimitData GetLevelLimitData(int limitId)
	{
		return this.levelLimitDataList.Find((DataManagerChara.LevelLimitData x) => x.levelLimitId == limitId && x.isStartTime);
	}

	// Token: 0x0600037A RID: 890 RVA: 0x00019881 File Offset: 0x00017A81
	public int GetLevelLimitDataListCount()
	{
		return this.levelLimitDataList.Count;
	}

	// Token: 0x0600037B RID: 891 RVA: 0x00019890 File Offset: 0x00017A90
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

	// Token: 0x0600037C RID: 892 RVA: 0x000198F8 File Offset: 0x00017AF8
	public int GetKizunaLevelLimitOverCount(int maxLevel)
	{
		int num = 0;
		if (6 >= maxLevel)
		{
			return num;
		}
		return maxLevel - 6;
	}

	// Token: 0x0600037D RID: 893 RVA: 0x00019910 File Offset: 0x00017B10
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

	// Token: 0x0600037E RID: 894 RVA: 0x000199B0 File Offset: 0x00017BB0
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

	// Token: 0x0600037F RID: 895 RVA: 0x00019A7C File Offset: 0x00017C7C
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

	// Token: 0x06000380 RID: 896 RVA: 0x00019B04 File Offset: 0x00017D04
	public void UpdateUserCharasClothesData()
	{
		foreach (CharaPackData charaPackData in this.userCharaMap.Values)
		{
			this.UpdateUserCharaClothesData(charaPackData);
		}
	}

	// Token: 0x06000381 RID: 897 RVA: 0x00019B5C File Offset: 0x00017D5C
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

	// Token: 0x06000382 RID: 898 RVA: 0x00019BEC File Offset: 0x00017DEC
	public void UpdateUserCharasContactData()
	{
		foreach (CharaPackData charaPackData in this.userCharaMap.Values)
		{
			this.UpdateUserCharaContactData(charaPackData);
		}
	}

	// Token: 0x06000383 RID: 899 RVA: 0x00019C44 File Offset: 0x00017E44
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

	// Token: 0x06000384 RID: 900 RVA: 0x00019D04 File Offset: 0x00017F04
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

	// Token: 0x06000385 RID: 901 RVA: 0x00019DEC File Offset: 0x00017FEC
	public void UpdateUserDataByTutorial(List<CharaPackData> charaList)
	{
		this.userCharaMap = new Dictionary<int, CharaPackData>();
		foreach (CharaPackData charaPackData in charaList)
		{
			this.userCharaMap.Add(charaPackData.id, charaPackData);
		}
	}

	// Token: 0x06000386 RID: 902 RVA: 0x00019E50 File Offset: 0x00018050
	public void UpdateUserDataByTutorial(List<MasterSkillPackData> skillList)
	{
		this.userMasterSkillMap = new Dictionary<int, MasterSkillPackData>();
		foreach (MasterSkillPackData masterSkillPackData in skillList)
		{
			this.userMasterSkillMap.Add(masterSkillPackData.id, masterSkillPackData);
		}
	}

	// Token: 0x06000387 RID: 903 RVA: 0x00019EB4 File Offset: 0x000180B4
	public void UpdateKizunaQualifiedByServer(CharaKizunaQualified qualified)
	{
		this.userCharaKizunaQualified = qualified;
	}

	// Token: 0x06000388 RID: 904 RVA: 0x00019EBD File Offset: 0x000180BD
	public void UpdateUserDataByDebug(int charaId)
	{
		if (!this.userCharaMap.ContainsKey(charaId))
		{
			this.userCharaMap.Add(charaId, CharaPackData.MakeInitial(charaId));
		}
	}

	// Token: 0x06000389 RID: 905 RVA: 0x00019EDF File Offset: 0x000180DF
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

	// Token: 0x0600038A RID: 906 RVA: 0x00019EF8 File Offset: 0x000180F8
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

	// Token: 0x0600038B RID: 907 RVA: 0x00019FE8 File Offset: 0x000181E8
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

	// Token: 0x0600038C RID: 908 RVA: 0x0001A200 File Offset: 0x00018400
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

	// Token: 0x0600038D RID: 909 RVA: 0x0001A634 File Offset: 0x00018834
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

	// Token: 0x0600038E RID: 910 RVA: 0x0001A67C File Offset: 0x0001887C
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

	// Token: 0x0600038F RID: 911 RVA: 0x0001A750 File Offset: 0x00018950
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

	// Token: 0x06000390 RID: 912 RVA: 0x0001A7A7 File Offset: 0x000189A7
	private void InitializeTacticsParam()
	{
		this.tacticsParam = AssetManager.GetAssetData("Charas/Parameter/TacticsParam") as TacticsParam;
	}

	// Token: 0x06000391 RID: 913 RVA: 0x0001A7C0 File Offset: 0x000189C0
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

	// Token: 0x06000392 RID: 914 RVA: 0x0001A82C File Offset: 0x00018A2C
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

	// Token: 0x06000393 RID: 915 RVA: 0x0001A89C File Offset: 0x00018A9C
	public long GetNeedExpByLimitLevel(int charaId)
	{
		int limitLevel = DataManager.DmChara.GetUserCharaData(charaId).dynamicData.limitLevel;
		return this.GetNeedExpByRangeLevel(charaId, limitLevel);
	}

	// Token: 0x06000394 RID: 916 RVA: 0x0001A8C8 File Offset: 0x00018AC8
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

	// Token: 0x06000395 RID: 917 RVA: 0x0001A980 File Offset: 0x00018B80
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

	// Token: 0x06000396 RID: 918 RVA: 0x0001A9E8 File Offset: 0x00018BE8
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

	// Token: 0x06000397 RID: 919 RVA: 0x0001AA54 File Offset: 0x00018C54
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

	// Token: 0x06000398 RID: 920 RVA: 0x0001AAC4 File Offset: 0x00018CC4
	public long GetNeedExpByKizunaLimitLevel(int charaId)
	{
		int kizunaLimitLevel = DataManager.DmChara.GetUserCharaData(charaId).dynamicData.KizunaLimitLevel;
		return this.GetNeedExpByRangeKizunaLevel(charaId, kizunaLimitLevel);
	}

	// Token: 0x06000399 RID: 921 RVA: 0x0001AAF0 File Offset: 0x00018CF0
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

	// Token: 0x0600039A RID: 922 RVA: 0x0001AB98 File Offset: 0x00018D98
	public List<CharaClothStatic> GetClothListByChara(int charaId)
	{
		return new List<CharaClothStatic>(this.charaClothesStaticMap.Values).FindAll((CharaClothStatic item) => item.CharaId == charaId);
	}

	// Token: 0x0600039B RID: 923 RVA: 0x0001ABD4 File Offset: 0x00018DD4
	public int GetRankupRewardItem(int charaId, int rank)
	{
		CharaClothStatic charaClothStatic = new List<CharaClothStatic>(this.charaClothesStaticMap.Values).Find((CharaClothStatic item) => item.CharaId == charaId && item.GetRank == rank);
		if (charaClothStatic == null)
		{
			return 0;
		}
		return charaClothStatic.GetId();
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x0600039C RID: 924 RVA: 0x0001AC22 File Offset: 0x00018E22
	// (set) Token: 0x0600039D RID: 925 RVA: 0x0001AC2A File Offset: 0x00018E2A
	public List<ItemInput> RankUpResultKemoBoardItem { get; set; }

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x0600039E RID: 926 RVA: 0x0001AC33 File Offset: 0x00018E33
	// (set) Token: 0x0600039F RID: 927 RVA: 0x0001AC3B File Offset: 0x00018E3B
	public CharaGrowMultiRequest CharaGrowMultiRequest { get; set; }

	// Token: 0x060003A0 RID: 928 RVA: 0x0001AC44 File Offset: 0x00018E44
	public DataManagerChara.CharaLevelupResult GetCharaLevelupResult()
	{
		return this.charaLevelupResult;
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x0001AC4C File Offset: 0x00018E4C
	public DataManagerChara.CharaLevelupResult GetCharaKizunaLevelupResult()
	{
		return this.charaKizunaLevelupResult;
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x0001AC54 File Offset: 0x00018E54
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

	// Token: 0x060003A3 RID: 931 RVA: 0x0001AD80 File Offset: 0x00018F80
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

	// Token: 0x060003A4 RID: 932 RVA: 0x0001AEAC File Offset: 0x000190AC
	public void Destory()
	{
		foreach (string text in this.loadAssetParameterList)
		{
			AssetManager.UnloadAssetData(AssetManager.PREFIX_PATH_CHARA_PARAM + text, AssetManager.OWNER.DataManagerChara);
		}
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x0001AF0C File Offset: 0x0001910C
	public void RequestActionCharaGrowMulti()
	{
		this.parentData.ServerRequest(CharaGrowMultiCmd.Create(this.CharaGrowMultiRequest), new Action<Command>(this.CbCharaGrowMultiCmd));
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x0001AF30 File Offset: 0x00019130
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

	// Token: 0x060003A7 RID: 935 RVA: 0x0001B050 File Offset: 0x00019250
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

	// Token: 0x060003A8 RID: 936 RVA: 0x0001B148 File Offset: 0x00019348
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

	// Token: 0x060003A9 RID: 937 RVA: 0x0001B1AE File Offset: 0x000193AE
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

	// Token: 0x060003AA RID: 938 RVA: 0x0001B1F0 File Offset: 0x000193F0
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

	// Token: 0x060003AB RID: 939 RVA: 0x0001B24D File Offset: 0x0001944D
	public void RequestActionCharaReleasePhotoFrame(int charaId, int targetPhotoPocketStep)
	{
		this.parentData.ServerRequest(CharaPpRelCmd.Create(charaId, targetPhotoPocketStep), new Action<Command>(this.CbCharaPpRelCmd));
	}

	// Token: 0x060003AC RID: 940 RVA: 0x0001B270 File Offset: 0x00019470
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

	// Token: 0x060003AD RID: 941 RVA: 0x0001B2C2 File Offset: 0x000194C2
	public void RequestActoinCharaChangeClothes(int charaId, int clothId)
	{
		this.parentData.ServerRequest(CharaChangeClothesCmd.Create(charaId, clothId), new Action<Command>(this.CbCharaChangeClothesCmd));
	}

	// Token: 0x060003AE RID: 942 RVA: 0x0001B2E2 File Offset: 0x000194E2
	public void RequestActoinCharaChangeIcon(int charaId, int iconId)
	{
		this.parentData.ServerRequest(CharaSelectFaceIconCmd.Create(charaId, iconId), new Action<Command>(this.CbCharaChangeIconCmd));
	}

	// Token: 0x060003AF RID: 943 RVA: 0x0001B304 File Offset: 0x00019504
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

	// Token: 0x060003B0 RID: 944 RVA: 0x0001B356 File Offset: 0x00019556
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

	// Token: 0x060003B1 RID: 945 RVA: 0x0001B395 File Offset: 0x00019595
	public void RequestActoinCharaAccessoryOpen(int charaId)
	{
		this.parentData.ServerRequest(CharaAccessoryOpenCmd.Create(charaId), new Action<Command>(this.CbCharaAccessoryOpenCmd));
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x0001B3B4 File Offset: 0x000195B4
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

	// Token: 0x060003B3 RID: 947 RVA: 0x0001B40C File Offset: 0x0001960C
	public void RequestActoinCharaAccessoryEffectStatus(int charaId, bool effectDisp)
	{
		int num = (effectDisp ? 0 : 1);
		this.parentData.ServerRequest(CharaAccessoryEffectStatusCmd.Create(charaId, num), new Action<Command>(this.CharaAccessoryEffectStatus));
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x0001B43F File Offset: 0x0001963F
	public void RequestActtionCharaFavoriteFlag(int charaId)
	{
		this.parentData.ServerRequest(CharaFavoriteFlagCmd.Create(charaId), new Action<Command>(this.CharaFavoriteFlag));
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x0001B45E File Offset: 0x0001965E
	public void RequestUpdateTotalKemoStatus()
	{
		if (this.IsNeedUpdateByUserAllCharaKemoStatusList)
		{
			this.IsNeedUpdateByUserAllCharaKemoStatusList = false;
			this.parentData.ServerRequest(UpdateTotalKemoStatusCmd.Create(this.UserAllCharaKemoStatus), new Action<Command>(this.CbUpdateTotalKemoStatusCmd));
		}
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x0001B491 File Offset: 0x00019691
	public void RequestCharaTouchCount(int charaId, int count)
	{
		this.parentData.ServerRequest(CharaTouchCountCmd.Create(charaId, count), new Action<Command>(this.CbCharaTouchCmd));
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x0001B4B4 File Offset: 0x000196B4
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

	// Token: 0x060003B8 RID: 952 RVA: 0x0001B514 File Offset: 0x00019714
	private void CbCharaLevelUpCmd(Command cmd)
	{
		CharaLevelUpResponse charaLevelUpResponse = cmd.response as CharaLevelUpResponse;
		this.parentData.UpdateUserAssetByAssets(charaLevelUpResponse.assets);
		this.UpdateCharaLevelupResult(charaLevelUpResponse.level_result);
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x0001B54C File Offset: 0x0001974C
	private void CbCharaKizunaLevelUpCmd(Command cmd)
	{
		CharaKizunaLevelUpResponse charaKizunaLevelUpResponse = cmd.response as CharaKizunaLevelUpResponse;
		this.parentData.UpdateUserAssetByAssets(charaKizunaLevelUpResponse.assets);
		this.UpdateCharaKizunaLevelupResult(charaKizunaLevelUpResponse.level_result);
	}

	// Token: 0x060003BA RID: 954 RVA: 0x0001B584 File Offset: 0x00019784
	private void UpdateCharaLevelupResult(LevelResult level_result)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(level_result.chara_id);
		this.charaLevelupResult.charaId = userCharaData.id;
		this.charaLevelupResult.exp = userCharaData.dynamicData.exp;
		this.charaLevelupResult.level = userCharaData.dynamicData.level;
		this.charaLevelupResult.returnItem = level_result.returnitem != 0;
		this.charaLevelupResult.successStatus = (DataManagerChara.CharaLevelupResult.Status)level_result.success_status;
	}

	// Token: 0x060003BB RID: 955 RVA: 0x0001B604 File Offset: 0x00019804
	private void UpdateCharaKizunaLevelupResult(KizunaLevelResult level_result)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(level_result.chara_id);
		this.charaKizunaLevelupResult.charaId = userCharaData.id;
		this.charaKizunaLevelupResult.exp = userCharaData.dynamicData.kizunaExp;
		this.charaKizunaLevelupResult.level = userCharaData.dynamicData.kizunaLevel;
		this.charaKizunaLevelupResult.returnItem = level_result.return_item != 0;
		this.charaKizunaLevelupResult.successStatus = (DataManagerChara.CharaLevelupResult.Status)level_result.success_status;
	}

	// Token: 0x060003BC RID: 956 RVA: 0x0001B684 File Offset: 0x00019884
	private void CbCharaWildRelCmd(Command cmd)
	{
		CharaWildRelResponse charaWildRelResponse = cmd.response as CharaWildRelResponse;
		this.parentData.UpdateUserAssetByAssets(charaWildRelResponse.assets);
	}

	// Token: 0x060003BD RID: 957 RVA: 0x0001B6B0 File Offset: 0x000198B0
	private void CbCharaNanairoReleaseCmd(Command cmd)
	{
		CharaNanairoAbilityReleaseResponse charaNanairoAbilityReleaseResponse = cmd.response as CharaNanairoAbilityReleaseResponse;
		this.parentData.UpdateUserAssetByAssets(charaNanairoAbilityReleaseResponse.assets);
	}

	// Token: 0x060003BE RID: 958 RVA: 0x0001B6DC File Offset: 0x000198DC
	private void CbCharaRankUpCmd(Command cmd)
	{
		CharaRankUpResponse charaRankUpResponse = cmd.response as CharaRankUpResponse;
		this.UpdateRankUpResultKemoBoardItem(charaRankUpResponse.assets.update_item_list);
		this.parentData.UpdateUserAssetByAssets(charaRankUpResponse.assets);
	}

	// Token: 0x060003BF RID: 959 RVA: 0x0001B718 File Offset: 0x00019918
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

	// Token: 0x060003C0 RID: 960 RVA: 0x0001B7AC File Offset: 0x000199AC
	private void CbCharaPpRelCmd(Command cmd)
	{
		CharaPpRelResponse charaPpRelResponse = cmd.response as CharaPpRelResponse;
		this.parentData.UpdateUserAssetByAssets(charaPpRelResponse.assets);
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x0001B7D8 File Offset: 0x000199D8
	private void CbCharaArtsUpCmd(Command cmd)
	{
		CharaArtsUpResponse charaArtsUpResponse = cmd.response as CharaArtsUpResponse;
		this.parentData.UpdateUserAssetByAssets(charaArtsUpResponse.assets);
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x0001B804 File Offset: 0x00019A04
	private void CbCharaChangeClothesCmd(Command cmd)
	{
		CharaChangeClothesResponse charaChangeClothesResponse = cmd.response as CharaChangeClothesResponse;
		this.parentData.UpdateUserAssetByAssets(charaChangeClothesResponse.assets);
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x0001B830 File Offset: 0x00019A30
	private void CbCharaChangeIconCmd(Command cmd)
	{
		CharaSelectFaceIconResponse charaSelectFaceIconResponse = cmd.response as CharaSelectFaceIconResponse;
		this.parentData.UpdateUserAssetByAssets(charaSelectFaceIconResponse.assets);
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x0001B85C File Offset: 0x00019A5C
	private void CbCharaLimitLevelUpCmd(Command cmd)
	{
		CharaLimitLevelUpResponse charaLimitLevelUpResponse = cmd.response as CharaLimitLevelUpResponse;
		this.parentData.UpdateUserAssetByAssets(charaLimitLevelUpResponse.assets);
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x0001B888 File Offset: 0x00019A88
	private void CbCharaKizunaLimitLevelUpCmd(Command cmd)
	{
		CharaKizunaLimitLevelUpResponse charaKizunaLimitLevelUpResponse = cmd.response as CharaKizunaLimitLevelUpResponse;
		this.parentData.UpdateUserAssetByAssets(charaKizunaLimitLevelUpResponse.assets);
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x0001B8B4 File Offset: 0x00019AB4
	private void CbCharaAccessoryOpenCmd(Command cmd)
	{
		CharaAccessoryOpenResponse charaAccessoryOpenResponse = cmd.response as CharaAccessoryOpenResponse;
		this.parentData.UpdateUserAssetByAssets(charaAccessoryOpenResponse.assets);
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x0001B8E0 File Offset: 0x00019AE0
	private void CbCharaAccessoryEquipCmd(Command cmd)
	{
		CharaAccessoryEquipResponse charaAccessoryEquipResponse = cmd.response as CharaAccessoryEquipResponse;
		this.parentData.UpdateUserAssetByAssets(charaAccessoryEquipResponse.assets);
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x0001B90C File Offset: 0x00019B0C
	private void CharaAccessoryEffectStatus(Command cmd)
	{
		CharaAccessoryEffectStatusResponse charaAccessoryEffectStatusResponse = cmd.response as CharaAccessoryEffectStatusResponse;
		this.parentData.UpdateUserAssetByAssets(charaAccessoryEffectStatusResponse.assets);
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x0001B938 File Offset: 0x00019B38
	private void CharaFavoriteFlag(Command cmd)
	{
		CharaFavoriteFlagResponse charaFavoriteFlagResponse = cmd.response as CharaFavoriteFlagResponse;
		this.parentData.UpdateUserAssetByAssets(charaFavoriteFlagResponse.assets);
	}

	// Token: 0x060003CA RID: 970 RVA: 0x0001B964 File Offset: 0x00019B64
	private void CbUpdateTotalKemoStatusCmd(Command cmd)
	{
		UpdateTotalKemoStatusResponse updateTotalKemoStatusResponse = cmd.response as UpdateTotalKemoStatusResponse;
		this.parentData.UpdateUserAssetByAssets(updateTotalKemoStatusResponse.assets);
	}

	// Token: 0x060003CB RID: 971 RVA: 0x0001B990 File Offset: 0x00019B90
	private void CbCharaTouchCmd(Command cmd)
	{
		CharaTouchCountResponse charaTouchCountResponse = cmd.response as CharaTouchCountResponse;
		this.parentData.UpdateUserAssetByAssets(charaTouchCountResponse.assets);
	}

	// Token: 0x060003CC RID: 972 RVA: 0x0001B9BA File Offset: 0x00019BBA
	public List<DataManagerPhoto.CalcDropBonusResult> CalcDropBonus(List<DataManagerPhoto.CalcDropBonusResult> result, List<DataManagerChara.BonusCharaData> dropBonusCharaList, List<int> pocketReleaseCountList)
	{
		return this.InternalCalcDropBonus(result, dropBonusCharaList, pocketReleaseCountList);
	}

	// Token: 0x060003CD RID: 973 RVA: 0x0001B9C8 File Offset: 0x00019BC8
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

	// Token: 0x040004AC RID: 1196
	private DataManager parentData;

	// Token: 0x040004AD RID: 1197
	private Dictionary<int, CharaPackData> userCharaMap = new Dictionary<int, CharaPackData>();

	// Token: 0x040004AE RID: 1198
	private Dictionary<int, MasterSkillPackData> userMasterSkillMap = new Dictionary<int, MasterSkillPackData>();

	// Token: 0x040004AF RID: 1199
	private List<CharaVoiceCombi> charaVoiceCombiList = new List<CharaVoiceCombi>();

	// Token: 0x040004B0 RID: 1200
	private List<DataManagerChara.LevelLimitData> levelLimitDataList = new List<DataManagerChara.LevelLimitData>();

	// Token: 0x040004B1 RID: 1201
	private List<DataManagerChara.LevelLimitRisingStatus> levelLimitRisingStatusList = new List<DataManagerChara.LevelLimitRisingStatus>();

	// Token: 0x040004B2 RID: 1202
	private List<string> loadAssetParameterList = new List<string>();

	// Token: 0x040004B3 RID: 1203
	private List<DataManagerChara.BonusCharaData> bonusCharaDataList;

	// Token: 0x040004B4 RID: 1204
	private Dictionary<int, CharaStaticData> charaStaticMap = new Dictionary<int, CharaStaticData>();

	// Token: 0x040004B5 RID: 1205
	private Dictionary<int, CharaStaticBase> charaStaticBaseMap = new Dictionary<int, CharaStaticBase>();

	// Token: 0x040004B6 RID: 1206
	private Dictionary<int, CharaContactStatic> charaContactStaticMap = new Dictionary<int, CharaContactStatic>();

	// Token: 0x040004B7 RID: 1207
	private Dictionary<int, List<CharaContactStatic>> charaContactStaticMapByChara = new Dictionary<int, List<CharaContactStatic>>();

	// Token: 0x040004B8 RID: 1208
	private Dictionary<int, int> charaId2modelId;

	// Token: 0x040004B9 RID: 1209
	private Dictionary<int, MstNpcData> npcMap = new Dictionary<int, MstNpcData>();

	// Token: 0x040004BA RID: 1210
	private List<MstCharaKizunaRewardData> mstCharaKizunaRewardList;

	// Token: 0x040004BB RID: 1211
	private List<MstKizunaRewardData> mstKizunaRewardList;

	// Token: 0x040004BC RID: 1212
	private List<MstCharaKizunaBuffData> mstCharaKizunaBuffList;

	// Token: 0x040004BD RID: 1213
	private List<MstCharaEffectData> mstCharaEffectDataList;

	// Token: 0x040004BE RID: 1214
	private Dictionary<int, HashSet<int>> groupCharaMapByChara = new Dictionary<int, HashSet<int>>();

	// Token: 0x040004C1 RID: 1217
	public CharaKizunaQualified userCharaKizunaQualified;

	// Token: 0x040004C4 RID: 1220
	private Dictionary<int, MasterStaticSkill> masterSkillStaticMap = new Dictionary<int, MasterStaticSkill>();

	// Token: 0x040004C5 RID: 1221
	private Dictionary<int, TacticsStaticSkill> tacticsSkillStaticMap = new Dictionary<int, TacticsStaticSkill>();

	// Token: 0x040004C6 RID: 1222
	private TacticsParam tacticsParam;

	// Token: 0x040004C7 RID: 1223
	private Dictionary<int, EnemyStaticData> enemyStaticMap = new Dictionary<int, EnemyStaticData>();

	// Token: 0x040004C8 RID: 1224
	private Dictionary<int, CharaClothStatic> charaClothesStaticMap = new Dictionary<int, CharaClothStatic>();

	// Token: 0x040004CD RID: 1229
	private DataManagerChara.CharaLevelupResult charaLevelupResult;

	// Token: 0x040004CE RID: 1230
	private DataManagerChara.CharaLevelupResult charaKizunaLevelupResult;

	// Token: 0x02000639 RID: 1593
	public class CharaLevelupResult
	{
		// Token: 0x04002E17 RID: 11799
		public int charaId;

		// Token: 0x04002E18 RID: 11800
		public int level;

		// Token: 0x04002E19 RID: 11801
		public long exp;

		// Token: 0x04002E1A RID: 11802
		public int befLevel;

		// Token: 0x04002E1B RID: 11803
		public long befExp;

		// Token: 0x04002E1C RID: 11804
		public bool returnItem;

		// Token: 0x04002E1D RID: 11805
		public DataManagerChara.CharaLevelupResult.Status successStatus;

		// Token: 0x0200110F RID: 4367
		public enum Status
		{
			// Token: 0x04005DE8 RID: 24040
			NORMAL,
			// Token: 0x04005DE9 RID: 24041
			SPECIAL_S,
			// Token: 0x04005DEA RID: 24042
			SPECIAL_L
		}
	}

	// Token: 0x0200063A RID: 1594
	public class SimulateAddExpResult
	{
		// Token: 0x04002E1E RID: 11806
		public int level;

		// Token: 0x04002E1F RID: 11807
		public long exp;

		// Token: 0x04002E20 RID: 11808
		public long maxExp;

		// Token: 0x04002E21 RID: 11809
		public int costGold;
	}

	// Token: 0x0200063B RID: 1595
	public class KiznaRewardData
	{
		// Token: 0x04002E22 RID: 11810
		public int level;

		// Token: 0x04002E23 RID: 11811
		public int artsMax;

		// Token: 0x04002E24 RID: 11812
		public int charaquest;

		// Token: 0x04002E25 RID: 11813
		public int itemId;

		// Token: 0x04002E26 RID: 11814
		public int itemNum;
	}

	// Token: 0x0200063C RID: 1596
	public class LevelLimitData
	{
		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06003080 RID: 12416 RVA: 0x001BAC3B File Offset: 0x001B8E3B
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

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06003081 RID: 12417 RVA: 0x001BAC54 File Offset: 0x001B8E54
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

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06003082 RID: 12418 RVA: 0x001BAC84 File Offset: 0x001B8E84
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

		// Token: 0x04002E27 RID: 11815
		public int levelLimitId;

		// Token: 0x04002E28 RID: 11816
		public int needItemId01;

		// Token: 0x04002E29 RID: 11817
		public int needItemNum01;

		// Token: 0x04002E2A RID: 11818
		public int needItemId02;

		// Token: 0x04002E2B RID: 11819
		public int needItemNum02;

		// Token: 0x04002E2C RID: 11820
		public int needGoldNum;

		// Token: 0x04002E2D RID: 11821
		public int maxLevel;

		// Token: 0x04002E2E RID: 11822
		public bool isStartTime;

		// Token: 0x04002E2F RID: 11823
		public string openImageName;

		// Token: 0x04002E30 RID: 11824
		public string compImageName;
	}

	// Token: 0x0200063D RID: 1597
	public class LevelLimitRisingStatus
	{
		// Token: 0x04002E31 RID: 11825
		public int patternId;

		// Token: 0x04002E32 RID: 11826
		public int addHp;

		// Token: 0x04002E33 RID: 11827
		public int addAtk;

		// Token: 0x04002E34 RID: 11828
		public int addDef;
	}

	// Token: 0x0200063E RID: 1598
	public class BonusCharaData
	{
		// Token: 0x06003085 RID: 12421 RVA: 0x001BACC4 File Offset: 0x001B8EC4
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

		// Token: 0x04002E35 RID: 11829
		public int charaId;

		// Token: 0x04002E36 RID: 11830
		public int hpBonusRatio;

		// Token: 0x04002E37 RID: 11831
		public int strBonusRatio;

		// Token: 0x04002E38 RID: 11832
		public int defBonusRatio;

		// Token: 0x04002E39 RID: 11833
		public int kizunaBonusRatio;

		// Token: 0x04002E3A RID: 11834
		public bool pickUpFlg;

		// Token: 0x04002E3B RID: 11835
		public int eventId;

		// Token: 0x04002E3C RID: 11836
		public int increaseItemId01;

		// Token: 0x04002E3D RID: 11837
		public int dropBonusRatio01;

		// Token: 0x04002E3E RID: 11838
		public int increaseItemId02;

		// Token: 0x04002E3F RID: 11839
		public int dropBonusRatio02;

		// Token: 0x04002E40 RID: 11840
		public DateTime startDatetime;

		// Token: 0x04002E41 RID: 11841
		public DateTime endDatetime;
	}

	// Token: 0x0200063F RID: 1599
	public class FilterData
	{
		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06003086 RID: 12422 RVA: 0x001BAD8A File Offset: 0x001B8F8A
		// (set) Token: 0x06003087 RID: 12423 RVA: 0x001BAD92 File Offset: 0x001B8F92
		public SortFilterDefine.CharacteristicFilterCategory Category { get; private set; }

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06003088 RID: 12424 RVA: 0x001BAD9B File Offset: 0x001B8F9B
		// (set) Token: 0x06003089 RID: 12425 RVA: 0x001BADA3 File Offset: 0x001B8FA3
		public string CategoryName { get; private set; }

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x0600308A RID: 12426 RVA: 0x001BADAC File Offset: 0x001B8FAC
		// (set) Token: 0x0600308B RID: 12427 RVA: 0x001BADB4 File Offset: 0x001B8FB4
		public string DisplayName1 { get; private set; }

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x0600308C RID: 12428 RVA: 0x001BADBD File Offset: 0x001B8FBD
		// (set) Token: 0x0600308D RID: 12429 RVA: 0x001BADC5 File Offset: 0x001B8FC5
		public string DisplayName2 { get; private set; }

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x0600308E RID: 12430 RVA: 0x001BADCE File Offset: 0x001B8FCE
		// (set) Token: 0x0600308F RID: 12431 RVA: 0x001BADD6 File Offset: 0x001B8FD6
		public List<DataManagerChara.FilterData.FilterElementOne> ElementList { get; set; }

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06003090 RID: 12432 RVA: 0x001BADE0 File Offset: 0x001B8FE0
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

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06003091 RID: 12433 RVA: 0x001BAE54 File Offset: 0x001B9054
		// (set) Token: 0x06003092 RID: 12434 RVA: 0x001BAE5C File Offset: 0x001B905C
		public int Priority { get; private set; }

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06003093 RID: 12435 RVA: 0x001BAE65 File Offset: 0x001B9065
		// (set) Token: 0x06003094 RID: 12436 RVA: 0x001BAE6D File Offset: 0x001B906D
		public DateTime StartDatetime { get; private set; }

		// Token: 0x06003095 RID: 12437 RVA: 0x001BAE78 File Offset: 0x001B9078
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

		// Token: 0x06003096 RID: 12438 RVA: 0x001BAEF0 File Offset: 0x001B90F0
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

		// Token: 0x04002E47 RID: 11847
		private bool _isEnable;

		// Token: 0x02001110 RID: 4368
		public class FilterElementOne
		{
			// Token: 0x17000C15 RID: 3093
			// (get) Token: 0x0600545C RID: 21596 RVA: 0x0024D9B5 File Offset: 0x0024BBB5
			// (set) Token: 0x0600545D RID: 21597 RVA: 0x0024D9BD File Offset: 0x0024BBBD
			public string FilterName { get; private set; }

			// Token: 0x17000C16 RID: 3094
			// (get) Token: 0x0600545E RID: 21598 RVA: 0x0024D9C6 File Offset: 0x0024BBC6
			// (set) Token: 0x0600545F RID: 21599 RVA: 0x0024D9CE File Offset: 0x0024BBCE
			public DateTime FilteringStartDatetime { get; set; }

			// Token: 0x17000C17 RID: 3095
			// (get) Token: 0x06005460 RID: 21600 RVA: 0x0024D9D7 File Offset: 0x0024BBD7
			// (set) Token: 0x06005461 RID: 21601 RVA: 0x0024D9DF File Offset: 0x0024BBDF
			public SortFilterDefine.FilterElementType FilterType { get; private set; }

			// Token: 0x17000C18 RID: 3096
			// (get) Token: 0x06005462 RID: 21602 RVA: 0x0024D9E8 File Offset: 0x0024BBE8
			public CharaDef.ConditionType Condition
			{
				get
				{
					return this._condition;
				}
			}

			// Token: 0x17000C19 RID: 3097
			// (get) Token: 0x06005463 RID: 21603 RVA: 0x0024D9F0 File Offset: 0x0024BBF0
			public CharaDef.AbilityTraits Traits
			{
				get
				{
					return this._traits;
				}
			}

			// Token: 0x17000C1A RID: 3098
			// (get) Token: 0x06005464 RID: 21604 RVA: 0x0024D9F8 File Offset: 0x0024BBF8
			public CharaDef.AbilityTraits2 Traits2
			{
				get
				{
					return this._traits2;
				}
			}

			// Token: 0x17000C1B RID: 3099
			// (get) Token: 0x06005465 RID: 21605 RVA: 0x0024DA00 File Offset: 0x0024BC00
			public CharaDef.ActionTargetType TargetType
			{
				get
				{
					return this._targetType;
				}
			}

			// Token: 0x17000C1C RID: 3100
			// (get) Token: 0x06005466 RID: 21606 RVA: 0x0024DA08 File Offset: 0x0024BC08
			public CharaDef.ActionBuffType BuffType
			{
				get
				{
					return this._buffType;
				}
			}

			// Token: 0x17000C1D RID: 3101
			// (get) Token: 0x06005467 RID: 21607 RVA: 0x0024DA10 File Offset: 0x0024BC10
			public CharaDef.ActionAbnormalMask AbnormalMask
			{
				get
				{
					return this._abnormalMask;
				}
			}

			// Token: 0x17000C1E RID: 3102
			// (get) Token: 0x06005468 RID: 21608 RVA: 0x0024DA18 File Offset: 0x0024BC18
			public CharaDef.ActionAbnormalMask2 AbnormalMask2
			{
				get
				{
					return this._abnormalMask2;
				}
			}

			// Token: 0x17000C1F RID: 3103
			// (get) Token: 0x06005469 RID: 21609 RVA: 0x0024DA20 File Offset: 0x0024BC20
			// (set) Token: 0x0600546A RID: 21610 RVA: 0x0024DA28 File Offset: 0x0024BC28
			public bool GutsListEnable { get; private set; }

			// Token: 0x0600546B RID: 21611 RVA: 0x0024DA34 File Offset: 0x0024BC34
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

			// Token: 0x04005DEE RID: 24046
			private CharaDef.ConditionType _condition;

			// Token: 0x04005DEF RID: 24047
			private CharaDef.AbilityTraits _traits;

			// Token: 0x04005DF0 RID: 24048
			private CharaDef.AbilityTraits2 _traits2;

			// Token: 0x04005DF1 RID: 24049
			private CharaDef.ActionTargetType _targetType;

			// Token: 0x04005DF2 RID: 24050
			private CharaDef.ActionBuffType _buffType;

			// Token: 0x04005DF3 RID: 24051
			private CharaDef.ActionAbnormalMask _abnormalMask;

			// Token: 0x04005DF4 RID: 24052
			private CharaDef.ActionAbnormalMask2 _abnormalMask2;
		}
	}
}
