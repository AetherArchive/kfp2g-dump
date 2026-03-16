using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Common;
using SGNFW.Mst;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

public class SceneStoryView : BaseScene
{
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

	private void OnClickPhotoAlbumButton(PguiButtonCtrl button)
	{
		this.requestNextScene = SceneManager.SceneName.ScenePhotoAlbum;
		this.requestNextSceneArgs = new ScenePhotoAlbum.OpenParam
		{
			resultNextSceneName = SceneManager.SceneName.SceneStoryView,
			resultNextSceneArgs = null
		};
	}

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

	private int QuestSort(int a, int b)
	{
		if (a >= 0 && b >= 0)
		{
			return a - b;
		}
		return a - b;
	}

	private void SetupFriends(int index, GameObject go)
	{
	}

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

	private void SetupChapter(int index, GameObject go)
	{
		this.UpdateChapter(index, go);
		go.GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickChapter), PguiButtonCtrl.SoundType.DEFAULT);
	}

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

	private void SetupStory(int index, GameObject go)
	{
		this.UpdateStory(index, go);
		go.GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickStory), PguiButtonCtrl.SoundType.DEFAULT);
	}

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

	private void UpdateBookmarkScenario(GameObject go, int target)
	{
		int currentBookmark = DataManager.DmBookmark.GetCurrentBookmark(this.currentCategory);
		go.transform.Find("BaseImage/Bookmark").gameObject.SetActive(target == currentBookmark);
	}

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

	private void UpdateBookmarkButtonActive()
	{
		int currentBookmark = DataManager.DmBookmark.GetCurrentBookmark(this.currentCategory);
		this.bookmarkStoryButton.SetActEnable(currentBookmark != 0, false, false);
	}

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

	private SceneStoryView.Args SceneArgs;

	private SimpleAnimation basePanel;

	private SimpleAnimation moviePanel;

	private SceneManager.SceneName requestNextScene;

	private object requestNextSceneArgs;

	private Transform hidePanel;

	private GameObject charaPlate;

	private GameObject moviePlate;

	private SceneStoryView.GUI guiData;

	private SceneStoryView.MOVIE movieData;

	private SceneStoryView.MODE mode;

	private SceneStoryView.MODE type;

	private SceneStoryView.MODE currentCategory;

	private List<CharaPackData> haveFriendsPackList;

	private List<CharaPackData> dispFriendsPackList;

	private SortFilterDefine.SortType sortType;

	private ReuseScroll friendsScroll;

	private List<Transform> friendsList;

	private Dictionary<int, List<int>> chapterList;

	private List<int> chapterKey;

	private ReuseScroll chapterScroll;

	private Dictionary<PguiButtonCtrl, int> chapterBtn;

	private PguiButtonCtrl bookmarkStoryButton;

	private List<int> storyList;

	private ReuseScroll storyScroll;

	private Dictionary<PguiButtonCtrl, int> storyBtn;

	private List<Transform> movieList;

	private ReuseScroll movieScroll;

	private CommunicationCtrl CommunicationCtrl;

	private ScenarioScene scenarioCtrl;

	private DateTime scenarioLoading;

	private int scenarioQuest;

	private List<string> scenarioName;

	private GameObject movieCtrl;

	private float movieTime;

	private float seekTime;

	private int movieId;

	private string movieName;

	private IEnumerator movieLoad;

	private int currentCharaId;

	private int currentChapterId;

	private int currentCategoryId;

	private bool isLoginScenario;

	private static readonly float CTRL_TIME = 1.5f;

	private static readonly string TITLE_MEMORYS = "思い出";

	private static readonly string TITLE_SCENARIO = "ストーリー回想";

	private static readonly string TITLE_MOVIE = "ムービー回想";

	private static readonly string TITLE_CATEGORY = "メインストーリー";

	private static readonly string TITLE_MAIN = "メインストーリー";

	private static readonly string TITLE_CELLVAL = "セーバルぶらり旅";

	private static readonly string TITLE_MAIN2 = "メインストーリー\u3000シーズン２";

	private static readonly string TITLE_MAIN3 = "メインストーリー\u3000シーズン３";

	private static readonly string TITLE_FRIENDS = "フレンズストーリー";

	private static readonly string TITLE_EVENT = "イベントストーリー";

	private static readonly string TITLE_ARAI = "アライさん隊長日誌";

	private static readonly string TITLE_OTHER = "その他";

	private static readonly string TITLE_PVPEVENT = "とくべつくんれんシナリオ";

	private static readonly string TITLE_COMMUNICATION = "交流";

	private static readonly string TITLE_LOGIN_STORY = "ログインストーリー";

	public class Args
	{
		public SceneStoryView.Args.VIEWTYPE viewType;

		public SceneManager.SceneName resultNextSceneName;

		public object resultNextSceneArgs;

		public enum VIEWTYPE
		{
			NONE,
			MOVIE,
			PVPEVENT
		}
	}

	public class GUI
	{
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

		public SimpleAnimation select;

		public SimpleAnimation scenario;

		public SimpleAnimation chapter;

		public SimpleAnimation friend;

		public SimpleAnimation story;

		public SimpleAnimation nothing;

		public SimpleAnimation movie;

		public SimpleAnimation communication;

		public SimpleAnimation communicationSelect;

		public PguiButtonCtrl BtnScenario;

		public PguiButtonCtrl BtnMovie;

		public PguiButtonCtrl BtnPhotoAlbum;

		public PguiButtonCtrl BtnCommunication;

		public PguiButtonCtrl BtnMain;

		public PguiButtonCtrl BtnFriends;

		public PguiButtonCtrl BtnEvent;

		public PguiButtonCtrl BtnArai;

		public PguiButtonCtrl BtnPvp;
	}

	public class MOVIE
	{
		public MOVIE(Transform baseTr)
		{
			this.BtnSkip = baseTr.Find("All/Btn_Skip").GetComponent<PguiButtonCtrl>();
			this.BtnPlay = baseTr.Find("All/Btn_Play").GetComponent<PguiButtonCtrl>();
			this.BtnStop = baseTr.Find("All/Btn_Stop").GetComponent<PguiButtonCtrl>();
			this.seek = baseTr.Find("All/SeekBar").GetComponent<PguiSlider>();
		}

		public PguiButtonCtrl BtnSkip;

		public PguiButtonCtrl BtnPlay;

		public PguiButtonCtrl BtnStop;

		public PguiSlider seek;
	}

	public enum MODE
	{
		NONE,
		SELECT,
		SCENARIO,
		ANY2CATEGORY,
		CATEGORY,
		ANY2MAIN,
		MAIN,
		ANY2CELLVAL,
		CELLVAL,
		ANY2MAIN2,
		MAIN2,
		FRIENDS,
		EVENT,
		ARAI,
		PVP,
		PVP2ETC,
		ETC,
		ETC2PVP,
		STORY,
		MOVIE,
		COMMUNICATIONCHARA,
		COMMUNICATIONSELECT,
		NOTHING,
		LOAD,
		PLAY,
		STOP,
		CLOSE,
		ANY2MAIN3,
		MAIN3,
		LOGIN_SCENARIO_GROUP,
		LOGIN_SCENARIO_LIST
	}
}
