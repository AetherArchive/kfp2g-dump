using System;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Login;
using SGNFW.Mst;

public class DataManagerHome
{
	public DataManagerHome(DataManager p)
	{
		this.parentData = p;
	}

	public string AtomInviteUrl { get; private set; }

	public string FriendInviteUrl { get; private set; }

	public string CollaboUrl { get; private set; }

	public HomeFurnitureStatic GetHomeFurnitureStaticData(int id)
	{
		if (!this.homeFurnitureStaticMap.ContainsKey(id))
		{
			Verbose<PrjLog>.LogError("Error : DataManagerHome.GetHomeFurnitureStaticData : 定義されていないID[" + id.ToString() + "]を生成しようとしました", null);
			return null;
		}
		return this.homeFurnitureStaticMap[id];
	}

	public Dictionary<int, HomeFurnitureStatic> GetHomeFurnitureStaticMap()
	{
		return this.homeFurnitureStaticMap;
	}

	public HomeFurnitureCountData GetHomeFurnitureCountData(int itemNum, bool isNext = false)
	{
		int num = 0;
		foreach (HomeFurnitureCountData homeFurnitureCountData in this.homeFurnitureCountDataList)
		{
			if (itemNum <= homeFurnitureCountData.furnitureCount)
			{
				if (itemNum < homeFurnitureCountData.furnitureCount)
				{
					num--;
					break;
				}
				break;
			}
			else
			{
				num++;
			}
		}
		if (isNext)
		{
			num++;
		}
		if (this.homeFurnitureCountDataList.Count <= num)
		{
			return null;
		}
		return this.homeFurnitureCountDataList[num];
	}

	public HomePlacementStatic GetHomePlacementStaticData(int id)
	{
		if (!this.homePlacementStaticMap.ContainsKey(id))
		{
			Verbose<PrjLog>.LogError("Error : DataManagerHome.GetHomePlacementStaticData : 定義されていないID[" + id.ToString() + "]を生成しようとしました", null);
			return null;
		}
		return this.homePlacementStaticMap[id];
	}

	public Dictionary<int, HomePlacementStatic> GetHomePlacementStaticMap()
	{
		return this.homePlacementStaticMap;
	}

	public List<HomeFurniturePackData> GetUserHomeFurnitureList()
	{
		return this.userHomeFurnitureList;
	}

	public List<HomeFurnitureMapping> GetUserHomeeFurnitureMappingList()
	{
		return this.userHomeeFurnitureMappingList;
	}

	public HomeCheckResult GetHomeCheckResult()
	{
		return this.homeCheckResult;
	}

	public void SetHomeCheckResultByDebug(HomeCheckResult hcr)
	{
		this.homeCheckResult = hcr;
	}

	public List<MstBonusData> GetMstLoginBonusList()
	{
		return this.mstLoginBonusList;
	}

	public List<MstBonusPresetData> GetMstLoginBonusPresetList()
	{
		return this.mstLoginBonusPresetList;
	}

	public List<DataManagerHome.HomeBgmPlaybackData> GetMstBgmPlaybackDataList()
	{
		return this.homeBgmPlaybackDataList;
	}

	public List<HomeBannerData> GetHomeBannerList()
	{
		return this.currentHomeBannerDataList;
	}

	public HomeBannerData GetHomeBannerData(int bannerId)
	{
		return this.allHomeBannerDataList.Find((HomeBannerData item) => item.bannerId == bannerId);
	}

	public List<DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo> GetPhotoGrowRewardInfoList(bool isClear)
	{
		List<DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo> list = new List<DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo>(this.photoGrowRewardInfoList);
		if (isClear)
		{
			this.photoGrowRewardInfoList.Clear();
		}
		return list;
	}

	public void RequestActionPutFurniture(List<HomeFurnitureMapping> mapList)
	{
		List<Furniture> list = mapList.ConvertAll<Furniture>((HomeFurnitureMapping item) => new Furniture
		{
			furniture_id = item.furnitureId,
			placement_id = item.placementId
		});
		this.parentData.ServerRequest(FurnitureChangeCmd.Create(list), new Action<Command>(this.CbFurnitureChangeCmd));
	}

