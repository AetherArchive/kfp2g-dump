using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TrainingMyScoreCmd : Command
	{
		private TrainingMyScoreCmd()
		{
		}

		private TrainingMyScoreCmd(int season_id)
		{
			this.request = new TrainingMyScoreRequest();
			((TrainingMyScoreRequest)this.request).season_id = season_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "TrainingMyScore.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static TrainingMyScoreCmd Create(int season_id)
		{
			return new TrainingMyScoreCmd(season_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TrainingMyScoreResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TrainingMyScore";
		}
	}
}
