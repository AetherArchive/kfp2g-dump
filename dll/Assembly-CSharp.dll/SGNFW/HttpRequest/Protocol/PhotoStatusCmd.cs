using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PhotoStatusCmd : Command
	{
		private PhotoStatusCmd()
		{
		}

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

		public static PhotoStatusCmd Create(List<long> lock_photo_id, List<long> lock_clear_photo_id, List<long> img_before_photo_id, List<long> img_after_photo_id, List<long> favorite_photo_id, List<long> favorite_clear_photo_id)
		{
			return new PhotoStatusCmd(lock_photo_id, lock_clear_photo_id, img_before_photo_id, img_after_photo_id, favorite_photo_id, favorite_clear_photo_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PhotoStatusResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PhotoStatus";
		}
	}
}