	public void RequestCheckHome()
	{
		this.homeCheckResult = null;
		DataManager.DmChara.IsNeedUpdateByUserAllCharaKemoStatusList = false;
		this.parentData.ServerRequest(HomeCheckCmd.Create(DataManager.DmChara.UserAllCharaKemoStatus), new Action<Command>(this.CbHomeCheckCmd));
	}

	public void RequestGetAtomInviteUrl()
	{
		this.parentData.ServerRequest(AtomInviteCmd.Create(), new Action<Command>(this.CbAtomInviteCmd));
	}

	public void RequestGetFriendInviteUrl()
	{
		string text = "";
		this.parentData.ServerRequest(FriendInviteCmd.Create(text), new Action<Command>(this.CbInviteCmd));
	}

	public void RequestGetCollaboUrl()
	{
		this.parentData.ServerRequest(CollaboURLCmd.Create(), new Action<Command>(this.CbCollaboCmd));
	}

	public void InitializeMstData(MstManager mstManager)
	{
		this.mstLoginBonusList = new List<MstBonusData>(mstManager.GetMst<List<MstBonusData>>(MstType.BONUS_DATA));
		this.mstLoginBonusPresetList = new List<MstBonusPresetData>(mstManager.GetMst<List<MstBonusPresetData>>(MstType.BONUS_PRESET_DATA));
		List<MstHomeBgmPlaybackData> mst = mstManager.GetMst<List<MstHomeBgmPlaybackData>>(MstType.HOME_BGM_PLAYBACK_DATA);
		this.homeBgmPlaybackDataList = new List<DataManagerHome.HomeBgmPlaybackData>();
		foreach (MstHomeBgmPlaybackData mstHomeBgmPlaybackData in mst)
		{
			DataManagerHome.HomeBgmPlaybackData homeBgmPlaybackData = new DataManagerHome.HomeBgmPlaybackData(mstHomeBgmPlaybackData);
			this.homeBgmPlaybackDataList.Add(homeBgmPlaybackData);
		}
		List<MstHomeFurnitureData> mst2 = mstManager.GetMst<List<MstHomeFurnitureData>>(MstType.HOME_FURNITURE_DATA);
		List<MstHomeFurnitureCountData> mst3 = mstManager.GetMst<List<MstHomeFurnitureCountData>>(MstType.HOME_FURNITURE_COUNT_DATA);
		List<MstHomePlacementData> mst4 = mstManager.GetMst<List<MstHomePlacementData>>(MstType.HOME_PLACEMENT_DATA);
		this.homeFurnitureStaticMap = new Dictionary<int, HomeFurnitureStatic>();
		foreach (MstHomeFurnitureData mstHomeFurnitureData in mst2)
		{
			this.homeFurnitureStaticMap.Add(mstHomeFurnitureData.id, new HomeFurnitureStatic(mstHomeFurnitureData));
		}
		this.homeFurnitureCountDataList = new List<HomeFurnitureCountData>();
		this.homeFurnitureCountDataList.Add(new HomeFurnitureCountData
		{
			furnitureCount = 0,
			kizunaPointIncrease = 0
		});
		foreach (MstHomeFurnitureCountData mstHomeFurnitureCountData in mst3)
		{
			HomeFurnitureCountData homeFurnitureCountData = new HomeFurnitureCountData
			{
				furnitureCount = mstHomeFurnitureCountData.furnitureCount,
				kizunaPointIncrease = mstHomeFurnitureCountData.kizunaPointIncrease
			};
			this.homeFurnitureCountDataList.Add(homeFurnitureCountData);
		}
		this.homeFurnitureCountDataList.Sort((HomeFurnitureCountData a, HomeFurnitureCountData b) => a.furnitureCount - b.furnitureCount);
		this.homePlacementStaticMap = new Dictionary<int, HomePlacementStatic>();
		foreach (MstHomePlacementData mstHomePlacementData in mst4)
		{
			this.homePlacementStaticMap.Add(mstHomePlacementData.positionId, new HomePlacementStatic(mstHomePlacementData));
		}
		List<ItemStaticBase> list = new List<ItemStaticBase>();
		foreach (HomeFurnitureStatic homeFurnitureStatic in this.homeFurnitureStaticMap.Values)
		{
			list.Add(homeFurnitureStatic);
		}
		DataManager.DmItem.AddMstDataByItem(list);
		List<MstBannerData> mst5 = mstManager.GetMst<List<MstBannerData>>(MstType.BANNER_DATA);
		this.allHomeBannerDataList = new List<HomeBannerData>();
		foreach (MstBannerData mstBannerData in mst5)
		{
			if (mstBannerData.platform == 0 || mstBannerData.platform == LoginManager.Platform)
			{
				this.allHomeBannerDataList.Add(new HomeBannerData(mstBannerData));
			}
		}
		this.allHomeBannerDataList.Sort((HomeBannerData a, HomeBannerData b) => a.priority.CompareTo(b.priority));
		this.UpdateHomeBanner();
	}

