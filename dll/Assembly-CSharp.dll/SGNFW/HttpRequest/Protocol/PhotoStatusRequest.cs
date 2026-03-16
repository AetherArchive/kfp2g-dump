using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PhotoStatusRequest : Request
	{
		public List<long> lock_photo_id;

		public List<long> lock_clear_photo_id;

		public List<long> img_before_photo_id;

		public List<long> img_after_photo_id;

		public List<long> favorite_photo_id;

		public List<long> favorite_clear_photo_id;
	}
}
