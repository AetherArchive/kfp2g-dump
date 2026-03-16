using System;
using System.Collections.Generic;
using System.Linq;
using CriWare;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

public class DataManagerGacha
{
	public DataManagerGacha(DataManager p)
	{
		this.parentData = p;
	}

	public HashSet<int> SelectedGachaIdHashSet { get; set; }

	public CriAtomExPlayback LatestGreetingVoice { get; set; }

	private void AddToGachaResultRank4PuCharaList(int itemId)
	{
		if (DataManagerGacha.GachaResultRank4PuCharaList == null || DataManagerGacha.GachaResultRank4PuCharaList.Contains(itemId))
		{
			return;
		}
		DataManagerGacha.GachaResultRank4PuCharaList.Add(itemId);
	}

	public static bool IsExistResultPuChara()
	{
		return DataManagerGacha.GachaResultRank4PuCharaList != null && DataManagerGacha.GachaResultRank4PuCharaList.Any<int>();
	}

	public static int GetGachaResultRank4PuRandomItemId()
	{
		if (DataManagerGacha.GachaResultRank4PuCharaList == null || DataManagerGacha.GachaResultRank4PuCharaList.Count == 0)
		{
			return -1;
		}
		return DataManagerGacha.GachaResultRank4PuCharaList.OrderBy<int, Guid>((int x) => Guid.NewGuid()).First<int>();
	}

	public static void ReleasePuCharaList()
	{
		if (DataManagerGacha.GachaResultRank4PuCharaList == null)
		{
			return;
		}
		DataManagerGacha.GachaResultRank4PuCharaList.Clear();
		DataManagerGacha.GachaResultRank4PuCharaList = null;
	}

	public List<DataManagerGacha.GachaPackData> CopyGachaPackDataList()
	{
		if (this.usableGachaPackDataList == null)
		{
			return null;
		}
		return new List<DataManagerGacha.GachaPackData>(this.usableGachaPackDataList);
	}

	public List<DataManagerGacha.GachaPackData> GetGachaPackDataList()
	{
		return this.usableGachaPackDataList;
	}

	public DataManagerGacha.GachaStaticData GetGachaStaticData(int gachaId)
	{
		DataManagerGacha.GachaStaticData gachaStaticData = (this.gachaStaticDataMap.ContainsKey(gachaId) ? this.gachaStaticDataMap[gachaId] : null);
		if (gachaStaticData == null)
		{
			gachaStaticData = new DataManagerGacha.GachaStaticData(new MstGachaData());
		}
		return gachaStaticData;
	}

	public DataManagerGacha.PlayResult GetLastPlayGachaResult()
	{
		return this.gachaPlayResult;
	}

	public DataManagerGacha.ProbabilityData GetLastRequestRateViewData()
	{
		return this.probabilityDataList.Find((DataManagerGacha.ProbabilityData x) => x.gachaId == this.lastRequestRateViewGachaId);
	}

	public int GetCeilingCountNow(int gachaId)
	{
		DataManagerGacha.GachaStaticData gachaStatic = (this.gachaStaticDataMap.ContainsKey(gachaId) ? this.gachaStaticDataMap[gachaId] : null);
		if (gachaStatic == null)
		{
			return 0;
		}
		DataManagerGacha.DynamicGachaGroup dynamicGachaGroup = this.gachaDynamicGachaGroupList.Find((DataManagerGacha.DynamicGachaGroup x) => x.GachaGroupId == gachaStatic.gachaGroupId);
		if (dynamicGachaGroup == null)
		{
			return 0;
		}
		return dynamicGachaGroup.ceilingCountNow;
	}

	public void SetLastPlayGachaResultByTutorial(DataManagerGacha.PlayResult gpr)
	{
		this.gachaPlayResult = gpr;
	}

	public void RequestGetGachaList()
	{
		if (!Singleton<DataManager>.Instance.DisableServerRequestByTutorial)
		{
			this.gachaPackDataList = new List<DataManagerGacha.GachaPackData>();
		}
		this.parentData.ServerRequest(GachaCmd.Create(), new Action<Command>(this.CbGachaCmd));
	}

	public void RequestActionPlayGacha(int id, int type, int useItemId)
	{
		if (this.gachaStaticDataMap.ContainsKey(id) && DataManagerGacha.Category.Box == this.gachaStaticDataMap[id].gachaCategory)
		{
			this.probabilityDataList.RemoveAll((DataManagerGacha.ProbabilityData x) => id == x.gachaId);
		}
		this.puResultList = this.PickCharaList(id);
		this.gachaPlayResult = new DataManagerGacha.PlayResult();
		this.parentData.ServerRequest(GachaExecCmd.Create(id, type, useItemId), new Action<Command>(this.CbGachaExecCmd));
	}

	public void RequestActionRateView(int id)
	{
		this.lastRequestRateViewGachaId = id;
		if (this.probabilityDataList.Find((DataManagerGacha.ProbabilityData x) => x.gachaId == id) == null)
		{
			this.parentData.ServerRequest(GachaRateViewCmd.Create(id), new Action<Command>(this.CbGachaRateViewCmd));
		}
	}

	public void RequestActionGachaReset(int id)
	{
		this.parentData.ServerRequest(GachaResetCmd.Create(id), new Action<Command>(this.CbGachaResetCmd));
	}

