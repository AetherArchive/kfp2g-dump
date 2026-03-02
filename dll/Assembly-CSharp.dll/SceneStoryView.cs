using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Common;
using SGNFW.Mst;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200015E RID: 350
public class SceneStoryView : BaseScene
{
	// Token: 0x06001406 RID: 5126 RVA: 0x000F3A7C File Offset: 0x000F1C7C
	public override void OnCreateScene()
	{
		this.basePanel = AssetManager.InstantiateAssetData("SceneMenu/GUI/Prefab/GUI_Memories", null).GetComponent<SimpleAnimation>();
		PguiPanel pguiPanel = this.basePanel.GetComponent<PguiPanel>();
		if (pguiPanel != null)
		{
			pguiPanel.raycastTarget = false;
		}
		this.moviePanel = AssetManager.InstantiateAssetData("SceneMenu/GUI/Prefab/GUI_Movie", null).GetComponent<SimpleAnimation>();
		pguiPanel = this.moviePanel.GetComponent<PguiPanel>();
		if (pguiPanel != null)
		{
			pguiPanel.raycastTarget = false;
		}
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.moviePanel.transform, true);
		this.guiData = new SceneStoryView.GUI(this.basePanel.transform);
		this.movieData = new SceneStoryView.MOVIE(this.moviePanel.transform);
		this.movieCtrl = new GameObject("movie", new Type[] { typeof(RectTransform) });
		RectTransform component = this.movieCtrl.GetComponent<RectTransform>();
		component.SetParent(this.basePanel.transform, false);
		component.sizeDelta = new Vector2(1280f, 720f);
		this.movieCtrl.AddComponent<RawImage>();
		this.movieCtrl.SetActive(false);
		this.hidePanel = new GameObject("hide").transform;
		this.hidePanel.SetParent(this.basePanel.transform, false);
		this.hidePanel.gameObject.SetActive(false);
		this.charaPlate = AssetManager.InstantiateAssetData("SceneCharaEdit/GUI/Prefab/CharaGrow_Btn_CharaSelect", null);
		this.charaPlate.transform.SetParent(this.hidePanel, false);
		this.moviePlate = AssetManager.InstantiateAssetData("SceneMenu/GUI/Prefab/Memories_Btn_MovieSelect", null);
		this.moviePlate.transform.SetParent(this.hidePanel, false);
		this.guiData.BtnScenario.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnMovie.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnPhotoAlbum.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPhotoAlbumButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnCommunication.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnMain.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnFriends.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnEvent.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnArai.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnPvp.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.movieData.BtnSkip.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.movieData.BtnPlay.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.movieData.BtnStop.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.haveFriendsPackList = new List<CharaPackData>();
		this.dispFriendsPackList = new List<CharaPackData>();
		this.sortType = SortFilterDefine.SortType.LEVEL;
		this.friendsList = new List<Transform>();
		this.friendsScroll = this.guiData.friend.transform.Find("All/WindowAll/ScrollView").GetComponent<ReuseScroll>();
		this.friendsScroll.InitForce();
		ReuseScroll reuseScroll = this.friendsScroll;
		reuseScroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll.onStartItem, new Action<int, GameObject>(this.SetupFriends));
		ReuseScroll reuseScroll2 = this.friendsScroll;
		reuseScroll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll2.onUpdateItem, new Action<int, GameObject>(this.UpdateFriends));
		this.friendsScroll.Setup(0, 0);
		this.chapterList = new Dictionary<int, List<int>>();
		this.chapterKey = new List<int>();
		this.chapterScroll = this.guiData.chapter.transform.Find("All/WindowAll/ScrollView").GetComponent<ReuseScroll>();
		this.chapterScroll.InitForce();
		ReuseScroll reuseScroll3 = this.chapterScroll;
		reuseScroll3.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll3.onStartItem, new Action<int, GameObject>(this.SetupChapter));
		ReuseScroll reuseScroll4 = this.chapterScroll;
		reuseScroll4.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll4.onUpdateItem, new Action<int, GameObject>(this.UpdateChapter));
		this.chapterScroll.Setup(0, 0);
		this.chapterBtn = new Dictionary<PguiButtonCtrl, int>();
		this.bookmarkStoryButton = this.guiData.chapter.transform.Find("All/WindowAll/ButtonL").GetComponent<PguiButtonCtrl>();
		this.bookmarkStoryButton.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.storyList = new List<int>();
		this.storyScroll = this.guiData.story.transform.Find("All/WindowAll/ScrollView").GetComponent<ReuseScroll>();
		this.storyScroll.InitForce();
		ReuseScroll reuseScroll5 = this.storyScroll;
		reuseScroll5.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll5.onStartItem, new Action<int, GameObject>(this.SetupStory));
		ReuseScroll reuseScroll6 = this.storyScroll;
		reuseScroll6.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll6.onUpdateItem, new Action<int, GameObject>(this.UpdateStory));
		this.storyScroll.Setup(0, 0);
		this.storyBtn = new Dictionary<PguiButtonCtrl, int>();
		this.movieList = new List<Transform>();
		this.movieScroll = this.guiData.movie.transform.Find("All/WindowAll/ScrollView").GetComponent<ReuseScroll>();
		this.movieScroll.InitForce();
		ReuseScroll reuseScroll7 = this.movieScroll;
		reuseScroll7.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll7.onStartItem, new Action<int, GameObject>(this.SetupMovie));
		ReuseScroll reuseScroll8 = this.movieScroll;
		reuseScroll8.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll8.onUpdateItem, new Action<int, GameObject>(this.SetupMovie));
		this.movieScroll.Setup(0, 0);
		this.CommunicationCtrl = new CommunicationCtrl();
		new List<int>();
		this.CommunicationCtrl.FilterButton = this.guiData.communicationSelect.transform.Find("All/WindowAll/SortFilterBtnsAll/Btn_FilterOnOff").GetComponent<PguiButtonCtrl>();
		this.CommunicationCtrl.SortButton = this.guiData.communicationSelect.transform.Find("All/WindowAll/SortFilterBtnsAll/Btn_Sort").GetComponent<PguiButtonCtrl>();
		this.CommunicationCtrl.SortUpDownButton = this.guiData.communicationSelect.transform.Find("All/WindowAll/SortFilterBtnsAll/Btn_SortUpDown").GetComponent<PguiButtonCtrl>();
		this.CommunicationCtrl.CharaSelectScroll = this.guiData.communicationSelect.transform.Find("All/WindowAll/ScrollView").GetComponent<ReuseScroll>();
		this.CommunicationCtrl.CharaSelectScroll.InitForce();
		ReuseScroll charaSelectScroll = this.CommunicationCtrl.CharaSelectScroll;
		charaSelectScroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(charaSelectScroll.onStartItem, new Action<int, GameObject>(this.SetupCommunicationSelect));
		ReuseScroll charaSelectScroll2 = this.CommunicationCtrl.CharaSelectScroll;
		charaSelectScroll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(charaSelectScroll2.onUpdateItem, new Action<int, GameObject>(this.UpdateCommunicationSelect));
		this.CommunicationCtrl.CharaSelectScroll.Setup(0, 0);
		this.CommunicationCtrl.CharaViewInitialize(this.guiData.communication.gameObject);
		this.storyBtn = new Dictionary<PguiButtonCtrl, int>();
		this.basePanel.gameObject.SetActive(false);
		this.moviePanel.gameObject.SetActive(false);
	}

	// Token: 0x06001407 RID: 5127 RVA: 0x000F41E0 File Offset: 0x000F23E0
	public override void OnEnableScene(object args)
	{
		this.requestNextScene = SceneManager.SceneName.None;
		this.requestNextSceneArgs = null;
		this.SceneArgs = args as SceneStoryView.Args;
		this.mode = SceneStoryView.MODE.SELECT;
		string text = SceneStoryView.TITLE_MEMORYS;
		if (this.SceneArgs != null)
		{
			if (this.SceneArgs.viewType == SceneStoryView.Args.VIEWTYPE.MOVIE)
			{
				this.mode = SceneStoryView.MODE.MOVIE;
				text = SceneStoryView.TITLE_MOVIE;
			}
			else if (this.SceneArgs.viewType == SceneStoryView.Args.VIEWTYPE.PVPEVENT)
			{
				this.mode = SceneStoryView.MODE.PVP;
				text = SceneStoryView.TITLE_OTHER;
				this.isLoginScenario = false;
			}
		}
		CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage(text), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", null, null);
		CanvasManager.SetBgTexture("selbg_home_in");
		SoundManager.PlayBGM("prd_bgm0013");
		this.guiData.select.gameObject.SetActive(false);
		this.guiData.scenario.gameObject.SetActive(false);
		this.guiData.chapter.gameObject.SetActive(false);
		this.guiData.friend.gameObject.SetActive(false);
		this.guiData.story.gameObject.SetActive(false);
		this.guiData.nothing.gameObject.SetActive(false);
		this.guiData.movie.gameObject.SetActive(false);
		this.guiData.communication.gameObject.SetActive(false);
		this.guiData.communicationSelect.gameObject.SetActive(false);
		this.basePanel.gameObject.SetActive(true);
		this.basePanel.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.moviePanel.gameObject.SetActive(false);
		this.haveFriendsPackList = new List<CharaPackData>(DataManager.DmChara.GetUserCharaMap().Values);
		this.dispFriendsPackList = new List<CharaPackData>(this.haveFriendsPackList);
		this.sortType = SortFilterDefine.SortType.LEVEL;
		this.friendsList = new List<Transform>();
		foreach (CharaPackData charaPackData in this.dispFriendsPackList)
		{
			GameObject obj = Object.Instantiate<GameObject>(this.charaPlate);
			obj.transform.SetParent(this.hidePanel, false);
			obj.name = charaPackData.id.ToString();
			if (obj.GetComponent<PguiTouchTrigger>() == null)
			{
				obj.AddComponent<PguiTouchTrigger>().AddListener(delegate
				{
					this.OnClickFriends(obj);
				}, delegate
				{
					this.OnClickFriends(obj);
				}, null, null, null);
			}
			obj.GetComponent<IconCharaCtrl>().Setup(charaPackData, this.sortType, false, null, 0, -1, 0);
			obj.GetComponent<IconCharaCtrl>().SetupStoryInfo(charaPackData, true);
			this.friendsList.Add(obj.transform);
		}
		this.currentCharaId = PlayerPrefs.GetInt("bookmarkCharaId");
		this.UpdateBookmarkChara(this.currentCharaId);
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData
		{
			register = SortFilterDefine.RegisterType.STORY_VIEW,
			filterButton = this.guiData.friend.transform.Find("All/WindowAll/SortFilterBtnsAll/Btn_FilterOnOff").GetComponent<PguiButtonCtrl>(),
			sortButton = this.guiData.friend.transform.Find("All/WindowAll/SortFilterBtnsAll/Btn_Sort").GetComponent<PguiButtonCtrl>(),
			sortUdButton = this.guiData.friend.transform.Find("All/WindowAll/SortFilterBtnsAll/Btn_SortUpDown").GetComponent<PguiButtonCtrl>(),
			funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget
			{
				charaList = this.haveFriendsPackList
			},
			funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this.dispFriendsPackList = item.charaList;
				this.sortType = item.sortType;
				this.friendsScroll.Resize((this.dispFriendsPackList.Count + 3 - 1) / 3, 0);
			}
		};
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData, true, null);
		this.chapterList = new Dictionary<int, List<int>>();
		this.chapterKey = new List<int>();
		this.chapterScroll.Resize(0, 0);
		this.chapterBtn = new Dictionary<PguiButtonCtrl, int>();
		this.storyList = new List<int>();
		this.storyScroll.Resize(0, 0);
		this.storyBtn = new Dictionary<PguiButtonCtrl, int>();
		this.movieList = new List<Transform>();
		foreach (MstMovieData mstMovieData in DataManager.DmServerMst.mstMovieDataList)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.moviePlate);
			gameObject.transform.SetParent(this.hidePanel, false);
			gameObject.name = mstMovieData.movieId.ToString();
			gameObject.GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMovie), PguiButtonCtrl.SoundType.DEFAULT);
			gameObject.transform.Find("BaseImage/Texture_Movie").GetComponent<PguiRawImageCtrl>().SetRawImage("Texture2D/Memories/" + mstMovieData.thumbnailFilename, true, false, null);
			gameObject.transform.Find("BaseImage/Txt_CharaName").GetComponent<PguiTextCtrl>().text = mstMovieData.movieTitle;
			this.movieList.Add(gameObject.transform);
		}
		this.movieScroll.Resize(this.movieList.Count, 0);
		this.CommunicationCtrl.CharaSelectInitialize(new List<CharaPackData>(this.haveFriendsPackList));
		this.type = SceneStoryView.MODE.NONE;
		this.scenarioCtrl = null;
		this.scenarioQuest = 0;
		this.scenarioName = new List<string>();
		this.movieCtrl.SetActive(false);
		this.movieName = "";
		this.movieLoad = null;
		if (this.mode == SceneStoryView.MODE.PVP)
		{
			this.UpdateChapterList(QuestStaticChapter.Category.PVP);
			int num = 0;
			if (!this.chapterList.TryGetValue(num, out this.storyList))
			{
				this.storyList = new List<int>();
			}
			this.storyScroll.Resize(this.storyList.Count, 0);
			this.type = this.mode;
			this.mode = ((this.storyList.Count > 0) ? SceneStoryView.MODE.STORY : SceneStoryView.MODE.NOTHING);
			this.guiData.story.transform.Find("All/WindowAll/SortFilterBtnsAll/Txt_btn").GetComponent<PguiTextCtrl>().text = SceneStoryView.TITLE_PVPEVENT;
		}
	}

	// Token: 0x06001408 RID: 5128 RVA: 0x000F4804 File Offset: 0x000F2A04
	public override void Update()
	{
		bool flag = true;
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, this.requestNextSceneArgs);
			flag = false;
			this.mode = SceneStoryView.MODE.NONE;
		}
		this.UpdatePanel(new List<SceneStoryView.MODE> { SceneStoryView.MODE.SELECT }, this.guiData.select);
		this.UpdatePanel(new List<SceneStoryView.MODE> { SceneStoryView.MODE.SCENARIO }, this.guiData.scenario);
		this.UpdatePanel(new List<SceneStoryView.MODE>
		{
			SceneStoryView.MODE.CATEGORY,
			SceneStoryView.MODE.MAIN,
			SceneStoryView.MODE.CELLVAL,
			SceneStoryView.MODE.MAIN2,
			SceneStoryView.MODE.MAIN3,
			SceneStoryView.MODE.EVENT,
			SceneStoryView.MODE.ARAI,
			SceneStoryView.MODE.PVP,
			SceneStoryView.MODE.ETC,
			SceneStoryView.MODE.LOGIN_SCENARIO_LIST
		}, this.guiData.chapter);
		this.UpdatePanel(new List<SceneStoryView.MODE> { SceneStoryView.MODE.FRIENDS }, this.guiData.friend);
		this.UpdatePanel(new List<SceneStoryView.MODE> { SceneStoryView.MODE.STORY }, this.guiData.story);
		this.UpdatePanel(new List<SceneStoryView.MODE> { SceneStoryView.MODE.MOVIE }, this.guiData.movie);
		this.UpdatePanel(new List<SceneStoryView.MODE> { SceneStoryView.MODE.COMMUNICATIONCHARA }, this.guiData.communication);
		this.UpdatePanel(new List<SceneStoryView.MODE> { SceneStoryView.MODE.COMMUNICATIONSELECT }, this.guiData.communicationSelect);
		this.UpdatePanel(new List<SceneStoryView.MODE> { SceneStoryView.MODE.NOTHING }, this.guiData.nothing);
		if (this.mode == SceneStoryView.MODE.LOAD)
		{
			if (this.type == SceneStoryView.MODE.MOVIE)
			{
				if (this.movieLoad == null)
				{
					MstMovieData mstMovieData = DataManager.DmServerMst.mstMovieDataList.Find((MstMovieData itm) => itm.movieId == this.movieId);
					if (mstMovieData != null)
					{
						this.movieName = mstMovieData.movieFilename;
						this.movieTime = -1f;
						this.movieLoad = AssetDownloadResolver.ResolveActionMovie(this.movieName, mstMovieData.movieTitle, delegate
						{
							this.movieTime = 1f;
						});
						CanvasManager.HdlCmnMenu.SetActiveMenu(false);
						this.moviePanel.gameObject.SetActive(true);
						this.moviePanel.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END);
					}
					else
					{
						this.mode = SceneStoryView.MODE.MOVIE;
					}
				}
				else if (!this.movieLoad.MoveNext())
				{
					this.movieLoad = null;
					if (this.movieTime > 0f)
					{
						this.mode = SceneStoryView.MODE.CLOSE;
					}
					else
					{
						SoundManager.StopBGM(15);
						this.movieCtrl.SetActive(true);
						MoviePlayer.Play(this.movieCtrl, this.movieName, false);
						this.movieTime = -1f;
						this.seekTime = -1f;
						this.mode = SceneStoryView.MODE.PLAY;
						MstMovieData mstMovieData2 = DataManager.DmServerMst.mstMovieDataList.Find((MstMovieData itm) => itm.movieId == this.movieId);
						DataManager.DmGameStatus.RequestActionShowMovie(mstMovieData2);
					}
				}
			}
			else if (this.scenarioCtrl == null)
			{
				this.ScenarioSetup();
				this.scenarioLoading = TimeManager.SystemNow;
				if (this.scenarioCtrl == null)
				{
					this.mode = SceneStoryView.MODE.STORY;
				}
				else
				{
					CanvasManager.HdlCmnMenu.SetActiveMenu(false);
				}
			}
			else if (this.scenarioCtrl.IsFinishLoad())
			{
				CanvasManager.HdlLoadAndTipsCtrl.SetDispLoading(false);
				this.mode = SceneStoryView.MODE.PLAY;
			}
			else if ((TimeManager.SystemNow - this.scenarioLoading).TotalSeconds > 1.0)
			{
				CanvasManager.HdlLoadAndTipsCtrl.SetDispLoading(true);
			}
		}
		else if (this.mode == SceneStoryView.MODE.PLAY || this.mode == SceneStoryView.MODE.STOP)
		{
			if (this.type == SceneStoryView.MODE.MOVIE)
			{
				MoviePlayer.Pause(this.movieCtrl, this.mode == SceneStoryView.MODE.STOP);
				Screen.sleepTimeout = ((this.mode == SceneStoryView.MODE.STOP) ? (-2) : (-1));
				if (MoviePlayer.Playing(this.movieCtrl))
				{
					if (this.movieData.seek.touch)
					{
						if (this.movieTime > 0f)
						{
							this.seekTime = this.movieData.seek.normalizedValue;
						}
						this.movieTime = SceneStoryView.CTRL_TIME;
					}
					else if (this.seekTime < 0f)
					{
						this.movieData.seek.normalizedValue = MoviePlayer.GetTime(this.movieCtrl);
					}
					else
					{
						this.mode = SceneStoryView.MODE.PLAY;
						MoviePlayer.SetTime(this.movieCtrl, this.seekTime);
						this.seekTime = -1f;
					}
					if (MoviePlayer.Touch(this.movieCtrl))
					{
						this.movieTime = SceneStoryView.CTRL_TIME;
					}
				}
				else
				{
					this.movieId = -1;
					this.SkipMovie();
				}
			}
			else if (this.scenarioCtrl.IsFinishPlay())
			{
				this.mode = SceneStoryView.MODE.CLOSE;
			}
		}
		else if (this.mode == SceneStoryView.MODE.CLOSE)
		{
			if (this.type == SceneStoryView.MODE.MOVIE)
			{
				Screen.sleepTimeout = -2;
				this.movieCtrl.SetActive(false);
				this.moviePanel.gameObject.SetActive(false);
				this.mode = SceneStoryView.MODE.MOVIE;
			}
			else
			{
				Object.Destroy(this.scenarioCtrl.gameObject);
				this.scenarioCtrl = null;
				this.ScenarioSetup();
				this.scenarioLoading = TimeManager.SystemNow;
				this.mode = ((this.scenarioCtrl == null) ? SceneStoryView.MODE.STORY : SceneStoryView.MODE.LOAD);
				CanvasManager.HdlLoadAndTipsCtrl.SetDispLoading(false);
			}
			if (this.mode != SceneStoryView.MODE.LOAD)
			{
				this.movieTime = 0f;
				CanvasManager.HdlCmnMenu.SetActiveMenu(true);
				SoundManager.PlayBGM("prd_bgm0013");
			}
		}
		if ((this.mode == SceneStoryView.MODE.PLAY || this.mode == SceneStoryView.MODE.STOP) && this.movieTime > 0f)
		{
			this.movieTime -= TimeManager.DeltaTime;
			if (!this.moviePanel.ExIsCurrent(SimpleAnimation.ExPguiStatus.START))
			{
				this.moviePanel.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			}
			this.movieData.BtnStop.gameObject.SetActive(this.mode == SceneStoryView.MODE.PLAY);
			this.movieData.BtnPlay.gameObject.SetActive(this.mode == SceneStoryView.MODE.STOP);
		}
		else if (!this.moviePanel.ExIsCurrent(SimpleAnimation.ExPguiStatus.END))
		{
			this.moviePanel.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
		}
		string text = "";
		if (this.mode == SceneStoryView.MODE.ANY2CATEGORY)
		{
			this.mode = SceneStoryView.MODE.CATEGORY;
			this.UpdateChapterList(QuestStaticChapter.Category.TUTORIAL);
			text = SceneStoryView.TITLE_CATEGORY;
		}
		else if (this.mode == SceneStoryView.MODE.ANY2MAIN)
		{
			this.type = SceneStoryView.MODE.ANY2CATEGORY;
			this.mode = SceneStoryView.MODE.MAIN;
			this.UpdateChapterList(QuestStaticChapter.Category.STORY);
			if (this.chapterList.Count <= 0)
			{
				this.mode = SceneStoryView.MODE.NOTHING;
			}
			text = SceneStoryView.TITLE_MAIN;
		}
		else if (this.mode == SceneStoryView.MODE.ANY2CELLVAL)
		{
			this.type = SceneStoryView.MODE.ANY2CATEGORY;
			this.mode = SceneStoryView.MODE.CELLVAL;
			this.UpdateChapterList(QuestStaticChapter.Category.CELLVAL);
			if (this.chapterList.Count <= 0)
			{
				this.mode = SceneStoryView.MODE.NOTHING;
			}
			text = SceneStoryView.TITLE_CELLVAL;
		}
		else if (this.mode == SceneStoryView.MODE.ANY2MAIN2)
		{
			this.type = SceneStoryView.MODE.ANY2CATEGORY;
			this.mode = SceneStoryView.MODE.MAIN2;
			this.UpdateChapterList(QuestStaticChapter.Category.STORY2);
			if (this.chapterList.Count <= 0)
			{
				this.mode = SceneStoryView.MODE.NOTHING;
			}
			text = SceneStoryView.TITLE_MAIN2;
		}
		else if (this.mode == SceneStoryView.MODE.ANY2MAIN3)
		{
			this.type = SceneStoryView.MODE.ANY2CATEGORY;
			this.mode = SceneStoryView.MODE.MAIN3;
			this.UpdateChapterList(QuestStaticChapter.Category.STORY3);
			if (this.chapterList.Count <= 0)
			{
				this.mode = SceneStoryView.MODE.NOTHING;
			}
			text = SceneStoryView.TITLE_MAIN3;
		}
		else if (this.mode == SceneStoryView.MODE.PVP2ETC)
		{
			this.type = SceneStoryView.MODE.ETC2PVP;
			this.mode = SceneStoryView.MODE.ETC;
			this.UpdateChapterList(QuestStaticChapter.Category.ETCETERA);
			if (this.chapterList.Count <= 0)
			{
				this.mode = SceneStoryView.MODE.NOTHING;
			}
			text = QuestUtil.TitleEtcetera;
		}
		else if (this.mode == SceneStoryView.MODE.ETC2PVP)
		{
			this.mode = SceneStoryView.MODE.PVP;
			this.UpdateChapterList(QuestStaticChapter.Category.PVP);
			text = SceneStoryView.TITLE_OTHER;
			this.isLoginScenario = false;
		}
		else if (this.mode == SceneStoryView.MODE.LOGIN_SCENARIO_GROUP)
		{
			this.UpdateChapterList(QuestStaticChapter.Category.PVP);
			this.mode = SceneStoryView.MODE.LOGIN_SCENARIO_LIST;
			this.type = SceneStoryView.MODE.ETC2PVP;
			if (this.chapterList.Count <= 0)
			{
				this.mode = SceneStoryView.MODE.NOTHING;
			}
			text = SceneStoryView.TITLE_LOGIN_STORY;
			this.isLoginScenario = true;
			this.currentCategoryId = -1;
		}
		if (this.mode == SceneStoryView.MODE.COMMUNICATIONCHARA || this.mode == SceneStoryView.MODE.COMMUNICATIONSELECT)
		{
			this.CommunicationCtrl.CommunicationAnimationUpdate();
		}
		if (!string.IsNullOrEmpty(text))
		{
			CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage(text), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", null, null);
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(flag, true);
	}

	// Token: 0x06001409 RID: 5129 RVA: 0x000F5064 File Offset: 0x000F3264
	private void ScenarioSetup()
	{
		if (this.scenarioName.Count > 0)
		{
			this.scenarioCtrl = AssetManager.InstantiateAssetData("SceneScenario/ScenarioPrefab", null).GetComponent<ScenarioScene>();
			this.scenarioCtrl.scenarioName = this.scenarioName[0];
			this.scenarioCtrl.questId = this.scenarioQuest;
			this.scenarioCtrl.storyType = 0;
			this.scenarioName.RemoveAt(0);
		}
	}

	// Token: 0x0600140A RID: 5130 RVA: 0x000F50D8 File Offset: 0x000F32D8
	private void UpdatePanel(List<SceneStoryView.MODE> m, SimpleAnimation a)
	{
		if (m.Contains(this.mode))
		{
			if (a.gameObject.activeSelf && a.ExIsCurrent(SimpleAnimation.ExPguiStatus.START))
			{
				a.ExResumeAnimation(null);
				return;
			}
			if (!this.basePanel.ExIsPlaying())
			{
				bool flag = true;
				foreach (object obj in a.transform.parent)
				{
					Transform transform = (Transform)obj;
					if (transform.gameObject.activeSelf && transform != a.transform)
					{
						flag = false;
					}
				}
				if (flag)
				{
					a.gameObject.SetActive(true);
					a.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
					return;
				}
			}
		}
		else if (a.gameObject.activeSelf)
		{
			if (a.ExIsCurrent(SimpleAnimation.ExPguiStatus.END))
			{
				if (!a.ExIsPlaying())
				{
					a.gameObject.SetActive(false);
					return;
				}
			}
			else
			{
				a.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			}
		}
	}

	// Token: 0x0600140B RID: 5131 RVA: 0x000F51D8 File Offset: 0x000F33D8
	public override void OnDisableScene()
	{
		CanvasManager.HdlCmnMenu.SetActiveMenu(true);
		this.movieCtrl.SetActive(false);
		if (this.scenarioCtrl != null)
		{
			Object.Destroy(this.scenarioCtrl.gameObject);
		}
		this.scenarioCtrl = null;
		this.basePanel.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
	}

	// Token: 0x0600140C RID: 5132 RVA: 0x000F5230 File Offset: 0x000F3430
	public override bool OnDisableSceneWait()
	{
		if (this.basePanel.ExIsPlaying())
		{
			return false;
		}
		this.friendsScroll.Resize(0, 0);
		this.haveFriendsPackList = new List<CharaPackData>();
		this.dispFriendsPackList = new List<CharaPackData>();
		foreach (Transform transform in this.friendsList)
		{
			Object.Destroy(transform.gameObject);
		}
		this.friendsList = new List<Transform>();
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData();
		registerData.register = SortFilterDefine.RegisterType.STORY_VIEW;
		registerData.filterButton = null;
		registerData.sortButton = null;
		registerData.sortUdButton = null;
		registerData.funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget();
		registerData.funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
		{
		};
		SortWindowCtrl.RegisterData registerData2 = registerData;
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData2, false, null);
		this.chapterList = new Dictionary<int, List<int>>();
		this.chapterKey = new List<int>();
		this.chapterScroll.Resize(0, 0);
		this.chapterBtn = new Dictionary<PguiButtonCtrl, int>();
		this.storyList = new List<int>();
		this.storyScroll.Resize(0, 0);
		this.storyBtn = new Dictionary<PguiButtonCtrl, int>();
		foreach (Transform transform2 in this.movieList)
		{
			Object.Destroy(transform2.gameObject);
		}
		this.movieList = new List<Transform>();
		this.movieScroll.Resize(0, 0);
		this.basePanel.gameObject.SetActive(false);
		this.moviePanel.gameObject.SetActive(false);
		return true;
	}

	// Token: 0x0600140D RID: 5133 RVA: 0x000F540C File Offset: 0x000F360C
	public override void OnDestroyScene()
	{
		this.guiData = null;
		this.movieData = null;
		this.friendsScroll = null;
		this.chapterScroll = null;
		this.storyScroll = null;
		this.movieScroll = null;
		this.hidePanel = null;
		Object.Destroy(this.charaPlate.gameObject);
		this.charaPlate = null;
		Object.Destroy(this.moviePlate.gameObject);
		this.moviePlate = null;
		Object.Destroy(this.basePanel.gameObject);
		this.basePanel = null;
		Object.Destroy(this.moviePanel.gameObject);
		this.moviePanel = null;
	}

	// Token: 0x0600140E RID: 5134 RVA: 0x000F54A8 File Offset: 0x000F36A8
	private void OnClickReturnButton()
	{
		SceneStoryView.MODE mode = this.mode;
		if (this.SceneArgs != null && this.SceneArgs.resultNextSceneName != SceneManager.SceneName.None)
		{
			this.requestNextScene = this.SceneArgs.resultNextSceneName;
			this.requestNextSceneArgs = this.SceneArgs.resultNextSceneArgs;
			return;
		}
		if (this.mode == SceneStoryView.MODE.SELECT)
		{
			this.requestNextScene = SceneManager.SceneName.SceneOtherMenuTop;
			return;
		}
		if (this.mode == SceneStoryView.MODE.SCENARIO)
		{
			this.mode = SceneStoryView.MODE.SELECT;
		}
		else if (this.mode == SceneStoryView.MODE.CATEGORY)
		{
			this.mode = SceneStoryView.MODE.SCENARIO;
		}
		else if (this.mode == SceneStoryView.MODE.MAIN || this.mode == SceneStoryView.MODE.ANY2MAIN)
		{
			this.mode = SceneStoryView.MODE.ANY2CATEGORY;
		}
		else if (this.mode == SceneStoryView.MODE.CELLVAL || this.mode == SceneStoryView.MODE.ANY2CELLVAL)
		{
			this.mode = SceneStoryView.MODE.ANY2CATEGORY;
		}
		else if (this.mode == SceneStoryView.MODE.MAIN2 || this.mode == SceneStoryView.MODE.ANY2MAIN2)
		{
			this.mode = SceneStoryView.MODE.ANY2CATEGORY;
		}
		else if (this.mode == SceneStoryView.MODE.MAIN3 || this.mode == SceneStoryView.MODE.ANY2MAIN3)
		{
			this.mode = SceneStoryView.MODE.ANY2CATEGORY;
		}
		else if (this.mode == SceneStoryView.MODE.FRIENDS)
		{
			this.mode = SceneStoryView.MODE.SCENARIO;
		}
		else if (this.mode == SceneStoryView.MODE.EVENT)
		{
			this.mode = SceneStoryView.MODE.SCENARIO;
		}
		else if (this.mode == SceneStoryView.MODE.ARAI)
		{
			this.mode = SceneStoryView.MODE.SCENARIO;
		}
		else if (this.mode == SceneStoryView.MODE.PVP)
		{
			this.mode = SceneStoryView.MODE.SCENARIO;
		}
		else if (this.mode == SceneStoryView.MODE.ETC || this.mode == SceneStoryView.MODE.PVP2ETC)
		{
			this.mode = SceneStoryView.MODE.ETC2PVP;
		}
		else if (this.mode == SceneStoryView.MODE.STORY)
		{
			this.mode = this.type;
		}
		else if (this.mode == SceneStoryView.MODE.MOVIE)
		{
			this.mode = SceneStoryView.MODE.SELECT;
		}
		else if (this.mode == SceneStoryView.MODE.COMMUNICATIONSELECT)
		{
			this.mode = SceneStoryView.MODE.SELECT;
		}
		else if (this.mode == SceneStoryView.MODE.COMMUNICATIONCHARA)
		{
			this.mode = SceneStoryView.MODE.COMMUNICATIONSELECT;
		}
		else if (this.mode == SceneStoryView.MODE.NOTHING)
		{
			this.mode = this.type;
		}
		else if (this.mode == SceneStoryView.MODE.LOGIN_SCENARIO_LIST || this.mode == SceneStoryView.MODE.LOGIN_SCENARIO_GROUP)
		{
			this.mode = SceneStoryView.MODE.ETC2PVP;
		}
		string text = "";
		if (this.mode == SceneStoryView.MODE.SELECT)
		{
			text = SceneStoryView.TITLE_MEMORYS;
		}
		else if (this.mode == SceneStoryView.MODE.SCENARIO)
		{
			text = SceneStoryView.TITLE_SCENARIO;
			this.isLoginScenario = false;
		}
		else if (this.mode == SceneStoryView.MODE.MOVIE)
		{
			text = SceneStoryView.TITLE_MOVIE;
		}
		if (!string.IsNullOrEmpty(text))
		{
			CanvasManager.HdlCmnMenu.SetupMenu(true, text, new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", null, null);
		}
		this.UpdateBookmarkButtonActive();
	}

	// Token: 0x0600140F RID: 5135 RVA: 0x000F5718 File Offset: 0x000F3918
	private void OnClickButton(PguiButtonCtrl button)
	{
		if (button == this.guiData.BtnScenario)
		{
			if (this.mode == SceneStoryView.MODE.SELECT)
			{
				this.mode = SceneStoryView.MODE.SCENARIO;
				CanvasManager.HdlCmnMenu.SetupMenu(true, SceneStoryView.TITLE_SCENARIO, new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", null, null);
				this.isLoginScenario = false;
				return;
			}
		}
		else if (button == this.guiData.BtnMovie)
		{
			if (this.mode == SceneStoryView.MODE.SELECT)
			{
				this.type = this.mode;
				this.mode = ((this.movieList.Count > 0) ? SceneStoryView.MODE.MOVIE : SceneStoryView.MODE.NOTHING);
				CanvasManager.HdlCmnMenu.SetupMenu(true, SceneStoryView.TITLE_MOVIE, new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", null, null);
				return;
			}
		}
		else if (button == this.guiData.BtnCommunication)
		{
			if (this.mode == SceneStoryView.MODE.SELECT)
			{
				this.type = this.mode;
				this.mode = SceneStoryView.MODE.COMMUNICATIONSELECT;
				CanvasManager.HdlCmnMenu.SetupMenu(true, SceneStoryView.TITLE_COMMUNICATION, new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", null, null);
				return;
			}
		}
		else if (button == this.guiData.BtnMain)
		{
			if (this.mode == SceneStoryView.MODE.SCENARIO)
			{
				this.type = this.mode;
				this.mode = SceneStoryView.MODE.ANY2CATEGORY;
				this.currentCategory = this.mode;
				return;
			}
		}
		else if (button == this.guiData.BtnFriends)
		{
			if (this.mode == SceneStoryView.MODE.SCENARIO)
			{
				this.type = this.mode;
				this.mode = SceneStoryView.MODE.FRIENDS;
				this.currentCategory = this.mode;
				this.UpdateChapterList(QuestStaticChapter.Category.CHARA);
				CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage(SceneStoryView.TITLE_FRIENDS), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", null, null);
				return;
			}
		}
		else if (button == this.guiData.BtnEvent)
		{
			if (this.mode == SceneStoryView.MODE.SCENARIO)
			{
				this.type = this.mode;
				this.mode = SceneStoryView.MODE.EVENT;
				this.currentCategory = this.mode;
				this.UpdateChapterList(QuestStaticChapter.Category.EVENT);
				if (this.chapterList.Count <= 0)
				{
					this.mode = SceneStoryView.MODE.NOTHING;
				}
				CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage(SceneStoryView.TITLE_EVENT), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", null, null);
				return;
			}
		}
		else if (button == this.guiData.BtnArai)
		{
			if (this.mode == SceneStoryView.MODE.SCENARIO)
			{
				this.type = this.mode;
				this.mode = SceneStoryView.MODE.ARAI;
				this.currentCategory = this.mode;
				this.UpdateChapterList(QuestStaticChapter.Category.SIDE_STORY);
				if (this.chapterList.Count <= 0)
				{
					this.mode = SceneStoryView.MODE.NOTHING;
				}
				CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage(SceneStoryView.TITLE_ARAI), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", null, null);
				return;
			}
		}
		else if (button == this.guiData.BtnPvp)
		{
			if (this.mode == SceneStoryView.MODE.SCENARIO)
			{
				this.type = this.mode;
				this.mode = SceneStoryView.MODE.PVP;
				this.currentCategory = this.mode;
				this.UpdateChapterList(QuestStaticChapter.Category.PVP);
				if (this.chapterList.Count <= 0)
				{
					this.mode = SceneStoryView.MODE.NOTHING;
				}
				CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage(SceneStoryView.TITLE_OTHER), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", null, null);
				return;
			}
		}
		else if (button == this.movieData.BtnSkip)
		{
			if ((this.mode == SceneStoryView.MODE.PLAY || this.mode == SceneStoryView.MODE.STOP) && this.movieTime > 0f)
			{
				this.mode = SceneStoryView.MODE.CLOSE;
				return;
			}
		}
		else
		{
			if (button == this.movieData.BtnStop)
			{
				if (this.mode == SceneStoryView.MODE.PLAY && this.type == SceneStoryView.MODE.MOVIE && this.movieTime > 0f)
				{
					this.mode = SceneStoryView.MODE.STOP;
				}
				this.movieTime = SceneStoryView.CTRL_TIME;
				return;
			}
			if (button == this.movieData.BtnPlay)
			{
				if (this.mode == SceneStoryView.MODE.STOP && this.type == SceneStoryView.MODE.MOVIE && this.movieTime > 0f)
				{
					this.mode = SceneStoryView.MODE.PLAY;
				}
				this.movieTime = SceneStoryView.CTRL_TIME;
				return;
			}
			if (button == this.bookmarkStoryButton)
			{
				int bookmark = DataManager.DmBookmark.GetCurrentBookmark(this.currentCategory);
				this.currentChapterId = DataManager.DmBookmark.GetCurrentBookmarkChapter(this.currentCategory);
				this.currentCategoryId = DataManager.DmBookmark.GetCurrentBookmarkCategory(this.currentCategory);
				this.UpdateModeByCategory();
				this.UpdateChapterByCategory();
				if (!this.chapterList.TryGetValue(this.currentChapterId, out this.storyList))
				{
					return;
				}
				if (this.currentCategory != SceneStoryView.MODE.FRIENDS)
				{
					string currentChapterName = DataManager.DmBookmark.GetCurrentChapterName(this.currentCategory);
					this.guiData.story.transform.Find("All/WindowAll/SortFilterBtnsAll/Txt_btn").GetComponent<PguiTextCtrl>().text = currentChapterName;
				}
				this.storyScroll.Resize(this.storyList.Count, 0);
				this.type = this.mode;
				this.mode = ((this.storyList.Count > 0) ? SceneStoryView.MODE.STORY : SceneStoryView.MODE.NOTHING);
				int num = this.storyList.FindIndex((int item) => item == bookmark);
				this.storyScroll.ForceFocus(num);
			}
		}
	}

	// Token: 0x06001410 RID: 5136 RVA: 0x000F5C66 File Offset: 0x000F3E66
	private void OnClickPhotoAlbumButton(PguiButtonCtrl button)
	{
		this.requestNextScene = SceneManager.SceneName.ScenePhotoAlbum;
		this.requestNextSceneArgs = new ScenePhotoAlbum.OpenParam
		{
			resultNextSceneName = SceneManager.SceneName.SceneStoryView,
			resultNextSceneArgs = null
		};
	}

	// Token: 0x06001411 RID: 5137 RVA: 0x000F5C8C File Offset: 0x000F3E8C
	private void UpdateChapterByCategory()
	{
		bool flag = this.mode == SceneStoryView.MODE.CATEGORY || this.mode == SceneStoryView.MODE.MAIN || this.mode == SceneStoryView.MODE.CELLVAL || this.mode == SceneStoryView.MODE.MAIN2 || this.mode == SceneStoryView.MODE.MAIN3;
		if (this.mode == SceneStoryView.MODE.PVP)
		{
			if (this.currentCategoryId == -1)
			{
				this.mode = SceneStoryView.MODE.LOGIN_SCENARIO_GROUP;
			}
			this.isLoginScenario = this.currentCategoryId == -1;
			this.UpdateChapterList(QuestStaticChapter.Category.PVP);
			return;
		}
		if (flag && this.currentCategoryId >= 0)
		{
			QuestStaticChapter.Category category = QuestStaticChapter.Category.INVALID;
			switch (this.currentCategoryId)
			{
			case 0:
				category = QuestStaticChapter.Category.TUTORIAL;
				break;
			case 1:
				category = QuestStaticChapter.Category.STORY;
				break;
			case 2:
				category = QuestStaticChapter.Category.CELLVAL;
				break;
			case 3:
				category = QuestStaticChapter.Category.STORY2;
				break;
			case 4:
				category = QuestStaticChapter.Category.STORY3;
				break;
			}
			this.UpdateChapterList(category);
		}
	}

	// Token: 0x06001412 RID: 5138 RVA: 0x000F5D4C File Offset: 0x000F3F4C
	private void UpdateModeByCategory()
	{
		string text = string.Empty;
		SceneStoryView.MODE mode = this.currentCategory;
		if (mode != SceneStoryView.MODE.ANY2CATEGORY)
		{
			switch (mode)
			{
			case SceneStoryView.MODE.EVENT:
				this.mode = SceneStoryView.MODE.EVENT;
				text = SceneStoryView.TITLE_EVENT;
				break;
			case SceneStoryView.MODE.ARAI:
				this.mode = SceneStoryView.MODE.ARAI;
				text = SceneStoryView.TITLE_ARAI;
				break;
			case SceneStoryView.MODE.PVP:
				this.mode = SceneStoryView.MODE.PVP;
				text = SceneStoryView.TITLE_OTHER;
				if (this.currentCategoryId == -1)
				{
					text = SceneStoryView.TITLE_LOGIN_STORY;
				}
				break;
			}
		}
		else
		{
			switch (this.currentCategoryId)
			{
			case 0:
				this.mode = SceneStoryView.MODE.CATEGORY;
				text = SceneStoryView.TITLE_CATEGORY;
				break;
			case 1:
				this.mode = SceneStoryView.MODE.MAIN;
				text = SceneStoryView.TITLE_MAIN;
				break;
			case 2:
				this.mode = SceneStoryView.MODE.CELLVAL;
				text = SceneStoryView.TITLE_CELLVAL;
				break;
			case 3:
				this.mode = SceneStoryView.MODE.MAIN2;
				text = SceneStoryView.TITLE_MAIN2;
				break;
			case 4:
				this.mode = SceneStoryView.MODE.MAIN3;
				text = SceneStoryView.TITLE_MAIN3;
				break;
			}
		}
		CanvasManager.HdlCmnMenu.SetupMenu(true, text, new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", null, null);
	}

	// Token: 0x06001413 RID: 5139 RVA: 0x000F5E54 File Offset: 0x000F4054
	private void UpdateChapterList(QuestStaticChapter.Category cat)
	{
		bool flag = false;
		this.chapterList = new Dictionary<int, List<int>>();
		if (cat == QuestStaticChapter.Category.TUTORIAL)
		{
			this.chapterList.Add(0, new List<int> { 11, 12, 13, 14 });
			this.chapterList.Add(1, new List<int>());
			if (QuestUtil.IsUnLockInformationCellvalQuest(TimeManager.Now))
			{
				this.chapterList.Add(2, new List<int>());
			}
			if (QuestUtil.IsUnLockInformationMainStory2(TimeManager.Now))
			{
				this.chapterList.Add(3, new List<int>());
			}
			if (QuestUtil.IsUnLockInformationMainStory3(TimeManager.Now))
			{
				this.chapterList.Add(4, new List<int>());
			}
		}
		else
		{
			if (this.mode == SceneStoryView.MODE.LOGIN_SCENARIO_GROUP)
			{
				int num = 0;
				using (Dictionary<string, List<DataManagerScenario.LoginScenarioData>>.Enumerator enumerator = DataManager.DmScenario.GetLoginScenarioGroupMap().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, List<DataManagerScenario.LoginScenarioData>> keyValuePair = enumerator.Current;
						List<int> list = new List<int>();
						using (List<DataManagerScenario.LoginScenarioData>.Enumerator enumerator2 = keyValuePair.Value.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								DataManagerScenario.LoginScenarioData scenarioData = enumerator2.Current;
								if (DataManager.DmScenario.GetLoginScenarioMemoryList().Find((DataManagerScenario.LoginScenarioData item) => item.id == scenarioData.id) != null)
								{
									list.Add(scenarioData.id);
								}
							}
						}
						if (list.Count > 0)
						{
							this.chapterList.Add(num, list);
							num++;
						}
					}
					goto IL_0658;
				}
			}
			if (cat == QuestStaticChapter.Category.PVP || cat == QuestStaticChapter.Category.TRAINING || cat == QuestStaticChapter.Category.SCENARIO_SP_PVP)
			{
				int num2 = 21;
				QuestOnePackData questOnePackData = (DataManager.DmGameStatus.MakeUserFlagData().TutorialFinishFlag.PvpFirst ? DataManager.DmQuest.GetQuestOnePackData(num2) : null);
				if (questOnePackData != null)
				{
					this.chapterList.Add(questOnePackData.questChapter.chapterId, new List<int> { num2 });
				}
				List<int> list2 = new List<int>();
				num2 = 10;
				questOnePackData = (DataManager.DmGameStatus.MakeUserFlagData().TutorialFinishFlag.SpecialPvpFirst ? DataManager.DmQuest.GetQuestOnePackData(num2) : null);
				if (questOnePackData != null)
				{
					list2.Add(num2);
				}
				foreach (PvpSpecialEffectData pvpSpecialEffectData in DataManager.DmPvp.GetReleasePvpSpecialEffectList())
				{
					list2.Add(pvpSpecialEffectData.ScenarioQuestOneId);
				}
				if (list2.Count > 0)
				{
					this.chapterList.Add(0, list2);
				}
				num2 = 50100;
				questOnePackData = (DataManager.DmGameStatus.MakeUserFlagData().TutorialFinishFlag.TrainigFirst ? DataManager.DmQuest.GetQuestOnePackData(num2) : null);
				if (questOnePackData != null)
				{
					this.chapterList.Add(questOnePackData.questChapter.chapterId, new List<int> { num2 });
				}
				this.chapterList.Add(1, new List<int>());
				this.chapterList.Add(-1, new List<int>());
			}
			else
			{
				foreach (QuestDynamicQuestOne questDynamicQuestOne in DataManager.DmQuest.QuestDynamicData.oneDataList)
				{
					if (questDynamicQuestOne.status == QuestOneStatus.CLEAR || questDynamicQuestOne.status == QuestOneStatus.COMPLETE)
					{
						QuestOnePackData questOnePackData2 = DataManager.DmQuest.GetQuestOnePackData(questDynamicQuestOne.questOneId);
						if (questOnePackData2 != null && questOnePackData2.questChapter.category == cat && (cat != QuestStaticChapter.Category.STORY || questOnePackData2.questChapter.chapterId <= 2000) && (cat != QuestStaticChapter.Category.STORY2 || questOnePackData2.questChapter.chapterId <= 2000) && (cat != QuestStaticChapter.Category.STORY3 || questOnePackData2.questChapter.chapterId <= 2000) && (!string.IsNullOrEmpty(questOnePackData2.questOne.scenarioBeforeId) || !string.IsNullOrEmpty(questOnePackData2.questOne.scenarioAfterId)))
						{
							int num3 = ((cat == QuestStaticChapter.Category.CHARA) ? questOnePackData2.questMap.questCharaId : questOnePackData2.questChapter.chapterId);
							if (!this.chapterList.ContainsKey(num3))
							{
								this.chapterList.Add(num3, new List<int>());
							}
							this.chapterList[num3].Add(questDynamicQuestOne.questOneId);
						}
					}
				}
				if (cat == QuestStaticChapter.Category.EVENT)
				{
					foreach (QuestStaticChapter questStaticChapter in DataManager.DmQuest.QuestStaticData.chapterDataList.FindAll((QuestStaticChapter itm) => itm.category == QuestStaticChapter.Category.EVENT))
					{
						int id = questStaticChapter.chapterId;
						DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventDataList().Find((DataManagerEvent.EventData itm) => itm.eventChapterId == id);
						if (eventData == null || !(eventData.endDatetime > TimeManager.Now))
						{
							foreach (QuestStaticMap questStaticMap in questStaticChapter.mapDataList)
							{
								foreach (QuestStaticQuestGroup questStaticQuestGroup in questStaticMap.questGroupList)
								{
									if (!(questStaticQuestGroup.endDatetime > TimeManager.Now))
									{
										foreach (QuestStaticQuestOne questStaticQuestOne in questStaticQuestGroup.questOneList)
										{
											if (!string.IsNullOrEmpty(questStaticQuestOne.scenarioBeforeId) || !string.IsNullOrEmpty(questStaticQuestOne.scenarioAfterId))
											{
												if (!this.chapterList.ContainsKey(id))
												{
													this.chapterList.Add(id, new List<int>());
												}
												if (!this.chapterList[id].Contains(questStaticQuestOne.questId))
												{
													this.chapterList[id].Add(questStaticQuestOne.questId);
												}
											}
										}
									}
								}
							}
						}
					}
				}
				flag = true;
			}
		}
		IL_0658:
		this.UpdateBookmarkButtonActive();
		this.chapterKey = new List<int>(this.chapterList.Keys);
		if (flag)
		{
			this.chapterKey.Sort();
		}
		foreach (int num4 in this.chapterKey)
		{
			this.chapterList[num4].Sort(new Comparison<int>(this.QuestSort));
		}
		this.chapterScroll.Resize(this.chapterList.Count, 0);
		if (cat == QuestStaticChapter.Category.CHARA)
		{
			this.friendsScroll.ForceFocus(0);
			this.currentCharaId = PlayerPrefs.GetInt("bookmarkCharaId");
			int num5 = this.dispFriendsPackList.FindIndex((CharaPackData item) => item.staticData.GetId() == this.currentCharaId);
			if (num5 == -1)
			{
				return;
			}
			int num6 = ((num5 + 3 >= this.dispFriendsPackList.Count) ? (num5 + 3) : num5);
			bool flag2 = num6 < 6;
			bool flag3 = num6 > this.dispFriendsPackList.Count - 6;
			bool flag4 = num6 < this.dispFriendsPackList.Count / 2 && !flag2;
			bool flag5 = num6 > this.dispFriendsPackList.Count / 2 && !flag3;
			if (num6 > 3)
			{
				num6 /= 3;
			}
			else
			{
				num6 = 0;
			}
			if (flag2 || flag4)
			{
				num6--;
			}
			else if (flag3 || flag5)
			{
				num6++;
			}
			this.friendsScroll.ForceFocus(num6);
		}
	}

	// Token: 0x06001414 RID: 5140 RVA: 0x000F6710 File Offset: 0x000F4910
	private int QuestSort(int a, int b)
	{
		if (a >= 0 && b >= 0)
		{
			return a - b;
		}
		return a - b;
	}

	// Token: 0x06001415 RID: 5141 RVA: 0x000F6730 File Offset: 0x000F4930
	private void SetupFriends(int index, GameObject go)
	{
	}

	// Token: 0x06001416 RID: 5142 RVA: 0x000F6734 File Offset: 0x000F4934
	private void UpdateFriends(int index, GameObject go)
	{
		List<Transform> list = new List<Transform>();
		foreach (object obj in go.transform)
		{
			Transform transform = (Transform)obj;
			list.Add(transform);
		}
		foreach (Transform transform2 in list)
		{
			transform2.SetParent(this.hidePanel, false);
		}
		int num = index * 3;
		int num2 = num + 3;
		int i = num;
		Predicate<Transform> <>9__0;
		while (i < num2 && i < this.dispFriendsPackList.Count)
		{
			List<Transform> list2 = this.friendsList;
			Predicate<Transform> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = (Transform itm) => itm.name == this.dispFriendsPackList[i].id.ToString());
			}
			Transform transform3 = list2.Find(predicate);
			if (transform3 != null)
			{
				transform3.SetParent(go.transform, false);
				transform3.GetComponent<IconCharaCtrl>().Setup(this.dispFriendsPackList[i], this.sortType, false, null, 0, -1, 0);
				transform3.GetComponent<IconCharaCtrl>().SetupStoryInfo(this.dispFriendsPackList[i], true);
				transform3.GetComponent<IconCharaCtrl>().DispPhotoPocketLevel(true);
			}
			int j = i;
			i = j + 1;
		}
	}

	// Token: 0x06001417 RID: 5143 RVA: 0x000F68CC File Offset: 0x000F4ACC
	private void SetupCommunicationSelect(int index, GameObject go)
	{
		if (this.CommunicationCtrl.BarIconMap == null)
		{
			return;
		}
		this.CommunicationCtrl.BarIconMap.Add(index, new CommunicationCtrl.ScrollBarIconGuiData(go));
		this.CommunicationCtrl.BarIconMap[index].BarObjList = new List<CommunicationCtrl.ScrollBarIconGuiData.IconOne>();
		for (int i = 0; i < 3; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.charaPlate);
			gameObject.transform.SetParent(go.transform, false);
			gameObject.name = i.ToString();
			CommunicationCtrl.ScrollBarIconGuiData.IconOne IconOne = new CommunicationCtrl.ScrollBarIconGuiData.IconOne(gameObject);
			this.CommunicationCtrl.BarIconMap[index].BarObjList.Add(IconOne);
			IconOne.Button.AddOnClickListener(delegate(PguiButtonCtrl x)
			{
				this.OnClickCommunicationFriendsIcon(IconOne.CharaId);
			}, PguiButtonCtrl.SoundType.DEFAULT);
		}
	}

	// Token: 0x06001418 RID: 5144 RVA: 0x000F69B0 File Offset: 0x000F4BB0
	private void UpdateCommunicationSelect(int index, GameObject go)
	{
		int count = this.CommunicationCtrl.BarIconMap.Count;
		CommunicationCtrl.ScrollBarIconGuiData scrollBarIconGuiData = this.CommunicationCtrl.BarIconMap[index % count];
		int num = 0;
		foreach (CommunicationCtrl.ScrollBarIconGuiData.IconOne iconOne in scrollBarIconGuiData.BarObjList)
		{
			int num2 = index * 3 + num;
			if (num2 < this.CommunicationCtrl.DispPackList.Count)
			{
				iconOne.CharaId = this.CommunicationCtrl.DispPackList[num2].id;
				iconOne.Icon.Setup(this.CommunicationCtrl.DispPackList[num2], this.CommunicationCtrl.SortType, false, null, 0, -1, 0);
				iconOne.Icon.DispPhotoPocketLevel(true);
			}
			else
			{
				iconOne.Icon.gameObject.SetActive(false);
			}
			num++;
		}
	}

	// Token: 0x06001419 RID: 5145 RVA: 0x000F6AB0 File Offset: 0x000F4CB0
	private void OnClickCommunicationFriendsIcon(int chId)
	{
		if (this.mode == SceneStoryView.MODE.COMMUNICATIONSELECT)
		{
			this.type = this.mode;
			this.mode = SceneStoryView.MODE.COMMUNICATIONCHARA;
			CanvasManager.HdlCmnMenu.SetupMenu(true, SceneStoryView.TITLE_COMMUNICATION, new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", null, null);
			this.CommunicationCtrl.SelectChara(chId);
		}
	}

	// Token: 0x0600141A RID: 5146 RVA: 0x000F6B0C File Offset: 0x000F4D0C
	private void OnClickFriends(GameObject go)
	{
		int id = ((this.mode == SceneStoryView.MODE.FRIENDS) ? int.Parse(go.name) : 0);
		if (id > 0)
		{
			CharaPackData charaPackData = this.haveFriendsPackList.Find((CharaPackData itm) => itm.id == id);
			this.currentCharaId = id;
			if (charaPackData != null)
			{
				if (!this.chapterList.TryGetValue(id, out this.storyList))
				{
					this.storyList = new List<int>();
				}
				this.storyScroll.Resize(this.storyList.Count, 0);
				this.type = this.mode;
				this.mode = ((this.storyList.Count > 0) ? SceneStoryView.MODE.STORY : SceneStoryView.MODE.NOTHING);
				this.guiData.story.transform.Find("All/WindowAll/SortFilterBtnsAll/Txt_btn").GetComponent<PguiTextCtrl>().text = charaPackData.staticData.GetName();
			}
		}
	}

	// Token: 0x0600141B RID: 5147 RVA: 0x000F6C03 File Offset: 0x000F4E03
	private void SetupChapter(int index, GameObject go)
	{
		this.UpdateChapter(index, go);
		go.GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickChapter), PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x0600141C RID: 5148 RVA: 0x000F6C28 File Offset: 0x000F4E28
	private void UpdateChapter(int index, GameObject go)
	{
		if (index < 0 || index >= this.chapterKey.Count)
		{
			return;
		}
		int num = this.chapterKey[index];
		this.chapterBtn[go.GetComponent<PguiButtonCtrl>()] = num;
		string text = "";
		QuestStaticChapter questStaticChapter = null;
		if (this.mode == SceneStoryView.MODE.CATEGORY || this.mode == SceneStoryView.MODE.MAIN || this.mode == SceneStoryView.MODE.MAIN2 || this.mode == SceneStoryView.MODE.MAIN3 || this.mode == SceneStoryView.MODE.EVENT || this.mode == SceneStoryView.MODE.CELLVAL || this.mode == SceneStoryView.MODE.ARAI)
		{
			if (num == 1)
			{
				text = SceneStoryView.TITLE_MAIN;
			}
			else if (num == 2)
			{
				text = SceneStoryView.TITLE_CELLVAL;
			}
			else if (num == 3)
			{
				text = SceneStoryView.TITLE_MAIN2;
			}
			else if (num == 4)
			{
				text = SceneStoryView.TITLE_MAIN3;
			}
			else if (this.chapterList[num].Count > 0)
			{
				questStaticChapter = DataManager.DmQuest.GetQuestOnePackData(this.chapterList[num][0]).questChapter;
			}
		}
		else if (this.mode == SceneStoryView.MODE.PVP)
		{
			if (num == 0)
			{
				text = SceneStoryView.TITLE_PVPEVENT;
			}
			else if (num == 1)
			{
				text = QuestUtil.TitleEtcetera;
			}
			else if (num == -1)
			{
				text = SceneStoryView.TITLE_LOGIN_STORY;
			}
			else if (!DataManager.DmQuest.QuestStaticData.chapterDataMap.TryGetValue(num, out questStaticChapter))
			{
				questStaticChapter = null;
			}
		}
		else if (this.mode == SceneStoryView.MODE.LOGIN_SCENARIO_GROUP || this.mode == SceneStoryView.MODE.LOGIN_SCENARIO_LIST)
		{
			int num2 = this.chapterList[num][0];
			text = DataManager.DmScenario.GetLoginScenarioData(num2).memoryGroupName;
		}
		if (questStaticChapter != null)
		{
			text = questStaticChapter.chapterName;
			if (string.IsNullOrEmpty(text) && (questStaticChapter.category == QuestStaticChapter.Category.STORY || questStaticChapter.category == QuestStaticChapter.Category.STORY2 || questStaticChapter.category == QuestStaticChapter.Category.STORY3 || questStaticChapter.category == QuestStaticChapter.Category.SIDE_STORY))
			{
				text = "第" + questStaticChapter.chapterNumber.ToString() + "章 ";
			}
			text = text + "\u3000" + questStaticChapter.chapterTitle;
		}
		go.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>().text = text;
	}

	// Token: 0x0600141D RID: 5149 RVA: 0x000F6E34 File Offset: 0x000F5034
	private void OnClickChapter(PguiButtonCtrl go)
	{
		if (this.mode != SceneStoryView.MODE.CATEGORY && this.mode != SceneStoryView.MODE.MAIN && this.mode != SceneStoryView.MODE.CELLVAL && this.mode != SceneStoryView.MODE.MAIN2 && this.mode != SceneStoryView.MODE.MAIN3 && this.mode != SceneStoryView.MODE.EVENT && this.mode != SceneStoryView.MODE.ARAI && this.mode != SceneStoryView.MODE.PVP && this.mode != SceneStoryView.MODE.ETC && this.mode != SceneStoryView.MODE.LOGIN_SCENARIO_LIST)
		{
			return;
		}
		int num = 0;
		if (!this.chapterBtn.TryGetValue(go, out num))
		{
			return;
		}
		if (this.mode == SceneStoryView.MODE.PVP || this.mode == SceneStoryView.MODE.CATEGORY)
		{
			this.currentCategoryId = num;
		}
		this.currentChapterId = num;
		if (this.mode == SceneStoryView.MODE.CATEGORY)
		{
			if (num == 1)
			{
				this.type = this.mode;
				this.mode = SceneStoryView.MODE.ANY2MAIN;
				return;
			}
			if (num == 2)
			{
				this.type = this.mode;
				this.mode = SceneStoryView.MODE.ANY2CELLVAL;
				return;
			}
			if (num == 3)
			{
				this.type = this.mode;
				this.mode = SceneStoryView.MODE.ANY2MAIN2;
				return;
			}
			if (num == 4)
			{
				this.type = this.mode;
				this.mode = SceneStoryView.MODE.ANY2MAIN3;
				return;
			}
		}
		else if (this.mode == SceneStoryView.MODE.PVP)
		{
			this.isLoginScenario = false;
			if (num == 1)
			{
				this.type = this.mode;
				this.mode = SceneStoryView.MODE.PVP2ETC;
				return;
			}
			if (num == -1)
			{
				this.type = this.mode;
				this.mode = SceneStoryView.MODE.LOGIN_SCENARIO_GROUP;
				this.isLoginScenario = true;
				return;
			}
		}
		if (!this.chapterList.TryGetValue(num, out this.storyList))
		{
			return;
		}
		this.storyScroll.Resize(this.storyList.Count, 0);
		this.type = this.mode;
		this.mode = ((this.storyList.Count > 0) ? SceneStoryView.MODE.STORY : SceneStoryView.MODE.NOTHING);
		this.guiData.story.transform.Find("All/WindowAll/SortFilterBtnsAll/Txt_btn").GetComponent<PguiTextCtrl>().text = go.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>().text;
	}

	// Token: 0x0600141E RID: 5150 RVA: 0x000F701C File Offset: 0x000F521C
	private void SetupStory(int index, GameObject go)
	{
		this.UpdateStory(index, go);
		go.GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickStory), PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x0600141F RID: 5151 RVA: 0x000F7040 File Offset: 0x000F5240
	private void UpdateStory(int index, GameObject go)
	{
		if (index < 0 || index >= this.storyList.Count)
		{
			return;
		}
		this.storyBtn[go.GetComponent<PguiButtonCtrl>()] = this.storyList[index];
		this.UpdateBookmarkScenario(go, this.storyList[index]);
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(this.storyList[index]);
		DataManagerScenario.LoginScenarioData loginScenarioData = DataManager.DmScenario.GetLoginScenarioData(this.storyList[index]);
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		string text5 = "";
		string text6 = "X";
		if (questOnePackData != null && !this.isLoginScenario)
		{
			text = string.Concat(new string[]
			{
				questOnePackData.questGroup.titleName,
				" ",
				questOnePackData.questGroup.storyName,
				" ",
				questOnePackData.questOne.questName
			});
			if (questOnePackData.questOne.memoryCharaId01 > 0)
			{
				text2 = "Texture2D/Icon_Chara/Chara/icon_chara_" + questOnePackData.questOne.memoryCharaId01.ToString("0000");
			}
			if (questOnePackData.questOne.memoryCharaId02 > 0)
			{
				text3 = "Texture2D/Icon_Chara/Chara/icon_chara_" + questOnePackData.questOne.memoryCharaId02.ToString("0000");
			}
			text4 = questOnePackData.questOne.memoryText01;
			text5 = questOnePackData.questOne.memoryText02;
			if (questOnePackData.questOne.memoryCharaId01 > 0)
			{
				if (questOnePackData.questOne.memoryCharaId02 > 0)
				{
					text6 = "A";
				}
				else if (string.IsNullOrEmpty(text5))
				{
					text6 = "B";
				}
				else
				{
					text6 = "D";
					text4 = text4 + "\n" + text5;
				}
			}
			else if (questOnePackData.questOne.memoryCharaId02 > 0)
			{
				if (string.IsNullOrEmpty(text4))
				{
					text6 = "C";
				}
				else
				{
					text6 = "E";
					text5 = text4 + "\n" + text5;
				}
			}
		}
		else if (loginScenarioData != null && loginScenarioData != null)
		{
			text = loginScenarioData.memoryTitleName;
			if (loginScenarioData.memoryCharaId01 > 0)
			{
				text2 = "Texture2D/Icon_Chara/Chara/icon_chara_" + loginScenarioData.memoryCharaId01.ToString("0000");
			}
			if (loginScenarioData.memoryCharaId02 > 0)
			{
				text3 = "Texture2D/Icon_Chara/Chara/icon_chara_" + loginScenarioData.memoryCharaId02.ToString("0000");
			}
			text4 = loginScenarioData.memoryText01;
			text5 = loginScenarioData.memoryText02;
			if (loginScenarioData.memoryCharaId01 > 0)
			{
				if (loginScenarioData.memoryCharaId02 > 0)
				{
					text6 = "A";
				}
				else if (string.IsNullOrEmpty(text5))
				{
					text6 = "B";
				}
				else
				{
					text6 = "D";
					text4 = text4 + "\n" + text5;
				}
			}
			else if (loginScenarioData.memoryCharaId02 > 0)
			{
				if (string.IsNullOrEmpty(text4))
				{
					text6 = "C";
				}
				else
				{
					text6 = "E";
					text5 = text4 + "\n" + text5;
				}
			}
		}
		go.transform.Find("BaseImage/Txt_FriendName").GetComponent<PguiTextCtrl>().text = text;
		Transform transform = go.transform.Find("BaseImage/Mask01");
		if (string.IsNullOrEmpty(text2))
		{
			transform.gameObject.SetActive(false);
		}
		else
		{
			transform.gameObject.SetActive(true);
			transform.Find("Texture_Chara").GetComponent<PguiRawImageCtrl>().SetRawImage(text2, true, false, null);
		}
		transform = go.transform.Find("BaseImage/Mask02");
		if (string.IsNullOrEmpty(text3))
		{
			transform.gameObject.SetActive(false);
		}
		else
		{
			transform.gameObject.SetActive(true);
			transform.Find("Texture_Chara").GetComponent<PguiRawImageCtrl>().SetRawImage(text3, true, false, null);
		}
		go.transform.Find("BaseImage/Chara_Txt_A/Chara01_Txt/Txt").GetComponent<PguiTextCtrl>().text = text4;
		go.transform.Find("BaseImage/Chara_Txt_A/Chara02_Txt/Txt").GetComponent<PguiTextCtrl>().text = text5;
		go.transform.Find("BaseImage/Chara_Txt_B/Chara01_Txt/Txt").GetComponent<PguiTextCtrl>().text = text4;
		go.transform.Find("BaseImage/Chara_Txt_C/Chara02_Txt/Txt").GetComponent<PguiTextCtrl>().text = text5;
		go.transform.Find("BaseImage/Chara_Txt_D/Chara01_Txt/Txt").GetComponent<PguiTextCtrl>().text = text4;
		go.transform.Find("BaseImage/Chara_Txt_E/Chara02_Txt/Txt").GetComponent<PguiTextCtrl>().text = text5;
		transform = go.transform.Find("BaseImage");
		foreach (object obj in transform)
		{
			Transform transform2 = (Transform)obj;
			if (transform2.name.StartsWith("Chara_Txt_"))
			{
				transform2.gameObject.SetActive(transform2.name.EndsWith(text6));
			}
		}
	}

	// Token: 0x06001420 RID: 5152 RVA: 0x000F7528 File Offset: 0x000F5728
	private void OnClickStory(PguiButtonCtrl go)
	{
		if (this.mode != SceneStoryView.MODE.STORY)
		{
			return;
		}
		int num = 0;
		if (!this.storyBtn.TryGetValue(go, out num))
		{
			return;
		}
		this.scenarioName = new List<string>();
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(num);
		int currentBookmarkCharaid = DataManager.DmBookmark.GetCurrentBookmarkCharaid();
		string text = this.guiData.story.transform.Find("All/WindowAll/SortFilterBtnsAll/Txt_btn").GetComponent<PguiTextCtrl>().text;
		DataManager.DmBookmark.UpdateBookmark(num, this.currentCharaId, this.currentChapterId, this.currentCategoryId, this.currentCategory, text);
		if (this.currentCategory == SceneStoryView.MODE.FRIENDS)
		{
			this.UpdateBookmarkChara(currentBookmarkCharaid);
		}
		else
		{
			this.UpdateBookmarkScenarioAllCheck();
		}
		if (questOnePackData != null && !this.isLoginScenario)
		{
			this.scenarioQuest = num;
			if (!string.IsNullOrEmpty(questOnePackData.questOne.scenarioBeforeId))
			{
				this.scenarioName.Add(questOnePackData.questOne.scenarioBeforeId);
			}
			if (!string.IsNullOrEmpty(questOnePackData.questOne.scenarioAfterId))
			{
				this.scenarioName.Add(questOnePackData.questOne.scenarioAfterId);
			}
		}
		else
		{
			this.scenarioName.Add(DataManager.DmScenario.GetLoginScenarioData(num).scenarioFileName);
		}
		if (this.scenarioName.Count > 0)
		{
			this.mode = SceneStoryView.MODE.LOAD;
		}
	}

	// Token: 0x06001421 RID: 5153 RVA: 0x000F766C File Offset: 0x000F586C
	private void UpdateBookmarkChara(int now)
	{
		Transform transform = this.friendsList.Find((Transform item) => item.gameObject.name == now.ToString());
		if (transform != null)
		{
			transform.GetComponent<IconCharaCtrl>().DispBookmark(false);
		}
		Transform transform2 = this.friendsList.Find((Transform item) => item.gameObject.name == this.currentCharaId.ToString());
		if (transform2 != null)
		{
			transform2.GetComponent<IconCharaCtrl>().DispBookmark(true);
		}
		this.UpdateBookmarkScenarioAllCheck();
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x000F76F0 File Offset: 0x000F58F0
	private void UpdateBookmarkScenario(GameObject go, int target)
	{
		int currentBookmark = DataManager.DmBookmark.GetCurrentBookmark(this.currentCategory);
		go.transform.Find("BaseImage/Bookmark").gameObject.SetActive(target == currentBookmark);
	}

	// Token: 0x06001423 RID: 5155 RVA: 0x000F772C File Offset: 0x000F592C
	private void UpdateBookmarkScenarioAllCheck()
	{
		int currentBookmark = DataManager.DmBookmark.GetCurrentBookmark(this.currentCategory);
		using (List<int>.Enumerator enumerator = this.storyList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int id = enumerator.Current;
				KeyValuePair<PguiButtonCtrl, int> keyValuePair = this.storyBtn.FirstOrDefault<KeyValuePair<PguiButtonCtrl, int>>((KeyValuePair<PguiButtonCtrl, int> item) => item.Value == id);
				if (!(keyValuePair.Key == null))
				{
					keyValuePair.Key.transform.Find("BaseImage/Bookmark").gameObject.SetActive(id == currentBookmark);
				}
			}
		}
	}

	// Token: 0x06001424 RID: 5156 RVA: 0x000F77E4 File Offset: 0x000F59E4
	private void UpdateBookmarkButtonActive()
	{
		int currentBookmark = DataManager.DmBookmark.GetCurrentBookmark(this.currentCategory);
		this.bookmarkStoryButton.SetActEnable(currentBookmark != 0, false, false);
	}

	// Token: 0x06001425 RID: 5157 RVA: 0x000F7814 File Offset: 0x000F5A14
	private void SetupMovie(int index, GameObject go)
	{
		List<Transform> list = new List<Transform>();
		foreach (object obj in go.transform)
		{
			Transform transform = (Transform)obj;
			list.Add(transform);
		}
		foreach (Transform transform2 in list)
		{
			transform2.SetParent(this.hidePanel, false);
		}
		int num = index * 3;
		int num2 = num + 3;
		int num3 = num;
		while (num3 < num2 && num3 < this.movieList.Count)
		{
			this.movieList[num3].SetParent(go.transform, false);
			num3++;
		}
	}

	// Token: 0x06001426 RID: 5158 RVA: 0x000F78F8 File Offset: 0x000F5AF8
	private void OnClickMovie(PguiButtonCtrl go)
	{
		if (this.mode != SceneStoryView.MODE.MOVIE)
		{
			return;
		}
		this.movieId = int.Parse(go.name);
		this.type = this.mode;
		this.mode = SceneStoryView.MODE.LOAD;
	}

	// Token: 0x06001427 RID: 5159 RVA: 0x000F792C File Offset: 0x000F5B2C
	private void SkipMovie()
	{
		MstMovieData mstMovieData = DataManager.DmServerMst.mstMovieDataList.Find((MstMovieData itm) => itm.movieId == this.movieId);
		if (mstMovieData == null)
		{
			this.mode = SceneStoryView.MODE.CLOSE;
			return;
		}
		int num = DataManager.DmServerMst.mstMovieDataList.IndexOf(mstMovieData);
		if (++num >= DataManager.DmServerMst.mstMovieDataList.Count)
		{
			this.mode = SceneStoryView.MODE.CLOSE;
			return;
		}
		this.movieId = DataManager.DmServerMst.mstMovieDataList[num].movieId;
		this.mode = SceneStoryView.MODE.LOAD;
	}

	// Token: 0x04001078 RID: 4216
	private SceneStoryView.Args SceneArgs;

	// Token: 0x04001079 RID: 4217
	private SimpleAnimation basePanel;

	// Token: 0x0400107A RID: 4218
	private SimpleAnimation moviePanel;

	// Token: 0x0400107B RID: 4219
	private SceneManager.SceneName requestNextScene;

	// Token: 0x0400107C RID: 4220
	private object requestNextSceneArgs;

	// Token: 0x0400107D RID: 4221
	private Transform hidePanel;

	// Token: 0x0400107E RID: 4222
	private GameObject charaPlate;

	// Token: 0x0400107F RID: 4223
	private GameObject moviePlate;

	// Token: 0x04001080 RID: 4224
	private SceneStoryView.GUI guiData;

	// Token: 0x04001081 RID: 4225
	private SceneStoryView.MOVIE movieData;

	// Token: 0x04001082 RID: 4226
	private SceneStoryView.MODE mode;

	// Token: 0x04001083 RID: 4227
	private SceneStoryView.MODE type;

	// Token: 0x04001084 RID: 4228
	private SceneStoryView.MODE currentCategory;

	// Token: 0x04001085 RID: 4229
	private List<CharaPackData> haveFriendsPackList;

	// Token: 0x04001086 RID: 4230
	private List<CharaPackData> dispFriendsPackList;

	// Token: 0x04001087 RID: 4231
	private SortFilterDefine.SortType sortType;

	// Token: 0x04001088 RID: 4232
	private ReuseScroll friendsScroll;

	// Token: 0x04001089 RID: 4233
	private List<Transform> friendsList;

	// Token: 0x0400108A RID: 4234
	private Dictionary<int, List<int>> chapterList;

	// Token: 0x0400108B RID: 4235
	private List<int> chapterKey;

	// Token: 0x0400108C RID: 4236
	private ReuseScroll chapterScroll;

	// Token: 0x0400108D RID: 4237
	private Dictionary<PguiButtonCtrl, int> chapterBtn;

	// Token: 0x0400108E RID: 4238
	private PguiButtonCtrl bookmarkStoryButton;

	// Token: 0x0400108F RID: 4239
	private List<int> storyList;

	// Token: 0x04001090 RID: 4240
	private ReuseScroll storyScroll;

	// Token: 0x04001091 RID: 4241
	private Dictionary<PguiButtonCtrl, int> storyBtn;

	// Token: 0x04001092 RID: 4242
	private List<Transform> movieList;

	// Token: 0x04001093 RID: 4243
	private ReuseScroll movieScroll;

	// Token: 0x04001094 RID: 4244
	private CommunicationCtrl CommunicationCtrl;

	// Token: 0x04001095 RID: 4245
	private ScenarioScene scenarioCtrl;

	// Token: 0x04001096 RID: 4246
	private DateTime scenarioLoading;

	// Token: 0x04001097 RID: 4247
	private int scenarioQuest;

	// Token: 0x04001098 RID: 4248
	private List<string> scenarioName;

	// Token: 0x04001099 RID: 4249
	private GameObject movieCtrl;

	// Token: 0x0400109A RID: 4250
	private float movieTime;

	// Token: 0x0400109B RID: 4251
	private float seekTime;

	// Token: 0x0400109C RID: 4252
	private int movieId;

	// Token: 0x0400109D RID: 4253
	private string movieName;

	// Token: 0x0400109E RID: 4254
	private IEnumerator movieLoad;

	// Token: 0x0400109F RID: 4255
	private int currentCharaId;

	// Token: 0x040010A0 RID: 4256
	private int currentChapterId;

	// Token: 0x040010A1 RID: 4257
	private int currentCategoryId;

	// Token: 0x040010A2 RID: 4258
	private bool isLoginScenario;

	// Token: 0x040010A3 RID: 4259
	private static readonly float CTRL_TIME = 1.5f;

	// Token: 0x040010A4 RID: 4260
	private static readonly string TITLE_MEMORYS = "思い出";

	// Token: 0x040010A5 RID: 4261
	private static readonly string TITLE_SCENARIO = "ストーリー回想";

	// Token: 0x040010A6 RID: 4262
	private static readonly string TITLE_MOVIE = "ムービー回想";

	// Token: 0x040010A7 RID: 4263
	private static readonly string TITLE_CATEGORY = "メインストーリー";

	// Token: 0x040010A8 RID: 4264
	private static readonly string TITLE_MAIN = "メインストーリー";

	// Token: 0x040010A9 RID: 4265
	private static readonly string TITLE_CELLVAL = "セーバルぶらり旅";

	// Token: 0x040010AA RID: 4266
	private static readonly string TITLE_MAIN2 = "メインストーリー\u3000シーズン２";

	// Token: 0x040010AB RID: 4267
	private static readonly string TITLE_MAIN3 = "メインストーリー\u3000シーズン３";

	// Token: 0x040010AC RID: 4268
	private static readonly string TITLE_FRIENDS = "フレンズストーリー";

	// Token: 0x040010AD RID: 4269
	private static readonly string TITLE_EVENT = "イベントストーリー";

	// Token: 0x040010AE RID: 4270
	private static readonly string TITLE_ARAI = "アライさん隊長日誌";

	// Token: 0x040010AF RID: 4271
	private static readonly string TITLE_OTHER = "その他";

	// Token: 0x040010B0 RID: 4272
	private static readonly string TITLE_PVPEVENT = "とくべつくんれんシナリオ";

	// Token: 0x040010B1 RID: 4273
	private static readonly string TITLE_COMMUNICATION = "交流";

	// Token: 0x040010B2 RID: 4274
	private static readonly string TITLE_LOGIN_STORY = "ログインストーリー";

	// Token: 0x02000B5B RID: 2907
	public class Args
	{
		// Token: 0x0400471C RID: 18204
		public SceneStoryView.Args.VIEWTYPE viewType;

		// Token: 0x0400471D RID: 18205
		public SceneManager.SceneName resultNextSceneName;

		// Token: 0x0400471E RID: 18206
		public object resultNextSceneArgs;

		// Token: 0x02001190 RID: 4496
		public enum VIEWTYPE
		{
			// Token: 0x04006035 RID: 24629
			NONE,
			// Token: 0x04006036 RID: 24630
			MOVIE,
			// Token: 0x04006037 RID: 24631
			PVPEVENT
		}
	}

	// Token: 0x02000B5C RID: 2908
	public class GUI
	{
		// Token: 0x0600429E RID: 17054 RVA: 0x00200B5C File Offset: 0x001FED5C
		public GUI(Transform baseTr)
		{
			this.select = baseTr.Find("Memories_Select").GetComponent<SimpleAnimation>();
			this.scenario = baseTr.Find("Scenario_Select").GetComponent<SimpleAnimation>();
			this.chapter = baseTr.Find("Scenario_Chapter").GetComponent<SimpleAnimation>();
			this.friend = baseTr.Find("Scenario_Chapter_Friend").GetComponent<SimpleAnimation>();
			this.story = baseTr.Find("Scenario_ScenarioList").GetComponent<SimpleAnimation>();
			this.nothing = baseTr.Find("Scenario_ScenarioNothing").GetComponent<SimpleAnimation>();
			this.movie = baseTr.Find("Scenario_Chapter_Movie").GetComponent<SimpleAnimation>();
			this.communication = baseTr.Find("Communication").GetComponent<SimpleAnimation>();
			this.communicationSelect = Object.Instantiate<GameObject>(baseTr.Find("Scenario_Chapter_Friend").gameObject, baseTr).GetComponent<SimpleAnimation>();
			this.communicationSelect.name = "CommunicationSelect";
			this.communicationSelect.gameObject.SetActive(false);
			this.BtnScenario = this.select.transform.Find("All/Memories_BtnScenario").GetComponent<PguiButtonCtrl>();
			this.BtnMovie = this.select.transform.Find("All/Memories_BtnMovie").GetComponent<PguiButtonCtrl>();
			this.BtnPhotoAlbum = this.select.transform.Find("All/Memories_BtnPhotoAlbum").GetComponent<PguiButtonCtrl>();
			this.BtnCommunication = this.select.transform.Find("All/Memories_BtnCommu").GetComponent<PguiButtonCtrl>();
			this.BtnMain = this.scenario.transform.Find("All/Scenario_Btn_MainStory").GetComponent<PguiButtonCtrl>();
			this.BtnFriends = this.scenario.transform.Find("All/Scenario_Btn_FriendsStory").GetComponent<PguiButtonCtrl>();
			this.BtnEvent = this.scenario.transform.Find("All/Scenario_Btn_EventStory").GetComponent<PguiButtonCtrl>();
			this.BtnArai = this.scenario.transform.Find("All/Scenario_Btn_AraiDiary").GetComponent<PguiButtonCtrl>();
			this.BtnPvp = this.scenario.transform.Find("All/Scenario_Btn_PVP").GetComponent<PguiButtonCtrl>();
		}

		// Token: 0x0400471F RID: 18207
		public SimpleAnimation select;

		// Token: 0x04004720 RID: 18208
		public SimpleAnimation scenario;

		// Token: 0x04004721 RID: 18209
		public SimpleAnimation chapter;

		// Token: 0x04004722 RID: 18210
		public SimpleAnimation friend;

		// Token: 0x04004723 RID: 18211
		public SimpleAnimation story;

		// Token: 0x04004724 RID: 18212
		public SimpleAnimation nothing;

		// Token: 0x04004725 RID: 18213
		public SimpleAnimation movie;

		// Token: 0x04004726 RID: 18214
		public SimpleAnimation communication;

		// Token: 0x04004727 RID: 18215
		public SimpleAnimation communicationSelect;

		// Token: 0x04004728 RID: 18216
		public PguiButtonCtrl BtnScenario;

		// Token: 0x04004729 RID: 18217
		public PguiButtonCtrl BtnMovie;

		// Token: 0x0400472A RID: 18218
		public PguiButtonCtrl BtnPhotoAlbum;

		// Token: 0x0400472B RID: 18219
		public PguiButtonCtrl BtnCommunication;

		// Token: 0x0400472C RID: 18220
		public PguiButtonCtrl BtnMain;

		// Token: 0x0400472D RID: 18221
		public PguiButtonCtrl BtnFriends;

		// Token: 0x0400472E RID: 18222
		public PguiButtonCtrl BtnEvent;

		// Token: 0x0400472F RID: 18223
		public PguiButtonCtrl BtnArai;

		// Token: 0x04004730 RID: 18224
		public PguiButtonCtrl BtnPvp;
	}

	// Token: 0x02000B5D RID: 2909
	public class MOVIE
	{
		// Token: 0x0600429F RID: 17055 RVA: 0x00200D84 File Offset: 0x001FEF84
		public MOVIE(Transform baseTr)
		{
			this.BtnSkip = baseTr.Find("All/Btn_Skip").GetComponent<PguiButtonCtrl>();
			this.BtnPlay = baseTr.Find("All/Btn_Play").GetComponent<PguiButtonCtrl>();
			this.BtnStop = baseTr.Find("All/Btn_Stop").GetComponent<PguiButtonCtrl>();
			this.seek = baseTr.Find("All/SeekBar").GetComponent<PguiSlider>();
		}

		// Token: 0x04004731 RID: 18225
		public PguiButtonCtrl BtnSkip;

		// Token: 0x04004732 RID: 18226
		public PguiButtonCtrl BtnPlay;

		// Token: 0x04004733 RID: 18227
		public PguiButtonCtrl BtnStop;

		// Token: 0x04004734 RID: 18228
		public PguiSlider seek;
	}

	// Token: 0x02000B5E RID: 2910
	public enum MODE
	{
		// Token: 0x04004736 RID: 18230
		NONE,
		// Token: 0x04004737 RID: 18231
		SELECT,
		// Token: 0x04004738 RID: 18232
		SCENARIO,
		// Token: 0x04004739 RID: 18233
		ANY2CATEGORY,
		// Token: 0x0400473A RID: 18234
		CATEGORY,
		// Token: 0x0400473B RID: 18235
		ANY2MAIN,
		// Token: 0x0400473C RID: 18236
		MAIN,
		// Token: 0x0400473D RID: 18237
		ANY2CELLVAL,
		// Token: 0x0400473E RID: 18238
		CELLVAL,
		// Token: 0x0400473F RID: 18239
		ANY2MAIN2,
		// Token: 0x04004740 RID: 18240
		MAIN2,
		// Token: 0x04004741 RID: 18241
		FRIENDS,
		// Token: 0x04004742 RID: 18242
		EVENT,
		// Token: 0x04004743 RID: 18243
		ARAI,
		// Token: 0x04004744 RID: 18244
		PVP,
		// Token: 0x04004745 RID: 18245
		PVP2ETC,
		// Token: 0x04004746 RID: 18246
		ETC,
		// Token: 0x04004747 RID: 18247
		ETC2PVP,
		// Token: 0x04004748 RID: 18248
		STORY,
		// Token: 0x04004749 RID: 18249
		MOVIE,
		// Token: 0x0400474A RID: 18250
		COMMUNICATIONCHARA,
		// Token: 0x0400474B RID: 18251
		COMMUNICATIONSELECT,
		// Token: 0x0400474C RID: 18252
		NOTHING,
		// Token: 0x0400474D RID: 18253
		LOAD,
		// Token: 0x0400474E RID: 18254
		PLAY,
		// Token: 0x0400474F RID: 18255
		STOP,
		// Token: 0x04004750 RID: 18256
		CLOSE,
		// Token: 0x04004751 RID: 18257
		ANY2MAIN3,
		// Token: 0x04004752 RID: 18258
		MAIN3,
		// Token: 0x04004753 RID: 18259
		LOGIN_SCENARIO_GROUP,
		// Token: 0x04004754 RID: 18260
		LOGIN_SCENARIO_LIST
	}
}
