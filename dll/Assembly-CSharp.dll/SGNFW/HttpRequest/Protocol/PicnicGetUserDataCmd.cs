using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PicnicGetUserDataCmd : Command
	{
		private PicnicGetUserDataCmd()
		{
		}

		private PicnicGetUserDataCmd(int isSkipUpdata)
		{
			this.request = new PicnicGetUserDataRequest();
			((PicnicGetUserDataRequest)this.request).isSkipUpdata = isSkipUpdata;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PicnicGetUserData.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PicnicGetUserDataCmd Create(int isSkipUpdata)
		{
			return new PicnicGetUserDataCmd(isSkipUpdata);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PicnicGetUserDataResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PicnicGetUserData";
		}
	}
}
