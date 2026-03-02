using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003B1 RID: 945
	[Serializable]
	public class Decks
	{
		// Token: 0x04002465 RID: 9317
		public int deck_id;

		// Token: 0x04002466 RID: 9318
		public string deck_name;

		// Token: 0x04002467 RID: 9319
		public int master_skil_id;

		// Token: 0x04002468 RID: 9320
		public int kemostatus;

		// Token: 0x04002469 RID: 9321
		public List<Deck> deck;

		// Token: 0x0400246A RID: 9322
		public int tactics_param1;

		// Token: 0x0400246B RID: 9323
		public int tactics_param2;

		// Token: 0x0400246C RID: 9324
		public int tactics_param3;
	}
}
