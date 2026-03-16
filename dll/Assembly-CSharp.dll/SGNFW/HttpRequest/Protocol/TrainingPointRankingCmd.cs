using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TrainingPointRankingCmd : Command
	{
		private TrainingPointRankingCmd()
		{
		}

		private TrainingPointRankingCmd(int season_id, long last_update_time)
		{
			this.request = new TrainingPointRankingRequest();
			TrainingPointRankingRequest trainingPointRankingRequest = (TrainingPointRankingRequest)this.request;
			trainingPointRankingRequest.season_id = season_id;
			trainingPointRankingRequest.last_update_time = last_update_time;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "TrainingPointRanking.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static TrainingPointRankingCmd Create(int season_id, long last_update_time)
		{
			return new TrainingPointRankingCmd(season_id, last_update_time);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TrainingPointRankingResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TrainingPointRanking";
		}
	}
}
