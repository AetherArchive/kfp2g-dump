using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TrainingRankingCmd : Command
	{
		private TrainingRankingCmd()
		{
		}

		private TrainingRankingCmd(int season_id, int dayofweek, long last_update_time)
		{
			this.request = new TrainingRankingRequest();
			TrainingRankingRequest trainingRankingRequest = (TrainingRankingRequest)this.request;
			trainingRankingRequest.season_id = season_id;
			trainingRankingRequest.dayofweek = dayofweek;
			trainingRankingRequest.last_update_time = last_update_time;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "TrainingRanking.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static TrainingRankingCmd Create(int season_id, int dayofweek, long last_update_time)
		{
			return new TrainingRankingCmd(season_id, dayofweek, last_update_time);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TrainingRankingResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TrainingRanking";
		}
	}
}
