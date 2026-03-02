using System;
using System.Collections.Generic;

// Token: 0x020000DA RID: 218
public class QuestStaticData
{
	// Token: 0x17000207 RID: 519
	// (get) Token: 0x060009B8 RID: 2488 RVA: 0x0003B30F File Offset: 0x0003950F
	// (set) Token: 0x060009B9 RID: 2489 RVA: 0x0003B317 File Offset: 0x00039517
	public Dictionary<int, QuestStaticChapter> chapterDataMap { get; set; } = new Dictionary<int, QuestStaticChapter>();

	// Token: 0x17000208 RID: 520
	// (get) Token: 0x060009BA RID: 2490 RVA: 0x0003B320 File Offset: 0x00039520
	// (set) Token: 0x060009BB RID: 2491 RVA: 0x0003B328 File Offset: 0x00039528
	public List<QuestStaticChapter> chapterDataList { get; set; }

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x060009BC RID: 2492 RVA: 0x0003B331 File Offset: 0x00039531
	// (set) Token: 0x060009BD RID: 2493 RVA: 0x0003B339 File Offset: 0x00039539
	public Dictionary<int, QuestStaticMap> mapDataMap { get; set; } = new Dictionary<int, QuestStaticMap>();

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x060009BE RID: 2494 RVA: 0x0003B342 File Offset: 0x00039542
	// (set) Token: 0x060009BF RID: 2495 RVA: 0x0003B34A File Offset: 0x0003954A
	public List<QuestStaticMap> mapDataList { get; set; }

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x060009C0 RID: 2496 RVA: 0x0003B353 File Offset: 0x00039553
	// (set) Token: 0x060009C1 RID: 2497 RVA: 0x0003B35B File Offset: 0x0003955B
	public Dictionary<int, QuestStaticQuestGroup> groupDataMap { get; set; } = new Dictionary<int, QuestStaticQuestGroup>();

	// Token: 0x1700020C RID: 524
	// (get) Token: 0x060009C2 RID: 2498 RVA: 0x0003B364 File Offset: 0x00039564
	// (set) Token: 0x060009C3 RID: 2499 RVA: 0x0003B36C File Offset: 0x0003956C
	public List<QuestStaticQuestGroup> groupDataList { get; set; }

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x060009C4 RID: 2500 RVA: 0x0003B375 File Offset: 0x00039575
	// (set) Token: 0x060009C5 RID: 2501 RVA: 0x0003B37D File Offset: 0x0003957D
	public Dictionary<int, QuestStaticQuestOne> oneDataMap { get; set; } = new Dictionary<int, QuestStaticQuestOne>();

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x060009C6 RID: 2502 RVA: 0x0003B386 File Offset: 0x00039586
	// (set) Token: 0x060009C7 RID: 2503 RVA: 0x0003B38E File Offset: 0x0003958E
	public List<QuestStaticQuestOne> oneDataList { get; set; }

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x060009C8 RID: 2504 RVA: 0x0003B397 File Offset: 0x00039597
	// (set) Token: 0x060009C9 RID: 2505 RVA: 0x0003B39F File Offset: 0x0003959F
	public Dictionary<int, HashSet<int>> dropItemQuestMap { get; set; } = new Dictionary<int, HashSet<int>>();

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x060009CA RID: 2506 RVA: 0x0003B3A8 File Offset: 0x000395A8
	// (set) Token: 0x060009CB RID: 2507 RVA: 0x0003B3B0 File Offset: 0x000395B0
	public List<QuestStaticRule> ruleDataList { get; set; }
}
