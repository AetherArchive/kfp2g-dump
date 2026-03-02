using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200034D RID: 845
	[Serializable]
	public class Assets
	{
		// Token: 0x0400237F RID: 9087
		public PlayerInfo player_info;

		// Token: 0x04002380 RID: 9088
		public List<Chara> update_chara_list;

		// Token: 0x04002381 RID: 9089
		public List<Item> update_item_list;

		// Token: 0x04002382 RID: 9090
		public List<Photo> update_photo_list;

		// Token: 0x04002383 RID: 9091
		public List<Mission> update_mission_list;

		// Token: 0x04002384 RID: 9092
		public List<MasterSkill> update_master_skill_list;

		// Token: 0x04002385 RID: 9093
		public List<LotteryItem> lottery_item_list;

		// Token: 0x04002386 RID: 9094
		public List<Accessory> update_accessory_list;

		// Token: 0x04002387 RID: 9095
		public List<Achievement> update_achievement_list;

		// Token: 0x04002388 RID: 9096
		public List<ItemBank> update_item_bank_list;

		// Token: 0x04002389 RID: 9097
		public int present_num;

		// Token: 0x0400238A RID: 9098
		public long exp_overflow;

		// Token: 0x0400238B RID: 9099
		public int kizunaConfirm;

		// Token: 0x0400238C RID: 9100
		public CharaKizunaQualified qualified;

		// Token: 0x0400238D RID: 9101
		public int practiceConfirm;

		// Token: 0x0400238E RID: 9102
		public AssistantData assistant_data;

		// Token: 0x0400238F RID: 9103
		public QuestSealedDatas update_sealed_data;
	}
}
