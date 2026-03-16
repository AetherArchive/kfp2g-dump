using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaAccessoryEquipRequest : Request
	{
		public List<EquipAccessory> equip_accessory_list;
	}
}
