using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004C9 RID: 1225
	public class PvPChalRecoveryCmd : Command
	{
		// Token: 0x06002C79 RID: 11385 RVA: 0x001AE76C File Offset: 0x001AC96C
		private PvPChalRecoveryCmd()
		{
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x001AE774 File Offset: 0x001AC974
		private PvPChalRecoveryCmd(int type_id, int season_id)
		{
			this.request = new PvPChalRecoveryRequest();
			PvPChalRecoveryRequest pvPChalRecoveryRequest = (PvPChalRecoveryRequest)this.request;
			pvPChalRecoveryRequest.type_id = type_id;
			pvPChalRecoveryRequest.season_id = season_id;
			this.Setting();
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x001AE7A8 File Offset: 0x001AC9A8
		private void Setting()
		{
			base.Url = "PvPChalRecovery.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002C7C RID: 11388 RVA: 0x001AE814 File Offset: 0x001ACA14
		public static PvPChalRecoveryCmd Create(int type_id, int season_id)
		{
			return new PvPChalRecoveryCmd(type_id, season_id);
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x001AE81D File Offset: 0x001ACA1D
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PvPChalRecoveryResponse>(__text);
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x001AE825 File Offset: 0x001ACA25
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PvPChalRecovery";
		}
	}
}
