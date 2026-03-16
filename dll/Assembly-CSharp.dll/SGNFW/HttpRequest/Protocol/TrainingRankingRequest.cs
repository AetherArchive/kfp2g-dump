using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TrainingRankingRequest : Request
	{
		public int season_id;

		public int dayofweek;

		public long last_update_time;
	}
}
