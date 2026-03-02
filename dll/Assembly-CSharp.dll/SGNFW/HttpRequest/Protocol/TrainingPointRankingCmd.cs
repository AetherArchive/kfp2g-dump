using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200054A RID: 1354
	public class TrainingPointRankingCmd : Command
	{
		// Token: 0x06002DAF RID: 11695 RVA: 0x001B07BF File Offset: 0x001AE9BF
		private TrainingPointRankingCmd()
		{
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x001B07C7 File Offset: 0x001AE9C7
		private TrainingPointRankingCmd(int season_id, long last_update_time)
		{
			this.request = new TrainingPointRankingRequest();
			TrainingPointRankingRequest trainingPointRankingRequest = (TrainingPointRankingRequest)this.request;
			trainingPointRankingRequest.season_id = season_id;
			trainingPointRankingRequest.last_update_time = last_update_time;
			this.Setting();
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x001B07F8 File Offset: 0x001AE9F8
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

		// Token: 0x06002DB2 RID: 11698 RVA: 0x001B0864 File Offset: 0x001AEA64
		public static TrainingPointRankingCmd Create(int season_id, long last_update_time)
		{
			return new TrainingPointRankingCmd(season_id, last_update_time);
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x001B086D File Offset: 0x001AEA6D
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TrainingPointRankingResponse>(__text);
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x001B0875 File Offset: 0x001AEA75
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TrainingPointRanking";
		}
	}
}
