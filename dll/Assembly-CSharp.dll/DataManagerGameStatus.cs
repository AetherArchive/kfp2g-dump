using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

public class DataManagerGameStatus
{
	public DataManagerGameStatus(DataManager p)
	{
		this.parentData = p;
	}

	private List<DataManagerGameStatus.UserFlagData.OneParamData> ParamDataList { get; set; } = new List<DataManagerGameStatus.UserFlagData.OneParamData>();

	public DataManagerGameStatus.UserFlagData MakeUserFlagData()
	{
		return new DataManagerGameStatus.UserFlagData(this.ParamDataList);
	}

	public KemoStatusRankingData GetKemoStatusRankingData()
	{
		return this.kemoStatusRankingData;
	}

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

	public void RequestActionShowMovie(MstMovieData mmd)
	{
		if (mmd == null)
		{
			return;
		}
		this.parentData.ServerRequest(PlayVideoCmd.Create(mmd.movieId), null);
	}

	public void RequestGetKemoStatusRanking()
	{
		long num = 0L;
		if (this.kemoStatusRankingData != null)
		{
			num = PrjUtil.ConvertTicksToTime(this.kemoStatusRankingData.lastUpdateTime.Ticks);
		}
		this.parentData.ServerRequest(KemostatusRankingCmd.Create(num), new Action<Command>(this.CbKemostatusRankingCmd));
	}

	private void CbNewFlgUpdateCmd(Command cmd)
	{
		NewFlgUpdateRequest newFlgUpdateRequest = cmd.request as NewFlgUpdateRequest;
		this.UpdateUserFlagByServer(newFlgUpdateRequest.new_flg_list);
	}

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

	private DataManager parentData;

	private KemoStatusRankingData kemoStatusRankingData;

	public class UserFlagData
	{
		public DataManagerGameStatus.UserFlagData.TutorialFinish TutorialFinishFlag { get; set; }

		public DataManagerGameStatus.UserFlagData.InformationFlag InformationsFlag { get; set; }

		public DataManagerGameStatus.UserFlagData.ReleaseMode ReleaseModeFlag { get; set; }

		private List<DataManagerGameStatus.UserFlagData.SortTypeData> SortTypeDataList { get; set; }

		private List<DataManagerGameStatus.UserFlagData.IconSizeData> IconSizeDataList { get; set; }

		public DataManagerGameStatus.UserFlagData.GachaNewInfo GachaNewInfoData { get; private set; }

		public DataManagerGameStatus.UserFlagData.CharaGrowTutorial CharaGrowTutorialFlag { get; set; }

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

		public class OneParamData
		{
			public int Id { get; set; }

			public DataManager.NewFlgCategory Category { get; set; }

			public int Value { get; set; }

			public OneParamData(int id, DataManager.NewFlgCategory category, int val)
			{
				this.Id = id;
				this.Category = category;
				this.Value = val;
			}

			public OneParamData(int id, int category, int val)
			{
				this.Id = id;
				this.Category = (DataManager.NewFlgCategory)category;
				this.Value = val;
			}

			public OneParamData(DataManagerGameStatus.UserFlagData.OneParamData opd)
			{
				this.Id = opd.Id;
				this.Category = opd.Category;
				this.Value = opd.Value;
			}

			public bool IsEqualProperty(DataManagerGameStatus.UserFlagData.OneParamData opd)
			{
				return opd != null && this.Id == opd.Id && this.Category == opd.Category && this.Value == opd.Value;
			}

			internal bool IsEqualKey(NewFlg newFlag)
			{
				return newFlag != null && this.Id == newFlag.any_id && this.Category == (DataManager.NewFlgCategory)newFlag.category;
			}
		}

		public class TutorialFinish
		{
			public bool PvpFirst { get; set; }

			public bool FirstAssetDownload { get; set; }

			public bool PicnicFirst { get; set; }

			public bool ReviewByShop { get; set; }

