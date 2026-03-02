using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200048E RID: 1166
	public class PhotoStatusCmd : Command
	{
		// Token: 0x06002BEC RID: 11244 RVA: 0x001AD963 File Offset: 0x001ABB63
		private PhotoStatusCmd()
		{
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x001AD96C File Offset: 0x001ABB6C
		private PhotoStatusCmd(List<long> lock_photo_id, List<long> lock_clear_photo_id, List<long> img_before_photo_id, List<long> img_after_photo_id, List<long> favorite_photo_id, List<long> favorite_clear_photo_id)
		{
			this.request = new PhotoStatusRequest();
			PhotoStatusRequest photoStatusRequest = (PhotoStatusRequest)this.request;
			photoStatusRequest.lock_photo_id = lock_photo_id;
			photoStatusRequest.lock_clear_photo_id = lock_clear_photo_id;
			photoStatusRequest.img_before_photo_id = img_before_photo_id;
			photoStatusRequest.img_after_photo_id = img_after_photo_id;
			photoStatusRequest.favorite_photo_id = favorite_photo_id;
			photoStatusRequest.favorite_clear_photo_id = favorite_clear_photo_id;
			this.Setting();
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x001AD9C8 File Offset: 0x001ABBC8
		private void Setting()
		{
			base.Url = "PhotoStatus.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x001ADA34 File Offset: 0x001ABC34
		public static PhotoStatusCmd Create(List<long> lock_photo_id, List<long> lock_clear_photo_id, List<long> img_before_photo_id, List<long> img_after_photo_id, List<long> favorite_photo_id, List<long> favorite_clear_photo_id)
		{
			return new PhotoStatusCmd(lock_photo_id, lock_clear_photo_id, img_before_photo_id, img_after_photo_id, favorite_photo_id, favorite_clear_photo_id);
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x001ADA43 File Offset: 0x001ABC43
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PhotoStatusResponse>(__text);
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x001ADA4B File Offset: 0x001ABC4B
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PhotoStatus";
		}
	}
}
