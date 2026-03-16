using System;

public class ItemData
{
	public int id { get; private set; }

	public int num { get; private set; }

	public ItemStaticBase staticData { get; private set; }

	public ItemData(int id, int num)
	{
		this.id = id;
		this.num = num;
		this.staticData = DataManager.DmItem.GetItemStaticBase(id);
	}

	public void skipNumSet(int num)
	{
		this.num = num;
	}
}
