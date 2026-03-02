using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003F8 RID: 1016
	public class HomeCheckCmd : Command
	{
		// Token: 0x06002A99 RID: 10905 RVA: 0x001AB8EF File Offset: 0x001A9AEF
		private HomeCheckCmd()
		{
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x001AB8F7 File Offset: 0x001A9AF7
		private HomeCheckCmd(int kemostatus)
		{
			this.request = new HomeCheckRequest();
			((HomeCheckRequest)this.request).kemostatus = kemostatus;
			this.Setting();
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x001AB924 File Offset: 0x001A9B24
		private void Setting()
		{
			base.Url = "HomeCheck.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x001AB990 File Offset: 0x001A9B90
		public static HomeCheckCmd Create(int kemostatus)
		{
			return new HomeCheckCmd(kemostatus);
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x001AB998 File Offset: 0x001A9B98
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<HomeCheckResponse>(__text);
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x001AB9A0 File Offset: 0x001A9BA0
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/HomeCheck";
		}
	}
}
