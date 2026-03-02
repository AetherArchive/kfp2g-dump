using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000352 RID: 850
	public class AtomRewardCmd : Command
	{
		// Token: 0x06002914 RID: 10516 RVA: 0x001A9286 File Offset: 0x001A7486
		private AtomRewardCmd()
		{
		}

		// Token: 0x06002915 RID: 10517 RVA: 0x001A928E File Offset: 0x001A748E
		private AtomRewardCmd(string url_scheme, string noah_id)
		{
			this.request = new AtomRewardRequest();
			AtomRewardRequest atomRewardRequest = (AtomRewardRequest)this.request;
			atomRewardRequest.url_scheme = url_scheme;
			atomRewardRequest.noah_id = noah_id;
			this.Setting();
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x001A92C0 File Offset: 0x001A74C0
		private void Setting()
		{
			base.Url = "atom/AtomReward.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x001A932C File Offset: 0x001A752C
		public static AtomRewardCmd Create(string url_scheme, string noah_id)
		{
			return new AtomRewardCmd(url_scheme, noah_id);
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x001A9335 File Offset: 0x001A7535
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AtomRewardResponse>(__text);
		}

		// Token: 0x06002919 RID: 10521 RVA: 0x001A933D File Offset: 0x001A753D
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AtomReward";
		}
	}
}
