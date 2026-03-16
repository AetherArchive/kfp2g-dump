using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomGetDataRequest : Request
	{
		public int get_furniture_flg;

		public int get_chara_flg;

		public int get_receive_stamp_log_flg;

		public int get_myset_flg;

		public int get_follow_flg;

		public int get_passing_flg;

		public int get_ranking_flg;

		public int get_stamp_log_flg;

		public int get_public_info_flg;
	}
}
