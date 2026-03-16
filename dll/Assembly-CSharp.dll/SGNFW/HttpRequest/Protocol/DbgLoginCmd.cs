using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class DbgLoginCmd : Command
	{
		private DbgLoginCmd()
		{
		}

		private DbgLoginCmd(string secure_id, string version, int lang)
		{
			this.request = new DbgLoginRequest();
			DbgLoginRequest dbgLoginRequest = (DbgLoginRequest)this.request;
			dbgLoginRequest.secure_id = secure_id;
			dbgLoginRequest.version = version;
			dbgLoginRequest.lang = lang;
			this.Setting();
		}

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

		public static DbgLoginCmd Create(string secure_id, string version, int lang)
		{
			return new DbgLoginCmd(secure_id, version, lang);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<DbgLoginResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/DbgLogin";
		}
	}
}
