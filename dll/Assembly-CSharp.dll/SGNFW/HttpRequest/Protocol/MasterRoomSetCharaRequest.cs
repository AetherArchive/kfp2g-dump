using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomSetCharaRequest : Request
	{
		public List<MasterRoomChara> chara_list;
	}
}
