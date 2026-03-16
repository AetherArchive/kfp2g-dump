using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;

public class HomeBannerCtrl : MonoBehaviour
{
	public void Init()
	{
		this.guiData = new HomeBannerCtrl.GUI(base.transform);
		this.banner = new Dictionary<GameObject, HomeBannerData>();
		this.timer = 0f;
		this.requestNextScene = SceneManager.SceneName.None;
		this.bannerList = new List<HomeBannerData>();
		ReuseScroll scrollView = this.guiData.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.onStartItem));
		ReuseScroll scrollView2 = this.guiData.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.onUpdateItem));
		this.guiData.ScrollView.Setup(0, 0);
	}

	private void MakeBannerList()
	{
		this.bannerList = new List<HomeBannerData>(DataManager.DmHome.GetHomeBannerList());
		this.bannerList.RemoveAll((HomeBannerData itm) => itm.actionType == HomeBannerData.ActionType.MOVE_MOVIE);
		List<HomeBannerData> list = new List<HomeBannerData>();
		using (List<HomeBannerData>.Enumerator enumerator = this.bannerList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				HomeBannerData hbd = enumerator.Current;
				DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventDataList().Find((DataManagerEvent.EventData x) => x.eventBannerId == hbd.bannerId);
				if (eventData != null && eventData.eventCategory == DataManagerEvent.Category.Mission)
				{
					list.Add(hbd);
				}
			}
		}
		foreach (HomeBannerData homeBannerData in list)
		{
			this.bannerList.Remove(homeBannerData);
		}
	}

	private void onStartItem(int index, GameObject go)
	{
		this.onUpdateItem(index, go);
		go.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			this.OnTouchRect(go);
		}, null, null, null, null);
	}

	private void OnTouchRect(GameObject go)
	{
		HomeBannerData homeBannerData = null;
		if (!this.banner.TryGetValue(go, out homeBannerData))
		{
			homeBannerData = null;
		}
		if (homeBannerData == null)
		{
			return;
		}
		SoundManager.Play("prd_se_click", false, false);
		if (homeBannerData.eventId > 0)
		{
			this.requestNextScene = SceneManager.SceneName.SceneQuest;
			this.requestNextArgs = new SceneQuest.Args
			{
				selectEventId = homeBannerData.eventId,
				isFromBanner = true
			};
			return;
		}
		switch (homeBannerData.actionType)
		{
		case HomeBannerData.ActionType.MOVE_GAME_SCENE:
		{
			this.requestNextScene = homeBannerData.actionParamScene;
			this.requestNextArgs = null;
			SceneManager.SceneName sceneName = this.requestNextScene;
			if (sceneName == SceneManager.SceneName.SceneShop)
			{
				this.requestNextArgs = new SceneShopArgs
				{
					resultNextSceneName = SceneManager.SceneName.SceneHome,
					shopId = homeBannerData.actionParamID
				};
				return;
			}
			if (sceneName == SceneManager.SceneName.SceneStoryView)
			{
				this.requestNextArgs = new SceneStoryView.Args
				{
					viewType = ((homeBannerData.actionParamID == 1) ? SceneStoryView.Args.VIEWTYPE.MOVIE : SceneStoryView.Args.VIEWTYPE.NONE),
					resultNextSceneName = SceneManager.SceneName.SceneHome,
					resultNextSceneArgs = null
				};
				return;
			}
			break;
		}
		case HomeBannerData.ActionType.OPEN_WEBVIEW:
		case HomeBannerData.ActionType.NOTICE:
			CanvasManager.HdlWebViewWindowCtrl.Open(homeBannerData.actionParamURL);
			this.requestNextArgs = null;
			return;
		case HomeBannerData.ActionType.OPEN_BROWSER:
			Application.OpenURL(homeBannerData.actionParamURL);
			this.requestNextArgs = null;
			return;
		case HomeBannerData.ActionType.NOAH_OFFER:
			PrjUtil.OpenOfferWallWebview();
			this.requestNextArgs = null;
			return;
		case HomeBannerData.ActionType.ATOM_INVITE:
			if (!string.IsNullOrEmpty(DataManager.DmHome.AtomInviteUrl))
			{
				Application.OpenURL(DataManager.DmHome.AtomInviteUrl);
			}
			this.requestNextArgs = null;
			return;
		case HomeBannerData.ActionType.MOVE_MOVIE:
			break;
		case HomeBannerData.ActionType.MOVE_PVP_SPECIAL:
		{
			bool flag = DataManager.DmGameStatus.MakeUserFlagData().ReleaseModeFlag.PvpMode == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
			if (flag)
			{
				DateTime now = TimeManager.Now;
				PvpStaticData pvpStaticDataBySeasonID = DataManager.DmPvp.GetPvpStaticDataBySeasonID(DataManager.DmPvp.GetSeasonIdByNow(now, PvpStaticData.Type.SPECIAL));
				if (pvpStaticDataBySeasonID == null || pvpStaticDataBySeasonID.seasonStartTime > now || now > pvpStaticDataBySeasonID.seasonEndTime)
				{
					flag = false;
				}
			}
			if (flag)
			{
				this.requestNextScene = SceneManager.SceneName.ScenePvp;
			}
			else
			{
				CanvasManager.HdlWebViewWindowCtrl.Open(homeBannerData.actionParamURL);
			}
			this.requestNextArgs = null;
			return;
		}
		case HomeBannerData.ActionType.FRIEND_INVITE:
			if (!string.IsNullOrEmpty(DataManager.DmHome.FriendInviteUrl))
			{
				Application.OpenURL(DataManager.DmHome.FriendInviteUrl);
			}
			this.requestNextArgs = null;
			return;
		case HomeBannerData.ActionType.EXCHANGE_PLAYER:
			if (!string.IsNullOrEmpty(DataManager.DmHome.CollaboUrl))
			{
				Application.OpenURL(DataManager.DmHome.CollaboUrl + "&campaign_id=" + homeBannerData.actionParamID.ToString());
			}
			this.requestNextArgs = null;
			break;
		default:
			return;
		}
	}

	private void onUpdateItem(int index, GameObject go)
	{
		int count = this.bannerList.Count;
		this.banner[go] = ((count > 0) ? this.bannerList[((index < 0) ? (count - -index % count) : index) % count] : null);
		go.GetComponent<PguiRawImageCtrl>().banner = ((this.banner[go] == null) ? "" : this.banner[go].bannerImagePath);
		go.transform.Find("Txt").GetComponent<PguiTextCtrl>().text = ((this.banner[go] == null) ? "" : this.banner[go].bannerText);
	}

	public void OnPlayAnimation(SimpleAnimation.ExPguiStatus uiType)
	{
		if (uiType == SimpleAnimation.ExPguiStatus.START)
		{
			base.gameObject.SetActive(true);
			this.guiData.HomeBanner.ExPlayAnimation(uiType, null);
			return;
		}
		this.guiData.HomeBanner.ExPlayAnimation(uiType, delegate
		{
			base.gameObject.SetActive(false);
		});
	}

	private void Update()
	{
		if (this.guiData.ScrollView.IsScroll || !CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			this.timer = 0f;
		}
		else if ((this.timer += TimeManager.DeltaTime) > 10f)
		{
			this.timer = 0f;
			this.guiData.ScrollView.NextFocusIndex(0.2f);
		}
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			this.MoveSceneByMenu(this.requestNextScene, this.requestNextArgs);
			this.requestNextScene = SceneManager.SceneName.None;
		}
		int num = this.bannerList.Count - this.guiData.ScrollView.CalcCurrentFocusIndex() - 1;
		for (int i = 0; i < this.guiData.Page.Count; i++)
		{
			this.guiData.Page[i].GetComponent<PguiReplaceSpriteCtrl>().Replace((i == num) ? 1 : 0);
		}
	}

	public void MoveSceneByMenu(SceneManager.SceneName nextScene, object args = null)
	{
		CanvasManager.HdlCmnMenu.MoveSceneByMenu(nextScene, args);
	}

	public void BannerRefresh()
	{
		this.banner = new Dictionary<GameObject, HomeBannerData>();
		this.MakeBannerList();
		int count = this.bannerList.Count;
		this.guiData.ScrollView.Resize(count, 0);
		this.guiData.ScrollView.gameObject.SetActive(count > 0);
		for (int i = 0; i < this.guiData.Page.Count; i++)
		{
			this.guiData.Page[i].gameObject.SetActive(i < count);
		}
	}

	private HomeBannerCtrl.GUI guiData;

	private Dictionary<GameObject, HomeBannerData> banner;

	private float timer;

	private SceneManager.SceneName requestNextScene;

	private object requestNextArgs;

	private List<HomeBannerData> bannerList = new List<HomeBannerData>();

	public class Args
	{
		public int actionParamID;
	}

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Page = new List<PguiImageCtrl>();
			int num = 1;
			for (;;)
			{
				Transform transform = baseTr.Find("PageDot/Page" + num.ToString("D2"));
				if (transform == null)
				{
					break;
				}
				this.Page.Add(transform.GetComponent<PguiImageCtrl>());
				num++;
			}
			this.HomeBanner = baseTr.GetComponent<SimpleAnimation>();
			this.ScrollView = baseTr.Find("ScrollView").GetComponent<ReuseScroll>();
		}

		public GameObject baseObj;

		public List<PguiImageCtrl> Page;

		public SimpleAnimation HomeBanner;

		public ReuseScroll ScrollView;
	}
}
