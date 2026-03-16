using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AccessoryGrowCmd : Command
	{
		private AccessoryGrowCmd()
		{
		}

		private AccessoryGrowCmd(long accessory_id, List<long> materials)
		{
			this.request = new AccessoryGrowRequest();
			AccessoryGrowRequest accessoryGrowRequest = (AccessoryGrowRequest)this.request;
			accessoryGrowRequest.accessory_id = accessory_id;
			accessoryGrowRequest.materials = materials;
			this.Setting();
		}

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

		public static AccessoryGrowCmd Create(long accessory_id, List<long> materials)
		{
			return new AccessoryGrowCmd(accessory_id, materials);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AccessoryGrowResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AccessoryGrow";
		}
	}
}
