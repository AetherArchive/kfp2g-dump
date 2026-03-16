using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class TraningScore
	{
		public int season_id;

		public int dayofweek;

		public long hiscore;

		public long playtime;

		public Party party;
	}
}
