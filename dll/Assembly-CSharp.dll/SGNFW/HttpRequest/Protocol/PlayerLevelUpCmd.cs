using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004B2 RID: 1202
	public class PlayerLevelUpCmd : Command
	{
		// Token: 0x06002C40 RID: 11328 RVA: 0x001AE176 File Offset: 0x001AC376
		private PlayerLevelUpCmd()
		{
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x001AE17E File Offset: 0x001AC37E
		private PlayerLevelUpCmd(long user_exp_overflow)
		{
			this.request = new PlayerLevelUpRequest();
			((PlayerLevelUpRequest)this.request).user_exp_overflow = user_exp_overflow;
			this.Setting();
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x001AE1A8 File Offset: 0x001AC3A8
		private void Setting()
		{
			base.Url = "PlayerLevelUp.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002C43 RID: 11331 RVA: 0x001AE214 File Offset: 0x001AC414
		public static PlayerLevelUpCmd Create(long user_exp_overflow)
		{
			return new PlayerLevelUpCmd(user_exp_overflow);
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x001AE21C File Offset: 0x001AC41C
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PlayerLevelUpResponse>(__text);
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x001AE224 File Offset: 0x001AC424
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PlayerLevelUp";
		}
	}
}
