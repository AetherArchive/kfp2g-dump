using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class PhotoGrowResult
	{
		public long photo_id;

		public int item_id;

		public int level;

		public long exp;

		public int result_limit_over_num;

		public int lot_result;

		public int notuse;
	}
}
