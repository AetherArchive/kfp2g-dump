using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200039D RID: 925
	public class CoopInfoCmd : Command
	{
		// Token: 0x060029D0 RID: 10704 RVA: 0x001AA607 File Offset: 0x001A8807
		private CoopInfoCmd()
		{
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x001AA60F File Offset: 0x001A880F
		private CoopInfoCmd(int event_id, int map_id)
		{
			this.request = new CoopInfoRequest();
			CoopInfoRequest coopInfoRequest = (CoopInfoRequest)this.request;
			coopInfoRequest.event_id = event_id;
			coopInfoRequest.map_id = map_id;
			this.Setting();
		}

		// Token: 0x060029D2 RID: 10706 RVA: 0x001AA640 File Offset: 0x001A8840
		private void Setting()
		{
			base.Url = "CoopInfo.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x060029D3 RID: 10707 RVA: 0x001AA6AC File Offset: 0x001A88AC
		public static CoopInfoCmd Create(int event_id, int map_id)
		{
			return new CoopInfoCmd(event_id, map_id);
		}

		// Token: 0x060029D4 RID: 10708 RVA: 0x001AA6B5 File Offset: 0x001A88B5
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CoopInfoResponse>(__text);
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x001AA6BD File Offset: 0x001A88BD
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CoopInfo";
		}
	}
}
