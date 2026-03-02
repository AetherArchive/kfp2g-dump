using System;
using System.Collections.Generic;

// Token: 0x02000053 RID: 83
public class EnemyAttackData
{
	// Token: 0x040002B4 RID: 692
	public CharaDef.EnemyActionPattern actionPattern;

	// Token: 0x040002B5 RID: 693
	public List<EnemyAttackData.Param> attackList = new List<EnemyAttackData.Param>();

	// Token: 0x02000601 RID: 1537
	public class Param
	{
		// Token: 0x04002C75 RID: 11381
		public int point;

		// Token: 0x04002C76 RID: 11382
		public CharaStaticAction param;

		// Token: 0x04002C77 RID: 11383
		public List<int> death;

		// Token: 0x04002C78 RID: 11384
		public List<int> alive;
	}
}
