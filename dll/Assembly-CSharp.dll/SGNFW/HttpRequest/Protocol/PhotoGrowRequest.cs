using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PhotoGrowRequest : Request
	{
		public long photo_id;

		public List<long> materials;
	}
}
