using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstPvpSeasonData
	{
		public int typeId;

		public int seasonId;

		public int pvpId;

		public long seasonStartDatetime;

		public long seasonEndDatetime;

		public int rankResetNum;

		public int rankResetRatio;

		public int eventId;
	}
}
