using System;

public class PvpPackData
{
	public PvpRankInfo GetPvpRankInfo()
	{
		return this.GetPvpRankInfoByPoint(this.dynamicData.userInfo.pvpPoint);
	}

	public static PvpPackData MakeDummy(int id)
	{
		return new PvpPackData
		{
			seasonId = id,
			staticData = PvpStaticData.MakeDummy(id),
			dynamicData = PvpDynamicData.MakeDummy(id)
		};
	}

	public PvpRankInfo GetPvpRankInfoByPoint(int point)
	{
		return this.staticData.GetPvpRankInfoByPoint(point);
	}

	private PvpStaticData.Type pvpType;

	public int seasonId;

	public PvpStaticData staticData;

	public PvpDynamicData dynamicData;
}
