using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000482 RID: 1154
	public class PhotoLockCmd : Command
	{
		// Token: 0x06002BCE RID: 11214 RVA: 0x001AD654 File Offset: 0x001AB854
		private PhotoLockCmd(List<long> lock_photo_id, List<long> lock_clear_photo_id)
		{
			this.request = new PhotoLockRequest();
			PhotoLockRequest photoLockRequest = (PhotoLockRequest)this.request;
			photoLockRequest.lock_photo_id = lock_photo_id;
			photoLockRequest.lock_clear_photo_id = lock_clear_photo_id;
			this.Setting();
		}

		// Token: 0x06002BCF RID: 11215 RVA: 0x001AD688 File Offset: 0x001AB888
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

		// Token: 0x06002BD0 RID: 11216 RVA: 0x001AD6F4 File Offset: 0x001AB8F4
		public static PhotoLockCmd Create(List<long> lock_photo_id, List<long> lock_clear_photo_id)
		{
			return new PhotoLockCmd(lock_photo_id, lock_clear_photo_id);
		}

		// Token: 0x06002BD1 RID: 11217 RVA: 0x001AD6FD File Offset: 0x001AB8FD
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PhotoLockResponse>(__text);
		}

		// Token: 0x06002BD2 RID: 11218 RVA: 0x001AD705 File Offset: 0x001AB905
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PhotoLock";
		}
	}
}
