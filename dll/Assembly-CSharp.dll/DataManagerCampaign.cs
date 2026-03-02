using System;
using System.Collections.Generic;
using SGNFW.Mst;

// Token: 0x02000070 RID: 112
public class DataManagerCampaign
{
	// Token: 0x0600032B RID: 811 RVA: 0x00017F72 File Offset: 0x00016172
	public DataManagerCampaign(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x0600032C RID: 812 RVA: 0x00017F81 File Offset: 0x00016181
	// (set) Token: 0x0600032D RID: 813 RVA: 0x00017F8E File Offset: 0x0001618E
	public List<DataManagerCampaign.CampaignGrowData> CampaignGrowCharaDataList
	{
		get
		{
			return new List<DataManagerCampaign.CampaignGrowData>(this.campaignGrowCharaDataList);
		}
		private set
		{
		}
	}

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x0600032E RID: 814 RVA: 0x00017F90 File Offset: 0x00016190
	// (set) Token: 0x0600032F RID: 815 RVA: 0x00017F9D File Offset: 0x0001619D
	public List<DataManagerCampaign.CampaignGrowData> CampaignGrowPhotoDataList
	{
		get
		{
			return new List<DataManagerCampaign.CampaignGrowData>(this.campaignGrowPhotoDataList);
		}
		private set
		{
		}
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x06000330 RID: 816 RVA: 0x00017F9F File Offset: 0x0001619F
	// (set) Token: 0x06000331 RID: 817 RVA: 0x00017FAC File Offset: 0x000161AC
	public List<DataManagerCampaign.CampaignItemDropData> CampaignItemDropDataList
	{
		get
		{
			return new List<DataManagerCampaign.CampaignItemDropData>(this.campaignItemDropDataList);
		}
		private set
		{
		}
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x06000332 RID: 818 RVA: 0x00017FAE File Offset: 0x000161AE
	// (set) Token: 0x06000333 RID: 819 RVA: 0x00017FBB File Offset: 0x000161BB
	public List<DataManagerCampaign.CampaignKizunaData> CampaignKizunaDataList
	{
		get
		{
			return new List<DataManagerCampaign.CampaignKizunaData>(this.campaignKizunaDataList);
		}
		private set
		{
		}
	}

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x06000334 RID: 820 RVA: 0x00017FBD File Offset: 0x000161BD
	// (set) Token: 0x06000335 RID: 821 RVA: 0x00017FCA File Offset: 0x000161CA
	public List<DataManagerCampaign.CampaignPvpCoinData> CampaignPvPDataList
	{
		get
		{
			return new List<DataManagerCampaign.CampaignPvpCoinData>(this.campaignPvPCoinDataList);
		}
		private set
		{
		}
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06000336 RID: 822 RVA: 0x00017FCC File Offset: 0x000161CC
	// (set) Token: 0x06000337 RID: 823 RVA: 0x00017FD9 File Offset: 0x000161D9
	public List<DataManagerCampaign.CampaignQuestStaminaData> CampaignQuestStaminaDataList
	{
		get
		{
			return new List<DataManagerCampaign.CampaignQuestStaminaData>(this.campaignQuestStaminaDataList);
		}
		private set
		{
		}
	}

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06000338 RID: 824 RVA: 0x00017FDB File Offset: 0x000161DB
	// (set) Token: 0x06000339 RID: 825 RVA: 0x00017FE8 File Offset: 0x000161E8
	public List<DataManagerCampaign.CampaignStaminaRecoveryData> CampaignStaminaRecoveryDataList
	{
		get
		{
			return new List<DataManagerCampaign.CampaignStaminaRecoveryData>(this.campaignStaminaRecoveryDataList);
		}
		private set
		{
		}
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x0600033A RID: 826 RVA: 0x00017FEC File Offset: 0x000161EC
	public DataManagerCampaign.CampaignGrowData PresentCampaignGrowCharaData
	{
		get
		{
			DataManagerCampaign.CampaignGrowData campaignGrowData = null;
			DateTime now = TimeManager.Now;
			foreach (DataManagerCampaign.CampaignGrowData campaignGrowData2 in this.campaignGrowCharaDataList)
			{
				if (!(now < campaignGrowData2.startDatetime) && !(campaignGrowData2.endDatetime < now))
				{
					campaignGrowData = campaignGrowData2;
					break;
				}
			}
			return campaignGrowData;
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x0600033B RID: 827 RVA: 0x00018064 File Offset: 0x00016264
	public DataManagerCampaign.CampaignGrowData PresentCampaignGrowPhotoData
	{
		get
		{
			DataManagerCampaign.CampaignGrowData campaignGrowData = null;
			DateTime now = TimeManager.Now;
			foreach (DataManagerCampaign.CampaignGrowData campaignGrowData2 in this.campaignGrowPhotoDataList)
			{
				if (!(now < campaignGrowData2.startDatetime) && !(campaignGrowData2.endDatetime < now))
				{
					campaignGrowData = campaignGrowData2;
					break;
				}
			}
			return campaignGrowData;
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x0600033C RID: 828 RVA: 0x000180DC File Offset: 0x000162DC
	public DataManagerCampaign.CampaignItemDropData PresentCampaignItemDropData
	{
		get
		{
			DataManagerCampaign.CampaignItemDropData campaignItemDropData = null;
			DateTime now = TimeManager.Now;
			foreach (DataManagerCampaign.CampaignItemDropData campaignItemDropData2 in this.campaignItemDropDataList)
			{
				if (!(now < campaignItemDropData2.startTime) && !(campaignItemDropData2.endTime < now))
				{
					campaignItemDropData = campaignItemDropData2;
					break;
				}
			}
			return campaignItemDropData;
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x0600033D RID: 829 RVA: 0x00018154 File Offset: 0x00016354
	public DataManagerCampaign.CampaignKizunaData PresentCampaignKizunaData
	{
		get
		{
			DataManagerCampaign.CampaignKizunaData campaignKizunaData = null;
			DateTime now = TimeManager.Now;
			foreach (DataManagerCampaign.CampaignKizunaData campaignKizunaData2 in this.campaignKizunaDataList)
			{
				if (!(now < campaignKizunaData2.startTime) && !(campaignKizunaData2.endTime < now))
				{
					campaignKizunaData = campaignKizunaData2;
					break;
				}
			}
			return campaignKizunaData;
		}
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x0600033E RID: 830 RVA: 0x000181CC File Offset: 0x000163CC
	public DataManagerCampaign.CampaignPvpCoinData PresentCampaignPvPCoinData
	{
		get
		{
			DataManagerCampaign.CampaignPvpCoinData campaignPvpCoinData = null;
			DateTime now = TimeManager.Now;
			foreach (DataManagerCampaign.CampaignPvpCoinData campaignPvpCoinData2 in this.campaignPvPCoinDataList)
			{
				if (!(now < campaignPvpCoinData2.startTime) && !(campaignPvpCoinData2.endTime < now))
				{
					campaignPvpCoinData = campaignPvpCoinData2;
					break;
				}
			}
			return campaignPvpCoinData;
		}
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x0600033F RID: 831 RVA: 0x00018244 File Offset: 0x00016444
	public List<DataManagerCampaign.CampaignQuestStaminaData> PresentCampaignQuestStaminaDataList
	{
		get
		{
			List<DataManagerCampaign.CampaignQuestStaminaData> list = new List<DataManagerCampaign.CampaignQuestStaminaData>();
			DateTime now = TimeManager.Now;
			foreach (DataManagerCampaign.CampaignQuestStaminaData campaignQuestStaminaData in this.campaignQuestStaminaDataList)
			{
				if (!(now < campaignQuestStaminaData.startTime) && !(campaignQuestStaminaData.endTime < now))
				{
					list.Add(campaignQuestStaminaData);
				}
			}
			list.Sort((DataManagerCampaign.CampaignQuestStaminaData a, DataManagerCampaign.CampaignQuestStaminaData b) => b.campaignId - a.campaignId);
			return list;
		}
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x06000340 RID: 832 RVA: 0x000182E8 File Offset: 0x000164E8
	public DataManagerCampaign.CampaignStaminaRecoveryData PresentCampaignStaminaRecoveryData
	{
		get
		{
			DataManagerCampaign.CampaignStaminaRecoveryData campaignStaminaRecoveryData = null;
			DateTime now = TimeManager.Now;
			foreach (DataManagerCampaign.CampaignStaminaRecoveryData campaignStaminaRecoveryData2 in this.campaignStaminaRecoveryDataList)
			{
				if (!(now < campaignStaminaRecoveryData2.startTime) && !(campaignStaminaRecoveryData2.endTime < now))
				{
					campaignStaminaRecoveryData = campaignStaminaRecoveryData2;
					break;
				}
			}
			return campaignStaminaRecoveryData;
		}
	}

	// Token: 0x06000341 RID: 833 RVA: 0x00018360 File Offset: 0x00016560
	public DataManagerCampaign.CampaignStaminaRecoveryData getBaseDateCampaignStaminaRecoveryData(long baseDateTicks)
	{
		DataManagerCampaign.CampaignStaminaRecoveryData campaignStaminaRecoveryData = null;
		foreach (DataManagerCampaign.CampaignStaminaRecoveryData campaignStaminaRecoveryData2 in this.campaignStaminaRecoveryDataList)
		{
			if (baseDateTicks >= campaignStaminaRecoveryData2.startTime.Ticks && campaignStaminaRecoveryData2.endTime.Ticks >= baseDateTicks)
			{
				campaignStaminaRecoveryData = campaignStaminaRecoveryData2;
				break;
			}
		}
		return campaignStaminaRecoveryData;
	}

	// Token: 0x06000342 RID: 834 RVA: 0x000183D8 File Offset: 0x000165D8
	public DataManagerCampaign.CampaignStaminaRecoveryData getBaseNextCampaignStaminaRecoveryData(long baseDateTicks)
	{
		DataManagerCampaign.CampaignStaminaRecoveryData campaignStaminaRecoveryData = null;
		if (this.getBaseDateCampaignStaminaRecoveryData(baseDateTicks) == null && this.campaignStaminaRecoveryDataList != null)
		{
			List<DataManagerCampaign.CampaignStaminaRecoveryData> list = new List<DataManagerCampaign.CampaignStaminaRecoveryData>(this.campaignStaminaRecoveryDataList);
			list.Reverse();
			foreach (DataManagerCampaign.CampaignStaminaRecoveryData campaignStaminaRecoveryData2 in list)
			{
				if (baseDateTicks < campaignStaminaRecoveryData2.startTime.Ticks)
				{
					campaignStaminaRecoveryData = campaignStaminaRecoveryData2;
					break;
				}
			}
		}
		return campaignStaminaRecoveryData;
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x06000343 RID: 835 RVA: 0x00018458 File Offset: 0x00016658
	public DataManagerCampaign.CampaignPicnicData PresentCampaignPicnicData
	{
		get
		{
			DataManagerCampaign.CampaignPicnicData campaignPicnicData = null;
			DateTime now = TimeManager.Now;
			foreach (DataManagerCampaign.CampaignPicnicData campaignPicnicData2 in this.campaignPicnicDataList)
			{
				if (!(now < campaignPicnicData2.startTime) && !(campaignPicnicData2.endTime < now))
				{
					campaignPicnicData = campaignPicnicData2;
					break;
				}
			}
			return campaignPicnicData;
		}
	}

	// Token: 0x06000344 RID: 836 RVA: 0x000184D0 File Offset: 0x000166D0
	public void InitializeMstData(MstManager mstManager)
	{
		List<MstCharaLotCampaignData> mst = mstManager.GetMst<List<MstCharaLotCampaignData>>(MstType.CHARA_LOT_CAMPAIGN_DATA);
		List<MstPhotoLotCampaignData> mst2 = mstManager.GetMst<List<MstPhotoLotCampaignData>>(MstType.PHOTO_LOT_CAMPAIGN_DATA);
		List<MstQuestCampaignItemData> mst3 = mstManager.GetMst<List<MstQuestCampaignItemData>>(MstType.QUEST_CAMPAIGN_ITEM_DATA);
		List<MstQuestCampaignItemmap> mst4 = mstManager.GetMst<List<MstQuestCampaignItemmap>>(MstType.QUEST_CAMPAIGN_ITEMMAP);
		List<MstQuestCampaignKizunaData> mst5 = mstManager.GetMst<List<MstQuestCampaignKizunaData>>(MstType.QUEST_CAMPAIGN_KIZUNA_DATA);
		List<MstQuestCampaignKizunamap> mst6 = mstManager.GetMst<List<MstQuestCampaignKizunamap>>(MstType.QUEST_CAMPAIGN_KIZUNAMAP);
		List<MstPvPCampaignCoinData> mst7 = mstManager.GetMst<List<MstPvPCampaignCoinData>>(MstType.PVP_CAMPAIGN_COIN_DATA);
		List<MstQuestCampaignStaminaData> mst8 = mstManager.GetMst<List<MstQuestCampaignStaminaData>>(MstType.QUEST_CAMPAIGN_STAMINA_DATA);
		List<MstStaminaRecoveryCampaignTimeData> mst9 = mstManager.GetMst<List<MstStaminaRecoveryCampaignTimeData>>(MstType.STAMINA_RECOVERY_CAMPAIGN_TIME_DATA);
		List<MstPicnicCampaignData> mst10 = mstManager.GetMst<List<MstPicnicCampaignData>>(MstType.PICNIC_CAMPAIGN_DATA);
		List<MstCharaKizunaLotCampaignData> mst11 = mstManager.GetMst<List<MstCharaKizunaLotCampaignData>>(MstType.CHARA_KIZUNA_LOT_CAMPAIGN_DATA);
		long num = PrjUtil.ConvertTicksToTime(TimeManager.Now.Ticks);
		this.campaignGrowCharaDataList = new List<DataManagerCampaign.CampaignGrowData>();
		foreach (MstCharaLotCampaignData mstCharaLotCampaignData in mst)
		{
			if (mstCharaLotCampaignData.endDatetime >= num)
			{
				DataManagerCampaign.CampaignGrowData campaignGrowData = new DataManagerCampaign.CampaignGrowData(mstCharaLotCampaignData);
				this.campaignGrowCharaDataList.Add(campaignGrowData);
			}
		}
		this.campaignGrowCharaDataList.Sort((DataManagerCampaign.CampaignGrowData a, DataManagerCampaign.CampaignGrowData b) => b.campaignId.CompareTo(a.campaignId));
		this.campaignGrowPhotoDataList = new List<DataManagerCampaign.CampaignGrowData>();
		foreach (MstPhotoLotCampaignData mstPhotoLotCampaignData in mst2)
		{
			if (mstPhotoLotCampaignData.endDatetime >= num)
			{
				DataManagerCampaign.CampaignGrowData campaignGrowData2 = new DataManagerCampaign.CampaignGrowData(mstPhotoLotCampaignData);
				this.campaignGrowPhotoDataList.Add(campaignGrowData2);
			}
		}
		this.campaignGrowPhotoDataList.Sort((DataManagerCampaign.CampaignGrowData a, DataManagerCampaign.CampaignGrowData b) => b.campaignId.CompareTo(a.campaignId));
		this.campaignKizunaDataList = new List<DataManagerCampaign.CampaignKizunaData>();
		using (List<MstQuestCampaignKizunaData>.Enumerator enumerator3 = mst5.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				MstQuestCampaignKizunaData campaignKizunaData = enumerator3.Current;
				if (campaignKizunaData.endTime >= num)
				{
					DataManagerCampaign.CampaignKizunaData campaignKizunaData2 = new DataManagerCampaign.CampaignKizunaData(campaignKizunaData);
					foreach (MstQuestCampaignKizunamap mstQuestCampaignKizunamap in mst6.FindAll((MstQuestCampaignKizunamap x) => x.campaignId == campaignKizunaData.campaignId))
					{
						DataManagerCampaign.CampaignTarget campaignTarget = new DataManagerCampaign.CampaignTarget(mstQuestCampaignKizunamap.id, mstQuestCampaignKizunamap.type);
						campaignKizunaData2.campaignTargetList.Add(campaignTarget);
					}
					this.campaignKizunaDataList.Add(campaignKizunaData2);
				}
			}
		}
		this.campaignKizunaDataList.Sort((DataManagerCampaign.CampaignKizunaData a, DataManagerCampaign.CampaignKizunaData b) => b.campaignId.CompareTo(a.campaignId));
		this.campaignItemDropDataList = new List<DataManagerCampaign.CampaignItemDropData>();
		using (List<MstQuestCampaignItemData>.Enumerator enumerator5 = mst3.GetEnumerator())
		{
			while (enumerator5.MoveNext())
			{
				MstQuestCampaignItemData campaignItemData = enumerator5.Current;
				if (campaignItemData.endTime >= num)
				{
					DataManagerCampaign.CampaignItemDropData campaignItemDropData = new DataManagerCampaign.CampaignItemDropData(campaignItemData);
					foreach (MstQuestCampaignItemmap mstQuestCampaignItemmap in mst4.FindAll((MstQuestCampaignItemmap x) => x.campaignId == campaignItemData.campaignId))
					{
						DataManagerCampaign.CampaignTarget campaignTarget2 = new DataManagerCampaign.CampaignTarget(mstQuestCampaignItemmap.id, mstQuestCampaignItemmap.type);
						campaignItemDropData.campaignTargetList.Add(campaignTarget2);
					}
					this.campaignItemDropDataList.Add(campaignItemDropData);
				}
			}
		}
		this.campaignItemDropDataList.Sort((DataManagerCampaign.CampaignItemDropData a, DataManagerCampaign.CampaignItemDropData b) => b.campaignId.CompareTo(a.campaignId));
		this.campaignPvPCoinDataList = new List<DataManagerCampaign.CampaignPvpCoinData>();
		foreach (MstPvPCampaignCoinData mstPvPCampaignCoinData in mst7)
		{
			if (mstPvPCampaignCoinData.endTime >= num)
			{
				DataManagerCampaign.CampaignPvpCoinData campaignPvpCoinData = new DataManagerCampaign.CampaignPvpCoinData(mstPvPCampaignCoinData);
				this.campaignPvPCoinDataList.Add(campaignPvpCoinData);
			}
		}
		this.campaignPvPCoinDataList.Sort((DataManagerCampaign.CampaignPvpCoinData a, DataManagerCampaign.CampaignPvpCoinData b) => b.campaignId.CompareTo(a.campaignId));
		this.campaignQuestStaminaDataList = new List<DataManagerCampaign.CampaignQuestStaminaData>();
		foreach (MstQuestCampaignStaminaData mstQuestCampaignStaminaData in mst8)
		{
			if (mstQuestCampaignStaminaData.endTime >= num)
			{
				DataManagerCampaign.CampaignQuestStaminaData campaignQuestStaminaData = new DataManagerCampaign.CampaignQuestStaminaData(mstQuestCampaignStaminaData);
				this.campaignQuestStaminaDataList.Add(campaignQuestStaminaData);
			}
		}
		this.campaignQuestStaminaDataList.Sort((DataManagerCampaign.CampaignQuestStaminaData a, DataManagerCampaign.CampaignQuestStaminaData b) => b.campaignId.CompareTo(a.campaignId));
		this.campaignStaminaRecoveryDataList = new List<DataManagerCampaign.CampaignStaminaRecoveryData>();
		foreach (MstStaminaRecoveryCampaignTimeData mstStaminaRecoveryCampaignTimeData in mst9)
		{
			if (mstStaminaRecoveryCampaignTimeData.endTime >= num)
			{
				DataManagerCampaign.CampaignStaminaRecoveryData campaignStaminaRecoveryData = new DataManagerCampaign.CampaignStaminaRecoveryData(mstStaminaRecoveryCampaignTimeData);
				this.campaignStaminaRecoveryDataList.Add(campaignStaminaRecoveryData);
			}
		}
		this.campaignStaminaRecoveryDataList.Sort((DataManagerCampaign.CampaignStaminaRecoveryData a, DataManagerCampaign.CampaignStaminaRecoveryData b) => b.campaignId.CompareTo(a.campaignId));
		this.campaignPicnicDataList = new List<DataManagerCampaign.CampaignPicnicData>();
		foreach (MstPicnicCampaignData mstPicnicCampaignData in mst10)
		{
			if (mstPicnicCampaignData.endTime >= num)
			{
				DataManagerCampaign.CampaignPicnicData campaignPicnicData = new DataManagerCampaign.CampaignPicnicData(mstPicnicCampaignData);
				this.campaignPicnicDataList.Add(campaignPicnicData);
			}
		}
		this.campaignPicnicDataList.Sort((DataManagerCampaign.CampaignPicnicData a, DataManagerCampaign.CampaignPicnicData b) => b.campaignId.CompareTo(a.campaignId));
		this.campaignGrowCharaKizunaDataList = new List<DataManagerCampaign.CampaignGrowData>();
		foreach (MstCharaKizunaLotCampaignData mstCharaKizunaLotCampaignData in mst11)
		{
			if (mstCharaKizunaLotCampaignData.endDatetime >= num)
			{
				DataManagerCampaign.CampaignGrowData campaignGrowData3 = new DataManagerCampaign.CampaignGrowData(mstCharaKizunaLotCampaignData);
				this.campaignGrowCharaKizunaDataList.Add(campaignGrowData3);
			}
		}
		this.campaignGrowCharaKizunaDataList.Sort((DataManagerCampaign.CampaignGrowData a, DataManagerCampaign.CampaignGrowData b) => b.campaignId.CompareTo(a.campaignId));
	}

	// Token: 0x040004A2 RID: 1186
	private DataManager parentData;

	// Token: 0x040004A3 RID: 1187
	private List<DataManagerCampaign.CampaignGrowData> campaignGrowCharaDataList;

	// Token: 0x040004A4 RID: 1188
	private List<DataManagerCampaign.CampaignGrowData> campaignGrowPhotoDataList;

	// Token: 0x040004A5 RID: 1189
	private List<DataManagerCampaign.CampaignItemDropData> campaignItemDropDataList;

	// Token: 0x040004A6 RID: 1190
	private List<DataManagerCampaign.CampaignKizunaData> campaignKizunaDataList;

	// Token: 0x040004A7 RID: 1191
	private List<DataManagerCampaign.CampaignPvpCoinData> campaignPvPCoinDataList;

	// Token: 0x040004A8 RID: 1192
	private List<DataManagerCampaign.CampaignQuestStaminaData> campaignQuestStaminaDataList;

	// Token: 0x040004A9 RID: 1193
	private List<DataManagerCampaign.CampaignStaminaRecoveryData> campaignStaminaRecoveryDataList;

	// Token: 0x040004AA RID: 1194
	private List<DataManagerCampaign.CampaignPicnicData> campaignPicnicDataList;

	// Token: 0x040004AB RID: 1195
	private List<DataManagerCampaign.CampaignGrowData> campaignGrowCharaKizunaDataList;

	// Token: 0x0200062D RID: 1581
	public class CampaignGrowData
	{
		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06003015 RID: 12309 RVA: 0x001BA362 File Offset: 0x001B8562
		// (set) Token: 0x06003016 RID: 12310 RVA: 0x001BA36A File Offset: 0x001B856A
		public int campaignId { get; private set; }

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06003017 RID: 12311 RVA: 0x001BA373 File Offset: 0x001B8573
		// (set) Token: 0x06003018 RID: 12312 RVA: 0x001BA37B File Offset: 0x001B857B
		public int successProbability { get; private set; }

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06003019 RID: 12313 RVA: 0x001BA384 File Offset: 0x001B8584
		// (set) Token: 0x0600301A RID: 12314 RVA: 0x001BA38C File Offset: 0x001B858C
		public int bigSuccessProbability { get; private set; }

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x0600301B RID: 12315 RVA: 0x001BA395 File Offset: 0x001B8595
		// (set) Token: 0x0600301C RID: 12316 RVA: 0x001BA39D File Offset: 0x001B859D
		public int greatSuccessProbability { get; private set; }

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x0600301D RID: 12317 RVA: 0x001BA3A6 File Offset: 0x001B85A6
		// (set) Token: 0x0600301E RID: 12318 RVA: 0x001BA3AE File Offset: 0x001B85AE
		public int successAcqRate { get; private set; }

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x0600301F RID: 12319 RVA: 0x001BA3B7 File Offset: 0x001B85B7
		// (set) Token: 0x06003020 RID: 12320 RVA: 0x001BA3BF File Offset: 0x001B85BF
		public int bigSuccessAcqRate { get; private set; }

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06003021 RID: 12321 RVA: 0x001BA3C8 File Offset: 0x001B85C8
		// (set) Token: 0x06003022 RID: 12322 RVA: 0x001BA3D0 File Offset: 0x001B85D0
		public int greatSuccessAcqRate { get; private set; }

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06003023 RID: 12323 RVA: 0x001BA3D9 File Offset: 0x001B85D9
		// (set) Token: 0x06003024 RID: 12324 RVA: 0x001BA3E1 File Offset: 0x001B85E1
		public bool IsDisplayCampaign { get; private set; }

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06003025 RID: 12325 RVA: 0x001BA3EA File Offset: 0x001B85EA
		// (set) Token: 0x06003026 RID: 12326 RVA: 0x001BA3F2 File Offset: 0x001B85F2
		public DateTime startDatetime { get; private set; }

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06003027 RID: 12327 RVA: 0x001BA3FB File Offset: 0x001B85FB
		// (set) Token: 0x06003028 RID: 12328 RVA: 0x001BA403 File Offset: 0x001B8603
		public DateTime endDatetime { get; private set; }

		// Token: 0x06003029 RID: 12329 RVA: 0x001BA40C File Offset: 0x001B860C
		public CampaignGrowData(MstCharaLotCampaignData chLotCampaignData)
		{
			this.campaignId = chLotCampaignData.campaignId;
			this.successProbability = chLotCampaignData.successProbability;
			this.bigSuccessProbability = chLotCampaignData.bigSuccessProbability;
			this.greatSuccessProbability = chLotCampaignData.greatSuccessProbability;
			this.successAcqRate = chLotCampaignData.successAcqRate;
			this.bigSuccessAcqRate = chLotCampaignData.bigSuccessAcqRate;
			this.greatSuccessAcqRate = chLotCampaignData.greatSuccessAcqRate;
			this.IsDisplayCampaign = this.campaignId != 1;
			this.startDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(chLotCampaignData.startDatetime));
			this.endDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(chLotCampaignData.endDatetime));
		}

		// Token: 0x0600302A RID: 12330 RVA: 0x001BA4B4 File Offset: 0x001B86B4
		public CampaignGrowData(MstPhotoLotCampaignData photoLotCampaignData)
		{
			this.campaignId = photoLotCampaignData.campaignId;
			this.successProbability = photoLotCampaignData.successProbability;
			this.bigSuccessProbability = photoLotCampaignData.bigSuccessProbability;
			this.greatSuccessProbability = photoLotCampaignData.greatSuccessProbability;
			this.successAcqRate = photoLotCampaignData.successAcqRate;
			this.bigSuccessAcqRate = photoLotCampaignData.bigSuccessAcqRate;
			this.greatSuccessAcqRate = photoLotCampaignData.greatSuccessAcqRate;
			this.IsDisplayCampaign = this.campaignId != 1;
			this.startDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(photoLotCampaignData.startDatetime));
			this.endDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(photoLotCampaignData.endDatetime));
		}

		// Token: 0x0600302B RID: 12331 RVA: 0x001BA55C File Offset: 0x001B875C
		public CampaignGrowData(MstCharaKizunaLotCampaignData kizunaLotCampaignData)
		{
			this.campaignId = kizunaLotCampaignData.campaignId;
			this.successProbability = kizunaLotCampaignData.successProbability;
			this.bigSuccessProbability = kizunaLotCampaignData.bigSuccessProbability;
			this.greatSuccessProbability = kizunaLotCampaignData.greatSuccessProbability;
			this.successAcqRate = kizunaLotCampaignData.successAcqRate;
			this.bigSuccessAcqRate = kizunaLotCampaignData.bigSuccessAcqRate;
			this.greatSuccessAcqRate = kizunaLotCampaignData.greatSuccessAcqRate;
			this.IsDisplayCampaign = this.campaignId != 1;
			this.startDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(kizunaLotCampaignData.startDatetime));
			this.endDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(kizunaLotCampaignData.endDatetime));
		}
	}

	// Token: 0x0200062E RID: 1582
	public class CampaignItemDropData
	{
		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x0600302C RID: 12332 RVA: 0x001BA601 File Offset: 0x001B8801
		// (set) Token: 0x0600302D RID: 12333 RVA: 0x001BA609 File Offset: 0x001B8809
		public int campaignId { get; private set; }

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x0600302E RID: 12334 RVA: 0x001BA612 File Offset: 0x001B8812
		// (set) Token: 0x0600302F RID: 12335 RVA: 0x001BA61A File Offset: 0x001B881A
		public int ratio { get; private set; }

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06003030 RID: 12336 RVA: 0x001BA623 File Offset: 0x001B8823
		// (set) Token: 0x06003031 RID: 12337 RVA: 0x001BA62B File Offset: 0x001B882B
		public List<DataManagerCampaign.CampaignTarget> campaignTargetList { get; private set; }

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06003032 RID: 12338 RVA: 0x001BA634 File Offset: 0x001B8834
		// (set) Token: 0x06003033 RID: 12339 RVA: 0x001BA63C File Offset: 0x001B883C
		public DateTime startTime { get; private set; }

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06003034 RID: 12340 RVA: 0x001BA645 File Offset: 0x001B8845
		// (set) Token: 0x06003035 RID: 12341 RVA: 0x001BA64D File Offset: 0x001B884D
		public DateTime endTime { get; private set; }

		// Token: 0x06003036 RID: 12342 RVA: 0x001BA658 File Offset: 0x001B8858
		public CampaignItemDropData(MstQuestCampaignItemData campaignItemData)
		{
			this.campaignId = campaignItemData.campaignId;
			this.ratio = campaignItemData.ratio;
			this.campaignTargetList = new List<DataManagerCampaign.CampaignTarget>();
			this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(campaignItemData.startTime));
			this.endTime = new DateTime(PrjUtil.ConvertTimeToTicks(campaignItemData.endTime));
		}
	}

	// Token: 0x0200062F RID: 1583
	public class CampaignKizunaData
	{
		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06003037 RID: 12343 RVA: 0x001BA6BA File Offset: 0x001B88BA
		// (set) Token: 0x06003038 RID: 12344 RVA: 0x001BA6C2 File Offset: 0x001B88C2
		public int campaignId { get; private set; }

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06003039 RID: 12345 RVA: 0x001BA6CB File Offset: 0x001B88CB
		// (set) Token: 0x0600303A RID: 12346 RVA: 0x001BA6D3 File Offset: 0x001B88D3
		public int ratio { get; private set; }

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x0600303B RID: 12347 RVA: 0x001BA6DC File Offset: 0x001B88DC
		// (set) Token: 0x0600303C RID: 12348 RVA: 0x001BA6E4 File Offset: 0x001B88E4
		public List<DataManagerCampaign.CampaignTarget> campaignTargetList { get; private set; }

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x0600303D RID: 12349 RVA: 0x001BA6ED File Offset: 0x001B88ED
		// (set) Token: 0x0600303E RID: 12350 RVA: 0x001BA6F5 File Offset: 0x001B88F5
		public DateTime startTime { get; private set; }

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x0600303F RID: 12351 RVA: 0x001BA6FE File Offset: 0x001B88FE
		// (set) Token: 0x06003040 RID: 12352 RVA: 0x001BA706 File Offset: 0x001B8906
		public DateTime endTime { get; private set; }

		// Token: 0x06003041 RID: 12353 RVA: 0x001BA710 File Offset: 0x001B8910
		public CampaignKizunaData(MstQuestCampaignKizunaData campaignKizunaData)
		{
			this.campaignId = campaignKizunaData.campaignId;
			this.ratio = campaignKizunaData.ratio;
			this.campaignTargetList = new List<DataManagerCampaign.CampaignTarget>();
			this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(campaignKizunaData.startTime));
			this.endTime = new DateTime(PrjUtil.ConvertTimeToTicks(campaignKizunaData.endTime));
		}
	}

	// Token: 0x02000630 RID: 1584
	public class CampaignPvpCoinData
	{
		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06003042 RID: 12354 RVA: 0x001BA772 File Offset: 0x001B8972
		// (set) Token: 0x06003043 RID: 12355 RVA: 0x001BA77A File Offset: 0x001B897A
		public int campaignId { get; private set; }

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06003044 RID: 12356 RVA: 0x001BA783 File Offset: 0x001B8983
		// (set) Token: 0x06003045 RID: 12357 RVA: 0x001BA78B File Offset: 0x001B898B
		public int pvpCoinRatio { get; private set; }

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06003046 RID: 12358 RVA: 0x001BA794 File Offset: 0x001B8994
		// (set) Token: 0x06003047 RID: 12359 RVA: 0x001BA79C File Offset: 0x001B899C
		public DateTime startTime { get; private set; }

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06003048 RID: 12360 RVA: 0x001BA7A5 File Offset: 0x001B89A5
		// (set) Token: 0x06003049 RID: 12361 RVA: 0x001BA7AD File Offset: 0x001B89AD
		public DateTime endTime { get; private set; }

		// Token: 0x0600304A RID: 12362 RVA: 0x001BA7B8 File Offset: 0x001B89B8
		public CampaignPvpCoinData(MstPvPCampaignCoinData pvpCampaignCoinData)
		{
			this.campaignId = pvpCampaignCoinData.campaignId;
			this.pvpCoinRatio = pvpCampaignCoinData.ratio;
			this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(pvpCampaignCoinData.startTime));
			this.endTime = new DateTime(PrjUtil.ConvertTimeToTicks(pvpCampaignCoinData.endTime));
		}
	}

	// Token: 0x02000631 RID: 1585
	public class CampaignQuestStaminaData
	{
		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x0600304B RID: 12363 RVA: 0x001BA80F File Offset: 0x001B8A0F
		// (set) Token: 0x0600304C RID: 12364 RVA: 0x001BA817 File Offset: 0x001B8A17
		public int campaignId { get; private set; }

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x0600304D RID: 12365 RVA: 0x001BA820 File Offset: 0x001B8A20
		// (set) Token: 0x0600304E RID: 12366 RVA: 0x001BA828 File Offset: 0x001B8A28
		public int value { get; private set; }

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x0600304F RID: 12367 RVA: 0x001BA831 File Offset: 0x001B8A31
		// (set) Token: 0x06003050 RID: 12368 RVA: 0x001BA839 File Offset: 0x001B8A39
		public List<DataManagerCampaign.CampaignTarget> campaignTargetList { get; private set; }

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06003051 RID: 12369 RVA: 0x001BA842 File Offset: 0x001B8A42
		// (set) Token: 0x06003052 RID: 12370 RVA: 0x001BA84A File Offset: 0x001B8A4A
		public DateTime startTime { get; private set; }

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06003053 RID: 12371 RVA: 0x001BA853 File Offset: 0x001B8A53
		// (set) Token: 0x06003054 RID: 12372 RVA: 0x001BA85B File Offset: 0x001B8A5B
		public DateTime endTime { get; private set; }

		// Token: 0x06003055 RID: 12373 RVA: 0x001BA864 File Offset: 0x001B8A64
		public CampaignQuestStaminaData(MstQuestCampaignStaminaData questCampaignstaminaData)
		{
			this.campaignId = questCampaignstaminaData.campaignId;
			this.value = questCampaignstaminaData.value;
			this.campaignTargetList = new List<DataManagerCampaign.CampaignTarget>
			{
				new DataManagerCampaign.CampaignTarget(questCampaignstaminaData.chapterId, 3)
			};
			this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(questCampaignstaminaData.startTime));
			this.endTime = new DateTime(PrjUtil.ConvertTimeToTicks(questCampaignstaminaData.endTime));
		}
	}

	// Token: 0x02000632 RID: 1586
	public class CampaignStaminaRecoveryData
	{
		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06003056 RID: 12374 RVA: 0x001BA8D8 File Offset: 0x001B8AD8
		// (set) Token: 0x06003057 RID: 12375 RVA: 0x001BA8E0 File Offset: 0x001B8AE0
		public int campaignId { get; private set; }

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06003058 RID: 12376 RVA: 0x001BA8E9 File Offset: 0x001B8AE9
		// (set) Token: 0x06003059 RID: 12377 RVA: 0x001BA8F1 File Offset: 0x001B8AF1
		public int staminaRecoveryTime { get; private set; }

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x0600305A RID: 12378 RVA: 0x001BA8FA File Offset: 0x001B8AFA
		// (set) Token: 0x0600305B RID: 12379 RVA: 0x001BA902 File Offset: 0x001B8B02
		public DateTime startTime { get; private set; }

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x0600305C RID: 12380 RVA: 0x001BA90B File Offset: 0x001B8B0B
		// (set) Token: 0x0600305D RID: 12381 RVA: 0x001BA913 File Offset: 0x001B8B13
		public DateTime endTime { get; private set; }

		// Token: 0x0600305E RID: 12382 RVA: 0x001BA91C File Offset: 0x001B8B1C
		public CampaignStaminaRecoveryData(MstStaminaRecoveryCampaignTimeData campaignStaminaRecoveryData)
		{
			this.campaignId = campaignStaminaRecoveryData.campaignId;
			this.staminaRecoveryTime = campaignStaminaRecoveryData.staminaRecoveryTime;
			this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(campaignStaminaRecoveryData.startTime));
			this.endTime = new DateTime(PrjUtil.ConvertTimeToTicks(campaignStaminaRecoveryData.endTime));
		}
	}

	// Token: 0x02000633 RID: 1587
	public class CampaignPicnicData
	{
		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x0600305F RID: 12383 RVA: 0x001BA973 File Offset: 0x001B8B73
		// (set) Token: 0x06003060 RID: 12384 RVA: 0x001BA97B File Offset: 0x001B8B7B
		public int campaignId { get; private set; }

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06003061 RID: 12385 RVA: 0x001BA984 File Offset: 0x001B8B84
		// (set) Token: 0x06003062 RID: 12386 RVA: 0x001BA98C File Offset: 0x001B8B8C
		public int picnicBuffAddratio { get; private set; }

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06003063 RID: 12387 RVA: 0x001BA995 File Offset: 0x001B8B95
		// (set) Token: 0x06003064 RID: 12388 RVA: 0x001BA99D File Offset: 0x001B8B9D
		public DateTime startTime { get; private set; }

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06003065 RID: 12389 RVA: 0x001BA9A6 File Offset: 0x001B8BA6
		// (set) Token: 0x06003066 RID: 12390 RVA: 0x001BA9AE File Offset: 0x001B8BAE
		public DateTime endTime { get; private set; }

		// Token: 0x06003067 RID: 12391 RVA: 0x001BA9B8 File Offset: 0x001B8BB8
		public CampaignPicnicData(MstPicnicCampaignData campaignPicnicData)
		{
			this.campaignId = campaignPicnicData.campaignId;
			this.picnicBuffAddratio = campaignPicnicData.picnicBuffAddratio;
			this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(campaignPicnicData.startTime));
			this.endTime = new DateTime(PrjUtil.ConvertTimeToTicks(campaignPicnicData.endTime));
		}
	}

	// Token: 0x02000634 RID: 1588
	public class CampaignTarget
	{
		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06003068 RID: 12392 RVA: 0x001BAA0F File Offset: 0x001B8C0F
		// (set) Token: 0x06003069 RID: 12393 RVA: 0x001BAA17 File Offset: 0x001B8C17
		public DataManagerCampaign.TARGET_TYPE TargetType { get; private set; }

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x0600306A RID: 12394 RVA: 0x001BAA20 File Offset: 0x001B8C20
		// (set) Token: 0x0600306B RID: 12395 RVA: 0x001BAA28 File Offset: 0x001B8C28
		public int TargetId { get; private set; }

		// Token: 0x0600306C RID: 12396 RVA: 0x001BAA34 File Offset: 0x001B8C34
		public CampaignTarget(int id, int type)
		{
			this.TargetId = id;
			switch (type)
			{
			case 1:
				this.TargetType = DataManagerCampaign.TARGET_TYPE.QuestGroup;
				return;
			case 2:
				this.TargetType = DataManagerCampaign.TARGET_TYPE.Map;
				return;
			case 3:
				this.TargetType = DataManagerCampaign.TARGET_TYPE.Chapter;
				return;
			default:
				this.TargetType = DataManagerCampaign.TARGET_TYPE.Undefained;
				return;
			}
		}
	}

	// Token: 0x02000635 RID: 1589
	public enum TARGET_TYPE
	{
		// Token: 0x04002E06 RID: 11782
		Undefained,
		// Token: 0x04002E07 RID: 11783
		Chapter,
		// Token: 0x04002E08 RID: 11784
		Map,
		// Token: 0x04002E09 RID: 11785
		QuestGroup
	}
}
