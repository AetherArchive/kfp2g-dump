using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000440 RID: 1088
	public class MasterRoomSendStampCmd : Command
	{
		// Token: 0x06002B35 RID: 11061 RVA: 0x001AC7EB File Offset: 0x001AA9EB
		private MasterRoomSendStampCmd()
		{
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x001AC7F3 File Offset: 0x001AA9F3
		private MasterRoomSendStampCmd(int friend_id, int stamp_id, int root)
		{
			this.request = new MasterRoomSendStampRequest();
			MasterRoomSendStampRequest masterRoomSendStampRequest = (MasterRoomSendStampRequest)this.request;
			masterRoomSendStampRequest.friend_id = friend_id;
			masterRoomSendStampRequest.stamp_id = stamp_id;
			masterRoomSendStampRequest.root = root;
			this.Setting();
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x001AC82C File Offset: 0x001AAA2C
		private void Setting()
		{
			base.Url = "MasterRoomSendStamp.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002B38 RID: 11064 RVA: 0x001AC898 File Offset: 0x001AAA98
		public static MasterRoomSendStampCmd Create(int friend_id, int stamp_id, int root)
		{
			return new MasterRoomSendStampCmd(friend_id, stamp_id, root);
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x001AC8A2 File Offset: 0x001AAAA2
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomSendStampResponse>(__text);
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x001AC8AA File Offset: 0x001AAAAA
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomSendStamp";
		}
	}
}
