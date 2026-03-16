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

public class DataManagerTreeHouse
{
	public DataManagerTreeHouse(DataManager p)
	{
		this.parentData = p;
	}

	public List<MstMasterRoomMachineData> GetMstMasterRoomMachineList()
	{
		return this.mstMasterRoomMachineList;
	}

	public TreeHouseFurnitureStatic GetTreeHouseFurnitureStaticData(int id)
	{
		return this.furnitureStaticMap.TryGetValueEx(id, null);
	}

	public List<TreeHouseFurnitureStatic> GetTreeHouseFurnitureStaticData()
	{
		return new List<TreeHouseFurnitureStatic>(this.furnitureStaticMap.Values);
	}

	public bool IsSpecialActionByChara(int actionId, int charaId, int clothItemId)
	{
		return this.mstMasterRoomActionList.Exists((MstMasterRoomActionData item) => item.masterRoomActionId == actionId && item.charaId == charaId && item.clothesItemId == clothItemId);
	}

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

	public List<MstMasterRoomFuelData> GetChargeFuelData()
	{
		return this.mstMasterRoomFuelList;
	}

	public List<TreeHouseFurniturePackData> UserFurniturePackList { get; private set; } = new List<TreeHouseFurniturePackData>();

	public TreeHouseKizunaBonusData KizunaBonusData { get; private set; }

	public TreeHouseStaminaBonusData StaminaBonusData { get; private set; }

	public HashSet<int> FavoriteFurnitureItemIdList { get; private set; }

	public List<TreeHouseFurnitureMapping> FurnitureMappingList { get; private set; }

	public List<TreeHousePutCharaData> PutCharaDataList { get; private set; }

	public List<TreeHouseMyset> MysetList { get; private set; }

	public TreeHouseReceiveStampLog ReceiveStampLog { get; private set; }

	public List<TreeHouseSocialUser> SocialFollowDataList { get; private set; }

	public List<TreeHouseSocialUser> SocialRankingDataList { get; private set; }

	public List<TreeHouseSocialUser> SocialPassingDataList { get; private set; }

	public List<TreeHouseSocialUser> SocialStampHistoryDataList { get; private set; }

	public TreeHousePublicInfo PublicInfo { get; private set; }

	public List<MasterRoomMachineReceiveModel> GetMachineReceiveList()
	{
		return this.machineReceiveList;
	}

	public TreeHouseSocialVisitUserData SocialVisitUserData { get; private set; }

	public bool PublicNameUpdateSuccess { get; private set; }

	public bool PublicCommentUpdateSuccess { get; private set; }

	public int ShopId { get; private set; }

	public void RequestGetTreeHouseBase(bool getStampLog)
	{
		this.parentData.ServerRequest(MasterRoomGetDataCmd.Create(1, 1, getStampLog ? 1 : 0, 1, 0, 0, 0, 0, 1), new Action<Command>(this.CbMasterRoomGetDataCmd));
	}

	public void RequestGetSocialTabData(TreeHouseSocialTabType tabType = TreeHouseSocialTabType.INVALID)
	{
		this.parentData.ServerRequest(MasterRoomGetDataCmd.Create(0, 0, 0, 0, (tabType == TreeHouseSocialTabType.INVALID || tabType == TreeHouseSocialTabType.FOLLOW) ? 1 : 0, (tabType == TreeHouseSocialTabType.INVALID || tabType == TreeHouseSocialTabType.PASSING) ? 1 : 0, (tabType == TreeHouseSocialTabType.INVALID || tabType == TreeHouseSocialTabType.RANKING) ? 1 : 0, (tabType == TreeHouseSocialTabType.INVALID || tabType == TreeHouseSocialTabType.STAMP_HISTORY) ? 1 : 0, (tabType == TreeHouseSocialTabType.INVALID || tabType == TreeHouseSocialTabType.PUBLIC) ? 1 : 0), new Action<Command>(this.CbMasterRoomGetDataCmd));
	}

	public void RequestGetCharaListData()
	{
		this.parentData.ServerRequest(MasterRoomGetDataCmd.Create(0, 1, 0, 1, 0, 0, 0, 0, 0), new Action<Command>(this.CbMasterRoomGetDataCmd));
	}

