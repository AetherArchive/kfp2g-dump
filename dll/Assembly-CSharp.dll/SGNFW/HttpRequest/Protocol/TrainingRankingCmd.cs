using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200054F RID: 1359
	public class TrainingRankingCmd : Command
	{
		// Token: 0x06002DB9 RID: 11705 RVA: 0x001B089C File Offset: 0x001AEA9C
		private TrainingRankingCmd()
		{
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x001B08A4 File Offset: 0x001AEAA4
		private TrainingRankingCmd(int season_id, int dayofweek, long last_update_time)
		{
			this.request = new TrainingRankingRequest();
			TrainingRankingRequest trainingRankingRequest = (TrainingRankingRequest)this.request;
			trainingRankingRequest.season_id = season_id;
			trainingRankingRequest.dayofweek = dayofweek;
			trainingRankingRequest.last_update_time = last_update_time;
			this.Setting();
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x001B08DC File Offset: 0x001AEADC
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

		// Token: 0x06002DBC RID: 11708 RVA: 0x001B0948 File Offset: 0x001AEB48
		public static TrainingRankingCmd Create(int season_id, int dayofweek, long last_update_time)
		{
			return new TrainingRankingCmd(season_id, dayofweek, last_update_time);
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x001B0952 File Offset: 0x001AEB52
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TrainingRankingResponse>(__text);
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x001B095A File Offset: 0x001AEB5A
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TrainingRanking";
		}
	}
}
