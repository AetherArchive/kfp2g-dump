using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000541 RID: 1345
	public class TrainingJoinTrialCmd : Command
	{
		// Token: 0x06002D98 RID: 11672 RVA: 0x001B056E File Offset: 0x001AE76E
		private TrainingJoinTrialCmd(int trial_id)
		{
			this.request = new TrainingJoinTrialParamRequest();
			((TrainingJoinTrialParamRequest)this.request).trial_id = trial_id;
			this.Setting();
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x001B0598 File Offset: 0x001AE798
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

		// Token: 0x06002D9A RID: 11674 RVA: 0x001B0604 File Offset: 0x001AE804
		public static TrainingJoinTrialCmd Create(int trial_id)
		{
			return new TrainingJoinTrialCmd(trial_id);
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x001B060C File Offset: 0x001AE80C
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TrainingJoinTrialParamResponse>(__text);
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x001B0614 File Offset: 0x001AE814
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TrainingJoinTrial";
		}
	}
}
