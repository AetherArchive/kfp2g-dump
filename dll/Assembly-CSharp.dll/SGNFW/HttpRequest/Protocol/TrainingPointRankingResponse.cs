using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TrainingPointRankingResponse : Response
	{
		public List<TrainingPointRankingData> training_point_ranking;

		public long last_update_time;

		public int myrank;

		public int confirm_flag;
	}
}
