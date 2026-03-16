using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class UserPresent
	{
		public long id;

		public string labelText;

		public int itemId;

		public int itemNum;

		public long receiveTime;
	}
}
