using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class Assets
	{
		public PlayerInfo player_info;

		public List<Chara> update_chara_list;

		public List<Item> update_item_list;

		public List<Photo> update_photo_list;

		public List<Mission> update_mission_list;

		public List<MasterSkill> update_master_skill_list;

		public List<LotteryItem> lottery_item_list;

		public List<Accessory> update_accessory_list;

		public List<Achievement> update_achievement_list;

		public List<Sticker> update_sticker_list;

		public List<ItemBank> update_item_bank_list;

		public int present_num;

		public long exp_overflow;

		public int kizunaConfirm;

		public CharaKizunaQualified qualified;

		public int practiceConfirm;

		public AssistantData assistant_data;

		public QuestSealedDatas update_sealed_data;
	}
}
