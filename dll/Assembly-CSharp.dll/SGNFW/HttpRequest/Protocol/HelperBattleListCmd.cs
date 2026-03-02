using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003EC RID: 1004
	public class HelperBattleListCmd : Command
	{
		// Token: 0x06002A7A RID: 10874 RVA: 0x001AB5D4 File Offset: 0x001A97D4
		private HelperBattleListCmd()
		{
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x001AB5DC File Offset: 0x001A97DC
		private HelperBattleListCmd(int questone_id)
		{
			this.request = new HelperBattleListRequest();
			((HelperBattleListRequest)this.request).questone_id = questone_id;
			this.Setting();
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x001AB608 File Offset: 0x001A9808
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

		// Token: 0x06002A7D RID: 10877 RVA: 0x001AB674 File Offset: 0x001A9874
		public static HelperBattleListCmd Create(int questone_id)
		{
			return new HelperBattleListCmd(questone_id);
		}

		// Token: 0x06002A7E RID: 10878 RVA: 0x001AB67C File Offset: 0x001A987C
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<HelperBattleListResponse>(__text);
		}

		// Token: 0x06002A7F RID: 10879 RVA: 0x001AB684 File Offset: 0x001A9884
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/HelperBattleList";
		}
	}
}
