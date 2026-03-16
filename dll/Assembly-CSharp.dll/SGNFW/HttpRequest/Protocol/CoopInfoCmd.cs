using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CoopInfoCmd : Command
	{
		private CoopInfoCmd()
		{
		}

		private CoopInfoCmd(int event_id, int map_id)
		{
			this.request = new CoopInfoRequest();
			CoopInfoRequest coopInfoRequest = (CoopInfoRequest)this.request;
			coopInfoRequest.event_id = event_id;
			coopInfoRequest.map_id = map_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CoopInfo.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CoopInfoCmd Create(int event_id, int map_id)
		{
			return new CoopInfoCmd(event_id, map_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CoopInfoResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CoopInfo";
		}
	}
}
