using System;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Login;
using SGNFW.Mst;

// Token: 0x0200007F RID: 127
public class DataManagerHome
{
	// Token: 0x060004C1 RID: 1217 RVA: 0x00022698 File Offset: 0x00020898
	public DataManagerHome(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x060004C2 RID: 1218 RVA: 0x000226D3 File Offset: 0x000208D3
	// (set) Token: 0x060004C3 RID: 1219 RVA: 0x000226DB File Offset: 0x000208DB
	public string AtomInviteUrl { get; private set; }

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x060004C4 RID: 1220 RVA: 0x000226E4 File Offset: 0x000208E4
	// (set) Token: 0x060004C5 RID: 1221 RVA: 0x000226EC File Offset: 0x000208EC
	public string FriendInviteUrl { get; private set; }

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x060004C6 RID: 1222 RVA: 0x000226F5 File Offset: 0x000208F5
	// (set) Token: 0x060004C7 RID: 1223 RVA: 0x000226FD File Offset: 0x000208FD
	public string CollaboUrl { get; private set; }

	// Token: 0x060004C8 RID: 1224 RVA: 0x00022706 File Offset: 0x00020906
	public HomeFurnitureStatic GetHomeFurnitureStaticData(int id)
	{
		if (!this.homeFurnitureStaticMap.ContainsKey(id))
		{
			Verbose<PrjLog>.LogError("Error : DataManagerHome.GetHomeFurnitureStaticData : 定義されていないID[" + id.ToString() + "]を生成しようとしました", null);
			return null;
		}
		return this.homeFurnitureStaticMap[id];
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x00022740 File Offset: 0x00020940
	public Dictionary<int, HomeFurnitureStatic> GetHomeFurnitureStaticMap()
	{
		return this.homeFurnitureStaticMap;
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x00022748 File Offset: 0x00020948
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

	// Token: 0x060004CB RID: 1227 RVA: 0x000227D8 File Offset: 0x000209D8
	public HomePlacementStatic GetHomePlacementStaticData(int id)
	{
		if (!this.homePlacementStaticMap.ContainsKey(id))
		{
			Verbose<PrjLog>.LogError("Error : DataManagerHome.GetHomePlacementStaticData : 定義されていないID[" + id.ToString() + "]を生成しようとしました", null);
			return null;
		}
		return this.homePlacementStaticMap[id];
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x00022812 File Offset: 0x00020A12
	public Dictionary<int, HomePlacementStatic> GetHomePlacementStaticMap()
	{
		return this.homePlacementStaticMap;
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x0002281A File Offset: 0x00020A1A
	public List<HomeFurniturePackData> GetUserHomeFurnitureList()
	{
		return this.userHomeFurnitureList;
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x00022822 File Offset: 0x00020A22
	public List<HomeFurnitureMapping> GetUserHomeeFurnitureMappingList()
	{
		return this.userHomeeFurnitureMappingList;
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x0002282A File Offset: 0x00020A2A
	public HomeCheckResult GetHomeCheckResult()
	{
		return this.homeCheckResult;
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x00022832 File Offset: 0x00020A32
	public void SetHomeCheckResultByDebug(HomeCheckResult hcr)
	{
		this.homeCheckResult = hcr;
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x0002283B File Offset: 0x00020A3B
	public List<MstBonusData> GetMstLoginBonusList()
	{
		return this.mstLoginBonusList;
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x00022843 File Offset: 0x00020A43
	public List<MstBonusPresetData> GetMstLoginBonusPresetList()
	{
		return this.mstLoginBonusPresetList;
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x0002284B File Offset: 0x00020A4B
	public List<DataManagerHome.HomeBgmPlaybackData> GetMstBgmPlaybackDataList()
	{
		return this.homeBgmPlaybackDataList;
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00022853 File Offset: 0x00020A53
	public List<HomeBannerData> GetHomeBannerList()
	{
		return this.currentHomeBannerDataList;
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x0002285C File Offset: 0x00020A5C
	public HomeBannerData GetHomeBannerData(int bannerId)
	{
		return this.allHomeBannerDataList.Find((HomeBannerData item) => item.bannerId == bannerId);
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x0002288D File Offset: 0x00020A8D
	public List<DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo> GetPhotoGrowRewardInfoList(bool isClear)
	{
		List<DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo> list = new List<DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo>(this.photoGrowRewardInfoList);
		if (isClear)
		{
			this.photoGrowRewardInfoList.Clear();
		}
		return list;
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x000228A8 File Offset: 0x00020AA8
	public void RequestActionPutFurniture(List<HomeFurnitureMapping> mapList)
	{
		List<Furniture> list = mapList.ConvertAll<Furniture>((HomeFurnitureMapping item) => new Furniture
		{
			furniture_id = item.furnitureId,
			placement_id = item.placementId
		});
		this.parentData.ServerRequest(FurnitureChangeCmd.Create(list), new Action<Command>(this.CbFurnitureChangeCmd));
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x000228F8 File Offset: 0x00020AF8
	public void RequestCheckHome()
	{
		this.homeCheckResult = null;
		DataManager.DmChara.IsNeedUpdateByUserAllCharaKemoStatusList = false;
		this.parentData.ServerRequest(HomeCheckCmd.Create(DataManager.DmChara.UserAllCharaKemoStatus), new Action<Command>(this.CbHomeCheckCmd));
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x00022932 File Offset: 0x00020B32
	public void RequestGetAtomInviteUrl()
	{
		this.parentData.ServerRequest(AtomInviteCmd.Create(), new Action<Command>(this.CbAtomInviteCmd));
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x00022950 File Offset: 0x00020B50
	public void RequestGetFriendInviteUrl()
	{
		string text = "";
		this.parentData.ServerRequest(FriendInviteCmd.Create(text), new Action<Command>(this.CbInviteCmd));
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x00022980 File Offset: 0x00020B80
	public void RequestGetCollaboUrl()
	{
		this.parentData.ServerRequest(CollaboURLCmd.Create(), new Action<Command>(this.CbCollaboCmd));
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x000229A0 File Offset: 0x00020BA0
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

	// Token: 0x060004DD RID: 1245 RVA: 0x00022CB8 File Offset: 0x00020EB8
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

	// Token: 0x060004DE RID: 1246 RVA: 0x00022DCC File Offset: 0x00020FCC
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

	// Token: 0x060004DF RID: 1247 RVA: 0x00022E8C File Offset: 0x0002108C
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

	// Token: 0x060004E0 RID: 1248 RVA: 0x00022F2C File Offset: 0x0002112C
	private void CbHomeCheckCmd(Command cmd)
	{
		HomeCheckResponse homeCheckResponse = cmd.response as HomeCheckResponse;
		this.homeCheckResult = new HomeCheckResult(homeCheckResponse);
		this.parentData.UpdateUserAssetByAssets(homeCheckResponse.assets);
		this.UpdateHomeBanner();
		LocalPushUtil.ResolveNotification(LocalPushUtil.NotificationID.COMEBACK1);
		LocalPushUtil.ResolveNotification(LocalPushUtil.NotificationID.COMEBACK2);
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x00022F74 File Offset: 0x00021174
	private void CbAtomInviteCmd(Command cmd)
	{
		AtomInviteResponse atomInviteResponse = cmd.response as AtomInviteResponse;
		this.AtomInviteUrl = atomInviteResponse.invite_url;
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x00022F9C File Offset: 0x0002119C
	private void CbInviteCmd(Command cmd)
	{
		FriendInviteResponse friendInviteResponse = cmd.response as FriendInviteResponse;
		this.FriendInviteUrl = friendInviteResponse.invite_url;
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x00022FC4 File Offset: 0x000211C4
	private void CbCollaboCmd(Command cmd)
	{
		CollaboURLResponce collaboURLResponce = cmd.response as CollaboURLResponce;
		this.CollaboUrl = collaboURLResponce.collabo_url;
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x00022FEC File Offset: 0x000211EC
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

	// Token: 0x04000534 RID: 1332
	private DataManager parentData;

	// Token: 0x04000535 RID: 1333
	private Dictionary<int, HomeFurnitureStatic> homeFurnitureStaticMap = new Dictionary<int, HomeFurnitureStatic>();

	// Token: 0x04000536 RID: 1334
	private Dictionary<int, HomePlacementStatic> homePlacementStaticMap = new Dictionary<int, HomePlacementStatic>();

	// Token: 0x04000537 RID: 1335
	private List<MstBonusData> mstLoginBonusList;

	// Token: 0x04000538 RID: 1336
	private List<MstBonusPresetData> mstLoginBonusPresetList;

	// Token: 0x04000539 RID: 1337
	private List<DataManagerHome.HomeBgmPlaybackData> homeBgmPlaybackDataList;

	// Token: 0x0400053A RID: 1338
	private List<HomeBannerData> allHomeBannerDataList;

	// Token: 0x0400053B RID: 1339
	private List<HomeBannerData> currentHomeBannerDataList = new List<HomeBannerData>();

	// Token: 0x0400053C RID: 1340
	private List<HomeFurnitureCountData> homeFurnitureCountDataList;

	// Token: 0x0400053D RID: 1341
	private HomeCheckResult homeCheckResult;

	// Token: 0x0400053E RID: 1342
	private List<DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo> photoGrowRewardInfoList = new List<DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo>();

	// Token: 0x04000542 RID: 1346
	private List<HomeFurniturePackData> userHomeFurnitureList;

	// Token: 0x04000543 RID: 1347
	private List<HomeFurnitureMapping> userHomeeFurnitureMappingList;

	// Token: 0x020006BE RID: 1726
	public class HomeBgmPlaybackData
	{
		// Token: 0x060032E3 RID: 13027 RVA: 0x001C0EC4 File Offset: 0x001BF0C4
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

		// Token: 0x0400304E RID: 12366
		public int id;

		// Token: 0x0400304F RID: 12367
		public string name;

		// Token: 0x04003050 RID: 12368
		public string imgPath;

		// Token: 0x04003051 RID: 12369
		public string fileName;

		// Token: 0x04003052 RID: 12370
		public int sortNum;

		// Token: 0x04003053 RID: 12371
		public int anyTime;

		// Token: 0x04003054 RID: 12372
		public int questId;

		// Token: 0x04003055 RID: 12373
		public int level;

		// Token: 0x04003056 RID: 12374
		public int friendsCount;

		// Token: 0x04003057 RID: 12375
		public DateTime startDatetime;

		// Token: 0x04003058 RID: 12376
		public DateTime endDatetime;
	}
}
