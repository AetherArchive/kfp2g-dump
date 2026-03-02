using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000412 RID: 1042
	public class KizunaConfirmUpdateCmd : Command
	{
		// Token: 0x06002AD5 RID: 10965 RVA: 0x001ABE87 File Offset: 0x001AA087
		private KizunaConfirmUpdateCmd()
		{
		}

		// Token: 0x06002AD6 RID: 10966 RVA: 0x001ABE8F File Offset: 0x001AA08F
		private KizunaConfirmUpdateCmd(int confirm)
		{
			this.request = new KizunaConfirmUpdateCmdRequest();
			((KizunaConfirmUpdateCmdRequest)this.request).confirm = confirm;
			this.Setting();
		}

		// Token: 0x06002AD7 RID: 10967 RVA: 0x001ABEBC File Offset: 0x001AA0BC
		private void Setting()
		{
			base.Url = "KizunaConfirmUpdate.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002AD8 RID: 10968 RVA: 0x001ABF28 File Offset: 0x001AA128
		public static KizunaConfirmUpdateCmd Create(int confirm)
		{
			return new KizunaConfirmUpdateCmd(confirm);
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x001ABF30 File Offset: 0x001AA130
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<KizunaConfirmUpdateCmdResponse>(__text);
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x001ABF38 File Offset: 0x001AA138
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/KizunaConfirmUpdate";
		}
	}
}
