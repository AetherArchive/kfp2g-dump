using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x0200007B RID: 123
public class DataManagerGameStatus
{
	// Token: 0x0600048C RID: 1164 RVA: 0x0002150D File Offset: 0x0001F70D
	public DataManagerGameStatus(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x0600048D RID: 1165 RVA: 0x00021527 File Offset: 0x0001F727
	// (set) Token: 0x0600048E RID: 1166 RVA: 0x0002152F File Offset: 0x0001F72F
	private List<DataManagerGameStatus.UserFlagData.OneParamData> ParamDataList { get; set; } = new List<DataManagerGameStatus.UserFlagData.OneParamData>();

	// Token: 0x0600048F RID: 1167 RVA: 0x00021538 File Offset: 0x0001F738
	public DataManagerGameStatus.UserFlagData MakeUserFlagData()
	{
		return new DataManagerGameStatus.UserFlagData(this.ParamDataList);
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x00021545 File Offset: 0x0001F745
	public KemoStatusRankingData GetKemoStatusRankingData()
	{
		return this.kemoStatusRankingData;
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x00021550 File Offset: 0x0001F750
	public void RequestActionUpdateSortType(List<DataManagerGameStatus.UserFlagData.SortTypeData> sortTypeDataList)
	{
		if (this.ParamDataList == null)
		{
			return;
		}
		if (sortTypeDataList == null)
		{
			return;
		}
		if (sortTypeDataList.Count == 0)
		{
			return;
		}
		List<DataManagerGameStatus.UserFlagData.OneParamData> list = this.ParamDataList.ConvertAll<DataManagerGameStatus.UserFlagData.OneParamData>((DataManagerGameStatus.UserFlagData.OneParamData item) => new DataManagerGameStatus.UserFlagData.OneParamData(item));
		using (List<DataManagerGameStatus.UserFlagData.SortTypeData>.Enumerator enumerator = sortTypeDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DataManagerGameStatus.UserFlagData.SortTypeData sortTypeData = enumerator.Current;
				DataManagerGameStatus.UserFlagData.OneParamData oneParamData = list.Find((DataManagerGameStatus.UserFlagData.OneParamData x) => x.Category == DataManager.NewFlgCategory.SORTTYPE && x.Id == (int)sortTypeData.RegisterType);
				if (oneParamData == null)
				{
					DataManagerGameStatus.UserFlagData.OneParamData oneParamData2 = new DataManagerGameStatus.UserFlagData.OneParamData((int)sortTypeData.RegisterType, DataManager.NewFlgCategory.SORTTYPE, (int)(sortTypeData.SortType * (sortTypeData.Order ? SortFilterDefine.SortType.LEVEL : ((SortFilterDefine.SortType)(-1)))));
					list.Add(oneParamData2);
				}
				else
				{
					DataManagerGameStatus.UserFlagData.OneParamData oneParamData3 = new DataManagerGameStatus.UserFlagData.OneParamData((int)sortTypeData.RegisterType, DataManager.NewFlgCategory.SORTTYPE, (int)(sortTypeData.SortType * (sortTypeData.Order ? SortFilterDefine.SortType.LEVEL : ((SortFilterDefine.SortType)(-1)))));
					oneParamData.Category = oneParamData3.Category;
					oneParamData.Id = oneParamData3.Id;
					oneParamData.Value = oneParamData3.Value;
				}
			}
		}
		List<NewFlg> list2 = new DataManagerGameStatus.UserFlagData(list).CreateServerRequestData(this.ParamDataList);
		if (list2.Count == 0)
		{
			return;
		}
		this.parentData.ServerRequest(NewFlgUpdateCmd.Create(list2), new Action<Command>(this.CbNewFlgUpdateCmd));
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x000216D0 File Offset: 0x0001F8D0
	public void RequestActionUpdateIconsideIndex(SortFilterDefine.IconPlace iconPlace, int sizeIndex)
	{
		if (this.ParamDataList == null)
		{
			return;
		}
		List<DataManagerGameStatus.UserFlagData.OneParamData> list = this.ParamDataList.ConvertAll<DataManagerGameStatus.UserFlagData.OneParamData>((DataManagerGameStatus.UserFlagData.OneParamData item) => new DataManagerGameStatus.UserFlagData.OneParamData(item));
		DataManagerGameStatus.UserFlagData.OneParamData oneParamData = list.Find((DataManagerGameStatus.UserFlagData.OneParamData x) => x.Category == DataManager.NewFlgCategory.IconSizeIndex && x.Id == (int)iconPlace);
		if (oneParamData == null)
		{
			DataManagerGameStatus.UserFlagData.OneParamData oneParamData2 = new DataManagerGameStatus.UserFlagData.OneParamData((int)iconPlace, DataManager.NewFlgCategory.IconSizeIndex, sizeIndex);
			list.Add(oneParamData2);
		}
		else
		{
			DataManagerGameStatus.UserFlagData.OneParamData oneParamData3 = new DataManagerGameStatus.UserFlagData.OneParamData((int)iconPlace, DataManager.NewFlgCategory.IconSizeIndex, sizeIndex);
			oneParamData.Category = oneParamData3.Category;
			oneParamData.Id = oneParamData3.Id;
			oneParamData.Value = oneParamData3.Value;
		}
		List<NewFlg> list2 = new DataManagerGameStatus.UserFlagData(list).CreateServerRequestData(this.ParamDataList);
		if (list2.Count == 0)
		{
			return;
		}
		this.parentData.ServerRequest(NewFlgUpdateCmd.Create(list2), new Action<Command>(this.CbNewFlgUpdateCmd));
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x000217C0 File Offset: 0x0001F9C0
	public void RequestActionUpdateUserFlag(DataManagerGameStatus.UserFlagData ufd)
	{
		if (this.ParamDataList == null)
		{
			return;
		}
		List<NewFlg> list = ufd.CreateServerRequestData(this.ParamDataList);
		if (list.Count == 0)
		{
			return;
		}
		this.parentData.ServerRequest(NewFlgUpdateCmd.Create(list), new Action<Command>(this.CbNewFlgUpdateCmd));
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x00021809 File Offset: 0x0001FA09
	public void RequestActionShowMovie(MstMovieData mmd)
	{
		if (mmd == null)
		{
			return;
		}
		this.parentData.ServerRequest(PlayVideoCmd.Create(mmd.movieId), null);
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x00021828 File Offset: 0x0001FA28
	public void RequestGetKemoStatusRanking()
	{
		long num = 0L;
		if (this.kemoStatusRankingData != null)
		{
			num = PrjUtil.ConvertTicksToTime(this.kemoStatusRankingData.lastUpdateTime.Ticks);
		}
		this.parentData.ServerRequest(KemostatusRankingCmd.Create(num), new Action<Command>(this.CbKemostatusRankingCmd));
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x00021878 File Offset: 0x0001FA78
	private void CbNewFlgUpdateCmd(Command cmd)
	{
		NewFlgUpdateRequest newFlgUpdateRequest = cmd.request as NewFlgUpdateRequest;
		this.UpdateUserFlagByServer(newFlgUpdateRequest.new_flg_list);
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x000218A0 File Offset: 0x0001FAA0
	private void CbKemostatusRankingCmd(Command cmd)
	{
		KemostatusRankingResponse kemostatusRankingResponse = cmd.response as KemostatusRankingResponse;
		if (this.kemoStatusRankingData != null && this.kemoStatusRankingData.lastUpdateTime.Ticks == PrjUtil.ConvertTimeToTicks(kemostatusRankingResponse.last_update_time))
		{
			return;
		}
		this.kemoStatusRankingData = new KemoStatusRankingData(kemostatusRankingResponse.last_update_time);
		this.kemoStatusRankingData.myRank = kemostatusRankingResponse.myrank;
		foreach (KemostatusRankData kemostatusRankData in kemostatusRankingResponse.kemostatus_ranking)
		{
			this.kemoStatusRankingData.rankingList.Add(new KemoStatusRankingData.RankingOne
			{
				number = kemostatusRankData.rank,
				kemoStatus = (long)kemostatusRankData.kemostatus,
				userName = kemostatusRankData.user_name,
				userLevel = kemostatusRankData.user_level,
				favoriteCharaId = kemostatusRankData.favorite_chara_id,
				favoriteCharaFaceId = kemostatusRankData.favorite_chara_face_id,
				achievementId = kemostatusRankData.achievement_id
			});
		}
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x000219B0 File Offset: 0x0001FBB0
	public void UpdateUserFlagByServer(List<NewFlg> newFlagList)
	{
		if (this.ParamDataList == null)
		{
			this.ParamDataList = new List<DataManagerGameStatus.UserFlagData.OneParamData>();
		}
		this.ParamDataList.RemoveAll((DataManagerGameStatus.UserFlagData.OneParamData paramData) => newFlagList.Exists((NewFlg newFlag) => paramData.IsEqualKey(newFlag)));
		foreach (NewFlg newFlg in newFlagList)
		{
			this.ParamDataList.Add(new DataManagerGameStatus.UserFlagData.OneParamData(newFlg.any_id, newFlg.category, newFlg.new_mgmt_flg));
		}
	}

	// Token: 0x04000513 RID: 1299
	private DataManager parentData;

	// Token: 0x04000515 RID: 1301
	private KemoStatusRankingData kemoStatusRankingData;

	// Token: 0x020006AA RID: 1706
	public class UserFlagData
	{
		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06003295 RID: 12949 RVA: 0x001BF5C2 File Offset: 0x001BD7C2
		// (set) Token: 0x06003296 RID: 12950 RVA: 0x001BF5CA File Offset: 0x001BD7CA
		public DataManagerGameStatus.UserFlagData.TutorialFinish TutorialFinishFlag { get; set; }

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06003297 RID: 12951 RVA: 0x001BF5D3 File Offset: 0x001BD7D3
		// (set) Token: 0x06003298 RID: 12952 RVA: 0x001BF5DB File Offset: 0x001BD7DB
		public DataManagerGameStatus.UserFlagData.InformationFlag InformationsFlag { get; set; }

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06003299 RID: 12953 RVA: 0x001BF5E4 File Offset: 0x001BD7E4
		// (set) Token: 0x0600329A RID: 12954 RVA: 0x001BF5EC File Offset: 0x001BD7EC
		public DataManagerGameStatus.UserFlagData.ReleaseMode ReleaseModeFlag { get; set; }

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x0600329B RID: 12955 RVA: 0x001BF5F5 File Offset: 0x001BD7F5
		// (set) Token: 0x0600329C RID: 12956 RVA: 0x001BF5FD File Offset: 0x001BD7FD
		private List<DataManagerGameStatus.UserFlagData.SortTypeData> SortTypeDataList { get; set; }

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x0600329D RID: 12957 RVA: 0x001BF606 File Offset: 0x001BD806
		// (set) Token: 0x0600329E RID: 12958 RVA: 0x001BF60E File Offset: 0x001BD80E
		private List<DataManagerGameStatus.UserFlagData.IconSizeData> IconSizeDataList { get; set; }

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x0600329F RID: 12959 RVA: 0x001BF617 File Offset: 0x001BD817
		// (set) Token: 0x060032A0 RID: 12960 RVA: 0x001BF61F File Offset: 0x001BD81F
		public DataManagerGameStatus.UserFlagData.GachaNewInfo GachaNewInfoData { get; private set; }

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x060032A1 RID: 12961 RVA: 0x001BF628 File Offset: 0x001BD828
		// (set) Token: 0x060032A2 RID: 12962 RVA: 0x001BF630 File Offset: 0x001BD830
		public DataManagerGameStatus.UserFlagData.CharaGrowTutorial CharaGrowTutorialFlag { get; set; }

		// Token: 0x060032A3 RID: 12963 RVA: 0x001BF63C File Offset: 0x001BD83C
		public DataManagerGameStatus.UserFlagData.SortTypeData GetSortTypeData(SortFilterDefine.RegisterType rType)
		{
			List<DataManagerGameStatus.UserFlagData.SortTypeData> sortTypeDataList = this.SortTypeDataList;
			DataManagerGameStatus.UserFlagData.SortTypeData sortTypeData = ((sortTypeDataList != null) ? sortTypeDataList.Find((DataManagerGameStatus.UserFlagData.SortTypeData x) => x.RegisterType == rType) : null);
			if (sortTypeData == null || sortTypeData.SortType == SortFilterDefine.SortType.INVALID)
			{
				SortFilterDefine.SortType sortType = (SortFilterDefine.DefaultSortTypeMap.ContainsKey(rType) ? SortFilterDefine.DefaultSortTypeMap[rType] : SortFilterDefine.SortType.LEVEL);
				sortTypeData = new DataManagerGameStatus.UserFlagData.SortTypeData(rType, sortType, true);
			}
			else
			{
				sortTypeData = new DataManagerGameStatus.UserFlagData.SortTypeData(sortTypeData.RegisterType, sortTypeData.SortType, sortTypeData.Order);
			}
			return sortTypeData;
		}

		// Token: 0x060032A4 RID: 12964 RVA: 0x001BF6D0 File Offset: 0x001BD8D0
		public DataManagerGameStatus.UserFlagData.IconSizeData GetIconSizeData(SortFilterDefine.IconPlace place)
		{
			DataManagerGameStatus.UserFlagData.<>c__DisplayClass29_0 CS$<>8__locals1 = new DataManagerGameStatus.UserFlagData.<>c__DisplayClass29_0();
			CS$<>8__locals1.place = place;
			List<DataManagerGameStatus.UserFlagData.IconSizeData> iconSizeDataList = this.IconSizeDataList;
			DataManagerGameStatus.UserFlagData.IconSizeData iconSizeData = ((iconSizeDataList != null) ? iconSizeDataList.Find((DataManagerGameStatus.UserFlagData.IconSizeData x) => x.IconPlace == CS$<>8__locals1.place) : null);
			if (iconSizeData == null || iconSizeData.IconPlace == SortFilterDefine.IconPlace.Invalid)
			{
				UserOptionData userOptionData = DataManager.DmUserInfo.optionData.Clone();
				int num;
				switch (CS$<>8__locals1.place)
				{
				case SortFilterDefine.IconPlace.PhotoAll:
					num = CS$<>8__locals1.<GetIconSizeData>g__IconSizeCheck|0(userOptionData.photoIconSizeAllView, CS$<>8__locals1.place);
					break;
				case SortFilterDefine.IconPlace.PhotoGrow:
					num = CS$<>8__locals1.<GetIconSizeData>g__IconSizeCheck|0(userOptionData.photoIconSizeGrow, CS$<>8__locals1.place);
					break;
				case SortFilterDefine.IconPlace.PhotoGrowMaterial:
					num = CS$<>8__locals1.<GetIconSizeData>g__IconSizeCheck|0(userOptionData.photoIconSizeSelectMaterial, CS$<>8__locals1.place);
					break;
				case SortFilterDefine.IconPlace.PhotoSell:
					num = CS$<>8__locals1.<GetIconSizeData>g__IconSizeCheck|0(userOptionData.photoIconSizeSell, CS$<>8__locals1.place);
					break;
				case SortFilterDefine.IconPlace.PhotoPartyEdit:
					num = CS$<>8__locals1.<GetIconSizeData>g__IconSizeCheck|0(userOptionData.photoIconSizePartyEdit, CS$<>8__locals1.place);
					break;
				case SortFilterDefine.IconPlace.PhotoAsistant:
					num = CS$<>8__locals1.<GetIconSizeData>g__IconSizeCheck|0(userOptionData.photoIconSizeAsistant, CS$<>8__locals1.place);
					break;
				case SortFilterDefine.IconPlace.PhotoAlbum:
					num = CS$<>8__locals1.<GetIconSizeData>g__IconSizeCheck|0(userOptionData.photoIconSizePhotoAlbum, CS$<>8__locals1.place);
					break;
				default:
					num = (SortFilterDefine.DefaultIconSizeIndexMap.ContainsKey(CS$<>8__locals1.place) ? SortFilterDefine.DefaultIconSizeIndexMap[CS$<>8__locals1.place] : 0);
					break;
				}
				iconSizeData = new DataManagerGameStatus.UserFlagData.IconSizeData(CS$<>8__locals1.place, num);
			}
			else
			{
				iconSizeData = new DataManagerGameStatus.UserFlagData.IconSizeData(iconSizeData.IconPlace, iconSizeData.SizeIndex);
			}
			return iconSizeData;
		}

		// Token: 0x060032A5 RID: 12965 RVA: 0x001BF83C File Offset: 0x001BDA3C
		public UserFlagData(List<DataManagerGameStatus.UserFlagData.OneParamData> paramList)
		{
			this.TutorialFinishFlag = new DataManagerGameStatus.UserFlagData.TutorialFinish();
			this.InformationsFlag = new DataManagerGameStatus.UserFlagData.InformationFlag();
			this.ReleaseModeFlag = new DataManagerGameStatus.UserFlagData.ReleaseMode();
			this.SortTypeDataList = new List<DataManagerGameStatus.UserFlagData.SortTypeData>();
			this.IconSizeDataList = new List<DataManagerGameStatus.UserFlagData.IconSizeData>();
			this.CharaGrowTutorialFlag = new DataManagerGameStatus.UserFlagData.CharaGrowTutorial();
			List<int> list = new List<int>();
			using (List<DataManagerGameStatus.UserFlagData.OneParamData>.Enumerator enumerator = paramList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DataManagerGameStatus.UserFlagData.OneParamData flag = enumerator.Current;
					switch (flag.Category)
					{
					case DataManager.NewFlgCategory.COMMON:
						switch (flag.Id)
						{
						case 1:
							this.TutorialFinishFlag.PvpFirst = flag.Value != 0;
							break;
						case 2:
							this.TutorialFinishFlag.FirstAssetDownload = flag.Value != 0;
							break;
						case 3:
							this.TutorialFinishFlag.PicnicFirst = flag.Value != 0;
							break;
						case 4:
							this.TutorialFinishFlag.ReviewByShop = flag.Value != 0;
							break;
						case 5:
							this.InformationsFlag.DisableMonthlyPackInfo1 = flag.Value != 0;
							break;
						case 6:
							this.InformationsFlag.DisableMonthlyPackInfo2 = flag.Value != 0;
							break;
						case 7:
							this.InformationsFlag.PicnicBuyCharge = flag.Value != 0;
							break;
						case 8:
							this.TutorialFinishFlag.TrainigFirst = flag.Value != 0;
							break;
						case 9:
							this.TutorialFinishFlag.KemoBoardFirst = flag.Value != 0;
							break;
						case 10:
							this.TutorialFinishFlag.DMMPurchaseWarning = flag.Value != 0;
							break;
						case 11:
							this.InformationsFlag.DisableAmazonCloseInfo = flag.Value != 0;
							break;
						case 12:
							this.TutorialFinishFlag.FirstViewAmazonCloseInfo = flag.Value != 0;
							break;
						case 13:
							this.TutorialFinishFlag.SpecialPvpFirst = flag.Value != 0;
							break;
						case 14:
							this.TutorialFinishFlag.TreeHouseFirst = (DataManagerGameStatus.UserFlagData.TREE_HOUSE_TUTORIAL)flag.Value;
							break;
						case 15:
							this.InformationsFlag.DisableTreeHouseTips = flag.Value != 0;
							break;
						case 16:
							this.InformationsFlag.DisableTreeHouseBgmChange = flag.Value != 0;
							break;
						}
						break;
					case DataManager.NewFlgCategory.SORTTYPE:
					{
						DataManagerGameStatus.UserFlagData.SortTypeData sortTypeData = this.SortTypeDataList.Find((DataManagerGameStatus.UserFlagData.SortTypeData x) => x._regType == flag.Id);
						if (sortTypeData != null)
						{
							sortTypeData = new DataManagerGameStatus.UserFlagData.SortTypeData(flag.Id, flag.Value);
						}
						else
						{
							DataManagerGameStatus.UserFlagData.SortTypeData sortTypeData2 = new DataManagerGameStatus.UserFlagData.SortTypeData(flag.Id, flag.Value);
							this.SortTypeDataList.Add(sortTypeData2);
						}
						break;
					}
					case DataManager.NewFlgCategory.RELEASEMODE:
						switch (flag.Id)
						{
						case 1:
							this.ReleaseModeFlag.GrowthQuest = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 2:
							this.ReleaseModeFlag.FriendsStory = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 3:
							this.ReleaseModeFlag.PvpMode = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 4:
							this.ReleaseModeFlag.Picnic = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 5:
							this.ReleaseModeFlag.Picnic2 = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 6:
							this.ReleaseModeFlag.Picnic3 = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 7:
							this.ReleaseModeFlag.Picnic4 = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 8:
							this.ReleaseModeFlag.AraiDiary = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 9:
							this.ReleaseModeFlag.HardMode1 = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 10:
							this.ReleaseModeFlag.HardMode1_5 = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 11:
							this.ReleaseModeFlag.HardMode2 = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 12:
							this.ReleaseModeFlag.AraiDiaryOpen = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 13:
							this.ReleaseModeFlag.TrainingByQuestTop = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 14:
							this.ReleaseModeFlag.TrainingByFriendsGrow = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 15:
							this.ReleaseModeFlag.CellvalQuest = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 16:
							this.ReleaseModeFlag.CellvalQuestOpen = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 17:
							this.ReleaseModeFlag.TreeHouse = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 18:
							this.ReleaseModeFlag.CharaCommunication = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 19:
							this.ReleaseModeFlag.EtceteraQuest = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 20:
							this.ReleaseModeFlag.MainStory2 = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 21:
							this.ReleaseModeFlag.CharaAccessoryOpen = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 22:
							this.ReleaseModeFlag.MainStory3 = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						case 23:
							this.ReleaseModeFlag.QuestAssistantOpen = (DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus)flag.Value;
							break;
						}
						break;
					case DataManager.NewFlgCategory.GACHANEWINFO:
						list.Add(flag.Value);
						break;
					case DataManager.NewFlgCategory.CHARAGROWTUTORIAL:
						switch (flag.Id)
						{
						case 1:
							this.CharaGrowTutorialFlag.LevelUp = 1 == flag.Value;
							break;
						case 2:
							this.CharaGrowTutorialFlag.Yasei = 1 == flag.Value;
							break;
						case 3:
							this.CharaGrowTutorialFlag.RankUp = 1 == flag.Value;
							break;
						case 4:
							this.CharaGrowTutorialFlag.Miracle = 1 == flag.Value;
							break;
						case 5:
							this.CharaGrowTutorialFlag.Photo = 1 == flag.Value;
							break;
						case 6:
							this.CharaGrowTutorialFlag.Nanairo = 1 == flag.Value;
							break;
						}
						break;
					case DataManager.NewFlgCategory.IconSizeIndex:
					{
						DataManagerGameStatus.UserFlagData.IconSizeData iconSizeData = this.IconSizeDataList.Find((DataManagerGameStatus.UserFlagData.IconSizeData x) => x._iconPlase == flag.Id);
						if (iconSizeData != null)
						{
							iconSizeData = new DataManagerGameStatus.UserFlagData.IconSizeData(flag.Id, flag.Value);
						}
						else
						{
							DataManagerGameStatus.UserFlagData.IconSizeData iconSizeData2 = new DataManagerGameStatus.UserFlagData.IconSizeData(flag.Id, flag.Value);
							this.IconSizeDataList.Add(iconSizeData2);
						}
						break;
					}
					}
				}
			}
			this.GachaNewInfoData = new DataManagerGameStatus.UserFlagData.GachaNewInfo(list);
		}

		// Token: 0x060032A6 RID: 12966 RVA: 0x001C0060 File Offset: 0x001BE260
		public List<NewFlg> CreateServerRequestData(List<DataManagerGameStatus.UserFlagData.OneParamData> beforeList)
		{
			List<DataManagerGameStatus.UserFlagData.OneParamData> list = new List<DataManagerGameStatus.UserFlagData.OneParamData>
			{
				new DataManagerGameStatus.UserFlagData.OneParamData(1, DataManager.NewFlgCategory.COMMON, this.TutorialFinishFlag.PvpFirst ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(2, DataManager.NewFlgCategory.COMMON, this.TutorialFinishFlag.FirstAssetDownload ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(3, DataManager.NewFlgCategory.COMMON, this.TutorialFinishFlag.PicnicFirst ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(4, DataManager.NewFlgCategory.COMMON, this.TutorialFinishFlag.ReviewByShop ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(5, DataManager.NewFlgCategory.COMMON, this.InformationsFlag.DisableMonthlyPackInfo1 ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(6, DataManager.NewFlgCategory.COMMON, this.InformationsFlag.DisableMonthlyPackInfo2 ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(7, DataManager.NewFlgCategory.COMMON, this.InformationsFlag.PicnicBuyCharge ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(8, DataManager.NewFlgCategory.COMMON, this.TutorialFinishFlag.TrainigFirst ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(9, DataManager.NewFlgCategory.COMMON, this.TutorialFinishFlag.KemoBoardFirst ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(10, DataManager.NewFlgCategory.COMMON, this.TutorialFinishFlag.DMMPurchaseWarning ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(11, DataManager.NewFlgCategory.COMMON, this.InformationsFlag.DisableAmazonCloseInfo ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(12, DataManager.NewFlgCategory.COMMON, this.TutorialFinishFlag.FirstViewAmazonCloseInfo ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(13, DataManager.NewFlgCategory.COMMON, this.TutorialFinishFlag.SpecialPvpFirst ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(14, DataManager.NewFlgCategory.COMMON, (int)this.TutorialFinishFlag.TreeHouseFirst),
				new DataManagerGameStatus.UserFlagData.OneParamData(15, DataManager.NewFlgCategory.COMMON, this.InformationsFlag.DisableTreeHouseTips ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(16, DataManager.NewFlgCategory.COMMON, this.InformationsFlag.DisableTreeHouseBgmChange ? 1 : 0)
			};
			List<DataManagerGameStatus.UserFlagData.OneParamData> list2 = new List<DataManagerGameStatus.UserFlagData.OneParamData>
			{
				new DataManagerGameStatus.UserFlagData.OneParamData(1, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.GrowthQuest),
				new DataManagerGameStatus.UserFlagData.OneParamData(2, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.FriendsStory),
				new DataManagerGameStatus.UserFlagData.OneParamData(3, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.PvpMode),
				new DataManagerGameStatus.UserFlagData.OneParamData(4, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.Picnic),
				new DataManagerGameStatus.UserFlagData.OneParamData(5, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.Picnic2),
				new DataManagerGameStatus.UserFlagData.OneParamData(6, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.Picnic3),
				new DataManagerGameStatus.UserFlagData.OneParamData(7, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.Picnic4),
				new DataManagerGameStatus.UserFlagData.OneParamData(8, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.AraiDiary),
				new DataManagerGameStatus.UserFlagData.OneParamData(9, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.HardMode1),
				new DataManagerGameStatus.UserFlagData.OneParamData(10, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.HardMode1_5),
				new DataManagerGameStatus.UserFlagData.OneParamData(11, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.HardMode2),
				new DataManagerGameStatus.UserFlagData.OneParamData(12, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.AraiDiaryOpen),
				new DataManagerGameStatus.UserFlagData.OneParamData(13, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.TrainingByQuestTop),
				new DataManagerGameStatus.UserFlagData.OneParamData(14, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.TrainingByFriendsGrow),
				new DataManagerGameStatus.UserFlagData.OneParamData(15, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.CellvalQuest),
				new DataManagerGameStatus.UserFlagData.OneParamData(16, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.CellvalQuestOpen),
				new DataManagerGameStatus.UserFlagData.OneParamData(17, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.TreeHouse),
				new DataManagerGameStatus.UserFlagData.OneParamData(18, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.CharaCommunication),
				new DataManagerGameStatus.UserFlagData.OneParamData(19, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.EtceteraQuest),
				new DataManagerGameStatus.UserFlagData.OneParamData(20, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.MainStory2),
				new DataManagerGameStatus.UserFlagData.OneParamData(21, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.CharaAccessoryOpen),
				new DataManagerGameStatus.UserFlagData.OneParamData(22, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.MainStory3),
				new DataManagerGameStatus.UserFlagData.OneParamData(23, DataManager.NewFlgCategory.RELEASEMODE, (int)this.ReleaseModeFlag.QuestAssistantOpen)
			};
			list.AddRange(list2);
			foreach (DataManagerGameStatus.UserFlagData.SortTypeData sortTypeData in this.SortTypeDataList)
			{
				DataManagerGameStatus.UserFlagData.OneParamData oneParamData = new DataManagerGameStatus.UserFlagData.OneParamData(sortTypeData._regType, DataManager.NewFlgCategory.SORTTYPE, sortTypeData._sortType);
				list.Add(oneParamData);
			}
			foreach (DataManagerGameStatus.UserFlagData.IconSizeData iconSizeData in this.IconSizeDataList)
			{
				DataManagerGameStatus.UserFlagData.OneParamData oneParamData2 = new DataManagerGameStatus.UserFlagData.OneParamData(iconSizeData._iconPlase, DataManager.NewFlgCategory.IconSizeIndex, iconSizeData._sizeIndex);
				list.Add(oneParamData2);
			}
			int num = 0;
			foreach (int num2 in this.GachaNewInfoData.CreateServerData())
			{
				num++;
				DataManagerGameStatus.UserFlagData.OneParamData oneParamData3 = new DataManagerGameStatus.UserFlagData.OneParamData(num, DataManager.NewFlgCategory.GACHANEWINFO, num2);
				list.Add(oneParamData3);
			}
			List<DataManagerGameStatus.UserFlagData.OneParamData> list3 = new List<DataManagerGameStatus.UserFlagData.OneParamData>
			{
				new DataManagerGameStatus.UserFlagData.OneParamData(1, DataManager.NewFlgCategory.CHARAGROWTUTORIAL, this.CharaGrowTutorialFlag.LevelUp ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(2, DataManager.NewFlgCategory.CHARAGROWTUTORIAL, this.CharaGrowTutorialFlag.Yasei ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(3, DataManager.NewFlgCategory.CHARAGROWTUTORIAL, this.CharaGrowTutorialFlag.RankUp ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(4, DataManager.NewFlgCategory.CHARAGROWTUTORIAL, this.CharaGrowTutorialFlag.Miracle ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(5, DataManager.NewFlgCategory.CHARAGROWTUTORIAL, this.CharaGrowTutorialFlag.Photo ? 1 : 0),
				new DataManagerGameStatus.UserFlagData.OneParamData(6, DataManager.NewFlgCategory.CHARAGROWTUTORIAL, this.CharaGrowTutorialFlag.Nanairo ? 1 : 0)
			};
			list.AddRange(list3);
			if (beforeList != null)
			{
				list.RemoveAll((DataManagerGameStatus.UserFlagData.OneParamData resultItem) => beforeList.Exists((DataManagerGameStatus.UserFlagData.OneParamData beforeItem) => beforeItem.IsEqualProperty(resultItem)));
			}
			return this.OneParamData2NewFlagList(list);
		}

		// Token: 0x060032A7 RID: 12967 RVA: 0x001C06A8 File Offset: 0x001BE8A8
		private List<NewFlg> OneParamData2NewFlagList(List<DataManagerGameStatus.UserFlagData.OneParamData> oneParamList)
		{
			List<NewFlg> list = new List<NewFlg>();
			foreach (DataManagerGameStatus.UserFlagData.OneParamData oneParamData in oneParamList)
			{
				NewFlg newFlg = new NewFlg
				{
					any_id = oneParamData.Id,
					category = (int)oneParamData.Category,
					new_mgmt_flg = oneParamData.Value
				};
				list.Add(newFlg);
			}
			return list;
		}

		// Token: 0x0200112A RID: 4394
		public class OneParamData
		{
			// Token: 0x17000C45 RID: 3141
			// (get) Token: 0x060054D8 RID: 21720 RVA: 0x0024E536 File Offset: 0x0024C736
			// (set) Token: 0x060054D7 RID: 21719 RVA: 0x0024E52D File Offset: 0x0024C72D
			public int Id { get; set; }

			// Token: 0x17000C46 RID: 3142
			// (get) Token: 0x060054DA RID: 21722 RVA: 0x0024E547 File Offset: 0x0024C747
			// (set) Token: 0x060054D9 RID: 21721 RVA: 0x0024E53E File Offset: 0x0024C73E
			public DataManager.NewFlgCategory Category { get; set; }

			// Token: 0x17000C47 RID: 3143
			// (get) Token: 0x060054DC RID: 21724 RVA: 0x0024E558 File Offset: 0x0024C758
			// (set) Token: 0x060054DB RID: 21723 RVA: 0x0024E54F File Offset: 0x0024C74F
			public int Value { get; set; }

			// Token: 0x060054DD RID: 21725 RVA: 0x0024E560 File Offset: 0x0024C760
			public OneParamData(int id, DataManager.NewFlgCategory category, int val)
			{
				this.Id = id;
				this.Category = category;
				this.Value = val;
			}

			// Token: 0x060054DE RID: 21726 RVA: 0x0024E57D File Offset: 0x0024C77D
			public OneParamData(int id, int category, int val)
			{
				this.Id = id;
				this.Category = (DataManager.NewFlgCategory)category;
				this.Value = val;
			}

			// Token: 0x060054DF RID: 21727 RVA: 0x0024E59A File Offset: 0x0024C79A
			public OneParamData(DataManagerGameStatus.UserFlagData.OneParamData opd)
			{
				this.Id = opd.Id;
				this.Category = opd.Category;
				this.Value = opd.Value;
			}

			// Token: 0x060054E0 RID: 21728 RVA: 0x0024E5C6 File Offset: 0x0024C7C6
			public bool IsEqualProperty(DataManagerGameStatus.UserFlagData.OneParamData opd)
			{
				return opd != null && this.Id == opd.Id && this.Category == opd.Category && this.Value == opd.Value;
			}

			// Token: 0x060054E1 RID: 21729 RVA: 0x0024E5F7 File Offset: 0x0024C7F7
			internal bool IsEqualKey(NewFlg newFlag)
			{
				return newFlag != null && this.Id == newFlag.any_id && this.Category == (DataManager.NewFlgCategory)newFlag.category;
			}
		}

		// Token: 0x0200112B RID: 4395
		public class TutorialFinish
		{
			// Token: 0x17000C48 RID: 3144
			// (get) Token: 0x060054E2 RID: 21730 RVA: 0x0024E61A File Offset: 0x0024C81A
			// (set) Token: 0x060054E3 RID: 21731 RVA: 0x0024E622 File Offset: 0x0024C822
			public bool PvpFirst { get; set; }

			// Token: 0x17000C49 RID: 3145
			// (get) Token: 0x060054E4 RID: 21732 RVA: 0x0024E62B File Offset: 0x0024C82B
			// (set) Token: 0x060054E5 RID: 21733 RVA: 0x0024E633 File Offset: 0x0024C833
			public bool FirstAssetDownload { get; set; }

			// Token: 0x17000C4A RID: 3146
			// (get) Token: 0x060054E6 RID: 21734 RVA: 0x0024E63C File Offset: 0x0024C83C
			// (set) Token: 0x060054E7 RID: 21735 RVA: 0x0024E644 File Offset: 0x0024C844
			public bool PicnicFirst { get; set; }

			// Token: 0x17000C4B RID: 3147
			// (get) Token: 0x060054E8 RID: 21736 RVA: 0x0024E64D File Offset: 0x0024C84D
			// (set) Token: 0x060054E9 RID: 21737 RVA: 0x0024E655 File Offset: 0x0024C855
			public bool ReviewByShop { get; set; }

			// Token: 0x17000C4C RID: 3148
			// (get) Token: 0x060054EA RID: 21738 RVA: 0x0024E65E File Offset: 0x0024C85E
			// (set) Token: 0x060054EB RID: 21739 RVA: 0x0024E666 File Offset: 0x0024C866
			public bool TrainigFirst { get; set; }

			// Token: 0x17000C4D RID: 3149
			// (get) Token: 0x060054EC RID: 21740 RVA: 0x0024E66F File Offset: 0x0024C86F
			// (set) Token: 0x060054ED RID: 21741 RVA: 0x0024E677 File Offset: 0x0024C877
			public bool KemoBoardFirst { get; set; }

			// Token: 0x17000C4E RID: 3150
			// (get) Token: 0x060054EE RID: 21742 RVA: 0x0024E680 File Offset: 0x0024C880
			// (set) Token: 0x060054EF RID: 21743 RVA: 0x0024E688 File Offset: 0x0024C888
			public bool DMMPurchaseWarning { get; set; }

			// Token: 0x17000C4F RID: 3151
			// (get) Token: 0x060054F0 RID: 21744 RVA: 0x0024E691 File Offset: 0x0024C891
			// (set) Token: 0x060054F1 RID: 21745 RVA: 0x0024E699 File Offset: 0x0024C899
			public bool FirstViewAmazonCloseInfo { get; set; }

			// Token: 0x17000C50 RID: 3152
			// (get) Token: 0x060054F2 RID: 21746 RVA: 0x0024E6A2 File Offset: 0x0024C8A2
			// (set) Token: 0x060054F3 RID: 21747 RVA: 0x0024E6AA File Offset: 0x0024C8AA
			public bool SpecialPvpFirst { get; set; }

			// Token: 0x17000C51 RID: 3153
			// (get) Token: 0x060054F4 RID: 21748 RVA: 0x0024E6B3 File Offset: 0x0024C8B3
			// (set) Token: 0x060054F5 RID: 21749 RVA: 0x0024E6BB File Offset: 0x0024C8BB
			public DataManagerGameStatus.UserFlagData.TREE_HOUSE_TUTORIAL TreeHouseFirst { get; set; }
		}

		// Token: 0x0200112C RID: 4396
		public enum TREE_HOUSE_TUTORIAL
		{
			// Token: 0x04005E66 RID: 24166
			NONE,
			// Token: 0x04005E67 RID: 24167
			FIRST,
			// Token: 0x04005E68 RID: 24168
			SECOND,
			// Token: 0x04005E69 RID: 24169
			LATEST
		}

		// Token: 0x0200112D RID: 4397
		public class InformationFlag
		{
			// Token: 0x17000C52 RID: 3154
			// (get) Token: 0x060054F7 RID: 21751 RVA: 0x0024E6CC File Offset: 0x0024C8CC
			// (set) Token: 0x060054F8 RID: 21752 RVA: 0x0024E6D4 File Offset: 0x0024C8D4
			public bool DisableMonthlyPackInfo1 { get; set; }

			// Token: 0x17000C53 RID: 3155
			// (get) Token: 0x060054F9 RID: 21753 RVA: 0x0024E6DD File Offset: 0x0024C8DD
			// (set) Token: 0x060054FA RID: 21754 RVA: 0x0024E6E5 File Offset: 0x0024C8E5
			public bool DisableMonthlyPackInfo2 { get; set; }

			// Token: 0x17000C54 RID: 3156
			// (get) Token: 0x060054FB RID: 21755 RVA: 0x0024E6EE File Offset: 0x0024C8EE
			// (set) Token: 0x060054FC RID: 21756 RVA: 0x0024E6F6 File Offset: 0x0024C8F6
			public bool PicnicBuyCharge { get; set; }

			// Token: 0x17000C55 RID: 3157
			// (get) Token: 0x060054FD RID: 21757 RVA: 0x0024E6FF File Offset: 0x0024C8FF
			// (set) Token: 0x060054FE RID: 21758 RVA: 0x0024E707 File Offset: 0x0024C907
			public bool DisableAmazonCloseInfo { get; set; }

			// Token: 0x17000C56 RID: 3158
			// (get) Token: 0x060054FF RID: 21759 RVA: 0x0024E710 File Offset: 0x0024C910
			// (set) Token: 0x06005500 RID: 21760 RVA: 0x0024E718 File Offset: 0x0024C918
			public bool DisableTreeHouseTips { get; set; }

			// Token: 0x17000C57 RID: 3159
			// (get) Token: 0x06005501 RID: 21761 RVA: 0x0024E721 File Offset: 0x0024C921
			// (set) Token: 0x06005502 RID: 21762 RVA: 0x0024E729 File Offset: 0x0024C929
			public bool DisableTreeHouseBgmChange { get; set; }
		}

		// Token: 0x0200112E RID: 4398
		public class ReleaseMode
		{
			// Token: 0x17000C58 RID: 3160
			// (get) Token: 0x06005504 RID: 21764 RVA: 0x0024E73A File Offset: 0x0024C93A
			// (set) Token: 0x06005505 RID: 21765 RVA: 0x0024E742 File Offset: 0x0024C942
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus GrowthQuest { get; set; }

			// Token: 0x17000C59 RID: 3161
			// (get) Token: 0x06005506 RID: 21766 RVA: 0x0024E74B File Offset: 0x0024C94B
			// (set) Token: 0x06005507 RID: 21767 RVA: 0x0024E753 File Offset: 0x0024C953
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus FriendsStory { get; set; }

			// Token: 0x17000C5A RID: 3162
			// (get) Token: 0x06005508 RID: 21768 RVA: 0x0024E75C File Offset: 0x0024C95C
			// (set) Token: 0x06005509 RID: 21769 RVA: 0x0024E764 File Offset: 0x0024C964
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus PvpMode { get; set; }

			// Token: 0x17000C5B RID: 3163
			// (get) Token: 0x0600550A RID: 21770 RVA: 0x0024E76D File Offset: 0x0024C96D
			// (set) Token: 0x0600550B RID: 21771 RVA: 0x0024E775 File Offset: 0x0024C975
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus Picnic { get; set; }

			// Token: 0x17000C5C RID: 3164
			// (get) Token: 0x0600550C RID: 21772 RVA: 0x0024E77E File Offset: 0x0024C97E
			// (set) Token: 0x0600550D RID: 21773 RVA: 0x0024E786 File Offset: 0x0024C986
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus Picnic2 { get; set; }

			// Token: 0x17000C5D RID: 3165
			// (get) Token: 0x0600550E RID: 21774 RVA: 0x0024E78F File Offset: 0x0024C98F
			// (set) Token: 0x0600550F RID: 21775 RVA: 0x0024E797 File Offset: 0x0024C997
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus Picnic3 { get; set; }

			// Token: 0x17000C5E RID: 3166
			// (get) Token: 0x06005510 RID: 21776 RVA: 0x0024E7A0 File Offset: 0x0024C9A0
			// (set) Token: 0x06005511 RID: 21777 RVA: 0x0024E7A8 File Offset: 0x0024C9A8
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus Picnic4 { get; set; }

			// Token: 0x17000C5F RID: 3167
			// (get) Token: 0x06005512 RID: 21778 RVA: 0x0024E7B1 File Offset: 0x0024C9B1
			// (set) Token: 0x06005513 RID: 21779 RVA: 0x0024E7B9 File Offset: 0x0024C9B9
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus AraiDiary { get; set; }

			// Token: 0x17000C60 RID: 3168
			// (get) Token: 0x06005514 RID: 21780 RVA: 0x0024E7C2 File Offset: 0x0024C9C2
			// (set) Token: 0x06005515 RID: 21781 RVA: 0x0024E7CA File Offset: 0x0024C9CA
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus AraiDiaryOpen { get; set; }

			// Token: 0x17000C61 RID: 3169
			// (get) Token: 0x06005516 RID: 21782 RVA: 0x0024E7D3 File Offset: 0x0024C9D3
			// (set) Token: 0x06005517 RID: 21783 RVA: 0x0024E7DB File Offset: 0x0024C9DB
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus HardMode1 { get; set; }

			// Token: 0x17000C62 RID: 3170
			// (get) Token: 0x06005518 RID: 21784 RVA: 0x0024E7E4 File Offset: 0x0024C9E4
			// (set) Token: 0x06005519 RID: 21785 RVA: 0x0024E7EC File Offset: 0x0024C9EC
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus HardMode1_5 { get; set; }

			// Token: 0x17000C63 RID: 3171
			// (get) Token: 0x0600551A RID: 21786 RVA: 0x0024E7F5 File Offset: 0x0024C9F5
			// (set) Token: 0x0600551B RID: 21787 RVA: 0x0024E7FD File Offset: 0x0024C9FD
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus HardMode2 { get; set; }

			// Token: 0x17000C64 RID: 3172
			// (get) Token: 0x0600551C RID: 21788 RVA: 0x0024E806 File Offset: 0x0024CA06
			// (set) Token: 0x0600551D RID: 21789 RVA: 0x0024E80E File Offset: 0x0024CA0E
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus TrainingByQuestTop { get; set; }

			// Token: 0x17000C65 RID: 3173
			// (get) Token: 0x0600551E RID: 21790 RVA: 0x0024E817 File Offset: 0x0024CA17
			// (set) Token: 0x0600551F RID: 21791 RVA: 0x0024E81F File Offset: 0x0024CA1F
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus TrainingByFriendsGrow { get; set; }

			// Token: 0x17000C66 RID: 3174
			// (get) Token: 0x06005520 RID: 21792 RVA: 0x0024E828 File Offset: 0x0024CA28
			// (set) Token: 0x06005521 RID: 21793 RVA: 0x0024E830 File Offset: 0x0024CA30
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus CellvalQuest { get; set; }

			// Token: 0x17000C67 RID: 3175
			// (get) Token: 0x06005522 RID: 21794 RVA: 0x0024E839 File Offset: 0x0024CA39
			// (set) Token: 0x06005523 RID: 21795 RVA: 0x0024E841 File Offset: 0x0024CA41
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus CellvalQuestOpen { get; set; }

			// Token: 0x17000C68 RID: 3176
			// (get) Token: 0x06005524 RID: 21796 RVA: 0x0024E84A File Offset: 0x0024CA4A
			// (set) Token: 0x06005525 RID: 21797 RVA: 0x0024E852 File Offset: 0x0024CA52
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus TreeHouse { get; set; }

			// Token: 0x17000C69 RID: 3177
			// (get) Token: 0x06005526 RID: 21798 RVA: 0x0024E85B File Offset: 0x0024CA5B
			// (set) Token: 0x06005527 RID: 21799 RVA: 0x0024E863 File Offset: 0x0024CA63
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus CharaCommunication { get; set; }

			// Token: 0x17000C6A RID: 3178
			// (get) Token: 0x06005528 RID: 21800 RVA: 0x0024E86C File Offset: 0x0024CA6C
			// (set) Token: 0x06005529 RID: 21801 RVA: 0x0024E874 File Offset: 0x0024CA74
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus EtceteraQuest { get; set; }

			// Token: 0x17000C6B RID: 3179
			// (get) Token: 0x0600552A RID: 21802 RVA: 0x0024E87D File Offset: 0x0024CA7D
			// (set) Token: 0x0600552B RID: 21803 RVA: 0x0024E885 File Offset: 0x0024CA85
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus MainStory2 { get; set; }

			// Token: 0x17000C6C RID: 3180
			// (get) Token: 0x0600552C RID: 21804 RVA: 0x0024E88E File Offset: 0x0024CA8E
			// (set) Token: 0x0600552D RID: 21805 RVA: 0x0024E896 File Offset: 0x0024CA96
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus MainStory3 { get; set; }

			// Token: 0x17000C6D RID: 3181
			// (get) Token: 0x0600552E RID: 21806 RVA: 0x0024E89F File Offset: 0x0024CA9F
			// (set) Token: 0x0600552F RID: 21807 RVA: 0x0024E8A7 File Offset: 0x0024CAA7
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus CharaAccessoryOpen { get; set; }

			// Token: 0x17000C6E RID: 3182
			// (get) Token: 0x06005530 RID: 21808 RVA: 0x0024E8B0 File Offset: 0x0024CAB0
			// (set) Token: 0x06005531 RID: 21809 RVA: 0x0024E8B8 File Offset: 0x0024CAB8
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus QuestAssistantOpen { get; set; }

			// Token: 0x02001232 RID: 4658
			public enum LockStatus
			{
				// Token: 0x0400639D RID: 25501
				Locked,
				// Token: 0x0400639E RID: 25502
				UnLocked,
				// Token: 0x0400639F RID: 25503
				Released
			}
		}

		// Token: 0x0200112F RID: 4399
		public class SortTypeData
		{
			// Token: 0x17000C6F RID: 3183
			// (get) Token: 0x06005533 RID: 21811 RVA: 0x0024E8C9 File Offset: 0x0024CAC9
			public SortFilterDefine.RegisterType RegisterType
			{
				get
				{
					return (SortFilterDefine.RegisterType)this._regType;
				}
			}

			// Token: 0x17000C70 RID: 3184
			// (get) Token: 0x06005534 RID: 21812 RVA: 0x0024E8D1 File Offset: 0x0024CAD1
			public SortFilterDefine.SortType SortType
			{
				get
				{
					return (SortFilterDefine.SortType)Math.Abs(this._sortType);
				}
			}

			// Token: 0x17000C71 RID: 3185
			// (get) Token: 0x06005535 RID: 21813 RVA: 0x0024E8DE File Offset: 0x0024CADE
			public bool Order
			{
				get
				{
					return 0 <= this._sortType;
				}
			}

			// Token: 0x06005536 RID: 21814 RVA: 0x0024E8EC File Offset: 0x0024CAEC
			public SortTypeData(SortFilterDefine.RegisterType regtype, SortFilterDefine.SortType sorttype, bool order)
			{
				this._regType = (int)regtype;
				this._sortType = (int)sorttype;
				if (!order)
				{
					this._sortType *= -1;
				}
			}

			// Token: 0x06005537 RID: 21815 RVA: 0x0024E913 File Offset: 0x0024CB13
			public SortTypeData(int regtype, int sorttype)
			{
				this._regType = regtype;
				this._sortType = sorttype;
			}

			// Token: 0x04005E87 RID: 24199
			public int _regType;

			// Token: 0x04005E88 RID: 24200
			public int _sortType;
		}

		// Token: 0x02001130 RID: 4400
		public class IconSizeData
		{
			// Token: 0x17000C72 RID: 3186
			// (get) Token: 0x06005538 RID: 21816 RVA: 0x0024E929 File Offset: 0x0024CB29
			public SortFilterDefine.IconPlace IconPlace
			{
				get
				{
					return (SortFilterDefine.IconPlace)this._iconPlase;
				}
			}

			// Token: 0x17000C73 RID: 3187
			// (get) Token: 0x06005539 RID: 21817 RVA: 0x0024E931 File Offset: 0x0024CB31
			public int SizeIndex
			{
				get
				{
					return this._sizeIndex;
				}
			}

			// Token: 0x0600553A RID: 21818 RVA: 0x0024E939 File Offset: 0x0024CB39
			public IconSizeData(SortFilterDefine.IconPlace iconPlace, int sizeIndex)
			{
				this._iconPlase = (int)iconPlace;
				this._sizeIndex = sizeIndex;
			}

			// Token: 0x0600553B RID: 21819 RVA: 0x0024E94F File Offset: 0x0024CB4F
			public IconSizeData(int iconPlace, int sizeIndex)
			{
				this._iconPlase = iconPlace;
				this._sizeIndex = sizeIndex;
			}

			// Token: 0x04005E89 RID: 24201
			public int _iconPlase;

			// Token: 0x04005E8A RID: 24202
			public int _sizeIndex;
		}

		// Token: 0x02001131 RID: 4401
		public class GachaNewInfo
		{
			// Token: 0x17000C74 RID: 3188
			// (get) Token: 0x0600553C RID: 21820 RVA: 0x0024E968 File Offset: 0x0024CB68
			public HashSet<int> DisplayedIDList
			{
				get
				{
					DataManagerGacha dmGacha = DataManager.DmGacha;
					List<DataManagerGacha.GachaPackData> list = ((dmGacha != null) ? dmGacha.CopyGachaPackDataList() : null);
					if (list == null)
					{
						return new HashSet<int>();
					}
					HashSet<int> hashSet = new HashSet<int>(this._privateDisplayedIDList);
					list.RemoveAll((DataManagerGacha.GachaPackData x) => x.staticData.endDatetime < TimeManager.Now);
					list.RemoveAll((DataManagerGacha.GachaPackData x) => x.staticData.dayOfWeekFlg && x.staticData.EndTimeOfDayOfWeek(TimeManager.Now) < TimeManager.Now);
					HashSet<int> hashSet2 = new HashSet<int>();
					foreach (DataManagerGacha.GachaPackData gachaPackData in list)
					{
						hashSet2.Add(gachaPackData.gachaId);
					}
					this._privateDisplayedIDList = new HashSet<int>();
					foreach (int num in hashSet)
					{
						if (hashSet2.Contains(num))
						{
							this._privateDisplayedIDList.Add(num);
							hashSet2.Remove(num);
						}
					}
					return this._privateDisplayedIDList;
				}
			}

			// Token: 0x17000C75 RID: 3189
			// (get) Token: 0x0600553D RID: 21821 RVA: 0x0024EAA4 File Offset: 0x0024CCA4
			// (set) Token: 0x0600553E RID: 21822 RVA: 0x0024EAAC File Offset: 0x0024CCAC
			public int ListLength { get; private set; }

			// Token: 0x17000C76 RID: 3190
			// (get) Token: 0x0600553F RID: 21823 RVA: 0x0024EAB8 File Offset: 0x0024CCB8
			public bool IsNew
			{
				get
				{
					List<DataManagerGacha.GachaPackData> list = DataManager.DmGacha.CopyGachaPackDataList();
					list.RemoveAll((DataManagerGacha.GachaPackData x) => x.staticData.endDatetime < TimeManager.Now);
					list.RemoveAll((DataManagerGacha.GachaPackData x) => x.staticData.dayOfWeekFlg && x.staticData.EndTimeOfDayOfWeek(TimeManager.Now) < TimeManager.Now);
					bool flag = false;
					foreach (DataManagerGacha.GachaPackData gachaPackData in list)
					{
						if (!this._privateDisplayedIDList.Contains(gachaPackData.gachaId))
						{
							flag = true;
							break;
						}
					}
					return flag;
				}
			}

			// Token: 0x06005540 RID: 21824 RVA: 0x0024EB70 File Offset: 0x0024CD70
			public GachaNewInfo(List<int> srvmst)
			{
				if (srvmst == null)
				{
					return;
				}
				this.ListLength = srvmst.Count;
				this._privateDisplayedIDList = new HashSet<int>(srvmst);
				this._privateDisplayedIDList.Remove(0);
			}

			// Token: 0x06005541 RID: 21825 RVA: 0x0024EBA4 File Offset: 0x0024CDA4
			public List<int> CreateServerData()
			{
				List<int> list = new List<int>(this._privateDisplayedIDList);
				if (this.ListLength > this._privateDisplayedIDList.Count)
				{
					int num = this.ListLength - this._privateDisplayedIDList.Count;
					list.Sort();
					for (int i = 0; i < num; i++)
					{
						list.Add(0);
					}
				}
				else
				{
					this.ListLength = this._privateDisplayedIDList.Count;
				}
				return list;
			}

			// Token: 0x06005542 RID: 21826 RVA: 0x0024EC10 File Offset: 0x0024CE10
			public bool RegisterIDs(HashSet<int> idList)
			{
				if (DataManager.DmGameStatus == null)
				{
					return false;
				}
				if (DataManager.DmGameStatus.MakeUserFlagData().GachaNewInfoData._privateDisplayedIDList.SequenceEqual<int>(idList))
				{
					return false;
				}
				DataManagerGacha dmGacha = DataManager.DmGacha;
				if (((dmGacha != null) ? dmGacha.CopyGachaPackDataList() : null) == null)
				{
					return false;
				}
				HashSet<int> hashSet = new HashSet<int>(this._privateDisplayedIDList);
				foreach (int num in idList)
				{
					hashSet.Add(num);
				}
				List<DataManagerGacha.GachaPackData> list = DataManager.DmGacha.CopyGachaPackDataList();
				list.RemoveAll((DataManagerGacha.GachaPackData x) => x.staticData.endDatetime < TimeManager.Now);
				list.RemoveAll((DataManagerGacha.GachaPackData x) => x.staticData.dayOfWeekFlg && x.staticData.EndTimeOfDayOfWeek(TimeManager.Now) < TimeManager.Now);
				HashSet<int> hashSet2 = new HashSet<int>();
				foreach (DataManagerGacha.GachaPackData gachaPackData in list)
				{
					hashSet2.Add(gachaPackData.gachaId);
				}
				this._privateDisplayedIDList = new HashSet<int>();
				foreach (int num2 in hashSet)
				{
					if (hashSet2.Contains(num2))
					{
						this._privateDisplayedIDList.Add(num2);
						hashSet2.Remove(num2);
					}
				}
				return true;
			}

			// Token: 0x06005543 RID: 21827 RVA: 0x0024EDAC File Offset: 0x0024CFAC
			public void ClearData()
			{
				this._privateDisplayedIDList = new HashSet<int>();
			}

			// Token: 0x04005E8B RID: 24203
			private HashSet<int> _privateDisplayedIDList;
		}

		// Token: 0x02001132 RID: 4402
		public class CharaGrowTutorial
		{
			// Token: 0x17000C77 RID: 3191
			// (get) Token: 0x06005544 RID: 21828 RVA: 0x0024EDB9 File Offset: 0x0024CFB9
			// (set) Token: 0x06005545 RID: 21829 RVA: 0x0024EDC1 File Offset: 0x0024CFC1
			public bool LevelUp { get; set; }

			// Token: 0x17000C78 RID: 3192
			// (get) Token: 0x06005546 RID: 21830 RVA: 0x0024EDCA File Offset: 0x0024CFCA
			// (set) Token: 0x06005547 RID: 21831 RVA: 0x0024EDD2 File Offset: 0x0024CFD2
			public bool Yasei { get; set; }

			// Token: 0x17000C79 RID: 3193
			// (get) Token: 0x06005548 RID: 21832 RVA: 0x0024EDDB File Offset: 0x0024CFDB
			// (set) Token: 0x06005549 RID: 21833 RVA: 0x0024EDE3 File Offset: 0x0024CFE3
			public bool RankUp { get; set; }

			// Token: 0x17000C7A RID: 3194
			// (get) Token: 0x0600554A RID: 21834 RVA: 0x0024EDEC File Offset: 0x0024CFEC
			// (set) Token: 0x0600554B RID: 21835 RVA: 0x0024EDF4 File Offset: 0x0024CFF4
			public bool Miracle { get; set; }

			// Token: 0x17000C7B RID: 3195
			// (get) Token: 0x0600554C RID: 21836 RVA: 0x0024EDFD File Offset: 0x0024CFFD
			// (set) Token: 0x0600554D RID: 21837 RVA: 0x0024EE05 File Offset: 0x0024D005
			public bool Photo { get; set; }

			// Token: 0x17000C7C RID: 3196
			// (get) Token: 0x0600554E RID: 21838 RVA: 0x0024EE0E File Offset: 0x0024D00E
			// (set) Token: 0x0600554F RID: 21839 RVA: 0x0024EE16 File Offset: 0x0024D016
			public bool Kizuna { get; set; }

			// Token: 0x17000C7D RID: 3197
			// (get) Token: 0x06005550 RID: 21840 RVA: 0x0024EE1F File Offset: 0x0024D01F
			// (set) Token: 0x06005551 RID: 21841 RVA: 0x0024EE27 File Offset: 0x0024D027
			public bool Nanairo { get; set; }
		}
	}
}
