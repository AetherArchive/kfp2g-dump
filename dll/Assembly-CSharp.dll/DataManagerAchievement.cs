using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

public class DataManagerAchievement
{
	public DataManagerAchievement.FILTER currentFilter { get; set; }

	public DataManagerAchievement(DataManager p)
	{
		this.parentData = p;
		this.currentFilter = DataManagerAchievement.FILTER.ALL;
	}

	public void InitializeMstData(MstManager mst)
	{
		this.haveAchievementDataMap = new Dictionary<int, DataManagerAchievement.AchievementData>();
		this.allAchievementDataMap = new Dictionary<int, DataManagerAchievement.AchievementStaticData>();
		List<MstAchievementData> mst2 = mst.GetMst<List<MstAchievementData>>(MstType.ACHIEVEMENT_DATA);
		List<ItemStaticBase> list = new List<ItemStaticBase>();
		if (mst2 != null)
		{
			foreach (MstAchievementData mstAchievementData in mst2)
			{
				DataManagerAchievement.AchievementStaticData achievementStaticData = new DataManagerAchievement.AchievementStaticData(mstAchievementData);
				this.allAchievementDataMap.Add(mstAchievementData.id, achievementStaticData);
				list.Add(achievementStaticData);
			}
		}
		DataManager.DmItem.AddMstDataByItem(list);
	}

	public void UpdateUserDataByServer(List<Achievement> serverDataList)
	{
		this.latestAcquiredAchievementIdList.Clear();
		foreach (Achievement achievement in serverDataList)
		{
			int achievement_id = achievement.achievement_id;
			DataManagerAchievement.AchievementDynamicData achievementDynamicData = new DataManagerAchievement.AchievementDynamicData(achievement);
			DataManagerAchievement.AchievementData achievementData = new DataManagerAchievement.AchievementData(this.allAchievementDataMap[achievement_id], achievementDynamicData);
			if (this.haveAchievementDataMap.ContainsKey(achievement_id))
			{
				this.haveAchievementDataMap[achievement_id] = achievementData;
			}
			else
			{
				this.haveAchievementDataMap.Add(achievement_id, achievementData);
				this.latestAcquiredAchievementIdList.Add(achievement_id);
			}
		}
	}

	public DataManagerAchievement.AchievementStaticData GetAchievementData(int id)
	{
		if (this.allAchievementDataMap.ContainsKey(id))
		{
			return this.allAchievementDataMap[id];
		}
		return null;
	}

	public Dictionary<int, DataManagerAchievement.AchievementStaticData> GetAllAchievementData()
	{
		return this.allAchievementDataMap;
	}

	public DataManagerAchievement.AchievementData GetHaveAchievementData(int id)
	{
		if (this.haveAchievementDataMap.ContainsKey(id))
		{
			return this.haveAchievementDataMap[id];
		}
		return null;
	}

	public List<DataManagerAchievement.AchievementStaticData> GetHaveAchievementDataList()
	{
		List<DataManagerAchievement.AchievementStaticData> list = new List<DataManagerAchievement.AchievementStaticData>();
		foreach (DataManagerAchievement.AchievementStaticData achievementStaticData in this.GetShowDataList())
		{
			if (this.GetHaveAchievementData(achievementStaticData.id) != null)
			{
				list.Add(achievementStaticData);
			}
		}
		return list;
	}

	public List<DataManagerAchievement.AchievementStaticData> GetNotHaveAchievementDataList()
	{
		List<DataManagerAchievement.AchievementStaticData> list = new List<DataManagerAchievement.AchievementStaticData>();
		foreach (DataManagerAchievement.AchievementStaticData achievementStaticData in this.GetShowDataList())
		{
			if (this.GetHaveAchievementData(achievementStaticData.id) == null)
			{
				list.Add(achievementStaticData);
			}
		}
		return list;
	}

	public DataManagerAchievement.AchievementData GetSelectData()
	{
		foreach (DataManagerAchievement.AchievementData achievementData in this.haveAchievementDataMap.Values)
		{
			if (achievementData.dynamicData.isSelectFlag)
			{
				return achievementData;
			}
		}
		return null;
	}

	public List<DataManagerAchievement.AchievementStaticData> GetShowDataList()
	{
		List<DataManagerAchievement.AchievementStaticData> list = new List<DataManagerAchievement.AchievementStaticData>();
		foreach (KeyValuePair<int, DataManagerAchievement.AchievementStaticData> keyValuePair in this.allAchievementDataMap.OrderBy<KeyValuePair<int, DataManagerAchievement.AchievementStaticData>, int>((KeyValuePair<int, DataManagerAchievement.AchievementStaticData> x) => x.Value.sortId))
		{
			if (this.haveAchievementDataMap.ContainsKey(keyValuePair.Key))
			{
				list.Add(keyValuePair.Value);
			}
			else if (!(TimeManager.Now < keyValuePair.Value.startTime) && keyValuePair.Value.hideFlag == 0)
			{
				list.Add(keyValuePair.Value);
			}
		}
		return list;
	}

