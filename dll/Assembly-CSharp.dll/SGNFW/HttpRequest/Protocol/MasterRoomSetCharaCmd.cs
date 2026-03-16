using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomSetCharaCmd : Command
	{
		private MasterRoomSetCharaCmd()
		{
		}

		private MasterRoomSetCharaCmd(List<MasterRoomChara> chara_list)
		{
			this.request = new MasterRoomSetCharaRequest();
			((MasterRoomSetCharaRequest)this.request).chara_list = chara_list;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "MasterRoomSetChara.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static MasterRoomSetCharaCmd Create(List<MasterRoomChara> chara_list)
		{
			return new MasterRoomSetCharaCmd(chara_list);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomSetCharaResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomSetChara";
		}
	}
}
