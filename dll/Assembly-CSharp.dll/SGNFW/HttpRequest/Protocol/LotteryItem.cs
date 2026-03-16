using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class LotteryItem
	{
		public int before_item_id;

		public Item after_item;

		public int is_new;

		public int is_present_box;
	}
}
