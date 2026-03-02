using System;

// Token: 0x020000C2 RID: 194
public class QuestUIMapInfo
{
	// Token: 0x060008CA RID: 2250 RVA: 0x00038230 File Offset: 0x00036430
	public static QuestUIMapInfo GetQuestUIMapInfo(int mapId, DateTime now, int eventId)
	{
		QuestUIMapInfo questUIMapInfo = new QuestUIMapInfo
		{
			mapId = mapId
		};
		QuestStaticMap questStaticMap = DataManager.DmQuest.QuestStaticData.mapDataMap.TryGetValueEx(mapId, null);
		if (questStaticMap.largeEventUIData.openItemInfoOneData != null)
		{
			questUIMapInfo.isLockByItem = true;
			questUIMapInfo.openItemData = new ItemData(questStaticMap.largeEventUIData.openItemInfoOneData.OpenKeyItem.itemId, questStaticMap.largeEventUIData.openItemInfoOneData.OpenKeyItem.num);
			QuestUIMapInfo questUIMapInfo2 = questUIMapInfo;
			DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(eventId);
			int? num = ((eventData != null) ? new int?(eventData.openKeyPaidItemID) : null);
			int id = questUIMapInfo.openItemData.id;
			questUIMapInfo2.isPaidOpenItem = (num.GetValueOrDefault() == id) & (num != null);
			QuestDynamicQuestOne questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataMap.TryGetValueEx(questStaticMap.largeEventUIData.openItemInfoOneData.questId, null);
			if (questDynamicQuestOne != null)
			{
				questUIMapInfo.isLockByItem = !questDynamicQuestOne.openFlagByItem;
			}
		}
		if (questStaticMap.largeEventUIData.pickupRewardOneData != null)
		{
			questUIMapInfo.pickupRewardItem = new ItemData(questStaticMap.largeEventUIData.pickupRewardOneData.RewardItemList[0].itemId, questStaticMap.largeEventUIData.pickupRewardOneData.RewardItemList[0].num);
			QuestDynamicQuestOne questDynamicQuestOne2 = DataManager.DmQuest.QuestDynamicData.oneDataMap.TryGetValueEx(questStaticMap.largeEventUIData.pickupRewardOneData.questId, null);
			if (questDynamicQuestOne2 != null)
			{
				questUIMapInfo.isGetPickupRewardItem = questDynamicQuestOne2.clearNum > 0;
			}
		}
		if (questStaticMap.StartDateTime <= now)
		{
			questUIMapInfo.isLockByTime = false;
		}
		else
		{
			questUIMapInfo.isLockByTime = true;
			questUIMapInfo.openTime = questStaticMap.StartDateTime;
		}
		return questUIMapInfo;
	}

	// Token: 0x04000748 RID: 1864
	public int mapId;

	// Token: 0x04000749 RID: 1865
	public bool isLockByTime;

	// Token: 0x0400074A RID: 1866
	public DateTime openTime;

	// Token: 0x0400074B RID: 1867
	public bool isLockByItem;

	// Token: 0x0400074C RID: 1868
	public ItemData openItemData;

	// Token: 0x0400074D RID: 1869
	public bool isPaidOpenItem;

	// Token: 0x0400074E RID: 1870
	public ItemData pickupRewardItem;

	// Token: 0x0400074F RID: 1871
	public bool isGetPickupRewardItem;
}
