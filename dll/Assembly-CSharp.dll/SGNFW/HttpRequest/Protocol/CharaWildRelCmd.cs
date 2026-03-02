using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000393 RID: 915
	public class CharaWildRelCmd : Command
	{
		// Token: 0x060029B8 RID: 10680 RVA: 0x001AA2F8 File Offset: 0x001A84F8
		private CharaWildRelCmd()
		{
		}

		// Token: 0x060029B9 RID: 10681 RVA: 0x001AA300 File Offset: 0x001A8500
		private CharaWildRelCmd(int chara_id, List<WildResult> promote_request, int is_promoteup_action)
		{
			this.request = new CharaWildRelRequest();
			CharaWildRelRequest charaWildRelRequest = (CharaWildRelRequest)this.request;
			charaWildRelRequest.chara_id = chara_id;
			charaWildRelRequest.promote_request = promote_request;
			charaWildRelRequest.is_promoteup_action = is_promoteup_action;
			this.Setting();
		}

		// Token: 0x060029BA RID: 10682 RVA: 0x001AA338 File Offset: 0x001A8538
		private void Setting()
		{
			base.Url = "CharaWildRel.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x001AA3A4 File Offset: 0x001A85A4
		public static CharaWildRelCmd Create(int chara_id, List<WildResult> promote_request, int is_promoteup_action)
		{
			return new CharaWildRelCmd(chara_id, promote_request, is_promoteup_action);
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x001AA3AE File Offset: 0x001A85AE
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaWildRelResponse>(__text);
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x001AA3B6 File Offset: 0x001A85B6
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaWildRel";
		}
	}
}
