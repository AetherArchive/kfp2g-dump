using System;
using SGNFW.Mst;

public class HomePlacementStatic
{
	public int id { get; set; }

	public HomeFurnitureStatic.Category enableFurnitureCategory { get; set; }

	public string name { get; set; }

	public string locatorName { get; set; }

	public int sortPriority { get; private set; }

	public HomePlacementStatic(MstHomePlacementData mst)
	{
		this.id = mst.positionId;
		this.enableFurnitureCategory = (HomeFurnitureStatic.Category)mst.enableFurnitureCategory;
		this.name = mst.name;
		this.locatorName = mst.locatorName;
		this.sortPriority = mst.sortNum;
	}
}
