using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200053E RID: 1342
	public class TrainingInfoCmd : Command
	{
		// Token: 0x06002D91 RID: 11665 RVA: 0x001B04B5 File Offset: 0x001AE6B5
		private TrainingInfoCmd()
		{
			this.request = new TrainingInfoRequest();
			TrainingInfoRequest trainingInfoRequest = (TrainingInfoRequest)this.request;
			this.Setting();
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x001B04DC File Offset: 0x001AE6DC
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

		// Token: 0x06002D93 RID: 11667 RVA: 0x001B0548 File Offset: 0x001AE748
		public static TrainingInfoCmd Create()
		{
			return new TrainingInfoCmd();
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x001B054F File Offset: 0x001AE74F
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TrainingInfoResponse>(__text);
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x001B0557 File Offset: 0x001AE757
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TrainingInfo";
		}
	}
}
