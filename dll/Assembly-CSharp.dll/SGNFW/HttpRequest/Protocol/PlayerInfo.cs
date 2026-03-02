using System;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004AB RID: 1195
	[Serializable]
	public class PlayerInfo
	{
		// Token: 0x040026BC RID: 9916
		public string player_name;

		// Token: 0x040026BD RID: 9917
		public int player_type;

		// Token: 0x040026BE RID: 9918
		public int player_rank;

		// Token: 0x040026BF RID: 9919
		public long player_exp;

		// Token: 0x040026C0 RID: 9920
		public int stamina;

		// Token: 0x040026C1 RID: 9921
		public long stamina_updated_at;

		// Token: 0x040026C2 RID: 9922
		public int tutorial_step;

		// Token: 0x040026C3 RID: 9923
		public int friend_id;

		// Token: 0x040026C4 RID: 9924
		public long last_login_time;

		// Token: 0x040026C5 RID: 9925
		public string comment;

		// Token: 0x040026C6 RID: 9926
		public int favorite_chara_id;

		// Token: 0x040026C7 RID: 9927
		public long birthday;

		// Token: 0x040026C8 RID: 9928
		public int monthlypack_id;

		// Token: 0x040026C9 RID: 9929
		public long monthlypack_endtime;

		// Token: 0x040026CA RID: 9930
		public int monthlypack_id_next;

		// Token: 0x040026CB RID: 9931
		public long monthlypack_endtime_next;

		// Token: 0x040026CC RID: 9932
		public string played_login_scenario_list;

		// Token: 0x040026CD RID: 9933
		public string played_introduction_list;
	}
}
