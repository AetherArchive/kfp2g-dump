using System;

[Serializable]
public class ItemInput
{
	public ItemInput()
	{
	}

	public ItemInput(int id, int num)
	{
		this.itemId = id;
		this.num = num;
	}

	public int itemId;

	public int num;
}
