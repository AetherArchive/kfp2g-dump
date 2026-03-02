using System;

// Token: 0x020000BA RID: 186
public class TreeHouseFurniturePackData
{
	// Token: 0x06000803 RID: 2051 RVA: 0x00035EBF File Offset: 0x000340BF
	public TreeHouseFurniturePackData(int id, int num)
	{
		this.id = id;
		this.num = num;
		this.staticData = DataManager.DmTreeHouse.GetTreeHouseFurnitureStaticData(id);
	}

	// Token: 0x040006FC RID: 1788
	public int id;

	// Token: 0x040006FD RID: 1789
	public int num;

	// Token: 0x040006FE RID: 1790
	public TreeHouseFurnitureStatic staticData;
}
