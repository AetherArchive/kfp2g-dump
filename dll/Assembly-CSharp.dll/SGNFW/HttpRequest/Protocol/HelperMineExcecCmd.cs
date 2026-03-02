using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003F2 RID: 1010
	public class HelperMineExcecCmd : Command
	{
		// Token: 0x06002A89 RID: 10889 RVA: 0x001AB75B File Offset: 0x001A995B
		private HelperMineExcecCmd()
		{
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x001AB763 File Offset: 0x001A9963
		private HelperMineExcecCmd(int action_type, List<int> target_friend_id_list)
		{
			this.request = new HelperMineExcecRequest();
			HelperMineExcecRequest helperMineExcecRequest = (HelperMineExcecRequest)this.request;
			helperMineExcecRequest.action_type = action_type;
			helperMineExcecRequest.target_friend_id_list = target_friend_id_list;
			this.Setting();
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x001AB794 File Offset: 0x001A9994
		private void Setting()
		{
			base.Url = "HelperMineExcec.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x001AB800 File Offset: 0x001A9A00
		public static HelperMineExcecCmd Create(int action_type, List<int> target_friend_id_list)
		{
			return new HelperMineExcecCmd(action_type, target_friend_id_list);
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x001AB809 File Offset: 0x001A9A09
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<HelperMineExcecResponse>(__text);
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x001AB811 File Offset: 0x001A9A11
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/HelperMineExcec";
		}
	}
}
