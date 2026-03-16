using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class RegistAccountCmd : Command
	{
		private RegistAccountCmd()
		{
		}

		private RegistAccountCmd(string device, string signature, int dmm_viewer_id)
		{
			this.request = new RegistAccountRequest();
			RegistAccountRequest registAccountRequest = (RegistAccountRequest)this.request;
			registAccountRequest.device = device;
			registAccountRequest.signature = signature;
			registAccountRequest.dmm_viewer_id = dmm_viewer_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "RegistAccount.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 5f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		public static RegistAccountCmd Create(string device, string signature, int dmm_viewer_id)
		{
			return new RegistAccountCmd(device, signature, dmm_viewer_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<RegistAccountResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/RegistAccount";
		}
	}
}