			public bool TrainigFirst { get; set; }

			public bool KemoBoardFirst { get; set; }

			public bool DMMPurchaseWarning { get; set; }

			public bool FirstViewAmazonCloseInfo { get; set; }

			public bool SpecialPvpFirst { get; set; }

			public DataManagerGameStatus.UserFlagData.TREE_HOUSE_TUTORIAL TreeHouseFirst { get; set; }
		}

		public enum TREE_HOUSE_TUTORIAL
		{
			NONE,
			FIRST,
			SECOND,
			LATEST
		}

		public class InformationFlag
		{
			public bool DisableMonthlyPackInfo1 { get; set; }

			public bool DisableMonthlyPackInfo2 { get; set; }

			public bool PicnicBuyCharge { get; set; }

			public bool DisableAmazonCloseInfo { get; set; }

			public bool DisableTreeHouseTips { get; set; }

			public bool DisableTreeHouseBgmChange { get; set; }
		}

		public class ReleaseMode
		{
			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus GrowthQuest { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus FriendsStory { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus PvpMode { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus Picnic { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus Picnic2 { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus Picnic3 { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus Picnic4 { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus AraiDiary { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus AraiDiaryOpen { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus HardMode1 { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus HardMode1_5 { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus HardMode2 { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus TrainingByQuestTop { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus TrainingByFriendsGrow { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus CellvalQuest { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus CellvalQuestOpen { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus TreeHouse { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus CharaCommunication { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus EtceteraQuest { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus MainStory2 { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus MainStory3 { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus CharaAccessoryOpen { get; set; }

			public DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus QuestAssistantOpen { get; set; }

			public enum LockStatus
			{
				Locked,
				UnLocked,
				Released
			}
		}

		public class SortTypeData
		{
			public SortFilterDefine.RegisterType RegisterType
			{
				get
				{
					return (SortFilterDefine.RegisterType)this._regType;
				}
			}

			public SortFilterDefine.SortType SortType
			{
				get
				{
					return (SortFilterDefine.SortType)Math.Abs(this._sortType);
				}
			}

			public bool Order
			{
				get
				{
					return 0 <= this._sortType;
				}
			}

			public SortTypeData(SortFilterDefine.RegisterType regtype, SortFilterDefine.SortType sorttype, bool order)
			{
				this._regType = (int)regtype;
				this._sortType = (int)sorttype;
				if (!order)
				{
					this._sortType *= -1;
				}
			}

			public SortTypeData(int regtype, int sorttype)
			{
				this._regType = regtype;
				this._sortType = sorttype;
			}

			public int _regType;

			public int _sortType;
		}

		public class IconSizeData
		{
			public SortFilterDefine.IconPlace IconPlace
			{
				get
				{
					return (SortFilterDefine.IconPlace)this._iconPlase;
				}
			}

			public int SizeIndex
			{
				get
				{
					return this._sizeIndex;
				}
			}

			public IconSizeData(SortFilterDefine.IconPlace iconPlace, int sizeIndex)
			{
				this._iconPlase = (int)iconPlace;
				this._sizeIndex = sizeIndex;
			}

			public IconSizeData(int iconPlace, int sizeIndex)
			{
				this._iconPlase = iconPlace;
				this._sizeIndex = sizeIndex;
			}

			public int _iconPlase;

			public int _sizeIndex;
		}

		public class GachaNewInfo
		{
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

			public int ListLength { get; private set; }

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

			public void ClearData()
			{
				this._privateDisplayedIDList = new HashSet<int>();
			}

			private HashSet<int> _privateDisplayedIDList;
		}

		public class CharaGrowTutorial
		{
			public bool LevelUp { get; set; }

			public bool Yasei { get; set; }

			public bool RankUp { get; set; }

			public bool Miracle { get; set; }

			public bool Photo { get; set; }

			public bool Kizuna { get; set; }

			public bool Nanairo { get; set; }
		}
	}
}
