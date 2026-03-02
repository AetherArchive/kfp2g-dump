using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200041D RID: 1053
	public class LoginDmmResponse : Response
	{
		// Token: 0x04002576 RID: 9590
		public string sid;

		// Token: 0x04002577 RID: 9591
		public string uuid;

		// Token: 0x04002578 RID: 9592
		public string account_id;

		// Token: 0x04002579 RID: 9593
		public Assets assets;

		// Token: 0x0400257A RID: 9594
		public List<Decks> decks;

		// Token: 0x0400257B RID: 9595
		public List<Chara> charas;

		// Token: 0x0400257C RID: 9596
		public List<Photo> photos;

		// Token: 0x0400257D RID: 9597
		public List<Accessory> accessories;

		// Token: 0x0400257E RID: 9598
		public List<Item> items;

		// Token: 0x0400257F RID: 9599
		public List<Quest> quests;

		// Token: 0x04002580 RID: 9600
		public List<Furniture> furnitures;

		// Token: 0x04002581 RID: 9601
		public List<MasterSkill> master_skills;

		// Token: 0x04002582 RID: 9602
		public List<int> kemoboard_panels;

		// Token: 0x04002583 RID: 9603
		public int is_transfer_password;

		// Token: 0x04002584 RID: 9604
		public string transfer_id;

		// Token: 0x04002585 RID: 9605
		public long training_last_starttime;

		// Token: 0x04002586 RID: 9606
		public List<RewardInfo> rewardinfoList;

		// Token: 0x04002587 RID: 9607
		public List<int> pvpspecialReleaseIdList;

		// Token: 0x04002588 RID: 9608
		public List<Achievement> achievements;

		// Token: 0x04002589 RID: 9609
		public List<ItemBank> item_banks;
	}
}
