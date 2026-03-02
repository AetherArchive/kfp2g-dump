using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003EF RID: 1007
	public class HelperFollowLimitCmd : Command
	{
		// Token: 0x06002A82 RID: 10882 RVA: 0x001AB69B File Offset: 0x001A989B
		private HelperFollowLimitCmd(int target_friend_id)
		{
			this.request = new HelperFollowLimitRequest();
			((HelperFollowLimitRequest)this.request).target_friend_id = target_friend_id;
			this.Setting();
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x001AB6C8 File Offset: 0x001A98C8
		private void Setting()
		{
			base.Url = "HelperFollowLimit.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002A84 RID: 10884 RVA: 0x001AB734 File Offset: 0x001A9934
		public static HelperFollowLimitCmd Create(int target_friend_id)
		{
			return new HelperFollowLimitCmd(target_friend_id);
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x001AB73C File Offset: 0x001A993C
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<HelperFollowLimitResponse>(__text);
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x001AB744 File Offset: 0x001A9944
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/HelperFollowLimit";
		}
	}
}
