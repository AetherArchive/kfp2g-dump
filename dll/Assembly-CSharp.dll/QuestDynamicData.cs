using System;
using System.Collections.Generic;

public class QuestDynamicData
{
	public bool IsClear(int oneId)
	{
		return this.oneDataMap.ContainsKey(oneId) && this.oneDataMap[oneId].clearNum > 0;
	}

	public Dictionary<int, QuestDynamicQuestOne> oneDataMap = new Dictionary<int, QuestDynamicQuestOne>();

	public List<QuestDynamicQuestOne> oneDataList = new List<QuestDynamicQuestOne>();
}
