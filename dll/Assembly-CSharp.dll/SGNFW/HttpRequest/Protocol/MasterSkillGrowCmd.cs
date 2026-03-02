using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200044F RID: 1103
	public class MasterSkillGrowCmd : Command
	{
		// Token: 0x06002B58 RID: 11096 RVA: 0x001ACB34 File Offset: 0x001AAD34
		private MasterSkillGrowCmd()
		{
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x001ACB3C File Offset: 0x001AAD3C
		private MasterSkillGrowCmd(int master_skill_id, List<UseItem> use_items)
		{
			this.request = new MasterSkillGrowRequest();
			MasterSkillGrowRequest masterSkillGrowRequest = (MasterSkillGrowRequest)this.request;
			masterSkillGrowRequest.master_skill_id = master_skill_id;
			masterSkillGrowRequest.use_items = use_items;
			this.Setting();
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x001ACB70 File Offset: 0x001AAD70
		private void Setting()
		{
			base.Url = "MasterSkillGrow.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002B5B RID: 11099 RVA: 0x001ACBDC File Offset: 0x001AADDC
		public static MasterSkillGrowCmd Create(int master_skill_id, List<UseItem> use_items)
		{
			return new MasterSkillGrowCmd(master_skill_id, use_items);
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x001ACBE5 File Offset: 0x001AADE5
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterSkillGrowResponse>(__text);
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x001ACBED File Offset: 0x001AADED
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterSkillGrow";
		}
	}
}
