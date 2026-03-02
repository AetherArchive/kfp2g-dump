using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004BC RID: 1212
	public class PresentBoxCmd : Command
	{
		// Token: 0x06002C59 RID: 11353 RVA: 0x001AE3D3 File Offset: 0x001AC5D3
		private PresentBoxCmd()
		{
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x001AE3DB File Offset: 0x001AC5DB
		private PresentBoxCmd(int rangeLow, int rangeHigh)
		{
			this.request = new PresentBoxRequest();
			PresentBoxRequest presentBoxRequest = (PresentBoxRequest)this.request;
			presentBoxRequest.rangeLow = rangeLow;
			presentBoxRequest.rangeHigh = rangeHigh;
			this.Setting();
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x001AE40C File Offset: 0x001AC60C
		private void Setting()
		{
			base.Url = "PresentBox.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002C5C RID: 11356 RVA: 0x001AE478 File Offset: 0x001AC678
		public static PresentBoxCmd Create(int rangeLow, int rangeHigh)
		{
			return new PresentBoxCmd(rangeLow, rangeHigh);
		}

		// Token: 0x06002C5D RID: 11357 RVA: 0x001AE481 File Offset: 0x001AC681
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PresentBoxResponse>(__text);
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x001AE489 File Offset: 0x001AC689
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PresentBox";
		}
	}
}
