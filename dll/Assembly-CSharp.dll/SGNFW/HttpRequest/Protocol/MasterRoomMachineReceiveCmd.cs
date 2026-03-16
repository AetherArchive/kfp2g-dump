using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomMachineReceiveCmd : Command
	{
		private MasterRoomMachineReceiveCmd()
		{
		}

		private MasterRoomMachineReceiveCmd(bool is_all, int furniture_index)
		{
			this.request = new MasterRoomMachineReceiveRequest();
			MasterRoomMachineReceiveRequest masterRoomMachineReceiveRequest = (MasterRoomMachineReceiveRequest)this.request;
			masterRoomMachineReceiveRequest.is_all = is_all;
			masterRoomMachineReceiveRequest.furniture_index = furniture_index;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "MasterRoomMachineReceive.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static MasterRoomMachineReceiveCmd Create(bool is_all, int furniture_indexs)
		{
			return new MasterRoomMachineReceiveCmd(is_all, furniture_indexs);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomMachineReceiveResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomMachineReceive";
		}
	}
}
