using System;

public class QuestUIMapInfo
{
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

	public int mapId;

	public bool isLockByTime;

	public DateTime openTime;

	public bool isLockByItem;

	public ItemData openItemData;

	public bool isPaidOpenItem;

	public ItemData pickupRewardItem;

	public bool isGetPickupRewardItem;
}
