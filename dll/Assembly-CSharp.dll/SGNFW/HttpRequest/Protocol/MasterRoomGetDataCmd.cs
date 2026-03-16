using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomGetDataCmd : Command
	{
		private MasterRoomGetDataCmd()
		{
		}

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

		public static MasterRoomGetDataCmd Create(int get_furniture_flg, int get_chara_flg, int get_receive_stamp_log_flg, int get_myset_flg, int get_follow_flg, int get_passing_flg, int get_ranking_flg, int get_stamp_log_flg, int get_public_info_flg)
		{
			return new MasterRoomGetDataCmd(get_furniture_flg, get_chara_flg, get_receive_stamp_log_flg, get_myset_flg, get_follow_flg, get_passing_flg, get_ranking_flg, get_stamp_log_flg, get_public_info_flg);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomGetDataResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomGetData";
		}
	}
}
