using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PhotoPictureBookCmd : Command
	{
		private PhotoPictureBookCmd()
		{
			this.request = new PhotoPictureBookRequest();
			PhotoPictureBookRequest photoPictureBookRequest = (PhotoPictureBookRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PhotoPictureBook.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PhotoPictureBookCmd Create()
		{
			return new PhotoPictureBookCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PhotoPictureBookResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PhotoPictureBook";
		}
	}
}
