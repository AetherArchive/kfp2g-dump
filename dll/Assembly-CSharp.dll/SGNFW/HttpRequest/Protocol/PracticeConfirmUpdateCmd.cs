using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004B9 RID: 1209
	public class PracticeConfirmUpdateCmd : Command
	{
		// Token: 0x06002C51 RID: 11345 RVA: 0x001AE30B File Offset: 0x001AC50B
		private PracticeConfirmUpdateCmd()
		{
		}

		// Token: 0x06002C52 RID: 11346 RVA: 0x001AE313 File Offset: 0x001AC513
		private PracticeConfirmUpdateCmd(int confirm)
		{
			this.request = new PracticeConfirmUpdateCmdRequest();
			((PracticeConfirmUpdateCmdRequest)this.request).confirm = confirm;
			this.Setting();
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x001AE340 File Offset: 0x001AC540
		private void Setting()
		{
			base.Url = "PracticeConfirmUpdate.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x001AE3AC File Offset: 0x001AC5AC
		public static PracticeConfirmUpdateCmd Create(int confirm)
		{
			return new PracticeConfirmUpdateCmd(confirm);
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x001AE3B4 File Offset: 0x001AC5B4
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PracticeConfirmUpdateCmdResponse>(__text);
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x001AE3BC File Offset: 0x001AC5BC
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PracticeConfirmUpdate";
		}
	}
}
