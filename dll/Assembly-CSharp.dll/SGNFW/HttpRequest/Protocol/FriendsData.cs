using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class FriendsData
	{
		public int chara_id;

		public string chara_name;

		public int hp_param_lv99;

		public int atk_param_lv99;

		public int def_param_lv99;

		public int max_stock_kp;

		public int avoid_ratio;

		public int total_param;
	}
}
