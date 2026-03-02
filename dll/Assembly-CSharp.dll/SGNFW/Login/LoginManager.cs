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
	// Token: 0x02000320 RID: 800
	public class LoginManager : Singleton<LoginManager>
	{
		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x0600285E RID: 10334 RVA: 0x001A7F5F File Offset: 0x001A615F
		public static string Uuid
		{
			get
			{
				return LoginManager.uuid_;
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x0600285F RID: 10335 RVA: 0x001A7F66 File Offset: 0x001A6166
		public static string Account
		{
			get
			{
				return LoginManager.account_;
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06002860 RID: 10336 RVA: 0x001A7F6D File Offset: 0x001A616D
		public static string TransferId
		{
			get
			{
				return LoginManager.transferId_;
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06002861 RID: 10337 RVA: 0x001A7F74 File Offset: 0x001A6174
		public static string SessionID
		{
			get
			{
				return LoginManager.sessionId_;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06002862 RID: 10338 RVA: 0x001A7F7B File Offset: 0x001A617B
		public static int Platform
		{
			get
			{
				return LoginManager.platform_;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06002863 RID: 10339 RVA: 0x001A7F82 File Offset: 0x001A6182
		public static string AssetBundleURL
		{
			get
			{
				return LoginManager.assetBundleURL_;
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06002864 RID: 10340 RVA: 0x001A7F89 File Offset: 0x001A6189
		public static string AssetBundleVersion
		{
			get
			{
				return LoginManager.assetBundleVersion_;
			}
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x001A7F90 File Offset: 0x001A6190
		public static void SetDeviceId(string deviceId)
		{
			LoginManager.deviceId_ = deviceId;
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06002866 RID: 10342 RVA: 0x001A7F98 File Offset: 0x001A6198
		// (set) Token: 0x06002867 RID: 10343 RVA: 0x001A7F9F File Offset: 0x001A619F
		public static bool IsBaseDataCheck { get; set; }

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06002868 RID: 10344 RVA: 0x001A7FA7 File Offset: 0x001A61A7
		// (set) Token: 0x06002869 RID: 10345 RVA: 0x001A7FAE File Offset: 0x001A61AE
		public static int MaintenanceLogin { get; private set; }

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x0600286A RID: 10346 RVA: 0x001A7FB6 File Offset: 0x001A61B6
		// (set) Token: 0x0600286B RID: 10347 RVA: 0x001A7FBD File Offset: 0x001A61BD
		public static Maintenance MaintenanceInfo { get; private set; }

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x0600286C RID: 10348 RVA: 0x001A7FC5 File Offset: 0x001A61C5
		// (set) Token: 0x0600286D RID: 10349 RVA: 0x001A7FCC File Offset: 0x001A61CC
		public static bool IsNeedVersionUp { get; private set; }

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x0600286E RID: 10350 RVA: 0x001A7FD4 File Offset: 0x001A61D4
		// (set) Token: 0x0600286F RID: 10351 RVA: 0x001A7FDB File Offset: 0x001A61DB
		public static bool IsATTNotDetermined { get; set; }

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06002870 RID: 10352 RVA: 0x001A7FE3 File Offset: 0x001A61E3
		// (set) Token: 0x06002871 RID: 10353 RVA: 0x001A7FEA File Offset: 0x001A61EA
		public static bool IsSettingTransferPassword { get; private set; }

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06002872 RID: 10354 RVA: 0x001A7FF2 File Offset: 0x001A61F2
		// (set) Token: 0x06002873 RID: 10355 RVA: 0x001A7FF9 File Offset: 0x001A61F9
		public static int FriendCode { get; private set; }

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06002874 RID: 10356 RVA: 0x001A8001 File Offset: 0x001A6201
		// (set) Token: 0x06002875 RID: 10357 RVA: 0x001A8008 File Offset: 0x001A6208
		public static bool IsDmmLink { get; private set; }

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06002876 RID: 10358 RVA: 0x001A8010 File Offset: 0x001A6210
		// (set) Token: 0x06002877 RID: 10359 RVA: 0x001A8017 File Offset: 0x001A6217
		public static string WebViewBaseURL { get; private set; }

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06002878 RID: 10360 RVA: 0x001A801F File Offset: 0x001A621F
		// (set) Token: 0x06002879 RID: 10361 RVA: 0x001A8027 File Offset: 0x001A6227
		public LoginManager.ServiceCloseData serviceCloseData { get; private set; } = new LoginManager.ServiceCloseData();

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x0600287A RID: 10362 RVA: 0x001A8030 File Offset: 0x001A6230
		// (set) Token: 0x0600287B RID: 10363 RVA: 0x001A8037 File Offset: 0x001A6237
		public static string NoahCrypt { get; private set; }

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x0600287C RID: 10364 RVA: 0x001A803F File Offset: 0x001A623F
		// (set) Token: 0x0600287D RID: 10365 RVA: 0x001A8046 File Offset: 0x001A6246
		public static bool IsCheckedTerms { get; private set; }

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x0600287E RID: 10366 RVA: 0x001A804E File Offset: 0x001A624E
		// (set) Token: 0x0600287F RID: 10367 RVA: 0x001A8055 File Offset: 0x001A6255
		public static bool IsGooglePlayGamesForPC { get; private set; }

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06002880 RID: 10368 RVA: 0x001A805D File Offset: 0x001A625D
		// (set) Token: 0x06002881 RID: 10369 RVA: 0x001A8064 File Offset: 0x001A6264
		public static string OfferParameter { get; private set; }

		// Token: 0x06002882 RID: 10370 RVA: 0x001A806C File Offset: 0x001A626C
		private void Start()
		{
			LoginManager.IsGooglePlayGamesForPC = this.IsGooglePlay();
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x001A807C File Offset: 0x001A627C
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

		// Token: 0x06002884 RID: 10372 RVA: 0x001A80EC File Offset: 0x001A62EC
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

		// Token: 0x06002885 RID: 10373 RVA: 0x001A8136 File Offset: 0x001A6336
		public void LoadAccountData()
		{
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x001A8138 File Offset: 0x001A6338
		private void SaveAccountData()
		{
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x001A813A File Offset: 0x001A633A
		public void DeleteAccountData()
		{
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x001A813C File Offset: 0x001A633C
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

		// Token: 0x06002889 RID: 10377 RVA: 0x001A81A1 File Offset: 0x001A63A1
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

		// Token: 0x0600288A RID: 10378 RVA: 0x001A81AC File Offset: 0x001A63AC
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

		// Token: 0x0600288B RID: 10379 RVA: 0x001A822C File Offset: 0x001A642C
		public void LoginGooglePlayGames(Action<GPGLoginResponse> action = null, Action<bool> isAuthFailed = null)
		{
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x001A8230 File Offset: 0x001A6430
		private bool IsGooglePlay()
		{
			return false;
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x001A8240 File Offset: 0x001A6440
		public string getSign()
		{
			return "none";
		}

		// Token: 0x0600288E RID: 10382 RVA: 0x001A8247 File Offset: 0x001A6447
		public void RegistAccountCb(Command cmd)
		{
			RegistAccountResponse registAccountResponse = cmd.response as RegistAccountResponse;
			LoginManager.account_ = registAccountResponse.account_id;
			LoginManager.uuid_ = registAccountResponse.uuid;
			LoginManager.transferId_ = registAccountResponse.transfer_id;
			this.SaveAccountData();
			this.LoginInternal();
		}

		// Token: 0x0600288F RID: 10383 RVA: 0x001A8280 File Offset: 0x001A6480
		public void AccountTransferPassword(Action<Command> gameCallback, string password)
		{
			Singleton<DataManager>.Instance.ServerRequest(AccountTransferPasswordRegistCmd.Create(LoginManager.transferId_, LoginUtil.GetPasswordDigest(password), LoginManager.uuid_, LoginUtil.GetLoginSecureID(LoginManager.account_, this.digestSalt_)), delegate(Command _cmd)
			{
				LoginManager.IsSettingTransferPassword = true;
				this.CbTransPass(_cmd);
				gameCallback(_cmd);
			});
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x001A82DC File Offset: 0x001A64DC
		private void CbTransPass(Command cmd)
		{
			AccountTransferPasswordRegistResponse accountTransferPasswordRegistResponse = cmd.response as AccountTransferPasswordRegistResponse;
			Singleton<DataManager>.Instance.UpdateUserAssetByAssets(accountTransferPasswordRegistResponse.assets);
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x001A8308 File Offset: 0x001A6508
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

		// Token: 0x06002892 RID: 10386 RVA: 0x001A835A File Offset: 0x001A655A
		public void AccountGPGTransfer(string account_id, string uuid, string transferId)
		{
			LoginManager.account_ = account_id;
			LoginManager.uuid_ = uuid;
			LoginManager.transferId_ = transferId;
			this.SaveAccountData();
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x001A8374 File Offset: 0x001A6574
		public void AccountDelete(Action<Command> gameCallback)
		{
			Singleton<DataManager>.Instance.ServerRequest(AccountDeleteCmd.Create(LoginManager.FriendCode), delegate(Command _cmd)
			{
				Request request = _cmd.request;
				this.DeleteAccountData();
				gameCallback(_cmd);
			});
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x001A83B8 File Offset: 0x001A65B8
		public void CheckAmazonServiceClose(Action<Command> gameCallback)
		{
			Singleton<DataManager>.Instance.ServerRequest(PlatformStatusCheckCmd.Create(), delegate(Command _cmd)
			{
				PlatformStatusCheckResponse platformStatusCheckResponse = _cmd.response as PlatformStatusCheckResponse;
				this.serviceCloseData = new LoginManager.ServiceCloseData(platformStatusCheckResponse);
				gameCallback(_cmd);
			});
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x001A83F4 File Offset: 0x001A65F4
		public void FreezeUserAmazonAccount(Action<Command> gameCallback)
		{
			Singleton<DataManager>.Instance.ServerRequest(FreezeAccountCmd.Create(), delegate(Command _cmd)
			{
				gameCallback(_cmd);
			});
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x001A842C File Offset: 0x001A662C
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

		// Token: 0x06002897 RID: 10391 RVA: 0x001A84C8 File Offset: 0x001A66C8
		public void DbgDeleteAccountFile()
		{
			this.DbgSaveAccountFile(null, "");
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x001A84D8 File Offset: 0x001A66D8
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

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06002899 RID: 10393 RVA: 0x001A85F0 File Offset: 0x001A67F0
		private string DbgAccFileName
		{
			get
			{
				return "account_" + this.serverId_ + ".txt";
			}
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x001A8607 File Offset: 0x001A6807
		public bool IsProd()
		{
			return this.serverId_.Equals("prod");
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x001A8619 File Offset: 0x001A6819
		public bool IsStage()
		{
			return this.serverId_.Equals("stg");
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x001A862B File Offset: 0x001A682B
		protected override void OnSingletonAwake()
		{
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x001A862D File Offset: 0x001A682D
		private void Update()
		{
			if (this.linkAccount != null && !DataManager.IsServerRequesting() && !this.linkAccount.MoveNext())
			{
				this.linkAccount = null;
			}
		}

		// Token: 0x0400232A RID: 9002
		private static string uuid_ = "";

		// Token: 0x0400232B RID: 9003
		private static string account_ = "";

		// Token: 0x0400232C RID: 9004
		private static string transferId_ = "";

		// Token: 0x0400232D RID: 9005
		private static string sessionId_ = "";

		// Token: 0x0400232E RID: 9006
		private static string deviceId_ = "";

		// Token: 0x0400232F RID: 9007
		private static int platform_ = 0;

		// Token: 0x04002330 RID: 9008
		private static Dictionary<string, string> basedata_version_chek_ = new Dictionary<string, string>();

		// Token: 0x04002331 RID: 9009
		private static string assetBundleURL_ = "";

		// Token: 0x04002332 RID: 9010
		private static string assetBundleVersion_ = "";

		// Token: 0x04002333 RID: 9011
		private string version_;

		// Token: 0x04002334 RID: 9012
		private string digestSalt_;

		// Token: 0x04002335 RID: 9013
		private int languageCode_;

		// Token: 0x04002336 RID: 9014
		private string serverId_ = "";

		// Token: 0x04002337 RID: 9015
		private bool isGPGConnect_;

		// Token: 0x04002338 RID: 9016
		private IEnumerator linkAccount;

		// Token: 0x04002347 RID: 9031
		private Action<Command> gameLoginCallBack_;

		// Token: 0x020010D1 RID: 4305
		public class ServiceCloseData
		{
			// Token: 0x060053EC RID: 21484 RVA: 0x0024C41C File Offset: 0x0024A61C
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

			// Token: 0x060053ED RID: 21485 RVA: 0x0024C4AB File Offset: 0x0024A6AB
			public ServiceCloseData(LoginManager.ServiceCloseData.ClosePhase cp)
			{
				this.ServiceClosePhase = cp;
			}

			// Token: 0x060053EE RID: 21486 RVA: 0x0024C4BA File Offset: 0x0024A6BA
			public ServiceCloseData()
			{
			}

			// Token: 0x17000C03 RID: 3075
			// (get) Token: 0x060053EF RID: 21487 RVA: 0x0024C4C2 File Offset: 0x0024A6C2
			// (set) Token: 0x060053F0 RID: 21488 RVA: 0x0024C4CA File Offset: 0x0024A6CA
			public string RepaymentID { get; private set; }

			// Token: 0x17000C04 RID: 3076
			// (get) Token: 0x060053F1 RID: 21489 RVA: 0x0024C4D3 File Offset: 0x0024A6D3
			// (set) Token: 0x060053F2 RID: 21490 RVA: 0x0024C4DB File Offset: 0x0024A6DB
			public int StoneChargeNum { get; private set; }

			// Token: 0x17000C05 RID: 3077
			// (get) Token: 0x060053F3 RID: 21491 RVA: 0x0024C4E4 File Offset: 0x0024A6E4
			// (set) Token: 0x060053F4 RID: 21492 RVA: 0x0024C4EC File Offset: 0x0024A6EC
			public LoginManager.ServiceCloseData.ClosePhase ServiceClosePhase { get; private set; }

			// Token: 0x17000C06 RID: 3078
			// (get) Token: 0x060053F5 RID: 21493 RVA: 0x0024C4F5 File Offset: 0x0024A6F5
			public bool IsServiceClose
			{
				get
				{
					return this.ServiceClosePhase == LoginManager.ServiceCloseData.ClosePhase.FINISH || this.ServiceClosePhase == LoginManager.ServiceCloseData.ClosePhase.FINISH_REFUND;
				}
			}

			// Token: 0x17000C07 RID: 3079
			// (get) Token: 0x060053F6 RID: 21494 RVA: 0x0024C50B File Offset: 0x0024A70B
			public bool IsNoticeServiceClose
			{
				get
				{
					return this.ServiceClosePhase == LoginManager.ServiceCloseData.ClosePhase.OPEN_NOTICE || this.ServiceClosePhase == LoginManager.ServiceCloseData.ClosePhase.FINISH || this.ServiceClosePhase == LoginManager.ServiceCloseData.ClosePhase.FINISH_REFUND;
				}
			}

			// Token: 0x17000C08 RID: 3080
			// (get) Token: 0x060053F7 RID: 21495 RVA: 0x0024C52A File Offset: 0x0024A72A
			// (set) Token: 0x060053F8 RID: 21496 RVA: 0x0024C532 File Offset: 0x0024A732
			public bool IsFreezeAccount { get; private set; }

			// Token: 0x04005D25 RID: 23845
			public DateTime lastCheckDateTime;

			// Token: 0x0200122C RID: 4652
			public enum ClosePhase
			{
				// Token: 0x0400637D RID: 25469
				INVALID,
				// Token: 0x0400637E RID: 25470
				OPEN,
				// Token: 0x0400637F RID: 25471
				OPEN_NOTICE,
				// Token: 0x04006380 RID: 25472
				FINISH_REFUND,
				// Token: 0x04006381 RID: 25473
				FINISH
			}
		}

		// Token: 0x020010D2 RID: 4306
		public enum STATIS_RESULT
		{
			// Token: 0x04005D29 RID: 23849
			SUCCESS = 1,
			// Token: 0x04005D2A RID: 23850
			UUID_FAILURE = 101,
			// Token: 0x04005D2B RID: 23851
			NOT_EXIST_ACCOUNT
		}

		// Token: 0x020010D3 RID: 4307
		public enum TRANSFER_RESULT
		{
			// Token: 0x04005D2D RID: 23853
			SUCCESS = 1,
			// Token: 0x04005D2E RID: 23854
			TRANSFER_ID_NO_EXIST = 101,
			// Token: 0x04005D2F RID: 23855
			TRANSFER_NOT_EXIST_PASSWORD,
			// Token: 0x04005D30 RID: 23856
			TRANSFER_PASSWORD_DIFFER
		}

		// Token: 0x020010D4 RID: 4308
		[Serializable]
		public class DbgAccountData
		{
			// Token: 0x060053F9 RID: 21497 RVA: 0x0024C53B File Offset: 0x0024A73B
			public DbgAccountData()
			{
			}

			// Token: 0x060053FA RID: 21498 RVA: 0x0024C543 File Offset: 0x0024A743
			public DbgAccountData(string account, string uuid, string transferId)
			{
				this.account = account;
				this.uuid = uuid;
				this.transferId = transferId;
			}

			// Token: 0x04005D31 RID: 23857
			public string account;

			// Token: 0x04005D32 RID: 23858
			public string uuid;

			// Token: 0x04005D33 RID: 23859
			public string transferId;
		}
	}
}
