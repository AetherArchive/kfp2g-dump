using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PhotoCmd : Command
	{
		private PhotoCmd()
		{
			this.request = new PhotoRequest();
			PhotoRequest photoRequest = (PhotoRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "Photo.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PhotoCmd Create()
		{
			return new PhotoCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PhotoResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/Photo";
		}
	}
}
