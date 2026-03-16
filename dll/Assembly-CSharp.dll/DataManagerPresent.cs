using System;
using System.Collections.Generic;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;

public class DataManagerPresent
{
	public DataManagerPresent(DataManager p)
	{
		this.parentData = p;
	}

	public List<GachaResult> LastReceiveReplacePresentList { get; private set; } = new List<GachaResult>();

	public int MaxPresentDataNum
	{
		get
		{
			return DataManager.DmServerMst.MstAppConfig.presentLimit;
		}
	}

	public int MaxGetHistoryDataNum
	{
		get
		{
			return DataManager.DmServerMst.MstAppConfig.historyLimit;
		}
	}

	public bool IsOverLimit { get; private set; }

	public bool IsGoldStock { get; private set; }

	public List<DataManagerPresent.UserPresentData> GetUserPresentList()
	{
		return this.userPresentList;
	}

	public List<DataManagerPresent.UserPresentData> GetUserReceivePresentList()
	{
		return this.userReceivePresentList;
	}

	public List<DataManagerPresent.UserReceiveHistoryData> GetUserReceiveHistoryList()
	{
		return this.userReceiveHistoryList;
	}

	public void RequestGetPresentList()
	{
		this.parentData.ServerRequest(PresentBoxCmd.Create(0, this.MaxPresentDataNum), new Action<Command>(this.CbPresentBoxCmd));
	}

	public void RequestGetHistoryist()
	{
		this.parentData.ServerRequest(ReceiveHistoryCmd.Create(0, this.MaxGetHistoryDataNum), new Action<Command>(this.CbReceiveHistoryCmd));
	}

	public void RequestActionPresentGetOne(long targetId, DataManagerPresent.UserPresentData userPresent)
	{
		this.IsGoldStock = userPresent.itemId == 30101 && DataManagerItem.IsExpectedItemStock(30101, (long)userPresent.itemNum);
		this.RequestActionPresentGetInternal(new List<long> { targetId });
	}

	public void RequestActionPresentGetAll()
	{
		List<long> list = new List<long>();
		long num = 0L;
		this.IsGoldStock = false;
		if (this.userPresentList != null)
		{
			foreach (DataManagerPresent.UserPresentData userPresentData in this.userPresentList)
			{
				ItemDef.Kind kind = ItemDef.Id2Kind(userPresentData.itemId);
				if (kind != ItemDef.Kind.CHARA && kind != ItemDef.Kind.CLOTHES && kind != ItemDef.Kind.ACHIEVEMENT)
				{
					list.Add(userPresentData.id);
				}
				if (userPresentData.itemId == 30101)
				{
					num += (long)userPresentData.itemNum;
				}
			}
			this.IsGoldStock = num > 0L && DataManagerItem.IsExpectedItemStock(30101, num);
			this.RequestActionPresentGetInternal(list);
		}
	}

	private void RequestActionPresentGetInternal(List<long> targetIdList)
	{
		this.targetIdCount = targetIdList.Count;
		this.LastReceiveReplacePresentList = new List<GachaResult>();
		this.parentData.ServerRequest(PresentGetCmd.Create(targetIdList, 0, this.MaxPresentDataNum, 0, this.MaxGetHistoryDataNum), new Action<Command>(this.CbPresentGetCmd));
	}

	private void CbPresentBoxCmd(Command cmd)
	{
		PresentBoxResponse presentBoxResponse = cmd.response as PresentBoxResponse;
		this.UpdatePresentList(presentBoxResponse.userPresentList);
	}

	private void CbReceiveHistoryCmd(Command cmd)
	{
		ReceiveHistoryResponse receiveHistoryResponse = cmd.response as ReceiveHistoryResponse;
		this.UpdateReceiveHistoryList(receiveHistoryResponse.userReceiveHistoryList);
	}

