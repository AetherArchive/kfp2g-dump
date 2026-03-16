using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class StatusCheckCmd : Command
	{
		private StatusCheckCmd()
		{
		}

		private StatusCheckCmd(string uuid, string version, int dmm_viewer_id)
		{
			this.request = new StatusCheckRequest();
			StatusCheckRequest statusCheckRequest = (StatusCheckRequest)this.request;
			statusCheckRequest.uuid = uuid;
			statusCheckRequest.version = version;
			statusCheckRequest.dmm_viewer_id = dmm_viewer_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "common/StatusCheck.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static StatusCheckCmd Create(string uuid, string version, int dmm_viewer_id)
		{
			return new StatusCheckCmd(uuid, version, dmm_viewer_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<StatusCheckResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/StatusCheck";
		}
	}
}
