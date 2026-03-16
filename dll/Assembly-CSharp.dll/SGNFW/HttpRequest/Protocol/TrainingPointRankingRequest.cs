using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TrainingPointRankingRequest : Request
	{
		public int season_id;

		public long last_update_time;
	}
}
