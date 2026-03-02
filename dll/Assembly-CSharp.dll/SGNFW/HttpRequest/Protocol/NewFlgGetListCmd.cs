using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200046C RID: 1132
	public class NewFlgGetListCmd : Command
	{
		// Token: 0x06002B9C RID: 11164 RVA: 0x001AD1A6 File Offset: 0x001AB3A6
		private NewFlgGetListCmd()
		{
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x001AD1AE File Offset: 0x001AB3AE
		private NewFlgGetListCmd(int category)
		{
			this.request = new NewFlgGetListRequest();
			((NewFlgGetListRequest)this.request).category = category;
			this.Setting();
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x001AD1D8 File Offset: 0x001AB3D8
		private void Setting()
		{
			base.Url = "NewFlgGetList.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x001AD244 File Offset: 0x001AB444
		public static NewFlgGetListCmd Create(int category)
		{
			return new NewFlgGetListCmd(category);
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x001AD24C File Offset: 0x001AB44C
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<NewFlgGetListResponse>(__text);
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x001AD254 File Offset: 0x001AB454
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/NewFlgGetList";
		}
	}
}
