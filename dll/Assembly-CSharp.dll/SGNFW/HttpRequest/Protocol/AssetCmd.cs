using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200034A RID: 842
	public class AssetCmd : Command
	{
		// Token: 0x06002904 RID: 10500 RVA: 0x001A9107 File Offset: 0x001A7307
		private AssetCmd()
		{
			this.request = new AssetRequest();
			AssetRequest assetRequest = (AssetRequest)this.request;
			this.Setting();
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x001A912C File Offset: 0x001A732C
		private void Setting()
		{
			base.Url = "Asset.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x001A9198 File Offset: 0x001A7398
		public static AssetCmd Create()
		{
			return new AssetCmd();
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x001A919F File Offset: 0x001A739F
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AssetResponse>(__text);
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x001A91A7 File Offset: 0x001A73A7
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/Asset";
		}
	}
}
