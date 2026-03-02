using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000367 RID: 871
	public class CharaCmd : Command
	{
		// Token: 0x06002947 RID: 10567 RVA: 0x001A9768 File Offset: 0x001A7968
		private CharaCmd()
		{
			this.request = new CharaRequest();
			CharaRequest charaRequest = (CharaRequest)this.request;
			this.Setting();
		}

		// Token: 0x06002948 RID: 10568 RVA: 0x001A9790 File Offset: 0x001A7990
		private void Setting()
		{
			base.Url = "Chara.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002949 RID: 10569 RVA: 0x001A97FC File Offset: 0x001A79FC
		public static CharaCmd Create()
		{
			return new CharaCmd();
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x001A9803 File Offset: 0x001A7A03
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaResponse>(__text);
		}

		// Token: 0x0600294B RID: 10571 RVA: 0x001A980B File Offset: 0x001A7A0B
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/Chara";
		}
	}
}
