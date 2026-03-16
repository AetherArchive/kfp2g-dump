using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MyHelperListCmd : Command
	{
		private MyHelperListCmd()
		{
			this.request = new MyHelperListRequest();
			MyHelperListRequest myHelperListRequest = (MyHelperListRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "MyHelperList.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static MyHelperListCmd Create()
		{
			return new MyHelperListCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MyHelperListResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MyHelperList";
		}
	}
}
