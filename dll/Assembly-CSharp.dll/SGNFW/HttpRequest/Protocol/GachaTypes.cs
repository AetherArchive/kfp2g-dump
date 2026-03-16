using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class GachaTypes
	{
		public int gacha_type;

		public int my_use_item_id;

		public int total_sub_play_num;

		public int continue_play_num;

		public long last_play_time;
	}
}
