using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000373 RID: 883
	public class CharaKizunaLimitLevelUpCmd : Command
	{
		// Token: 0x06002966 RID: 10598 RVA: 0x001A9AE0 File Offset: 0x001A7CE0
		private CharaKizunaLimitLevelUpCmd()
		{
		}

		// Token: 0x06002967 RID: 10599 RVA: 0x001A9AE8 File Offset: 0x001A7CE8
		private CharaKizunaLimitLevelUpCmd(int chara_id)
		{
			this.request = new CharaKizunaLimitLevelUpRequest();
			((CharaKizunaLimitLevelUpRequest)this.request).chara_id = chara_id;
			this.Setting();
		}

		// Token: 0x06002968 RID: 10600 RVA: 0x001A9B14 File Offset: 0x001A7D14
		private void Setting()
		{
			base.Url = "CharaKizunaLimitLevelUp.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x001A9B80 File Offset: 0x001A7D80
		public static CharaKizunaLimitLevelUpCmd Create(int chara_id)
		{
			return new CharaKizunaLimitLevelUpCmd(chara_id);
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x001A9B88 File Offset: 0x001A7D88
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaKizunaLimitLevelUpResponse>(__text);
		}

		// Token: 0x0600296B RID: 10603 RVA: 0x001A9B90 File Offset: 0x001A7D90
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaKizunaLimitLevelUp";
		}
	}
}
