using System;
using System.Collections.Generic;
using System.Linq;
using CriWare;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x0200007A RID: 122
public class DataManagerGacha
{
	// Token: 0x0600046F RID: 1135 RVA: 0x0001E8D3 File Offset: 0x0001CAD3
	public DataManagerGacha(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000470 RID: 1136 RVA: 0x0001E8E2 File Offset: 0x0001CAE2
	// (set) Token: 0x06000471 RID: 1137 RVA: 0x0001E8EA File Offset: 0x0001CAEA
	public HashSet<int> SelectedGachaIdHashSet { get; set; }

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06000472 RID: 1138 RVA: 0x0001E8F3 File Offset: 0x0001CAF3
	// (set) Token: 0x06000473 RID: 1139 RVA: 0x0001E8FB File Offset: 0x0001CAFB
	public CriAtomExPlayback LatestGreetingVoice { get; set; }

	// Token: 0x06000474 RID: 1140 RVA: 0x0001E904 File Offset: 0x0001CB04
	private void AddToGachaResultRank4PuCharaList(int itemId)
	{
		if (DataManagerGacha.GachaResultRank4PuCharaList == null || DataManagerGacha.GachaResultRank4PuCharaList.Contains(itemId))
		{
			return;
		}
		DataManagerGacha.GachaResultRank4PuCharaList.Add(itemId);
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x0001E926 File Offset: 0x0001CB26
	public static bool IsExistResultPuChara()
	{
		return DataManagerGacha.GachaResultRank4PuCharaList != null && DataManagerGacha.GachaResultRank4PuCharaList.Any<int>();
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x0001E93C File Offset: 0x0001CB3C
	public static int GetGachaResultRank4PuRandomItemId()
	{
		if (DataManagerGacha.GachaResultRank4PuCharaList == null || DataManagerGacha.GachaResultRank4PuCharaList.Count == 0)
		{
			return -1;
		}
		return DataManagerGacha.GachaResultRank4PuCharaList.OrderBy<int, Guid>((int x) => Guid.NewGuid()).First<int>();
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x0001E98C File Offset: 0x0001CB8C
	public static void ReleasePuCharaList()
	{
		if (DataManagerGacha.GachaResultRank4PuCharaList == null)
		{
			return;
		}
		DataManagerGacha.GachaResultRank4PuCharaList.Clear();
		DataManagerGacha.GachaResultRank4PuCharaList = null;
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x0001E9A6 File Offset: 0x0001CBA6
	public List<DataManagerGacha.GachaPackData> CopyGachaPackDataList()
	{
		if (this.usableGachaPackDataList == null)
		{
			return null;
		}
		return new List<DataManagerGacha.GachaPackData>(this.usableGachaPackDataList);
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x0001E9BD File Offset: 0x0001CBBD
	public List<DataManagerGacha.GachaPackData> GetGachaPackDataList()
	{
		return this.usableGachaPackDataList;
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x0001E9C8 File Offset: 0x0001CBC8
	public DataManagerGacha.GachaStaticData GetGachaStaticData(int gachaId)
	{
		DataManagerGacha.GachaStaticData gachaStaticData = (this.gachaStaticDataMap.ContainsKey(gachaId) ? this.gachaStaticDataMap[gachaId] : null);
		if (gachaStaticData == null)
		{
			gachaStaticData = new DataManagerGacha.GachaStaticData(new MstGachaData());
		}
		return gachaStaticData;
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x0001EA02 File Offset: 0x0001CC02
	public DataManagerGacha.PlayResult GetLastPlayGachaResult()
	{
		return this.gachaPlayResult;
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x0001EA0A File Offset: 0x0001CC0A
	public DataManagerGacha.ProbabilityData GetLastRequestRateViewData()
	{
		return this.probabilityDataList.Find((DataManagerGacha.ProbabilityData x) => x.gachaId == this.lastRequestRateViewGachaId);
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x0001EA24 File Offset: 0x0001CC24
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

	// Token: 0x0600047E RID: 1150 RVA: 0x0001EA87 File Offset: 0x0001CC87
	public void SetLastPlayGachaResultByTutorial(DataManagerGacha.PlayResult gpr)
	{
		this.gachaPlayResult = gpr;
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x0001EA90 File Offset: 0x0001CC90
	public void RequestGetGachaList()
	{
		if (!Singleton<DataManager>.Instance.DisableServerRequestByTutorial)
		{
			this.gachaPackDataList = new List<DataManagerGacha.GachaPackData>();
		}
		this.parentData.ServerRequest(GachaCmd.Create(), new Action<Command>(this.CbGachaCmd));
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x0001EAC8 File Offset: 0x0001CCC8
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

	// Token: 0x06000481 RID: 1153 RVA: 0x0001EB68 File Offset: 0x0001CD68
	public void RequestActionRateView(int id)
	{
		this.lastRequestRateViewGachaId = id;
		if (this.probabilityDataList.Find((DataManagerGacha.ProbabilityData x) => x.gachaId == id) == null)
		{
			this.parentData.ServerRequest(GachaRateViewCmd.Create(id), new Action<Command>(this.CbGachaRateViewCmd));
		}
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x0001EBCB File Offset: 0x0001CDCB
	public void RequestActionGachaReset(int id)
	{
		this.parentData.ServerRequest(GachaResetCmd.Create(id), new Action<Command>(this.CbGachaResetCmd));
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x0001EBEC File Offset: 0x0001CDEC
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

	// Token: 0x06000484 RID: 1156 RVA: 0x0001F318 File Offset: 0x0001D518
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

	// Token: 0x06000485 RID: 1157 RVA: 0x0001FE64 File Offset: 0x0001E064
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

	// Token: 0x06000486 RID: 1158 RVA: 0x00020A80 File Offset: 0x0001EC80
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

	// Token: 0x06000487 RID: 1159 RVA: 0x00020AFC File Offset: 0x0001ECFC
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

	// Token: 0x06000488 RID: 1160 RVA: 0x000211E0 File Offset: 0x0001F3E0
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

	// Token: 0x06000489 RID: 1161 RVA: 0x00021480 File Offset: 0x0001F680
	public void SetGachaPackDataByDirect(DataManagerGacha.GachaPackData _gachaPackData)
	{
		this.gachaPackDataList = new List<DataManagerGacha.GachaPackData> { _gachaPackData };
		this.usableGachaPackDataList = this.gachaPackDataList;
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x000214A0 File Offset: 0x0001F6A0
	private List<DataManagerGacha.GachaItemdata> PickCharaList(int search_gacha_id)
	{
		List<DataManagerGacha.GachaItemdata> list = new List<DataManagerGacha.GachaItemdata>();
		if (this.gachaStaticDataMap.ContainsKey(search_gacha_id))
		{
			list = this.gachaStaticDataMap[search_gacha_id].gachaItemData.Where<DataManagerGacha.GachaItemdata>((DataManagerGacha.GachaItemdata item) => item.pickUpFlg).ToList<DataManagerGacha.GachaItemdata>();
		}
		return list;
	}

	// Token: 0x04000507 RID: 1287
	private DataManager parentData;

	// Token: 0x04000508 RID: 1288
	private Dictionary<int, DataManagerGacha.GachaStaticData> gachaStaticDataMap;

	// Token: 0x04000509 RID: 1289
	private List<DataManagerGacha.GachaPackData> gachaPackDataList;

	// Token: 0x0400050A RID: 1290
	private List<DataManagerGacha.GachaPackData> usableGachaPackDataList;

	// Token: 0x0400050B RID: 1291
	private List<DataManagerGacha.DynamicGachaGroup> gachaDynamicGachaGroupList;

	// Token: 0x0400050C RID: 1292
	private DataManagerGacha.PlayResult gachaPlayResult;

	// Token: 0x0400050D RID: 1293
	private List<DataManagerGacha.ProbabilityData> probabilityDataList;

	// Token: 0x0400050E RID: 1294
	private int lastRequestRateViewGachaId;

	// Token: 0x0400050F RID: 1295
	private List<DataManagerGacha.GachaItemdata> puResultList;

	// Token: 0x04000512 RID: 1298
	private static List<int> GachaResultRank4PuCharaList;

	// Token: 0x02000684 RID: 1668
	public class GachaPackData
	{
		// Token: 0x0600321C RID: 12828 RVA: 0x001BE8E4 File Offset: 0x001BCAE4
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

		// Token: 0x04002F69 RID: 12137
		public int gachaId;

		// Token: 0x04002F6A RID: 12138
		public DataManagerGacha.GachaStaticData staticData = new DataManagerGacha.GachaStaticData(new MstGachaData());

		// Token: 0x04002F6B RID: 12139
		public DataManagerGacha.DynamicGachaData dynamicData = new DataManagerGacha.DynamicGachaData();
	}

	// Token: 0x02000685 RID: 1669
	public class GachaStaticData
	{
		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x0600321E RID: 12830 RVA: 0x001BE977 File Offset: 0x001BCB77
		// (set) Token: 0x0600321F RID: 12831 RVA: 0x001BE97F File Offset: 0x001BCB7F
		public DateTime StepResetTime { get; private set; }

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06003220 RID: 12832 RVA: 0x001BE988 File Offset: 0x001BCB88
		// (set) Token: 0x06003221 RID: 12833 RVA: 0x001BE990 File Offset: 0x001BCB90
		public bool RateHideFlg { get; private set; }

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06003222 RID: 12834 RVA: 0x001BE999 File Offset: 0x001BCB99
		// (set) Token: 0x06003223 RID: 12835 RVA: 0x001BE9A1 File Offset: 0x001BCBA1
		public List<int> InfoDispIdList { get; set; }

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06003224 RID: 12836 RVA: 0x001BE9AA File Offset: 0x001BCBAA
		// (set) Token: 0x06003225 RID: 12837 RVA: 0x001BE9B2 File Offset: 0x001BCBB2
		public DateTime startDatetime { get; set; }

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06003226 RID: 12838 RVA: 0x001BE9BB File Offset: 0x001BCBBB
		// (set) Token: 0x06003227 RID: 12839 RVA: 0x001BE9C3 File Offset: 0x001BCBC3
		public DateTime endDatetime { get; set; }

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06003228 RID: 12840 RVA: 0x001BE9CC File Offset: 0x001BCBCC
		// (set) Token: 0x06003229 RID: 12841 RVA: 0x001BE9D4 File Offset: 0x001BCBD4
		public bool dayOfWeekFlg { get; set; }

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x0600322A RID: 12842 RVA: 0x001BE9DD File Offset: 0x001BCBDD
		// (set) Token: 0x0600322B RID: 12843 RVA: 0x001BE9E5 File Offset: 0x001BCBE5
		public bool IsResultTreeHouseFurnitureInfo { get; private set; }

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x0600322C RID: 12844 RVA: 0x001BE9EE File Offset: 0x001BCBEE
		// (set) Token: 0x0600322D RID: 12845 RVA: 0x001BE9F6 File Offset: 0x001BCBF6
		public int ReplaceGroupId { get; private set; }

		// Token: 0x0600322E RID: 12846 RVA: 0x001BE9FF File Offset: 0x001BCBFF
		public GachaStaticData()
		{
		}

		// Token: 0x0600322F RID: 12847 RVA: 0x001BEA40 File Offset: 0x001BCC40
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

		// Token: 0x06003230 RID: 12848 RVA: 0x001BECBC File Offset: 0x001BCEBC
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

		// Token: 0x04002F6C RID: 12140
		public int gachaId;

		// Token: 0x04002F6D RID: 12141
		public DataManagerGacha.Category gachaCategory;

		// Token: 0x04002F6E RID: 12142
		public string gachaName;

		// Token: 0x04002F6F RID: 12143
		public int gachaGroupId;

		// Token: 0x04002F70 RID: 12144
		public string labelTextureName;

		// Token: 0x04002F71 RID: 12145
		public string banner;

		// Token: 0x04002F72 RID: 12146
		public int sortIndex;

		// Token: 0x04002F73 RID: 12147
		public int highLimit;

		// Token: 0x04002F74 RID: 12148
		public bool highLimitCountFlag;

		// Token: 0x04002F75 RID: 12149
		public string detailDispText;

		// Token: 0x04002F76 RID: 12150
		public int availableCount;

		// Token: 0x04002F77 RID: 12151
		public bool recommendFlg;

		// Token: 0x04002F78 RID: 12152
		public int stepPreviousGachaId;

		// Token: 0x04002F79 RID: 12153
		public int stepNextGachaId;

		// Token: 0x04002F7B RID: 12155
		public string stepupBtnText;

		// Token: 0x04002F7D RID: 12157
		public List<DataManagerGacha.GachaStaticTypeData> typeDataList = new List<DataManagerGacha.GachaStaticTypeData>();

		// Token: 0x04002F7E RID: 12158
		public List<DataManagerGacha.GachaItemdata> gachaItemData = new List<DataManagerGacha.GachaItemdata>();

		// Token: 0x04002F80 RID: 12160
		public List<int> enableBonusItemIdList = new List<int>();

		// Token: 0x04002F81 RID: 12161
		public List<int> bonusIdList = new List<int>();

		// Token: 0x04002F85 RID: 12165
		public bool[] dayOfWeek = new bool[7];

		// Token: 0x04002F86 RID: 12166
		public DataManagerGacha.TabCategory tabCategory;
	}

	// Token: 0x02000686 RID: 1670
	public enum Category
	{
		// Token: 0x04002F8A RID: 12170
		INVALID,
		// Token: 0x04002F8B RID: 12171
		KiraKira,
		// Token: 0x04002F8C RID: 12172
		Active,
		// Token: 0x04002F8D RID: 12173
		SPECIAL,
		// Token: 0x04002F8E RID: 12174
		MonthlyPack,
		// Token: 0x04002F8F RID: 12175
		StepUp,
		// Token: 0x04002F90 RID: 12176
		Box,
		// Token: 0x04002F91 RID: 12177
		Roulette
	}

	// Token: 0x02000687 RID: 1671
	public enum TabCategory
	{
		// Token: 0x04002F93 RID: 12179
		All,
		// Token: 0x04002F94 RID: 12180
		Limited,
		// Token: 0x04002F95 RID: 12181
		LimitedReprint
	}

	// Token: 0x02000688 RID: 1672
	public class GachaStaticTypeData
	{
		// Token: 0x06003231 RID: 12849 RVA: 0x001BED5C File Offset: 0x001BCF5C
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

		// Token: 0x06003232 RID: 12850 RVA: 0x001BEE2F File Offset: 0x001BD02F
		public GachaStaticTypeData(int lot, int itemId, int itemNum)
		{
			this.lotTime = lot;
			this.useItemId = itemId;
			this.useItemNumber = itemNum;
		}

		// Token: 0x04002F96 RID: 12182
		public int gachaType;

		// Token: 0x04002F97 RID: 12183
		public DataManagerGacha.DiscountData discountData;

		// Token: 0x04002F98 RID: 12184
		public int lotTime;

		// Token: 0x04002F99 RID: 12185
		public string balloonText;

		// Token: 0x04002F9A RID: 12186
		public int useItemId;

		// Token: 0x04002F9B RID: 12187
		public int useItemNumber;

		// Token: 0x04002F9C RID: 12188
		public int substituteItemId;

		// Token: 0x04002F9D RID: 12189
		public int substituteItemNumber;

		// Token: 0x04002F9E RID: 12190
		public int subItemUseCondition;

		// Token: 0x04002F9F RID: 12191
		public string bonusItemDispMessage;

		// Token: 0x04002FA0 RID: 12192
		public int bonusItemOneId;

		// Token: 0x04002FA1 RID: 12193
		public int bonusItemSetId;

		// Token: 0x04002FA2 RID: 12194
		public int bonusItemNumber;

		// Token: 0x04002FA3 RID: 12195
		public int bonusItemLimit;

		// Token: 0x04002FA4 RID: 12196
		public int lastTimeBenefitFriends;

		// Token: 0x04002FA5 RID: 12197
		public int lastTimeBenefitRarity;

		// Token: 0x04002FA6 RID: 12198
		public int lastTimeBenefitRarity4;
	}

	// Token: 0x02000689 RID: 1673
	public class DiscountData
	{
		// Token: 0x04002FA7 RID: 12199
		public int discountId;

		// Token: 0x04002FA8 RID: 12200
		public string discountName;

		// Token: 0x04002FA9 RID: 12201
		public DataManagerGacha.DiscountType discountType;

		// Token: 0x04002FAA RID: 12202
		public int discountNum;

		// Token: 0x04002FAB RID: 12203
		public DateTime startDatetime;

		// Token: 0x04002FAC RID: 12204
		public DateTime endDatetime;

		// Token: 0x04002FAD RID: 12205
		public int availableCount;
	}

	// Token: 0x0200068A RID: 1674
	public enum DiscountType
	{
		// Token: 0x04002FAF RID: 12207
		Undefined,
		// Token: 0x04002FB0 RID: 12208
		NoneReset,
		// Token: 0x04002FB1 RID: 12209
		OnceADay
	}

	// Token: 0x0200068B RID: 1675
	public class GachaItemdata
	{
		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06003234 RID: 12852 RVA: 0x001BEE54 File Offset: 0x001BD054
		public bool EnableBonusItem01
		{
			get
			{
				return this.bonusItemId01 != 0 && 0 < this.bonusItemNum01;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06003235 RID: 12853 RVA: 0x001BEE69 File Offset: 0x001BD069
		public bool EnableBonusItem02
		{
			get
			{
				return this.bonusItemId02 != 0 && 0 < this.bonusItemNum02;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06003236 RID: 12854 RVA: 0x001BEE7E File Offset: 0x001BD07E
		public bool EnableBonusItem03
		{
			get
			{
				return this.bonusItemId03 != 0 && 0 < this.bonusItemNum03;
			}
		}

		// Token: 0x04002FB2 RID: 12210
		public int itemId;

		// Token: 0x04002FB3 RID: 12211
		public int itemNum;

		// Token: 0x04002FB4 RID: 12212
		public int stackNum;

		// Token: 0x04002FB5 RID: 12213
		public int sortNum;

		// Token: 0x04002FB6 RID: 12214
		public bool pickUpFlg;

		// Token: 0x04002FB7 RID: 12215
		public int dispClientTypeLimit;

		// Token: 0x04002FB8 RID: 12216
		public int dispClientTypeNew;

		// Token: 0x04002FB9 RID: 12217
		public int bonusItemId01;

		// Token: 0x04002FBA RID: 12218
		public int bonusItemNum01;

		// Token: 0x04002FBB RID: 12219
		public int bonusItemId02;

		// Token: 0x04002FBC RID: 12220
		public int bonusItemNum02;

		// Token: 0x04002FBD RID: 12221
		public int bonusItemId03;

		// Token: 0x04002FBE RID: 12222
		public int bonusItemNum03;
	}

	// Token: 0x0200068C RID: 1676
	public class ProbabilityData
	{
		// Token: 0x06003238 RID: 12856 RVA: 0x001BEE9C File Offset: 0x001BD09C
		public List<DataManagerGacha.ProbabilityData.Element> GetElements(DataManagerGacha.ProbabilityData.Type type)
		{
			return this.elements.FindAll((DataManagerGacha.ProbabilityData.Element e) => e.type == type);
		}

		// Token: 0x06003239 RID: 12857 RVA: 0x001BEED0 File Offset: 0x001BD0D0
		public List<DataManagerGacha.ProbabilityData.Element> GetElements(DataManagerGacha.ProbabilityData.Category category)
		{
			return this.elements.FindAll((DataManagerGacha.ProbabilityData.Element e) => e.category == category);
		}

		// Token: 0x0600323A RID: 12858 RVA: 0x001BEF04 File Offset: 0x001BD104
		public List<DataManagerGacha.ProbabilityData.Element> GetElements(DataManagerGacha.ProbabilityData.Type type, DataManagerGacha.ProbabilityData.Category category)
		{
			return this.elements.FindAll((DataManagerGacha.ProbabilityData.Element e) => e.type == type).FindAll((DataManagerGacha.ProbabilityData.Element e) => e.category == category);
		}

		// Token: 0x04002FBF RID: 12223
		public int gachaId;

		// Token: 0x04002FC0 RID: 12224
		public List<DataManagerGacha.ProbabilityData.Element> elements;

		// Token: 0x02001121 RID: 4385
		public class ItemOne
		{
			// Token: 0x060054C9 RID: 21705 RVA: 0x0024E3DC File Offset: 0x0024C5DC
			public ItemOne(double n, double d, double d3, double d4, double dc)
			{
				int num = 0;
				this.Initialize(num, n, d, d3, d4, dc);
			}

			// Token: 0x060054CA RID: 21706 RVA: 0x0024E3FF File Offset: 0x0024C5FF
			public ItemOne(int r, double n, double d, double d3, double d4, double dc)
			{
				this.Initialize(r, n, d, d3, d4, dc);
			}

			// Token: 0x060054CB RID: 21707 RVA: 0x0024E416 File Offset: 0x0024C616
			private void Initialize(int r, double n, double d, double d3, double d4, double dc)
			{
				this.rarity = r;
				this.normal = n;
				this.decided = d;
				this.decided3 = d3;
				this.decided4 = d4;
				this.decidedCeiling = dc;
			}

			// Token: 0x04005E2E RID: 24110
			public int rarity;

			// Token: 0x04005E2F RID: 24111
			public double normal;

			// Token: 0x04005E30 RID: 24112
			public double decided;

			// Token: 0x04005E31 RID: 24113
			public double decided3;

			// Token: 0x04005E32 RID: 24114
			public double decided4;

			// Token: 0x04005E33 RID: 24115
			public double decidedCeiling;
		}

		// Token: 0x02001122 RID: 4386
		public class Element
		{
			// Token: 0x060054CC RID: 21708 RVA: 0x0024E445 File Offset: 0x0024C645
			public Element(DataManagerGacha.ProbabilityData.Type t, DataManagerGacha.ProbabilityData.Category c)
			{
				this.type = t;
				this.category = c;
				this.item_id = 0;
				this.sortNum = 0;
			}

			// Token: 0x060054CD RID: 21709 RVA: 0x0024E46C File Offset: 0x0024C66C
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

			// Token: 0x04005E34 RID: 24116
			public DataManagerGacha.ProbabilityData.Type type;

			// Token: 0x04005E35 RID: 24117
			public DataManagerGacha.ProbabilityData.Category category;

			// Token: 0x04005E36 RID: 24118
			public List<DataManagerGacha.ProbabilityData.ItemOne> rate;

			// Token: 0x04005E37 RID: 24119
			public int item_id;

			// Token: 0x04005E38 RID: 24120
			public int item_num;

			// Token: 0x04005E39 RID: 24121
			public int sortNum;

			// Token: 0x04005E3A RID: 24122
			public int numerator;

			// Token: 0x04005E3B RID: 24123
			public int denominator;
		}

		// Token: 0x02001123 RID: 4387
		public enum Type
		{
			// Token: 0x04005E3D RID: 24125
			Undefined,
			// Token: 0x04005E3E RID: 24126
			Chara,
			// Token: 0x04005E3F RID: 24127
			Photo,
			// Token: 0x04005E40 RID: 24128
			Item,
			// Token: 0x04005E41 RID: 24129
			TreeHouseFurniture
		}

		// Token: 0x02001124 RID: 4388
		public enum Category
		{
			// Token: 0x04005E43 RID: 24131
			Rarity,
			// Token: 0x04005E44 RID: 24132
			PickUp,
			// Token: 0x04005E45 RID: 24133
			Other
		}
	}

	// Token: 0x0200068D RID: 1677
	public class DynamicGachaData
	{
		// Token: 0x04002FC1 RID: 12225
		public int gachaId;

		// Token: 0x04002FC2 RID: 12226
		public List<DataManagerGacha.DynamicGachaTypeData> gachaTypeData;
	}

	// Token: 0x0200068E RID: 1678
	public class DynamicGachaTypeData
	{
		// Token: 0x04002FC3 RID: 12227
		public int gachaType;

		// Token: 0x04002FC4 RID: 12228
		public int totalSubPlayNum;

		// Token: 0x04002FC5 RID: 12229
		public int continuePlayNum;

		// Token: 0x04002FC6 RID: 12230
		public int boxRemainNum;

		// Token: 0x04002FC7 RID: 12231
		public int resetNum;

		// Token: 0x04002FC8 RID: 12232
		public int discountPlayNum;

		// Token: 0x04002FC9 RID: 12233
		public DateTime lastPlayDateTime;

		// Token: 0x04002FCA RID: 12234
		public DateTime lastPlayTodayDateTime;
	}

	// Token: 0x0200068F RID: 1679
	public class DynamicGachaGroup
	{
		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x0600323E RID: 12862 RVA: 0x001BEF65 File Offset: 0x001BD165
		// (set) Token: 0x0600323F RID: 12863 RVA: 0x001BEF6D File Offset: 0x001BD16D
		public int GachaGroupId { get; private set; }

		// Token: 0x06003240 RID: 12864 RVA: 0x001BEF76 File Offset: 0x001BD176
		public DynamicGachaGroup(int id)
		{
			this.GachaGroupId = id;
		}

		// Token: 0x04002FCC RID: 12236
		public int ceilingCountNow;
	}

	// Token: 0x02000690 RID: 1680
	public class PlayResult
	{
		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06003241 RID: 12865 RVA: 0x001BEF85 File Offset: 0x001BD185
		// (set) Token: 0x06003242 RID: 12866 RVA: 0x001BEF8D File Offset: 0x001BD18D
		public bool GetBonusOne { get; set; }

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06003243 RID: 12867 RVA: 0x001BEF96 File Offset: 0x001BD196
		// (set) Token: 0x06003244 RID: 12868 RVA: 0x001BEF9E File Offset: 0x001BD19E
		public bool GetBonusSet { get; set; }

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06003245 RID: 12869 RVA: 0x001BEFA7 File Offset: 0x001BD1A7
		// (set) Token: 0x06003246 RID: 12870 RVA: 0x001BEFAF File Offset: 0x001BD1AF
		public bool GetPrizeBonus { get; set; }

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06003247 RID: 12871 RVA: 0x001BEFB8 File Offset: 0x001BD1B8
		// (set) Token: 0x06003248 RID: 12872 RVA: 0x001BEFC0 File Offset: 0x001BD1C0
		public int highestRarity { get; set; }

		// Token: 0x04002FCD RID: 12237
		public int gachaId;

		// Token: 0x04002FCE RID: 12238
		public int gachaType;

		// Token: 0x04002FCF RID: 12239
		public List<DataManagerGacha.PlayResult.OneData> gachaResult = new List<DataManagerGacha.PlayResult.OneData>();

		// Token: 0x04002FD4 RID: 12244
		public DataManagerGacha.PlayResult.BonusOneData bonusOne;

		// Token: 0x04002FD5 RID: 12245
		public List<DataManagerGacha.PlayResult.BonusOneData> bonusSet = new List<DataManagerGacha.PlayResult.BonusOneData>();

		// Token: 0x04002FD6 RID: 12246
		public List<DataManagerGacha.PlayResult.BonusOneData> prizeBonusList = new List<DataManagerGacha.PlayResult.BonusOneData>();

		// Token: 0x02001128 RID: 4392
		public class OneData
		{
			// Token: 0x04005E4A RID: 24138
			public int charaId;

			// Token: 0x04005E4B RID: 24139
			public int itemId;

			// Token: 0x04005E4C RID: 24140
			public int itemNum;

			// Token: 0x04005E4D RID: 24141
			public bool isNew;

			// Token: 0x04005E4E RID: 24142
			public bool replaced;

			// Token: 0x04005E4F RID: 24143
			public ItemData replaceItem;

			// Token: 0x04005E50 RID: 24144
			public ItemData replaceItemEx;

			// Token: 0x04005E51 RID: 24145
			public bool isPresent;

			// Token: 0x04005E52 RID: 24146
			public bool replaceItemIsNew;

			// Token: 0x04005E53 RID: 24147
			public bool replaceItemExIsNew;
		}

		// Token: 0x02001129 RID: 4393
		public class BonusOneData
		{
			// Token: 0x04005E54 RID: 24148
			public bool replaced;

			// Token: 0x04005E55 RID: 24149
			public bool isNew;

			// Token: 0x04005E56 RID: 24150
			public bool isPresentBox;

			// Token: 0x04005E57 RID: 24151
			public ItemData itemData;
		}
	}
}
