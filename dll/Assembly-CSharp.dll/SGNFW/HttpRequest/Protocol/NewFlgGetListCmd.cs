using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class NewFlgGetListCmd : Command
	{
		private NewFlgGetListCmd()
		{
		}

		private NewFlgGetListCmd(int category)
		{
			this.request = new NewFlgGetListRequest();
			((NewFlgGetListRequest)this.request).category = category;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "NewFlgGetList.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static NewFlgGetListCmd Create(int category)
		{
			return new NewFlgGetListCmd(category);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<NewFlgGetListResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/NewFlgGetList";
		}
	}
}
