using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PhotoSellRequest : Request
	{
		public List<long> photo_id;
	}
}
