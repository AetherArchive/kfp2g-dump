using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomSendStampRequest : Request
	{
		public int friend_id;

		public int stamp_id;

		public int root;
	}
}
