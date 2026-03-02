using System;
using System.Collections.Generic;

// Token: 0x020000D7 RID: 215
public class QuestDynamicData
{
	// Token: 0x060009B3 RID: 2483 RVA: 0x0003B19B File Offset: 0x0003939B
	public bool IsClear(int oneId)
	{
		return this.oneDataMap.ContainsKey(oneId) && this.oneDataMap[oneId].clearNum > 0;
	}

	// Token: 0x040007D0 RID: 2000
	public Dictionary<int, QuestDynamicQuestOne> oneDataMap = new Dictionary<int, QuestDynamicQuestOne>();

	// Token: 0x040007D1 RID: 2001
	public List<QuestDynamicQuestOne> oneDataList = new List<QuestDynamicQuestOne>();
}
