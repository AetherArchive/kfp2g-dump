using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000418 RID: 1048
	public class LoginCmd : Command
	{
		// Token: 0x06002AE0 RID: 10976 RVA: 0x001ABF67 File Offset: 0x001AA167
		private LoginCmd()
		{
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x001ABF70 File Offset: 0x001AA170
		private LoginCmd(string uuid, string secure_id, string version, int lang, string device, string device_id, string signature)
		{
			this.request = new LoginRequest();
			LoginRequest loginRequest = (LoginRequest)this.request;
			loginRequest.uuid = uuid;
			loginRequest.secure_id = secure_id;
			loginRequest.version = version;
			loginRequest.lang = lang;
			loginRequest.device = device;
			loginRequest.device_id = device_id;
			loginRequest.signature = signature;
			this.Setting();
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x001ABFD4 File Offset: 0x001AA1D4
		private void Setting()
		{
			base.Url = "Login.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		// Token: 0x06002AE3 RID: 10979 RVA: 0x001AC040 File Offset: 0x001AA240
		public static LoginCmd Create(string uuid, string secure_id, string version, int lang, string device, string device_id, string signature)
		{
			return new LoginCmd(uuid, secure_id, version, lang, device, device_id, signature);
		}

		// Token: 0x06002AE4 RID: 10980 RVA: 0x001AC051 File Offset: 0x001AA251
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<LoginResponse>(__text);
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x001AC059 File Offset: 0x001AA259
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/Login";
		}
	}
}
