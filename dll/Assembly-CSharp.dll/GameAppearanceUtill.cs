using System;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Mst;

// Token: 0x020000C4 RID: 196
public class GameAppearanceUtill
{
	// Token: 0x060008CF RID: 2255 RVA: 0x000387F8 File Offset: 0x000369F8
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