	public void UpdateUserDataByServer(List<Item> userItemList, List<Furniture> userFurnitureList, List<RewardInfo> photoRewardInfo)
	{
		this.userHomeFurnitureList = new List<HomeFurniturePackData>();
		foreach (Item item2 in userItemList)
		{
			if (ItemDef.Id2Kind(item2.item_id) == ItemDef.Kind.FURNITURE)
			{
				this.userHomeFurnitureList.Add(new HomeFurniturePackData(item2.item_id, item2.item_num));
			}
		}
		this.userHomeeFurnitureMappingList = new List<HomeFurnitureMapping>();
		foreach (Furniture furniture in userFurnitureList)
		{
			this.userHomeeFurnitureMappingList.Add(new HomeFurnitureMapping
			{
				furnitureId = furniture.furniture_id,
				placementId = furniture.placement_id
			});
		}
		if (photoRewardInfo != null)
		{
			this.photoGrowRewardInfoList = photoRewardInfo.ConvertAll<DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo>((RewardInfo item) => new DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo(item));
		}
	}

	public void UpdateUserDataByServer(List<Item> userItemList)
	{
		using (List<Item>.Enumerator enumerator = userItemList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Item item = enumerator.Current;
				if (ItemDef.Id2Kind(item.item_id) == ItemDef.Kind.FURNITURE)
				{
					HomeFurniturePackData homeFurniturePackData = this.userHomeFurnitureList.Find((HomeFurniturePackData f) => f.id == item.item_id);
					if (homeFurniturePackData != null)
					{
						homeFurniturePackData.num = item.item_num;
					}
					else
					{
						this.userHomeFurnitureList.Add(new HomeFurniturePackData(item.item_id, item.item_num));
					}
				}
			}
		}
	}

	private void UpdateHomeBanner()
	{
		DateTime now = TimeManager.Now;
		this.currentHomeBannerDataList = this.allHomeBannerDataList.FindAll((HomeBannerData item) => item.startTime <= now && now <= item.endTime);
		if (!DataManager.DmServerMst.IsEnableNoahWeb())
		{
			this.currentHomeBannerDataList.RemoveAll((HomeBannerData item) => item.actionType == HomeBannerData.ActionType.NOAH_OFFER);
		}
		this.currentHomeBannerDataList.RemoveAll((HomeBannerData item) => item.actionType == HomeBannerData.ActionType.ATOM_INVITE);
	}

	private void CbHomeCheckCmd(Command cmd)
	{
		HomeCheckResponse homeCheckResponse = cmd.response as HomeCheckResponse;
		this.homeCheckResult = new HomeCheckResult(homeCheckResponse);
		this.parentData.UpdateUserAssetByAssets(homeCheckResponse.assets);
		this.UpdateHomeBanner();
		LocalPushUtil.ResolveNotification(LocalPushUtil.NotificationID.COMEBACK1);
		LocalPushUtil.ResolveNotification(LocalPushUtil.NotificationID.COMEBACK2);
	}

	private void CbAtomInviteCmd(Command cmd)
	{
		AtomInviteResponse atomInviteResponse = cmd.response as AtomInviteResponse;
		this.AtomInviteUrl = atomInviteResponse.invite_url;
	}

	private void CbInviteCmd(Command cmd)
	{
		FriendInviteResponse friendInviteResponse = cmd.response as FriendInviteResponse;
		this.FriendInviteUrl = friendInviteResponse.invite_url;
	}

	private void CbCollaboCmd(Command cmd)
	{
		CollaboURLResponce collaboURLResponce = cmd.response as CollaboURLResponce;
		this.CollaboUrl = collaboURLResponce.collabo_url;
	}

	private void CbFurnitureChangeCmd(Command cmd)
	{
		FurnitureChangeRequest furnitureChangeRequest = cmd.request as FurnitureChangeRequest;
		FurnitureChangeResponse furnitureChangeResponse = cmd.response as FurnitureChangeResponse;
		using (List<Furniture>.Enumerator enumerator = furnitureChangeRequest.furnitures.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Furniture furn = enumerator.Current;
				HomeFurnitureMapping homeFurnitureMapping = this.userHomeeFurnitureMappingList.Find((HomeFurnitureMapping item) => item.placementId == furn.placement_id);
				if (homeFurnitureMapping == null)
				{
					this.userHomeeFurnitureMappingList.Add(new HomeFurnitureMapping
					{
						placementId = furn.placement_id,
						furnitureId = furn.furniture_id
					});
				}
				else
				{
					homeFurnitureMapping.furnitureId = furn.furniture_id;
				}
			}
		}
		this.parentData.UpdateUserAssetByAssets(furnitureChangeResponse.assets);
	}

	private DataManager parentData;

	private Dictionary<int, HomeFurnitureStatic> homeFurnitureStaticMap = new Dictionary<int, HomeFurnitureStatic>();

	private Dictionary<int, HomePlacementStatic> homePlacementStaticMap = new Dictionary<int, HomePlacementStatic>();

	private List<MstBonusData> mstLoginBonusList;

	private List<MstBonusPresetData> mstLoginBonusPresetList;

	private List<DataManagerHome.HomeBgmPlaybackData> homeBgmPlaybackDataList;

	private List<HomeBannerData> allHomeBannerDataList;

	private List<HomeBannerData> currentHomeBannerDataList = new List<HomeBannerData>();

	private List<HomeFurnitureCountData> homeFurnitureCountDataList;

	private HomeCheckResult homeCheckResult;

	private List<DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo> photoGrowRewardInfoList = new List<DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo>();

	private List<HomeFurniturePackData> userHomeFurnitureList;

	private List<HomeFurnitureMapping> userHomeeFurnitureMappingList;

	public class HomeBgmPlaybackData
	{
		public HomeBgmPlaybackData(MstHomeBgmPlaybackData mstHomeBgmPlaybackData)
		{
			this.id = mstHomeBgmPlaybackData.id;
			this.name = mstHomeBgmPlaybackData.name;
			this.imgPath = mstHomeBgmPlaybackData.imgPath;
			this.fileName = mstHomeBgmPlaybackData.fileName;
			this.sortNum = mstHomeBgmPlaybackData.sortNum;
			this.anyTime = mstHomeBgmPlaybackData.anyTime;
			this.questId = mstHomeBgmPlaybackData.questId;
			this.level = mstHomeBgmPlaybackData.level;
			this.friendsCount = mstHomeBgmPlaybackData.friendsCount;
			this.startDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mstHomeBgmPlaybackData.startDatetime));
			this.endDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mstHomeBgmPlaybackData.endDatetime));
		}

		public int id;

		public string name;

		public string imgPath;

		public string fileName;

		public int sortNum;

		public int anyTime;

		public int questId;

		public int level;

		public int friendsCount;

		public DateTime startDatetime;

		public DateTime endDatetime;
	}
}
