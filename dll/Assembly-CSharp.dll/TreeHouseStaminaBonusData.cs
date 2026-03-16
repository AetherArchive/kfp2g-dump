using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Mst;

public class TreeHouseStaminaBonusData
{
	public TreeHouseStaminaBonusData()
	{
	}

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

	public int staminaBonus;

	public int nextUpStaminaBonus;

	public int staminaFurnitureCount;

	public int nextStaminaFurnitureCount;
}
