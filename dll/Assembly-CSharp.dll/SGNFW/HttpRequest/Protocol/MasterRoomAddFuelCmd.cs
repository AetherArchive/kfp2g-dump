using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomAddFuelCmd : Command
	{
		private MasterRoomAddFuelCmd()
		{
		}

		private MasterRoomAddFuelCmd(List<UseItem> use_items)
		{
			this.request = new MasterRoomAddFuelRequest();
			((MasterRoomAddFuelRequest)this.request).use_items = use_items;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "MasterRoomAddFuel.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static MasterRoomAddFuelCmd Create(List<UseItem> use_items)
		{
			return new MasterRoomAddFuelCmd(use_items);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomAddFuelResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomAddFuel";
		}
	}
}
