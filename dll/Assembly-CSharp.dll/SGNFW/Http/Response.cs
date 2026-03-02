using System;

namespace SGNFW.Http
{
	// Token: 0x02000250 RID: 592
	public abstract class Response
	{
		// Token: 0x06002559 RID: 9561 RVA: 0x0019F355 File Offset: 0x0019D555
		public Response()
		{
		}

		// Token: 0x04001BA7 RID: 7079
		public const int RES_CODE_SUCCESS = 0;

		// Token: 0x04001BA8 RID: 7080
		public ErrorCode error_code;

		// Token: 0x04001BA9 RID: 7081
		public double client_wait;

		// Token: 0x04001BAA RID: 7082
		public long server_time;
	}
}
