using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004AF RID: 1199
	public class PlayerInfoCmd : Command
	{
		// Token: 0x06002C39 RID: 11321 RVA: 0x001AE0BF File Offset: 0x001AC2BF
		private PlayerInfoCmd()
		{
			this.request = new PlayerInfoRequest();
			PlayerInfoRequest playerInfoRequest = (PlayerInfoRequest)this.request;
			this.Setting();
		}

		// Token: 0x06002C3A RID: 11322 RVA: 0x001AE0E4 File Offset: 0x001AC2E4
		private void Setting()
		{
			base.Url = "PlayerInfo.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002C3B RID: 11323 RVA: 0x001AE150 File Offset: 0x001AC350
		public static PlayerInfoCmd Create()
		{
			return new PlayerInfoCmd();
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x001AE157 File Offset: 0x001AC357
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PlayerInfoResponse>(__text);
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x001AE15F File Offset: 0x001AC35F
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PlayerInfo";
		}
	}
}
