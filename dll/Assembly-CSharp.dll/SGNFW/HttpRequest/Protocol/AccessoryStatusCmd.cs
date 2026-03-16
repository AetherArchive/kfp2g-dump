using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AccessoryStatusCmd : Command
	{
		private AccessoryStatusCmd()
		{
		}

		private AccessoryStatusCmd(List<long> lock_accessory_id_list, List<long> lock_clear_accessory_id_list)
		{
			this.request = new AccessoryStatusRequest();
			AccessoryStatusRequest accessoryStatusRequest = (AccessoryStatusRequest)this.request;
			accessoryStatusRequest.lock_accessory_id_list = lock_accessory_id_list;
			accessoryStatusRequest.lock_clear_accessory_id_list = lock_clear_accessory_id_list;
			this.Setting();
		}

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

		public static AccessoryStatusCmd Create(List<long> lock_accessory_id_list, List<long> lock_clear_accessory_id_list)
		{
			return new AccessoryStatusCmd(lock_accessory_id_list, lock_clear_accessory_id_list);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AccessoryStatusResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AccessoryStatus";
		}
	}
}
