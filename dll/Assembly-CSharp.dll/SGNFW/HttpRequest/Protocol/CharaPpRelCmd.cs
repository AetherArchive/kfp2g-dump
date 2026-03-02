using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000380 RID: 896
	public class CharaPpRelCmd : Command
	{
		// Token: 0x06002987 RID: 10631 RVA: 0x001A9E13 File Offset: 0x001A8013
		private CharaPpRelCmd()
		{
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x001A9E1B File Offset: 0x001A801B
		private CharaPpRelCmd(int chara_id, int target_pp_step)
		{
			this.request = new CharaPpRelRequest();
			CharaPpRelRequest charaPpRelRequest = (CharaPpRelRequest)this.request;
			charaPpRelRequest.chara_id = chara_id;
			charaPpRelRequest.target_pp_step = target_pp_step;
			this.Setting();
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x001A9E4C File Offset: 0x001A804C
		private void Setting()
		{
			base.Url = "CharaPpRel.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x001A9EB8 File Offset: 0x001A80B8
		public static CharaPpRelCmd Create(int chara_id, int target_pp_step)
		{
			return new CharaPpRelCmd(chara_id, target_pp_step);
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x001A9EC1 File Offset: 0x001A80C1
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaPpRelResponse>(__text);
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x001A9EC9 File Offset: 0x001A80C9
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaPpRel";
		}
	}
}
