using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PhotoReleaseRequest : Request
	{
		public List<long> photoIdList;
	}
}
