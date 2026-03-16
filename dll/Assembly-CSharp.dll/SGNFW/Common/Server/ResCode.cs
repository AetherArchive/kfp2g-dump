using System;

namespace SGNFW.Common.Server
{
	public class ResCode
	{
		public const int SUCCESS = 0;

		public const int ERROR = -1;

		public const int FATAL_ERROR = -2;

		public const int SESSION_TIMEOUT = -3;

		public const int MAINTENANCE_NOW = -4;

		public const int CONNECTION_ERROR = -5;

		public const int DOUBLE_LOGIN = -6;

		public const int DATA_VERSION_UPDATE = -7;

		public const int CLIENT_VERSION_UPDATE = -8;

		public const int ACCESS_RESTRICTION = -10;

		public const int FATAL_ERROR_EMPTY_QUERY_PARAMETER = -100;

		public const int UUID_EMPTY = 100;

		public const int SECURE_ID_CHECK_FAILURE = 101;

		public const int UUID_CHECK_FAILURE = 102;

		public const int BAN_USER = 103;

		public const int TRANSFER_PASSWORD_LENGTH_SHORT = 104;

		public const int TRANSFER_PASSWORD_LENGTH_LONG = 105;

		public const int TRANSFER_PASSWORD_COOLTIME = 106;

		public const int TRANSFER_PASSWORD_NOT_EXIST = 107;

		public const int TRANSFER_PASSWORD_DIFFER = 108;

		public const int TRANSFER_PASSWORD_OTHER_PLATFORM = 109;

		public const int TRANSFER_PASSWORD_TERM_AFTER = 110;

		public const int CP_ERROR = 200;

		public const int CP_LESS = 201;

		public const int CP_ADD_NEED_AGE = 202;

		public const int CP_ADD_OVER_LIMIT = 203;

		public const int IAP_RECEIPT_DUPLICATE = 1102;

		public const int IAP_PLATFORM_MISMATCH = 1101;

		public const int IAP_VERIFY_FAILURE = 1100;

		public const int IAP_VERIFY_GOOGLE_API_FAILURE = 1110;

		public const int IAP_REFUNDED = 1111;

		public const int IAP_ANDROID_PENDING_ERROR = 1112;

		public const int IAP_RECEIPT_UNMATCH = 1113;

		public const int IAP_RECEIPT_FOR_OTHER_ACCOUNT = 302;

		public const int ATOM_SERVER_CONNECTION_ERROR = 400;

		public const int ATOM_SERVER_PARAMETER_NG = 401;

		public const int ATOM_REWARD_INTERNAL_ERROR = 402;

		public const int ATOM_REWARD_GIFTBOX_FULL = 403;

		public const int ATOM_REWARD_PREINST_DOUBLE = 404;

		public const int ATOM_REWARD_PREINST_NO_REWARD = 405;

		public const int ATOM_REWARD_CAMP_DOUBLE = 406;

		public const int ATOM_REWARD_CAMP_NO_REWARD = 407;

		public const int ATOM_REWARD_CAMP_MAX_REACHED = 408;

		public const int ATOM_REWARD_INVITE_MYSELF = 409;

		public const int ATOM_REWARD_INVITE_ALREADY_INVITED = 410;

		public const int ATOM_REWARD_INVITE_EACH_OTHER = 411;

		public const int ATOM_REWARD_INVITE_INVITER_NOT_EXIST = 412;

		public const int ATOM_REWARD_INVITE_MAX = 413;

		public const int PLAYBIT_REWARD_INTERNAL_ERROR = 500;
	}
}
