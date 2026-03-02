using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000485 RID: 1157
	public class PhotoPictureBookCmd : Command
	{
		// Token: 0x06002BD5 RID: 11221 RVA: 0x001AD71C File Offset: 0x001AB91C
		private PhotoPictureBookCmd()
		{
			this.request = new PhotoPictureBookRequest();
			PhotoPictureBookRequest photoPictureBookRequest = (PhotoPictureBookRequest)this.request;
			this.Setting();
		}

		// Token: 0x06002BD6 RID: 11222 RVA: 0x001AD744 File Offset: 0x001AB944
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

		// Token: 0x06002BD7 RID: 11223 RVA: 0x001AD7B0 File Offset: 0x001AB9B0
		public static PhotoPictureBookCmd Create()
		{
			return new PhotoPictureBookCmd();
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x001AD7B7 File Offset: 0x001AB9B7
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PhotoPictureBookResponse>(__text);
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x001AD7BF File Offset: 0x001AB9BF
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PhotoPictureBook";
		}
	}
}
