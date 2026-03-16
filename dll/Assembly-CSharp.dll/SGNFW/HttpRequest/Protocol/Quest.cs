using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class Quest
	{
		public int quest_id;

		public int eval;

		public int clear_num;

		public int play_num;

		public int today_clear_num;

		public long first_clear_time;

		public long last_clear_time;

		public int today_recovery_num;

		public int open_key_flag;

		public int skip_count;

		public int skip_recovery_count;
	}
}
