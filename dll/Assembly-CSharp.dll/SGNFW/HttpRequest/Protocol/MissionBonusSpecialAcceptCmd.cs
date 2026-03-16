using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MissionBonusSpecialAcceptCmd : Command
	{
		private MissionBonusSpecialAcceptCmd()
		{
		}

		private MissionBonusSpecialAcceptCmd(List<int> mission_id_list, List<AcceptMission> accept_mission_list)
		{
			this.request = new MissionBonusSpecialAcceptRequest();
			MissionBonusSpecialAcceptRequest missionBonusSpecialAcceptRequest = (MissionBonusSpecialAcceptRequest)this.request;
			missionBonusSpecialAcceptRequest.mission_id_list = mission_id_list;
			missionBonusSpecialAcceptRequest.accept_mission_list = accept_mission_list;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "MissionBonusSpecialAccept.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static MissionBonusSpecialAcceptCmd Create(List<int> mission_id_list, List<AcceptMission> accept_mission_list)
		{
			return new MissionBonusSpecialAcceptCmd(mission_id_list, accept_mission_list);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MissionBonusSpecialAcceptResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MissionBonusSpecialAccept";
		}
	}
}
