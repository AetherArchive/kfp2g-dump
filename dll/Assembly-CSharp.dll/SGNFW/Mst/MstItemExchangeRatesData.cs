using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstItemExchangeRatesData
	{
		public int targetItemId;

		public int sourceItemId;

		public int useNum;

		public int gainNum;

		public int monthlyExchangeLimit;
	}
}
