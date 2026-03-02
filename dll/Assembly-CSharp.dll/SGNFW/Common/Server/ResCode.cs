using System;

namespace SGNFW.Common.Server
{
	// Token: 0x02000268 RID: 616
	public class ResCode
	{
		// Token: 0x04001C12 RID: 7186
		public const int SUCCESS = 0;

		// Token: 0x04001C13 RID: 7187
		public const int ERROR = -1;

		// Token: 0x04001C14 RID: 7188
		public const int FATAL_ERROR = -2;

		// Token: 0x04001C15 RID: 7189
		public const int SESSION_TIMEOUT = -3;

		// Token: 0x04001C16 RID: 7190
		public const int MAINTENANCE_NOW = -4;

		// Token: 0x04001C17 RID: 7191
		public const int CONNECTION_ERROR = -5;

		// Token: 0x04001C18 RID: 7192
		public const int DOUBLE_LOGIN = -6;

		// Token: 0x04001C19 RID: 7193
		public const int DATA_VERSION_UPDATE = -7;

		// Token: 0x04001C1A RID: 7194
		public const int CLIENT_VERSION_UPDATE = -8;

		// Token: 0x04001C1B RID: 7195
		public const int ACCESS_RESTRICTION = -10;

		// Token: 0x04001C1C RID: 7196
		public const int FATAL_ERROR_EMPTY_QUERY_PARAMETER = -100;

		// Token: 0x04001C1D RID: 7197
		public const int UUID_EMPTY = 100;

		// Token: 0x04001C1E RID: 7198
		public const int SECURE_ID_CHECK_FAILURE = 101;

		// Token: 0x04001C1F RID: 7199
		public const int UUID_CHECK_FAILURE = 102;

		// Token: 0x04001C20 RID: 7200
		public const int BAN_USER = 103;

		// Token: 0x04001C21 RID: 7201
		public const int TRANSFER_PASSWORD_LENGTH_SHORT = 104;

		// Token: 0x04001C22 RID: 7202
		public const int TRANSFER_PASSWORD_LENGTH_LONG = 105;

		// Token: 0x04001C23 RID: 7203
		public const int TRANSFER_PASSWORD_COOLTIME = 106;

		// Token: 0x04001C24 RID: 7204
		public const int TRANSFER_PASSWORD_NOT_EXIST = 107;

		// Token: 0x04001C25 RID: 7205
		public const int TRANSFER_PASSWORD_DIFFER = 108;

		// Token: 0x04001C26 RID: 7206
		public const int TRANSFER_PASSWORD_OTHER_PLATFORM = 109;

		// Token: 0x04001C27 RID: 7207
		public const int TRANSFER_PASSWORD_TERM_AFTER = 110;

		// Token: 0x04001C28 RID: 7208
		public const int CP_ERROR = 200;

		// Token: 0x04001C29 RID: 7209
		public const int CP_LESS = 201;

		// Token: 0x04001C2A RID: 7210
		public const int CP_ADD_NEED_AGE = 202;

		// Token: 0x04001C2B RID: 7211
		public const int CP_ADD_OVER_LIMIT = 203;

		// Token: 0x04001C2C RID: 7212
		public const int IAP_RECEIPT_DUPLICATE = 1102;

		// Token: 0x04001C2D RID: 7213
		public const int IAP_PLATFORM_MISMATCH = 1101;

		// Token: 0x04001C2E RID: 7214
		public const int IAP_VERIFY_FAILURE = 1100;

		// Token: 0x04001C2F RID: 7215
		public const int IAP_VERIFY_GOOGLE_API_FAILURE = 1110;

		// Token: 0x04001C30 RID: 7216
		public const int IAP_REFUNDED = 1111;

		// Token: 0x04001C31 RID: 7217
		public const int IAP_ANDROID_PENDING_ERROR = 1112;

		// Token: 0x04001C32 RID: 7218
		public const int IAP_RECEIPT_UNMATCH = 1113;

		// Token: 0x04001C33 RID: 7219
		public const int IAP_RECEIPT_FOR_OTHER_ACCOUNT = 302;

		// Token: 0x04001C34 RID: 7220
		public const int ATOM_SERVER_CONNECTION_ERROR = 400;

		// Token: 0x04001C35 RID: 7221
		public const int ATOM_SERVER_PARAMETER_NG = 401;

		// Token: 0x04001C36 RID: 7222
		public const int ATOM_REWARD_INTERNAL_ERROR = 402;

		// Token: 0x04001C37 RID: 7223
		public const int ATOM_REWARD_GIFTBOX_FULL = 403;

		// Token: 0x04001C38 RID: 7224
		public const int ATOM_REWARD_PREINST_DOUBLE = 404;

		// Token: 0x04001C39 RID: 7225
		public const int ATOM_REWARD_PREINST_NO_REWARD = 405;

		// Token: 0x04001C3A RID: 7226
		public const int ATOM_REWARD_CAMP_DOUBLE = 406;

		// Token: 0x04001C3B RID: 7227
		public const int ATOM_REWARD_CAMP_NO_REWARD = 407;

		// Token: 0x04001C3C RID: 7228
		public const int ATOM_REWARD_CAMP_MAX_REACHED = 408;

		// Token: 0x04001C3D RID: 7229
		public const int ATOM_REWARD_INVITE_MYSELF = 409;

		// Token: 0x04001C3E RID: 7230
		public const int ATOM_REWARD_INVITE_ALREADY_INVITED = 410;

		// Token: 0x04001C3F RID: 7231
		public const int ATOM_REWARD_INVITE_EACH_OTHER = 411;

		// Token: 0x04001C40 RID: 7232
		public const int ATOM_REWARD_INVITE_INVITER_NOT_EXIST = 412;

		// Token: 0x04001C41 RID: 7233
		public const int ATOM_REWARD_INVITE_MAX = 413;

		// Token: 0x04001C42 RID: 7234
		public const int PLAYBIT_REWARD_INTERNAL_ERROR = 500;
	}
}
