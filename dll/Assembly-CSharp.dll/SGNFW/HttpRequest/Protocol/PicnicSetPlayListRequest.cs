using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PicnicSetPlayListRequest : Request
	{
		public List<int> play_id_list;
	}
}
