using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200047B RID: 1147
	public class PhotoCmd : Command
	{
		// Token: 0x06002BBE RID: 11198 RVA: 0x001AD4C7 File Offset: 0x001AB6C7
		private PhotoCmd()
		{
			this.request = new PhotoRequest();
			PhotoRequest photoRequest = (PhotoRequest)this.request;
			this.Setting();
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x001AD4EC File Offset: 0x001AB6EC
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

		// Token: 0x06002BC0 RID: 11200 RVA: 0x001AD558 File Offset: 0x001AB758
		public static PhotoCmd Create()
		{
			return new PhotoCmd();
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x001AD55F File Offset: 0x001AB75F
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PhotoResponse>(__text);
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x001AD567 File Offset: 0x001AB767
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/Photo";
		}
	}
}
