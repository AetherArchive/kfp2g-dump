using System;
using System.Collections.Generic;

// Token: 0x0200004F RID: 79
public class CharaPromotePreset
{
	// Token: 0x17000059 RID: 89
	// (get) Token: 0x0600023E RID: 574 RVA: 0x00013F4C File Offset: 0x0001214C
	// (set) Token: 0x0600023F RID: 575 RVA: 0x00013F54 File Offset: 0x00012154
	public int PromoteNum { get; set; }

	// Token: 0x06000240 RID: 576 RVA: 0x00013F60 File Offset: 0x00012160
	public static CharaPromotePreset MakeDummy(int depth)
	{
		return new CharaPromotePreset
		{
			promoteOneList = new List<CharaPromoteOne>
			{
				new CharaPromoteOne
				{
					promoteUseItemId = 14001,
					promoteUseItemNum = depth,
					costGoldNum = depth * 100
				},
				new CharaPromoteOne
				{
					promoteUseItemId = 14001,
					promoteUseItemNum = depth,
					costGoldNum = depth * 100
				},
				new CharaPromoteOne
				{
					promoteUseItemId = 14001,
					promoteUseItemNum = depth,
					costGoldNum = depth * 100
				},
				new CharaPromoteOne
				{
					promoteUseItemId = 14001,
					promoteUseItemNum = depth,
					costGoldNum = depth * 100
				},
				new CharaPromoteOne
				{
					promoteUseItemId = 14001,
					promoteUseItemNum = depth,
					costGoldNum = depth * 100
				},
				new CharaPromoteOne
				{
					promoteUseItemId = 14001,
					promoteUseItemNum = depth,
					costGoldNum = depth * 100
				}
			}
		};
	}

	// Token: 0x040002A1 RID: 673
	public List<CharaPromoteOne> promoteOneList;
}
