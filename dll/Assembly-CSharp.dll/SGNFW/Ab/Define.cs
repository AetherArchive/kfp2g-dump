using System;

namespace SGNFW.Ab
{
	// Token: 0x02000280 RID: 640
	public class Define
	{
		// Token: 0x04001C90 RID: 7312
		public const string LIST_FILENAME = "ab_list.txt";

		// Token: 0x04001C91 RID: 7313
		public const string ENV_FILENAME = "ab_env.txt";

		// Token: 0x04001C92 RID: 7314
		public const string GUID_FILENAME = "ab_guid.txt";

		// Token: 0x04001C93 RID: 7315
		public const string FILE_SIZE_FILENAME = "filesize.csv";

		// Token: 0x04001C94 RID: 7316
		public const string STREAMING_ASSETS_PATH = "Assets/StreamingAssets/contents";

		// Token: 0x020010A8 RID: 4264
		public enum Language
		{
			// Token: 0x04005C69 RID: 23657
			All = -1,
			// Token: 0x04005C6A RID: 23658
			ja,
			// Token: 0x04005C6B RID: 23659
			en,
			// Token: 0x04005C6C RID: 23660
			zh_hans,
			// Token: 0x04005C6D RID: 23661
			zh_hant,
			// Token: 0x04005C6E RID: 23662
			fr,
			// Token: 0x04005C6F RID: 23663
			it,
			// Token: 0x04005C70 RID: 23664
			de,
			// Token: 0x04005C71 RID: 23665
			es,
			// Token: 0x04005C72 RID: 23666
			nl,
			// Token: 0x04005C73 RID: 23667
			ko,
			// Token: 0x04005C74 RID: 23668
			pt,
			// Token: 0x04005C75 RID: 23669
			ru
		}

		// Token: 0x020010A9 RID: 4265
		public enum EntryType
		{
			// Token: 0x04005C77 RID: 23671
			None = -1,
			// Token: 0x04005C78 RID: 23672
			Asset,
			// Token: 0x04005C79 RID: 23673
			Raw,
			// Token: 0x04005C7A RID: 23674
			AssetDirectory,
			// Token: 0x04005C7B RID: 23675
			RawDirectory,
			// Token: 0x04005C7C RID: 23676
			Pack,
			// Token: 0x04005C7D RID: 23677
			PackEnd
		}

		// Token: 0x020010AA RID: 4266
		public enum Platform
		{
			// Token: 0x04005C7F RID: 23679
			None = -1,
			// Token: 0x04005C80 RID: 23680
			All,
			// Token: 0x04005C81 RID: 23681
			Windows,
			// Token: 0x04005C82 RID: 23682
			OSX,
			// Token: 0x04005C83 RID: 23683
			Android,
			// Token: 0x04005C84 RID: 23684
			iOS,
			// Token: 0x04005C85 RID: 23685
			PSP2
		}
	}
}
