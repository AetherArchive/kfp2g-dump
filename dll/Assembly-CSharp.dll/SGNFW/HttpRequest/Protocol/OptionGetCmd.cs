using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000473 RID: 1139
	public class OptionGetCmd : Command
	{
		// Token: 0x06002BAD RID: 11181 RVA: 0x001AD33B File Offset: 0x001AB53B
		private OptionGetCmd()
		{
			this.request = new OptionGetRequest();
			OptionGetRequest optionGetRequest = (OptionGetRequest)this.request;
			this.Setting();
		}

		// Token: 0x06002BAE RID: 11182 RVA: 0x001AD360 File Offset: 0x001AB560
		private void Setting()
		{
			base.Url = "OptionGet.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x001AD3CC File Offset: 0x001AB5CC
		public static OptionGetCmd Create()
		{
			return new OptionGetCmd();
		}

		// Token: 0x06002BB0 RID: 11184 RVA: 0x001AD3D3 File Offset: 0x001AB5D3
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<OptionGetResponse>(__text);
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x001AD3DB File Offset: 0x001AB5DB
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/OptionGet";
		}
	}
}
