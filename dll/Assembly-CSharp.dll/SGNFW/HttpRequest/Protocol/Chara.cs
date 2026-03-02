using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000357 RID: 855
	[Serializable]
	public class Chara
	{
		// Token: 0x040023A2 RID: 9122
		public int chara_id;

		// Token: 0x040023A3 RID: 9123
		public int level;

		// Token: 0x040023A4 RID: 9124
		public long exp;

		// Token: 0x040023A5 RID: 9125
		public int rank;

		// Token: 0x040023A6 RID: 9126
		public int kizuna_level;

		// Token: 0x040023A7 RID: 9127
		public long kizuna_exp;

		// Token: 0x040023A8 RID: 9128
		public int scenario_status;

		// Token: 0x040023A9 RID: 9129
		public int equip_clothes_id;

		// Token: 0x040023AA RID: 9130
		public int select_faceicon_id;

		// Token: 0x040023AB RID: 9131
		public int promote_num;

		// Token: 0x040023AC RID: 9132
		public int promote_flag00;

		// Token: 0x040023AD RID: 9133
		public int promote_flag01;

		// Token: 0x040023AE RID: 9134
		public int promote_flag02;

		// Token: 0x040023AF RID: 9135
		public int promote_flag03;

		// Token: 0x040023B0 RID: 9136
		public int promote_flag04;

		// Token: 0x040023B1 RID: 9137
		public int promote_flag05;

		// Token: 0x040023B2 RID: 9138
		public int arts_level;

		// Token: 0x040023B3 RID: 9139
		public int photo_frame_step;

		// Token: 0x040023B4 RID: 9140
		public long insert_time;

		// Token: 0x040023B5 RID: 9141
		public long update_time;

		// Token: 0x040023B6 RID: 9142
		public List<int> clothes_list;

		// Token: 0x040023B7 RID: 9143
		public List<Photo> photo_list;

		// Token: 0x040023B8 RID: 9144
		public int limit_over_num;

		// Token: 0x040023B9 RID: 9145
		public int kizuna_limit_over_num;

		// Token: 0x040023BA RID: 9146
		public Accessory accessory;

		// Token: 0x040023BB RID: 9147
		public int accessory_effect;

		// Token: 0x040023BC RID: 9148
		public int favorite_flag;

		// Token: 0x040023BD RID: 9149
		public int nanairo_ability_release_flag;
	}
}
