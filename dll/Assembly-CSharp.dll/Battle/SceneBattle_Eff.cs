using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
	// Token: 0x02000213 RID: 531
	public class SceneBattle_Eff
	{
		// Token: 0x0600224A RID: 8778 RVA: 0x00193050 File Offset: 0x00191250
		public SceneBattle_Eff()
		{
			this.tag = null;
			this.tim = 0f;
			this.nam = null;
			this.fly = -1f;
			this.height = -1f;
			this.node = CharaDef.TargetNodeName.root;
			this.pos = Vector3.zero;
			this.rot = Vector3.zero;
			this.scl = Vector3.one;
			this.hittag = null;
		}

		// Token: 0x04001933 RID: 6451
		public List<SceneBattle_Chara> tag;

		// Token: 0x04001934 RID: 6452
		public float tim;

		// Token: 0x04001935 RID: 6453
		public string nam;

		// Token: 0x04001936 RID: 6454
		public bool light;

		// Token: 0x04001937 RID: 6455
		public float fly;

		// Token: 0x04001938 RID: 6456
		public float height;

		// Token: 0x04001939 RID: 6457
		public CharaDef.TargetNodeName node;

		// Token: 0x0400193A RID: 6458
		public Vector3 pos;

		// Token: 0x0400193B RID: 6459
		public Vector3 rot;

		// Token: 0x0400193C RID: 6460
		public Vector3 scl;

		// Token: 0x0400193D RID: 6461
		public List<SceneBattle_Tag> hittag;
	}
}
