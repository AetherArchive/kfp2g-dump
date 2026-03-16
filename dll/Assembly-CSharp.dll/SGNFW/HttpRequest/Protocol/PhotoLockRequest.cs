using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PhotoLockRequest : Request
	{
		public List<long> lock_photo_id;

		public List<long> lock_clear_photo_id;
	}
}
