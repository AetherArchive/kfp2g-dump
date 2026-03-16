using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class GachaItemOmakeResult
	{
		public int item_id;

		public int item_num;

		public List<RepItem> rep_item_list;

		public int is_new;

		public int is_present_box;
	}
}
