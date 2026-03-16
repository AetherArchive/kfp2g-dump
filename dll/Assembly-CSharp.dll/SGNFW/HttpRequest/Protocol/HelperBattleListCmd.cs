using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class HelperBattleListCmd : Command
	{
		private HelperBattleListCmd()
		{
		}

		private HelperBattleListCmd(int questone_id)
		{
			this.request = new HelperBattleListRequest();
			((HelperBattleListRequest)this.request).questone_id = questone_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "HelperBattleList.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static HelperBattleListCmd Create(int questone_id)
		{
			return new HelperBattleListCmd(questone_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<HelperBattleListResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/HelperBattleList";
		}
	}
}