	public void RequestActionPutFurniture(List<TreeHouseFurnitureMapping> mappingList)
	{
		this.parentData.ServerRequest(MasterRoomSetFurnitureCmd.Create(mappingList.ConvertAll<MasterRoomFurniture>((TreeHouseFurnitureMapping item) => item.ConvertServerData())), new Action<Command>(this.CbMasterRoomSetFurnitureCmd));
	}

	public void RequestActionSetChara(List<TreeHousePutCharaData> putCharaList)
	{
		this.parentData.ServerRequest(MasterRoomSetCharaCmd.Create(putCharaList.ConvertAll<MasterRoomChara>((TreeHousePutCharaData item) => item.ConvertServerData())), new Action<Command>(this.CbMasterRoomSetCharaCmd));
	}

	public void RequestActionOtherVisit(int friendId, TreeHouseSocialTabType tabType)
	{
		this.parentData.ServerRequest(MasterRoomVisitCmd.Create(friendId, (int)tabType), new Action<Command>(this.CbMasterRoomVisitCmd));
	}

	public void RequestActionSendStamp(int friendId, int stampId, TreeHouseSocialTabType tabType)
	{
		this.parentData.ServerRequest(MasterRoomSendStampCmd.Create(friendId, stampId, (int)tabType), new Action<Command>(this.CbMasterRoomSendStampCmd));
	}

	public void RequestActionSaveByMyset(int saveMysetId)
	{
		this.parentData.ServerRequest(MasterRoomSaveMysetCmd.Create(saveMysetId), new Action<Command>(this.CbMasterRoomSaveMysetCmd));
	}

	public void RequestActionLoadByMyset(int loadMysetId)
	{
		this.parentData.ServerRequest(MasterRoomLoadMysetCmd.Create(loadMysetId), new Action<Command>(this.CbMasterRoomLoadMysetCmd));
	}

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

	public void RequestActionUpdateMachineData()
	{
		this.parentData.ServerRequest(MasterRoomGetMachineDataCmd.Create(), new Action<Command>(this.CbMasterRoomGetMachineDataCmd));
	}

	public void RequestActionUseFuel(List<UseItem> use_items)
	{
		this.parentData.ServerRequest(MasterRoomAddFuelCmd.Create(use_items), new Action<Command>(this.CbMasterRoomAddFuelCmd));
	}

	public void RequestActionReceiveMcnItem(bool is_all, int furniture_index)
	{
		this.parentData.ServerRequest(MasterRoomMachineReceiveCmd.Create(is_all, furniture_index), new Action<Command>(this.CbMasterRoomMachineReceiveCmd));
	}

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

	private void CbMasterRoomSetCharaCmd(Command cmd)
	{
		Response response = cmd.response;
		MasterRoomSetCharaRequest masterRoomSetCharaRequest = cmd.request as MasterRoomSetCharaRequest;
		this.PutCharaDataList = masterRoomSetCharaRequest.chara_list.ConvertAll<TreeHousePutCharaData>((MasterRoomChara item) => new TreeHousePutCharaData(item));
	}

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

	public List<TreeHouseSmallFurnitureData> GetSmallFurnitureData()
	{
		return this.smallFurnitureDataList;
	}

	private DataManager parentData;

	private Dictionary<int, TreeHouseFurnitureStatic> furnitureStaticMap = new Dictionary<int, TreeHouseFurnitureStatic>();

	private List<MstMasterRoomKizunaBonus> mstMasterRoomKizunaBonusList;

	private List<MstMasterRoomActionData> mstMasterRoomActionList;

	private List<MstMasterRoomMachineData> mstMasterRoomMachineList;

	private List<MstMasterRoomFuelData> mstMasterRoomFuelList;

	private List<MstMasterRoomStaminaBonus> mstMasterRoomStaminaBonusList;

	private List<TreeHouseSmallFurnitureData> smallFurnitureDataList = new List<TreeHouseSmallFurnitureData>();

	private List<MasterRoomMachineReceiveModel> machineReceiveList = new List<MasterRoomMachineReceiveModel>();

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

	public enum MachineType
	{
		INVALID,
		BATTERY,
		FURNITURE
	}
}
