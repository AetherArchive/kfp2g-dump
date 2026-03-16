using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class GetGrowthEventCharaIdCmd : Command
	{
		private GetGrowthEventCharaIdCmd()
		{
		}

		private GetGrowthEventCharaIdCmd(int event_id)
		{
			this.request = new GetGrowthEventCharaIdRequest();
			((GetGrowthEventCharaIdRequest)this.request).event_id = event_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "GetGrowthEventCharaId.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static GetGrowthEventCharaIdCmd Create(int event_id)
		{
			return new GetGrowthEventCharaIdCmd(event_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<GetGrowthEventCharaIdResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/GetGrowthEventCharaId";
		}
	}
}
