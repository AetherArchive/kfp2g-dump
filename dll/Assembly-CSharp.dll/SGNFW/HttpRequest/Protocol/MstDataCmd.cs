using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MstDataCmd : Command
	{
		private MstDataCmd()
		{
		}

		private MstDataCmd(string type, int dmm_viewer_id)
		{
			this.request = new MstDataRequest();
			MstDataRequest mstDataRequest = (MstDataRequest)this.request;
			mstDataRequest.type = type;
			mstDataRequest.dmm_viewer_id = dmm_viewer_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "common/MstData.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		public static MstDataCmd Create(string type, int dmm_viewer_id)
		{
			return new MstDataCmd(type, dmm_viewer_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MstDataResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MstData";
		}
	}
}
