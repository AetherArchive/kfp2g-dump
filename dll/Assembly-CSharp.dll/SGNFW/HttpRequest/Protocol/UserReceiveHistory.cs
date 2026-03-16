using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class UserReceiveHistory
	{
		public string labelText;

		public int itemId;

		public int itemNum;

		public long receiveTime;
	}
}