	public List<int> GetLatestAcquiredAchievementIdList()
	{
		return this.latestAcquiredAchievementIdList;
	}

	public void RequestActionSelectFlag(int achievementId)
	{
		this.parentData.ServerRequest(AchievementSelectFlagCmd.Create(achievementId), new Action<Command>(this.AchievementSelectFlag));
	}

	public void RequestActionNewFlag(int achievementId)
	{
		this.parentData.ServerRequest(AchievementNewFlagCmd.Create(achievementId), new Action<Command>(this.AchievementNewFlag));
	}

	private void AchievementSelectFlag(Command cmd)
	{
		AchievementSelectFlagResponse achievementSelectFlagResponse = cmd.response as AchievementSelectFlagResponse;
		this.parentData.UpdateUserAssetByAssets(achievementSelectFlagResponse.assets);
	}

	private void AchievementNewFlag(Command cmd)
	{
		AchievementNewFlagResponse achievementNewFlagResponse = cmd.response as AchievementNewFlagResponse;
		this.parentData.UpdateUserAssetByAssets(achievementNewFlagResponse.assets);
	}

	public void RequestAchievementNewAcquisition()
	{
		this.parentData.ServerRequest(AchievementNewAcquisitionCmd.Create(), new Action<Command>(this.CbAchievementNewAcquisition));
	}

	private void CbAchievementNewAcquisition(Command cmd)
	{
		AchievementNewAcquisitionResponse achievementNewAcquisitionResponse = cmd.response as AchievementNewAcquisitionResponse;
		this.parentData.UpdateUserAssetByAssets(achievementNewAcquisitionResponse.assets);
	}

	private DataManager parentData;

	private Dictionary<int, DataManagerAchievement.AchievementStaticData> allAchievementDataMap = new Dictionary<int, DataManagerAchievement.AchievementStaticData>();

	private Dictionary<int, DataManagerAchievement.AchievementData> haveAchievementDataMap = new Dictionary<int, DataManagerAchievement.AchievementData>();

	private List<int> latestAcquiredAchievementIdList = new List<int>();

	public class AchievementDynamicData
	{
		public AchievementDynamicData(Achievement data)
		{
			this.achivementId = data.achievement_id;
			this.isSelectFlag = data.select_flag == 1;
			this.isNewFlag = data.new_flag == 1;
			this.insertTime = new DateTime(PrjUtil.ConvertTimeToTicks(data.insert_time));
			this.updateTime = new DateTime(PrjUtil.ConvertTimeToTicks(data.update_time));
		}

		public int achivementId;

		public bool isSelectFlag;

		public bool isNewFlag;

		public DateTime insertTime;

		public DateTime updateTime;
	}

	public class AchievementStaticData : ItemStaticBase
	{
		public AchievementStaticData(MstAchievementData mstData)
		{
			this.id = mstData.id;
			this.rarity = mstData.rarity;
			this.name = mstData.name;
			this.flavorText = mstData.flavorText;
			this.achievementName = mstData.achievementName;
			this.iconName = mstData.iconName;
			this.achievementIcon = mstData.achievementIcon;
			this.achievementBg = mstData.achievementBg;
			this.achievementFrame = mstData.achievementFrame;
			this.duplicateItemId = mstData.duplicateItemId;
			this.duplicateItemNum = mstData.duplicateItemNum;
			this.sortId = mstData.sortId;
			this.infoGettext = mstData.infoGettext ?? "";
			this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(mstData.startTime));
			this.hideFlag = mstData.hideFlag;
		}

		public override int GetId()
		{
			return this.id;
		}

		public override ItemDef.Kind GetKind()
		{
			return ItemDef.Kind.ACHIEVEMENT;
		}

		public override string GetName()
		{
			return this.name;
		}

		public override string GetInfo()
		{
			return this.flavorText;
		}

		public override ItemDef.Rarity GetRarity()
		{
			return (ItemDef.Rarity)this.rarity;
		}

		public override int GetStackMax()
		{
			return 99999999;
		}

		public override string GetIconName()
		{
			return "Texture2D/Icon_Item/" + this.iconName;
		}

		public override int GetSalePrice()
		{
			return 0;
		}

		public int id;

		public int rarity;

		public string name;

		public string flavorText;

		public string achievementName;

		public string iconName;

		public string achievementIcon;

		public string achievementBg;

		public string achievementFrame;

		public int duplicateItemId;

		public int duplicateItemNum;

		public int sortId;

		public string infoGettext;

		public DateTime startTime;

		public int hideFlag;
	}

	public class AchievementData
	{
		public AchievementData(DataManagerAchievement.AchievementStaticData staticData, DataManagerAchievement.AchievementDynamicData dynamicData)
		{
			this.staticData = staticData;
			this.dynamicData = dynamicData;
		}

		public DataManagerAchievement.AchievementStaticData staticData;

		public DataManagerAchievement.AchievementDynamicData dynamicData;
	}

	public enum FILTER
	{
		ALL,
		HAVE,
		NOT_HAVE
	}
}
