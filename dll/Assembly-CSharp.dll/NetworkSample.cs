using System;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Login;
using SGNFW.Mst;
using SGNFW.Thread;
using UnityEngine;

// Token: 0x0200010C RID: 268
public class NetworkSample : MonoBehaviour
{
	// Token: 0x06000CF3 RID: 3315 RVA: 0x00050818 File Offset: 0x0004EA18
	private void Start()
	{
		FitGUI.Initialize(1080, 1920, 24, 24, 32, 0, 0);
		base.gameObject.AddComponent<DataManager>().Initialize();
		base.gameObject.AddComponent<LoginManager>();
		base.gameObject.AddComponent<ThreadPool>();
		base.gameObject.AddComponent<MstManager>();
		Manager.SetUserAgent("sim", "SEGA Web Client for Project 2015");
		Manager.SetUserAgent("chat", "SEGA Web Client for Project 2015 CHAT");
		Manager.DefaultEncryptKey = "MEDARAP";
		Manager.AccountEncryptKey = "ACCOUNT_STRING";
		Verbose<Verbose>.Enabled = true;
		Verbose<PrjLog>.Enabled = true;
		this.state_ = NetworkSample.State.SelectServer;
		this.stateStr = "サーバ選択";
	}

	// Token: 0x06000CF4 RID: 3316 RVA: 0x000508C0 File Offset: 0x0004EAC0
	private void OnGUI()
	{
		GUILayout.BeginVertical(new GUILayoutOption[] { FitGUI.Width(FitGUI.orgWidth) });
		float orgWidth = FitGUI.orgWidth;
		float orgHeight = FitGUI.orgHeight;
		FitGUI.Label(this.sampleTitle_ + " " + this.stateStr, orgWidth);
		switch (this.state_)
		{
		case NetworkSample.State.SelectServer:
			this.OnGUI_SelectServer(orgWidth, orgHeight);
			goto IL_00B7;
		case NetworkSample.State.GetUrl:
			goto IL_00B7;
		case NetworkSample.State.Login:
			break;
		case NetworkSample.State.GetMstDataWait:
			if (!Singleton<MstManager>.Instance.IsLoadFinish)
			{
				goto IL_00B7;
			}
			this.state_ = NetworkSample.State.Login;
			break;
		case NetworkSample.State.AccountTransfer:
			this.OnGUI_AccountTransfer(orgWidth, orgHeight);
			goto IL_00B7;
		case NetworkSample.State.AccountTransferPass:
			this.OnGUI_AccountTransferPass(orgWidth, orgHeight);
			goto IL_00B7;
		case NetworkSample.State.Menu:
			this.state_ = NetworkSample.State.Menu;
			this.OnGUI_Menu(orgWidth, orgHeight);
			goto IL_00B7;
		default:
			goto IL_00B7;
		}
		this.OnGUI_Login(orgWidth, orgHeight);
		IL_00B7:
		GUILayout.EndVertical();
	}

	// Token: 0x06000CF5 RID: 3317 RVA: 0x0005098C File Offset: 0x0004EB8C
	private void OnGUI_SelectServer(float w, float h)
	{
		if (FitGUI.Button("Intra01", w, h * 0.1f, null))
		{
			Manager.SetServerRoot("root", "http://paradem-intra01.am2d.local:8080/paradesv/");
			this.GetUrl();
		}
		if (FitGUI.Button("Intra02", w, h * 0.1f, null))
		{
			Manager.SetServerRoot("root", "http://paradem-intra02.am2d.local:8080/paradesv/");
			this.GetUrl();
		}
		if (FitGUI.Button("Intra11", w, h * 0.1f, null))
		{
			Manager.SetServerRoot("root", "http://paradem-intra01.am2d.local:8080/paradesv11/");
			this.GetUrl();
		}
		if (FitGUI.Button("Develop01", w, h * 0.1f, null))
		{
			Manager.SetServerRoot("root", "http://ec2-18-223-4-178.us-east-2.compute.amazonaws.com/paradesv/");
			Manager.Proxy = "am2pr1.am2d.local:8080";
			this.GetUrl();
		}
		if (FitGUI.Button("Local", w, h * 0.1f, null))
		{
			Manager.SetServerRoot("root", "http://localhost:8080/paradesv/");
			this.GetUrl();
		}
	}

