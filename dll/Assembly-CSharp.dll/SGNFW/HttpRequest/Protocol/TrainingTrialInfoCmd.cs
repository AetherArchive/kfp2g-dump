using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000557 RID: 1367
	public class TrainingTrialInfoCmd : Command
	{
		// Token: 0x06002DCB RID: 11723 RVA: 0x001B0A78 File Offset: 0x001AEC78
		private TrainingTrialInfoCmd(int trial_id)
		{
			this.request = new TrainingTrialInfoParamRequest();
			((TrainingTrialInfoParamRequest)this.request).trial_id = trial_id;
			this.Setting();
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x001B0AA4 File Offset: 0x001AECA4
		private void Setting()
		{
			base.Url = "TrainingTrialInfo.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x001B0B10 File Offset: 0x001AED10
		public static TrainingTrialInfoCmd Create(int trial_id)
		{
			return new TrainingTrialInfoCmd(trial_id);
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x001B0B18 File Offset: 0x001AED18
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TrainingTrialInfoParamResponse>(__text);
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x001B0B20 File Offset: 0x001AED20
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TrainingTrialInfo";
		}
	}
}
