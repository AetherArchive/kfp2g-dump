using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class TrainingScore
	{
		public int season_id;

		public int dayofweek;

		public long hiscore;

		public long playtime;

		public Party party;
	}
}
