using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class GachaResult
	{
		public int item_id;

		public int item_num;

		public int rep_flg;

		public List<RepItem> rep_item_list;

		public long photo_id;

		public List<GachaItemOmakeResult> omake_item_list;

		public int is_new;

		public int is_present_box;
	}
}
