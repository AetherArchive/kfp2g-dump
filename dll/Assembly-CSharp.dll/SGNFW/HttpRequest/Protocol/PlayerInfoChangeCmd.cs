using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004AC RID: 1196
	public class PlayerInfoChangeCmd : Command
	{
		// Token: 0x06002C31 RID: 11313 RVA: 0x001ADFFA File Offset: 0x001AC1FA
		private PlayerInfoChangeCmd()
		{
		}

		// Token: 0x06002C32 RID: 11314 RVA: 0x001AE002 File Offset: 0x001AC202
		private PlayerInfoChangeCmd(PlayerInfo playerInfo)
		{
			this.request = new PlayerInfoChangeRequest();
			((PlayerInfoChangeRequest)this.request).playerInfo = playerInfo;
			this.Setting();
		}

		// Token: 0x06002C33 RID: 11315 RVA: 0x001AE02C File Offset: 0x001AC22C
		private void Setting()
		{
			base.Url = "PlayerInfoChange.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002C34 RID: 11316 RVA: 0x001AE098 File Offset: 0x001AC298
		public static PlayerInfoChangeCmd Create(PlayerInfo playerInfo)
		{
			return new PlayerInfoChangeCmd(playerInfo);
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x001AE0A0 File Offset: 0x001AC2A0
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PlayerInfoChangeResponse>(__text);
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x001AE0A8 File Offset: 0x001AC2A8
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PlayerInfoChange";
		}
	}
}
