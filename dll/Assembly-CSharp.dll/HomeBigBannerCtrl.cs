using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;

// Token: 0x0200014D RID: 333
public class HomeBigBannerCtrl : MonoBehaviour
{
	// Token: 0x0600128A RID: 4746 RVA: 0x000DFFE0 File Offset: 0x000DE1E0
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

	// Token: 0x0600128B RID: 4747 RVA: 0x000E0090 File Offset: 0x000DE290
	private void MakeBannerList()
	{
		this.bannerList = new List<DataManagerEvent.EventBannerData>(DataManager.DmEvent.GetEventBannerDataList());
	}

	// Token: 0x0600128C RID: 4748 RVA: 0x000E00A8 File Offset: 0x000DE2A8
	private void onStartItem(int index, GameObject go)
	{
		this.onUpdateItem(index, go);
		go.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			this.OnTouchRect(go);
		}, null, null, null, null);
	}

	// Token: 0x0600128D RID: 4749 RVA: 0x000E00F8 File Offset: 0x000DE2F8
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

	// Token: 0x0600128E RID: 4750 RVA: 0x000E0274 File Offset: 0x000DE474
	private void onUpdateItem(int index, GameObject go)
	{
		int count = this.bannerList.Count;
		this.banner[go] = ((count > 0) ? this.bannerList[((index < 0) ? (count - -index % count) : index) % count] : null);
		go.GetComponent<PguiRawImageCtrl>().banner = ((this.banner[go] == null) ? "" : this.banner[go].BannerFilename);
		go.transform.Find("Info/Txt").GetComponent<PguiTextCtrl>().text = ((this.banner[go] == null) ? "" : this.banner[go].BannerText);
	}

	// Token: 0x0600128F RID: 4751 RVA: 0x000E032C File Offset: 0x000DE52C
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

	// Token: 0x06001290 RID: 4752 RVA: 0x000E0378 File Offset: 0x000DE578
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

	// Token: 0x06001291 RID: 4753 RVA: 0x000E046A File Offset: 0x000DE66A
	public void MoveSceneByMenu(SceneManager.SceneName nextScene, object args = null)
	{
		CanvasManager.HdlCmnMenu.MoveSceneByMenu(nextScene, args);
	}

	// Token: 0x06001292 RID: 4754 RVA: 0x000E0478 File Offset: 0x000DE678
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

	// Token: 0x04000F3C RID: 3900
	private HomeBigBannerCtrl.GUI guiData;

	// Token: 0x04000F3D RID: 3901
	private Dictionary<GameObject, DataManagerEvent.EventBannerData> banner;

	// Token: 0x04000F3E RID: 3902
	private float timer;

	// Token: 0x04000F3F RID: 3903
	private SceneManager.SceneName requestNextScene;

	// Token: 0x04000F40 RID: 3904
	private object requestNextArgs;

	// Token: 0x04000F41 RID: 3905
	private List<DataManagerEvent.EventBannerData> bannerList = new List<DataManagerEvent.EventBannerData>();

	// Token: 0x02000AEE RID: 2798
	public class Args
	{
		// Token: 0x04004545 RID: 17733
		public int actionParamID;
	}

	// Token: 0x02000AEF RID: 2799
	public class GUI
	{
		// Token: 0x060040F9 RID: 16633 RVA: 0x001F7D20 File Offset: 0x001F5F20
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

		// Token: 0x04004546 RID: 17734
		public GameObject baseObj;

		// Token: 0x04004547 RID: 17735
		public List<PguiImageCtrl> Page;

		// Token: 0x04004548 RID: 17736
		public SimpleAnimation BigBanner;

		// Token: 0x04004549 RID: 17737
		public ReuseScroll ScrollView;
	}
}
