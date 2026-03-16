using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class Chara
	{
		public int chara_id;

		public int level;

		public long exp;

		public int rank;

		public int kizuna_level;

		public long kizuna_exp;

		public int scenario_status;

		public int equip_clothes_id;

		public int select_faceicon_id;

		public int promote_num;

		public int promote_flag00;

		public int promote_flag01;

		public int promote_flag02;

		public int promote_flag03;

		public int promote_flag04;

		public int promote_flag05;

		public int arts_level;

		public int photo_frame_step;

		public long insert_time;

		public long update_time;

		public List<int> clothes_list;

		public List<Photo> photo_list;

		public int limit_over_num;

		public int kizuna_limit_over_num;

		public Accessory accessory;

		public int accessory_effect;

		public int favorite_flag;

		public int nanairo_ability_release_flag;
	}
}
