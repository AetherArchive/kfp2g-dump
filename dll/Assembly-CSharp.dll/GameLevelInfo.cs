using System;
using System.Collections.Generic;
using SGNFW.Mst;

// Token: 0x020000A4 RID: 164
public class GameLevelInfo
{
	// Token: 0x0400065F RID: 1631
	public int level;

	// Token: 0x04000660 RID: 1632
	public long userLevelExp;

	// Token: 0x04000661 RID: 1633
	public int staminaLimit;

	// Token: 0x04000662 RID: 1634
	public Dictionary<int, GameLevelInfo.KizunaLevelData> kizunaLevelExp = new Dictionary<int, GameLevelInfo.KizunaLevelData>();

	// Token: 0x04000663 RID: 1635
	public Dictionary<int, long> charaLevelExp = new Dictionary<int, long>();

	// Token: 0x04000664 RID: 1636
	public Dictionary<int, long> photoLevelExp = new Dictionary<int, long>();

	// Token: 0x02000783 RID: 1923
	public class KizunaLevelData
	{
		// Token: 0x06003672 RID: 13938 RVA: 0x001C651D File Offset: 0x001C471D
		public KizunaLevelData(long exp, MstCharaKizunaLevelData data)
		{
			this.LevelExp = exp;
			this.releaseItemId = data.releaseItemId;
			this.releaseItemNum = data.releaseItemNum;
		}

		// Token: 0x04003354 RID: 13140
		public long LevelExp;

		// Token: 0x04003355 RID: 13141
		public int releaseItemId;

		// Token: 0x04003356 RID: 13142
		public int releaseItemNum;
	}
}
