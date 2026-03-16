using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class FreezeAccountCmd : Command
	{
		private FreezeAccountCmd()
		{
			this.request = new FreezeAccountRequest();
			FreezeAccountRequest freezeAccountRequest = (FreezeAccountRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "FreezeAccount.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static FreezeAccountCmd Create()
		{
			return new FreezeAccountCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<FreezeAccountResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/FreezeAccount";
		}
	}
}
