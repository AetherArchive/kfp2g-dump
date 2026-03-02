using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200047E RID: 1150
	public class PhotoGrowCmd : Command
	{
		// Token: 0x06002BC5 RID: 11205 RVA: 0x001AD57E File Offset: 0x001AB77E
		private PhotoGrowCmd()
		{
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x001AD586 File Offset: 0x001AB786
		private PhotoGrowCmd(long photo_id, List<long> materials)
		{
			this.request = new PhotoGrowRequest();
			PhotoGrowRequest photoGrowRequest = (PhotoGrowRequest)this.request;
			photoGrowRequest.photo_id = photo_id;
			photoGrowRequest.materials = materials;
			this.Setting();
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x001AD5B8 File Offset: 0x001AB7B8
		private void Setting()
		{
			base.Url = "PhotoGrow.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002BC8 RID: 11208 RVA: 0x001AD624 File Offset: 0x001AB824
		public static PhotoGrowCmd Create(long photo_id, List<long> materials)
		{
			return new PhotoGrowCmd(photo_id, materials);
		}

		// Token: 0x06002BC9 RID: 11209 RVA: 0x001AD62D File Offset: 0x001AB82D
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PhotoGrowResponse>(__text);
		}

		// Token: 0x06002BCA RID: 11210 RVA: 0x001AD635 File Offset: 0x001AB835
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PhotoGrow";
		}
	}
}
