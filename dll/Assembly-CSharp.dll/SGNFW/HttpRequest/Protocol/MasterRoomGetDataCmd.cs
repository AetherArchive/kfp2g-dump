using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000427 RID: 1063
	public class MasterRoomGetDataCmd : Command
	{
		// Token: 0x06002AFE RID: 11006 RVA: 0x001AC263 File Offset: 0x001AA463
		private MasterRoomGetDataCmd()
		{
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x001AC26C File Offset: 0x001AA46C
		private MasterRoomGetDataCmd(int get_furniture_flg, int get_chara_flg, int get_receive_stamp_log_flg, int get_myset_flg, int get_follow_flg, int get_passing_flg, int get_ranking_flg, int get_stamp_log_flg, int get_public_info_flg)
		{
			this.request = new MasterRoomGetDataRequest();
			MasterRoomGetDataRequest masterRoomGetDataRequest = (MasterRoomGetDataRequest)this.request;
			masterRoomGetDataRequest.get_furniture_flg = get_furniture_flg;
			masterRoomGetDataRequest.get_chara_flg = get_chara_flg;
			masterRoomGetDataRequest.get_receive_stamp_log_flg = get_receive_stamp_log_flg;
			masterRoomGetDataRequest.get_myset_flg = get_myset_flg;
			masterRoomGetDataRequest.get_follow_flg = get_follow_flg;
			masterRoomGetDataRequest.get_passing_flg = get_passing_flg;
			masterRoomGetDataRequest.get_ranking_flg = get_ranking_flg;
			masterRoomGetDataRequest.get_stamp_log_flg = get_stamp_log_flg;
			masterRoomGetDataRequest.get_public_info_flg = get_public_info_flg;
			this.Setting();
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x001AC2E0 File Offset: 0x001AA4E0
		private void Setting()
		{
			base.Url = "MasterRoomGetData.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x001AC34C File Offset: 0x001AA54C
		public static MasterRoomGetDataCmd Create(int get_furniture_flg, int get_chara_flg, int get_receive_stamp_log_flg, int get_myset_flg, int get_follow_flg, int get_passing_flg, int get_ranking_flg, int get_stamp_log_flg, int get_public_info_flg)
		{
			return new MasterRoomGetDataCmd(get_furniture_flg, get_chara_flg, get_receive_stamp_log_flg, get_myset_flg, get_follow_flg, get_passing_flg, get_ranking_flg, get_stamp_log_flg, get_public_info_flg);
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x001AC36C File Offset: 0x001AA56C
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomGetDataResponse>(__text);
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x001AC374 File Offset: 0x001AA574
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomGetData";
		}
	}
}
