using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class Photo
	{
		public long photo_id;

		public int item_id;

		public int level;

		public long exp;

		public int limit_over_num;

		public int lock_flg;

		public int manage_status;

		public int img_flg;

		public int favorite_flg;

		public long insert_time;

		public long update_time;
	}
}
