using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200055B RID: 1371
	public class TutorialSkipCmd : Command
	{
		// Token: 0x06002DD3 RID: 11731 RVA: 0x001B0B3F File Offset: 0x001AED3F
		private TutorialSkipCmd()
		{
			this.request = new TutorialSkipRequest();
			this.Setting();
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x001B0B58 File Offset: 0x001AED58
		private void Setting()
		{
			base.Url = "TutorialSkip.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x001B0BC4 File Offset: 0x001AEDC4
		public static TutorialSkipCmd Create()
		{
			return new TutorialSkipCmd();
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x001B0BCB File Offset: 0x001AEDCB
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TutorialSkipResponse>(__text);
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x001B0BD3 File Offset: 0x001AEDD3
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TutorialSkip";
		}
	}
}
