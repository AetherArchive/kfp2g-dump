using System;
using UnityEngine;

namespace Battle
{
	// Token: 0x0200020E RID: 526
	public class SceneBattle_CardInfo
	{
		// Token: 0x06002234 RID: 8756 RVA: 0x001921E6 File Offset: 0x001903E6
		public SceneBattle_CardInfo()
		{
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x001921F0 File Offset: 0x001903F0
		public SceneBattle_CardInfo(SceneBattle_CardInfo original)
		{
			this.cid = original.cid;
			this.card = original.card;
			this.chara = original.chara;
			this.select = original.select;
			this.selArts = original.selArts;
			this.action = original.action;
			this.actArts = original.actArts;
			this.selGry = original.selGry;
			this.artGry = original.artGry;
		}

		// Token: 0x040018BD RID: 6333
		public int cid;

		// Token: 0x040018BE RID: 6334
		public CharaOrderCard card;

		// Token: 0x040018BF RID: 6335
		public GameObject chara;

		// Token: 0x040018C0 RID: 6336
		public GameObject select;

		// Token: 0x040018C1 RID: 6337
		public GameObject selArts;

		// Token: 0x040018C2 RID: 6338
		public GameObject action;

		// Token: 0x040018C3 RID: 6339
		public GameObject actArts;

		// Token: 0x040018C4 RID: 6340
		public Grayscale selGry;

		// Token: 0x040018C5 RID: 6341
		public Grayscale artGry;
	}
}
