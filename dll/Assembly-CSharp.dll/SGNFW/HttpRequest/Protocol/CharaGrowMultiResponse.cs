using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200036F RID: 879
	public class CharaGrowMultiResponse : Response
	{
		// Token: 0x040023D7 RID: 9175
		public Assets assets;

		// Token: 0x040023D8 RID: 9176
		public LevelResult level_result;

		// Token: 0x040023D9 RID: 9177
		public List<WildResult> wild_result;

		// Token: 0x040023DA RID: 9178
		public RankUpResult rankup_result;
	}
}
