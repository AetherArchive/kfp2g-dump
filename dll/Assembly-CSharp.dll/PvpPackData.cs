using System;

// Token: 0x0200009C RID: 156
public class PvpPackData
{
	// Token: 0x060006AF RID: 1711 RVA: 0x0002D1DB File Offset: 0x0002B3DB
	public PvpRankInfo GetPvpRankInfo()
	{
		return this.GetPvpRankInfoByPoint(this.dynamicData.userInfo.pvpPoint);
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x0002D1F3 File Offset: 0x0002B3F3
	public static PvpPackData MakeDummy(int id)
	{
		return new PvpPackData
		{
			seasonId = id,
			staticData = PvpStaticData.MakeDummy(id),
			dynamicData = PvpDynamicData.MakeDummy(id)
		};
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x0002D219 File Offset: 0x0002B419
	public PvpRankInfo GetPvpRankInfoByPoint(int point)
	{
		return this.staticData.GetPvpRankInfoByPoint(point);
	}

	// Token: 0x04000619 RID: 1561
	private PvpStaticData.Type pvpType;

	// Token: 0x0400061A RID: 1562
	public int seasonId;

	// Token: 0x0400061B RID: 1563
	public PvpStaticData staticData;

	// Token: 0x0400061C RID: 1564
	public PvpDynamicData dynamicData;
}
