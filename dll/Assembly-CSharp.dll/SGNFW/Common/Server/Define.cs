using System;

namespace SGNFW.Common.Server
{
	public static class Define
	{
		public const int LANGUAGE_JA = 0;

		public const int LANGUAGE_EN = 1;

		public const int LANGUAGE_ZH_TW = 2;

		public const int LANGUAGE_MAX = 3;

		public const int PLATFORM_NONE = 0;

		public const int PLATFORM_IOS = 1;

		public const int PLATFORM_ANDROID = 2;

		public const int PLATFORM_AMAZON = 3;

		public const int PLATFORM_DMM = 4;

		public const int PLATFORM_APPPAY = 5;

		public const int STORE_NONE = 0;

		public const int STORE_APP_STORE = 1;

		public const int STORE_GOOGLE_PLAY = 2;

		public const int STORE_AMAZON = 3;

		public const int STORE_DMM = 4;

		public const int CPBUY_QUALIFY_STATE_NONE = 0;

		public const int CPBUY_QUALIFY_STATE_CONFIRMED = 1;

		public const int CPBUY_QUALIFY_STATE_AGE_QUALIFIED = 2;

		public const int CPBUY_AGE_CHECK_LIMIT_AGE_LOW = 15;

		public const int CPBUY_AGE_CHECK_LIMIT_AGE_HIGH = 19;

		public const int CPBUY_AGE_CHECK_LIMIT_POINT_LOW = 5000;

		public const int CPBUY_AGE_CHECK_LIMIT_POINT_HIGH = 20000;

		public const int CPBUY_AGE_CHECK_LIMIT_POINT_NOLIMIT = -1;

		public const int CP_DESTINATION_NONE = 0;

		public const int CP_DESTINATION_USER_RESOURCE = 1;

		public const int CP_DESTINATION_PRESENTBOX = 2;

		public const int CPBUY_FAILURE_NO_PRODUCT_ID = 0;

		public const int CPBUY_FAILURE_TRANSACTIONID_MISMATCH = 12;

		public const int CPBUY_FAILURE_REQUEST_NOT_REGISTERED = 13;

		public const int CPBUY_FAILURE_STATUS_CODE = 14;

		public const int CPBUY_FAILURE_VERIFICATION = 102;

		public const int CPBUY_FAILURE_NOT_PURCHASE = 202;

		public const int CPBUY_FAILURE_ORDER_ALREADY_PROCESSED = 203;

		public const int CPBUY_CHECK_RESULT_OK = 0;

		public const int CPBUY_CHECK_RESULT_LIMIT_OVER = 1;

		public const int CPBUY_CHECK_RESULT_NEED_AGE = 2;

		public const int ATOM_CAMPAIGN_NORMAL_SERIAL = 0;

		public const int ATOM_CAMPAIGN_NORMAL_SINGLE = 1;

		public const int ATOM_CAMPAIGN_PRE_REGISTRATION = 2;

		public const int ATOM_CAMPAIGN_PRE_REGISTRATION_EXT = 3;

		public const int ATOM_CAMPAIGN_INVITE = 4;

		public const int ATOM_CAMPAIGN_INVITED = 5;

		public const int ATOM_CAMPAIGN_PRE_REGISTRATION_GACHA = 6;

		public const int ATOM_SERVER_ENV_TYPE_BOTH = 0;

		public const int ATOM_SERVER_ENV_TYPE_SANDBOX = 1;

		public const int ATOM_SERVER_ENV_TYPE_SERVICE = 2;
	}
}
