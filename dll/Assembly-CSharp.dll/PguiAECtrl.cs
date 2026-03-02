using System;
using System.Collections.Generic;
using AEAuth3;

// Token: 0x020001CA RID: 458
public class PguiAECtrl : PguiBehaviour
{
	// Token: 0x06001F56 RID: 8022 RVA: 0x00183968 File Offset: 0x00181B68
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

	// Token: 0x06001F57 RID: 8023 RVA: 0x00183A27 File Offset: 0x00181C27
	public void PauseAnimationLastFrame(PguiAECtrl.AmimeType type)
	{
		this.PlayAnimation(type, this.currentFinishCallback);
		this.m_AEImage.playSpeed = 0f;
		this.m_AEImage.playTime = this.m_AEImage.playOutTime;
	}

	// Token: 0x06001F58 RID: 8024 RVA: 0x00183A5C File Offset: 0x00181C5C
	public void PauseAnimation(PguiAECtrl.AmimeType type, PguiAECtrl.FinishCallback finishCb = null)
	{
		this.PlayAnimation(type, finishCb);
		this.m_AEImage.playSpeed = 0f;
	}

	// Token: 0x06001F59 RID: 8025 RVA: 0x00183A78 File Offset: 0x00181C78
	public void PauseAnimation(PguiAECtrl.AmimeType type, float startTime, PguiAECtrl.FinishCallback finishCb = null)
	{
		this.PlayAnimation(type, finishCb);
		this.m_AEImage.playSpeed = 0f;
		AEImage aeimage = this.m_AEImage;
		this.m_AEImage.playInTime = startTime;
		aeimage.playTime = startTime;
		this.m_AEImage.PlayUpdate();
	}

	// Token: 0x06001F5A RID: 8026 RVA: 0x00183AC2 File Offset: 0x00181CC2
	public void ResumeAnimation()
	{
		this.m_AEImage.playSpeed = 1f;
	}

	// Token: 0x06001F5B RID: 8027 RVA: 0x00183AD4 File Offset: 0x00181CD4
	public void ForceEnd()
	{
		this.m_AEImage.playTime = this.m_AEImage.playOutTime;
	}

	// Token: 0x06001F5C RID: 8028 RVA: 0x00183AEC File Offset: 0x00181CEC
	public bool IsPlaying()
	{
		return !this.m_AEImage.end;
	}

	// Token: 0x06001F5D RID: 8029 RVA: 0x00183AFC File Offset: 0x00181CFC
	public PguiAECtrl.AmimeType GetAnimeType()
	{
		return this.animType;
	}

	// Token: 0x06001F5E RID: 8030 RVA: 0x00183B04 File Offset: 0x00181D04
	public PguiAECtrl.FinishCallback GetFinishCallback()
	{
		return this.currentFinishCallback;
	}

	// Token: 0x06001F5F RID: 8031 RVA: 0x00183B0C File Offset: 0x00181D0C
	private void Update()
	{
		if (this.currentFinishCallback != null && !this.IsPlaying())
		{
			PguiAECtrl.FinishCallback finishCallback = this.currentFinishCallback;
			this.currentFinishCallback = null;
			finishCallback();
		}
	}

	// Token: 0x040016C8 RID: 5832
	public AEImage m_AEImage;

	// Token: 0x040016C9 RID: 5833
	public List<PguiAECtrl.AnimeParam> m_AnimeParam;

	// Token: 0x040016CA RID: 5834
	private PguiAECtrl.FinishCallback currentFinishCallback;

	// Token: 0x040016CB RID: 5835
	private PguiAECtrl.AmimeType animType;

	// Token: 0x02001006 RID: 4102
	[Serializable]
	public class AnimeParam
	{
		// Token: 0x060051B0 RID: 20912 RVA: 0x00247567 File Offset: 0x00245767
		public AnimeParam()
		{
		}

		// Token: 0x060051B1 RID: 20913 RVA: 0x0024756F File Offset: 0x0024576F
		public AnimeParam(PguiAECtrl.AmimeType t, bool l)
		{
			this.enable = false;
			this.type = t;
			this.loop = l;
			this.start = 0f;
			this.end = 0f;
		}

		// Token: 0x040059F7 RID: 23031
		public PguiAECtrl.AmimeType type;

		// Token: 0x040059F8 RID: 23032
		public float start;

		// Token: 0x040059F9 RID: 23033
		public float end;

		// Token: 0x040059FA RID: 23034
		public bool loop;

		// Token: 0x040059FB RID: 23035
		[NonSerialized]
		public bool enable;
	}

	// Token: 0x02001007 RID: 4103
	// (Invoke) Token: 0x060051B3 RID: 20915
	public delegate void FinishCallback();

	// Token: 0x02001008 RID: 4104
	public enum AmimeType
	{
		// Token: 0x040059FD RID: 23037
		START,
		// Token: 0x040059FE RID: 23038
		LOOP,
		// Token: 0x040059FF RID: 23039
		END,
		// Token: 0x04005A00 RID: 23040
		START_SUB,
		// Token: 0x04005A01 RID: 23041
		LOOP_SUB,
		// Token: 0x04005A02 RID: 23042
		END_SUB,
		// Token: 0x04005A03 RID: 23043
		MAX
	}
}
