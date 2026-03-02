using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200040E RID: 1038
	public class KemostatusRankingCmd : Command
	{
		// Token: 0x06002ACC RID: 10956 RVA: 0x001ABDB8 File Offset: 0x001A9FB8
		private KemostatusRankingCmd()
		{
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x001ABDC0 File Offset: 0x001A9FC0
		private KemostatusRankingCmd(long last_update_time)
		{
			this.request = new KemostatusRankingRequest();
			((KemostatusRankingRequest)this.request).last_update_time = last_update_time;
			this.Setting();
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x001ABDEC File Offset: 0x001A9FEC
		private void Setting()
		{
			base.Url = "KemostatusRanking.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x001ABE58 File Offset: 0x001AA058
		public static KemostatusRankingCmd Create(long last_update_time)
		{
			return new KemostatusRankingCmd(last_update_time);
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x001ABE60 File Offset: 0x001AA060
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<KemostatusRankingResponse>(__text);
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x001ABE68 File Offset: 0x001AA068
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/KemostatusRanking";
		}
	}
}
