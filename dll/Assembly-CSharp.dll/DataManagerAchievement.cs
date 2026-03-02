using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x0200006D RID: 109
public class DataManagerAchievement
{
	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x06000302 RID: 770 RVA: 0x0001766E File Offset: 0x0001586E
	// (set) Token: 0x06000303 RID: 771 RVA: 0x00017676 File Offset: 0x00015876
	public DataManagerAchievement.FILTER currentFilter { get; set; }

	// Token: 0x06000304 RID: 772 RVA: 0x0001767F File Offset: 0x0001587F
	public DataManagerAchievement(DataManager p)
	{
		this.parentData = p;
		this.currentFilter = DataManagerAchievement.FILTER.ALL;
	}

	// Token: 0x06000305 RID: 773 RVA: 0x000176B8 File Offset: 0x000158B8
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

	// Token: 0x06000306 RID: 774 RVA: 0x00017758 File Offset: 0x00015958
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

	// Token: 0x06000307 RID: 775 RVA: 0x00017800 File Offset: 0x00015A00
	public DataManagerAchievement.AchievementStaticData GetAchievementData(int id)
	{
		if (this.allAchievementDataMap.ContainsKey(id))
		{
			return this.allAchievementDataMap[id];
		}
		return null;
	}

	// Token: 0x06000308 RID: 776 RVA: 0x0001781E File Offset: 0x00015A1E
	public Dictionary<int, DataManagerAchievement.AchievementStaticData> GetAllAchievementData()
	{
		return this.allAchievementDataMap;
	}

	// Token: 0x06000309 RID: 777 RVA: 0x00017826 File Offset: 0x00015A26
	public DataManagerAchievement.AchievementData GetHaveAchievementData(int id)
	{
		if (this.haveAchievementDataMap.ContainsKey(id))
		{
			return this.haveAchievementDataMap[id];
		}
		return null;
	}

	// Token: 0x0600030A RID: 778 RVA: 0x00017844 File Offset: 0x00015A44
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

	// Token: 0x0600030B RID: 779 RVA: 0x000178AC File Offset: 0x00015AAC
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

	// Token: 0x0600030C RID: 780 RVA: 0x00017914 File Offset: 0x00015B14
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

	// Token: 0x0600030D RID: 781 RVA: 0x0001797C File Offset: 0x00015B7C
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

	// Token: 0x0600030E RID: 782 RVA: 0x00017A44 File Offset: 0x00015C44
	public List<int> GetLatestAcquiredAchievementIdList()
	{
		return this.latestAcquiredAchievementIdList;
	}

	// Token: 0x0600030F RID: 783 RVA: 0x00017A4C File Offset: 0x00015C4C
	public void RequestActionSelectFlag(int achievementId)
	{
		this.parentData.ServerRequest(AchievementSelectFlagCmd.Create(achievementId), new Action<Command>(this.AchievementSelectFlag));
	}

	// Token: 0x06000310 RID: 784 RVA: 0x00017A6B File Offset: 0x00015C6B
	public void RequestActionNewFlag(int achievementId)
	{
		this.parentData.ServerRequest(AchievementNewFlagCmd.Create(achievementId), new Action<Command>(this.AchievementNewFlag));
	}

	// Token: 0x06000311 RID: 785 RVA: 0x00017A8C File Offset: 0x00015C8C
	private void AchievementSelectFlag(Command cmd)
	{
		AchievementSelectFlagResponse achievementSelectFlagResponse = cmd.response as AchievementSelectFlagResponse;
		this.parentData.UpdateUserAssetByAssets(achievementSelectFlagResponse.assets);
	}

	// Token: 0x06000312 RID: 786 RVA: 0x00017AB8 File Offset: 0x00015CB8
	private void AchievementNewFlag(Command cmd)
	{
		AchievementNewFlagResponse achievementNewFlagResponse = cmd.response as AchievementNewFlagResponse;
		this.parentData.UpdateUserAssetByAssets(achievementNewFlagResponse.assets);
	}

	// Token: 0x06000313 RID: 787 RVA: 0x00017AE2 File Offset: 0x00015CE2
	public void RequestAchievementNewAcquisition()
	{
		this.parentData.ServerRequest(AchievementNewAcquisitionCmd.Create(), new Action<Command>(this.CbAchievementNewAcquisition));
	}

	// Token: 0x06000314 RID: 788 RVA: 0x00017B00 File Offset: 0x00015D00
	private void CbAchievementNewAcquisition(Command cmd)
	{
		AchievementNewAcquisitionResponse achievementNewAcquisitionResponse = cmd.response as AchievementNewAcquisitionResponse;
		this.parentData.UpdateUserAssetByAssets(achievementNewAcquisitionResponse.assets);
	}

	// Token: 0x0400049A RID: 1178
	private DataManager parentData;

	// Token: 0x0400049B RID: 1179
	private Dictionary<int, DataManagerAchievement.AchievementStaticData> allAchievementDataMap = new Dictionary<int, DataManagerAchievement.AchievementStaticData>();

