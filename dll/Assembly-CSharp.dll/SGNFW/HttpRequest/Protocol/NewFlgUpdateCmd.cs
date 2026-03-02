using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200046F RID: 1135
	public class NewFlgUpdateCmd : Command
	{
		// Token: 0x06002BA4 RID: 11172 RVA: 0x001AD26B File Offset: 0x001AB46B
		private NewFlgUpdateCmd()
		{
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x001AD273 File Offset: 0x001AB473
		private NewFlgUpdateCmd(List<NewFlg> new_flg_list)
		{
			this.request = new NewFlgUpdateRequest();
			((NewFlgUpdateRequest)this.request).new_flg_list = new_flg_list;
			this.Setting();
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x001AD2A0 File Offset: 0x001AB4A0
		private void Setting()
		{
			base.Url = "NewFlgUpdate.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x001AD30C File Offset: 0x001AB50C
		public static NewFlgUpdateCmd Create(List<NewFlg> new_flg_list)
		{
			return new NewFlgUpdateCmd(new_flg_list);
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x001AD314 File Offset: 0x001AB514
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<NewFlgUpdateResponse>(__text);
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x001AD31C File Offset: 0x001AB51C
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/NewFlgUpdate";
		}
	}
}
