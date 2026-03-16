using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TrainingLimitRecoveryCmd : Command
	{
		private TrainingLimitRecoveryCmd()
		{
		}

		private TrainingLimitRecoveryCmd(int season_id, int dayofweek)
		{
			this.request = new TrainingLimitRecoveryRequest();
			TrainingLimitRecoveryRequest trainingLimitRecoveryRequest = (TrainingLimitRecoveryRequest)this.request;
			trainingLimitRecoveryRequest.season_id = season_id;
			trainingLimitRecoveryRequest.dayofweek = dayofweek;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "TrainingLimitRecovery.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static TrainingLimitRecoveryCmd Create(int season_id, int dayofweek)
		{
			return new TrainingLimitRecoveryCmd(season_id, dayofweek);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TrainingLimitRecoveryResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TrainingLimitRecovery";
		}
	}
}
