using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003F5 RID: 1013
	public class HelperSearchCmd : Command
	{
		// Token: 0x06002A91 RID: 10897 RVA: 0x001AB828 File Offset: 0x001A9A28
		private HelperSearchCmd()
		{
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x001AB830 File Offset: 0x001A9A30
		private HelperSearchCmd(int target_friend_id)
		{
			this.request = new HelperSearchRequest();
			((HelperSearchRequest)this.request).target_friend_id = target_friend_id;
			this.Setting();
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x001AB85C File Offset: 0x001A9A5C
		private void Setting()
		{
			base.Url = "HelperSearch.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x001AB8C8 File Offset: 0x001A9AC8
		public static HelperSearchCmd Create(int target_friend_id)
		{
			return new HelperSearchCmd(target_friend_id);
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x001AB8D0 File Offset: 0x001A9AD0
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<HelperSearchResponse>(__text);
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x001AB8D8 File Offset: 0x001A9AD8
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/HelperSearch";
		}
	}
}
