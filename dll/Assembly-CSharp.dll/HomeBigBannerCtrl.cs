using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;

public class HomeBigBannerCtrl : MonoBehaviour
{
	public void Init()
	{
		this.guiData = new HomeBigBannerCtrl.GUI(base.transform);
		this.banner = new Dictionary<GameObject, DataManagerEvent.EventBannerData>();
		this.timer = 0f;
		this.requestNextScene = SceneManager.SceneName.None;
		this.bannerList = new List<DataManagerEvent.EventBannerData>();
		ReuseScroll scrollView = this.guiData.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.onStartItem));
		ReuseScroll scrollView2 = this.guiData.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.onUpdateItem));
		this.guiData.ScrollView.Setup(0, 0);
	}

	private void MakeBannerList()
	{
		this.bannerList = new List<DataManagerEvent.EventBannerData>(DataManager.DmEvent.GetEventBannerDataList());
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
		DataManagerEvent.EventBannerData eventBannerData = null;
		if (!this.banner.TryGetValue(go, out eventBannerData))
		{
			eventBannerData = null;
		}
		if (eventBannerData == null)
		{
			return;
		}
		SoundManager.Play("prd_se_click", false, false);
		switch (eventBannerData.LinkType)
		{
		case DataManagerEvent.EventBannerData.Type.Move:
		{
			PrjUtil.EnumTryParse<SceneManager.SceneName>(eventBannerData.LinkAddress, true, out this.requestNextScene);
			this.requestNextArgs = null;
			SceneManager.SceneName sceneName = this.requestNextScene;
			if (sceneName == SceneManager.SceneName.SceneQuest)
			{
				this.requestNextArgs = new SceneQuest.Args
				{
					selectEventId = eventBannerData.LinkValue
				};
				return;
			}
			if (sceneName == SceneManager.SceneName.SceneShop)
			{
				this.requestNextArgs = new SceneShopArgs
				{
					resultNextSceneName = SceneManager.SceneName.SceneHome,
					shopId = eventBannerData.LinkValue
				};
				return;
			}
			if (sceneName != SceneManager.SceneName.SceneStoryView)
			{
				return;
			}
			this.requestNextArgs = new SceneStoryView.Args
			{
				viewType = ((eventBannerData.LinkValue == 1) ? SceneStoryView.Args.VIEWTYPE.MOVIE : SceneStoryView.Args.VIEWTYPE.NONE),
				resultNextSceneName = SceneManager.SceneName.SceneHome,
				resultNextSceneArgs = null
			};
			return;
		}
		case DataManagerEvent.EventBannerData.Type.WebView:
		case DataManagerEvent.EventBannerData.Type.HomeInfo:
			CanvasManager.HdlWebViewWindowCtrl.Open(eventBannerData.LinkAddress);
			this.requestNextArgs = null;
			return;
		case DataManagerEvent.EventBannerData.Type.Browser:
			Application.OpenURL(eventBannerData.LinkAddress);
			this.requestNextArgs = null;
			return;
		case DataManagerEvent.EventBannerData.Type.Noah:
			PrjUtil.OpenOfferWallWebview();
			this.requestNextArgs = null;
			return;
		case DataManagerEvent.EventBannerData.Type.AtomInvite:
			if (!string.IsNullOrEmpty(DataManager.DmHome.AtomInviteUrl))
			{
				Application.OpenURL(DataManager.DmHome.AtomInviteUrl);
			}
			this.requestNextArgs = null;
			return;
		case DataManagerEvent.EventBannerData.Type.FriendInvite:
			if (!string.IsNullOrEmpty(DataManager.DmHome.FriendInviteUrl))
			{
				Application.OpenURL(DataManager.DmHome.FriendInviteUrl);
			}
			this.requestNextArgs = null;
			return;
		default:
			return;
		}
	}

	private void onUpdateItem(int index, GameObject go)
	{
		int count = this.bannerList.Count;
		this.banner[go] = ((count > 0) ? this.bannerList[((index < 0) ? (count - -index % count) : index) % count] : null);
		go.GetComponent<PguiRawImageCtrl>().banner = ((this.banner[go] == null) ? "" : this.banner[go].BannerFilename);
		go.transform.Find("Info/Txt").GetComponent<PguiTextCtrl>().text = ((this.banner[go] == null) ? "" : this.banner[go].BannerText);
	}

	public void OnPlayAnimation(SimpleAnimation.ExPguiStatus uiType)
	{
		if (uiType == SimpleAnimation.ExPguiStatus.START)
		{
			base.gameObject.SetActive(true);
			this.guiData.BigBanner.ExPlayAnimation(uiType, null);
			return;
		}
		this.guiData.BigBanner.ExPlayAnimation(uiType, delegate
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
		this.banner = new Dictionary<GameObject, DataManagerEvent.EventBannerData>();
		this.MakeBannerList();
		int num = this.bannerList.Count;
		this.guiData.ScrollView.Resize(num, 0);
		this.guiData.ScrollView.gameObject.SetActive(num > 0);
		this.guiData.ScrollView.GetComponent<FixedScrollRect>().enabled = num > 1;
		if (num <= 1)
		{
			num = 0;
		}
		for (int i = 0; i < this.guiData.Page.Count; i++)
		{
			this.guiData.Page[i].gameObject.SetActive(i < num);
		}
	}

	private HomeBigBannerCtrl.GUI guiData;

	private Dictionary<GameObject, DataManagerEvent.EventBannerData> banner;

	private float timer;

	private SceneManager.SceneName requestNextScene;

	private object requestNextArgs;

	private List<DataManagerEvent.EventBannerData> bannerList = new List<DataManagerEvent.EventBannerData>();

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
				Transform transform = baseTr.Find("Banner/PageDot/Page" + num.ToString("D2"));
				if (transform == null)
				{
					break;
				}
				this.Page.Add(transform.GetComponent<PguiImageCtrl>());
				num++;
			}
			this.BigBanner = baseTr.GetComponent<SimpleAnimation>();
			this.ScrollView = baseTr.Find("Banner/ScrollView").GetComponent<ReuseScroll>();
		}

		public GameObject baseObj;

		public List<PguiImageCtrl> Page;

		public SimpleAnimation BigBanner;

		public ReuseScroll ScrollView;
	}
}
