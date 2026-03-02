using System;

// Token: 0x02000081 RID: 129
public class HomeFurniturePackData
{
	// Token: 0x170000EF RID: 239
	// (get) Token: 0x060004E6 RID: 1254 RVA: 0x000230D4 File Offset: 0x000212D4
	// (set) Token: 0x060004E7 RID: 1255 RVA: 0x000230DC File Offset: 0x000212DC
	public int id { get; private set; }

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x060004E8 RID: 1256 RVA: 0x000230E5 File Offset: 0x000212E5
	// (set) Token: 0x060004E9 RID: 1257 RVA: 0x000230ED File Offset: 0x000212ED
	public int num { get; set; }

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x060004EA RID: 1258 RVA: 0x000230F6 File Offset: 0x000212F6
	// (set) Token: 0x060004EB RID: 1259 RVA: 0x000230FE File Offset: 0x000212FE
	public HomeFurnitureStatic staticData { get; private set; }

	// Token: 0x060004EC RID: 1260 RVA: 0x00023107 File Offset: 0x00021307
	public HomeFurniturePackData(int id, int num)
	{
		this.id = id;
		this.num = num;
		this.staticData = DataManager.DmHome.GetHomeFurnitureStaticData(id);
	}
}
