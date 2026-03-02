using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Mst;

// Token: 0x020000B0 RID: 176
public class TreeHouseKizunaBonusData
{
	// Token: 0x060007E7 RID: 2023 RVA: 0x00035530 File Offset: 0x00033730
	public int GetKizunaBonusRatio(bool isEnableMonthlyPack)
	{
		if (!isEnableMonthlyPack)
		{
			return this.kizunaBonusRatio;
		}
		return this.kizunaBonusRatioWithMonthlyPack;
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x00035542 File Offset: 0x00033742
	public TreeHouseKizunaBonusData()
	{
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x0003554C File Offset: 0x0003374C
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

	// Token: 0x040006BC RID: 1724
	private int kizunaBonusRatio;

	// Token: 0x040006BD RID: 1725
	private int kizunaBonusRatioWithMonthlyPack;

	// Token: 0x040006BE RID: 1726
	public int totalFurniturePoint;

	// Token: 0x040006BF RID: 1727
	public int nextLevelFurniturePoint;
}
