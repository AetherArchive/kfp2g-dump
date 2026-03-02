using System;
using System.Collections.Generic;

namespace Battle
{
	// Token: 0x02000224 RID: 548
	public class SceneBattle_Tag
	{
		// Token: 0x06002313 RID: 8979 RVA: 0x00195D8C File Offset: 0x00193F8C
		public SceneBattle_Tag(SceneBattle_Chara chr)
		{
			this.tag = chr;
			this.cover = null;
			this.flg = 0;
			this.hp = 0;
			this.kp = 0;
			this.buf = null;
			this.recover = null;
			this.infKey = 0;
			this.noExe = 0;
			this.dup = 1f;
		}

		// Token: 0x04001A68 RID: 6760
		public SceneBattle_Chara tag;

		// Token: 0x04001A69 RID: 6761
		public SceneBattle_Chara cover;

		// Token: 0x04001A6A RID: 6762
		public int flg;

		// Token: 0x04001A6B RID: 6763
		public int hp;

		// Token: 0x04001A6C RID: 6764
		public int kp;

		// Token: 0x04001A6D RID: 6765
		public SceneBattle_Buff buf;

		// Token: 0x04001A6E RID: 6766
		public List<int> recover;

		// Token: 0x04001A6F RID: 6767
		public int infKey;

		// Token: 0x04001A70 RID: 6768
		public int noExe;

		// Token: 0x04001A71 RID: 6769
		public float dup;
	}
}
