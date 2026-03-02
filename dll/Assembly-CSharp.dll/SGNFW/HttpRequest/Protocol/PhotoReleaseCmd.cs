using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000488 RID: 1160
	public class PhotoReleaseCmd : Command
	{
		// Token: 0x06002BDC RID: 11228 RVA: 0x001AD7D6 File Offset: 0x001AB9D6
		private PhotoReleaseCmd()
		{
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x001AD7DE File Offset: 0x001AB9DE
		private PhotoReleaseCmd(List<long> photoIdList)
		{
			this.request = new PhotoReleaseRequest();
			((PhotoReleaseRequest)this.request).photoIdList = photoIdList;
			this.Setting();
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x001AD808 File Offset: 0x001ABA08
		private void Setting()
		{
			base.Url = "PhotoRelease.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x001AD874 File Offset: 0x001ABA74
		public static PhotoReleaseCmd Create(List<long> photoIdList)
		{
			return new PhotoReleaseCmd(photoIdList);
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x001AD87C File Offset: 0x001ABA7C
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PhotoReleaseResponse>(__text);
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x001AD884 File Offset: 0x001ABA84
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PhotoRelease";
		}
	}
}
