using System;
using System.Collections.Generic;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x02000090 RID: 144
public class DataManagerKemoBoard
{
	// Token: 0x06000590 RID: 1424 RVA: 0x000258F3 File Offset: 0x00023AF3
	public DataManagerKemoBoard(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x06000591 RID: 1425 RVA: 0x00025902 File Offset: 0x00023B02
	public List<DataManagerKemoBoard.KemoBoardAreaData> KemoBoardAreaDataList
	{
		get
		{
			return this.kemoBoardAreaStaticDataList;
		}
	}

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x06000592 RID: 1426 RVA: 0x0002590A File Offset: 0x00023B0A
	public List<DataManagerKemoBoard.KemoBoardPanelData> KemoBoardPanelDataList
	{
		get
		{
			return this.kemoBoardPanelStaticDataList;
		}
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06000593 RID: 1427 RVA: 0x00025912 File Offset: 0x00023B12
	public Dictionary<CharaDef.AttributeType, DataManagerKemoBoard.KemoBoardBonusParam> KemoBoardBonusParamMap
	{
		get
		{
			return this.kemoBoardBonusParamMap;
		}
	}

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x06000594 RID: 1428 RVA: 0x0002591A File Offset: 0x00023B1A
	public Dictionary<CharaDef.AttributeType, HashSet<int>> ReleaseKamoPanelMap
	{
		get
		{
			return this.releaseKamoPanelMap;
		}
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06000595 RID: 1429 RVA: 0x00025922 File Offset: 0x00023B22
	public HashSet<int> ReleaseKemoPanelSet
	{
		get
		{
			return this.releaseKemoPanelSet;
		}
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x0002592A File Offset: 0x00023B2A
	public void RequestGetKemoBoard()
	{
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x0002592C File Offset: 0x00023B2C
	public void RequestOpenKemoBoard(int panelId)
	{
		this.parentData.ServerRequest(KemoboardOpenCmd.Create(panelId), new Action<Command>(this.CbOpenKemoBoardCmd));
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x0002594C File Offset: 0x00023B4C
	public void RequestOpenKemoBoard(CharaDef.AttributeType attr, int id)
	{
		int num = (int)(attr * (CharaDef.AttributeType)1000 + id);
		this.parentData.ServerRequest(KemoboardOpenCmd.Create(num), new Action<Command>(this.CbOpenKemoBoardCmd));
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x00025980 File Offset: 0x00023B80
	public void RequestResetKemoBoard(CharaDef.AttributeType attr, bool useStone)
	{
		this.parentData.ServerRequest(KemoboardResetCmd.Create((int)attr, useStone ? 1 : 0), new Action<Command>(this.CbResetKemoBoardCmd));
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x000259B4 File Offset: 0x00023BB4
	public void CbOpenKemoBoardCmd(Command cmd)
	{
		KemoboardOpenResponse kemoboardOpenResponse = cmd.response as KemoboardOpenResponse;
		KemoboardOpenRequest kemoboardOpenRequest = cmd.request as KemoboardOpenRequest;
		this.parentData.UpdateUserAssetByAssets(kemoboardOpenResponse.assets);
		CharaDef.AttributeType attributeType = (CharaDef.AttributeType)(kemoboardOpenRequest.panel_id / 1000);
		this.releaseKamoPanelMap[attributeType].Add(kemoboardOpenRequest.panel_id % 1000);
		this.releaseKemoPanelSet.Add(kemoboardOpenRequest.panel_id);
		this.UpdateKemoboardBonusParam(ref this.kemoBoardBonusParamMap, kemoboardOpenRequest.panel_id);
		this.parentData.UpdateSumUserAllCharaKemoStatus(null);
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x00025A48 File Offset: 0x00023C48
	public void CbResetKemoBoardCmd(Command cmd)
	{
		KemoboardResetResponse kemoboardResetResponse = cmd.response as KemoboardResetResponse;
		KemoboardResetRequest req = cmd.request as KemoboardResetRequest;
		this.parentData.UpdateUserAssetByAssets(kemoboardResetResponse.assets);
		CharaDef.AttributeType area_id = (CharaDef.AttributeType)req.area_id;
		this.releaseKamoPanelMap[area_id] = new HashSet<int>();
		this.releaseKemoPanelSet.RemoveWhere((int x) => req.area_id == x / 1000);
		this.kemoBoardBonusParamMap[area_id] = new DataManagerKemoBoard.KemoBoardBonusParam(area_id);
		foreach (int num in kemoboardResetResponse.kemoboard_panels)
		{
			CharaDef.AttributeType attributeType = (CharaDef.AttributeType)(num / 1000);
			if (area_id == attributeType)
			{
				this.releaseKamoPanelMap[area_id].Add(num % 1000);
				this.releaseKemoPanelSet.Add(num);
				this.kemoBoardBonusParamMap[area_id].AddPanel(num);
			}
		}
		this.parentData.UpdateSumUserAllCharaKemoStatus(null);
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x00025B68 File Offset: 0x00023D68
	public void InitializeMstData(MstManager mstManager)
	{
		List<MstKemoboardAreaData> mst = mstManager.GetMst<List<MstKemoboardAreaData>>(MstType.KEMOBOARD_AREA_DATA);
		List<MstKemoboardPanelData> mst2 = mstManager.GetMst<List<MstKemoboardPanelData>>(MstType.KEMOBOARD_PANEL_DATA);
		this.kemoBoardAreaStaticDataList = new List<DataManagerKemoBoard.KemoBoardAreaData>();
		foreach (MstKemoboardAreaData mstKemoboardAreaData in mst)
		{
			this.kemoBoardAreaStaticDataList.Add(new DataManagerKemoBoard.KemoBoardAreaData(mstKemoboardAreaData));
		}
		this.kemoBoardPanelStaticDataList = new List<DataManagerKemoBoard.KemoBoardPanelData>();
		foreach (MstKemoboardPanelData mstKemoboardPanelData in mst2)
		{
			this.kemoBoardPanelStaticDataList.Add(new DataManagerKemoBoard.KemoBoardPanelData(mstKemoboardPanelData, this.kemoBoardAreaStaticDataList));
		}
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x00025C38 File Offset: 0x00023E38
	public void UpdateUserDataByServer(List<int> kemoPanelIDList)
	{
		this.releaseKamoPanelMap = new Dictionary<CharaDef.AttributeType, HashSet<int>>
		{
			{
				CharaDef.AttributeType.RED,
				new HashSet<int>()
			},
			{
				CharaDef.AttributeType.GREEN,
				new HashSet<int>()
			},
			{
				CharaDef.AttributeType.BLUE,
				new HashSet<int>()
			},
			{
				CharaDef.AttributeType.PINK,
				new HashSet<int>()
			},
			{
				CharaDef.AttributeType.LIME,
				new HashSet<int>()
			},
			{
				CharaDef.AttributeType.AQUA,
				new HashSet<int>()
			}
		};
		this.kemoBoardBonusParamMap = new Dictionary<CharaDef.AttributeType, DataManagerKemoBoard.KemoBoardBonusParam>
		{
			{
				CharaDef.AttributeType.RED,
				new DataManagerKemoBoard.KemoBoardBonusParam(CharaDef.AttributeType.RED)
			},
			{
				CharaDef.AttributeType.GREEN,
				new DataManagerKemoBoard.KemoBoardBonusParam(CharaDef.AttributeType.GREEN)
			},
			{
				CharaDef.AttributeType.BLUE,
				new DataManagerKemoBoard.KemoBoardBonusParam(CharaDef.AttributeType.BLUE)
			},
			{
				CharaDef.AttributeType.PINK,
				new DataManagerKemoBoard.KemoBoardBonusParam(CharaDef.AttributeType.PINK)
			},
			{
				CharaDef.AttributeType.LIME,
				new DataManagerKemoBoard.KemoBoardBonusParam(CharaDef.AttributeType.LIME)
			},
			{
				CharaDef.AttributeType.AQUA,
				new DataManagerKemoBoard.KemoBoardBonusParam(CharaDef.AttributeType.AQUA)
			}
		};
		this.releaseKemoPanelSet = new HashSet<int>();
		foreach (int num in kemoPanelIDList)
		{
			CharaDef.AttributeType attributeType = (CharaDef.AttributeType)(num / 1000);
			this.releaseKamoPanelMap[attributeType].Add(num % 1000);
			this.releaseKemoPanelSet.Add(num);
			this.UpdateKemoboardBonusParam(ref this.kemoBoardBonusParamMap, num);
		}
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x00025D74 File Offset: 0x00023F74
	public void UpdateKemoboardBonusParam(ref Dictionary<CharaDef.AttributeType, DataManagerKemoBoard.KemoBoardBonusParam> KBbonusParamMap, int kemoPanelId)
	{
		CharaDef.AttributeType attributeType = (CharaDef.AttributeType)(kemoPanelId / 1000);
		KBbonusParamMap[attributeType].AddPanel(kemoPanelId);
	}

	// Token: 0x0400059D RID: 1437
	private DataManager parentData;

	// Token: 0x0400059E RID: 1438
	private List<DataManagerKemoBoard.KemoBoardAreaData> kemoBoardAreaStaticDataList;

	// Token: 0x0400059F RID: 1439
	private List<DataManagerKemoBoard.KemoBoardPanelData> kemoBoardPanelStaticDataList;

	// Token: 0x040005A0 RID: 1440
	private Dictionary<CharaDef.AttributeType, DataManagerKemoBoard.KemoBoardBonusParam> kemoBoardBonusParamMap;

	// Token: 0x040005A1 RID: 1441
	private Dictionary<CharaDef.AttributeType, HashSet<int>> releaseKamoPanelMap;

	// Token: 0x040005A2 RID: 1442
	private HashSet<int> releaseKemoPanelSet;

	// Token: 0x020006E2 RID: 1762
	public class KemoBoardAreaData
	{
		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x0600333E RID: 13118 RVA: 0x001C1888 File Offset: 0x001BFA88
		// (set) Token: 0x0600333F RID: 13119 RVA: 0x001C1890 File Offset: 0x001BFA90
		public int Id { get; private set; }

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06003340 RID: 13120 RVA: 0x001C1899 File Offset: 0x001BFA99
		// (set) Token: 0x06003341 RID: 13121 RVA: 0x001C18A1 File Offset: 0x001BFAA1
		public CharaDef.AttributeType Attr { get; private set; }

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06003342 RID: 13122 RVA: 0x001C18AA File Offset: 0x001BFAAA
		// (set) Token: 0x06003343 RID: 13123 RVA: 0x001C18B2 File Offset: 0x001BFAB2
		public int ResetStoneNum { get; set; }

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06003344 RID: 13124 RVA: 0x001C18BB File Offset: 0x001BFABB
		// (set) Token: 0x06003345 RID: 13125 RVA: 0x001C18C3 File Offset: 0x001BFAC3
		public int ResetItemId { get; private set; }

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06003346 RID: 13126 RVA: 0x001C18CC File Offset: 0x001BFACC
		// (set) Token: 0x06003347 RID: 13127 RVA: 0x001C18D4 File Offset: 0x001BFAD4
		public int ResetItemNum { get; private set; }

		// Token: 0x06003348 RID: 13128 RVA: 0x001C18E0 File Offset: 0x001BFAE0
		public KemoBoardAreaData(MstKemoboardAreaData mstKemoArea)
		{
			this.Id = mstKemoArea.id;
			this.Attr = (CharaDef.AttributeType)mstKemoArea.attribute;
			this.ResetStoneNum = mstKemoArea.resetStoneNum;
			this.ResetItemId = mstKemoArea.resetItemId;
			this.ResetItemNum = mstKemoArea.resetItemNum;
		}
	}

	// Token: 0x020006E3 RID: 1763
	public class KemoBoardPanelData
	{
		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06003349 RID: 13129 RVA: 0x001C192F File Offset: 0x001BFB2F
		// (set) Token: 0x0600334A RID: 13130 RVA: 0x001C1937 File Offset: 0x001BFB37
		public int Id { get; set; }

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x0600334B RID: 13131 RVA: 0x001C1940 File Offset: 0x001BFB40
		// (set) Token: 0x0600334C RID: 13132 RVA: 0x001C1948 File Offset: 0x001BFB48
		public int AreaId { get; set; }

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x0600334D RID: 13133 RVA: 0x001C1951 File Offset: 0x001BFB51
		// (set) Token: 0x0600334E RID: 13134 RVA: 0x001C1959 File Offset: 0x001BFB59
		public CharaDef.AttributeType Attr { get; private set; }

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x0600334F RID: 13135 RVA: 0x001C1962 File Offset: 0x001BFB62
		// (set) Token: 0x06003350 RID: 13136 RVA: 0x001C196A File Offset: 0x001BFB6A
		public int ParentId { get; set; }

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06003351 RID: 13137 RVA: 0x001C1973 File Offset: 0x001BFB73
		// (set) Token: 0x06003352 RID: 13138 RVA: 0x001C197B File Offset: 0x001BFB7B
		public int bonusTypeNum { get; set; }

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06003353 RID: 13139 RVA: 0x001C1984 File Offset: 0x001BFB84
		// (set) Token: 0x06003354 RID: 13140 RVA: 0x001C198C File Offset: 0x001BFB8C
		public DataManagerKemoBoard.BonusType bonusType { get; set; }

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06003355 RID: 13141 RVA: 0x001C1995 File Offset: 0x001BFB95
		// (set) Token: 0x06003356 RID: 13142 RVA: 0x001C199D File Offset: 0x001BFB9D
		public int BonusValue { get; set; }

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06003357 RID: 13143 RVA: 0x001C19A6 File Offset: 0x001BFBA6
		// (set) Token: 0x06003358 RID: 13144 RVA: 0x001C19AE File Offset: 0x001BFBAE
		public int BonusItemId { get; set; }

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06003359 RID: 13145 RVA: 0x001C19B7 File Offset: 0x001BFBB7
		// (set) Token: 0x0600335A RID: 13146 RVA: 0x001C19BF File Offset: 0x001BFBBF
		public List<ItemInput> UseItemList { get; set; }

		// Token: 0x0600335B RID: 13147 RVA: 0x001C19C8 File Offset: 0x001BFBC8
		public KemoBoardPanelData(MstKemoboardPanelData mstKemoPanel, List<DataManagerKemoBoard.KemoBoardAreaData> kbadList)
		{
			this.Id = mstKemoPanel.id % 1000;
			this.AreaId = mstKemoPanel.areaId;
			DataManagerKemoBoard.KemoBoardAreaData kemoBoardAreaData = kbadList.Find((DataManagerKemoBoard.KemoBoardAreaData x) => x.Id == this.AreaId);
			if (kemoBoardAreaData == null)
			{
				this.Attr = CharaDef.AttributeType.ALL;
			}
			else
			{
				this.Attr = kemoBoardAreaData.Attr;
			}
			this.ParentId = mstKemoPanel.parentId % 1000;
			this.bonusTypeNum = mstKemoPanel.bonusType;
			this.bonusType = (DataManagerKemoBoard.BonusType)mstKemoPanel.bonusType;
			this.BonusValue = mstKemoPanel.bonusVal;
			this.BonusItemId = mstKemoPanel.bonusId;
			this.UseItemList = new List<ItemInput>();
			if (mstKemoPanel.useItemId00 != 0)
			{
				this.UseItemList.Add(new ItemInput(mstKemoPanel.useItemId00, mstKemoPanel.useItemNum00));
			}
			if (mstKemoPanel.useItemId01 != 0)
			{
				this.UseItemList.Add(new ItemInput(mstKemoPanel.useItemId01, mstKemoPanel.useItemNum01));
			}
			if (mstKemoPanel.useItemId02 != 0)
			{
				this.UseItemList.Add(new ItemInput(mstKemoPanel.useItemId02, mstKemoPanel.useItemNum02));
			}
		}
	}

	// Token: 0x020006E4 RID: 1764
	public class KemoBoardBonusParam
	{
		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x0600335D RID: 13149 RVA: 0x001C1AED File Offset: 0x001BFCED
		// (set) Token: 0x0600335E RID: 13150 RVA: 0x001C1AF5 File Offset: 0x001BFCF5
		public CharaDef.AttributeType Attribute { get; private set; }

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x0600335F RID: 13151 RVA: 0x001C1AFE File Offset: 0x001BFCFE
		public int KemoStatus
		{
			get
			{
				return this.innerkemoStatus.totalParam;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06003360 RID: 13152 RVA: 0x001C1B0B File Offset: 0x001BFD0B
		// (set) Token: 0x06003361 RID: 13153 RVA: 0x001C1B13 File Offset: 0x001BFD13
		public int Hp { get; private set; }

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06003362 RID: 13154 RVA: 0x001C1B1C File Offset: 0x001BFD1C
		// (set) Token: 0x06003363 RID: 13155 RVA: 0x001C1B24 File Offset: 0x001BFD24
		public int Attack { get; private set; }

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06003364 RID: 13156 RVA: 0x001C1B2D File Offset: 0x001BFD2D
		// (set) Token: 0x06003365 RID: 13157 RVA: 0x001C1B35 File Offset: 0x001BFD35
		public int Difence { get; private set; }

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06003366 RID: 13158 RVA: 0x001C1B3E File Offset: 0x001BFD3E
		// (set) Token: 0x06003367 RID: 13159 RVA: 0x001C1B46 File Offset: 0x001BFD46
		public int Avoid { get; private set; }

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06003368 RID: 13160 RVA: 0x001C1B50 File Offset: 0x001BFD50
		public string AvoidPercent2String
		{
			get
			{
				return ((float)this.Avoid / 10f).ToString("F1");
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06003369 RID: 13161 RVA: 0x001C1B77 File Offset: 0x001BFD77
		// (set) Token: 0x0600336A RID: 13162 RVA: 0x001C1B7F File Offset: 0x001BFD7F
		public int BeatDamage { get; private set; }

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x0600336B RID: 13163 RVA: 0x001C1B88 File Offset: 0x001BFD88
		public string BeatDamagePercent2String
		{
			get
			{
				return ((float)this.BeatDamage / 10f).ToString("F1");
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x0600336C RID: 13164 RVA: 0x001C1BAF File Offset: 0x001BFDAF
		// (set) Token: 0x0600336D RID: 13165 RVA: 0x001C1BB7 File Offset: 0x001BFDB7
		public int ActionDamage { get; private set; }

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x0600336E RID: 13166 RVA: 0x001C1BC0 File Offset: 0x001BFDC0
		public string ActionDamagePercent2String
		{
			get
			{
				return ((float)this.ActionDamage / 10f).ToString("F1");
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x0600336F RID: 13167 RVA: 0x001C1BE7 File Offset: 0x001BFDE7
		// (set) Token: 0x06003370 RID: 13168 RVA: 0x001C1BEF File Offset: 0x001BFDEF
		public int TryDamage { get; private set; }

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06003371 RID: 13169 RVA: 0x001C1BF8 File Offset: 0x001BFDF8
		public string TryDamagePercent2String
		{
			get
			{
				return ((float)this.TryDamage / 10f).ToString("F1");
			}
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x001C1C1F File Offset: 0x001BFE1F
		public KemoBoardBonusParam(CharaDef.AttributeType attr)
		{
			this.Attribute = attr;
			this.innerkemoStatus = new PrjUtil.ParamPreset();
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x001C1C3C File Offset: 0x001BFE3C
		public void AddPanel(int panelId)
		{
			int area = panelId / 1000;
			int id = panelId % 1000;
			DataManagerKemoBoard.KemoBoardPanelData kemoBoardPanelData = DataManager.DmKemoBoard.KemoBoardPanelDataList.Find((DataManagerKemoBoard.KemoBoardPanelData x) => x.Id == id && x.AreaId == area);
			if (kemoBoardPanelData == null)
			{
				return;
			}
			this.AddParam(kemoBoardPanelData.bonusType, kemoBoardPanelData.BonusValue);
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x001C1C9C File Offset: 0x001BFE9C
		private void AddParam(DataManagerKemoBoard.BonusType type, int value)
		{
			switch (type)
			{
			case DataManagerKemoBoard.BonusType.Hp:
				this.Hp += value;
				break;
			case DataManagerKemoBoard.BonusType.Attack:
				this.Attack += value;
				break;
			case DataManagerKemoBoard.BonusType.Difence:
				this.Difence += value;
				break;
			case DataManagerKemoBoard.BonusType.Avoid:
				this.Avoid += value;
				break;
			case DataManagerKemoBoard.BonusType.BeatDamage:
				this.BeatDamage += value;
				break;
			case DataManagerKemoBoard.BonusType.ActionDamage:
				this.ActionDamage += value;
				break;
			case DataManagerKemoBoard.BonusType.TryDamage:
				this.TryDamage += value;
				break;
			}
			this.innerkemoStatus = new PrjUtil.ParamPreset
			{
				hp = this.Hp,
				atk = this.Attack,
				def = this.Difence,
				avoid = this.Avoid,
				beatDamageRatio = this.BeatDamage,
				actionDamageRatio = this.ActionDamage,
				tryDamageRatio = this.TryDamage
			};
		}

		// Token: 0x0400311F RID: 12575
		private PrjUtil.ParamPreset innerkemoStatus;
	}

	// Token: 0x020006E5 RID: 1765
	public enum BonusType
	{
		// Token: 0x04003121 RID: 12577
		Blank,
		// Token: 0x04003122 RID: 12578
		Hp,
		// Token: 0x04003123 RID: 12579
		Attack,
		// Token: 0x04003124 RID: 12580
		Difence,
		// Token: 0x04003125 RID: 12581
		Avoid,
		// Token: 0x04003126 RID: 12582
		BeatDamage,
		// Token: 0x04003127 RID: 12583
		ActionDamage,
		// Token: 0x04003128 RID: 12584
		TryDamage,
		// Token: 0x04003129 RID: 12585
		Item = 10
	}
}
