using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class LoginDmmResponse : Response
	{
		public string sid;

		public string uuid;

		public string account_id;

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

		public string transfer_id;

		public long training_last_starttime;

		public List<RewardInfo> rewardinfoList;

		public List<int> pvpspecialReleaseIdList;

		public List<Achievement> achievements;

		public List<ItemBank> item_banks;
	}
}
