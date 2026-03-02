using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200041B RID: 1051
	public class LoginDmmCmd : Command
	{
		// Token: 0x06002AE8 RID: 10984 RVA: 0x001AC070 File Offset: 0x001AA270
		private LoginDmmCmd()
		{
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x001AC078 File Offset: 0x001AA278
		private LoginDmmCmd(string version, int lang, string device, string signature, int dmm_viewer_id, string onetime_token)
		{
			this.request = new LoginDmmRequest();
			LoginDmmRequest loginDmmRequest = (LoginDmmRequest)this.request;
			loginDmmRequest.version = version;
			loginDmmRequest.lang = lang;
			loginDmmRequest.device = device;
			loginDmmRequest.signature = signature;
			loginDmmRequest.dmm_viewer_id = dmm_viewer_id;
			loginDmmRequest.onetime_token = onetime_token;
			this.Setting();
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x001AC0D4 File Offset: 0x001AA2D4
		private void Setting()
		{
			base.Url = "LoginDmm.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x001AC140 File Offset: 0x001AA340
		public static LoginDmmCmd Create(string version, int lang, string device, string signature, int dmm_viewer_id, string onetime_token)
		{
			return new LoginDmmCmd(version, lang, device, signature, dmm_viewer_id, onetime_token);
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x001AC14F File Offset: 0x001AA34F
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<LoginDmmResponse>(__text);
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x001AC157 File Offset: 0x001AA357
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/LoginDmm";
		}
	}
}
