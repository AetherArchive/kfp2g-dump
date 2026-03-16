using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstLevelData
	{
		public int level;

		public long playerExp;

		public long playerReleaseDatetime;

		public int staminaLimit;

		public long staminaReleaseDatetime;

		public int partyCostLimit;

		public long partyReleaseDatetime;
	}
}
