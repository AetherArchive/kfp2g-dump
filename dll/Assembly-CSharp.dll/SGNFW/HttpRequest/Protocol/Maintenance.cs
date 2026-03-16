using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class Maintenance
	{
		public string title;

		public string text;

		public string link_adress;

		public int link_type;
	}
}
