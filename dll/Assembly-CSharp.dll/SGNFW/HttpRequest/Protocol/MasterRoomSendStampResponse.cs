using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomSendStampResponse : Response
	{
		public Assets assets;

		public long send_stamp_time;
	}
}
