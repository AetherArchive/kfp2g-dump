using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PhotoReleaseResponse : Response
	{
		public int result;

		public List<MyHelper> helperList;

		public List<Decks> decks;
	}
}
