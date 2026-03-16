using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class MasterRoomStampLog
	{
		public int friend_id;

		public int stamp_id;

		public string name;

		public int user_rank;

		public MasterRoomPublicInfo public_info;

		public long send_stamp_time;

		public int favorite_chara_id;

		public int favorite_chara_face_id;

		public int achievement_id;

		public long time;

		public int send_flg;

		public int visit_flg;

		public int follow_status;
	}
}