	// Token: 0x0400049C RID: 1180
	private Dictionary<int, DataManagerAchievement.AchievementData> haveAchievementDataMap = new Dictionary<int, DataManagerAchievement.AchievementData>();

	// Token: 0x0400049D RID: 1181
	private List<int> latestAcquiredAchievementIdList = new List<int>();

	// Token: 0x02000627 RID: 1575
	public class AchievementDynamicData
	{
		// Token: 0x06003006 RID: 12294 RVA: 0x001BA16C File Offset: 0x001B836C
		public AchievementDynamicData(Achievement data)
		{
			this.achivementId = data.achievement_id;
			this.isSelectFlag = data.select_flag == 1;
			this.isNewFlag = data.new_flag == 1;
			this.insertTime = new DateTime(PrjUtil.ConvertTimeToTicks(data.insert_time));
			this.updateTime = new DateTime(PrjUtil.ConvertTimeToTicks(data.update_time));
		}

		// Token: 0x04002DBE RID: 11710
		public int achivementId;

		// Token: 0x04002DBF RID: 11711
		public bool isSelectFlag;

		// Token: 0x04002DC0 RID: 11712
		public bool isNewFlag;

		// Token: 0x04002DC1 RID: 11713
		public DateTime insertTime;

		// Token: 0x04002DC2 RID: 11714
		public DateTime updateTime;
	}

	// Token: 0x02000628 RID: 1576
	public class AchievementStaticData : ItemStaticBase
	{
		// Token: 0x06003007 RID: 12295 RVA: 0x001BA1D8 File Offset: 0x001B83D8
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

		// Token: 0x06003008 RID: 12296 RVA: 0x001BA2B2 File Offset: 0x001B84B2
		public override int GetId()
		{
			return this.id;
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x001BA2BA File Offset: 0x001B84BA
		public override ItemDef.Kind GetKind()
		{
			return ItemDef.Kind.ACHIEVEMENT;
		}

		// Token: 0x0600300A RID: 12298 RVA: 0x001BA2BE File Offset: 0x001B84BE
		public override string GetName()
		{
			return this.name;
		}

		// Token: 0x0600300B RID: 12299 RVA: 0x001BA2C6 File Offset: 0x001B84C6
		public override string GetInfo()
		{
			return this.flavorText;
		}

		// Token: 0x0600300C RID: 12300 RVA: 0x001BA2CE File Offset: 0x001B84CE
		public override ItemDef.Rarity GetRarity()
		{
			return (ItemDef.Rarity)this.rarity;
		}

		// Token: 0x0600300D RID: 12301 RVA: 0x001BA2D6 File Offset: 0x001B84D6
		public override int GetStackMax()
		{
			return 99999999;
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x001BA2DD File Offset: 0x001B84DD
		public override string GetIconName()
		{
			return "Texture2D/Icon_Item/" + this.iconName;
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x001BA2EF File Offset: 0x001B84EF
		public override int GetSalePrice()
		{
			return 0;
		}

		// Token: 0x04002DC3 RID: 11715
		public int id;

		// Token: 0x04002DC4 RID: 11716
		public int rarity;

		// Token: 0x04002DC5 RID: 11717
		public string name;

		// Token: 0x04002DC6 RID: 11718
		public string flavorText;

		// Token: 0x04002DC7 RID: 11719
		public string achievementName;

		// Token: 0x04002DC8 RID: 11720
		public string iconName;

		// Token: 0x04002DC9 RID: 11721
		public string achievementIcon;

		// Token: 0x04002DCA RID: 11722
		public string achievementBg;

		// Token: 0x04002DCB RID: 11723
		public string achievementFrame;

		// Token: 0x04002DCC RID: 11724
		public int duplicateItemId;

		// Token: 0x04002DCD RID: 11725
		public int duplicateItemNum;

		// Token: 0x04002DCE RID: 11726
		public int sortId;

		// Token: 0x04002DCF RID: 11727
		public string infoGettext;

		// Token: 0x04002DD0 RID: 11728
		public DateTime startTime;

		// Token: 0x04002DD1 RID: 11729
		public int hideFlag;
	}

	// Token: 0x02000629 RID: 1577
	public class AchievementData
	{
		// Token: 0x06003010 RID: 12304 RVA: 0x001BA2F2 File Offset: 0x001B84F2
		public AchievementData(DataManagerAchievement.AchievementStaticData staticData, DataManagerAchievement.AchievementDynamicData dynamicData)
		{
			this.staticData = staticData;
			this.dynamicData = dynamicData;
		}

		// Token: 0x04002DD2 RID: 11730
		public DataManagerAchievement.AchievementStaticData staticData;

		// Token: 0x04002DD3 RID: 11731
		public DataManagerAchievement.AchievementDynamicData dynamicData;
	}

	// Token: 0x0200062A RID: 1578
	public enum FILTER
	{
		// Token: 0x04002DD5 RID: 11733
		ALL,
		// Token: 0x04002DD6 RID: 11734
		HAVE,
		// Token: 0x04002DD7 RID: 11735
		NOT_HAVE
	}
}
