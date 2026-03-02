using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200048A RID: 1162
	public class PhotoReleaseResponse : Response
	{
		// Token: 0x0400268B RID: 9867
		public int result;

		// Token: 0x0400268C RID: 9868
		public List<MyHelper> helperList;

		// Token: 0x0400268D RID: 9869
		public List<Decks> decks;
	}
}
