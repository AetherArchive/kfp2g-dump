using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PresentGetRequest : Request
	{
		public List<long> targetIdList;

		public int rangeLow;

		public int rangeHigh;

		public int histRangeLow;

		public int histRangeHigh;
	}
}
