using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class KizunaLevelResult
	{
		public int chara_id;

		public int kizunalevel;

		public long kizunaexp;

		public int success_status;

		public int return_item;
	}
}
