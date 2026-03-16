using System;

namespace SGNFW.Ab
{
	public class Define
	{
		public const string LIST_FILENAME = "ab_list.txt";

		public const string ENV_FILENAME = "ab_env.txt";

		public const string GUID_FILENAME = "ab_guid.txt";

		public const string FILE_SIZE_FILENAME = "filesize.csv";

		public const string STREAMING_ASSETS_PATH = "Assets/StreamingAssets/contents";

		public enum Language
		{
			All = -1,
			ja,
			en,
			zh_hans,
			zh_hant,
			fr,
			it,
			de,
			es,
			nl,
			ko,
			pt,
			ru
		}

		public enum EntryType
		{
			None = -1,
			Asset,
			Raw,
			AssetDirectory,
			RawDirectory,
			Pack,
			PackEnd
		}

		public enum Platform
		{
			None = -1,
			All,
			Windows,
			OSX,
			Android,
			iOS,
			PSP2
		}
	}
}
