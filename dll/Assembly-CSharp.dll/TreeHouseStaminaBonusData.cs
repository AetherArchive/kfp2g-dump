using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Mst;

// Token: 0x020000B1 RID: 177
public class TreeHouseStaminaBonusData
{
	// Token: 0x060007EA RID: 2026 RVA: 0x000355FC File Offset: 0x000337FC
	public TreeHouseStaminaBonusData()
	{
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x00035604 File Offset: 0x00033804
	public TreeHouseStaminaBonusData(List<TreeHouseFurniturePackData> userDataList, List<MstMasterRoomStaminaBonus> mstList)
	{
		this.staminaFurnitureCount = userDataList.Sum<TreeHouseFurniturePackData>(delegate(TreeHouseFurniturePackData itm)
		{
			if (itm.staticData.GetRarity() != ItemDef.Rarity.STAR5)
			{
				return 0;
			}
			return itm.num;
		});
		int i = 0;
		while (i < mstList.Count)
		{
			MstMasterRoomStaminaBonus mstMasterRoomStaminaBonus = mstList[i];
			if (mstMasterRoomStaminaBonus.count <= this.staminaFurnitureCount)
			{
				this.staminaBonus = mstMasterRoomStaminaBonus.bonusPoint;
				if (i > 0)
				{
					this.nextStaminaFurnitureCount = mstList[i - 1].count - this.staminaFurnitureCount;
					this.nextUpStaminaBonus = mstList[i - 1].bonusPoint - this.staminaBonus;
					break;
				}
				break;
			}
			else
			{
				i++;
			}
		}
		this.nextStaminaFurnitureCount = Math.Max(this.nextStaminaFurnitureCount, 0);
		this.nextUpStaminaBonus = Math.Max(this.nextUpStaminaBonus, 0);
		if (DataManager.DmUserInfo.staminaInfo != null)
		{
			DataManager.DmUserInfo.staminaInfo.limitBonus = this.staminaBonus;
		}
	}

	// Token: 0x040006C0 RID: 1728
	public int staminaBonus;

	// Token: 0x040006C1 RID: 1729
	public int nextUpStaminaBonus;

	// Token: 0x040006C2 RID: 1730
	public int staminaFurnitureCount;

	// Token: 0x040006C3 RID: 1731
	public int nextStaminaFurnitureCount;
}
