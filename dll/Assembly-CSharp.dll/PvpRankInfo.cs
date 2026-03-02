using System;
using System.Collections.Generic;

// Token: 0x0200009D RID: 157
public class PvpRankInfo
{
	// Token: 0x0400061D RID: 1565
	public int id;

	// Token: 0x0400061E RID: 1566
	public int pointRangeLow;

	// Token: 0x0400061F RID: 1567
	public string rankName;

	// Token: 0x04000620 RID: 1568
	public string rankIcon;

	// Token: 0x04000621 RID: 1569
	public PvpRankInfo nexRankInfo;

	// Token: 0x04000622 RID: 1570
	public List<ItemData> rewardItemList = new List<ItemData>();
}
