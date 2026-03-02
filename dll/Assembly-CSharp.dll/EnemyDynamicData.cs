using System;

// Token: 0x02000051 RID: 81
public class EnemyDynamicData
{
	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000243 RID: 579 RVA: 0x00014077 File Offset: 0x00012277
	// (set) Token: 0x06000244 RID: 580 RVA: 0x0001407F File Offset: 0x0001227F
	public int id { get; private set; }

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x06000245 RID: 581 RVA: 0x00014088 File Offset: 0x00012288
	// (set) Token: 0x06000246 RID: 582 RVA: 0x00014090 File Offset: 0x00012290
	public int level { get; private set; }

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06000247 RID: 583 RVA: 0x00014099 File Offset: 0x00012299
	// (set) Token: 0x06000248 RID: 584 RVA: 0x000140A1 File Offset: 0x000122A1
	public int hpratio { get; private set; }

	// Token: 0x06000249 RID: 585 RVA: 0x000140AA File Offset: 0x000122AA
	public EnemyDynamicData(int id, int level, int hpratio)
	{
		this.id = id;
		this.level = level;
		this.hpratio = hpratio;
	}
}
