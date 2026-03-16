using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class MasterRoomPassing
	{
		public int friend_id;

		public string name;

		public int user_rank;

		public MasterRoomPublicInfo public_info;

		public long send_stamp_time;

		public int favorite_chara_id;

		public int favorite_chara_face_id;

		public int achievement_id;

		public long stamp_point;

		public long update_time;

		public int visit_flg;

		public int follow_status;
	}
}
