using System;
using System.Collections.Generic;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

public class DataManagerMasterSkill
{
	public DataManagerMasterSkill(DataManager p)
	{
		this.parentData = p;
	}

	public List<DataManagerMasterSkill.MasterSkillData> UserMasterSkillDataList { get; private set; }

	public MasterSkillGrowResult LatestMasterSkillGrowResult { get; private set; }

	private List<DataManagerMasterSkill.SkillData> SkillDataList { get; set; }

	private List<DataManagerMasterSkill.LevelData> LevelDataList { get; set; }

	private Dictionary<int, List<DataManagerMasterSkill.LevelData>> LevelDataMap { get; set; }

	private Dictionary<int, DataManagerMasterSkill.LevelItemData> LevelItemDataMap { get; set; }

	public void RequestActionMasterSkillGrow(int skillId, List<UseItem> useItemList)
	{
		this.parentData.ServerRequest(MasterSkillGrowCmd.Create(skillId, useItemList), new Action<Command>(this.CbMasterSkillGrowCmd));
	}

	private void CbMasterSkillGrowCmd(Command cmd)
	{
		MasterSkillGrowResponse masterSkillGrowResponse = cmd.response as MasterSkillGrowResponse;
		this.LatestMasterSkillGrowResult = masterSkillGrowResponse.level_result;
		this.parentData.UpdateUserAssetByAssets(masterSkillGrowResponse.assets);
	}

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

	public DataManagerMasterSkill.LevelItemData GetLevelItemData(int itemId)
	{
		if (this.LevelItemDataMap.ContainsKey(itemId))
		{
			return this.LevelItemDataMap[itemId];
		}
		return new DataManagerMasterSkill.LevelItemData(new MstMasterSkillLevelItem());
	}

	public List<DataManagerMasterSkill.LevelItemData> GetLevelItemDataList()
	{
		List<DataManagerMasterSkill.LevelItemData> list = new List<DataManagerMasterSkill.LevelItemData>();
		foreach (DataManagerMasterSkill.LevelItemData levelItemData in this.LevelItemDataMap.Values)
		{
			list.Add(levelItemData);
		}
		return list;
	}

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

	public DataManagerMasterSkill.SkillData GetSkillData(int skillId)
	{
		return this.SkillDataList.Find((DataManagerMasterSkill.SkillData x) => x.SkillId == skillId);
	}

	public List<DataManagerMasterSkill.LevelData> GetLevenDataList(int lvId)
	{
		if (this.LevelDataMap.ContainsKey(lvId))
		{
			return this.LevelDataMap[lvId];
		}
		return new List<DataManagerMasterSkill.LevelData>();
	}

	private DataManager parentData;

	public class MasterSkillData
	{
		public int SkillId { get; private set; }

		public int Level { get; set; }

		public long Exp { get; set; }

		public MasterSkillData(int skillId, int lv, long exp)
		{
			this.SkillId = skillId;
			this.Level = lv;
			this.Exp = exp;
		}
	}

	public class SkillData
	{
		public int SkillId { get; private set; }

		public int LevelId { get; private set; }

		public int LevelMax { get; set; }

		public SkillData(MstMasterSkillData skillData)
		{
			this.SkillId = skillData.id;
			this.LevelId = skillData.levelId;
		}
	}

	public class LevelData
	{
		public int LevelId { get; private set; }

		public int Level { get; private set; }

		public long Exp { get; private set; }

		public LevelData(int lvId, int lv, long exp)
		{
			this.LevelId = lvId;
			this.Level = lv;
			this.Exp = exp;
		}
	}

	public class LevelItemData
	{
		public int ItemId { get; private set; }

		public int CoinNum { get; private set; }

		public long Exp { get; private set; }

		public LevelItemData(MstMasterSkillLevelItem mst)
		{
			this.ItemId = mst.itemId;
			this.CoinNum = mst.strengCoinNum;
			this.Exp = mst.exp;
		}
	}

	public class NextCalcSkillLevelExp
	{
		public int SkillId { get; private set; }

		public int BaseLevel { get; private set; }

		public int AfterLevel { get; set; }

		public long Numerator { get; set; }

		public long Denominator { get; set; }

		public long RequiredExp
		{
			get
			{
				return this.Denominator - this.Numerator;
			}
		}

		public long BaseExp { get; private set; }

		public long AddExp { get; set; }

		public long SumExp
		{
			get
			{
				return this.BaseExp + this.AddExp;
			}
		}

		public NextCalcSkillLevelExp(int skillId)
		{
			this.SkillId = skillId;
			DataManagerMasterSkill.MasterSkillData masterSkillData = DataManager.DmMasterSkill.UserMasterSkillDataList.Find((DataManagerMasterSkill.MasterSkillData x) => x.SkillId == this.SkillId);
			this.BaseLevel = masterSkillData.Level;
			this.BaseExp = masterSkillData.Exp;
		}
	}
}
