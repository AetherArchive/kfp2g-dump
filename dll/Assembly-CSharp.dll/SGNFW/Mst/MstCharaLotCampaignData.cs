using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstCharaLotCampaignData
	{
		public int campaignId;

		public int successProbability;

		public int bigSuccessProbability;

		public int greatSuccessProbability;

		public int successAcqRate;

		public int bigSuccessAcqRate;

		public int greatSuccessAcqRate;

		public long startDatetime;

		public long endDatetime;
	}
}
