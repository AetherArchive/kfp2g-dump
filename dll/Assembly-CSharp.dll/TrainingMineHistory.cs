using System;
using System.Collections.Generic;
using Battle;

// Token: 0x020000AC RID: 172
public class TrainingMineHistory
{
	// Token: 0x04000697 RID: 1687
	public int seasonId;

	// Token: 0x04000698 RID: 1688
	public Dictionary<DayOfWeek, TrainingMineHistory.DayOfWeekHistory> dayOfWeekDataList = new Dictionary<DayOfWeek, TrainingMineHistory.DayOfWeekHistory>();

	// Token: 0x02000799 RID: 1945
	public class DayOfWeekHistory
	{
		// Token: 0x040033BA RID: 13242
		public DayOfWeek dayOfWeek;

		// Token: 0x040033BB RID: 13243
		public long point;

		// Token: 0x040033BC RID: 13244
		public DateTime updateTime;

		// Token: 0x040033BD RID: 13245
		public SceneBattle_DeckInfo deckInfo;
	}
}
