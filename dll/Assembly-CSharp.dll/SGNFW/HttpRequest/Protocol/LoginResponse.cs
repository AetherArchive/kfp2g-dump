using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200041A RID: 1050
	public class LoginResponse : Response
	{
		// Token: 0x0400255D RID: 9565
		public string sid;

		// Token: 0x0400255E RID: 9566
		public Assets assets;

		// Token: 0x0400255F RID: 9567
		public List<Decks> decks;

		// Token: 0x04002560 RID: 9568
		public List<Chara> charas;

		// Token: 0x04002561 RID: 9569
		public List<Photo> photos;

		// Token: 0x04002562 RID: 9570
		public List<Accessory> accessories;

		// Token: 0x04002563 RID: 9571
		public List<Item> items;

		// Token: 0x04002564 RID: 9572
		public List<Quest> quests;

		// Token: 0x04002565 RID: 9573
		public List<Furniture> furnitures;

		// Token: 0x04002566 RID: 9574
		public List<MasterSkill> master_skills;

		// Token: 0x04002567 RID: 9575
		public List<int> kemoboard_panels;

		// Token: 0x04002568 RID: 9576
		public int is_transfer_password;

		// Token: 0x04002569 RID: 9577
		public long training_last_starttime;

		// Token: 0x0400256A RID: 9578
		public List<RewardInfo> rewardinfoList;

		// Token: 0x0400256B RID: 9579
		public List<int> pvpspecialReleaseIdList;

		// Token: 0x0400256C RID: 9580
		public string noah_crypt;

		// Token: 0x0400256D RID: 9581
		public List<Achievement> achievements;

		// Token: 0x0400256E RID: 9582
		public List<ItemBank> item_banks;

		// Token: 0x0400256F RID: 9583
		public string offer_parameter;
	}
}
