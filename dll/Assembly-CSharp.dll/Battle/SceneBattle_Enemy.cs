using System;
using UnityEngine;

namespace Battle
{
	// Token: 0x02000214 RID: 532
	public class SceneBattle_Enemy : SceneBattle_Chara
	{
		// Token: 0x0400193E RID: 6462
		public int actionNum;

		// Token: 0x0400193F RID: 6463
		public CharaDef.EnemyActionPattern actionPattern;

		// Token: 0x04001940 RID: 6464
		public int actCount;

		// Token: 0x04001941 RID: 6465
		public ItemData dropItem;

		// Token: 0x04001942 RID: 6466
		public GameObject authObj;

		// Token: 0x04001943 RID: 6467
		public bool death;

		// Token: 0x04001944 RID: 6468
		public int huge;

		// Token: 0x04001945 RID: 6469
		public float hugeX;

		// Token: 0x04001946 RID: 6470
		public SceneBattle_Enemy body;

		// Token: 0x04001947 RID: 6471
		public string partsName;

		// Token: 0x04001948 RID: 6472
		public string partsModelName;

		// Token: 0x04001949 RID: 6473
		public CharaDef.PartsType partsType;

		// Token: 0x0400194A RID: 6474
		public Transform parts;

		// Token: 0x0400194B RID: 6475
		public GameObject partModel;

		// Token: 0x0400194C RID: 6476
		public ModelHandle modelHandle;

		// Token: 0x0400194D RID: 6477
		public float actZ;

		// Token: 0x0400194E RID: 6478
		public string deathEffect;

		// Token: 0x0400194F RID: 6479
		public float deathEffectScale;
	}
}
