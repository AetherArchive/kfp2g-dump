using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class Accessory
	{
		public long accessory_id;

		public int item_id;

		public int level;

		public int owner_id;

		public int lock_flg;

		public int manage_status;

		public long insert_time;

		public long update_time;
	}
}
