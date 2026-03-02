using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200037A RID: 890
	public class CharaLimitLevelUpCmd : Command
	{
		// Token: 0x06002977 RID: 10615 RVA: 0x001A9C7C File Offset: 0x001A7E7C
		private CharaLimitLevelUpCmd()
		{
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x001A9C84 File Offset: 0x001A7E84
		private CharaLimitLevelUpCmd(int chara_id, int target_level_limit_id)
		{
			this.request = new CharaLimitLevelUpRequest();
			CharaLimitLevelUpRequest charaLimitLevelUpRequest = (CharaLimitLevelUpRequest)this.request;
			charaLimitLevelUpRequest.chara_id = chara_id;
			charaLimitLevelUpRequest.target_level_limit_id = target_level_limit_id;
			this.Setting();
		}

		// Token: 0x06002979 RID: 10617 RVA: 0x001A9CB8 File Offset: 0x001A7EB8
		private void Setting()
		{
			base.Url = "CharaLimitLevelUp.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x001A9D24 File Offset: 0x001A7F24
		public static CharaLimitLevelUpCmd Create(int chara_id, int target_level_limit_id)
		{
			return new CharaLimitLevelUpCmd(chara_id, target_level_limit_id);
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x001A9D2D File Offset: 0x001A7F2D
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaLimitLevelUpResponse>(__text);
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x001A9D35 File Offset: 0x001A7F35
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaLimitLevelUp";
		}
	}
}
