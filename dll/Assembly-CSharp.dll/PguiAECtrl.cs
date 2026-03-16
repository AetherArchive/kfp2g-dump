using System;
using System.Collections.Generic;
using AEAuth3;

public class PguiAECtrl : PguiBehaviour
{
	public void PlayAnimation(PguiAECtrl.AmimeType type, PguiAECtrl.FinishCallback finishCb = null)
	{
		this.animType = type;
		PguiAECtrl.AnimeParam animeParam = this.m_AnimeParam.Find((PguiAECtrl.AnimeParam item) => item.type == this.animType);
		bool flag = animeParam != null;
		this.m_AEImage.playTime = (this.m_AEImage.playInTime = (flag ? animeParam.start : 0f));
		this.m_AEImage.playOutTime = (flag ? animeParam.end : this.m_AEImage.duration);
		this.m_AEImage.playSpeed = 1f;
		this.m_AEImage.playLoop = (flag ? animeParam.loop : (type == PguiAECtrl.AmimeType.LOOP || type == PguiAECtrl.AmimeType.LOOP_SUB));
		this.m_AEImage.autoPlay = true;
		this.currentFinishCallback = finishCb;
	}

	public void PauseAnimationLastFrame(PguiAECtrl.AmimeType type)
	{
		this.PlayAnimation(type, this.currentFinishCallback);
		this.m_AEImage.playSpeed = 0f;
		this.m_AEImage.playTime = this.m_AEImage.playOutTime;
	}

	public void PauseAnimation(PguiAECtrl.AmimeType type, PguiAECtrl.FinishCallback finishCb = null)
	{
		this.PlayAnimation(type, finishCb);
		this.m_AEImage.playSpeed = 0f;
	}

	public void PauseAnimation(PguiAECtrl.AmimeType type, float startTime, PguiAECtrl.FinishCallback finishCb = null)
	{
		this.PlayAnimation(type, finishCb);
		this.m_AEImage.playSpeed = 0f;
		AEImage aeimage = this.m_AEImage;
		this.m_AEImage.playInTime = startTime;
		aeimage.playTime = startTime;
		this.m_AEImage.PlayUpdate();
	}

	public void ResumeAnimation()
	{
		this.m_AEImage.playSpeed = 1f;
	}

	public void ForceEnd()
	{
		this.m_AEImage.playTime = this.m_AEImage.playOutTime;
	}

	public bool IsPlaying()
	{
		return !this.m_AEImage.end;
	}

	public PguiAECtrl.AmimeType GetAnimeType()
	{
		return this.animType;
	}

	public PguiAECtrl.FinishCallback GetFinishCallback()
	{
		return this.currentFinishCallback;
	}

	private void Update()
	{
		if (this.currentFinishCallback != null && !this.IsPlaying())
		{
			PguiAECtrl.FinishCallback finishCallback = this.currentFinishCallback;
			this.currentFinishCallback = null;
			finishCallback();
		}
	}

	public AEImage m_AEImage;

	public List<PguiAECtrl.AnimeParam> m_AnimeParam;

	private PguiAECtrl.FinishCallback currentFinishCallback;

	private PguiAECtrl.AmimeType animType;

	[Serializable]
	public class AnimeParam
	{
		public AnimeParam()
		{
		}

		public AnimeParam(PguiAECtrl.AmimeType t, bool l)
		{
			this.enable = false;
			this.type = t;
			this.loop = l;
			this.start = 0f;
			this.end = 0f;
		}

		public PguiAECtrl.AmimeType type;

		public float start;

		public float end;

		public bool loop;

		[NonSerialized]
		public bool enable;
	}

	public delegate void FinishCallback();

	public enum AmimeType
	{
		START,
		LOOP,
		END,
		START_SUB,
		LOOP_SUB,
		END_SUB,
		MAX
	}
}
