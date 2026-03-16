using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class GPGLoginCmd : Command
	{
		private GPGLoginCmd()
		{
		}

		private GPGLoginCmd(string gpg_player_id, string auth_code)
		{
			this.request = new GPGLoginRequest();
			GPGLoginRequest gpgloginRequest = (GPGLoginRequest)this.request;
			gpgloginRequest.gpg_player_id = gpg_player_id;
			gpgloginRequest.auth_code = auth_code;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "common/GPGLogin.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static GPGLoginCmd Create(string gpg_player_id, string auth_code)
		{
			return new GPGLoginCmd(gpg_player_id, auth_code);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<GPGLoginResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/GPGLogin";
		}

		public enum RESULT_CODE
		{
			INVALID,
			SUCCESS_NO_DATA_NO_TRANSFER,
			SUCCESS_NO_DATA_YES_TRANSFER,
			SUCCESS_YES_DATA_NO_TRANSFER,
			SUCCESS_YES_DATA_YES_TRANSFER
		}
	}
}
