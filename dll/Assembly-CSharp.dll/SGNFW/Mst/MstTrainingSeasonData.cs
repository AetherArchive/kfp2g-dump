using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstTrainingSeasonData
	{
		public int seasonId;

		public long seasonStartDatetime;

		public long seasonEndDatetime;

		public int recoveryMax;

		public int recoveryItemNum;
	}
}
