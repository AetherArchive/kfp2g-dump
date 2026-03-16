using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomVisitCmd : Command
	{
		private MasterRoomVisitCmd()
		{
		}

		private MasterRoomVisitCmd(int friend_id, int root)
		{
			this.request = new MasterRoomVisitRequest();
			MasterRoomVisitRequest masterRoomVisitRequest = (MasterRoomVisitRequest)this.request;
			masterRoomVisitRequest.friend_id = friend_id;
			masterRoomVisitRequest.root = root;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "MasterRoomVisit.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static MasterRoomVisitCmd Create(int friend_id, int root)
		{
			return new MasterRoomVisitCmd(friend_id, root);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomVisitResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomVisit";
		}
	}
}
