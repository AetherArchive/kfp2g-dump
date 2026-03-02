using System;

namespace AppsFlyerSDK
{
	// Token: 0x02000587 RID: 1415
	public interface IAppsFlyerUserInvite
	{
		// Token: 0x06002ED7 RID: 11991
		void onInviteLinkGenerated(string link);

		// Token: 0x06002ED8 RID: 11992
		void onInviteLinkGeneratedFailure(string error);

		// Token: 0x06002ED9 RID: 11993
		void onOpenStoreLinkGenerated(string link);
	}
}
