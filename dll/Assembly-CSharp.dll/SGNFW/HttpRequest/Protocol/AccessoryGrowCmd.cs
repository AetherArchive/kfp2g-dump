using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000324 RID: 804
	public class AccessoryGrowCmd : Command
	{
		// Token: 0x060028A4 RID: 10404 RVA: 0x001A878D File Offset: 0x001A698D
		private AccessoryGrowCmd()
		{
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x001A8795 File Offset: 0x001A6995
		private AccessoryGrowCmd(long accessory_id, List<long> materials)
		{
			this.request = new AccessoryGrowRequest();
			AccessoryGrowRequest accessoryGrowRequest = (AccessoryGrowRequest)this.request;
			accessoryGrowRequest.accessory_id = accessory_id;
			accessoryGrowRequest.materials = materials;
			this.Setting();
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x001A87C8 File Offset: 0x001A69C8
		private void Setting()
		{
			base.Url = "AccessoryGrow.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x001A8834 File Offset: 0x001A6A34
		public static AccessoryGrowCmd Create(long accessory_id, List<long> materials)
		{
			return new AccessoryGrowCmd(accessory_id, materials);
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x001A883D File Offset: 0x001A6A3D
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AccessoryGrowResponse>(__text);
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x001A8845 File Offset: 0x001A6A45
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AccessoryGrow";
		}
	}
}
