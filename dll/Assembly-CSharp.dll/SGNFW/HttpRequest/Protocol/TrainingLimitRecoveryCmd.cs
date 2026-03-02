using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000544 RID: 1348
	public class TrainingLimitRecoveryCmd : Command
	{
		// Token: 0x06002D9F RID: 11679 RVA: 0x001B062B File Offset: 0x001AE82B
		private TrainingLimitRecoveryCmd()
		{
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x001B0633 File Offset: 0x001AE833
		private TrainingLimitRecoveryCmd(int season_id, int dayofweek)
		{
			this.request = new TrainingLimitRecoveryRequest();
			TrainingLimitRecoveryRequest trainingLimitRecoveryRequest = (TrainingLimitRecoveryRequest)this.request;
			trainingLimitRecoveryRequest.season_id = season_id;
			trainingLimitRecoveryRequest.dayofweek = dayofweek;
			this.Setting();
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x001B0664 File Offset: 0x001AE864
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

		// Token: 0x06002DA2 RID: 11682 RVA: 0x001B06D0 File Offset: 0x001AE8D0
		public static TrainingLimitRecoveryCmd Create(int season_id, int dayofweek)
		{
			return new TrainingLimitRecoveryCmd(season_id, dayofweek);
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x001B06D9 File Offset: 0x001AE8D9
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TrainingLimitRecoveryResponse>(__text);
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x001B06E1 File Offset: 0x001AE8E1
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TrainingLimitRecovery";
		}
	}
}
