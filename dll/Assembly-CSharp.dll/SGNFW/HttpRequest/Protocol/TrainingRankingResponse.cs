using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TrainingRankingResponse : Response
	{
		public List<TrainingRanking> training_ranking;

		public long last_update_time;
	}
}
