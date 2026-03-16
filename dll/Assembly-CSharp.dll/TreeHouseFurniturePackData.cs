using System;

public class TreeHouseFurniturePackData
{
	public TreeHouseFurniturePackData(int id, int num)
	{
		this.id = id;
		this.num = num;
		this.staticData = DataManager.DmTreeHouse.GetTreeHouseFurnitureStaticData(id);
	}

	public int id;

	public int num;

	public TreeHouseFurnitureStatic staticData;
}
