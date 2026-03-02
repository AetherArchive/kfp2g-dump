using System;
using System.Collections;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000154 RID: 340
public class SceneOpening : BaseScene
{
	// Token: 0x06001385 RID: 4997 RVA: 0x000F1770 File Offset: 0x000EF970
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

	// Token: 0x06001386 RID: 4998 RVA: 0x000F18C4 File Offset: 0x000EFAC4
	public override void OnEnableScene(object args)
	{
		this.basePanel.SetActive(true);
		this.moviePanel.SetActive(false);
		this.ctrlPanel.gameObject.SetActive(true);
		this.ctrlPanel.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END);
		SoundManager.PlayBGM("prd_bgm0007");
		this.step = SceneOpening.STEP.INIT;
	}

	// Token: 0x06001387 RID: 4999 RVA: 0x000F1917 File Offset: 0x000EFB17
	public override void OnStartControl()
	{
	}

	// Token: 0x06001388 RID: 5000 RVA: 0x000F191C File Offset: 0x000EFB1C
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

	// Token: 0x06001389 RID: 5001 RVA: 0x000F1BA3 File Offset: 0x000EFDA3
	public override void OnDisableScene()
	{
		this.moviePanel.SetActive(false);
		this.basePanel.SetActive(false);
		this.ctrlPanel.gameObject.SetActive(false);
	}

	// Token: 0x0600138A RID: 5002 RVA: 0x000F1BCE File Offset: 0x000EFDCE
	public override void OnDestroyScene()
	{
		this.moviePanel = null;
		Object.Destroy(this.basePanel);
		this.basePanel = null;
		Object.Destroy(this.ctrlPanel.gameObject);
		this.ctrlPanel = null;
	}

	// Token: 0x0600138B RID: 5003 RVA: 0x000F1C00 File Offset: 0x000EFE00
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

	// Token: 0x0400103B RID: 4155
	private GameObject basePanel;

	// Token: 0x0400103C RID: 4156
	private GameObject moviePanel;

	// Token: 0x0400103D RID: 4157
	private SceneOpening.STEP step;

	// Token: 0x0400103E RID: 4158
	private IEnumerator load;

	// Token: 0x0400103F RID: 4159
	private bool cancel;

	// Token: 0x04001040 RID: 4160
	private SimpleAnimation ctrlPanel;

	// Token: 0x04001041 RID: 4161
	private SceneOpening.CTRL ctrlData;

	// Token: 0x04001042 RID: 4162
	private float movieTime;

	// Token: 0x04001043 RID: 4163
	private float seekTime;

	// Token: 0x04001044 RID: 4164
	private static readonly float CTRL_TIME = 1.5f;

	// Token: 0x02000B3A RID: 2874
	private enum STEP
	{
		// Token: 0x04004684 RID: 18052
		INIT,
		// Token: 0x04004685 RID: 18053
		DOWNLOAD,
		// Token: 0x04004686 RID: 18054
		PLAY,
		// Token: 0x04004687 RID: 18055
		STOP,
		// Token: 0x04004688 RID: 18056
		END
	}

	// Token: 0x02000B3B RID: 2875
	public class CTRL
	{
		// Token: 0x0600421E RID: 16926 RVA: 0x001FEB48 File Offset: 0x001FCD48
		public CTRL(Transform baseTr)
		{
			this.BtnSkip = baseTr.Find("All/Btn_Skip").GetComponent<PguiButtonCtrl>();
			this.BtnPlay = baseTr.Find("All/Btn_Play").GetComponent<PguiButtonCtrl>();
			this.BtnStop = baseTr.Find("All/Btn_Stop").GetComponent<PguiButtonCtrl>();
			this.seek = baseTr.Find("All/SeekBar").GetComponent<PguiSlider>();
		}

		// Token: 0x04004689 RID: 18057
		public PguiButtonCtrl BtnSkip;

		// Token: 0x0400468A RID: 18058
		public PguiButtonCtrl BtnPlay;

		// Token: 0x0400468B RID: 18059
		public PguiButtonCtrl BtnStop;

		// Token: 0x0400468C RID: 18060
		public PguiSlider seek;
	}
}
