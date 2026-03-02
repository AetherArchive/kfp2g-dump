using System;

namespace SGNFW.Http
{
	// Token: 0x02000252 RID: 594
	[Flags]
	public enum ActionTypeMask
	{
		// Token: 0x04001BB0 RID: 7088
		INVALID = 0,
		// Token: 0x04001BB1 RID: 7089
		THROUGH = 1,
		// Token: 0x04001BB2 RID: 7090
		MAINTE_WEB = 2,
		// Token: 0x04001BB3 RID: 7091
		REFRESH = 4,
		// Token: 0x04001BB4 RID: 7092
		RETRY = 8,
		// Token: 0x04001BB5 RID: 7093
		STORE = 16,
		// Token: 0x04001BB6 RID: 7094
		TITLE = 32,
		// Token: 0x04001BB7 RID: 7095
		HOME = 64,
		// Token: 0x04001BB8 RID: 7096
		SHUTDOWN = 128
	}
}
