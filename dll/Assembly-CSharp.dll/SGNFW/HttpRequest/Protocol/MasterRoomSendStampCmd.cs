using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomSendStampCmd : Command
	{
		private MasterRoomSendStampCmd()
		{
		}

		private MasterRoomSendStampCmd(int friend_id, int stamp_id, int root)
		{
			this.request = new MasterRoomSendStampRequest();
			MasterRoomSendStampRequest masterRoomSendStampRequest = (MasterRoomSendStampRequest)this.request;
			masterRoomSendStampRequest.friend_id = friend_id;
			masterRoomSendStampRequest.stamp_id = stamp_id;
			masterRoomSendStampRequest.root = root;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "MasterRoomSendStamp.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static MasterRoomSendStampCmd Create(int friend_id, int stamp_id, int root)
		{
			return new MasterRoomSendStampCmd(friend_id, stamp_id, root);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomSendStampResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomSendStamp";
		}
	}
}
