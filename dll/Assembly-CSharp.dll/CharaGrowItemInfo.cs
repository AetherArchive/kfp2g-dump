using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x020000C3 RID: 195
public class CharaGrowItemInfo
{
	// Token: 0x060008CC RID: 2252 RVA: 0x000383F0 File Offset: 0x000365F0
	public static List<CharaGrowItemInfo.NeedInfo> MakeCharaGrowNeedInfoList(int itemId)
	{
		List<CharaGrowItemInfo.NeedInfo> list = new List<CharaGrowItemInfo.NeedInfo>();
		Func<ItemData, bool> <>9__3;
		foreach (CharaPackData charaPackData in DataManager.DmChara.GetUserCharaMap().Values)
		{
			if (charaPackData.dynamicData.promoteNum < charaPackData.staticData.maxPromoteNum)
			{
				for (int i = 0; i < charaPackData.dynamicData.promoteFlag.Count; i++)
				{
					CharaPromotePreset charaPromotePreset = charaPackData.staticData.promoteList[charaPackData.dynamicData.promoteNum];
					if (!charaPackData.dynamicData.promoteFlag[i] && charaPromotePreset.promoteOneList[i].promoteUseItemId == itemId)
					{
						list.Add(new CharaGrowItemInfo.NeedInfo
						{
							charaId = charaPackData.id,
							num = charaPromotePreset.promoteOneList[i].promoteUseItemNum,
							needType = ItemDef.Kind.PROMOTE,
							charaPromoteOne = charaPromotePreset.promoteOneList[i],
							promoteIndex = i
						});
					}
				}
			}
			GrowItemList nextItemByArtsUp = charaPackData.GetNextItemByArtsUp(0);
			if (nextItemByArtsUp != null)
			{
				foreach (ItemData itemData in nextItemByArtsUp.itemList)
				{
					if (itemData.id == itemId)
					{
						list.Add(new CharaGrowItemInfo.NeedInfo
						{
							charaId = charaPackData.id,
							num = itemData.num,
							needType = ItemDef.Kind.ARTS_UP
						});
					}
				}
			}
			GrowItemData nextItemByRankup = charaPackData.GetNextItemByRankup(0);
			if (nextItemByRankup != null && nextItemByRankup.item.id == itemId)
			{
				list.Add(new CharaGrowItemInfo.NeedInfo
				{
					charaId = charaPackData.id,
					num = nextItemByRankup.item.num,
					needType = ItemDef.Kind.RANK_UP
				});
			}
			GrowItemData nextItemByReleasePhotoFrame = charaPackData.GetNextItemByReleasePhotoFrame();
			if (nextItemByReleasePhotoFrame != null && nextItemByReleasePhotoFrame.item.id == itemId)
			{
				list.Add(new CharaGrowItemInfo.NeedInfo
				{
					charaId = charaPackData.id,
					num = nextItemByReleasePhotoFrame.item.num,
					needType = ItemDef.Kind.PHOTO_FRAME_UP
				});
			}
			GrowItemList releaseItemByNanairoAbilityRelease = charaPackData.GetReleaseItemByNanairoAbilityRelease();
			if (releaseItemByNanairoAbilityRelease != null && charaPackData.IsNanairoAbilityReleaseAvailable && charaPackData.IsHaveNanairoAbility)
			{
				IEnumerable<ItemData> itemList = releaseItemByNanairoAbilityRelease.itemList;
				Func<ItemData, bool> func;
				if ((func = <>9__3) == null)
				{
					func = (<>9__3 = (ItemData x) => x.id == itemId);
				}
				foreach (ItemData itemData2 in itemList.Where<ItemData>(func))
				{
					list.Add(new CharaGrowItemInfo.NeedInfo
					{
						charaId = charaPackData.id,
						num = itemData2.num,
						needType = ItemDef.Kind.ABILITY_RELEASE
					});
				}
			}
		}
		PrjUtil.InsertionSort<CharaGrowItemInfo.NeedInfo>(ref list, (CharaGrowItemInfo.NeedInfo a, CharaGrowItemInfo.NeedInfo b) => a.promoteIndex.CompareTo(b.promoteIndex));
		PrjUtil.InsertionSort<CharaGrowItemInfo.NeedInfo>(ref list, (CharaGrowItemInfo.NeedInfo a, CharaGrowItemInfo.NeedInfo b) => CharaGrowItemInfo.sortKind.IndexOf(a.needType).CompareTo(CharaGrowItemInfo.sortKind.IndexOf(b.needType)));
		PrjUtil.InsertionSort<CharaGrowItemInfo.NeedInfo>(ref list, (CharaGrowItemInfo.NeedInfo a, CharaGrowItemInfo.NeedInfo b) => a.charaId.CompareTo(b.charaId));
		return list;
	}

	// Token: 0x04000750 RID: 1872
	private static readonly List<ItemDef.Kind> sortKind = new List<ItemDef.Kind>
	{
		ItemDef.Kind.PROMOTE,
		ItemDef.Kind.PROMOTE_EXT,
		ItemDef.Kind.RANK_UP,
		ItemDef.Kind.ARTS_UP,
		ItemDef.Kind.PHOTO_FRAME_UP,
		ItemDef.Kind.ABILITY_RELEASE
	};

	// Token: 0x020007BD RID: 1981
	public class NeedInfo
	{
		// Token: 0x04003483 RID: 13443
		public ItemDef.Kind needType;

		// Token: 0x04003484 RID: 13444
		public int num;

		// Token: 0x04003485 RID: 13445
		public int charaId;

		// Token: 0x04003486 RID: 13446
		public CharaPromoteOne charaPromoteOne;

		// Token: 0x04003487 RID: 13447
		public int promoteIndex;
	}
}
