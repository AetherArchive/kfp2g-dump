using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000327 RID: 807
	public class AccessorySellCmd : Command
	{
		// Token: 0x060028AC RID: 10412 RVA: 0x001A885C File Offset: 0x001A6A5C
		private AccessorySellCmd()
		{
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x001A8864 File Offset: 0x001A6A64
		private AccessorySellCmd(List<long> accessory_idList)
		{
			this.request = new AccessorySellRequest();
			((AccessorySellRequest)this.request).accessory_idList = accessory_idList;
			this.Setting();
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x001A8890 File Offset: 0x001A6A90
		private void Setting()
		{
			base.Url = "AccessorySell.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x001A88FC File Offset: 0x001A6AFC
		public static AccessorySellCmd Create(List<long> accessory_idList)
		{
			return new AccessorySellCmd(accessory_idList);
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x001A8904 File Offset: 0x001A6B04
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AccessorySellResponse>(__text);
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x001A890C File Offset: 0x001A6B0C
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AccessorySell";
		}
	}
}
