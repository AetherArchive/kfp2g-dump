using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class CoopPlayerInfo
	{
		public int rank;

		public string user_name;

		public int favorite_chara_id;

		public int favorite_chara_face_id;

		public int achievement_id;

		public int user_level;

		public string comment;

		public int point;
	}
}
