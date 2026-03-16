using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class DbgRegistAccountCmd : Command
	{
		private DbgRegistAccountCmd()
		{
		}

		private DbgRegistAccountCmd(string dbg_account)
		{
			this.request = new DbgRegistAccountRequest();
			((DbgRegistAccountRequest)this.request).dbg_account = dbg_account;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "dbg/DbgRegistAccount.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = "";
			base.TimeoutTime = 5f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		public static DbgRegistAccountCmd Create(string dbg_account)
		{
			return new DbgRegistAccountCmd(dbg_account);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<DbgRegistAccountResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/DbgRegistAccount";
		}
	}
}
