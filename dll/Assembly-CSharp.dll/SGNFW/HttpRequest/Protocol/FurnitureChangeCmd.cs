using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003CC RID: 972
	public class FurnitureChangeCmd : Command
	{
		// Token: 0x06002A33 RID: 10803 RVA: 0x001AAF4B File Offset: 0x001A914B
		private FurnitureChangeCmd()
		{
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x001AAF53 File Offset: 0x001A9153
		private FurnitureChangeCmd(List<Furniture> furnitures)
		{
			this.request = new FurnitureChangeRequest();
			((FurnitureChangeRequest)this.request).furnitures = furnitures;
			this.Setting();
		}

		// Token: 0x06002A35 RID: 10805 RVA: 0x001AAF80 File Offset: 0x001A9180
		private void Setting()
		{
			base.Url = "FurnitureChange.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x001AAFEC File Offset: 0x001A91EC
		public static FurnitureChangeCmd Create(List<Furniture> furnitures)
		{
			return new FurnitureChangeCmd(furnitures);
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x001AAFF4 File Offset: 0x001A91F4
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<FurnitureChangeResponse>(__text);
		}

		// Token: 0x06002A38 RID: 10808 RVA: 0x001AAFFC File Offset: 0x001A91FC
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/FurnitureChange";
		}
	}
}
