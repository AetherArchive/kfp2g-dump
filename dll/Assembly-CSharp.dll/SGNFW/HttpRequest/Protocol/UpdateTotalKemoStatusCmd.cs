using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class UpdateTotalKemoStatusCmd : Command
	{
		private UpdateTotalKemoStatusCmd()
		{
		}

		private UpdateTotalKemoStatusCmd(int kemostatus)
		{
			this.request = new UpdateTotalKemoStatusRequest();
			((UpdateTotalKemoStatusRequest)this.request).kemostatus = kemostatus;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "UpdateTotalKemoStatus.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static UpdateTotalKemoStatusCmd Create(int kemostatus)
		{
			return new UpdateTotalKemoStatusCmd(kemostatus);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<UpdateTotalKemoStatusResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/UpdateTotalKemoStatus";
		}
	}
}