	private void CbPresentGetCmd(Command cmd)
	{
		PresentGetResponse presentGetResponse = cmd.response as PresentGetResponse;
		this.userReceivePresentList = new List<DataManagerPresent.UserPresentData>();
		if (presentGetResponse.receiveIdList != null)
		{
			foreach (DataManagerPresent.UserPresentData userPresentData in this.userPresentList)
			{
				foreach (long num in presentGetResponse.receiveIdList)
				{
					if (userPresentData.id == num)
					{
						this.userReceivePresentList.Add(new DataManagerPresent.UserPresentData
						{
							id = userPresentData.id,
							itemId = userPresentData.itemId,
							itemNum = userPresentData.itemNum,
							labelText = userPresentData.labelText,
							receiveTime = userPresentData.receiveTime
						});
					}
				}
			}
		}
		this.IsOverLimit = this.targetIdCount != this.userReceivePresentList.Count;
		this.UpdatePresentList(presentGetResponse.userPresentList);
		this.UpdateReceiveHistoryList(presentGetResponse.userReceiveHistoryList);
		this.LastReceiveReplacePresentList = new List<GachaResult>();
		if (presentGetResponse.gacha_result != null)
		{
			foreach (GachaResult gachaResult in presentGetResponse.gacha_result)
			{
				if (gachaResult.rep_flg == 1 && gachaResult.rep_item_list != null)
				{
					this.LastReceiveReplacePresentList.Add(gachaResult);
				}
			}
		}
		this.parentData.UpdateUserAssetByAssets(presentGetResponse.assets);
	}

	private void UpdatePresentList(List<UserPresent> upList)
	{
		this.userPresentList = new List<DataManagerPresent.UserPresentData>();
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		if (homeCheckResult != null)
		{
			homeCheckResult.presentBoxNum = 0;
		}
		if (upList == null)
		{
			return;
		}
		foreach (UserPresent userPresent in upList)
		{
			this.userPresentList.Add(new DataManagerPresent.UserPresentData
			{
				id = userPresent.id,
				itemId = userPresent.itemId,
				itemNum = userPresent.itemNum,
				labelText = userPresent.labelText,
				receiveTime = new DateTime(PrjUtil.ConvertTimeToTicks(userPresent.receiveTime))
			});
		}
		if (homeCheckResult != null)
		{
			homeCheckResult.presentBoxNum = this.userPresentList.Count;
		}
	}

	private void UpdateReceiveHistoryList(List<UserReceiveHistory> urhList)
	{
		this.userReceiveHistoryList = new List<DataManagerPresent.UserReceiveHistoryData>();
		if (urhList == null)
		{
			return;
		}
		foreach (UserReceiveHistory userReceiveHistory in urhList)
		{
			if (ItemDef.Kind.CLOTHES == ItemDef.Id2Kind(userReceiveHistory.itemId))
			{
				CharaClothStatic charaClothesStaticData = DataManager.DmChara.GetCharaClothesStaticData(userReceiveHistory.itemId);
				if (charaClothesStaticData.GetRank != 0)
				{
					continue;
				}
			}
			this.userReceiveHistoryList.Add(new DataManagerPresent.UserReceiveHistoryData
			{
				itemId = userReceiveHistory.itemId,
				itemNum = userReceiveHistory.itemNum,
				labelText = userReceiveHistory.labelText,
				receiveTime = new DateTime(PrjUtil.ConvertTimeToTicks(userReceiveHistory.receiveTime))
			});
		}
	}

	private DataManager parentData;

	private List<DataManagerPresent.UserPresentData> userPresentList;

	private List<DataManagerPresent.UserReceiveHistoryData> userReceiveHistoryList;

	private List<DataManagerPresent.UserPresentData> userReceivePresentList;

	private int targetIdCount;

	public class UserPresentData
	{
		public ItemData GetItemData()
		{
			return new ItemData(this.itemId, this.itemNum);
		}

		public long id;

		public string labelText;

		public int itemId;

		public int itemNum;

		public DateTime receiveTime;
	}

	public class UserReceiveHistoryData
	{
		public ItemData GetItemData()
		{
			return new ItemData(this.itemId, this.itemNum);
		}

		public string labelText;

		public int itemId;

		public int itemNum;

		public DateTime receiveTime;
	}
}
