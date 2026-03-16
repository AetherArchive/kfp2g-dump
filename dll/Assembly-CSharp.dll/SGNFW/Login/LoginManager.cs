using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DMMHelper;
using SGNFW.Common;
using SGNFW.Common.Json;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using UnityEngine;

namespace SGNFW.Login
{
	public class LoginManager : Singleton<LoginManager>
	{
		public static string Uuid
		{
			get
			{
				return LoginManager.uuid_;
			}
		}

		public static string Account
		{
			get
			{
				return LoginManager.account_;
			}
		}

		public static string TransferId
		{
			get
			{
				return LoginManager.transferId_;
			}
		}

		public static string SessionID
		{
			get
			{
				return LoginManager.sessionId_;
			}
		}

		public static int Platform
		{
			get
			{
				return LoginManager.platform_;
			}
		}

		public static string AssetBundleURL
		{
			get
			{
				return LoginManager.assetBundleURL_;
			}
		}

		public static string AssetBundleVersion
		{
			get
			{
				return LoginManager.assetBundleVersion_;
			}
		}

		public static void SetDeviceId(string deviceId)
		{
			LoginManager.deviceId_ = deviceId;
		}

		public static bool IsBaseDataCheck { get; set; }

		public static int MaintenanceLogin { get; private set; }

		public static Maintenance MaintenanceInfo { get; private set; }

		public static bool IsNeedVersionUp { get; private set; }

		public static bool IsATTNotDetermined { get; set; }

		public static bool IsSettingTransferPassword { get; private set; }

		public static int FriendCode { get; private set; }

		public static bool IsDmmLink { get; private set; }

		public static string WebViewBaseURL { get; private set; }

		public LoginManager.ServiceCloseData serviceCloseData { get; private set; } = new LoginManager.ServiceCloseData();

		public static string NoahCrypt { get; private set; }

		public static bool IsCheckedTerms { get; private set; }

		public static bool IsGooglePlayGamesForPC { get; private set; }

		public static string OfferParameter { get; private set; }

		private void Start()
		{
			LoginManager.IsGooglePlayGamesForPC = this.IsGooglePlay();
		}

		public void GetUrl(Action<Command> gameCallback, string version, int languageCodeServer, int platform)
		{
			this.version_ = version;
			LoginManager.platform_ = platform;
			this.languageCode_ = languageCodeServer;
			LoginManager.MaintenanceInfo = null;
			LoginManager.IsNeedVersionUp = false;
			Singleton<DataManager>.Instance.ServerRequest(GetUrlCmd.Create(this.version_, Singleton<DMMHelpManager>.Instance.VewerID), delegate(Command cmd)
			{
				GetUrlResponse getUrlResponse = cmd.response as GetUrlResponse;
				Manager.SetServerRoot("sim", getUrlResponse.base_data_url);
				this.serverId_ = getUrlResponse.server_id;
				LoginManager.assetBundleURL_ = getUrlResponse.asset_bundle_url;
				LoginManager.assetBundleVersion_ = getUrlResponse.asset_bundle_version;
				LoginManager.MaintenanceInfo = getUrlResponse.maintenance;
				LoginManager.IsNeedVersionUp = getUrlResponse.is_need_version_up == 1;
				LoginManager.WebViewBaseURL = getUrlResponse.webview_url;
				gameCallback(cmd);
			});
		}

		public void StatusCheck(Action<Command> gameCallback)
		{
			Singleton<DataManager>.Instance.ServerRequest(StatusCheckCmd.Create(LoginManager.uuid_, this.version_, Singleton<DMMHelpManager>.Instance.VewerID), delegate(Command cmd)
			{
				StatusCheckResponse statusCheckResponse = cmd.response as StatusCheckResponse;
				LoginManager.MaintenanceLogin = statusCheckResponse.maintenance_login;
				LoginManager.FriendCode = statusCheckResponse.friend_code;
				LoginManager.IsDmmLink = statusCheckResponse.dmm_data_linked_flg != 0;
				LoginManager.IsCheckedTerms = statusCheckResponse.reaccept_flg == 1;
				if (LoginManager.MaintenanceInfo == null)
				{
					LoginManager.MaintenanceInfo = statusCheckResponse.maintenance;
				}
				if (statusCheckResponse.result == 101)
				{
					LoginManager.account_ = "";
					statusCheckResponse.result = 0;
				}
				else if (statusCheckResponse.not_regist_flg == 0)
				{
					LoginManager.account_ = "dmm_user";
				}
				gameCallback(cmd);
			});
		}

