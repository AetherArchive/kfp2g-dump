using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class LevelResult
	{
		public int chara_id;

		public int level;

		public long exp;

		public int success_status;

		public int returnitem;
	}
}
