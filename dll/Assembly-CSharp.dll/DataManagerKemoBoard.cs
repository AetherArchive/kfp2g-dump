using System;
using System.Collections.Generic;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

public class DataManagerKemoBoard
{
	public DataManagerKemoBoard(DataManager p)
	{
		this.parentData = p;
	}

	public List<DataManagerKemoBoard.KemoBoardAreaData> KemoBoardAreaDataList
	{
		get
		{
			return this.kemoBoardAreaStaticDataList;
		}
	}

	public List<DataManagerKemoBoard.KemoBoardPanelData> KemoBoardPanelDataList
	{
		get
		{
			return this.kemoBoardPanelStaticDataList;
		}
	}

	public Dictionary<CharaDef.AttributeType, DataManagerKemoBoard.KemoBoardBonusParam> KemoBoardBonusParamMap
	{
		get
		{
			return this.kemoBoardBonusParamMap;
		}
	}

	public Dictionary<CharaDef.AttributeType, HashSet<int>> ReleaseKamoPanelMap
	{
		get
		{
			return this.releaseKamoPanelMap;
		}
	}

	public HashSet<int> ReleaseKemoPanelSet
	{
		get
		{
			return this.releaseKemoPanelSet;
		}
	}

	public void RequestGetKemoBoard()
	{
	}

	public void RequestOpenKemoBoard(int panelId)
	{
		this.parentData.ServerRequest(KemoboardOpenCmd.Create(panelId), new Action<Command>(this.CbOpenKemoBoardCmd));
	}

	public void RequestOpenKemoBoard(CharaDef.AttributeType attr, int id)
	{
		int num = (int)(attr * (CharaDef.AttributeType)1000 + id);
		this.parentData.ServerRequest(KemoboardOpenCmd.Create(num), new Action<Command>(this.CbOpenKemoBoardCmd));
	}

	public void RequestResetKemoBoard(CharaDef.AttributeType attr, bool useStone)
	{
		this.parentData.ServerRequest(KemoboardResetCmd.Create((int)attr, useStone ? 1 : 0), new Action<Command>(this.CbResetKemoBoardCmd));
	}

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

	public void UpdateKemoboardBonusParam(ref Dictionary<CharaDef.AttributeType, DataManagerKemoBoard.KemoBoardBonusParam> KBbonusParamMap, int kemoPanelId)
	{
		CharaDef.AttributeType attributeType = (CharaDef.AttributeType)(kemoPanelId / 1000);
		KBbonusParamMap[attributeType].AddPanel(kemoPanelId);
	}

	private DataManager parentData;

	private List<DataManagerKemoBoard.KemoBoardAreaData> kemoBoardAreaStaticDataList;

	private List<DataManagerKemoBoard.KemoBoardPanelData> kemoBoardPanelStaticDataList;

	private Dictionary<CharaDef.AttributeType, DataManagerKemoBoard.KemoBoardBonusParam> kemoBoardBonusParamMap;

	private Dictionary<CharaDef.AttributeType, HashSet<int>> releaseKamoPanelMap;

	private HashSet<int> releaseKemoPanelSet;

	public class KemoBoardAreaData
	{
		public int Id { get; private set; }

		public CharaDef.AttributeType Attr { get; private set; }

		public int ResetStoneNum { get; set; }

		public int ResetItemId { get; private set; }

		public int ResetItemNum { get; private set; }

		public KemoBoardAreaData(MstKemoboardAreaData mstKemoArea)
		{
			this.Id = mstKemoArea.id;
			this.Attr = (CharaDef.AttributeType)mstKemoArea.attribute;
			this.ResetStoneNum = mstKemoArea.resetStoneNum;
			this.ResetItemId = mstKemoArea.resetItemId;
			this.ResetItemNum = mstKemoArea.resetItemNum;
		}
	}

	public class KemoBoardPanelData
	{
		public int Id { get; set; }

		public int AreaId { get; set; }

		public CharaDef.AttributeType Attr { get; private set; }

		public int ParentId { get; set; }

		public int bonusTypeNum { get; set; }

		public DataManagerKemoBoard.BonusType bonusType { get; set; }

		public int BonusValue { get; set; }

		public int BonusItemId { get; set; }

		public List<ItemInput> UseItemList { get; set; }

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

	public class KemoBoardBonusParam
	{
		public CharaDef.AttributeType Attribute { get; private set; }

		public int KemoStatus
		{
			get
			{
				return this.innerkemoStatus.totalParam;
			}
		}

		public int Hp { get; private set; }

		public int Attack { get; private set; }

		public int Difence { get; private set; }

		public int Avoid { get; private set; }

		public string AvoidPercent2String
		{
			get
			{
				return ((float)this.Avoid / 10f).ToString("F1");
			}
		}

		public int BeatDamage { get; private set; }

		public string BeatDamagePercent2String
		{
			get
			{
				return ((float)this.BeatDamage / 10f).ToString("F1");
			}
		}

		public int ActionDamage { get; private set; }

		public string ActionDamagePercent2String
		{
			get
			{
				return ((float)this.ActionDamage / 10f).ToString("F1");
			}
		}

		public int TryDamage { get; private set; }

		public string TryDamagePercent2String
		{
			get
			{
				return ((float)this.TryDamage / 10f).ToString("F1");
			}
		}

		public KemoBoardBonusParam(CharaDef.AttributeType attr)
		{
			this.Attribute = attr;
			this.innerkemoStatus = new PrjUtil.ParamPreset();
		}

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

		private PrjUtil.ParamPreset innerkemoStatus;
	}

	public enum BonusType
	{
		Blank,
		Hp,
		Attack,
		Difence,
		Avoid,
		BeatDamage,
		ActionDamage,
		TryDamage,
		Item = 10
	}
}
