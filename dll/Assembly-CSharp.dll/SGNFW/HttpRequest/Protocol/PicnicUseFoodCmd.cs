using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004A4 RID: 1188
	public class PicnicUseFoodCmd : Command
	{
		// Token: 0x06002C20 RID: 11296 RVA: 0x001ADE6B File Offset: 0x001AC06B
		private PicnicUseFoodCmd()
		{
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x001ADE73 File Offset: 0x001AC073
		private PicnicUseFoodCmd(List<int> itemList)
		{
			this.request = new PicnicUseFoodRequest();
			((PicnicUseFoodRequest)this.request).itemList = itemList;
			this.Setting();
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x001ADEA0 File Offset: 0x001AC0A0
		private void Setting()
		{
			base.Url = "PicnicUseFood.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x001ADF0C File Offset: 0x001AC10C
		public static PicnicUseFoodCmd Create(List<int> itemList)
		{
			return new PicnicUseFoodCmd(itemList);
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x001ADF14 File Offset: 0x001AC114
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PicnicUseFoodResponse>(__text);
		}

		// Token: 0x06002C25 RID: 11301 RVA: 0x001ADF1C File Offset: 0x001AC11C
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PicnicUseFood";
		}
	}
}
