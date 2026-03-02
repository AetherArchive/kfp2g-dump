using System;
using System.Collections.Generic;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;

// Token: 0x02000099 RID: 153
public class DataManagerPresent
{
	// Token: 0x06000651 RID: 1617 RVA: 0x0002AA24 File Offset: 0x00028C24
	public DataManagerPresent(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x06000652 RID: 1618 RVA: 0x0002AA3E File Offset: 0x00028C3E
	// (set) Token: 0x06000653 RID: 1619 RVA: 0x0002AA46 File Offset: 0x00028C46
	public List<GachaResult> LastReceiveReplacePresentList { get; private set; } = new List<GachaResult>();

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x06000654 RID: 1620 RVA: 0x0002AA4F File Offset: 0x00028C4F
	public int MaxPresentDataNum
	{
		get
		{
			return DataManager.DmServerMst.MstAppConfig.presentLimit;
		}
	}

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x06000655 RID: 1621 RVA: 0x0002AA60 File Offset: 0x00028C60
	public int MaxGetHistoryDataNum
	{
		get
		{
			return DataManager.DmServerMst.MstAppConfig.historyLimit;
		}
	}

	// Token: 0x17000135 RID: 309
	// (get) Token: 0x06000656 RID: 1622 RVA: 0x0002AA71 File Offset: 0x00028C71
	// (set) Token: 0x06000657 RID: 1623 RVA: 0x0002AA79 File Offset: 0x00028C79
	public bool IsOverLimit { get; private set; }

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x06000658 RID: 1624 RVA: 0x0002AA82 File Offset: 0x00028C82
	// (set) Token: 0x06000659 RID: 1625 RVA: 0x0002AA8A File Offset: 0x00028C8A
	public bool IsGoldStock { get; private set; }

	// Token: 0x0600065A RID: 1626 RVA: 0x0002AA93 File Offset: 0x00028C93
	public List<DataManagerPresent.UserPresentData> GetUserPresentList()
	{
		return this.userPresentList;
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x0002AA9B File Offset: 0x00028C9B
	public List<DataManagerPresent.UserPresentData> GetUserReceivePresentList()
	{
		return this.userReceivePresentList;
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x0002AAA3 File Offset: 0x00028CA3
	public List<DataManagerPresent.UserReceiveHistoryData> GetUserReceiveHistoryList()
	{
		return this.userReceiveHistoryList;
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x0002AAAB File Offset: 0x00028CAB
	public void RequestGetPresentList()
	{
		this.parentData.ServerRequest(PresentBoxCmd.Create(0, this.MaxPresentDataNum), new Action<Command>(this.CbPresentBoxCmd));
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x0002AAD0 File Offset: 0x00028CD0
	public void RequestGetHistoryist()
	{
		this.parentData.ServerRequest(ReceiveHistoryCmd.Create(0, this.MaxGetHistoryDataNum), new Action<Command>(this.CbReceiveHistoryCmd));
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x0002AAF5 File Offset: 0x00028CF5
	public void RequestActionPresentGetOne(long targetId, DataManagerPresent.UserPresentData userPresent)
	{
		this.IsGoldStock = userPresent.itemId == 30101 && DataManagerItem.IsExpectedItemStock(30101, (long)userPresent.itemNum);
		this.RequestActionPresentGetInternal(new List<long> { targetId });
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x0002AB30 File Offset: 0x00028D30
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

	// Token: 0x06000661 RID: 1633 RVA: 0x0002ABF8 File Offset: 0x00028DF8
	private void RequestActionPresentGetInternal(List<long> targetIdList)
	{
		this.targetIdCount = targetIdList.Count;
		this.LastReceiveReplacePresentList = new List<GachaResult>();
		this.parentData.ServerRequest(PresentGetCmd.Create(targetIdList, 0, this.MaxPresentDataNum, 0, this.MaxGetHistoryDataNum), new Action<Command>(this.CbPresentGetCmd));
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x0002AC48 File Offset: 0x00028E48
	private void CbPresentBoxCmd(Command cmd)
	{
		PresentBoxResponse presentBoxResponse = cmd.response as PresentBoxResponse;
		this.UpdatePresentList(presentBoxResponse.userPresentList);
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x0002AC70 File Offset: 0x00028E70
	private void CbReceiveHistoryCmd(Command cmd)
	{
		ReceiveHistoryResponse receiveHistoryResponse = cmd.response as ReceiveHistoryResponse;
		this.UpdateReceiveHistoryList(receiveHistoryResponse.userReceiveHistoryList);
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x0002AC98 File Offset: 0x00028E98
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

	// Token: 0x06000665 RID: 1637 RVA: 0x0002AE54 File Offset: 0x00029054
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

	// Token: 0x06000666 RID: 1638 RVA: 0x0002AF2C File Offset: 0x0002912C
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

	// Token: 0x040005F6 RID: 1526
	private DataManager parentData;

	// Token: 0x040005F7 RID: 1527
	private List<DataManagerPresent.UserPresentData> userPresentList;

	// Token: 0x040005F8 RID: 1528
	private List<DataManagerPresent.UserReceiveHistoryData> userReceiveHistoryList;

	// Token: 0x040005F9 RID: 1529
	private List<DataManagerPresent.UserPresentData> userReceivePresentList;

	// Token: 0x040005FA RID: 1530
	private int targetIdCount;

	// Token: 0x02000723 RID: 1827
	public class UserPresentData
	{
		// Token: 0x0600353B RID: 13627 RVA: 0x001C3ADC File Offset: 0x001C1CDC
		public ItemData GetItemData()
		{
			return new ItemData(this.itemId, this.itemNum);
		}

		// Token: 0x04003235 RID: 12853
		public long id;

		// Token: 0x04003236 RID: 12854
		public string labelText;

		// Token: 0x04003237 RID: 12855
		public int itemId;

		// Token: 0x04003238 RID: 12856
		public int itemNum;

		// Token: 0x04003239 RID: 12857
		public DateTime receiveTime;
	}

	// Token: 0x02000724 RID: 1828
	public class UserReceiveHistoryData
	{
		// Token: 0x0600353D RID: 13629 RVA: 0x001C3AF7 File Offset: 0x001C1CF7
		public ItemData GetItemData()
		{
			return new ItemData(this.itemId, this.itemNum);
		}

		// Token: 0x0400323A RID: 12858
		public string labelText;

		// Token: 0x0400323B RID: 12859
		public int itemId;

		// Token: 0x0400323C RID: 12860
		public int itemNum;

		// Token: 0x0400323D RID: 12861
		public DateTime receiveTime;
	}
}
