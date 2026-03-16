using System;

public class HomeFurniturePackData
{
	public int id { get; private set; }

	public int num { get; set; }

	public HomeFurnitureStatic staticData { get; private set; }

	public HomeFurniturePackData(int id, int num)
	{
		this.id = id;
		this.num = num;
		this.staticData = DataManager.DmHome.GetHomeFurnitureStaticData(id);
	}
}
