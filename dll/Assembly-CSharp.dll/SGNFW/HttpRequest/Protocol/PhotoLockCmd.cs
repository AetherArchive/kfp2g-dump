using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PhotoLockCmd : Command
	{
		private PhotoLockCmd(List<long> lock_photo_id, List<long> lock_clear_photo_id)
		{
			this.request = new PhotoLockRequest();
			PhotoLockRequest photoLockRequest = (PhotoLockRequest)this.request;
			photoLockRequest.lock_photo_id = lock_photo_id;
			photoLockRequest.lock_clear_photo_id = lock_clear_photo_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PhotoLock.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PhotoLockCmd Create(List<long> lock_photo_id, List<long> lock_clear_photo_id)
		{
			return new PhotoLockCmd(lock_photo_id, lock_clear_photo_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PhotoLockResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PhotoLock";
		}
	}
}
