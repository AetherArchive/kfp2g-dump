using System;

// Token: 0x02000089 RID: 137
public class ItemData
{
	// Token: 0x17000104 RID: 260
	// (get) Token: 0x0600055A RID: 1370 RVA: 0x0002500E File Offset: 0x0002320E
	// (set) Token: 0x0600055B RID: 1371 RVA: 0x00025016 File Offset: 0x00023216
	public int id { get; private set; }

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x0600055C RID: 1372 RVA: 0x0002501F File Offset: 0x0002321F
	// (set) Token: 0x0600055D RID: 1373 RVA: 0x00025027 File Offset: 0x00023227
	public int num { get; private set; }

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x0600055E RID: 1374 RVA: 0x00025030 File Offset: 0x00023230
	// (set) Token: 0x0600055F RID: 1375 RVA: 0x00025038 File Offset: 0x00023238
	public ItemStaticBase staticData { get; private set; }

	// Token: 0x06000560 RID: 1376 RVA: 0x00025041 File Offset: 0x00023241
	public ItemData(int id, int num)
	{
		this.id = id;
		this.num = num;
		this.staticData = DataManager.DmItem.GetItemStaticBase(id);
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x00025068 File Offset: 0x00023268
	public void skipNumSet(int num)
	{
		this.num = num;
	}
}
