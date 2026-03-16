using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstEventLargeEventData
	{
		public int eventId;

		public int mapMoveX;

		public int mapMoveY;

		public int mapRangeOriginX;

		public int mapRangeOriginY;

		public int mapRangeWidth;

		public int mapRangeHeight;

		public double mapOffsetX;

		public double mapOffsetY;

		public string tipsFilepath01;

		public string tipsFilepath02;

		public string tipsFilepath03;

		public string tipsFilepath04;

		public string mapFilepath;

		public string changemapFilepath01;

		public int changemapQuest01;

		public string changemapFilepath02;

		public int changemapQuest02;

		public string changemapFilepath03;

		public string bgmFilepath;
	}
}
