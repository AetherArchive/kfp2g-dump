using System;
using UnityEngine;

// Token: 0x02000201 RID: 513
public class uGUITweenScale : MonoBehaviour
{
	// Token: 0x0600217E RID: 8574 RVA: 0x00190179 File Offset: 0x0018E379
	private void Start()
	{
		if (!base.GetComponent<Transform>())
		{
			this._RTSelf = base.GetComponent<RectTransform>();
			this._SelfUIChecker = true;
			return;
		}
		this._Tran = base.GetComponent<Transform>();
		this._SelfUIChecker = false;
	}

	// Token: 0x0600217F RID: 8575 RVA: 0x001901AF File Offset: 0x0018E3AF
	private void OnEnable()
	{
		this._Time = 0f;
		this._AnimCurvescale = 0f;
	}

	// Token: 0x06002180 RID: 8576 RVA: 0x001901C8 File Offset: 0x0018E3C8
	private void Update()
	{
		float num = 1f / this._Duration;
		if (this.synchronize)
		{
			float num2 = 10000000f * this._Duration;
			if (this._playType == uGUITweenScale.PlayType._PingPong)
			{
				num2 *= 2f;
			}
			long num3 = TimeManager.SystemNow.Ticks % (long)num2;
			this._Time = (float)num3 / num2;
			this._AnimCurvescale = Mathf.Clamp(this._Time, 0f, 1f);
			if (this._AnimCurvescale > 0.5f && this._playType == uGUITweenScale.PlayType._PingPong)
			{
				this._AnimCurvescale = 1f - this._AnimCurvescale;
			}
			if (this._playType == uGUITweenScale.PlayType._PingPong)
			{
				this._AnimCurvescale *= 2f;
				this._AnimCurvescale = Mathf.Clamp(this._AnimCurvescale, 0f, 1f);
			}
			if (this._AnimCurvescale <= 0f)
			{
				this._ReverceTrigger = true;
			}
			if (this._AnimCurvescale >= 1f)
			{
				uGUITweenScale.PlayType playType = this._playType;
			}
			if (this._AnimCurvescale >= 0.5f && this._playType == uGUITweenScale.PlayType._PingPong)
			{
				this._ReverceTrigger = false;
			}
			if (this._AnimCurvescale >= 1f && this._playType == uGUITweenScale.PlayType._Loop)
			{
				this._AnimCurvescale -= 0.999f;
			}
		}
		else
		{
			this._Time = Time.deltaTime * num;
			if (!this._ReverceTrigger)
			{
				this._Time *= -1f;
			}
			this._AnimCurvescale = Mathf.Clamp(this._AnimCurvescale + this._Time, 0f, 1f);
			if (this._AnimCurvescale <= 0f)
			{
				this._ReverceTrigger = true;
			}
			if (this._AnimCurvescale >= 1f)
			{
				uGUITweenScale.PlayType playType2 = this._playType;
			}
			if (this._AnimCurvescale >= 1f && this._playType == uGUITweenScale.PlayType._PingPong)
			{
				this._ReverceTrigger = false;
			}
			if (this._AnimCurvescale >= 1f && this._playType == uGUITweenScale.PlayType._Loop)
			{
				this._AnimCurvescale -= 0.999f;
			}
		}
		Vector3 vector = new Vector3(Mathf.Lerp(this._From.x, this._To.x, this._AnimationCurve.Evaluate(this._AnimCurvescale)), Mathf.Lerp(this._From.y, this._To.y, this._AnimationCurve.Evaluate(this._AnimCurvescale)), Mathf.Lerp(this._From.z, this._To.z, this._AnimationCurve.Evaluate(this._AnimCurvescale)));
		if (this._SelfUIChecker)
		{
			this._RTSelf.sizeDelta = vector;
			return;
		}
		this._Tran.transform.localScale = vector;
	}

	// Token: 0x0400184D RID: 6221
	public Vector3 _From = new Vector3(1f, 1f, 1f);

	// Token: 0x0400184E RID: 6222
	public Vector3 _To = new Vector3(1f, 1f, 1f);

	// Token: 0x0400184F RID: 6223
	public AnimationCurve _AnimationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	// Token: 0x04001850 RID: 6224
	public float _Duration = 1f;

	// Token: 0x04001851 RID: 6225
	[SerializeField]
	private uGUITweenScale.PlayType _playType = uGUITweenScale.PlayType._PingPong;

	// Token: 0x04001852 RID: 6226
	[SerializeField]
	private bool synchronize;

	// Token: 0x04001853 RID: 6227
	private bool _ReverceTrigger;

	// Token: 0x04001854 RID: 6228
	private float _AnimCurvescale;

	// Token: 0x04001855 RID: 6229
	private RectTransform _RTSelf;

	// Token: 0x04001856 RID: 6230
	private Transform _Tran;

	// Token: 0x04001857 RID: 6231
	private bool _SelfUIChecker = true;

	// Token: 0x04001858 RID: 6232
	private float _Time;

	// Token: 0x02001049 RID: 4169
	public enum PlayType
	{
		// Token: 0x04005B5B RID: 23387
		_Default,
		// Token: 0x04005B5C RID: 23388
		_PingPong,
		// Token: 0x04005B5D RID: 23389
		_Loop
	}
}
