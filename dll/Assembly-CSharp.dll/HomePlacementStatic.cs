using System;
using SGNFW.Mst;

// Token: 0x02000084 RID: 132
public class HomePlacementStatic
{
	// Token: 0x170000FA RID: 250
	// (get) Token: 0x06000507 RID: 1287 RVA: 0x0002329D File Offset: 0x0002149D
	// (set) Token: 0x06000508 RID: 1288 RVA: 0x000232A5 File Offset: 0x000214A5
	public int id { get; set; }

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x06000509 RID: 1289 RVA: 0x000232AE File Offset: 0x000214AE
	// (set) Token: 0x0600050A RID: 1290 RVA: 0x000232B6 File Offset: 0x000214B6
	public HomeFurnitureStatic.Category enableFurnitureCategory { get; set; }

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x0600050B RID: 1291 RVA: 0x000232BF File Offset: 0x000214BF
	// (set) Token: 0x0600050C RID: 1292 RVA: 0x000232C7 File Offset: 0x000214C7
	public string name { get; set; }

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x0600050D RID: 1293 RVA: 0x000232D0 File Offset: 0x000214D0
	// (set) Token: 0x0600050E RID: 1294 RVA: 0x000232D8 File Offset: 0x000214D8
	public string locatorName { get; set; }

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x0600050F RID: 1295 RVA: 0x000232E1 File Offset: 0x000214E1
	// (set) Token: 0x06000510 RID: 1296 RVA: 0x000232E9 File Offset: 0x000214E9
	public int sortPriority { get; private set; }

	// Token: 0x06000511 RID: 1297 RVA: 0x000232F4 File Offset: 0x000214F4
	public HomePlacementStatic(MstHomePlacementData mst)
	{
		this.id = mst.positionId;
		this.enableFurnitureCategory = (HomeFurnitureStatic.Category)mst.enableFurnitureCategory;
		this.name = mst.name;
		this.locatorName = mst.locatorName;
		this.sortPriority = mst.sortNum;
	}
}
