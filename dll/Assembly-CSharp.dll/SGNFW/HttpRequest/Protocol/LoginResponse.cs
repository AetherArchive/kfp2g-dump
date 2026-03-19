using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class LoginResponse : Response
	{
		public string sid;

		public Assets assets;

		public List<Decks> decks;

		public List<Chara> charas;

		public List<Photo> photos;

		public List<Accessory> accessories;

		public List<Item> items;

		public List<Quest> quests;

		public List<Furniture> furnitures;

		public List<MasterSkill> master_skills;

		public List<int> kemoboard_panels;

		public int is_transfer_password;

		public long training_last_starttime;

		public List<RewardInfo> rewardinfoList;

		public List<int> pvpspecialReleaseIdList;

		public string noah_crypt;

		public List<Achievement> achievements;

		public List<ItemBank> item_banks;

		public string offer_parameter;

		public List<Sticker> stickers;
	}
}
