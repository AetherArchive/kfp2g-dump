using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MyHelperChangeCmd : Command
	{
		private MyHelperChangeCmd()
		{
		}

		private MyHelperChangeCmd(List<MyHelper> helperList)
		{
			this.request = new MyHelperChangeRequest();
			((MyHelperChangeRequest)this.request).helperList = helperList;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "MyHelperChange.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static MyHelperChangeCmd Create(List<MyHelper> helperList)
		{
			return new MyHelperChangeCmd(helperList);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MyHelperChangeResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MyHelperChange";
		}
	}
}
