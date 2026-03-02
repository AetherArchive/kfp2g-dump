using System;
using System.Collections;
using System.Collections.Generic;
using DMMHelper;
using SGNFW.Ab;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Login;
using SGNFW.Mst;
using SGNFW.Touch;
using UnityEngine;

// Token: 0x02000125 RID: 293
public class DataInitializeResolver
{
	// Token: 0x06000EF6 RID: 3830 RVA: 0x000B52ED File Offset: 0x000B34ED
	public static IEnumerator InitializeActionDataManager()
	{
		Singleton<DataManager>.Instance.Initialize();
		DataManager.DmServerMst.RequestDownloadServerTime(null);
		IEnumerator ienum = Singleton<DataManager>.Instance.InitializeMstData();
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = null;
		ienum = DataInitializeResolver.UpdateUserData(false);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = null;
		yield break;
	}

	// Token: 0x06000EF7 RID: 3831 RVA: 0x000B52F5 File Offset: 0x000B34F5
	public static IEnumerator InitializeActionDataManagerForDebug()
	{
		IEnumerator ienum = Singleton<DataManager>.Instance.ReInitializeMstData();
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = null;
		yield break;
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x000B52FD File Offset: 0x000B34FD
	public static IEnumerator InitializeActionBeforeMst()
	{
		DataInitializeResolver.isFinishedInitializeActionBeforeMst = false;
		DataInitializeResolver.isRunningInitializeActionBeforeMst = true;
		TimeManager.EnableTimeCmd = false;
		TimeManager.TimeScale = 1f;
		Screen.sleepTimeout = -2;
		SGNFW.Touch.Manager.UnRegisterAll();
		Singleton<SceneManager>.Instance.DestroyAliveScene();
		EffectManager.DestroyEffectAll();
		Singleton<AssetManager>.Instance.Destroy();
		SceneManager.CanvasSetActive(SceneManager.CanvasType.FRONT, false);
		SceneManager.CanvasSetActive(SceneManager.CanvasType.BACK, false);
		SceneManager.CanvasSetActive(SceneManager.CanvasType.SYSTEM, false);
		yield return null;
		PrjUtil.ReleaseMemory(PrjUtil.Garbagecollection);
		yield return null;
		CanvasManager.CanvasDestroy();
		CanvasManager.Initialize();
		yield return null;
		SceneManager.Crean3DField();
		yield return null;
		SceneManager.Initialize3DField();
		yield return null;
		PrjUtil.ReleaseMemory(PrjUtil.Garbagecollection);
		yield return null;
		Singleton<DataManager>.Instance.Initialize();
		yield return null;
		SGNFW.Http.Manager.SetUserAgent("sim", "SEGA Web Client for Project 2015");
		SGNFW.Http.Manager.SetUserAgent("chat", "SEGA Web Client for Project 2015 CHAT");
		SGNFW.Http.Manager.DefaultEncryptKey = "MEDARAP";
		SGNFW.Http.Manager.AccountEncryptKey = "ACCOUNT_STRING";
		SGNFW.Http.Manager.SetServerRoot("root", DataInitializeResolver.ServerEnv.url);
		Verbose<SGNFW.Http.Verbose>.Enabled = true;
		yield return null;
		IEnumerator ienum = Singleton<DMMHelpManager>.Instance.ResolveinItialize();
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = null;
		DataInitializeResolver.<>c__DisplayClass11_0 CS$<>8__locals1 = new DataInitializeResolver.<>c__DisplayClass11_0();
		CS$<>8__locals1.isFinish = false;
		Singleton<LoginManager>.Instance.GetUrl(delegate(Command res)
		{
			CS$<>8__locals1.isFinish = true;
		}, DataInitializeResolver.ServerEnv.ver, 0, 4);
		while (!CS$<>8__locals1.isFinish)
		{
			yield return null;
		}
		CS$<>8__locals1 = null;
		Singleton<LoginManager>.Instance.LoadAccountData();
		yield return null;
		ienum = DataInitializeResolver.LoginStatusCheck();
		object currentObject = null;
		while (ienum.MoveNext())
		{
			currentObject = ienum.Current;
			yield return null;
		}
		KeyValuePair<SceneManager.SceneName, object>? keyValuePair = currentObject as KeyValuePair<SceneManager.SceneName, object>?;
		if (keyValuePair != null)
		{
			yield return keyValuePair;
		}
		ienum = null;
		currentObject = null;
		SGNFW.Ab.Manager.Initialize(LoginManager.AssetBundleURL, LoginManager.AssetBundleVersion, Define.Language.ja);
		Singleton<AssetManager>.Instance.Initialize();
		while (!AssetManager.IsFinishInitialize)
		{
			yield return null;
		}
		DataInitializeResolver.isRunningInitializeActionBeforeMst = false;
		DataInitializeResolver.isFinishedInitializeActionBeforeMst = true;
		yield break;
	}

	// Token: 0x06000EF9 RID: 3833 RVA: 0x000B5305 File Offset: 0x000B3505
	public static IEnumerator InitializeActionBeforeTitle()
	{
		IEnumerator ienum = DataInitializeResolver.InitializeActionBeforeMst();
		object currentObject = null;
		while (ienum.MoveNext())
		{
			currentObject = ienum.Current;
			yield return null;
		}
		KeyValuePair<SceneManager.SceneName, object>? keyValuePair = currentObject as KeyValuePair<SceneManager.SceneName, object>?;
		if (keyValuePair != null)
		{
			yield return keyValuePair;
			yield break;
		}
		PrjUtil.AppsFlyerActivate(LoginManager.FriendCode);
		TimeManager.EnableTimeCmd = true;
		DataManager.DmServerMst.RequestDownloadServerTime(null);
		yield return null;
		Singleton<MstManager>.Instance.Refresh();
		while (!Singleton<MstManager>.Instance.IsLoadFinish)
		{
			yield return null;
		}
		MstGameAppearanceData mstGameAppearanceData = GameAppearanceUtill.GetMstGameAppearanceData(TimeManager.Now);
		DataInitializeResolver.titleLogo = "Texture2D/Title_Logo/" + (string.IsNullOrEmpty(mstGameAppearanceData.titleTexturePath) ? "title_logo_001" : mstGameAppearanceData.titleTexturePath);
		AssetManager.LoadAssetData(DataInitializeResolver.titleLogo, AssetManager.OWNER.DataInitialize, 0, null);
		while (!AssetManager.IsLoadFinishAssetData(DataInitializeResolver.titleLogo))
		{
			yield return null;
		}
		DataInitializeResolver.<>c__DisplayClass12_0 CS$<>8__locals1 = new DataInitializeResolver.<>c__DisplayClass12_0();
		bool flag = true;
		CS$<>8__locals1.isFinish = false;
		CS$<>8__locals1.device_id = "";
		LoginManager.IsATTNotDetermined = false;
		if (flag && Application.RequestAdvertisingIdentifierAsync(delegate(string advertisingIdentifier, bool trackingEnabled, string error)
		{
			CS$<>8__locals1.device_id = advertisingIdentifier;
			CS$<>8__locals1.isFinish = true;
		}) && !CS$<>8__locals1.isFinish)
		{
			yield return null;
		}
		LoginManager.SetDeviceId(CS$<>8__locals1.device_id);
		CS$<>8__locals1 = null;
		DataManager.DmServerMst.InitializeAdvertiseBannerData();
		yield break;
	}

	// Token: 0x1700033A RID: 826
	// (get) Token: 0x06000EFA RID: 3834 RVA: 0x000B5310 File Offset: 0x000B3510
	private static string Consumer_key
	{
		get
		{
			RuntimePlatform platform = Application.platform;
			if (platform == RuntimePlatform.IPhonePlayer)
			{
				return DataInitializeResolver.NOAH_IOS_CONSUMER_KEY;
			}
			if (platform != RuntimePlatform.Android)
			{
				return "";
			}
			return DataInitializeResolver.NOAH_ANDROID_CONSUMER_KEY;
		}
	}

	// Token: 0x1700033B RID: 827
	// (get) Token: 0x06000EFB RID: 3835 RVA: 0x000B5340 File Offset: 0x000B3540
	private static string Secret_key
	{
		get
		{
			RuntimePlatform platform = Application.platform;
			if (platform == RuntimePlatform.IPhonePlayer)
			{
				return DataInitializeResolver.NOAH_IOS_SECRET_KEY;
			}
			if (platform != RuntimePlatform.Android)
			{
				return "";
			}
			return DataInitializeResolver.NOAH_ANDROID_SECRET_KEY;
		}
	}

	// Token: 0x1700033C RID: 828
	// (get) Token: 0x06000EFC RID: 3836 RVA: 0x000B5370 File Offset: 0x000B3570
	private static string Action_id
	{
		get
		{
			RuntimePlatform platform = Application.platform;
			if (platform == RuntimePlatform.IPhonePlayer)
			{
				return DataInitializeResolver.NOAH_IOS_ACTION_ID;
			}
			if (platform != RuntimePlatform.Android)
			{
				return "";
			}
			return DataInitializeResolver.NOAH_ANDROID_ACTION_ID;
		}
	}

	// Token: 0x06000EFD RID: 3837 RVA: 0x000B539F File Offset: 0x000B359F
	public static IEnumerator InitializeActionAfterTitle(bool isGPGConnect = false)
	{
		IEnumerator ienum = AssetDownloadResolver.ResolveActionNeedOndly();
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = null;
		ienum = Singleton<DataManager>.Instance.InitializeMstData();
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = null;
		List<string> list = AssetManager.LoadAssetDataByCategory(AssetManager.ASSET_CATEGORY_FACE_PACK, AssetManager.OWNER.DataInitialize, 0, null);
		foreach (string assetName in list)
		{
			while (!AssetManager.IsLoadFinishAssetData(AssetManager.PREFIX_PATH_FACE_PACK + assetName))
			{
				yield return null;
			}
			assetName = null;
		}
		List<string>.Enumerator enumerator = default(List<string>.Enumerator);
		FacePackData.GetAllPackData(true);
		ienum = Singleton<EffectManager>.Instance.MappingSePackData();
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = null;
		List<string> list2 = new List<string> { "ch_1000_a_mot.prefab", "assetpathparameter.asset" };
		foreach (string text in list2)
		{
			Data data = SGNFW.Ab.Manager.GetData(text);
			if (data != null)
			{
				SGNFW.Ab.Manager.Load(data, null, false);
				for (;;)
				{
					SGNFW.Ab.Manager.CheckState(data);
					if (data.IsLoaded)
					{
						break;
					}
					yield return null;
				}
			}
			data = null;
		}
		enumerator = default(List<string>.Enumerator);
		ienum = DataInitializeResolver.UpdateUserData(isGPGConnect);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = null;
		yield break;
		yield break;
	}

	// Token: 0x06000EFE RID: 3838 RVA: 0x000B53AE File Offset: 0x000B35AE
	private static IEnumerator UpdateUserData(bool isGPGConnect = false)
	{
		int[] opt = SceneManager.GetOption();
		DataInitializeResolver.<>c__DisplayClass26_0 CS$<>8__locals1 = new DataInitializeResolver.<>c__DisplayClass26_0();
		CS$<>8__locals1.isFinish = false;
		Singleton<LoginManager>.Instance.Login(delegate(Command cmd)
		{
			CS$<>8__locals1.isFinish = true;
			Singleton<DataManager>.Instance.UpdateUserAssetByLogin(cmd.response as LoginDmmResponse);
		}, "PARADEMSV", isGPGConnect);
		while (!CS$<>8__locals1.isFinish)
		{
			yield return null;
		}
		CS$<>8__locals1 = null;
		if (!Singleton<LoginManager>.Instance.serviceCloseData.IsServiceClose)
		{
			DataManager.DmUserInfo.RequestGetLoanPackList();
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			DataManager.DmUserInfo.RequestGetUserOption();
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			DataManager.DmPurchase.RequestGetPurchaseInternalList();
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			DataManager.DmQuest.RequestGetUserQuestInfo();
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			DataManager.DmMission.RequestGetMissionList();
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			DataManager.DmChMission.RequestGetMissionList();
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			DataManager.DmItem.RequestGetNewFlag();
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			DataManager.DmShop.RequestGetShopList();
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			DataManager.DmHome.RequestGetFriendInviteUrl();
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			DataManager.DmHome.RequestGetCollaboUrl();
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			DataManager.DmQuest.RequestGetSealedCharaDatas();
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			SoundManager.SetCategoryVolume(DataManager.DmUserInfo.optionData.Clone().VolumeList);
			CanvasManager.HdlOpenWindowSortFilter.Initialize();
		}
		if (!Singleton<LoginManager>.Instance.serviceCloseData.IsServiceClose)
		{
			UserOptionData userOptionData = DataManager.DmUserInfo.optionData.Clone();
			userOptionData.DisplayDirection = opt[0];
			userOptionData.Quality = opt[1];
			DataManager.DmUserInfo.RequestActionUpdateUserOption(userOptionData);
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			Singleton<CanvasManager>.Instance.SetDisplayDirection(DataManager.DmUserInfo.optionData.DisplayDirection);
			DataManager.DmUserInfo.optionData.SetDisplayQuality();
			DataManager.DmUserInfo.optionData.SetFrameRate();
		}
		yield break;
	}

	// Token: 0x06000EFF RID: 3839 RVA: 0x000B53BD File Offset: 0x000B35BD
	private static IEnumerator LoginStatusCheck()
	{
		bool isFinish = false;
		StatusCheckResponse response = null;
		Singleton<LoginManager>.Instance.StatusCheck(delegate(Command cmd)
		{
			response = cmd.response as StatusCheckResponse;
			isFinish = true;
		});
		while (!isFinish)
		{
			yield return null;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		if (response != null)
		{
			if (LoginManager.MaintenanceInfo != null && response.maintenance_login == 0)
			{
				flag = true;
			}
			if (response.dif_version == 1 || LoginManager.IsNeedVersionUp)
			{
				flag3 = true;
			}
			LoginManager.STATIS_RESULT result = (LoginManager.STATIS_RESULT)response.result;
			if (result != LoginManager.STATIS_RESULT.SUCCESS)
			{
				if (result != LoginManager.STATIS_RESULT.UUID_FAILURE)
				{
					if (result == LoginManager.STATIS_RESULT.NOT_EXIST_ACCOUNT)
					{
						flag4 = true;
					}
				}
				else
				{
					flag2 = true;
				}
			}
		}
		DataInitializeResolver.isDispInfoBeforeMst = flag || flag2 || flag3;
		if (flag)
		{
			DataInitializeResolver.<>c__DisplayClass27_1 CS$<>8__locals2 = new DataInitializeResolver.<>c__DisplayClass27_1();
			CS$<>8__locals2.isWindowFinish = false;
			CanvasManager.HdlOpenWindowBasic.Setup(LoginManager.MaintenanceInfo.title, LoginManager.MaintenanceInfo.text, new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>
			{
				new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "再起動"),
				new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "お知らせ")
			}, false, delegate(int index)
			{
				if (index == 1)
				{
					switch (LoginManager.MaintenanceInfo.link_type)
					{
					case 2:
					case 4:
						CanvasManager.HdlWebViewWindowCtrl.Open(LoginManager.MaintenanceInfo.link_adress);
						break;
					case 3:
						Application.OpenURL(LoginManager.MaintenanceInfo.link_adress);
						break;
					}
					return false;
				}
				CS$<>8__locals2.isWindowFinish = true;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			while (!CS$<>8__locals2.isWindowFinish)
			{
				yield return null;
			}
			yield return new KeyValuePair<SceneManager.SceneName, object>(SceneManager.SceneName.SceneDataInitialize, null);
			CS$<>8__locals2 = null;
		}
		else if (flag3)
		{
			bool isWindowFinish = false;
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("アップデート"), PrjUtil.MakeMessage("新しいバージョンがあります\nアプリを終了します"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.OK), false, delegate(int index)
			{
				PrjUtil.ForceShutdown();
				return false;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			while (!isWindowFinish)
			{
				yield return null;
			}
		}
		else if (flag2 || flag4)
		{
			DataInitializeResolver.<>c__DisplayClass27_2 CS$<>8__locals3 = new DataInitializeResolver.<>c__DisplayClass27_2();
			CS$<>8__locals3.isWindowFinish = false;
			string text = (flag4 ? PrjUtil.MakeMessage("ゲームデータが無効です\n\nアプリを再起動し\n新しくゲームデータを作成します") : PrjUtil.MakeMessage("この端末のゲームデータは\n他の端末に引き継ぎ済みです\n\nアプリを再起動し\n新しくゲームデータを作成します"));
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("ゲームデータの初期化"), text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.OK), false, delegate(int index)
			{
				CS$<>8__locals3.isWindowFinish = true;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			while (!CS$<>8__locals3.isWindowFinish)
			{
				yield return null;
			}
			Singleton<LoginManager>.Instance.DeleteAccountData();
			yield return new KeyValuePair<SceneManager.SceneName, object>(SceneManager.SceneName.SceneDataInitialize, null);
			CS$<>8__locals3 = null;
		}
		yield break;
	}

	// Token: 0x04000D8D RID: 3469
	public static readonly DataInitializeResolver.SEVER_ENV_INFO[] ServerEnvInfo = new DataInitializeResolver.SEVER_ENV_INFO[]
	{
		new DataInitializeResolver.SEVER_ENV_INFO
		{
			env = DataInitializeResolver.SEVER_ENV.LOCAL,
			url = "http://localhost:8080/paradesv_local/",
			ver = "1.0.0",
			proxy = null
		},
		new DataInitializeResolver.SEVER_ENV_INFO
		{
			env = DataInitializeResolver.SEVER_ENV.DEVELOP01,
			url = "https://parade-mobile-develop01-app.kemono-friends-3.jp/paradesv",
			ver = "1.0.0",
			proxy = null
		},
		new DataInitializeResolver.SEVER_ENV_INFO
		{
			env = DataInitializeResolver.SEVER_ENV.DEVELOP02,
			url = "https://parade-mobile-develop02-app.kemono-friends-3.jp/paradesv/",
			ver = "1.0.0",
			proxy = null
		},
		new DataInitializeResolver.SEVER_ENV_INFO
		{
			env = DataInitializeResolver.SEVER_ENV.DEVELOP03,
			url = "https://parade-mobile-develop03-app.kemono-friends-3.jp/paradesv/",
			ver = "1.0.0",
			proxy = null
		},
		new DataInitializeResolver.SEVER_ENV_INFO
		{
			env = DataInitializeResolver.SEVER_ENV.STAGE,
			url = "https://parade-mobile-stg-app.kemono-friends-3.jp/paradesv/",
			ver = "1.0.0",
			proxy = null
		},
		new DataInitializeResolver.SEVER_ENV_INFO
		{
			env = DataInitializeResolver.SEVER_ENV.PROD,
			url = "https://parade-mobile-prod-app.kemono-friends-3.jp/paradesv/",
			ver = "2.37.0",
			proxy = null
		},
		new DataInitializeResolver.SEVER_ENV_INFO
		{
			env = DataInitializeResolver.SEVER_ENV.DEVELOP04,
			url = "https://parade-mobile-develop04-app.kemono-friends-3.jp/paradesv/",
			ver = "1.0.0",
			proxy = null
		},
		new DataInitializeResolver.SEVER_ENV_INFO
		{
			env = DataInitializeResolver.SEVER_ENV.QA,
			url = "https://parade-mobile-qa-app.kemono-friends-3.jp/paradesv/",
			ver = "1.0.0",
			proxy = null
		}
	};

	// Token: 0x04000D8E RID: 3470
	private const int DEFAULT_SERVER = 5;

	// Token: 0x04000D8F RID: 3471
	public static DataInitializeResolver.SEVER_ENV_INFO ServerEnv = DataInitializeResolver.SEVER_ENV_INFO.DeepCopy(DataInitializeResolver.ServerEnvInfo[5]);

	// Token: 0x04000D90 RID: 3472
	public static string titleLogo;

	// Token: 0x04000D91 RID: 3473
	public static bool isRunningInitializeActionBeforeMst = false;

	// Token: 0x04000D92 RID: 3474
	public static bool isFinishedInitializeActionBeforeMst = false;

	// Token: 0x04000D93 RID: 3475
	public static bool isDispInfoBeforeMst = false;

	// Token: 0x04000D94 RID: 3476
	private static readonly string NOAH_IOS_CONSUMER_KEY = "APP_0585be92eb1b7822";

	// Token: 0x04000D95 RID: 3477
	private static readonly string NOAH_IOS_SECRET_KEY = "KEY_8935be92eb1b7884";

	// Token: 0x04000D96 RID: 3478
	private static readonly string NOAH_IOS_ACTION_ID = "OFF_4435be92ee9855bb";

	// Token: 0x04000D97 RID: 3479
	private static readonly string NOAH_ANDROID_CONSUMER_KEY = "APP_4225be9302807325";

	// Token: 0x04000D98 RID: 3480
	private static readonly string NOAH_ANDROID_SECRET_KEY = "KEY_6265be9302807373";

	// Token: 0x04000D99 RID: 3481
	private static readonly string NOAH_ANDROID_ACTION_ID = "OFF_8275be9305da08eb";

	// Token: 0x0200095A RID: 2394
	public class SEVER_ENV_INFO
	{
		// Token: 0x06003B8E RID: 15246 RVA: 0x001D5F6D File Offset: 0x001D416D
		public static DataInitializeResolver.SEVER_ENV_INFO DeepCopy(DataInitializeResolver.SEVER_ENV_INFO src)
		{
			return new DataInitializeResolver.SEVER_ENV_INFO
			{
				env = src.env,
				url = src.url,
				ver = src.ver,
				proxy = src.proxy
			};
		}

		// Token: 0x04003CA4 RID: 15524
		public DataInitializeResolver.SEVER_ENV env;

		// Token: 0x04003CA5 RID: 15525
		public string url;

		// Token: 0x04003CA6 RID: 15526
		public string ver;

		// Token: 0x04003CA7 RID: 15527
		public string proxy;
	}

	// Token: 0x0200095B RID: 2395
	public enum SEVER_ENV
	{
		// Token: 0x04003CA9 RID: 15529
		LOCAL,
		// Token: 0x04003CAA RID: 15530
		DEVELOP01,
		// Token: 0x04003CAB RID: 15531
		DEVELOP02,
		// Token: 0x04003CAC RID: 15532
		DEVELOP03,
		// Token: 0x04003CAD RID: 15533
		STAGE,
		// Token: 0x04003CAE RID: 15534
		PROD,
		// Token: 0x04003CAF RID: 15535
		DEVELOP04,
		// Token: 0x04003CB0 RID: 15536
		QA
	}
}
