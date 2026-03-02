using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000468 RID: 1128
	public class MyHelperListCmd : Command
	{
		// Token: 0x06002B94 RID: 11156 RVA: 0x001AD0E7 File Offset: 0x001AB2E7
		private MyHelperListCmd()
		{
			this.request = new MyHelperListRequest();
			MyHelperListRequest myHelperListRequest = (MyHelperListRequest)this.request;
			this.Setting();
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x001AD10C File Offset: 0x001AB30C
		private void Setting()
		{
			base.Url = "MyHelperList.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x001AD178 File Offset: 0x001AB378
		public static MyHelperListCmd Create()
		{
			return new MyHelperListCmd();
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x001AD17F File Offset: 0x001AB37F
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MyHelperListResponse>(__text);
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x001AD187 File Offset: 0x001AB387
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MyHelperList";
		}
	}
}