	// Token: 0x06000CF6 RID: 3318 RVA: 0x00050A75 File Offset: 0x0004EC75
	private void GetUrl()
	{
		Singleton<LoginManager>.Instance.GetUrl(delegate(Command res)
		{
			this.state_ = NetworkSample.State.Login;
			this.stateStr = "ログイン";
			this.account = LoginManager.Account;
		}, "1.0.0", 0, 2);
	}

	// Token: 0x06000CF7 RID: 3319 RVA: 0x00050A94 File Offset: 0x0004EC94
	private void OnGUI_Login(float w, float h)
	{
		if (FitGUI.Button("LOGIN", w, h * 0.1f, null))
		{
			Singleton<LoginManager>.Instance.LoadAccountData();
			this.StartLogin();
			return;
		}
		if (FitGUI.Button("CLEAR ACCOUNT", w, h * 0.1f, null))
		{
			Singleton<LoginManager>.Instance.DeleteAccountData();
			return;
		}
		if (FitGUI.Button("DEBUG LOAD ACCOUNT FILE", w, h * 0.1f, null))
		{
			Singleton<LoginManager>.Instance.DbgLoadAccountFile();
			return;
		}
		if (FitGUI.Button("ACCOUNT_TRANCEFER", w, h * 0.1f, null))
		{
			this.state_ = NetworkSample.State.AccountTransfer;
			return;
		}
		if (FitGUI.Button("STATUS CHECK", w, h * 0.1f, null))
		{
			Singleton<LoginManager>.Instance.LoadAccountData();
			Singleton<LoginManager>.Instance.StatusCheck(delegate(Command _cmd)
			{
			});
			return;
		}
		if (FitGUI.Button("Mst Data Get", w, h * 0.1f, null))
		{
			Singleton<MstManager>.Instance.Refresh();
			this.state_ = NetworkSample.State.GetMstDataWait;
			return;
		}
		if (FitGUI.Button("Mst Data Delete", w, h * 0.1f, null))
		{
			Singleton<MstManager>.Instance.RemoveMstDataCaches();
			return;
		}
		if (FitGUI.Button("Mst Data Client Ex", w, h * 0.1f, null))
		{
			Singleton<MstManager>.Instance.GetMst<List<MstLevelData>>(MstType.LEVEL_DATA);
			return;
		}
		if (FitGUI.Button("ServerConfig", w, h * 0.1f, null))
		{
			ServerConfigCmd.Create(0);
		}
	}

	// Token: 0x06000CF8 RID: 3320 RVA: 0x00050BF5 File Offset: 0x0004EDF5
	private void CbUserInfo(Command cmd, int resCode, string errorMsg)
	{
		if (resCode != 0)
		{
			this.stateStr = errorMsg;
			Verbose<PrjLog>.LogError(errorMsg, null);
			return;
		}
		Response response = cmd.response;
	}

	// Token: 0x06000CF9 RID: 3321 RVA: 0x00050C10 File Offset: 0x0004EE10
	private void OnGUI_AccountTransfer(float w, float h)
	{
		this.transferId_ = FitGUI.TextField(this.transferId_, w, h * 0.1f);
		this.password_ = FitGUI.TextField(this.password_, w, h * 0.1f);
		if (FitGUI.Button("EXEC ACCOUNT TRANCEFER", w, h * 0.1f, null))
		{
			Singleton<LoginManager>.Instance.AccountTransfer(delegate(Command _cmd)
			{
				this.transferId_ = "";
				this.password_ = "";
				this.state_ = NetworkSample.State.Login;
			}, this.transferId_, this.password_);
		}
		if (FitGUI.Button("BACK", w, h * 0.1f, null))
		{
			this.transferId_ = "";
			this.password_ = "";
			this.state_ = NetworkSample.State.Login;
		}
	}

