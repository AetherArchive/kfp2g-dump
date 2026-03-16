using System;
using System.Collections.Generic;
using System.Linq;

public class CharaGrowItemInfo
{
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

	private static readonly List<ItemDef.Kind> sortKind = new List<ItemDef.Kind>
	{
		ItemDef.Kind.PROMOTE,
		ItemDef.Kind.PROMOTE_EXT,
		ItemDef.Kind.RANK_UP,
		ItemDef.Kind.ARTS_UP,
		ItemDef.Kind.PHOTO_FRAME_UP,
		ItemDef.Kind.ABILITY_RELEASE
	};

	public class NeedInfo
	{
		public ItemDef.Kind needType;

		public int num;

		public int charaId;

		public CharaPromoteOne charaPromoteOne;

		public int promoteIndex;
	}
}
