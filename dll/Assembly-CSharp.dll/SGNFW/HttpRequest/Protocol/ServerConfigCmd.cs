using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ServerConfigCmd : Command
	{
		private ServerConfigCmd()
		{
		}

		private ServerConfigCmd(int dmm_viewer_id)
		{
			this.request = new ServerConfigRequest();
			((ServerConfigRequest)this.request).dmm_viewer_id = dmm_viewer_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "common/ServerConfig.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 5f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		public static ServerConfigCmd Create(int dmm_viewer_id)
		{
			return new ServerConfigCmd(dmm_viewer_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ServerConfigResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ServerConfig";
		}
	}
}
