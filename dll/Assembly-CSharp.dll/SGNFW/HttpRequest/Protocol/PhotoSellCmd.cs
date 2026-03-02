using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200048B RID: 1163
	public class PhotoSellCmd : Command
	{
		// Token: 0x06002BE4 RID: 11236 RVA: 0x001AD89B File Offset: 0x001ABA9B
		private PhotoSellCmd()
		{
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x001AD8A3 File Offset: 0x001ABAA3
		private PhotoSellCmd(List<long> photo_id)
		{
			this.request = new PhotoSellRequest();
			((PhotoSellRequest)this.request).photo_id = photo_id;
			this.Setting();
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x001AD8D0 File Offset: 0x001ABAD0
		private void Setting()
		{
			base.Url = "PhotoSell.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002BE7 RID: 11239 RVA: 0x001AD93C File Offset: 0x001ABB3C
		public static PhotoSellCmd Create(List<long> photo_id)
		{
			return new PhotoSellCmd(photo_id);
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x001AD944 File Offset: 0x001ABB44
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PhotoSellResponse>(__text);
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x001AD94C File Offset: 0x001ABB4C
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PhotoSell";
		}
	}
}
