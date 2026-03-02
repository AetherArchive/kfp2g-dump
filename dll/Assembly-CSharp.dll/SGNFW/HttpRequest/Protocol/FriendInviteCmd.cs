using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003C4 RID: 964
	public class FriendInviteCmd : Command
	{
		// Token: 0x06002A21 RID: 10785 RVA: 0x001AADAE File Offset: 0x001A8FAE
		private FriendInviteCmd()
		{
		}

		// Token: 0x06002A22 RID: 10786 RVA: 0x001AADB6 File Offset: 0x001A8FB6
		private FriendInviteCmd(string noah_id)
		{
			this.request = new FriendInviteRequest();
			((FriendInviteRequest)this.request).noah_id = noah_id;
			this.Setting();
		}

		// Token: 0x06002A23 RID: 10787 RVA: 0x001AADE0 File Offset: 0x001A8FE0
		private void Setting()
		{
			base.Url = "FriendInvite.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x001AAE4C File Offset: 0x001A904C
		public static FriendInviteCmd Create(string noah_id)
		{
			return new FriendInviteCmd(noah_id);
		}

		// Token: 0x06002A25 RID: 10789 RVA: 0x001AAE54 File Offset: 0x001A9054
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<FriendInviteResponse>(__text);
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x001AAE5C File Offset: 0x001A905C
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/FriendInvite";
		}
	}
}
