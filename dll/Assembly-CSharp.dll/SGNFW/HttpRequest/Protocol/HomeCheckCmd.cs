using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class HomeCheckCmd : Command
	{
		private HomeCheckCmd()
		{
		}

		private HomeCheckCmd(int kemostatus)
		{
			this.request = new HomeCheckRequest();
			((HomeCheckRequest)this.request).kemostatus = kemostatus;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "HomeCheck.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static HomeCheckCmd Create(int kemostatus)
		{
			return new HomeCheckCmd(kemostatus);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<HomeCheckResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/HomeCheck";
		}
	}
}