	// Token: 0x06000CFA RID: 3322 RVA: 0x00050CB8 File Offset: 0x0004EEB8
	private void OnGUI_AccountTransferPass(float w, float h)
	{
		this.password_ = FitGUI.TextField(this.password_, w, h * 0.1f);
		if (FitGUI.Button("SET ACCOUNT TRANCEFER PASSWORD", w, h * 0.1f, null))
		{
			Singleton<LoginManager>.Instance.AccountTransferPassword(delegate(Command _cmd)
			{
				this.password_ = "";
				this.state_ = NetworkSample.State.Menu;
			}, this.password_);
		}
		if (FitGUI.Button("BACK", w, h * 0.1f, null))
		{
			this.transferId_ = "";
			this.password_ = "";
			this.state_ = NetworkSample.State.Menu;
		}
	}

	// Token: 0x06000CFB RID: 3323 RVA: 0x00050D41 File Offset: 0x0004EF41
	private void StartLogin()
	{
		Singleton<LoginManager>.Instance.Login(new Action<Command>(this.OnCompleteLogin), "PARADEMSV", false);
	}

	// Token: 0x06000CFC RID: 3324 RVA: 0x00050D5F File Offset: 0x0004EF5F
	private void OnCompleteLogin(Command cmd)
	{
		this.state_ = NetworkSample.State.Menu;
		this.stateStr = "ログイン完了";
	}

