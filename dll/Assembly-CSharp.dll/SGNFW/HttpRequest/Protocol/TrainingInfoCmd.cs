using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TrainingInfoCmd : Command
	{
		private TrainingInfoCmd()
		{
			this.request = new TrainingInfoRequest();
			TrainingInfoRequest trainingInfoRequest = (TrainingInfoRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "TrainingInfo.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static TrainingInfoCmd Create()
		{
			return new TrainingInfoCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TrainingInfoResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TrainingInfo";
		}
	}
}
