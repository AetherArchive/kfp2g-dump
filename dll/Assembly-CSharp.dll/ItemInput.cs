using System;

// Token: 0x0200008E RID: 142
[Serializable]
public class ItemInput
{
	// Token: 0x06000588 RID: 1416 RVA: 0x0002544F File Offset: 0x0002364F
	public ItemInput()
	{
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x00025457 File Offset: 0x00023657
	public ItemInput(int id, int num)
	{
		this.itemId = id;
		this.num = num;
	}

	// Token: 0x0400058E RID: 1422
	public int itemId;

	// Token: 0x0400058F RID: 1423
	public int num;
}
