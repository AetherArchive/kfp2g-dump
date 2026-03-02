using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003E2 RID: 994
	public class GetGrowthEventCharaIdCmd : Command
	{
		// Token: 0x06002A61 RID: 10849 RVA: 0x001AB367 File Offset: 0x001A9567
		private GetGrowthEventCharaIdCmd()
		{
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x001AB36F File Offset: 0x001A956F
		private GetGrowthEventCharaIdCmd(int event_id)
		{
			this.request = new GetGrowthEventCharaIdRequest();
			((GetGrowthEventCharaIdRequest)this.request).event_id = event_id;
			this.Setting();
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x001AB39C File Offset: 0x001A959C
		private void Setting()
		{
			base.Url = "GetGrowthEventCharaId.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x001AB408 File Offset: 0x001A9608
		public static GetGrowthEventCharaIdCmd Create(int event_id)
		{
			return new GetGrowthEventCharaIdCmd(event_id);
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x001AB410 File Offset: 0x001A9610
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<GetGrowthEventCharaIdResponse>(__text);
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x001AB418 File Offset: 0x001A9618
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/GetGrowthEventCharaId";
		}
	}
}
