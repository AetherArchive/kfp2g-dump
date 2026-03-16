using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class Mission
	{
		public int mission_id;

		public long mission_datetime;

		public int mission_status;

		public int delivery_flg;

		public int accept_flg;
	}
}
