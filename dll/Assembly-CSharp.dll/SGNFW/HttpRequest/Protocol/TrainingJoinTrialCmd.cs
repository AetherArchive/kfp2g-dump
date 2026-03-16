using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TrainingJoinTrialCmd : Command
	{
		private TrainingJoinTrialCmd(int trial_id)
		{
			this.request = new TrainingJoinTrialParamRequest();
			((TrainingJoinTrialParamRequest)this.request).trial_id = trial_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "TrainingJoinTrial.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static TrainingJoinTrialCmd Create(int trial_id)
		{
			return new TrainingJoinTrialCmd(trial_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TrainingJoinTrialParamResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TrainingJoinTrial";
		}
	}
}