		public void LoadAccountData()
		{
		}

		private void SaveAccountData()
		{
		}

		public void DeleteAccountData()
		{
		}

		public void Login(Action<Command> gameCallback, string digestSalt, bool isGPGConnect = false)
		{
			this.digestSalt_ = digestSalt;
			this.gameLoginCallBack_ = gameCallback;
			this.isGPGConnect_ = isGPGConnect;
			if (string.IsNullOrEmpty(LoginManager.account_))
			{
				Singleton<DataManager>.Instance.ServerRequest(RegistAccountCmd.Create(SystemInfo.deviceModel, this.getSign(), Singleton<DMMHelpManager>.Instance.VewerID), new Action<Command>(this.RegistAccountCb));
				return;
			}
			this.LoginInternal();
		}

		private IEnumerator LinkAccountGPG()
		{
			Singleton<DataManager>.Instance.ServerRequest(AccountGPGConnectCmd.Create(), delegate(Command cmd)
			{
				Response response = cmd.response;
			});
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			yield break;
		}

		private void LoginInternal()
		{
			LoginDmmCmd cmd = LoginDmmCmd.Create(this.version_, this.languageCode_, SystemInfo.deviceModel, this.getSign(), Singleton<DMMHelpManager>.Instance.VewerID, Singleton<DMMHelpManager>.Instance.OnetimeToken);
			if (this.isGPGConnect_)
			{
				this.linkAccount = this.LinkAccountGPG();
			}
			Singleton<DataManager>.Instance.ServerRequest(cmd, delegate(Command _cmd)
			{
				LoginDmmResponse loginDmmResponse = cmd.response as LoginDmmResponse;
				LoginManager.account_ = loginDmmResponse.account_id;
				LoginManager.uuid_ = loginDmmResponse.uuid;
				LoginManager.transferId_ = loginDmmResponse.transfer_id;
				DataManager.DmTraining.UpdateLastTrainingTime(loginDmmResponse.training_last_starttime);
				Manager.AccountEncryptKey = LoginManager.account_;
				LoginManager.sessionId_ = loginDmmResponse.sid;
				LoginManager.IsSettingTransferPassword = loginDmmResponse.is_transfer_password == 1;
				this.gameLoginCallBack_(_cmd);
			});
		}

		public void LoginGooglePlayGames(Action<GPGLoginResponse> action = null, Action<bool> isAuthFailed = null)
		{
		}

		private bool IsGooglePlay()
		{
			return false;
		}

		public string getSign()
		{
			return "none";
		}

		public void RegistAccountCb(Command cmd)
		{
			RegistAccountResponse registAccountResponse = cmd.response as RegistAccountResponse;
			LoginManager.account_ = registAccountResponse.account_id;
			LoginManager.uuid_ = registAccountResponse.uuid;
			LoginManager.transferId_ = registAccountResponse.transfer_id;
			this.SaveAccountData();
			this.LoginInternal();
		}

		public void AccountTransferPassword(Action<Command> gameCallback, string password)
		{
			Singleton<DataManager>.Instance.ServerRequest(AccountTransferPasswordRegistCmd.Create(LoginManager.transferId_, LoginUtil.GetPasswordDigest(password), LoginManager.uuid_, LoginUtil.GetLoginSecureID(LoginManager.account_, this.digestSalt_)), delegate(Command _cmd)
			{
				LoginManager.IsSettingTransferPassword = true;
				this.CbTransPass(_cmd);
				gameCallback(_cmd);
			});
		}

		private void CbTransPass(Command cmd)
		{
			AccountTransferPasswordRegistResponse accountTransferPasswordRegistResponse = cmd.response as AccountTransferPasswordRegistResponse;
			Singleton<DataManager>.Instance.UpdateUserAssetByAssets(accountTransferPasswordRegistResponse.assets);
		}

