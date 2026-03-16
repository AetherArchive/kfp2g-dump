using System;
using System.Collections.Generic;

public class PvpRankInfo
{
	public int id;

	public int pointRangeLow;

	public string rankName;

	public string rankIcon;

	public PvpRankInfo nexRankInfo;

	public List<ItemData> rewardItemList = new List<ItemData>();
}
