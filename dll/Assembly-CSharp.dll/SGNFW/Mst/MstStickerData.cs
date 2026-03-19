using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstStickerData
	{
		public int id;

		public int rarity;

		public int stickerType;

		public string name;

		public string flavorText;

		public string iconName;

		public int stackMax;

		public string bgTextureName;

		public int bonusWeight;

		public long startTime;
	}
}