		public void AccountTransfer(Action<Command> gameCallback, string transferId, string password)
		{
			Singleton<DataManager>.Instance.ServerRequest(AccountTransferCmd.Create(transferId, LoginUtil.GetPasswordDigest(password), Singleton<DMMHelpManager>.Instance.VewerID, SystemInfo.deviceModel), delegate(Command _cmd)
			{
				AccountTransferRequest accountTransferRequest = _cmd.request as AccountTransferRequest;
				AccountTransferResponse accountTransferResponse = _cmd.response as AccountTransferResponse;
				if (accountTransferResponse.result == 1)
				{
					LoginManager.account_ = accountTransferResponse.account_id;
					LoginManager.uuid_ = accountTransferResponse.uuid;
					LoginManager.transferId_ = accountTransferRequest.transfer_id;
					this.SaveAccountData();
				}
				gameCallback(_cmd);
			});
		}

		public void AccountGPGTransfer(string account_id, string uuid, string transferId)
		{
			LoginManager.account_ = account_id;
			LoginManager.uuid_ = uuid;
			LoginManager.transferId_ = transferId;
			this.SaveAccountData();
		}

		public void AccountDelete(Action<Command> gameCallback)
		{
			Singleton<DataManager>.Instance.ServerRequest(AccountDeleteCmd.Create(LoginManager.FriendCode), delegate(Command _cmd)
			{
				Request request = _cmd.request;
				this.DeleteAccountData();
				gameCallback(_cmd);
			});
		}

		public void CheckAmazonServiceClose(Action<Command> gameCallback)
		{
			Singleton<DataManager>.Instance.ServerRequest(PlatformStatusCheckCmd.Create(), delegate(Command _cmd)
			{
				PlatformStatusCheckResponse platformStatusCheckResponse = _cmd.response as PlatformStatusCheckResponse;
				this.serviceCloseData = new LoginManager.ServiceCloseData(platformStatusCheckResponse);
				gameCallback(_cmd);
			});
		}

		public void FreezeUserAmazonAccount(Action<Command> gameCallback)
		{
			Singleton<DataManager>.Instance.ServerRequest(FreezeAccountCmd.Create(), delegate(Command _cmd)
			{
				gameCallback(_cmd);
			});
		}

		public LoginManager.DbgAccountData DbgLoadAccountFile()
		{
			if (!File.Exists(this.DbgAccFileName))
			{
				return null;
			}
			string text;
			using (FileStream fileStream = new FileStream(this.DbgAccFileName, FileMode.Open))
			{
				StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8);
				text = streamReader.ReadLine();
				streamReader.Close();
				fileStream.Close();
			}
			LoginManager.DbgAccountData dbgAccountData = PrjJson.FromJson<LoginManager.DbgAccountData>(text);
			if (dbgAccountData == null)
			{
				this.DeleteAccountData();
			}
			else
			{
				LoginManager.account_ = dbgAccountData.account;
				LoginManager.uuid_ = dbgAccountData.uuid;
				LoginManager.transferId_ = dbgAccountData.transferId;
				this.SaveAccountData();
			}
			return dbgAccountData;
		}

		public void DbgDeleteAccountFile()
		{
			this.DbgSaveAccountFile(null, "");
		}

