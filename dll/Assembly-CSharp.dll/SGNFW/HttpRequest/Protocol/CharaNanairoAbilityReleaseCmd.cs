using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200037D RID: 893
	public class CharaNanairoAbilityReleaseCmd : Command
	{
		// Token: 0x0600297F RID: 10623 RVA: 0x001A9D4C File Offset: 0x001A7F4C
		private CharaNanairoAbilityReleaseCmd()
		{
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x001A9D54 File Offset: 0x001A7F54
		private CharaNanairoAbilityReleaseCmd(int chara_id)
		{
			this.request = new CharaNanairoAbilityReleaseRequest();
			((CharaNanairoAbilityReleaseRequest)this.request).chara_id = chara_id;
			this.Setting();
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x001A9D80 File Offset: 0x001A7F80
		private void Setting()
		{
			base.Url = "CharaNanairoAbilityRelease.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x001A9DEC File Offset: 0x001A7FEC
		public static CharaNanairoAbilityReleaseCmd Create(int chara_id)
		{
			return new CharaNanairoAbilityReleaseCmd(chara_id);
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x001A9DF4 File Offset: 0x001A7FF4
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaNanairoAbilityReleaseResponse>(__text);
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x001A9DFC File Offset: 0x001A7FFC
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaNanairoAbilityRelease";
		}
	}
}
