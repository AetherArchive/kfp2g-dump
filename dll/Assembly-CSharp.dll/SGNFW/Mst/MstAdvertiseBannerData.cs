using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstAdvertiseBannerData
	{
		public int id;

		public int platform;

		public string bannerName;

		public string bannerText;

		public long startTime;

		public long endTime;

		public string linkAddress;
	}
}
