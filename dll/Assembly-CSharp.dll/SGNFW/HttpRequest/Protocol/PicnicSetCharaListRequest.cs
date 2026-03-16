using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PicnicSetCharaListRequest : Request
	{
		public List<int> chara_id_list;
	}
}
