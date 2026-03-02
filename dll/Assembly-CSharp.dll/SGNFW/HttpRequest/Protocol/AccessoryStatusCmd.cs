using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200032A RID: 810
	public class AccessoryStatusCmd : Command
	{
		// Token: 0x060028B4 RID: 10420 RVA: 0x001A8923 File Offset: 0x001A6B23
		private AccessoryStatusCmd()
		{
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x001A892B File Offset: 0x001A6B2B
		private AccessoryStatusCmd(List<long> lock_accessory_id_list, List<long> lock_clear_accessory_id_list)
		{
			this.request = new AccessoryStatusRequest();
			AccessoryStatusRequest accessoryStatusRequest = (AccessoryStatusRequest)this.request;
			accessoryStatusRequest.lock_accessory_id_list = lock_accessory_id_list;
			accessoryStatusRequest.lock_clear_accessory_id_list = lock_clear_accessory_id_list;
			this.Setting();
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x001A895C File Offset: 0x001A6B5C
		private void Setting()
		{
			base.Url = "AccessoryStatus.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x001A89C8 File Offset: 0x001A6BC8
		public static AccessoryStatusCmd Create(List<long> lock_accessory_id_list, List<long> lock_clear_accessory_id_list)
		{
			return new AccessoryStatusCmd(lock_accessory_id_list, lock_clear_accessory_id_list);
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x001A89D1 File Offset: 0x001A6BD1
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AccessoryStatusResponse>(__text);
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x001A89D9 File Offset: 0x001A6BD9
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AccessoryStatus";
		}
	}
}
