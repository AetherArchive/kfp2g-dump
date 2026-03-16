using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaAccessoryEquipCmd : Command
	{
		private CharaAccessoryEquipCmd()
		{
		}

		private CharaAccessoryEquipCmd(List<EquipAccessory> equip_accessory_list)
		{
			this.request = new CharaAccessoryEquipRequest();
			((CharaAccessoryEquipRequest)this.request).equip_accessory_list = equip_accessory_list;
			this.Setting();
		}

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

		public static CharaAccessoryEquipCmd Create(List<EquipAccessory> equip_accessory_list)
		{
			return new CharaAccessoryEquipCmd(equip_accessory_list);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaAccessoryEquipResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaAccessoryEquip";
		}
	}
}
