using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class Gacha
	{
		public int gacha_id;

		public int gacha_type;

		public int ceiling_count;

		public int total_sub_play_num;

		public int continue_play_num;

		public int disc_play_num;

		public long last_play_time;

		public long today_last_play_time;

		public int reset_num;

		public int box_remain_num;
	}
}
