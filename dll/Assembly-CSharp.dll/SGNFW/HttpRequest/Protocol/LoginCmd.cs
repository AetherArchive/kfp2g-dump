using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class LoginCmd : Command
	{
		private LoginCmd()
		{
		}

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

		public static LoginCmd Create(string uuid, string secure_id, string version, int lang, string device, string device_id, string signature)
		{
			return new LoginCmd(uuid, secure_id, version, lang, device, device_id, signature);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<LoginResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/Login";
		}
	}
}