	private void CbGachaCmd(Command cmd)
	{
		GachaResponse gachaResponse = cmd.response as GachaResponse;
		this.gachaPackDataList = new List<DataManagerGacha.GachaPackData>();
		using (List<Gacha>.Enumerator enumerator = gachaResponse.gachas.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Gacha gacha = enumerator.Current;
				DataManagerGacha.GachaPackData gpd = this.gachaPackDataList.Find((DataManagerGacha.GachaPackData item) => item.gachaId == gacha.gacha_id);
				if (gpd == null)
				{
					gpd = new DataManagerGacha.GachaPackData
					{
						gachaId = gacha.gacha_id,
						staticData = (this.gachaStaticDataMap.ContainsKey(gacha.gacha_id) ? this.gachaStaticDataMap[gacha.gacha_id] : null),
						dynamicData = new DataManagerGacha.DynamicGachaData()
					};
					gpd.dynamicData.gachaTypeData = new List<DataManagerGacha.DynamicGachaTypeData>();
					this.gachaPackDataList.Add(gpd);
				}
				DataManagerGacha.DynamicGachaTypeData dynamicGachaTypeData = gpd.dynamicData.gachaTypeData.Find((DataManagerGacha.DynamicGachaTypeData x) => x.gachaType == gacha.gacha_type);
				if (dynamicGachaTypeData == null)
				{
					dynamicGachaTypeData = new DataManagerGacha.DynamicGachaTypeData
					{
						gachaType = gacha.gacha_type,
						continuePlayNum = gacha.continue_play_num,
						totalSubPlayNum = gacha.total_sub_play_num,
						boxRemainNum = gacha.box_remain_num,
						resetNum = gacha.reset_num,
						discountPlayNum = gacha.disc_play_num,
						lastPlayDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(gacha.last_play_time)),
						lastPlayTodayDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(gacha.today_last_play_time))
					};
					gpd.dynamicData.gachaTypeData.Add(dynamicGachaTypeData);
					gpd.dynamicData.gachaTypeData.Sort((DataManagerGacha.DynamicGachaTypeData a, DataManagerGacha.DynamicGachaTypeData b) => a.gachaType - b.gachaType);
				}
				else
				{
					dynamicGachaTypeData.continuePlayNum = gacha.continue_play_num;
					dynamicGachaTypeData.totalSubPlayNum = gacha.total_sub_play_num;
					dynamicGachaTypeData.discountPlayNum = gacha.disc_play_num;
					dynamicGachaTypeData.resetNum = gacha.reset_num;
					dynamicGachaTypeData.lastPlayDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(gacha.last_play_time));
					dynamicGachaTypeData.lastPlayTodayDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(gacha.today_last_play_time));
				}
				this.gachaDynamicGachaGroupList.Find((DataManagerGacha.DynamicGachaGroup x) => x.GachaGroupId == gpd.staticData.gachaGroupId).ceilingCountNow = gacha.ceiling_count;
			}
		}
		this.gachaPackDataList.Sort((DataManagerGacha.GachaPackData a, DataManagerGacha.GachaPackData b) => a.staticData.sortIndex - b.staticData.sortIndex);
		this.usableGachaPackDataList = new List<DataManagerGacha.GachaPackData>(this.gachaPackDataList);
		HashSet<int> removeIdSet = new HashSet<int>();
		foreach (DataManagerGacha.GachaPackData gachaPackData in this.usableGachaPackDataList)
		{
			if (gachaPackData.staticData.availableCount > 0)
			{
				bool flag = false;
				foreach (DataManagerGacha.DynamicGachaTypeData dynamicGachaTypeData2 in gachaPackData.dynamicData.gachaTypeData)
				{
					if (gachaPackData.staticData.availableCount > dynamicGachaTypeData2.totalSubPlayNum)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					removeIdSet.Add(gachaPackData.gachaId);
				}
			}
			if (gachaPackData.staticData.gachaCategory == DataManagerGacha.Category.SPECIAL)
			{
				bool flag2 = false;
				foreach (DataManagerGacha.GachaStaticTypeData gachaStaticTypeData in gachaPackData.staticData.typeDataList)
				{
					if (0 < DataManager.DmItem.GetUserItemData(gachaStaticTypeData.useItemId).num || (gachaStaticTypeData.substituteItemId != 0 && 0 < DataManager.DmItem.GetUserItemData(gachaStaticTypeData.substituteItemId).num))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					removeIdSet.Add(gachaPackData.gachaId);
				}
			}
			if (gachaPackData.staticData.stepPreviousGachaId == 0 && gachaPackData.staticData.stepNextGachaId != 0)
			{
				bool flag3 = true;
				DataManagerGacha.GachaPackData nowGacha = gachaPackData;
				HashSet<int> hashSet = new HashSet<int>();
				int num = nowGacha.gachaId;
				Predicate<DataManagerGacha.GachaPackData> <>9__6;
				while (flag3)
				{
					hashSet.Add(nowGacha.gachaId);
					List<DataManagerGacha.GachaPackData> list = this.usableGachaPackDataList;
					Predicate<DataManagerGacha.GachaPackData> predicate;
					if ((predicate = <>9__6) == null)
					{
						predicate = (<>9__6 = (DataManagerGacha.GachaPackData item) => nowGacha.staticData.stepNextGachaId == item.gachaId);
					}
					DataManagerGacha.GachaPackData gachaPackData2 = list.Find(predicate);
					if (gachaPackData2 == null)
					{
						flag3 = false;
						break;
					}
					if (num != gachaPackData.gachaId)
					{
						nowGacha = gachaPackData2;
					}
					else
					{
						using (List<DataManagerGacha.DynamicGachaTypeData>.Enumerator enumerator3 = nowGacha.dynamicData.gachaTypeData.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								DataManagerGacha.DynamicGachaTypeData nowGachaTypeData = enumerator3.Current;
								DataManagerGacha.DynamicGachaTypeData dynamicGachaTypeData3 = gachaPackData2.dynamicData.gachaTypeData.Find((DataManagerGacha.DynamicGachaTypeData x) => nowGachaTypeData.gachaType == x.gachaType);
								if (dynamicGachaTypeData3 != null)
								{
									if (gachaPackData.staticData.gachaCategory == DataManagerGacha.Category.StepUp)
									{
										if (nowGachaTypeData.totalSubPlayNum > dynamicGachaTypeData3.totalSubPlayNum)
										{
											num = gachaPackData2.gachaId;
										}
									}
									else if (gachaPackData.staticData.gachaCategory == DataManagerGacha.Category.Box && nowGachaTypeData.resetNum > dynamicGachaTypeData3.resetNum)
									{
										num = gachaPackData2.gachaId;
									}
									nowGacha = gachaPackData2;
									break;
								}
							}
						}
					}
				}
				hashSet.Remove(num);
				foreach (int num2 in hashSet)
				{
					removeIdSet.Add(num2);
				}
			}
			if (gachaPackData.staticData.gachaCategory == DataManagerGacha.Category.MonthlyPack)
			{
				removeIdSet.Add(gachaPackData.gachaId);
			}
		}
		this.usableGachaPackDataList.RemoveAll((DataManagerGacha.GachaPackData x) => removeIdSet.Contains(x.gachaId));
		this.SortPlayableGachaList();
	}

	private void CbGachaExecCmd(Command cmd)
	{
		GachaExecResponse res = cmd.response as GachaExecResponse;
		DataManagerGacha.GachaResultRank4PuCharaList = new List<int>();
		this.gachaPlayResult.gachaId = res.gacha.gacha_id;
		this.gachaPlayResult.gachaType = res.gacha.gacha_type;
		this.gachaPlayResult.highestRarity = 0;
		List<DataManagerGacha.PlayResult.BonusOneData> list = new List<DataManagerGacha.PlayResult.BonusOneData>();
		foreach (GachaResult gachaResult in res.gacha_result)
		{
			DataManagerGacha.PlayResult.OneData oneData = new DataManagerGacha.PlayResult.OneData
			{
				itemId = gachaResult.item_id,
				itemNum = gachaResult.item_num,
				replaced = (1 == gachaResult.rep_flg),
				isNew = (1 == gachaResult.is_new)
			};
			ItemDef.Kind kind = ItemDef.Id2Kind(oneData.itemId);
			int num = 0;
			if (kind == ItemDef.Kind.CHARA)
			{
				num = DataManager.DmChara.GetCharaStaticData(oneData.itemId).baseData.rankLow;
				using (List<DataManagerGacha.GachaItemdata>.Enumerator enumerator2 = this.puResultList.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if ((enumerator2.Current.itemId == oneData.itemId) & (num > 3))
						{
							this.AddToGachaResultRank4PuCharaList(oneData.itemId);
						}
					}
					goto IL_017D;
				}
				goto IL_0160;
			}
			if (kind == ItemDef.Kind.PHOTO)
			{
				goto IL_0160;
			}
			IL_017D:
			this.gachaPlayResult.highestRarity = ((this.gachaPlayResult.highestRarity < num) ? num : this.gachaPlayResult.highestRarity);
			if (gachaResult.rep_item_list != null)
			{
				foreach (RepItem repItem in gachaResult.rep_item_list)
				{
					if (repItem.rep_item_id == 100003)
					{
						oneData.replaceItemEx = new ItemData(repItem.rep_item_id, repItem.rep_item_num);
						oneData.replaceItemExIsNew = 1 == repItem.is_new;
					}
					else if (repItem.rep_item_id != 0)
					{
						using (List<DataManagerGacha.GachaItemdata>.Enumerator enumerator2 = this.puResultList.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if ((enumerator2.Current.itemId == oneData.itemId) & (num > 3) & (kind == ItemDef.Kind.CHARA))
								{
									this.AddToGachaResultRank4PuCharaList(oneData.itemId);
								}
							}
						}
						oneData.replaceItem = new ItemData(repItem.rep_item_id, repItem.rep_item_num);
						oneData.replaceItemIsNew = 1 == repItem.is_new;
					}
				}
			}
			this.gachaPlayResult.gachaResult.Add(oneData);
			if (gachaResult.omake_item_list != null && 0 < gachaResult.omake_item_list.Count)
			{
				foreach (GachaItemOmakeResult gachaItemOmakeResult in gachaResult.omake_item_list)
				{
					int num2 = gachaItemOmakeResult.item_id;
					int num3 = gachaItemOmakeResult.item_num;
					bool flag = 1 == gachaItemOmakeResult.is_new;
					bool flag2 = false;
					bool flag3 = 1 == gachaItemOmakeResult.is_present_box;
					if (gachaItemOmakeResult.rep_item_list != null && 0 < gachaItemOmakeResult.rep_item_list.Count)
					{
						flag2 = true;
						foreach (RepItem repItem2 in gachaItemOmakeResult.rep_item_list)
						{
							if (repItem2.rep_item_id != 0)
							{
								num2 = repItem2.rep_item_id;
								num3 = repItem2.rep_item_num;
								flag = 1 == repItem2.is_new;
								flag3 |= 1 == repItem2.is_present_box;
							}
						}
					}
					if (num2 != 0)
					{
						list.Add(new DataManagerGacha.PlayResult.BonusOneData
						{
							itemData = new ItemData(num2, num3),
							replaced = flag2,
							isNew = flag,
							isPresentBox = flag3
						});
					}
				}
				continue;
			}
			continue;
			IL_0160:
			num = (int)DataManager.DmPhoto.GetPhotoStaticData(oneData.itemId).baseData.rarity;
			goto IL_017D;
		}
		this.gachaPlayResult.GetPrizeBonus = 0 < list.Count;
		this.gachaPlayResult.prizeBonusList = list;
		List<LotteryItem> list2 = ((res.assets.lottery_item_list != null) ? new List<LotteryItem>(res.assets.lottery_item_list) : new List<LotteryItem>());
		if (res.gachatype_omake != null)
		{
			int bonusItemId2 = res.gachatype_omake.item_id;
			int num4 = res.gachatype_omake.item_num;
			bool flag4 = 1 == res.gachatype_omake.is_new;
			bool flag5 = 1 == res.gachatype_omake.rep_flg;
			bool flag6 = 1 == res.gachatype_omake.is_present_box;
			if (flag5 && res.gachatype_omake.rep_item_list != null && 0 < res.gachatype_omake.rep_item_list.Count)
			{
				foreach (RepItem repItem3 in res.gachatype_omake.rep_item_list)
				{
					if (repItem3.rep_item_id != 0)
					{
						bonusItemId2 = repItem3.rep_item_id;
						num4 = repItem3.rep_item_num;
						flag4 = 1 == repItem3.is_new;
						flag6 |= 1 == repItem3.is_present_box;
					}
				}
			}
			if (ItemDef.Kind.LOTTERY_ITEM == ItemDef.Id2Kind(bonusItemId2))
			{
				int num5 = list2.FindIndex((LotteryItem x) => x.before_item_id == bonusItemId2);
				if (0 <= num5)
				{
					bonusItemId2 = list2[num5].after_item.item_id;
					num4 = list2[num5].after_item.item_num;
					flag4 = 1 == list2[num5].is_new;
					flag6 |= 1 == list2[num5].is_present_box;
					list2.RemoveAt(num5);
				}
			}
			if (bonusItemId2 != 0)
			{
				this.gachaPlayResult.GetBonusOne = true;
				this.gachaPlayResult.bonusOne = new DataManagerGacha.PlayResult.BonusOneData
				{
					itemData = new ItemData(bonusItemId2, num4),
					replaced = flag5,
					isNew = flag4,
					isPresentBox = flag6
				};
			}
		}
		if (res.gachatype_omake_preset != null && 0 < res.gachatype_omake_preset.Count)
		{
			List<DataManagerGacha.PlayResult.BonusOneData> list3 = new List<DataManagerGacha.PlayResult.BonusOneData>();
			foreach (GachaResult gachaResult2 in res.gachatype_omake_preset)
			{
				int bonusItemId = gachaResult2.item_id;
				int num6 = gachaResult2.item_num;
				bool flag7 = 1 == gachaResult2.is_new;
				bool flag8 = 1 == gachaResult2.rep_flg;
				bool flag9 = 1 == gachaResult2.is_present_box;
				if (flag8 && gachaResult2.rep_item_list != null && 0 < gachaResult2.rep_item_list.Count)
				{
					foreach (RepItem repItem4 in gachaResult2.rep_item_list)
					{
						if (repItem4.rep_item_id != 0)
						{
							bonusItemId = repItem4.rep_item_id;
							num6 = repItem4.rep_item_num;
							flag7 = 1 == repItem4.is_new;
							flag9 |= 1 == repItem4.is_present_box;
						}
					}
				}
				if (ItemDef.Kind.LOTTERY_ITEM == ItemDef.Id2Kind(bonusItemId))
				{
					int num7 = list2.FindIndex((LotteryItem x) => x.before_item_id == bonusItemId);
					if (0 <= num7)
					{
						bonusItemId = list2[num7].after_item.item_id;
						num6 = list2[num7].after_item.item_num;
						flag7 = 1 == list2[num7].is_new;
						flag9 |= 1 == list2[num7].is_present_box;
						list2.RemoveAt(num7);
					}
				}
				if (bonusItemId != 0)
				{
					list3.Add(new DataManagerGacha.PlayResult.BonusOneData
					{
						itemData = new ItemData(bonusItemId, num6),
						replaced = flag8,
						isNew = flag7,
						isPresentBox = flag9
					});
				}
			}
			this.gachaPlayResult.GetBonusSet = 0 < list3.Count;
			this.gachaPlayResult.bonusSet = list3;
		}
		this.parentData.UpdateUserAssetByAssets(res.assets);
		DataManagerGacha.GachaPackData gachaPackData = this.gachaPackDataList.Find((DataManagerGacha.GachaPackData x) => x.staticData.gachaId == res.gacha.gacha_id);
		if (gachaPackData == null)
		{
			return;
		}
		DataManagerGacha.DynamicGachaGroup dynamicGachaGroup = this.gachaDynamicGachaGroupList.Find((DataManagerGacha.DynamicGachaGroup x) => x.GachaGroupId == gachaPackData.staticData.gachaGroupId);
		if (dynamicGachaGroup == null)
		{
			return;
		}
		dynamicGachaGroup.ceilingCountNow = res.gacha.ceiling_count;
		DataManagerGacha.GachaStaticData gsd = (this.gachaStaticDataMap.ContainsKey(this.gachaPlayResult.gachaId) ? this.gachaStaticDataMap[this.gachaPlayResult.gachaId] : null);
		if (gsd == null)
		{
			return;
		}
		gsd.typeDataList.Find((DataManagerGacha.GachaStaticTypeData x) => x.gachaType == this.gachaPlayResult.gachaType);
		DataManagerGacha.DynamicGachaTypeData dynamicGachaTypeData = gachaPackData.dynamicData.gachaTypeData.Find((DataManagerGacha.DynamicGachaTypeData x) => x.gachaType == res.gacha.gacha_type);
		if (dynamicGachaTypeData != null)
		{
			dynamicGachaTypeData.continuePlayNum = res.gacha.continue_play_num;
			dynamicGachaTypeData.totalSubPlayNum = res.gacha.total_sub_play_num;
			dynamicGachaTypeData.lastPlayDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(res.gacha.last_play_time));
			dynamicGachaTypeData.lastPlayTodayDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(res.gacha.today_last_play_time));
		}
		if (gsd.gachaCategory == DataManagerGacha.Category.Box)
		{
			this.probabilityDataList.RemoveAll((DataManagerGacha.ProbabilityData x) => x.gachaId == gsd.gachaId);
			return;
		}
		if (gsd.gachaCategory == DataManagerGacha.Category.Roulette)
		{
			HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
			if ((homeCheckResult != null || homeCheckResult.rouletteData != null) && homeCheckResult.rouletteData.remainingDrawCount > 0)
			{
				homeCheckResult.rouletteData.remainingDrawCount--;
			}
		}
	}

	private void CbGachaRateViewCmd(Command cmd)
	{
		GachaRateViewResponse gachaRateViewResponse = cmd.response as GachaRateViewResponse;
		RarityViewResult gacha_rarity_result = gachaRateViewResponse.gacha_rarity_result;
		CharaViewResult gacha_charas_result = gachaRateViewResponse.gacha_charas_result;
		PhotoViewResult gacha_photos_result = gachaRateViewResponse.gacha_photos_result;
		MasterRoomFurnitureViewResult gacha_master_room_furniture_result = gachaRateViewResponse.gacha_master_room_furniture_result;
		ItemViewResult gacha_item_result = gachaRateViewResponse.gacha_item_result;
		DataManagerGacha.ProbabilityData probabilityData = new DataManagerGacha.ProbabilityData
		{
			gachaId = this.lastRequestRateViewGachaId,
			elements = new List<DataManagerGacha.ProbabilityData.Element>()
		};
		List<DataManagerGacha.GachaItemdata> gachaItemData = this.GetGachaStaticData(this.lastRequestRateViewGachaId).gachaItemData;
		DataManagerGacha.ProbabilityData.Element element = new DataManagerGacha.ProbabilityData.Element(DataManagerGacha.ProbabilityData.Type.Chara, DataManagerGacha.ProbabilityData.Category.Rarity);
		element.rate = new List<DataManagerGacha.ProbabilityData.ItemOne>();
		foreach (GachaRateRarity gachaRateRarity in gacha_rarity_result.rarity_charas.rarity)
		{
			DataManagerGacha.ProbabilityData.ItemOne itemOne = new DataManagerGacha.ProbabilityData.ItemOne(gachaRateRarity.rarity, gachaRateRarity.normal, gachaRateRarity.decided, gachaRateRarity.decided_3, gachaRateRarity.decided_4, gachaRateRarity.decided_ceiling);
			element.rate.Add(itemOne);
		}
		probabilityData.elements.Add(element);
		DataManagerGacha.ProbabilityData.Element element2 = new DataManagerGacha.ProbabilityData.Element(DataManagerGacha.ProbabilityData.Type.Photo, DataManagerGacha.ProbabilityData.Category.Rarity);
		element2.rate = new List<DataManagerGacha.ProbabilityData.ItemOne>();
		foreach (GachaRateRarity gachaRateRarity2 in gacha_rarity_result.rarity_photos.rarity)
		{
			DataManagerGacha.ProbabilityData.ItemOne itemOne2 = new DataManagerGacha.ProbabilityData.ItemOne(gachaRateRarity2.rarity, gachaRateRarity2.normal, gachaRateRarity2.decided, gachaRateRarity2.decided_3, gachaRateRarity2.decided_4, gachaRateRarity2.decided_ceiling);
			element2.rate.Add(itemOne2);
		}
		probabilityData.elements.Add(element2);
		DataManagerGacha.ProbabilityData.Element element3 = new DataManagerGacha.ProbabilityData.Element(DataManagerGacha.ProbabilityData.Type.TreeHouseFurniture, DataManagerGacha.ProbabilityData.Category.Rarity);
		element3.rate = new List<DataManagerGacha.ProbabilityData.ItemOne>();
		foreach (GachaRateRarity gachaRateRarity3 in gacha_rarity_result.rarity_master_room_furnitures.rarity)
		{
			DataManagerGacha.ProbabilityData.ItemOne itemOne3 = new DataManagerGacha.ProbabilityData.ItemOne(gachaRateRarity3.rarity, gachaRateRarity3.normal, gachaRateRarity3.decided, gachaRateRarity3.decided_3, gachaRateRarity3.decided_4, gachaRateRarity3.decided_ceiling);
			element3.rate.Add(itemOne3);
		}
		probabilityData.elements.Add(element3);
		DataManagerGacha.ProbabilityData.Element element4 = new DataManagerGacha.ProbabilityData.Element(DataManagerGacha.ProbabilityData.Type.Item, DataManagerGacha.ProbabilityData.Category.Rarity);
		element4.rate = new List<DataManagerGacha.ProbabilityData.ItemOne>();
		foreach (GachaRateRarity gachaRateRarity4 in gacha_rarity_result.rarity_items.rarity)
		{
			DataManagerGacha.ProbabilityData.ItemOne itemOne4 = new DataManagerGacha.ProbabilityData.ItemOne(gachaRateRarity4.rarity, gachaRateRarity4.normal, gachaRateRarity4.decided, gachaRateRarity4.decided_3, gachaRateRarity4.decided_4, gachaRateRarity4.decided_ceiling);
			element4.rate.Add(itemOne4);
		}
		probabilityData.elements.Add(element4);
		using (List<GachaRateItem>.Enumerator enumerator2 = gacha_charas_result.pickup.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				GachaRateItem pickup2 = enumerator2.Current;
				DataManagerGacha.GachaItemdata gachaItemdata = gachaItemData.Find((DataManagerGacha.GachaItemdata x) => x.itemId == pickup2.item_id && x.itemNum == pickup2.item_num);
				DataManagerGacha.ProbabilityData.Element element5 = new DataManagerGacha.ProbabilityData.Element(DataManagerGacha.ProbabilityData.Type.Chara, DataManagerGacha.ProbabilityData.Category.PickUp, gachaItemdata, pickup2.remain_num);
				element5.rate = new List<DataManagerGacha.ProbabilityData.ItemOne>();
				DataManagerGacha.ProbabilityData.ItemOne itemOne5 = new DataManagerGacha.ProbabilityData.ItemOne(pickup2.normal, pickup2.decided, pickup2.decided_3, pickup2.decided_4, pickup2.decided_ceiling);
				CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(pickup2.item_id);
				itemOne5.rarity = charaStaticData.baseData.rankLow;
				element5.rate.Add(itemOne5);
				element5.item_num = pickup2.item_num;
				probabilityData.elements.Add(element5);
			}
		}
		using (List<GachaRateItem>.Enumerator enumerator2 = gacha_charas_result.other.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				GachaRateItem other2 = enumerator2.Current;
				DataManagerGacha.GachaItemdata gachaItemdata2 = gachaItemData.Find((DataManagerGacha.GachaItemdata x) => x.itemId == other2.item_id && x.itemNum == other2.item_num);
				DataManagerGacha.ProbabilityData.Element element6 = new DataManagerGacha.ProbabilityData.Element(DataManagerGacha.ProbabilityData.Type.Chara, DataManagerGacha.ProbabilityData.Category.Other, gachaItemdata2, other2.remain_num);
				element6.rate = new List<DataManagerGacha.ProbabilityData.ItemOne>();
				DataManagerGacha.ProbabilityData.ItemOne itemOne6 = new DataManagerGacha.ProbabilityData.ItemOne(other2.normal, other2.decided, other2.decided_3, other2.decided_4, other2.decided_ceiling);
				CharaStaticData charaStaticData2 = DataManager.DmChara.GetCharaStaticData(other2.item_id);
				itemOne6.rarity = charaStaticData2.baseData.rankLow;
				element6.rate.Add(itemOne6);
				element6.item_num = other2.item_num;
				probabilityData.elements.Add(element6);
			}
		}
		using (List<GachaRateItem>.Enumerator enumerator2 = gacha_photos_result.pickup.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				GachaRateItem pickup3 = enumerator2.Current;
				DataManagerGacha.GachaItemdata gachaItemdata3 = gachaItemData.Find((DataManagerGacha.GachaItemdata x) => x.itemId == pickup3.item_id && x.itemNum == pickup3.item_num);
				DataManagerGacha.ProbabilityData.Element element7 = new DataManagerGacha.ProbabilityData.Element(DataManagerGacha.ProbabilityData.Type.Photo, DataManagerGacha.ProbabilityData.Category.PickUp, gachaItemdata3, pickup3.remain_num);
				element7.rate = new List<DataManagerGacha.ProbabilityData.ItemOne>();
				DataManagerGacha.ProbabilityData.ItemOne itemOne7 = new DataManagerGacha.ProbabilityData.ItemOne(pickup3.normal, pickup3.decided, pickup3.decided_3, pickup3.decided_4, pickup3.decided_ceiling);
				PhotoStaticData photoStaticData = DataManager.DmPhoto.GetPhotoStaticData(pickup3.item_id);
				itemOne7.rarity = (int)photoStaticData.GetRarity();
				element7.rate.Add(itemOne7);
				element7.item_num = pickup3.item_num;
				probabilityData.elements.Add(element7);
			}
		}
		using (List<GachaRateItem>.Enumerator enumerator2 = gacha_photos_result.other.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				GachaRateItem other3 = enumerator2.Current;
				DataManagerGacha.GachaItemdata gachaItemdata4 = gachaItemData.Find((DataManagerGacha.GachaItemdata x) => x.itemId == other3.item_id && x.itemNum == other3.item_num);
				DataManagerGacha.ProbabilityData.Element element8 = new DataManagerGacha.ProbabilityData.Element(DataManagerGacha.ProbabilityData.Type.Photo, DataManagerGacha.ProbabilityData.Category.Other, gachaItemdata4, other3.remain_num);
				element8.rate = new List<DataManagerGacha.ProbabilityData.ItemOne>();
				DataManagerGacha.ProbabilityData.ItemOne itemOne8 = new DataManagerGacha.ProbabilityData.ItemOne(other3.normal, other3.decided, other3.decided_3, other3.decided_4, other3.decided_ceiling);
				PhotoStaticData photoStaticData2 = DataManager.DmPhoto.GetPhotoStaticData(other3.item_id);
				itemOne8.rarity = (int)photoStaticData2.GetRarity();
				element8.rate.Add(itemOne8);
				element8.item_num = other3.item_num;
				probabilityData.elements.Add(element8);
			}
		}
		using (List<GachaRateItem>.Enumerator enumerator2 = gacha_master_room_furniture_result.pickup.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				GachaRateItem pickup4 = enumerator2.Current;
				DataManagerGacha.GachaItemdata gachaItemdata5 = gachaItemData.Find((DataManagerGacha.GachaItemdata x) => x.itemId == pickup4.item_id && x.itemNum == pickup4.item_num);
				DataManagerGacha.ProbabilityData.Element element9 = new DataManagerGacha.ProbabilityData.Element(DataManagerGacha.ProbabilityData.Type.TreeHouseFurniture, DataManagerGacha.ProbabilityData.Category.PickUp, gachaItemdata5, pickup4.remain_num);
				element9.rate = new List<DataManagerGacha.ProbabilityData.ItemOne>();
				DataManagerGacha.ProbabilityData.ItemOne itemOne9 = new DataManagerGacha.ProbabilityData.ItemOne(pickup4.normal, pickup4.decided, pickup4.decided_3, pickup4.decided_4, pickup4.decided_ceiling);
				TreeHouseFurnitureStatic treeHouseFurnitureStaticData = DataManager.DmTreeHouse.GetTreeHouseFurnitureStaticData(pickup4.item_id);
				itemOne9.rarity = (int)treeHouseFurnitureStaticData.GetRarity();
				element9.rate.Add(itemOne9);
				element9.item_num = pickup4.item_num;
				probabilityData.elements.Add(element9);
			}
		}
		using (List<GachaRateItem>.Enumerator enumerator2 = gacha_master_room_furniture_result.other.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				GachaRateItem other4 = enumerator2.Current;
				DataManagerGacha.GachaItemdata gachaItemdata6 = gachaItemData.Find((DataManagerGacha.GachaItemdata x) => x.itemId == other4.item_id && x.itemNum == other4.item_num);
				DataManagerGacha.ProbabilityData.Element element10 = new DataManagerGacha.ProbabilityData.Element(DataManagerGacha.ProbabilityData.Type.TreeHouseFurniture, DataManagerGacha.ProbabilityData.Category.Other, gachaItemdata6, other4.remain_num);
				element10.rate = new List<DataManagerGacha.ProbabilityData.ItemOne>();
				DataManagerGacha.ProbabilityData.ItemOne itemOne10 = new DataManagerGacha.ProbabilityData.ItemOne(other4.normal, other4.decided, other4.decided_3, other4.decided_4, other4.decided_ceiling);
				ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(other4.item_id);
				itemOne10.rarity = (int)itemStaticBase.GetRarity();
				element10.rate.Add(itemOne10);
				element10.item_num = other4.item_num;
				probabilityData.elements.Add(element10);
			}
		}
		using (List<GachaRateItem>.Enumerator enumerator2 = gacha_item_result.pickup.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				GachaRateItem pickup = enumerator2.Current;
				DataManagerGacha.GachaItemdata gachaItemdata7 = gachaItemData.Find((DataManagerGacha.GachaItemdata x) => x.itemId == pickup.item_id && x.itemNum == pickup.item_num);
				DataManagerGacha.ProbabilityData.Element element11 = new DataManagerGacha.ProbabilityData.Element(DataManagerGacha.ProbabilityData.Type.Item, DataManagerGacha.ProbabilityData.Category.PickUp, gachaItemdata7, pickup.remain_num);
				element11.rate = new List<DataManagerGacha.ProbabilityData.ItemOne>();
				DataManagerGacha.ProbabilityData.ItemOne itemOne11 = new DataManagerGacha.ProbabilityData.ItemOne(pickup.normal, pickup.decided, pickup.decided_3, pickup.decided_4, pickup.decided_ceiling);
				ItemStaticBase itemStaticBase2 = DataManager.DmItem.GetItemStaticBase(pickup.item_id);
				itemOne11.rarity = (int)itemStaticBase2.GetRarity();
				element11.rate.Add(itemOne11);
				element11.item_num = pickup.item_num;
				probabilityData.elements.Add(element11);
			}
		}
		using (List<GachaRateItem>.Enumerator enumerator2 = gacha_item_result.other.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				GachaRateItem other = enumerator2.Current;
				DataManagerGacha.GachaItemdata gachaItemdata8 = gachaItemData.Find((DataManagerGacha.GachaItemdata x) => x.itemId == other.item_id && x.itemNum == other.item_num);
				DataManagerGacha.ProbabilityData.Element element12 = new DataManagerGacha.ProbabilityData.Element(DataManagerGacha.ProbabilityData.Type.Item, DataManagerGacha.ProbabilityData.Category.Other, gachaItemdata8, other.remain_num);
				element12.rate = new List<DataManagerGacha.ProbabilityData.ItemOne>();
				DataManagerGacha.ProbabilityData.ItemOne itemOne12 = new DataManagerGacha.ProbabilityData.ItemOne(other.normal, other.decided, other.decided_3, other.decided_4, other.decided_ceiling);
				ItemStaticBase itemStaticBase3 = DataManager.DmItem.GetItemStaticBase(other.item_id);
				itemOne12.rarity = (int)itemStaticBase3.GetRarity();
				element12.rate.Add(itemOne12);
				element12.item_num = other.item_num;
				probabilityData.elements.Add(element12);
			}
		}
		this.probabilityDataList.Add(probabilityData);
	}

	private void CbGachaResetCmd(Command cmd)
	{
		Response response = cmd.response;
		GachaResetRequest gachaResetRequest = cmd.request as GachaResetRequest;
		DataManagerGacha.GachaStaticData gsd = (this.gachaStaticDataMap.ContainsKey(gachaResetRequest.gacha_id) ? this.gachaStaticDataMap[gachaResetRequest.gacha_id] : null);
		if (gsd.gachaCategory == DataManagerGacha.Category.Box)
		{
			this.probabilityDataList.RemoveAll((DataManagerGacha.ProbabilityData x) => x.gachaId == gsd.gachaId);
		}
	}

	public void InitializeMstData(MstManager mstManager)
	{
		this.gachaStaticDataMap = new Dictionary<int, DataManagerGacha.GachaStaticData>();
		this.gachaDynamicGachaGroupList = new List<DataManagerGacha.DynamicGachaGroup>();
		List<MstGachaData> mst = mstManager.GetMst<List<MstGachaData>>(MstType.GACHA_DATA);
		List<MstGachaTypeData> mst2 = mstManager.GetMst<List<MstGachaTypeData>>(MstType.GACHA_TYPE_DATA);
		List<MstGachaItemData> mst3 = mstManager.GetMst<List<MstGachaItemData>>(MstType.GACHA_ITEM_DATA);
		List<MstGachaDiscData> mst4 = mstManager.GetMst<List<MstGachaDiscData>>(MstType.GACHA_DISC_DATA);
		using (List<MstGachaData>.Enumerator enumerator = mst.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MstGachaData mstGachaData = enumerator.Current;
				if (this.gachaDynamicGachaGroupList.Find((DataManagerGacha.DynamicGachaGroup x) => x.GachaGroupId == mstGachaData.gachaGroupId) == null)
				{
					DataManagerGacha.DynamicGachaGroup dynamicGachaGroup = new DataManagerGacha.DynamicGachaGroup(mstGachaData.gachaGroupId)
					{
						ceilingCountNow = 0
					};
					this.gachaDynamicGachaGroupList.Add(dynamicGachaGroup);
				}
				DataManagerGacha.GachaStaticData gsd = new DataManagerGacha.GachaStaticData(mstGachaData);
				gsd.bonusIdList = new List<int>();
				List<MstGachaTypeData> list = mst2.FindAll((MstGachaTypeData item) => item.gachaId == mstGachaData.gachaId);
				list.Sort((MstGachaTypeData a, MstGachaTypeData b) => a.gachaType - b.gachaType);
				gsd.typeDataList = new List<DataManagerGacha.GachaStaticTypeData>();
				using (List<MstGachaTypeData>.Enumerator enumerator2 = list.GetEnumerator())
				{
					Action<ItemPresetData.Item> <>9__5;
					while (enumerator2.MoveNext())
					{
						MstGachaTypeData typeData = enumerator2.Current;
						DataManagerGacha.GachaStaticTypeData gachaStaticTypeData = new DataManagerGacha.GachaStaticTypeData(typeData);
						MstGachaDiscData mstGachaDiscData = mst4.Find((MstGachaDiscData x) => x.discountId == typeData.discountId);
						if (mstGachaDiscData != null)
						{
							gachaStaticTypeData.discountData = new DataManagerGacha.DiscountData
							{
								discountId = mstGachaDiscData.discountId,
								discountName = mstGachaDiscData.discountName,
								discountType = (DataManagerGacha.DiscountType)mstGachaDiscData.discountType,
								discountNum = mstGachaDiscData.discountNum,
								startDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mstGachaDiscData.startDatetime)),
								endDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mstGachaDiscData.endDatetime)),
								availableCount = mstGachaDiscData.availableCount
							};
						}
						gsd.typeDataList.Add(gachaStaticTypeData);
						if (gachaStaticTypeData.bonusItemOneId > 0)
						{
							gsd.bonusIdList.Add(gachaStaticTypeData.bonusItemOneId);
						}
						if (gachaStaticTypeData.bonusItemSetId > 0)
						{
							List<ItemPresetData.Item> setItemList = (new ItemData(gachaStaticTypeData.bonusItemSetId, 0).staticData as ItemPresetData).SetItemList;
							Action<ItemPresetData.Item> action;
							if ((action = <>9__5) == null)
							{
								action = (<>9__5 = delegate(ItemPresetData.Item x)
								{
									gsd.bonusIdList.Add(x.itemId);
								});
							}
							setItemList.ForEach(action);
						}
					}
				}
				gsd.typeDataList.Sort((DataManagerGacha.GachaStaticTypeData a, DataManagerGacha.GachaStaticTypeData b) => a.gachaType - b.gachaType);
				List<MstGachaItemData> list2 = mst3.FindAll((MstGachaItemData x) => x.gachaId == gsd.gachaId);
				gsd.gachaItemData = new List<DataManagerGacha.GachaItemdata>();
				foreach (MstGachaItemData mstGachaItemData in list2)
				{
					DataManagerGacha.GachaItemdata gachaItemdata = new DataManagerGacha.GachaItemdata
					{
						itemId = mstGachaItemData.itemId,
						itemNum = mstGachaItemData.itemNum,
						sortNum = mstGachaItemData.sortNum,
						stackNum = mstGachaItemData.stackNum,
						pickUpFlg = (1 == mstGachaItemData.pickupFlg),
						dispClientTypeLimit = mstGachaItemData.dispClientTypeLimit,
						dispClientTypeNew = mstGachaItemData.dispClientTypeNew,
						bonusItemId01 = mstGachaItemData.bonusItemId01,
						bonusItemNum01 = mstGachaItemData.bonusItemNum01,
						bonusItemId02 = mstGachaItemData.bonusItemId02,
						bonusItemNum02 = mstGachaItemData.bonusItemNum02,
						bonusItemId03 = mstGachaItemData.bonusItemId03,
						bonusItemNum03 = mstGachaItemData.bonusItemNum03
					};
					gsd.gachaItemData.Add(gachaItemdata);
				}
				List<MstGachaItemData> list3 = list2.FindAll((MstGachaItemData x) => 1 == x.dispClientType);
				list3.Sort((MstGachaItemData a, MstGachaItemData b) => a.sortNum - b.sortNum);
				gsd.InfoDispIdList = new List<int>();
				foreach (MstGachaItemData mstGachaItemData2 in list3)
				{
					gsd.InfoDispIdList.Add(mstGachaItemData2.itemId);
				}
				List<MstGachaItemData> list4 = list2.FindAll((MstGachaItemData x) => (x.bonusItemId01 != 0 && 0 < x.bonusItemNum01) || (x.bonusItemId02 != 0 && 0 < x.bonusItemNum02) || (x.bonusItemId03 != 0 && 0 < x.bonusItemNum03));
				list4.Sort((MstGachaItemData a, MstGachaItemData b) => a.sortNum - b.sortNum);
				gsd.enableBonusItemIdList = new List<int>();
				foreach (MstGachaItemData mstGachaItemData3 in list4)
				{
					gsd.enableBonusItemIdList.Add(mstGachaItemData3.itemId);
					if (mstGachaItemData3.bonusItemId01 != 0 && 0 < mstGachaItemData3.bonusItemNum01)
					{
						gsd.bonusIdList.Add(mstGachaItemData3.bonusItemId01);
					}
					if (mstGachaItemData3.bonusItemId02 != 0 && 0 < mstGachaItemData3.bonusItemNum02)
					{
						gsd.bonusIdList.Add(mstGachaItemData3.bonusItemId02);
					}
					if (mstGachaItemData3.bonusItemId03 != 0 && 0 < mstGachaItemData3.bonusItemNum03)
					{
						gsd.bonusIdList.Add(mstGachaItemData3.bonusItemId03);
					}
				}
				this.gachaStaticDataMap.Add(gsd.gachaId, gsd);
				this.probabilityDataList = new List<DataManagerGacha.ProbabilityData>();
			}
		}
		foreach (KeyValuePair<int, DataManagerGacha.GachaStaticData> keyValuePair in this.gachaStaticDataMap)
		{
			if (this.gachaStaticDataMap.ContainsKey(keyValuePair.Value.stepPreviousGachaId))
			{
				this.gachaStaticDataMap[keyValuePair.Value.stepPreviousGachaId].stepNextGachaId = keyValuePair.Value.gachaId;
			}
		}
	}

	public void SortPlayableGachaList()
	{
		this.usableGachaPackDataList.Sort((DataManagerGacha.GachaPackData a, DataManagerGacha.GachaPackData b) => a.staticData.gachaId - b.staticData.gachaId);
		PrjUtil.InsertionSort<DataManagerGacha.GachaPackData>(ref this.usableGachaPackDataList, (DataManagerGacha.GachaPackData a, DataManagerGacha.GachaPackData b) => a.staticData.sortIndex - b.staticData.sortIndex);
		List<DataManagerGacha.GachaPackData> list = this.usableGachaPackDataList.FindAll((DataManagerGacha.GachaPackData x) => x.staticData.typeDataList.Any<DataManagerGacha.GachaStaticTypeData>((DataManagerGacha.GachaStaticTypeData y) => y.useItemId == 30100 && y.useItemNumber == 0));
		List<DataManagerGacha.GachaPackData> list2 = this.usableGachaPackDataList.FindAll((DataManagerGacha.GachaPackData x) => x.staticData.typeDataList.Any<DataManagerGacha.GachaStaticTypeData>((DataManagerGacha.GachaStaticTypeData y) => y.discountData != null && y.discountData.discountType == DataManagerGacha.DiscountType.OnceADay));
		List<DataManagerGacha.GachaPackData> list3 = new List<DataManagerGacha.GachaPackData>();
		DateTime now = TimeManager.Now;
		foreach (DataManagerGacha.GachaPackData gachaPackData in list2)
		{
			DataManagerGacha.GachaStaticTypeData typeData = gachaPackData.staticData.typeDataList[0];
			DateTime dateTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
			DataManagerGacha.DynamicGachaTypeData dynamicGachaTypeData = gachaPackData.dynamicData.gachaTypeData.Find((DataManagerGacha.DynamicGachaTypeData x) => x.gachaType == typeData.gachaType);
			int num = typeData.useItemNumber;
			if (dynamicGachaTypeData.lastPlayDateTime < dateTime && typeData.discountData != null)
			{
				num -= typeData.discountData.discountNum;
			}
			if (num > 0)
			{
				list3.Add(gachaPackData);
			}
		}
		foreach (DataManagerGacha.GachaPackData gachaPackData2 in list3)
		{
			list2.Remove(gachaPackData2);
		}
		PrjUtil.InsertionSort<DataManagerGacha.GachaPackData>(ref list, (DataManagerGacha.GachaPackData a, DataManagerGacha.GachaPackData b) => b.staticData.gachaGroupId.CompareTo(a.staticData.gachaGroupId));
		PrjUtil.InsertionSort<DataManagerGacha.GachaPackData>(ref list, (DataManagerGacha.GachaPackData a, DataManagerGacha.GachaPackData b) => b.staticData.sortIndex.CompareTo(a.staticData.sortIndex));
		list.AddRange(list2);
		if (list.Count == 0)
		{
			return;
		}
		foreach (DataManagerGacha.GachaPackData gachaPackData3 in list)
		{
			this.usableGachaPackDataList.Remove(gachaPackData3);
			this.usableGachaPackDataList.Insert(0, gachaPackData3);
		}
	}

	public void SetGachaPackDataByDirect(DataManagerGacha.GachaPackData _gachaPackData)
	{
		this.gachaPackDataList = new List<DataManagerGacha.GachaPackData> { _gachaPackData };
		this.usableGachaPackDataList = this.gachaPackDataList;
	}

	private List<DataManagerGacha.GachaItemdata> PickCharaList(int search_gacha_id)
	{
		List<DataManagerGacha.GachaItemdata> list = new List<DataManagerGacha.GachaItemdata>();
		if (this.gachaStaticDataMap.ContainsKey(search_gacha_id))
		{
			list = this.gachaStaticDataMap[search_gacha_id].gachaItemData.Where<DataManagerGacha.GachaItemdata>((DataManagerGacha.GachaItemdata item) => item.pickUpFlg).ToList<DataManagerGacha.GachaItemdata>();
		}
		return list;
	}

	private DataManager parentData;

	private Dictionary<int, DataManagerGacha.GachaStaticData> gachaStaticDataMap;

	private List<DataManagerGacha.GachaPackData> gachaPackDataList;

	private List<DataManagerGacha.GachaPackData> usableGachaPackDataList;

	private List<DataManagerGacha.DynamicGachaGroup> gachaDynamicGachaGroupList;

	private DataManagerGacha.PlayResult gachaPlayResult;

	private List<DataManagerGacha.ProbabilityData> probabilityDataList;

	private int lastRequestRateViewGachaId;

	private List<DataManagerGacha.GachaItemdata> puResultList;

	private static List<int> GachaResultRank4PuCharaList;

	public class GachaPackData
	{
		public int GetUseItemId(int typeId)
		{
			DataManagerGacha.GachaStaticTypeData gachaStaticTypeData = this.staticData.typeDataList.Find((DataManagerGacha.GachaStaticTypeData item) => item.gachaType == typeId);
			if (gachaStaticTypeData == null)
			{
				return 0;
			}
			if (gachaStaticTypeData.substituteItemId == 0)
			{
				return gachaStaticTypeData.useItemId;
			}
			if (DataManager.DmItem.GetUserItemData(gachaStaticTypeData.substituteItemId).num <= 0)
			{
				return gachaStaticTypeData.useItemId;
			}
			return gachaStaticTypeData.substituteItemId;
		}

		public int gachaId;

		public DataManagerGacha.GachaStaticData staticData = new DataManagerGacha.GachaStaticData(new MstGachaData());

		public DataManagerGacha.DynamicGachaData dynamicData = new DataManagerGacha.DynamicGachaData();
	}

	public class GachaStaticData
	{
		public DateTime StepResetTime { get; private set; }

		public bool RateHideFlg { get; private set; }

		public List<int> InfoDispIdList { get; set; }

		public DateTime startDatetime { get; set; }

		public DateTime endDatetime { get; set; }

		public bool dayOfWeekFlg { get; set; }

		public bool IsResultTreeHouseFurnitureInfo { get; private set; }

		public int ReplaceGroupId { get; private set; }

		public GachaStaticData()
		{
		}

		public GachaStaticData(MstGachaData mstGachaData)
		{
			this.gachaId = mstGachaData.gachaId;
			this.gachaGroupId = mstGachaData.gachaGroupId;
			this.gachaCategory = (DataManagerGacha.Category)mstGachaData.gachaCategory;
			this.gachaName = mstGachaData.gachaName;
			this.labelTextureName = mstGachaData.labelTextureName;
			this.banner = mstGachaData.banner;
			this.sortIndex = mstGachaData.sortIndex;
			this.highLimit = mstGachaData.highLimit;
			this.highLimitCountFlag = 1 == mstGachaData.highLimitCountFlg;
			this.detailDispText = mstGachaData.detailDispText;
			this.availableCount = mstGachaData.availableCount;
			this.recommendFlg = 1 == mstGachaData.recommendFlg;
			this.RateHideFlg = 1 == mstGachaData.rateHiddenFlg;
			this.ReplaceGroupId = mstGachaData.replaceGroupId;
			this.stepPreviousGachaId = mstGachaData.stepParentGachaId;
			this.StepResetTime = new DateTime(1900, 1, 1, 0, 0, 0);
			if (!string.IsNullOrEmpty(mstGachaData.resetStepTime))
			{
				string[] array = mstGachaData.resetStepTime.Split(':', StringSplitOptions.None);
				if (3 == array.Length)
				{
					int num = int.Parse(array[0]);
					int num2 = int.Parse(array[1]);
					int num3 = int.Parse(array[2]);
					this.StepResetTime = new DateTime(2000, 1, 1, num, num2, num3);
				}
			}
			this.stepupBtnText = mstGachaData.stepupBtnText;
			this.startDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mstGachaData.startDatetime));
			this.endDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mstGachaData.endDatetime));
			this.dayOfWeek[0] = 1 == mstGachaData.sundayFlg;
			this.dayOfWeek[1] = 1 == mstGachaData.mondayFlg;
			this.dayOfWeek[2] = 1 == mstGachaData.tuesdayFlg;
			this.dayOfWeek[3] = 1 == mstGachaData.wednesdayFlg;
			this.dayOfWeek[4] = 1 == mstGachaData.thursdayFlg;
			this.dayOfWeek[5] = 1 == mstGachaData.fridayFlg;
			this.dayOfWeek[6] = 1 == mstGachaData.saturdayFlg;
			this.tabCategory = (DataManagerGacha.TabCategory)mstGachaData.tabCategory;
			int num4 = 0;
			bool[] array2 = this.dayOfWeek;
			for (int i = 0; i < array2.Length; i++)
			{
				if (array2[i])
				{
					num4++;
				}
			}
			if (num4 >= 7)
			{
				this.dayOfWeekFlg = false;
			}
			else if (num4 >= 1)
			{
				this.dayOfWeekFlg = true;
			}
			this.IsResultTreeHouseFurnitureInfo = mstGachaData.resultInfoType == 1;
		}

		public DateTime EndTimeOfDayOfWeek(DateTime nowTime)
		{
			DateTime dateTime = this.endDatetime;
			DayOfWeek dayOfWeek = nowTime.DayOfWeek;
			if (!this.dayOfWeekFlg || !this.dayOfWeek[(int)dayOfWeek])
			{
				return dateTime;
			}
			int num = 0;
			while (num <= 6 && this.dayOfWeek[(int)((dayOfWeek + num) % (DayOfWeek)7)])
			{
				dateTime = nowTime.AddDays((double)num);
				num++;
			}
			if (this.endDatetime <= dateTime && this.dayOfWeek[(int)this.endDatetime.DayOfWeek])
			{
				return this.endDatetime;
			}
			return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
		}

		public int gachaId;

		public DataManagerGacha.Category gachaCategory;

		public string gachaName;

		public int gachaGroupId;

		public string labelTextureName;

		public string banner;

		public int sortIndex;

		public int highLimit;

		public bool highLimitCountFlag;

		public string detailDispText;

		public int availableCount;

		public bool recommendFlg;

		public int stepPreviousGachaId;

		public int stepNextGachaId;

		public string stepupBtnText;

		public List<DataManagerGacha.GachaStaticTypeData> typeDataList = new List<DataManagerGacha.GachaStaticTypeData>();

		public List<DataManagerGacha.GachaItemdata> gachaItemData = new List<DataManagerGacha.GachaItemdata>();

		public List<int> enableBonusItemIdList = new List<int>();

		public List<int> bonusIdList = new List<int>();

		public bool[] dayOfWeek = new bool[7];

		public DataManagerGacha.TabCategory tabCategory;
	}

	public enum Category
	{
		INVALID,
		KiraKira,
		Active,
		SPECIAL,
		MonthlyPack,
		StepUp,
		Box,
		Roulette
	}

	public enum TabCategory
	{
		All,
		Limited,
		LimitedReprint
	}

	public class GachaStaticTypeData
	{
		public GachaStaticTypeData(MstGachaTypeData typeData)
		{
			this.gachaType = typeData.gachaType;
			this.balloonText = typeData.balloonText;
			this.lotTime = typeData.lotTime;
			this.useItemId = typeData.useItemId;
			this.useItemNumber = typeData.useItemNum;
			this.substituteItemId = typeData.subItemId;
			this.substituteItemNumber = typeData.subItemNum;
			this.subItemUseCondition = typeData.subItemUseCondition;
			this.bonusItemDispMessage = typeData.bonusItemDispMessage;
			this.bonusItemOneId = typeData.bonusItemId;
			this.bonusItemSetId = typeData.bonusItemPresetId;
			this.bonusItemNumber = typeData.bonusItemNum;
			this.bonusItemLimit = typeData.bonusItemLimit;
			this.lastTimeBenefitFriends = typeData.lastTimeBenefitFriends;
			this.lastTimeBenefitRarity = typeData.lastTimeBenefitRarity;
			this.lastTimeBenefitRarity4 = typeData.lastTimeBenefitRarity4;
		}

		public GachaStaticTypeData(int lot, int itemId, int itemNum)
		{
			this.lotTime = lot;
			this.useItemId = itemId;
			this.useItemNumber = itemNum;
		}

		public int gachaType;

		public DataManagerGacha.DiscountData discountData;

		public int lotTime;

		public string balloonText;

		public int useItemId;

		public int useItemNumber;

		public int substituteItemId;

		public int substituteItemNumber;

		public int subItemUseCondition;

		public string bonusItemDispMessage;

		public int bonusItemOneId;

		public int bonusItemSetId;

		public int bonusItemNumber;

		public int bonusItemLimit;

		public int lastTimeBenefitFriends;

		public int lastTimeBenefitRarity;

		public int lastTimeBenefitRarity4;
	}

	public class DiscountData
	{
		public int discountId;

		public string discountName;

		public DataManagerGacha.DiscountType discountType;

		public int discountNum;

		public DateTime startDatetime;

		public DateTime endDatetime;

		public int availableCount;
	}

	public enum DiscountType
	{
		Undefined,
		NoneReset,
		OnceADay
	}

	public class GachaItemdata
	{
		public bool EnableBonusItem01
		{
			get
			{
				return this.bonusItemId01 != 0 && 0 < this.bonusItemNum01;
			}
		}

		public bool EnableBonusItem02
		{
			get
			{
				return this.bonusItemId02 != 0 && 0 < this.bonusItemNum02;
			}
		}

		public bool EnableBonusItem03
		{
			get
			{
				return this.bonusItemId03 != 0 && 0 < this.bonusItemNum03;
			}
		}

		public int itemId;

		public int itemNum;

		public int stackNum;

		public int sortNum;

		public bool pickUpFlg;

		public int dispClientTypeLimit;

		public int dispClientTypeNew;

		public int bonusItemId01;

		public int bonusItemNum01;

		public int bonusItemId02;

		public int bonusItemNum02;

		public int bonusItemId03;

		public int bonusItemNum03;
	}

	public class ProbabilityData
	{
		public List<DataManagerGacha.ProbabilityData.Element> GetElements(DataManagerGacha.ProbabilityData.Type type)
		{
			return this.elements.FindAll((DataManagerGacha.ProbabilityData.Element e) => e.type == type);
		}

		public List<DataManagerGacha.ProbabilityData.Element> GetElements(DataManagerGacha.ProbabilityData.Category category)
		{
			return this.elements.FindAll((DataManagerGacha.ProbabilityData.Element e) => e.category == category);
		}

		public List<DataManagerGacha.ProbabilityData.Element> GetElements(DataManagerGacha.ProbabilityData.Type type, DataManagerGacha.ProbabilityData.Category category)
		{
			return this.elements.FindAll((DataManagerGacha.ProbabilityData.Element e) => e.type == type).FindAll((DataManagerGacha.ProbabilityData.Element e) => e.category == category);
		}

		public int gachaId;

		public List<DataManagerGacha.ProbabilityData.Element> elements;

		public class ItemOne
		{
			public ItemOne(double n, double d, double d3, double d4, double dc)
			{
				int num = 0;
				this.Initialize(num, n, d, d3, d4, dc);
			}

			public ItemOne(int r, double n, double d, double d3, double d4, double dc)
			{
				this.Initialize(r, n, d, d3, d4, dc);
			}

			private void Initialize(int r, double n, double d, double d3, double d4, double dc)
			{
				this.rarity = r;
				this.normal = n;
				this.decided = d;
				this.decided3 = d3;
				this.decided4 = d4;
				this.decidedCeiling = dc;
			}

			public int rarity;

			public double normal;

			public double decided;

			public double decided3;

			public double decided4;

			public double decidedCeiling;
		}

		public class Element
		{
			public Element(DataManagerGacha.ProbabilityData.Type t, DataManagerGacha.ProbabilityData.Category c)
			{
				this.type = t;
				this.category = c;
				this.item_id = 0;
				this.sortNum = 0;
			}

			public Element(DataManagerGacha.ProbabilityData.Type t, DataManagerGacha.ProbabilityData.Category c, DataManagerGacha.GachaItemdata item, int remineNum)
			{
				this.type = t;
				this.category = c;
				this.item_id = item.itemId;
				this.item_num = item.itemNum;
				this.sortNum = item.sortNum;
				this.numerator = remineNum;
				this.denominator = item.stackNum;
			}

			public DataManagerGacha.ProbabilityData.Type type;

			public DataManagerGacha.ProbabilityData.Category category;

			public List<DataManagerGacha.ProbabilityData.ItemOne> rate;

			public int item_id;

			public int item_num;

			public int sortNum;

			public int numerator;

			public int denominator;
		}

		public enum Type
		{
			Undefined,
			Chara,
			Photo,
			Item,
			TreeHouseFurniture
		}

		public enum Category
		{
			Rarity,
			PickUp,
			Other
		}
	}

	public class DynamicGachaData
	{
		public int gachaId;

		public List<DataManagerGacha.DynamicGachaTypeData> gachaTypeData;
	}

	public class DynamicGachaTypeData
	{
		public int gachaType;

		public int totalSubPlayNum;

		public int continuePlayNum;

		public int boxRemainNum;

		public int resetNum;

		public int discountPlayNum;

		public DateTime lastPlayDateTime;

		public DateTime lastPlayTodayDateTime;
	}

	public class DynamicGachaGroup
	{
		public int GachaGroupId { get; private set; }

		public DynamicGachaGroup(int id)
		{
			this.GachaGroupId = id;
		}

		public int ceilingCountNow;
	}

	public class PlayResult
	{
		public bool GetBonusOne { get; set; }

		public bool GetBonusSet { get; set; }

		public bool GetPrizeBonus { get; set; }

		public int highestRarity { get; set; }

		public int gachaId;

		public int gachaType;

		public List<DataManagerGacha.PlayResult.OneData> gachaResult = new List<DataManagerGacha.PlayResult.OneData>();

		public DataManagerGacha.PlayResult.BonusOneData bonusOne;

		public List<DataManagerGacha.PlayResult.BonusOneData> bonusSet = new List<DataManagerGacha.PlayResult.BonusOneData>();

		public List<DataManagerGacha.PlayResult.BonusOneData> prizeBonusList = new List<DataManagerGacha.PlayResult.BonusOneData>();

		public class OneData
		{
			public int charaId;

			public int itemId;

			public int itemNum;

			public bool isNew;

			public bool replaced;

			public ItemData replaceItem;

			public ItemData replaceItemEx;

			public bool isPresent;

			public bool replaceItemIsNew;

			public bool replaceItemExIsNew;
		}

		public class BonusOneData
		{
			public bool replaced;

			public bool isNew;

			public bool isPresentBox;

			public ItemData itemData;
		}
	}
}
