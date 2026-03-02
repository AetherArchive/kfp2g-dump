using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000396 RID: 918
	public class CollaboURLCmd : Command
	{
		// Token: 0x060029C0 RID: 10688 RVA: 0x001AA3CD File Offset: 0x001A85CD
		private CollaboURLCmd()
		{
			this.request = new CollaboURLRequest();
			CollaboURLRequest collaboURLRequest = (CollaboURLRequest)this.request;
			this.Setting();
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x001AA3F4 File Offset: 0x001A85F4
		private void Setting()
		{
			base.Url = "CollaboUrl.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x001AA460 File Offset: 0x001A8660
		public static CollaboURLCmd Create()
		{
			return new CollaboURLCmd();
		}

		// Token: 0x060029C3 RID: 10691 RVA: 0x001AA467 File Offset: 0x001A8667
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CollaboURLResponce>(__text);
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x001AA46F File Offset: 0x001A866F
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CollaboURL";
		}
	}
}
