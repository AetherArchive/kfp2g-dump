using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CoopStartCmd : Command
	{
		private CoopStartCmd()
		{
		}

		private CoopStartCmd(int quest_id, int deck_id, int friend_id, int helper_chara_id, int kemostatus, List<long> photo_id_List, long get_info_time, int event_id, long coop_last_update_point)
		{
			this.request = new CoopStartRequest();
			CoopStartRequest coopStartRequest = (CoopStartRequest)this.request;
			coopStartRequest.quest_id = quest_id;
			coopStartRequest.deck_id = deck_id;
			coopStartRequest.friend_id = friend_id;
			coopStartRequest.helper_chara_id = helper_chara_id;
			coopStartRequest.kemostatus = kemostatus;
			coopStartRequest.photo_id_List = photo_id_List;
			coopStartRequest.get_info_time = get_info_time;
			coopStartRequest.event_id = event_id;
			coopStartRequest.coop_last_update_point = coop_last_update_point;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CoopStart.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CoopStartCmd Create(int quest_id, int deck_id, int friend_id, int helper_chara_id, int kemostatus, List<long> photo_id_List, long get_info_time, int event_id, long coop_last_update_point)
		{
			return new CoopStartCmd(quest_id, deck_id, friend_id, helper_chara_id, kemostatus, photo_id_List, get_info_time, event_id, coop_last_update_point);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CoopStartResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CoopStart";
		}
	}
}
