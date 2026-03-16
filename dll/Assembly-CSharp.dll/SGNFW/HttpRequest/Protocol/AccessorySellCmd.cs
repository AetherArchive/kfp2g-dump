using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AccessorySellCmd : Command
	{
		private AccessorySellCmd()
		{
		}

		private AccessorySellCmd(List<long> accessory_idList)
		{
			this.request = new AccessorySellRequest();
			((AccessorySellRequest)this.request).accessory_idList = accessory_idList;
			this.Setting();
		}

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

		public static AccessorySellCmd Create(List<long> accessory_idList)
		{
			return new AccessorySellCmd(accessory_idList);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AccessorySellResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AccessorySell";
		}
	}
}
