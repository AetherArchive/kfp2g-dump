using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class GetUrlCmd : Command
	{
		private GetUrlCmd()
		{
		}

		private GetUrlCmd(string version, int dmm_viewer_id)
		{
			this.request = new GetUrlRequest();
			GetUrlRequest getUrlRequest = (GetUrlRequest)this.request;
			getUrlRequest.version = version;
			getUrlRequest.dmm_viewer_id = dmm_viewer_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "common/GetUrl.do";
			base.Server = Manager.ServerRoot["root"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 30f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		public static GetUrlCmd Create(string version, int dmm_viewer_id)
		{
			return new GetUrlCmd(version, dmm_viewer_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<GetUrlResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/GetUrl";
		}
	}
}
