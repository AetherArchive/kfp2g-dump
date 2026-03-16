using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class RecoveryCmd : Command
	{
		private RecoveryCmd()
		{
		}

		private RecoveryCmd(int itemId, int itemNum, int category)
		{
			this.request = new RecoveryRequest();
			RecoveryRequest recoveryRequest = (RecoveryRequest)this.request;
			recoveryRequest.itemId = itemId;
			recoveryRequest.itemNum = itemNum;
			recoveryRequest.category = category;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "Recovery.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static RecoveryCmd Create(int itemId, int itemNum, int category)
		{
			return new RecoveryCmd(itemId, itemNum, category);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<RecoveryResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/Recovery";
		}
	}
}
