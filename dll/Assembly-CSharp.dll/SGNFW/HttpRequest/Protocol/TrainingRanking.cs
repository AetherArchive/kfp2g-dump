using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class TrainingRanking
	{
		public int rank;

		public int user_id;

		public int friend_id;

		public string user_name;

		public int favorite_chara_id;

		public int favorite_chara_face_id;

		public int achievement_id;

		public TrainingScore trainingscore;

		public int user_level;
	}
}
