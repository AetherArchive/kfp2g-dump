using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003A4 RID: 932
	public class CoopStartCmd : Command
	{
		// Token: 0x060029DC RID: 10716 RVA: 0x001AA6F4 File Offset: 0x001A88F4
		private CoopStartCmd()
		{
		}

		// Token: 0x060029DD RID: 10717 RVA: 0x001AA6FC File Offset: 0x001A88FC
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

		// Token: 0x060029DE RID: 10718 RVA: 0x001AA770 File Offset: 0x001A8970
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

		// Token: 0x060029DF RID: 10719 RVA: 0x001AA7DC File Offset: 0x001A89DC
		public static CoopStartCmd Create(int quest_id, int deck_id, int friend_id, int helper_chara_id, int kemostatus, List<long> photo_id_List, long get_info_time, int event_id, long coop_last_update_point)
		{
			return new CoopStartCmd(quest_id, deck_id, friend_id, helper_chara_id, kemostatus, photo_id_List, get_info_time, event_id, coop_last_update_point);
		}

		// Token: 0x060029E0 RID: 10720 RVA: 0x001AA7FC File Offset: 0x001A89FC
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CoopStartResponse>(__text);
		}

		// Token: 0x060029E1 RID: 10721 RVA: 0x001AA804 File Offset: 0x001A8A04
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CoopStart";
		}
	}
}
