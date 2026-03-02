using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004B5 RID: 1205
	public class PlayVideoCmd : Command
	{
		// Token: 0x06002C48 RID: 11336 RVA: 0x001AE23B File Offset: 0x001AC43B
		private PlayVideoCmd()
		{
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x001AE243 File Offset: 0x001AC443
		private PlayVideoCmd(int video_id)
		{
			this.request = new PlayVideoRequest();
			((PlayVideoRequest)this.request).video_id = video_id;
			this.Setting();
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x001AE270 File Offset: 0x001AC470
		private void Setting()
		{
			base.Url = "PlayVideo.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x001AE2DC File Offset: 0x001AC4DC
		public static PlayVideoCmd Create(int video_id)
		{
			return new PlayVideoCmd(video_id);
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x001AE2E4 File Offset: 0x001AC4E4
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PlayVideoResponse>(__text);
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x001AE2EC File Offset: 0x001AC4EC
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PlayVideo";
		}
	}
}
