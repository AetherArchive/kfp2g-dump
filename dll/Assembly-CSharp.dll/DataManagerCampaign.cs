using System;
using System.Collections.Generic;
using SGNFW.Mst;

public class DataManagerCampaign
{
	public DataManagerCampaign(DataManager p)
	{
		this.parentData = p;
	}

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

	private DataManager parentData;

	private List<DataManagerCampaign.CampaignGrowData> campaignGrowCharaDataList;

	private List<DataManagerCampaign.CampaignGrowData> campaignGrowPhotoDataList;

	private List<DataManagerCampaign.CampaignItemDropData> campaignItemDropDataList;

	private List<DataManagerCampaign.CampaignKizunaData> campaignKizunaDataList;

	private List<DataManagerCampaign.CampaignPvpCoinData> campaignPvPCoinDataList;

	private List<DataManagerCampaign.CampaignQuestStaminaData> campaignQuestStaminaDataList;

	private List<DataManagerCampaign.CampaignStaminaRecoveryData> campaignStaminaRecoveryDataList;

	private List<DataManagerCampaign.CampaignPicnicData> campaignPicnicDataList;

	private List<DataManagerCampaign.CampaignGrowData> campaignGrowCharaKizunaDataList;

	public class CampaignGrowData
	{
		public int campaignId { get; private set; }

		public int successProbability { get; private set; }

		public int bigSuccessProbability { get; private set; }

		public int greatSuccessProbability { get; private set; }

		public int successAcqRate { get; private set; }

		public int bigSuccessAcqRate { get; private set; }

		public int greatSuccessAcqRate { get; private set; }

		public bool IsDisplayCampaign { get; private set; }

		public DateTime startDatetime { get; private set; }

		public DateTime endDatetime { get; private set; }

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

	public class CampaignItemDropData
	{
		public int campaignId { get; private set; }

		public int ratio { get; private set; }

		public List<DataManagerCampaign.CampaignTarget> campaignTargetList { get; private set; }

		public DateTime startTime { get; private set; }

		public DateTime endTime { get; private set; }

		public CampaignItemDropData(MstQuestCampaignItemData campaignItemData)
		{
			this.campaignId = campaignItemData.campaignId;
			this.ratio = campaignItemData.ratio;
			this.campaignTargetList = new List<DataManagerCampaign.CampaignTarget>();
			this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(campaignItemData.startTime));
			this.endTime = new DateTime(PrjUtil.ConvertTimeToTicks(campaignItemData.endTime));
		}
	}

	public class CampaignKizunaData
	{
		public int campaignId { get; private set; }

		public int ratio { get; private set; }

		public List<DataManagerCampaign.CampaignTarget> campaignTargetList { get; private set; }

		public DateTime startTime { get; private set; }

		public DateTime endTime { get; private set; }

		public CampaignKizunaData(MstQuestCampaignKizunaData campaignKizunaData)
		{
			this.campaignId = campaignKizunaData.campaignId;
			this.ratio = campaignKizunaData.ratio;
			this.campaignTargetList = new List<DataManagerCampaign.CampaignTarget>();
			this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(campaignKizunaData.startTime));
			this.endTime = new DateTime(PrjUtil.ConvertTimeToTicks(campaignKizunaData.endTime));
		}
	}

	public class CampaignPvpCoinData
	{
		public int campaignId { get; private set; }

		public int pvpCoinRatio { get; private set; }

		public DateTime startTime { get; private set; }

		public DateTime endTime { get; private set; }

		public CampaignPvpCoinData(MstPvPCampaignCoinData pvpCampaignCoinData)
		{
			this.campaignId = pvpCampaignCoinData.campaignId;
			this.pvpCoinRatio = pvpCampaignCoinData.ratio;
			this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(pvpCampaignCoinData.startTime));
			this.endTime = new DateTime(PrjUtil.ConvertTimeToTicks(pvpCampaignCoinData.endTime));
		}
	}

	public class CampaignQuestStaminaData
	{
		public int campaignId { get; private set; }

		public int value { get; private set; }

		public List<DataManagerCampaign.CampaignTarget> campaignTargetList { get; private set; }

		public DateTime startTime { get; private set; }

		public DateTime endTime { get; private set; }

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

	public class CampaignStaminaRecoveryData
	{
		public int campaignId { get; private set; }

		public int staminaRecoveryTime { get; private set; }

		public DateTime startTime { get; private set; }

		public DateTime endTime { get; private set; }

		public CampaignStaminaRecoveryData(MstStaminaRecoveryCampaignTimeData campaignStaminaRecoveryData)
		{
			this.campaignId = campaignStaminaRecoveryData.campaignId;
			this.staminaRecoveryTime = campaignStaminaRecoveryData.staminaRecoveryTime;
			this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(campaignStaminaRecoveryData.startTime));
			this.endTime = new DateTime(PrjUtil.ConvertTimeToTicks(campaignStaminaRecoveryData.endTime));
		}
	}

	public class CampaignPicnicData
	{
		public int campaignId { get; private set; }

		public int picnicBuffAddratio { get; private set; }

		public DateTime startTime { get; private set; }

		public DateTime endTime { get; private set; }

		public CampaignPicnicData(MstPicnicCampaignData campaignPicnicData)
		{
			this.campaignId = campaignPicnicData.campaignId;
			this.picnicBuffAddratio = campaignPicnicData.picnicBuffAddratio;
			this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(campaignPicnicData.startTime));
			this.endTime = new DateTime(PrjUtil.ConvertTimeToTicks(campaignPicnicData.endTime));
		}
	}

	public class CampaignTarget
	{
		public DataManagerCampaign.TARGET_TYPE TargetType { get; private set; }

		public int TargetId { get; private set; }

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

	public enum TARGET_TYPE
	{
		Undefained,
		Chapter,
		Map,
		QuestGroup
	}
}
