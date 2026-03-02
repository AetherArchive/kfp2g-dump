using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000492 RID: 1170
	[Serializable]
	public class Picnic
	{
		// Token: 0x0400269B RID: 9883
		public Assets assets;

		// Token: 0x0400269C RID: 9884
		public List<PicnicChara> charalist;

		// Token: 0x0400269D RID: 9885
		public int energy;

		// Token: 0x0400269E RID: 9886
		public List<PicnicPlay> playlist;
	}
}
