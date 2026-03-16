using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TrainingStartCmd : Command
	{
		private TrainingStartCmd()
		{
		}

		private TrainingStartCmd(int season_id, int dayofweek, int quest_id, int deck_id, int kemostatus)
		{
			this.request = new TrainingStartRequest();
			TrainingStartRequest trainingStartRequest = (TrainingStartRequest)this.request;
			trainingStartRequest.season_id = season_id;
			trainingStartRequest.dayofweek = dayofweek;
			trainingStartRequest.quest_id = quest_id;
			trainingStartRequest.deck_id = deck_id;
			trainingStartRequest.kemostatus = kemostatus;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "TrainingStart.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static TrainingStartCmd Create(int season_id, int dayofweek, int quest_id, int deck_id, int kemostatus)
		{
			return new TrainingStartCmd(season_id, dayofweek, quest_id, deck_id, kemostatus);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TrainingStartResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TrainingStart";
		}
	}
}
