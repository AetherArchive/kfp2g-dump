using System;

namespace SGNFW.Http
{
	public abstract class Response
	{
		public Response()
		{
		}

		public const int RES_CODE_SUCCESS = 0;

		public ErrorCode error_code;

		public double client_wait;

		public long server_time;
	}
}
