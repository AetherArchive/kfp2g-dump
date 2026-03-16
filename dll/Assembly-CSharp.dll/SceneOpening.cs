using System;
using System.Collections;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.UI;

public class SceneOpening : BaseScene
{
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject("GUI_Opening", new Type[] { typeof(RectTransform) });
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		this.moviePanel = new GameObject("movie", new Type[] { typeof(RectTransform) });
		RectTransform component = this.moviePanel.GetComponent<RectTransform>();
		component.SetParent(this.basePanel.transform, false);
		component.sizeDelta = new Vector2(1280f, 720f);
		this.moviePanel.AddComponent<RawImage>();
		this.basePanel.SetActive(false);
		this.ctrlPanel = AssetManager.InstantiateAssetData("SceneMenu/GUI/Prefab/GUI_Movie", null).GetComponent<SimpleAnimation>();
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.ctrlPanel.transform, true);
		this.ctrlData = new SceneOpening.CTRL(this.ctrlPanel.transform);
		this.ctrlData.BtnSkip.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.ctrlData.BtnPlay.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.ctrlData.BtnStop.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.ctrlPanel.gameObject.SetActive(false);
	}

	public override void OnEnableScene(object args)
	{
		this.basePanel.SetActive(true);
		this.moviePanel.SetActive(false);
		this.ctrlPanel.gameObject.SetActive(true);
		this.ctrlPanel.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END);
		SoundManager.PlayBGM("prd_bgm0007");
		this.step = SceneOpening.STEP.INIT;
	}

	public override void OnStartControl()
	{
	}

	public override void Update()
	{
		switch (this.step)
		{
		case SceneOpening.STEP.DOWNLOAD:
			if (!this.load.MoveNext())
			{
				this.load = null;
				if (this.cancel)
				{
					this.step = SceneOpening.STEP.END;
				}
				else
				{
					SoundManager.StopBGM(15);
					this.moviePanel.SetActive(true);
					MoviePlayer.Play(this.moviePanel, AssetManager.ASSET_OP_MOVIE, false);
					this.movieTime = -1f;
					this.seekTime = -1f;
					this.step = SceneOpening.STEP.PLAY;
				}
			}
			break;
		case SceneOpening.STEP.PLAY:
		case SceneOpening.STEP.STOP:
			MoviePlayer.Pause(this.moviePanel, this.step == SceneOpening.STEP.STOP);
			Screen.sleepTimeout = ((this.step == SceneOpening.STEP.STOP) ? (-2) : (-1));
			if (MoviePlayer.Playing(this.moviePanel))
			{
				if (MoviePlayer.Touch(this.moviePanel))
				{
					this.movieTime = SceneOpening.CTRL_TIME;
				}
				if (this.ctrlData.seek.touch)
				{
					this.movieTime = SceneOpening.CTRL_TIME;
					this.seekTime = this.ctrlData.seek.normalizedValue;
				}
				else if (this.seekTime < 0f)
				{
					this.ctrlData.seek.normalizedValue = MoviePlayer.GetTime(this.moviePanel);
				}
				else
				{
					this.step = SceneOpening.STEP.PLAY;
					MoviePlayer.SetTime(this.moviePanel, this.seekTime);
					this.seekTime = -1f;
				}
			}
			else
			{
				this.step = SceneOpening.STEP.END;
				this.movieTime = 0f;
			}
			break;
		case SceneOpening.STEP.END:
			Screen.sleepTimeout = -2;
			if (DataManager.DmUserInfo.tutorialSequence == TutorialUtil.Sequence.OPENING_MOVIE)
			{
				TutorialUtil.RequestNextSequence(DataManager.DmUserInfo.tutorialSequence);
			}
			else
			{
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneTitle, null);
			}
			break;
		default:
			this.cancel = false;
			this.load = AssetDownloadResolver.ResolveActionOpMovie(delegate
			{
				this.cancel = true;
			});
			this.step = SceneOpening.STEP.DOWNLOAD;
			break;
		}
		if ((this.step == SceneOpening.STEP.PLAY || this.step == SceneOpening.STEP.STOP) && this.movieTime > 0f)
		{
			this.movieTime -= TimeManager.DeltaTime;
			if (!this.ctrlPanel.ExIsCurrent(SimpleAnimation.ExPguiStatus.START))
			{
				this.ctrlPanel.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			}
			this.ctrlData.BtnStop.gameObject.SetActive(this.step == SceneOpening.STEP.PLAY);
			this.ctrlData.BtnPlay.gameObject.SetActive(this.step == SceneOpening.STEP.STOP);
			return;
		}
		if (!this.ctrlPanel.ExIsCurrent(SimpleAnimation.ExPguiStatus.END))
		{
			this.ctrlPanel.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
		}
	}

	public override void OnDisableScene()
	{
		this.moviePanel.SetActive(false);
		this.basePanel.SetActive(false);
		this.ctrlPanel.gameObject.SetActive(false);
	}

	public override void OnDestroyScene()
	{
		this.moviePanel = null;
		Object.Destroy(this.basePanel);
		this.basePanel = null;
		Object.Destroy(this.ctrlPanel.gameObject);
		this.ctrlPanel = null;
	}

	private void OnClickButton(PguiButtonCtrl button)
	{
		if (button == this.ctrlData.BtnSkip)
		{
			if (this.step == SceneOpening.STEP.PLAY || this.step == SceneOpening.STEP.STOP)
			{
				this.step = SceneOpening.STEP.END;
				return;
			}
		}
		else
		{
			if (button == this.ctrlData.BtnStop)
			{
				if (this.step == SceneOpening.STEP.PLAY)
				{
					this.step = SceneOpening.STEP.STOP;
				}
				this.movieTime = SceneOpening.CTRL_TIME;
				return;
			}
			if (button == this.ctrlData.BtnPlay)
			{
				if (this.step == SceneOpening.STEP.STOP)
				{
					this.step = SceneOpening.STEP.PLAY;
				}
				this.movieTime = SceneOpening.CTRL_TIME;
			}
		}
	}

	private GameObject basePanel;

	private GameObject moviePanel;

	private SceneOpening.STEP step;

	private IEnumerator load;

	private bool cancel;

	private SimpleAnimation ctrlPanel;

	private SceneOpening.CTRL ctrlData;

	private float movieTime;

	private float seekTime;

	private static readonly float CTRL_TIME = 1.5f;

	private enum STEP
	{
		INIT,
		DOWNLOAD,
		PLAY,
		STOP,
		END
	}

	public class CTRL
	{
		public CTRL(Transform baseTr)
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
}
