using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003A7 RID: 935
	public class DbgLoginCmd : Command
	{
		// Token: 0x060029E4 RID: 10724 RVA: 0x001AA81B File Offset: 0x001A8A1B
		private DbgLoginCmd()
		{
		}

		// Token: 0x060029E5 RID: 10725 RVA: 0x001AA823 File Offset: 0x001A8A23
		private DbgLoginCmd(string secure_id, string version, int lang)
		{
			this.request = new DbgLoginRequest();
			DbgLoginRequest dbgLoginRequest = (DbgLoginRequest)this.request;
			dbgLoginRequest.secure_id = secure_id;
			dbgLoginRequest.version = version;
			dbgLoginRequest.lang = lang;
			this.Setting();
		}

		// Token: 0x060029E6 RID: 10726 RVA: 0x001AA85C File Offset: 0x001A8A5C
		private void Setting()
		{
			base.Url = "dbg/DbgLogin.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x001AA8C8 File Offset: 0x001A8AC8
		public static DbgLoginCmd Create(string secure_id, string version, int lang)
		{
			return new DbgLoginCmd(secure_id, version, lang);
		}

		// Token: 0x060029E8 RID: 10728 RVA: 0x001AA8D2 File Offset: 0x001A8AD2
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<DbgLoginResponse>(__text);
		}

		// Token: 0x060029E9 RID: 10729 RVA: 0x001AA8DA File Offset: 0x001A8ADA
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/DbgLogin";
		}
	}
}
