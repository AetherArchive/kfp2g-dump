using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003AA RID: 938
	public class DbgRegistAccountCmd : Command
	{
		// Token: 0x060029EC RID: 10732 RVA: 0x001AA8F1 File Offset: 0x001A8AF1
		private DbgRegistAccountCmd()
		{
		}

		// Token: 0x060029ED RID: 10733 RVA: 0x001AA8F9 File Offset: 0x001A8AF9
		private DbgRegistAccountCmd(string dbg_account)
		{
			this.request = new DbgRegistAccountRequest();
			((DbgRegistAccountRequest)this.request).dbg_account = dbg_account;
			this.Setting();
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x001AA924 File Offset: 0x001A8B24
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

		// Token: 0x060029EF RID: 10735 RVA: 0x001AA990 File Offset: 0x001A8B90
		public static DbgRegistAccountCmd Create(string dbg_account)
		{
			return new DbgRegistAccountCmd(dbg_account);
		}

		// Token: 0x060029F0 RID: 10736 RVA: 0x001AA998 File Offset: 0x001A8B98
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<DbgRegistAccountResponse>(__text);
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x001AA9A0 File Offset: 0x001A8BA0
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/DbgRegistAccount";
		}
	}
}
