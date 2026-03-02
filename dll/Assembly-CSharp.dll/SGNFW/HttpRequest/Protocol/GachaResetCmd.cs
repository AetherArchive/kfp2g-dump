using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003DC RID: 988
	public class GachaResetCmd : Command
	{
		// Token: 0x06002A56 RID: 10838 RVA: 0x001AB287 File Offset: 0x001A9487
		private GachaResetCmd()
		{
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x001AB28F File Offset: 0x001A948F
		private GachaResetCmd(int gacha_id)
		{
			this.request = new GachaResetRequest();
			((GachaResetRequest)this.request).gacha_id = gacha_id;
			this.Setting();
		}

		// Token: 0x06002A58 RID: 10840 RVA: 0x001AB2BC File Offset: 0x001A94BC
		private void Setting()
		{
			base.Url = "GachaReset.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002A59 RID: 10841 RVA: 0x001AB328 File Offset: 0x001A9528
		public static GachaResetCmd Create(int gacha_id)
		{
			return new GachaResetCmd(gacha_id);
		}

		// Token: 0x06002A5A RID: 10842 RVA: 0x001AB330 File Offset: 0x001A9530
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<GachaResetResponse>(__text);
		}

		// Token: 0x06002A5B RID: 10843 RVA: 0x001AB338 File Offset: 0x001A9538
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/GachaReset";
		}
	}
}
