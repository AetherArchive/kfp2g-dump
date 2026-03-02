using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003D9 RID: 985
	public class GachaRateViewCmd : Command
	{
		// Token: 0x06002A4E RID: 10830 RVA: 0x001AB1C1 File Offset: 0x001A93C1
		private GachaRateViewCmd()
		{
		}

		// Token: 0x06002A4F RID: 10831 RVA: 0x001AB1C9 File Offset: 0x001A93C9
		private GachaRateViewCmd(int gacha_id)
		{
			this.request = new GachaRateViewRequest();
			((GachaRateViewRequest)this.request).gacha_id = gacha_id;
			this.Setting();
		}

		// Token: 0x06002A50 RID: 10832 RVA: 0x001AB1F4 File Offset: 0x001A93F4
		private void Setting()
		{
			base.Url = "GachaRateView.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002A51 RID: 10833 RVA: 0x001AB260 File Offset: 0x001A9460
		public static GachaRateViewCmd Create(int gacha_id)
		{
			return new GachaRateViewCmd(gacha_id);
		}

		// Token: 0x06002A52 RID: 10834 RVA: 0x001AB268 File Offset: 0x001A9468
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<GachaRateViewResponse>(__text);
		}

		// Token: 0x06002A53 RID: 10835 RVA: 0x001AB270 File Offset: 0x001A9470
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/GachaRateView";
		}
	}
}
