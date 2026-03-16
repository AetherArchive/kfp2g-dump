using System;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Mst;

public class GameAppearanceUtill
{
	public static MstGameAppearanceData GetMstGameAppearanceData(DateTime time)
	{
		List<MstGameAppearanceData> mst = Singleton<MstManager>.Instance.GetMst<List<MstGameAppearanceData>>(MstType.GAME_APPEARANCE_DATA);
		long num = PrjUtil.ConvertTicksToTime(time.Ticks);
		foreach (MstGameAppearanceData mstGameAppearanceData in mst)
		{
			if (mstGameAppearanceData.startTime <= num && mstGameAppearanceData.endTime >= num)
			{
				return mstGameAppearanceData;
			}
		}
		return new MstGameAppearanceData
		{
			homeModelFllePathA = "",
			homeModelFllePathB = "",
			titleTexturePath = ""
		};
	}
}
