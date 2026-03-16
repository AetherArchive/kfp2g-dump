using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstBannerData
	{
		public int id;

		public string bannerName;

		public string bannerText;

		public long startTime;

		public long endTime;

		public string linkAdress;

		public int linkType;

		public int linkValue;

		public int priority;

		public int platform;
	}
}
