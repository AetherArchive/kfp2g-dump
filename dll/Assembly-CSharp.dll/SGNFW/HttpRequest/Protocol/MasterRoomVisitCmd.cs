using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200044B RID: 1099
	public class MasterRoomVisitCmd : Command
	{
		// Token: 0x06002B4F RID: 11087 RVA: 0x001ACA5F File Offset: 0x001AAC5F
		private MasterRoomVisitCmd()
		{
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x001ACA67 File Offset: 0x001AAC67
		private MasterRoomVisitCmd(int friend_id, int root)
		{
			this.request = new MasterRoomVisitRequest();
			MasterRoomVisitRequest masterRoomVisitRequest = (MasterRoomVisitRequest)this.request;
			masterRoomVisitRequest.friend_id = friend_id;
			masterRoomVisitRequest.root = root;
			this.Setting();
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x001ACA98 File Offset: 0x001AAC98
		private void Setting()
		{
			base.Url = "MasterRoomVisit.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x001ACB04 File Offset: 0x001AAD04
		public static MasterRoomVisitCmd Create(int friend_id, int root)
		{
			return new MasterRoomVisitCmd(friend_id, root);
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x001ACB0D File Offset: 0x001AAD0D
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomVisitResponse>(__text);
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x001ACB15 File Offset: 0x001AAD15
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomVisit";
		}
	}
}