	// Token: 0x06000CFD RID: 3325 RVA: 0x00050D74 File Offset: 0x0004EF74
	private void OnGUI_Menu(float w, float h)
	{
		float width = FitGUI.GetWidth(w - 20f);
		float height = FitGUI.GetHeight(h - 60f);
		float num = w - 40f;
		float num2 = 150f;
		this.scrollViewVec = GUI.BeginScrollView(new Rect(FitGUI.GetWidth(10f), FitGUI.GetHeight(100f), width, height), this.scrollViewVec, new Rect(FitGUI.GetWidth(10f), FitGUI.GetHeight(100f), FitGUI.GetWidth(num), FitGUI.GetHeight((num2 + 10f) * 48f)));
		if (FitGUI.Button("Gacha", num, num2, null))
		{
			GachaCmd.Create();
		}
		else if (FitGUI.Button("GachaExe", num, num2, null))
		{
			GachaExecCmd.Create(4000001, 2, 30002);
		}
		else if (FitGUI.Button("setTransferPsss", num, num2, null))
		{
			this.state_ = NetworkSample.State.AccountTransferPass;
		}
		else if (FitGUI.Button("Photo", num, num2, null))
		{
			PhotoCmd.Create();
		}
		else if (FitGUI.Button("PhotoGrow", num, num2, null))
		{
			PhotoGrowCmd.Create(60000000001L, new List<long> { 60000000002L, 60000000003L, 60000000004L });
		}
		else if (FitGUI.Button("PhotoLock", num, num2, null))
		{
			PhotoLockCmd.Create(new List<long> { 2L, 3L, 4L }, new List<long> { 1L });
		}
		else if (FitGUI.Button("PhotoSell", num, num2, null))
		{
			PhotoSellCmd.Create(new List<long> { 2L, 3L, 4L });
		}
		else if (FitGUI.Button("Chara", num, num2, null))
		{
			CharaCmd.Create();
		}
		else if (FitGUI.Button("CharaLevelUp", num, num2, null))
		{
			CharaLevelUpCmd.Create(1, new List<UseItem>
			{
				new UseItem
				{
					use_item_id = 13001,
					use_item_num = 50
				}
			});
		}
		else if (FitGUI.Button("CharaFavoriteFlag", num, num2, null))
		{
			CharaFavoriteFlagCmd.Create(1);
		}
		else if (FitGUI.Button("CharaWildRel", num, num2, null))
		{
			CharaWildRelCmd.Create(1, new List<WildResult>(), 1);
		}
		else if (FitGUI.Button("CharaArtsUp", num, num2, null))
		{
			CharaArtsUpCmd.Create(1, 2);
		}
		else if (FitGUI.Button("CharaRankUp", num, num2, null))
		{
			CharaRankUpCmd.Create(1, 2);
		}
		else if (FitGUI.Button("CharaPpRel", num, num2, null))
		{
			CharaPpRelCmd.Create(1, 2);
		}
		else if (FitGUI.Button("CharaChangeClothes", num, num2, null))
		{
			CharaChangeClothesCmd.Create(1, 2);
		}
		else if (FitGUI.Button("CharaPxChange", num, num2, null))
		{
			CharaPxChangeCmd.Create(1, 5);
		}
		else if (FitGUI.Button("CharaSpxChange", num, num2, null))
		{
			CharaSpxChangeCmd.Create(1, 5);
		}
		else if (FitGUI.Button("Quest", num, num2, null))
		{
			QuestCmd.Create(0);
		}
		else if (FitGUI.Button("QuestStart", num, num2, null))
		{
			QuestStartCmd questStartCmd = QuestStartCmd.Create(10010101, 1, 0, 2, 999999999, null);
			questStartCmd.onSuccess = (Action<Command>)Delegate.Combine(questStartCmd.onSuccess, new Action<Command>(delegate(Command _cmd)
			{
				this.questStartHash = (_cmd.response as QuestStartResponse).hash_id;
			}));
		}
		else if (FitGUI.Button("QuestEnd", num, num2, null))
		{
			QuestEndCmd.Create(10010101, 1, 1, 10, this.questStartHash, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null);
		}
		else if (FitGUI.Button("DeckList", num, num2, null))
		{
			DeckListCmd.Create(0);
		}
		else if (FitGUI.Button("DeckUpdate", num, num2, null))
		{
			DeckUpdateCmd.Create(new List<Decks>());
		}
		else if (FitGUI.Button("PresentBox", num, num2, null))
		{
			PresentBoxCmd.Create(12, 13);
		}
		else if (FitGUI.Button("PresentGet", num, num2, null))
		{
			PresentGetCmd.Create(new List<long> { 1L }, 0, 100, 0, 100);
		}
		else if (FitGUI.Button("ItemReceiveHistory", num, num2, null))
		{
			ReceiveHistoryCmd.Create(12, 13);
		}
		else if (FitGUI.Button("PurcaseInfo", num, num2, null))
		{
			PurchaseInfoCmd.Create(null, false);
		}
		else if (FitGUI.Button("Purcase", num, num2, null))
		{
			PurchaseCmd.Create("", "", "android01", "", 0, "", new List<string>());
		}
		else if (FitGUI.Button("Shop List", num, num2, null))
		{
			ShopListCmd.Create();
		}
		else if (FitGUI.Button("Shop Buy", num, num2, null))
		{
			ShopBuyCmd.Create(1, 2);
		}
		else if (FitGUI.Button("Option Get", num, num2, null))
		{
			OptionGetCmd.Create();
		}
		else if (FitGUI.Button("Option Set", num, num2, null))
		{
			OptionSetCmd.Create(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
		}
		else if (FitGUI.Button("HelperBattleList", num, num2, null))
		{
			HelperBattleListCmd.Create(0);
		}
		else if (FitGUI.Button("HelperMineExcec", num, num2, null))
		{
			HelperMineExcecCmd.Create(1, new List<int> { 101573403, 189345573 });
		}
		else if (FitGUI.Button("HelperSearch", num, num2, null))
		{
			HelperSearchCmd.Create(101573403);
		}
		else if (FitGUI.Button("MyHelperList", num, num2, null))
		{
			MyHelperListCmd.Create();
		}
		else if (FitGUI.Button("MyHelperChange", num, num2, null))
		{
			MyHelperChangeCmd.Create(new List<MyHelper>
			{
				new MyHelper
				{
					chara_id = 2,
					photo_id00 = 0L,
					photo_id01 = 0L,
					photo_id02 = 0L,
					photo_id03 = 0L
				}
			});
		}
		else if (FitGUI.Button("PlayerInfo", num, num2, null))
		{
			PlayerInfoCmd.Create();
		}
		else if (FitGUI.Button("PlayerInfoChange", num, num2, null))
		{
			PlayerInfoChangeCmd.Create(new PlayerInfo
			{
				player_name = "変更サンプルユーザー"
			});
		}
		else if (FitGUI.Button("HomeCheckAction", num, num2, null))
		{
			HomeCheckCmd.Create(100);
		}
		else if (FitGUI.Button("FunitureChangeAction", num, num2, null))
		{
			FurnitureChangeCmd.Create(new List<Furniture>
			{
				new Furniture
				{
					furniture_id = 1,
					placement_id = 2
				}
			});
		}
		else if (FitGUI.Button("Recovery", num, num2, null))
		{
			RecoveryCmd.Create(31001, 8, 1);
		}
		else if (FitGUI.Button("MissionList", num, num2, null))
		{
			MissionListCmd.Create(new List<int>());
		}
		else if (FitGUI.Button("MissionBonusAccept", num, num2, null))
		{
			MissionBonusAcceptCmd.Create(new List<int> { 1001, 1002 }, null);
		}
		else if (FitGUI.Button("FriendsData", num, num2, null))
		{
			FriendsDataCmd.Create(new List<FriendsData>
			{
				new FriendsData
				{
					chara_id = 1,
					chara_name = "ドール",
					atk_param_lv99 = 1980,
					def_param_lv99 = 1980,
					hp_param_lv99 = 5410,
					max_stock_kp = 40,
					avoid_ratio = 0,
					total_param = 9984
				}
			});
		}
		else if (FitGUI.Button("NewFlgGetList", num, num2, null))
		{
			NewFlgGetListCmd.Create(0);
		}
		else if (FitGUI.Button("NewFlgUpdate", num, num2, null))
		{
			NewFlgUpdateCmd.Create(new List<NewFlg>
			{
				new NewFlg
				{
					any_id = 100,
					category = 5,
					new_mgmt_flg = 0
				}
			});
		}
		else if (FitGUI.Button("ItemSell", num, num2, null))
		{
			ItemSellCmd.Create(new List<Item>
			{
				new Item
				{
					item_id = 11038,
					item_num = 2
				}
			});
		}
		else if (FitGUI.Button("NanairoRelease", num, num2, null))
		{
			CharaNanairoAbilityReleaseCmd.Create(1);
		}
		GUI.EndScrollView();
	}

	// Token: 0x04000A57 RID: 2647
	private string sampleTitle_ = "Network Sample";

	// Token: 0x04000A58 RID: 2648
	private string stateStr = "";

	// Token: 0x04000A59 RID: 2649
	private NetworkSample.State state_;

	// Token: 0x04000A5A RID: 2650
	private string account = "";

	// Token: 0x04000A5B RID: 2651
	private string transferId_ = "";

	// Token: 0x04000A5C RID: 2652
	private string password_ = "";

	// Token: 0x04000A5D RID: 2653
	private Vector2 scrollViewVec = Vector2.zero;

	// Token: 0x04000A5E RID: 2654
	private const int btnCount = 48;

	// Token: 0x04000A5F RID: 2655
	private long questStartHash;

	// Token: 0x0200084D RID: 2125
	private enum State
	{
		// Token: 0x04003764 RID: 14180
		SelectServer,
		// Token: 0x04003765 RID: 14181
		GetUrl,
		// Token: 0x04003766 RID: 14182
		Login,
		// Token: 0x04003767 RID: 14183
		GetMstDataWait,
		// Token: 0x04003768 RID: 14184
		AccountTransfer,
		// Token: 0x04003769 RID: 14185
		AccountTransferPass,
		// Token: 0x0400376A RID: 14186
		Menu
	}
}
