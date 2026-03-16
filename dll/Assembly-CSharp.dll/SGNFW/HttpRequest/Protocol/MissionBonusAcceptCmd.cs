using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MissionBonusAcceptCmd : Command
	{
		private MissionBonusAcceptCmd()
		{
		}

		private MissionBonusAcceptCmd(List<int> mission_id_list, List<AcceptMission> accept_mission_list)
		{
			this.request = new MissionBonusAcceptRequest();
			MissionBonusAcceptRequest missionBonusAcceptRequest = (MissionBonusAcceptRequest)this.request;
			missionBonusAcceptRequest.mission_id_list = mission_id_list;
			missionBonusAcceptRequest.accept_mission_list = accept_mission_list;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "MissionBonusAccept.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static MissionBonusAcceptCmd Create(List<int> mission_id_list, List<AcceptMission> accept_mission_list)
		{
			return new MissionBonusAcceptCmd(mission_id_list, accept_mission_list);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MissionBonusAcceptResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MissionBonusAccept";
		}
	}
}
