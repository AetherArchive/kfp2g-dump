using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200045A RID: 1114
	public class MissionListCmd : Command
	{
		// Token: 0x06002B72 RID: 11122 RVA: 0x001ACDB4 File Offset: 0x001AAFB4
		private MissionListCmd()
		{
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x001ACDBC File Offset: 0x001AAFBC
		private MissionListCmd(List<int> mission_types)
		{
			this.request = new MissionListRequest();
			((MissionListRequest)this.request).mission_types = mission_types;
			this.Setting();
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x001ACDE8 File Offset: 0x001AAFE8
		private void Setting()
		{
			base.Url = "MissionList.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002B75 RID: 11125 RVA: 0x001ACE54 File Offset: 0x001AB054
		public static MissionListCmd Create(List<int> mission_types)
		{
			return new MissionListCmd(mission_types);
		}

		// Token: 0x06002B76 RID: 11126 RVA: 0x001ACE5C File Offset: 0x001AB05C
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MissionListResponse>(__text);
		}

		// Token: 0x06002B77 RID: 11127 RVA: 0x001ACE64 File Offset: 0x001AB064
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MissionList";
		}
	}
}
