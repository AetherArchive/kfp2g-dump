using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200036A RID: 874
	public class CharaFavoriteFlagCmd : Command
	{
		// Token: 0x0600294E RID: 10574 RVA: 0x001A9822 File Offset: 0x001A7A22
		private CharaFavoriteFlagCmd()
		{
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x001A982A File Offset: 0x001A7A2A
		private CharaFavoriteFlagCmd(int chara_id)
		{
			this.request = new CharaFavoriteFlagRequest();
			((CharaFavoriteFlagRequest)this.request).chara_id = chara_id;
			this.Setting();
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x001A9854 File Offset: 0x001A7A54
		private void Setting()
		{
			base.Url = "CharaFavoriteFlag.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x001A98C0 File Offset: 0x001A7AC0
		public static CharaFavoriteFlagCmd Create(int chara_id)
		{
			return new CharaFavoriteFlagCmd(chara_id);
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x001A98C8 File Offset: 0x001A7AC8
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaFavoriteFlagResponse>(__text);
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x001A98D0 File Offset: 0x001A7AD0
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaFavoriteFlag";
		}
	}
}
