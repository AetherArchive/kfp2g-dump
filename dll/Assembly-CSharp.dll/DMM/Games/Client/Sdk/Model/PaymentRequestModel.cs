using System;

namespace DMM.Games.Client.Sdk.Model
{
	[Serializable]
	public class PaymentRequestModel : RequestModel
	{
		public string itemId;

		public string itemName;

		public int unitPrice;

		public int quantity;

		public string callbackurl;

		public string finishurl;
	}
}
