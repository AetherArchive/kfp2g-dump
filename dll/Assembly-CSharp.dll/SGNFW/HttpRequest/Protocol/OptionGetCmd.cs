using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class OptionGetCmd : Command
	{
		private OptionGetCmd()
		{
			this.request = new OptionGetRequest();
			OptionGetRequest optionGetRequest = (OptionGetRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "OptionGet.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static OptionGetCmd Create()
		{
			return new OptionGetCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<OptionGetResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/OptionGet";
		}
	}
}
