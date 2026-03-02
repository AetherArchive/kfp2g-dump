using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SGNFW.Ab;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using UnityEngine;

// Token: 0x020000AF RID: 175
public class DataManagerTreeHouse
{
	// Token: 0x0600079C RID: 1948 RVA: 0x00034240 File Offset: 0x00032440
	public DataManagerTreeHouse(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x0003427B File Offset: 0x0003247B
	public List<MstMasterRoomMachineData> GetMstMasterRoomMachineList()
	{
		return this.mstMasterRoomMachineList;
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x00034283 File Offset: 0x00032483
	public TreeHouseFurnitureStatic GetTreeHouseFurnitureStaticData(int id)
	{
		return this.furnitureStaticMap.TryGetValueEx(id, null);
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x00034292 File Offset: 0x00032492
	public List<TreeHouseFurnitureStatic> GetTreeHouseFurnitureStaticData()
	{
		return new List<TreeHouseFurnitureStatic>(this.furnitureStaticMap.Values);
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x000342A4 File Offset: 0x000324A4
	public bool IsSpecialActionByChara(int actionId, int charaId, int clothItemId)
	{
		return this.mstMasterRoomActionList.Exists((MstMasterRoomActionData item) => item.masterRoomActionId == actionId && item.charaId == charaId && item.clothesItemId == clothItemId);
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x000342E4 File Offset: 0x000324E4
	public MstMasterRoomMachineData GetChargeBatteryData()
	{
		MstMasterRoomMachineData mstMasterRoomMachineData;
		if ((mstMasterRoomMachineData = this.mstMasterRoomMachineList.Find((MstMasterRoomMachineData item) => item.type == 1)) == null)
		{
			MstMasterRoomMachineData mstMasterRoomMachineData2 = new MstMasterRoomMachineData();
			mstMasterRoomMachineData2.getItemId = 30790;
			mstMasterRoomMachineData2.getItemNum = 0;
			mstMasterRoomMachineData = mstMasterRoomMachineData2;
			mstMasterRoomMachineData2.getItemTime = 525600;
		}
		return mstMasterRoomMachineData;
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x00034341 File Offset: 0x00032541
	public List<MstMasterRoomFuelData> GetChargeFuelData()
	{
		return this.mstMasterRoomFuelList;
	}

	// Token: 0x1700016E RID: 366
	// (get) Token: 0x060007A3 RID: 1955 RVA: 0x00034349 File Offset: 0x00032549
	// (set) Token: 0x060007A4 RID: 1956 RVA: 0x00034351 File Offset: 0x00032551
	public List<TreeHouseFurniturePackData> UserFurniturePackList { get; private set; } = new List<TreeHouseFurniturePackData>();

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x060007A5 RID: 1957 RVA: 0x0003435A File Offset: 0x0003255A
	// (set) Token: 0x060007A6 RID: 1958 RVA: 0x00034362 File Offset: 0x00032562
	public TreeHouseKizunaBonusData KizunaBonusData { get; private set; }

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x060007A7 RID: 1959 RVA: 0x0003436B File Offset: 0x0003256B
	// (set) Token: 0x060007A8 RID: 1960 RVA: 0x00034373 File Offset: 0x00032573
	public TreeHouseStaminaBonusData StaminaBonusData { get; private set; }

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x060007A9 RID: 1961 RVA: 0x0003437C File Offset: 0x0003257C
	// (set) Token: 0x060007AA RID: 1962 RVA: 0x00034384 File Offset: 0x00032584
	public HashSet<int> FavoriteFurnitureItemIdList { get; private set; }

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x060007AB RID: 1963 RVA: 0x0003438D File Offset: 0x0003258D
	// (set) Token: 0x060007AC RID: 1964 RVA: 0x00034395 File Offset: 0x00032595
	public List<TreeHouseFurnitureMapping> FurnitureMappingList { get; private set; }

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x060007AD RID: 1965 RVA: 0x0003439E File Offset: 0x0003259E
	// (set) Token: 0x060007AE RID: 1966 RVA: 0x000343A6 File Offset: 0x000325A6
	public List<TreeHousePutCharaData> PutCharaDataList { get; private set; }

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x060007AF RID: 1967 RVA: 0x000343AF File Offset: 0x000325AF
	// (set) Token: 0x060007B0 RID: 1968 RVA: 0x000343B7 File Offset: 0x000325B7
	public List<TreeHouseMyset> MysetList { get; private set; }

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x060007B1 RID: 1969 RVA: 0x000343C0 File Offset: 0x000325C0
	// (set) Token: 0x060007B2 RID: 1970 RVA: 0x000343C8 File Offset: 0x000325C8
	public TreeHouseReceiveStampLog ReceiveStampLog { get; private set; }

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x060007B3 RID: 1971 RVA: 0x000343D1 File Offset: 0x000325D1
	// (set) Token: 0x060007B4 RID: 1972 RVA: 0x000343D9 File Offset: 0x000325D9
	public List<TreeHouseSocialUser> SocialFollowDataList { get; private set; }

	// Token: 0x17000177 RID: 375
	// (get) Token: 0x060007B5 RID: 1973 RVA: 0x000343E2 File Offset: 0x000325E2
	// (set) Token: 0x060007B6 RID: 1974 RVA: 0x000343EA File Offset: 0x000325EA
	public List<TreeHouseSocialUser> SocialRankingDataList { get; private set; }

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x060007B7 RID: 1975 RVA: 0x000343F3 File Offset: 0x000325F3
	// (set) Token: 0x060007B8 RID: 1976 RVA: 0x000343FB File Offset: 0x000325FB
	public List<TreeHouseSocialUser> SocialPassingDataList { get; private set; }

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x060007B9 RID: 1977 RVA: 0x00034404 File Offset: 0x00032604
	// (set) Token: 0x060007BA RID: 1978 RVA: 0x0003440C File Offset: 0x0003260C
	public List<TreeHouseSocialUser> SocialStampHistoryDataList { get; private set; }

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x060007BB RID: 1979 RVA: 0x00034415 File Offset: 0x00032615
	// (set) Token: 0x060007BC RID: 1980 RVA: 0x0003441D File Offset: 0x0003261D
	public TreeHousePublicInfo PublicInfo { get; private set; }

	// Token: 0x060007BD RID: 1981 RVA: 0x00034426 File Offset: 0x00032626
	public List<MasterRoomMachineReceiveModel> GetMachineReceiveList()
	{
		return this.machineReceiveList;
	}

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x060007BE RID: 1982 RVA: 0x0003442E File Offset: 0x0003262E
	// (set) Token: 0x060007BF RID: 1983 RVA: 0x00034436 File Offset: 0x00032636
	public TreeHouseSocialVisitUserData SocialVisitUserData { get; private set; }

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x060007C0 RID: 1984 RVA: 0x0003443F File Offset: 0x0003263F
	// (set) Token: 0x060007C1 RID: 1985 RVA: 0x00034447 File Offset: 0x00032647
	public bool PublicNameUpdateSuccess { get; private set; }

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x060007C2 RID: 1986 RVA: 0x00034450 File Offset: 0x00032650
	// (set) Token: 0x060007C3 RID: 1987 RVA: 0x00034458 File Offset: 0x00032658
	public bool PublicCommentUpdateSuccess { get; private set; }

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x060007C4 RID: 1988 RVA: 0x00034461 File Offset: 0x00032661
	// (set) Token: 0x060007C5 RID: 1989 RVA: 0x00034469 File Offset: 0x00032669
	public int ShopId { get; private set; }

	// Token: 0x060007C6 RID: 1990 RVA: 0x00034474 File Offset: 0x00032674
	public void RequestGetTreeHouseBase(bool getStampLog)
	{
		this.parentData.ServerRequest(MasterRoomGetDataCmd.Create(1, 1, getStampLog ? 1 : 0, 1, 0, 0, 0, 0, 1), new Action<Command>(this.CbMasterRoomGetDataCmd));
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x000344AC File Offset: 0x000326AC
	public void RequestGetSocialTabData(TreeHouseSocialTabType tabType = TreeHouseSocialTabType.INVALID)
	{
		this.parentData.ServerRequest(MasterRoomGetDataCmd.Create(0, 0, 0, 0, (tabType == TreeHouseSocialTabType.INVALID || tabType == TreeHouseSocialTabType.FOLLOW) ? 1 : 0, (tabType == TreeHouseSocialTabType.INVALID || tabType == TreeHouseSocialTabType.PASSING) ? 1 : 0, (tabType == TreeHouseSocialTabType.INVALID || tabType == TreeHouseSocialTabType.RANKING) ? 1 : 0, (tabType == TreeHouseSocialTabType.INVALID || tabType == TreeHouseSocialTabType.STAMP_HISTORY) ? 1 : 0, (tabType == TreeHouseSocialTabType.INVALID || tabType == TreeHouseSocialTabType.PUBLIC) ? 1 : 0), new Action<Command>(this.CbMasterRoomGetDataCmd));
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x00034510 File Offset: 0x00032710
	public void RequestGetCharaListData()
	{
		this.parentData.ServerRequest(MasterRoomGetDataCmd.Create(0, 1, 0, 1, 0, 0, 0, 0, 0), new Action<Command>(this.CbMasterRoomGetDataCmd));
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x00034544 File Offset: 0x00032744
	public void RequestActionPutFurniture(List<TreeHouseFurnitureMapping> mappingList)
	{
		this.parentData.ServerRequest(MasterRoomSetFurnitureCmd.Create(mappingList.ConvertAll<MasterRoomFurniture>((TreeHouseFurnitureMapping item) => item.ConvertServerData())), new Action<Command>(this.CbMasterRoomSetFurnitureCmd));
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x00034594 File Offset: 0x00032794
	public void RequestActionSetChara(List<TreeHousePutCharaData> putCharaList)
	{
		this.parentData.ServerRequest(MasterRoomSetCharaCmd.Create(putCharaList.ConvertAll<MasterRoomChara>((TreeHousePutCharaData item) => item.ConvertServerData())), new Action<Command>(this.CbMasterRoomSetCharaCmd));
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x000345E2 File Offset: 0x000327E2
	public void RequestActionOtherVisit(int friendId, TreeHouseSocialTabType tabType)
	{
		this.parentData.ServerRequest(MasterRoomVisitCmd.Create(friendId, (int)tabType), new Action<Command>(this.CbMasterRoomVisitCmd));
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x00034602 File Offset: 0x00032802
	public void RequestActionSendStamp(int friendId, int stampId, TreeHouseSocialTabType tabType)
	{
		this.parentData.ServerRequest(MasterRoomSendStampCmd.Create(friendId, stampId, (int)tabType), new Action<Command>(this.CbMasterRoomSendStampCmd));
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x00034623 File Offset: 0x00032823
	public void RequestActionSaveByMyset(int saveMysetId)
	{
		this.parentData.ServerRequest(MasterRoomSaveMysetCmd.Create(saveMysetId), new Action<Command>(this.CbMasterRoomSaveMysetCmd));
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x00034642 File Offset: 0x00032842
	public void RequestActionLoadByMyset(int loadMysetId)
	{
		this.parentData.ServerRequest(MasterRoomLoadMysetCmd.Create(loadMysetId), new Action<Command>(this.CbMasterRoomLoadMysetCmd));
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x00034664 File Offset: 0x00032864
	public void RequestActionUpdatePublicInfo(string houseName, string houseComment, TreeHousePublicInfo.PublicType publicType)
	{
		MasterRoomPublicInfo masterRoomPublicInfo = new MasterRoomPublicInfo
		{
			room_name = houseName,
			comment = houseComment,
			public_type = (int)publicType
		};
		this.parentData.ServerRequest(MasterRoomPublicSettingCmd.Create(masterRoomPublicInfo), new Action<Command>(this.CbMasterRoomPublicSettingCmd));
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x000346AC File Offset: 0x000328AC
	public void RequestActionUpdateFavorite(HashSet<int> offIdList, HashSet<int> onIdList)
	{
		List<NewFlg> list = new List<NewFlg>();
		if (offIdList != null && offIdList.Count > 0)
		{
			list.AddRange(offIdList.ToList<int>().ConvertAll<NewFlg>((int item) => new NewFlg
			{
				category = 13,
				any_id = item,
				new_mgmt_flg = 0
			}));
		}
		if (onIdList != null && onIdList.Count > 0)
		{
			list.AddRange(onIdList.ToList<int>().ConvertAll<NewFlg>((int item) => new NewFlg
			{
				category = 13,
				any_id = item,
				new_mgmt_flg = 1
			}));
		}
		this.parentData.ServerRequest(NewFlgUpdateCmd.Create(list), new Action<Command>(this.CbNewFlgUpdateCmd));
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x00034754 File Offset: 0x00032954
	public void RequestActionUpdateFavorite(int itemId, bool isOn)
	{
		List<NewFlg> list = new List<NewFlg>
		{
			new NewFlg
			{
				category = 13,
				any_id = itemId,
				new_mgmt_flg = (isOn ? 1 : 0)
			}
		};
		this.parentData.ServerRequest(NewFlgUpdateCmd.Create(list), new Action<Command>(this.CbNewFlgUpdateCmd));
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x000347AB File Offset: 0x000329AB
	public void RequestActionUpdateMachineData()
	{
		this.parentData.ServerRequest(MasterRoomGetMachineDataCmd.Create(), new Action<Command>(this.CbMasterRoomGetMachineDataCmd));
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x000347C9 File Offset: 0x000329C9
	public void RequestActionUseFuel(List<UseItem> use_items)
	{
		this.parentData.ServerRequest(MasterRoomAddFuelCmd.Create(use_items), new Action<Command>(this.CbMasterRoomAddFuelCmd));
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x000347E8 File Offset: 0x000329E8
	public void RequestActionReceiveMcnItem(bool is_all, int furniture_index)
	{
		this.parentData.ServerRequest(MasterRoomMachineReceiveCmd.Create(is_all, furniture_index), new Action<Command>(this.CbMasterRoomMachineReceiveCmd));
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x00034808 File Offset: 0x00032A08
	public void InitializeMstData(MstManager mstManager)
	{
		List<MstMasterRoomFurnitureData> mst = mstManager.GetMst<List<MstMasterRoomFurnitureData>>(MstType.MASTER_ROOM_FURNITURE_DATA);
		List<ItemStaticBase> list = new List<ItemStaticBase>();
		foreach (MstMasterRoomFurnitureData mstMasterRoomFurnitureData in mst)
		{
			TreeHouseFurnitureStatic treeHouseFurnitureStatic = new TreeHouseFurnitureStatic(mstMasterRoomFurnitureData);
			this.furnitureStaticMap.Add(mstMasterRoomFurnitureData.id, treeHouseFurnitureStatic);
			list.Add(treeHouseFurnitureStatic);
		}
		this.mstMasterRoomKizunaBonusList = mstManager.GetMst<List<MstMasterRoomKizunaBonus>>(MstType.MASTER_ROOM_KIZUNA_BONUS);
		this.mstMasterRoomKizunaBonusList.Sort((MstMasterRoomKizunaBonus a, MstMasterRoomKizunaBonus b) => b.weight.CompareTo(a.weight));
		this.mstMasterRoomActionList = mstManager.GetMst<List<MstMasterRoomActionData>>(MstType.MASTER_ROOM_ACTION_DATA);
		this.mstMasterRoomMachineList = mstManager.GetMst<List<MstMasterRoomMachineData>>(MstType.MASTER_ROOM_MACHINE_DATA);
		this.mstMasterRoomFuelList = mstManager.GetMst<List<MstMasterRoomFuelData>>(MstType.MASTER_ROOM_FUEL_DATA);
		this.mstMasterRoomStaminaBonusList = mstManager.GetMst<List<MstMasterRoomStaminaBonus>>(MstType.MASTER_ROOM_STAMINA_BONUS);
		this.mstMasterRoomStaminaBonusList.Sort((MstMasterRoomStaminaBonus a, MstMasterRoomStaminaBonus b) => b.count.CompareTo(a.count));
		DataManager.DmItem.AddMstDataByItem(list);
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x00034924 File Offset: 0x00032B24
	public IEnumerator InitializeSmallFurnitureData()
	{
		List<int> list = new List<int>();
		List<string> loadAssetTreeHouseList = AssetManager.LoadAssetDataByCategory(AssetManager.ASSET_CATEGORY_TREE_HOUSE, AssetManager.OWNER.TreeHouseStage, 0, null);
		foreach (string assetName in loadAssetTreeHouseList)
		{
			while (!AssetManager.IsLoadFinishAssetData(AssetManager.PREFIX_PATH_TREEHOUSE_MODEL_DATA + assetName))
			{
				yield return null;
			}
			assetName = null;
		}
		List<string>.Enumerator enumerator = default(List<string>.Enumerator);
		string text = "smallfurnituredata_";
		foreach (string text2 in loadAssetTreeHouseList)
		{
			string[] allAssetNames = SGNFW.Ab.Manager.GetData(text2).asset.GetAllAssetNames();
			for (int i = 0; i < allAssetNames.Length; i++)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(allAssetNames[i]);
				if (fileNameWithoutExtension.StartsWith(text))
				{
					string[] array = fileNameWithoutExtension.Split('_', StringSplitOptions.None);
					if (!string.IsNullOrEmpty(array[1]))
					{
						list.Add(int.Parse(array[1]));
					}
				}
			}
		}
		using (List<int>.Enumerator enumerator3 = list.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				int num = enumerator3.Current;
				TreeHouseSmallFurnitureData treeHouseSmallFurnitureData = ScriptableObject.CreateInstance<TreeHouseSmallFurnitureData>();
				treeHouseSmallFurnitureData = AssetManager.GetAssetData("TreeHouse/SmallFurnitureData/SmallFurnitureData_" + num.ToString("0000")) as TreeHouseSmallFurnitureData;
				if (!(treeHouseSmallFurnitureData == null))
				{
					this.smallFurnitureDataList.Add(treeHouseSmallFurnitureData);
				}
			}
			yield break;
		}
		yield break;
		yield break;
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x00034934 File Offset: 0x00032B34
	public void UpdateUserDataByServer(List<Item> userItemList)
	{
		using (List<Item>.Enumerator enumerator = userItemList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Item item = enumerator.Current;
				if (ItemDef.Id2Kind(item.item_id) == ItemDef.Kind.TREEHOUSE_FURNITURE)
				{
					TreeHouseFurniturePackData treeHouseFurniturePackData = this.UserFurniturePackList.Find((TreeHouseFurniturePackData f) => f.id == item.item_id);
					if (treeHouseFurniturePackData != null)
					{
						treeHouseFurniturePackData.num = item.item_num;
					}
					else
					{
						this.UserFurniturePackList.Add(new TreeHouseFurniturePackData(item.item_id, item.item_num));
					}
				}
			}
		}
		this.KizunaBonusData = new TreeHouseKizunaBonusData(this.UserFurniturePackList, this.mstMasterRoomKizunaBonusList);
		this.StaminaBonusData = new TreeHouseStaminaBonusData(this.UserFurniturePackList, this.mstMasterRoomStaminaBonusList);
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x00034A20 File Offset: 0x00032C20
	private void CbMasterRoomGetDataCmd(Command cmd)
	{
		MasterRoomGetDataResponse masterRoomGetDataResponse = cmd.response as MasterRoomGetDataResponse;
		Request request = cmd.request;
		if (masterRoomGetDataResponse.furniture_list != null)
		{
			this.FurnitureMappingList = masterRoomGetDataResponse.furniture_list.ConvertAll<TreeHouseFurnitureMapping>((MasterRoomFurniture item) => new TreeHouseFurnitureMapping(item));
		}
		if (masterRoomGetDataResponse.chara_list != null)
		{
			this.PutCharaDataList = masterRoomGetDataResponse.chara_list.ConvertAll<TreeHousePutCharaData>((MasterRoomChara item) => new TreeHousePutCharaData(item));
		}
		if (masterRoomGetDataResponse.receive_stamp_log_list != null)
		{
			this.ReceiveStampLog = new TreeHouseReceiveStampLog(masterRoomGetDataResponse);
		}
		if (masterRoomGetDataResponse.myset_list != null)
		{
			this.MysetList = new List<TreeHouseMyset>();
			int id2;
			int id;
			for (id = 1; id <= 4; id = id2 + 1)
			{
				MasterRoomMyset masterRoomMyset = masterRoomGetDataResponse.myset_list.Find((MasterRoomMyset item) => item.myset_id == id);
				this.MysetList.Add(new TreeHouseMyset(id, masterRoomMyset));
				id2 = id;
			}
		}
		if (masterRoomGetDataResponse.follow_list != null)
		{
			this.SocialFollowDataList = masterRoomGetDataResponse.follow_list.ConvertAll<TreeHouseSocialUser>((MasterRoomFollow item) => new TreeHouseSocialUser(item));
		}
		if (masterRoomGetDataResponse.passing_list != null)
		{
			this.SocialPassingDataList = masterRoomGetDataResponse.passing_list.ConvertAll<TreeHouseSocialUser>((MasterRoomPassing item) => new TreeHouseSocialUser(item));
		}
		if (masterRoomGetDataResponse.ranking_list != null)
		{
			this.SocialRankingDataList = masterRoomGetDataResponse.ranking_list.ConvertAll<TreeHouseSocialUser>((MasterRoomRanking item) => new TreeHouseSocialUser(item));
		}
		if (masterRoomGetDataResponse.stamp_log_list != null)
		{
			this.SocialStampHistoryDataList = masterRoomGetDataResponse.stamp_log_list.ConvertAll<TreeHouseSocialUser>((MasterRoomStampLog item) => new TreeHouseSocialUser(item));
		}
		if (masterRoomGetDataResponse.public_info != null)
		{
			this.PublicInfo = new TreeHousePublicInfo(masterRoomGetDataResponse.public_info, masterRoomGetDataResponse.receive_stamp_list);
		}
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		if (homeCheckResult != null)
		{
			homeCheckResult.SetTreeHouseCharge(masterRoomGetDataResponse.master_room_machine_list, masterRoomGetDataResponse.server_time);
		}
		this.parentData.UpdateUserAssetByAssets(masterRoomGetDataResponse.assets);
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x00034C60 File Offset: 0x00032E60
	private void CbMasterRoomSetFurnitureCmd(Command cmd)
	{
		MasterRoomSetFurnitureResponse masterRoomSetFurnitureResponse = cmd.response as MasterRoomSetFurnitureResponse;
		MasterRoomSetFurnitureRequest masterRoomSetFurnitureRequest = cmd.request as MasterRoomSetFurnitureRequest;
		this.FurnitureMappingList = masterRoomSetFurnitureRequest.furniture_list.ConvertAll<TreeHouseFurnitureMapping>((MasterRoomFurniture item) => new TreeHouseFurnitureMapping(item));
		this.parentData.UpdateUserAssetByAssets(masterRoomSetFurnitureResponse.assets);
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		if (homeCheckResult == null)
		{
			return;
		}
		homeCheckResult.SetTreeHouseCharge(masterRoomSetFurnitureResponse.master_room_machine_list, masterRoomSetFurnitureResponse.server_time);
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x00034CE8 File Offset: 0x00032EE8
	private void CbMasterRoomSetCharaCmd(Command cmd)
	{
		Response response = cmd.response;
		MasterRoomSetCharaRequest masterRoomSetCharaRequest = cmd.request as MasterRoomSetCharaRequest;
		this.PutCharaDataList = masterRoomSetCharaRequest.chara_list.ConvertAll<TreeHousePutCharaData>((MasterRoomChara item) => new TreeHousePutCharaData(item));
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x00034D38 File Offset: 0x00032F38
	private void CbMasterRoomVisitCmd(Command cmd)
	{
		MasterRoomVisitResponse masterRoomVisitResponse = cmd.response as MasterRoomVisitResponse;
		MasterRoomVisitRequest req = cmd.request as MasterRoomVisitRequest;
		if (masterRoomVisitResponse.error_type > 0)
		{
			if (DataManager.DmTreeHouse.SocialVisitUserData == null || DataManager.DmTreeHouse.SocialVisitUserData.friendId == 0)
			{
				this.SocialVisitUserData = new TreeHouseSocialVisitUserData(0, null);
				return;
			}
		}
		else
		{
			this.SocialVisitUserData = new TreeHouseSocialVisitUserData(req.friend_id, masterRoomVisitResponse);
			if (this.SocialFollowDataList != null)
			{
				foreach (TreeHouseSocialUser treeHouseSocialUser in this.SocialFollowDataList.FindAll((TreeHouseSocialUser item) => item.friendId == req.friend_id))
				{
					treeHouseSocialUser.isDispNew = false;
				}
			}
			if (this.SocialRankingDataList != null)
			{
				foreach (TreeHouseSocialUser treeHouseSocialUser2 in this.SocialRankingDataList.FindAll((TreeHouseSocialUser item) => item.friendId == req.friend_id))
				{
					treeHouseSocialUser2.isDispNew = false;
				}
			}
			if (this.SocialPassingDataList != null)
			{
				foreach (TreeHouseSocialUser treeHouseSocialUser3 in this.SocialPassingDataList.FindAll((TreeHouseSocialUser item) => item.friendId == req.friend_id))
				{
					treeHouseSocialUser3.isDispNew = false;
				}
			}
			if (this.SocialStampHistoryDataList != null)
			{
				foreach (TreeHouseSocialUser treeHouseSocialUser4 in this.SocialStampHistoryDataList.FindAll((TreeHouseSocialUser item) => item.friendId == req.friend_id))
				{
					treeHouseSocialUser4.isDispNew = false;
				}
			}
		}
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x00034F20 File Offset: 0x00033120
	private void CbMasterRoomSendStampCmd(Command cmd)
	{
		MasterRoomSendStampResponse masterRoomSendStampResponse = cmd.response as MasterRoomSendStampResponse;
		MasterRoomSendStampRequest req = cmd.request as MasterRoomSendStampRequest;
		if (this.SocialVisitUserData != null && this.SocialVisitUserData.friendId == req.friend_id)
		{
			this.SocialVisitUserData.isFinishSendStamp = true;
			if (this.SocialFollowDataList != null)
			{
				foreach (TreeHouseSocialUser treeHouseSocialUser in this.SocialFollowDataList.FindAll((TreeHouseSocialUser item) => item.friendId == req.friend_id))
				{
					treeHouseSocialUser.isFinishSendStamp = true;
				}
			}
			if (this.SocialRankingDataList != null)
			{
				foreach (TreeHouseSocialUser treeHouseSocialUser2 in this.SocialRankingDataList.FindAll((TreeHouseSocialUser item) => item.friendId == req.friend_id))
				{
					treeHouseSocialUser2.isFinishSendStamp = true;
				}
			}
			if (this.SocialPassingDataList != null)
			{
				foreach (TreeHouseSocialUser treeHouseSocialUser3 in this.SocialPassingDataList.FindAll((TreeHouseSocialUser item) => item.friendId == req.friend_id))
				{
					treeHouseSocialUser3.isFinishSendStamp = true;
				}
			}
			if (this.SocialStampHistoryDataList != null)
			{
				foreach (TreeHouseSocialUser treeHouseSocialUser4 in this.SocialStampHistoryDataList.FindAll((TreeHouseSocialUser item) => item.friendId == req.friend_id))
				{
					treeHouseSocialUser4.isFinishSendStamp = true;
				}
			}
		}
		this.parentData.UpdateUserAssetByAssets(masterRoomSendStampResponse.assets);
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x000350FC File Offset: 0x000332FC
	private void CbMasterRoomSaveMysetCmd(Command cmd)
	{
		MasterRoomSaveMysetResponse masterRoomSaveMysetResponse = cmd.response as MasterRoomSaveMysetResponse;
		MasterRoomSaveMysetRequest req = cmd.request as MasterRoomSaveMysetRequest;
		if (this.MysetList != null)
		{
			TreeHouseMyset treeHouseMyset = this.MysetList.Find((TreeHouseMyset item) => item.mysetId == req.myset_id);
			if (treeHouseMyset != null)
			{
				treeHouseMyset.name = masterRoomSaveMysetResponse.myset_name;
				treeHouseMyset.saveTime = new DateTime(PrjUtil.ConvertTimeToTicks(masterRoomSaveMysetResponse.update_time));
				treeHouseMyset.isDataEnable = true;
			}
		}
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x00035178 File Offset: 0x00033378
	private void CbMasterRoomLoadMysetCmd(Command cmd)
	{
		MasterRoomLoadMysetResponse masterRoomLoadMysetResponse = cmd.response as MasterRoomLoadMysetResponse;
		Request request = cmd.request;
		if (this.PublicInfo != null)
		{
			this.PublicInfo.houseName = masterRoomLoadMysetResponse.room_name;
		}
		this.FurnitureMappingList = masterRoomLoadMysetResponse.furniture_list.ConvertAll<TreeHouseFurnitureMapping>((MasterRoomFurniture item) => new TreeHouseFurnitureMapping(item));
		this.parentData.UpdateUserAssetByAssets(masterRoomLoadMysetResponse.assets);
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		if (homeCheckResult == null)
		{
			return;
		}
		homeCheckResult.SetTreeHouseCharge(masterRoomLoadMysetResponse.master_room_machine_list, masterRoomLoadMysetResponse.server_time);
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x00035214 File Offset: 0x00033414
	private void CbMasterRoomPublicSettingCmd(Command cmd)
	{
		MasterRoomPublicSettingResponse masterRoomPublicSettingResponse = cmd.response as MasterRoomPublicSettingResponse;
		MasterRoomPublicSettingRequest masterRoomPublicSettingRequest = cmd.request as MasterRoomPublicSettingRequest;
		this.PublicNameUpdateSuccess = masterRoomPublicSettingResponse.result_room_name == 1;
		this.PublicCommentUpdateSuccess = masterRoomPublicSettingResponse.result_comment == 1;
		if (this.PublicNameUpdateSuccess)
		{
			this.PublicInfo.houseName = masterRoomPublicSettingRequest.public_info.room_name;
		}
		if (this.PublicCommentUpdateSuccess)
		{
			this.PublicInfo.houseComment = masterRoomPublicSettingRequest.public_info.comment;
		}
		this.PublicInfo.publicType = (TreeHousePublicInfo.PublicType)masterRoomPublicSettingRequest.public_info.public_type;
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x000352AC File Offset: 0x000334AC
	public void InsertNewList(List<NewFlg> newFlagList)
	{
		this.FavoriteFurnitureItemIdList = new HashSet<int>();
		foreach (NewFlg newFlg in newFlagList)
		{
			if (newFlg.new_mgmt_flg == 1 && newFlg.category == 13)
			{
				this.FavoriteFurnitureItemIdList.Add(newFlg.any_id);
			}
		}
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x00035324 File Offset: 0x00033524
	private void CbNewFlgUpdateCmd(Command cmd)
	{
		foreach (NewFlg newFlg in (cmd.request as NewFlgUpdateRequest).new_flg_list)
		{
			if (newFlg.category == 13)
			{
				if (newFlg.new_mgmt_flg == 1)
				{
					this.FavoriteFurnitureItemIdList.Add(newFlg.any_id);
				}
				else
				{
					this.FavoriteFurnitureItemIdList.Remove(newFlg.any_id);
				}
			}
		}
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x000353B4 File Offset: 0x000335B4
	private void CbMasterRoomGetMachineDataCmd(Command cmd)
	{
		MasterRoomGetMachineDataResponse masterRoomGetMachineDataResponse = cmd.response as MasterRoomGetMachineDataResponse;
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		if (homeCheckResult != null)
		{
			homeCheckResult.SetTreeHouseCharge(masterRoomGetMachineDataResponse.master_room_machine_list, masterRoomGetMachineDataResponse.server_time);
		}
		this.parentData.UpdateUserAssetByAssets(masterRoomGetMachineDataResponse.assets);
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x00035400 File Offset: 0x00033600
	private void CbMasterRoomAddFuelCmd(Command cmd)
	{
		MasterRoomAddFuelResponse masterRoomAddFuelResponse = cmd.response as MasterRoomAddFuelResponse;
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		if (homeCheckResult != null)
		{
			homeCheckResult.SetTreeHouseCharge(masterRoomAddFuelResponse.master_room_machine_list, masterRoomAddFuelResponse.server_time);
		}
		this.parentData.UpdateUserAssetByAssets(masterRoomAddFuelResponse.assets);
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x0003544C File Offset: 0x0003364C
	private void CbMasterRoomMachineReceiveCmd(Command cmd)
	{
		MasterRoomMachineReceiveResponse masterRoomMachineReceiveResponse = cmd.response as MasterRoomMachineReceiveResponse;
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		if (homeCheckResult != null)
		{
			homeCheckResult.SetTreeHouseCharge(masterRoomMachineReceiveResponse.master_room_machine_list, masterRoomMachineReceiveResponse.server_time);
		}
		this.parentData.UpdateUserAssetByAssets(masterRoomMachineReceiveResponse.assets);
		this.machineReceiveList = masterRoomMachineReceiveResponse.machine_receive_list;
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x000354A3 File Offset: 0x000336A3
	public List<TreeHouseSmallFurnitureData> GetSmallFurnitureData()
	{
		return this.smallFurnitureDataList;
	}

	// Token: 0x040006A1 RID: 1697
	private DataManager parentData;

	// Token: 0x040006A2 RID: 1698
	private Dictionary<int, TreeHouseFurnitureStatic> furnitureStaticMap = new Dictionary<int, TreeHouseFurnitureStatic>();

	// Token: 0x040006A3 RID: 1699
	private List<MstMasterRoomKizunaBonus> mstMasterRoomKizunaBonusList;

	// Token: 0x040006A4 RID: 1700
	private List<MstMasterRoomActionData> mstMasterRoomActionList;

	// Token: 0x040006A5 RID: 1701
	private List<MstMasterRoomMachineData> mstMasterRoomMachineList;

	// Token: 0x040006A6 RID: 1702
	private List<MstMasterRoomFuelData> mstMasterRoomFuelList;

	// Token: 0x040006A7 RID: 1703
	private List<MstMasterRoomStaminaBonus> mstMasterRoomStaminaBonusList;

	// Token: 0x040006A8 RID: 1704
	private List<TreeHouseSmallFurnitureData> smallFurnitureDataList = new List<TreeHouseSmallFurnitureData>();

	// Token: 0x040006B6 RID: 1718
	private List<MasterRoomMachineReceiveModel> machineReceiveList = new List<MasterRoomMachineReceiveModel>();

	// Token: 0x040006BB RID: 1723
	public static readonly Dictionary<TreeHouseFurnitureStatic.Category, string> categoryList = new Dictionary<TreeHouseFurnitureStatic.Category, string>
	{
		{
			TreeHouseFurnitureStatic.Category.LARGE_FURNITURE,
			"大型インテリア"
		},
		{
			TreeHouseFurnitureStatic.Category.STAND,
			"スタンド"
		},
		{
			TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE,
			"雑貨"
		},
		{
			TreeHouseFurnitureStatic.Category.RUG,
			"敷物"
		},
		{
			TreeHouseFurnitureStatic.Category.WALL_HANGINGS,
			"壁掛け"
		},
		{
			TreeHouseFurnitureStatic.Category.CURTAIN,
			"カーテン"
		},
		{
			TreeHouseFurnitureStatic.Category.CEIL_LIGHT,
			"天井照明"
		},
		{
			TreeHouseFurnitureStatic.Category.WALL_PAPER,
			"壁紙"
		},
		{
			TreeHouseFurnitureStatic.Category.FLOOR_PAPER,
			"フロアシート"
		}
	};

	// Token: 0x0200079D RID: 1949
	public enum MachineType
	{
		// Token: 0x040033D2 RID: 13266
		INVALID,
		// Token: 0x040033D3 RID: 13267
		BATTERY,
		// Token: 0x040033D4 RID: 13268
		FURNITURE
	}
}
