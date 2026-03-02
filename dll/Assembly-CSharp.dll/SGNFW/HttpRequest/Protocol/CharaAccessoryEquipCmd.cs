using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200035B RID: 859
	public class CharaAccessoryEquipCmd : Command
	{
		// Token: 0x06002927 RID: 10535 RVA: 0x001A943C File Offset: 0x001A763C
		private CharaAccessoryEquipCmd()
		{
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x001A9444 File Offset: 0x001A7644
		private CharaAccessoryEquipCmd(List<EquipAccessory> equip_accessory_list)
		{
			this.request = new CharaAccessoryEquipRequest();
			((CharaAccessoryEquipRequest)this.request).equip_accessory_list = equip_accessory_list;
			this.Setting();
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x001A9470 File Offset: 0x001A7670
		private void Setting()
		{
			base.Url = "CharaAccessoryEquip.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x001A94DC File Offset: 0x001A76DC
		public static CharaAccessoryEquipCmd Create(List<EquipAccessory> equip_accessory_list)
		{
			return new CharaAccessoryEquipCmd(equip_accessory_list);
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x001A94E4 File Offset: 0x001A76E4
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaAccessoryEquipResponse>(__text);
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x001A94EC File Offset: 0x001A76EC
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaAccessoryEquip";
		}
	}
}
