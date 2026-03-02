using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200034F RID: 847
	public class AtomInviteCmd : Command
	{
		// Token: 0x0600290D RID: 10509 RVA: 0x001A91CE File Offset: 0x001A73CE
		private AtomInviteCmd()
		{
			this.request = new AtomInviteRequest();
			AtomInviteRequest atomInviteRequest = (AtomInviteRequest)this.request;
			this.Setting();
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x001A91F4 File Offset: 0x001A73F4
		private void Setting()
		{
			base.Url = "atom/AtomInvite.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x001A9260 File Offset: 0x001A7460
		public static AtomInviteCmd Create()
		{
			return new AtomInviteCmd();
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x001A9267 File Offset: 0x001A7467
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AtomInviteResponse>(__text);
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x001A926F File Offset: 0x001A746F
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AtomInvite";
		}
	}
}