		private void DbgSaveAccountFile(LoginManager.DbgAccountData data, string name)
		{
			List<string> list = new List<string>();
			if (File.Exists(this.DbgAccFileName))
			{
				using (FileStream fileStream = new FileStream(this.DbgAccFileName, FileMode.Open))
				{
					StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8);
					while (streamReader.Peek() > -1)
					{
						list.Add(streamReader.ReadLine());
					}
					streamReader.Close();
					fileStream.Close();
				}
			}
			string text = ((data != null) ? (PrjJson.ToJson(data) + "\t// " + name) : "");
			if (0 < list.Count)
			{
				list[0] = text;
			}
			else
			{
				list.Add(text);
			}
			using (FileStream fileStream2 = File.Create(this.DbgAccFileName))
			{
				StreamWriter sw = new StreamWriter(fileStream2, Encoding.UTF8);
				list.ForEach(delegate(string V)
				{
					sw.WriteLine(V);
				});
				sw.Close();
				fileStream2.Close();
			}
		}

		private string DbgAccFileName
		{
			get
			{
				return "account_" + this.serverId_ + ".txt";
			}
		}

		public bool IsProd()
		{
			return this.serverId_.Equals("prod");
		}

		public bool IsStage()
		{
			return this.serverId_.Equals("stg");
		}

		protected override void OnSingletonAwake()
		{
		}

		private void Update()
		{
			if (this.linkAccount != null && !DataManager.IsServerRequesting() && !this.linkAccount.MoveNext())
			{
				this.linkAccount = null;
			}
		}

		private static string uuid_ = "";

		private static string account_ = "";

		private static string transferId_ = "";

		private static string sessionId_ = "";

		private static string deviceId_ = "";

		private static int platform_ = 0;

		private static Dictionary<string, string> basedata_version_chek_ = new Dictionary<string, string>();

		private static string assetBundleURL_ = "";

		private static string assetBundleVersion_ = "";

		private string version_;

		private string digestSalt_;

		private int languageCode_;

		private string serverId_ = "";

		private bool isGPGConnect_;

		private IEnumerator linkAccount;

		private Action<Command> gameLoginCallBack_;

		public class ServiceCloseData
		{
			public ServiceCloseData(PlatformStatusCheckResponse res)
			{
				switch (res.phase)
				{
				case 1:
					this.ServiceClosePhase = LoginManager.ServiceCloseData.ClosePhase.OPEN_NOTICE;
					break;
				case 2:
					this.ServiceClosePhase = LoginManager.ServiceCloseData.ClosePhase.FINISH_REFUND;
					break;
				case 3:
					this.ServiceClosePhase = LoginManager.ServiceCloseData.ClosePhase.FINISH;
					break;
				default:
					this.ServiceClosePhase = LoginManager.ServiceCloseData.ClosePhase.OPEN;
					break;
				}
				this.IsFreezeAccount = res.freeze_flg != 0;
				this.RepaymentID = res.repayment_id;
				this.StoneChargeNum = res.stone_charge_num;
				this.lastCheckDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(res.last_check_datetime));
			}

			public ServiceCloseData(LoginManager.ServiceCloseData.ClosePhase cp)
			{
				this.ServiceClosePhase = cp;
			}

			public ServiceCloseData()
			{
			}

			public string RepaymentID { get; private set; }

			public int StoneChargeNum { get; private set; }

			public LoginManager.ServiceCloseData.ClosePhase ServiceClosePhase { get; private set; }

			public bool IsServiceClose
			{
				get
				{
					return this.ServiceClosePhase == LoginManager.ServiceCloseData.ClosePhase.FINISH || this.ServiceClosePhase == LoginManager.ServiceCloseData.ClosePhase.FINISH_REFUND;
				}
			}

			public bool IsNoticeServiceClose
			{
				get
				{
					return this.ServiceClosePhase == LoginManager.ServiceCloseData.ClosePhase.OPEN_NOTICE || this.ServiceClosePhase == LoginManager.ServiceCloseData.ClosePhase.FINISH || this.ServiceClosePhase == LoginManager.ServiceCloseData.ClosePhase.FINISH_REFUND;
				}
			}

			public bool IsFreezeAccount { get; private set; }

			public DateTime lastCheckDateTime;

			public enum ClosePhase
			{
				INVALID,
				OPEN,
				OPEN_NOTICE,
				FINISH_REFUND,
				FINISH
			}
		}

		public enum STATIS_RESULT
		{
			SUCCESS = 1,
			UUID_FAILURE = 101,
			NOT_EXIST_ACCOUNT
		}

		public enum TRANSFER_RESULT
		{
			SUCCESS = 1,
			TRANSFER_ID_NO_EXIST = 101,
			TRANSFER_NOT_EXIST_PASSWORD,
			TRANSFER_PASSWORD_DIFFER
		}

		[Serializable]
		public class DbgAccountData
		{
			public DbgAccountData()
			{
			}

			public DbgAccountData(string account, string uuid, string transferId)
			{
				this.account = account;
				this.uuid = uuid;
				this.transferId = transferId;
			}

			public string account;

			public string uuid;

			public string transferId;
		}
	}
}
