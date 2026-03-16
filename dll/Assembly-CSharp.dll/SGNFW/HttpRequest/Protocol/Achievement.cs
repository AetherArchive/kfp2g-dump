using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class Achievement
	{
		public int achievement_id;

		public int select_flag;

		public int new_flag;

		public long insert_time;

		public long update_time;
	}
}
