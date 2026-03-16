using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Mst;

public class TreeHouseKizunaBonusData
{
	public int GetKizunaBonusRatio(bool isEnableMonthlyPack)
	{
		if (!isEnableMonthlyPack)
		{
			return this.kizunaBonusRatio;
		}
		return this.kizunaBonusRatioWithMonthlyPack;
	}

	public TreeHouseKizunaBonusData()
	{
	}

	public TreeHouseKizunaBonusData(List<TreeHouseFurniturePackData> userDataList, List<MstMasterRoomKizunaBonus> mstList)
	{
		this.totalFurniturePoint = userDataList.Sum<TreeHouseFurniturePackData>((TreeHouseFurniturePackData item) => item.num * item.staticData.kizunaBonusExp);
		int i = 0;
		while (i < mstList.Count)
		{
			MstMasterRoomKizunaBonus mstMasterRoomKizunaBonus = mstList[i];
			if (mstMasterRoomKizunaBonus.weight <= this.totalFurniturePoint)
			{
				this.kizunaBonusRatio = mstMasterRoomKizunaBonus.kizunaPointIncrease;
				this.kizunaBonusRatioWithMonthlyPack = mstMasterRoomKizunaBonus.kizunaPointIncreaseMonthlyPack;
				if (i > 0)
				{
					this.nextLevelFurniturePoint = mstList[i - 1].weight - this.totalFurniturePoint;
					break;
				}
				break;
			}
			else
			{
				i++;
			}
		}
		this.nextLevelFurniturePoint = Math.Max(this.nextLevelFurniturePoint, 0);
	}

	private int kizunaBonusRatio;

	private int kizunaBonusRatioWithMonthlyPack;

	public int totalFurniturePoint;

	public int nextLevelFurniturePoint;
}
