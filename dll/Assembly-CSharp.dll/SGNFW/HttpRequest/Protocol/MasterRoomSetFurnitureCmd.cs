using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomSetFurnitureCmd : Command
	{
		private MasterRoomSetFurnitureCmd()
		{
		}

		private MasterRoomSetFurnitureCmd(List<MasterRoomFurniture> furniture_list)
		{
			this.request = new MasterRoomSetFurnitureRequest();
			((MasterRoomSetFurnitureRequest)this.request).furniture_list = furniture_list;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "MasterRoomSetFurniture.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static MasterRoomSetFurnitureCmd Create(List<MasterRoomFurniture> furniture_list)
		{
			return new MasterRoomSetFurnitureCmd(furniture_list);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomSetFurnitureResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomSetFurniture";
		}
	}
}
