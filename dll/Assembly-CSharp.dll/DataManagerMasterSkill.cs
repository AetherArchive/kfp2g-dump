using System;
using System.Collections.Generic;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x02000091 RID: 145
public class DataManagerMasterSkill
{
	// Token: 0x0600059F RID: 1439 RVA: 0x00025D97 File Offset: 0x00023F97
	public DataManagerMasterSkill(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x060005A0 RID: 1440 RVA: 0x00025DA6 File Offset: 0x00023FA6
	// (set) Token: 0x060005A1 RID: 1441 RVA: 0x00025DAE File Offset: 0x00023FAE
	public List<DataManagerMasterSkill.MasterSkillData> UserMasterSkillDataList { get; private set; }

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x060005A2 RID: 1442 RVA: 0x00025DB7 File Offset: 0x00023FB7
	// (set) Token: 0x060005A3 RID: 1443 RVA: 0x00025DBF File Offset: 0x00023FBF
	public MasterSkillGrowResult LatestMasterSkillGrowResult { get; private set; }

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x060005A4 RID: 1444 RVA: 0x00025DC8 File Offset: 0x00023FC8
	// (set) Token: 0x060005A5 RID: 1445 RVA: 0x00025DD0 File Offset: 0x00023FD0
	private List<DataManagerMasterSkill.SkillData> SkillDataList { get; set; }

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x060005A6 RID: 1446 RVA: 0x00025DD9 File Offset: 0x00023FD9
	// (set) Token: 0x060005A7 RID: 1447 RVA: 0x00025DE1 File Offset: 0x00023FE1
	private List<DataManagerMasterSkill.LevelData> LevelDataList { get; set; }

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x060005A8 RID: 1448 RVA: 0x00025DEA File Offset: 0x00023FEA
	// (set) Token: 0x060005A9 RID: 1449 RVA: 0x00025DF2 File Offset: 0x00023FF2
	private Dictionary<int, List<DataManagerMasterSkill.LevelData>> LevelDataMap { get; set; }

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x060005AA RID: 1450 RVA: 0x00025DFB File Offset: 0x00023FFB
	// (set) Token: 0x060005AB RID: 1451 RVA: 0x00025E03 File Offset: 0x00024003
	private Dictionary<int, DataManagerMasterSkill.LevelItemData> LevelItemDataMap { get; set; }

	// Token: 0x060005AC RID: 1452 RVA: 0x00025E0C File Offset: 0x0002400C
	public void RequestActionMasterSkillGrow(int skillId, List<UseItem> useItemList)
	{
		this.parentData.ServerRequest(MasterSkillGrowCmd.Create(skillId, useItemList), new Action<Command>(this.CbMasterSkillGrowCmd));
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x00025E2C File Offset: 0x0002402C
	private void CbMasterSkillGrowCmd(Command cmd)
	{
		MasterSkillGrowResponse masterSkillGrowResponse = cmd.response as MasterSkillGrowResponse;
		this.LatestMasterSkillGrowResult = masterSkillGrowResponse.level_result;
		this.parentData.UpdateUserAssetByAssets(masterSkillGrowResponse.assets);
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x00025E64 File Offset: 0x00024064
	public void InitializeMstData(MstManager mstManager)
	{
		List<MstMasterSkillData> mst = mstManager.GetMst<List<MstMasterSkillData>>(MstType.MASTER_SKILL_DATA);
		List<MstMasterSkillLevelData> mst2 = mstManager.GetMst<List<MstMasterSkillLevelData>>(MstType.MASTER_SKILL_LEVEL_DATA);
		List<MstMasterSkillLevelItem> mst3 = mstManager.GetMst<List<MstMasterSkillLevelItem>>(MstType.MASTER_SKILL_LEVEL_ITEM);
		this.SkillDataList = new List<DataManagerMasterSkill.SkillData>();
		foreach (MstMasterSkillData mstMasterSkillData in mst)
		{
			this.SkillDataList.Add(new DataManagerMasterSkill.SkillData(mstMasterSkillData));
		}
		this.LevelDataList = new List<DataManagerMasterSkill.LevelData>();
		this.LevelDataMap = new Dictionary<int, List<DataManagerMasterSkill.LevelData>>();
		mst2.Sort((MstMasterSkillLevelData a, MstMasterSkillLevelData b) => a.exp.CompareTo(b.exp));
		PrjUtil.InsertionSort<MstMasterSkillLevelData>(ref mst2, (MstMasterSkillLevelData a, MstMasterSkillLevelData b) => a.levelId - b.levelId);
		foreach (MstMasterSkillLevelData mstMasterSkillLevelData in mst2)
		{
			this.LevelDataList.Add(new DataManagerMasterSkill.LevelData(mstMasterSkillLevelData.levelId, mstMasterSkillLevelData.level, mstMasterSkillLevelData.exp));
			if (!this.LevelDataMap.ContainsKey(mstMasterSkillLevelData.levelId))
			{
				this.LevelDataMap.Add(mstMasterSkillLevelData.levelId, new List<DataManagerMasterSkill.LevelData>());
			}
			this.LevelDataMap[mstMasterSkillLevelData.levelId].Add(new DataManagerMasterSkill.LevelData(mstMasterSkillLevelData.levelId, mstMasterSkillLevelData.level, mstMasterSkillLevelData.exp));
		}
		this.LevelItemDataMap = new Dictionary<int, DataManagerMasterSkill.LevelItemData>();
		foreach (MstMasterSkillLevelItem mstMasterSkillLevelItem in mst3)
		{
			if (!this.LevelItemDataMap.ContainsKey(mstMasterSkillLevelItem.itemId))
			{
				this.LevelItemDataMap.Add(mstMasterSkillLevelItem.itemId, new DataManagerMasterSkill.LevelItemData(mstMasterSkillLevelItem));
			}
		}
		foreach (DataManagerMasterSkill.SkillData skillData in this.SkillDataList)
		{
			MasterStaticSkill masterSkillStaticData = DataManager.DmChara.GetMasterSkillStaticData(skillData.SkillId);
			if (this.LevelDataMap.ContainsKey(skillData.LevelId))
			{
				foreach (DataManagerMasterSkill.LevelData levelData in this.LevelDataMap[skillData.LevelId])
				{
					masterSkillStaticData.maxLevel = levelData.Level;
					skillData.LevelMax = levelData.Level;
				}
			}
		}
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x00026140 File Offset: 0x00024340
	public void UpdateUserDataByServer(List<MasterSkill> masterSkillList)
	{
		if (masterSkillList == null)
		{
			return;
		}
		if (this.UserMasterSkillDataList == null)
		{
			this.UserMasterSkillDataList = new List<DataManagerMasterSkill.MasterSkillData>();
		}
		foreach (MasterSkill masterSkill in masterSkillList)
		{
			this.UpdateUserMasterSkill(masterSkill.item_id, masterSkill.level, masterSkill.exp);
		}
		this.UserMasterSkillDataList.Sort((DataManagerMasterSkill.MasterSkillData a, DataManagerMasterSkill.MasterSkillData b) => a.SkillId - b.SkillId);
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x000261E0 File Offset: 0x000243E0
	private void UpdateUserMasterSkill(int id, int lv, long exp)
	{
		DataManagerMasterSkill.MasterSkillData masterSkillData = this.UserMasterSkillDataList.Find((DataManagerMasterSkill.MasterSkillData x) => x.SkillId == id);
		if (masterSkillData == null)
		{
			this.UserMasterSkillDataList.Add(new DataManagerMasterSkill.MasterSkillData(id, lv, exp));
		}
		else
		{
			masterSkillData.Level = lv;
			masterSkillData.Exp = exp;
		}
		DataManager.DmChara.GetUserMasterSkillData(id).dynamicData.level = lv;
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x00026258 File Offset: 0x00024458
	public DataManagerMasterSkill.LevelItemData GetLevelItemData(int itemId)
	{
		if (this.LevelItemDataMap.ContainsKey(itemId))
		{
			return this.LevelItemDataMap[itemId];
		}
		return new DataManagerMasterSkill.LevelItemData(new MstMasterSkillLevelItem());
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x00026280 File Offset: 0x00024480
	public List<DataManagerMasterSkill.LevelItemData> GetLevelItemDataList()
	{
		List<DataManagerMasterSkill.LevelItemData> list = new List<DataManagerMasterSkill.LevelItemData>();
		foreach (DataManagerMasterSkill.LevelItemData levelItemData in this.LevelItemDataMap.Values)
		{
			list.Add(levelItemData);
		}
		return list;
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x000262E0 File Offset: 0x000244E0
	public DataManagerMasterSkill.NextCalcSkillLevelExp CalcMasterSkillLevelExp(int skillId, List<ItemInput> useItemList)
	{
		DataManagerMasterSkill.NextCalcSkillLevelExp nextCalcSkillLevelExp = new DataManagerMasterSkill.NextCalcSkillLevelExp(skillId);
		foreach (ItemInput itemInput in useItemList)
		{
			if (this.LevelItemDataMap.ContainsKey(itemInput.itemId))
			{
				nextCalcSkillLevelExp.AddExp += this.LevelItemDataMap[itemInput.itemId].Exp * (long)itemInput.num;
			}
		}
		DataManagerMasterSkill.SkillData skillData = this.SkillDataList.Find((DataManagerMasterSkill.SkillData x) => x.SkillId == skillId);
		DataManagerMasterSkill.LevelData levelData = new DataManagerMasterSkill.LevelData(0, 0, 0L);
		foreach (DataManagerMasterSkill.LevelData levelData2 in this.LevelDataMap[skillData.LevelId])
		{
			if (nextCalcSkillLevelExp.SumExp < levelData2.Exp)
			{
				nextCalcSkillLevelExp.AfterLevel = levelData2.Level - 1;
				nextCalcSkillLevelExp.Numerator = nextCalcSkillLevelExp.SumExp - levelData.Exp;
				nextCalcSkillLevelExp.Denominator = levelData2.Exp - levelData.Exp;
				break;
			}
			if (levelData2.Exp <= nextCalcSkillLevelExp.SumExp)
			{
				nextCalcSkillLevelExp.AfterLevel = levelData2.Level;
			}
			levelData = levelData2;
		}
		return nextCalcSkillLevelExp;
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x00026458 File Offset: 0x00024658
	public DataManagerMasterSkill.SkillData GetSkillData(int skillId)
	{
		return this.SkillDataList.Find((DataManagerMasterSkill.SkillData x) => x.SkillId == skillId);
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x0002648D File Offset: 0x0002468D
	public List<DataManagerMasterSkill.LevelData> GetLevenDataList(int lvId)
	{
		if (this.LevelDataMap.ContainsKey(lvId))
		{
			return this.LevelDataMap[lvId];
		}
		return new List<DataManagerMasterSkill.LevelData>();
	}

	// Token: 0x040005A3 RID: 1443
	private DataManager parentData;

	// Token: 0x020006E7 RID: 1767
	public class MasterSkillData
	{
		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06003377 RID: 13175 RVA: 0x001C1DC8 File Offset: 0x001BFFC8
		// (set) Token: 0x06003378 RID: 13176 RVA: 0x001C1DD0 File Offset: 0x001BFFD0
		public int SkillId { get; private set; }

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06003379 RID: 13177 RVA: 0x001C1DD9 File Offset: 0x001BFFD9
		// (set) Token: 0x0600337A RID: 13178 RVA: 0x001C1DE1 File Offset: 0x001BFFE1
		public int Level { get; set; }

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x0600337B RID: 13179 RVA: 0x001C1DEA File Offset: 0x001BFFEA
		// (set) Token: 0x0600337C RID: 13180 RVA: 0x001C1DF2 File Offset: 0x001BFFF2
		public long Exp { get; set; }

		// Token: 0x0600337D RID: 13181 RVA: 0x001C1DFB File Offset: 0x001BFFFB
		public MasterSkillData(int skillId, int lv, long exp)
		{
			this.SkillId = skillId;
			this.Level = lv;
			this.Exp = exp;
		}
	}

	// Token: 0x020006E8 RID: 1768
	public class SkillData
	{
		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x0600337E RID: 13182 RVA: 0x001C1E18 File Offset: 0x001C0018
		// (set) Token: 0x0600337F RID: 13183 RVA: 0x001C1E20 File Offset: 0x001C0020
		public int SkillId { get; private set; }

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06003380 RID: 13184 RVA: 0x001C1E29 File Offset: 0x001C0029
		// (set) Token: 0x06003381 RID: 13185 RVA: 0x001C1E31 File Offset: 0x001C0031
		public int LevelId { get; private set; }

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06003382 RID: 13186 RVA: 0x001C1E3A File Offset: 0x001C003A
		// (set) Token: 0x06003383 RID: 13187 RVA: 0x001C1E42 File Offset: 0x001C0042
		public int LevelMax { get; set; }

		// Token: 0x06003384 RID: 13188 RVA: 0x001C1E4B File Offset: 0x001C004B
		public SkillData(MstMasterSkillData skillData)
		{
			this.SkillId = skillData.id;
			this.LevelId = skillData.levelId;
		}
	}

	// Token: 0x020006E9 RID: 1769
	public class LevelData
	{
		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06003385 RID: 13189 RVA: 0x001C1E6B File Offset: 0x001C006B
		// (set) Token: 0x06003386 RID: 13190 RVA: 0x001C1E73 File Offset: 0x001C0073
		public int LevelId { get; private set; }

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06003387 RID: 13191 RVA: 0x001C1E7C File Offset: 0x001C007C
		// (set) Token: 0x06003388 RID: 13192 RVA: 0x001C1E84 File Offset: 0x001C0084
		public int Level { get; private set; }

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06003389 RID: 13193 RVA: 0x001C1E8D File Offset: 0x001C008D
		// (set) Token: 0x0600338A RID: 13194 RVA: 0x001C1E95 File Offset: 0x001C0095
		public long Exp { get; private set; }

		// Token: 0x0600338B RID: 13195 RVA: 0x001C1E9E File Offset: 0x001C009E
		public LevelData(int lvId, int lv, long exp)
		{
			this.LevelId = lvId;
			this.Level = lv;
			this.Exp = exp;
		}
	}

	// Token: 0x020006EA RID: 1770
	public class LevelItemData
	{
		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x0600338C RID: 13196 RVA: 0x001C1EBB File Offset: 0x001C00BB
		// (set) Token: 0x0600338D RID: 13197 RVA: 0x001C1EC3 File Offset: 0x001C00C3
		public int ItemId { get; private set; }

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x0600338E RID: 13198 RVA: 0x001C1ECC File Offset: 0x001C00CC
		// (set) Token: 0x0600338F RID: 13199 RVA: 0x001C1ED4 File Offset: 0x001C00D4
		public int CoinNum { get; private set; }

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06003390 RID: 13200 RVA: 0x001C1EDD File Offset: 0x001C00DD
		// (set) Token: 0x06003391 RID: 13201 RVA: 0x001C1EE5 File Offset: 0x001C00E5
		public long Exp { get; private set; }

		// Token: 0x06003392 RID: 13202 RVA: 0x001C1EEE File Offset: 0x001C00EE
		public LevelItemData(MstMasterSkillLevelItem mst)
		{
			this.ItemId = mst.itemId;
			this.CoinNum = mst.strengCoinNum;
			this.Exp = mst.exp;
		}
	}

	// Token: 0x020006EB RID: 1771
	public class NextCalcSkillLevelExp
	{
		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06003393 RID: 13203 RVA: 0x001C1F1A File Offset: 0x001C011A
		// (set) Token: 0x06003394 RID: 13204 RVA: 0x001C1F22 File Offset: 0x001C0122
		public int SkillId { get; private set; }

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06003395 RID: 13205 RVA: 0x001C1F2B File Offset: 0x001C012B
		// (set) Token: 0x06003396 RID: 13206 RVA: 0x001C1F33 File Offset: 0x001C0133
		public int BaseLevel { get; private set; }

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06003397 RID: 13207 RVA: 0x001C1F3C File Offset: 0x001C013C
		// (set) Token: 0x06003398 RID: 13208 RVA: 0x001C1F44 File Offset: 0x001C0144
		public int AfterLevel { get; set; }

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06003399 RID: 13209 RVA: 0x001C1F4D File Offset: 0x001C014D
		// (set) Token: 0x0600339A RID: 13210 RVA: 0x001C1F55 File Offset: 0x001C0155
		public long Numerator { get; set; }

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x0600339B RID: 13211 RVA: 0x001C1F5E File Offset: 0x001C015E
		// (set) Token: 0x0600339C RID: 13212 RVA: 0x001C1F66 File Offset: 0x001C0166
		public long Denominator { get; set; }

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x0600339D RID: 13213 RVA: 0x001C1F6F File Offset: 0x001C016F
		public long RequiredExp
		{
			get
			{
				return this.Denominator - this.Numerator;
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x0600339E RID: 13214 RVA: 0x001C1F7E File Offset: 0x001C017E
		// (set) Token: 0x0600339F RID: 13215 RVA: 0x001C1F86 File Offset: 0x001C0186
		public long BaseExp { get; private set; }

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x060033A0 RID: 13216 RVA: 0x001C1F8F File Offset: 0x001C018F
		// (set) Token: 0x060033A1 RID: 13217 RVA: 0x001C1F97 File Offset: 0x001C0197
		public long AddExp { get; set; }

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x060033A2 RID: 13218 RVA: 0x001C1FA0 File Offset: 0x001C01A0
		public long SumExp
		{
			get
			{
				return this.BaseExp + this.AddExp;
			}
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x001C1FB0 File Offset: 0x001C01B0
		public NextCalcSkillLevelExp(int skillId)
		{
			this.SkillId = skillId;
			DataManagerMasterSkill.MasterSkillData masterSkillData = DataManager.DmMasterSkill.UserMasterSkillDataList.Find((DataManagerMasterSkill.MasterSkillData x) => x.SkillId == this.SkillId);
			this.BaseLevel = masterSkillData.Level;
			this.BaseExp = masterSkillData.Exp;
		}
	}
}
