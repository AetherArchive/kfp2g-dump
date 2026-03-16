using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TrainingInfoResponse : Response
	{
		public int season_id;

		public int dayofweek;

		public long hiscore;

		public int today_recovery_num;

		public int today_play_num;

		public int firsttme_flag;
	}
}
