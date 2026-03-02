using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000553 RID: 1363
	public class TrainingStartCmd : Command
	{
		// Token: 0x06002DC2 RID: 11714 RVA: 0x001B0979 File Offset: 0x001AEB79
		private TrainingStartCmd()
		{
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x001B0984 File Offset: 0x001AEB84
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

		// Token: 0x06002DC4 RID: 11716 RVA: 0x001B09D8 File Offset: 0x001AEBD8
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

		// Token: 0x06002DC5 RID: 11717 RVA: 0x001B0A44 File Offset: 0x001AEC44
		public static TrainingStartCmd Create(int season_id, int dayofweek, int quest_id, int deck_id, int kemostatus)
		{
			return new TrainingStartCmd(season_id, dayofweek, quest_id, deck_id, kemostatus);
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x001B0A51 File Offset: 0x001AEC51
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TrainingStartResponse>(__text);
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x001B0A59 File Offset: 0x001AEC59
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TrainingStart";
		}
	}
}
