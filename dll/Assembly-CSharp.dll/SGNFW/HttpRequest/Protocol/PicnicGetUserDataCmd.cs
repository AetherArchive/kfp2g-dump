using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000497 RID: 1175
	public class PicnicGetUserDataCmd : Command
	{
		// Token: 0x06002BFF RID: 11263 RVA: 0x001ADB3F File Offset: 0x001ABD3F
		private PicnicGetUserDataCmd()
		{
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x001ADB47 File Offset: 0x001ABD47
		private PicnicGetUserDataCmd(int isSkipUpdata)
		{
			this.request = new PicnicGetUserDataRequest();
			((PicnicGetUserDataRequest)this.request).isSkipUpdata = isSkipUpdata;
			this.Setting();
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x001ADB74 File Offset: 0x001ABD74
		private void Setting()
		{
			base.Url = "PicnicGetUserData.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x001ADBE0 File Offset: 0x001ABDE0
		public static PicnicGetUserDataCmd Create(int isSkipUpdata)
		{
			return new PicnicGetUserDataCmd(isSkipUpdata);
		}

		// Token: 0x06002C03 RID: 11267 RVA: 0x001ADBE8 File Offset: 0x001ABDE8
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PicnicGetUserDataResponse>(__text);
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x001ADBF0 File Offset: 0x001ABDF0
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PicnicGetUserData";
		}
	}
}
