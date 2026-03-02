using System;
using System.Collections.Generic;

namespace Battle
{
	// Token: 0x0200020B RID: 523
	public class SceneBattle_Act
	{
		// Token: 0x0600222F RID: 8751 RVA: 0x001920BD File Offset: 0x001902BD
		public SceneBattle_Act()
		{
			this.dmg = null;
			this.buf = null;
			this.tag = null;
			this.tim = 0f;
			this.idx = 0;
			this.ability = null;
			this.tactics = null;
		}

		// Token: 0x0400187D RID: 6269
		public CharaDamageParam dmg;

		// Token: 0x0400187E RID: 6270
		public CharaBuffParam buf;

		// Token: 0x0400187F RID: 6271
		public List<SceneBattle_Tag> tag;

		// Token: 0x04001880 RID: 6272
		public float tim;

		// Token: 0x04001881 RID: 6273
		public int idx;

		// Token: 0x04001882 RID: 6274
		public List<SceneBattle_Tag> ability;

		// Token: 0x04001883 RID: 6275
		public List<SceneBattle_Tag> tactics;
	}
}
