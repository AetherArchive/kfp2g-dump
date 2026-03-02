using System;
using UnityEngine;

// Token: 0x020001FF RID: 511
public class uGUITweenPosition : MonoBehaviour
{
	// Token: 0x06002177 RID: 8567 RVA: 0x0018FCC8 File Offset: 0x0018DEC8
	private void Start()
	{
		if (base.GetComponent<RectTransform>())
		{
			this._RTSelf = base.GetComponent<RectTransform>();
			this._SelfUIChecker = true;
			return;
		}
		this._Tran = base.GetComponent<Transform>();
		this._SelfUIChecker = false;
	}

	// Token: 0x06002178 RID: 8568 RVA: 0x0018FCFE File Offset: 0x0018DEFE
	private void OnEnable()
	{
		this._Time = 0f;
		this._AnimCurvepos = 0f;
	}

	// Token: 0x06002179 RID: 8569 RVA: 0x0018FD18 File Offset: 0x0018DF18
	private void Update()
	{
		float num = 1f / this._Duration;
		this._Time = (this.IsUnscaledTime ? (Time.unscaledDeltaTime * num) : (Time.deltaTime * num));
		if (!this._ReverceTrigger)
		{
			this._Time *= -1f;
		}
		this._AnimCurvepos = Mathf.Clamp(this._AnimCurvepos + this._Time, 0f, 1f);
		if (this._AnimCurvepos <= 0f)
		{
			this._ReverceTrigger = true;
		}
		if (this._AnimCurvepos >= 1f)
		{
			uGUITweenPosition.PlayType playType = this._playType;
		}
		if (this._AnimCurvepos >= 1f && this._playType == uGUITweenPosition.PlayType._PingPong)
		{
			this._ReverceTrigger = false;
		}
		if (this._AnimCurvepos >= 1f && this._playType == uGUITweenPosition.PlayType._Loop)
		{
			this._AnimCurvepos -= 0.999f;
		}
		Vector3 vector = new Vector3(Mathf.Lerp(this._From.x, this._To.x, this._AnimationCurve.Evaluate(this._AnimCurvepos)), Mathf.Lerp(this._From.y, this._To.y, this._AnimationCurve.Evaluate(this._AnimCurvepos)), Mathf.Lerp(this._From.z, this._To.z, this._AnimationCurve.Evaluate(this._AnimCurvepos)));
		if (this._SelfUIChecker)
		{
			this._RTSelf.anchoredPosition = vector;
			return;
		}
		this._Tran.transform.localPosition = vector;
	}

	// Token: 0x04001836 RID: 6198
	public Vector3 _From;

	// Token: 0x04001837 RID: 6199
	public Vector3 _To;

	// Token: 0x04001838 RID: 6200
	public AnimationCurve _AnimationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	// Token: 0x04001839 RID: 6201
	public float _Duration = 1f;

	// Token: 0x0400183A RID: 6202
	[SerializeField]
	private uGUITweenPosition.PlayType _playType = uGUITweenPosition.PlayType._PingPong;

	// Token: 0x0400183B RID: 6203
	[SerializeField]
	private bool IsUnscaledTime;

	// Token: 0x0400183C RID: 6204
	private bool _ReverceTrigger;

	// Token: 0x0400183D RID: 6205
	private float _AnimCurvepos;

	// Token: 0x0400183E RID: 6206
	private RectTransform _RTSelf;

	// Token: 0x0400183F RID: 6207
	private Transform _Tran;

	// Token: 0x04001840 RID: 6208
	private bool _SelfUIChecker = true;

	// Token: 0x04001841 RID: 6209
	private float _Time;

	// Token: 0x02001047 RID: 4167
	public enum PlayType
	{
		// Token: 0x04005B53 RID: 23379
		_Default,
		// Token: 0x04005B54 RID: 23380
		_PingPong,
		// Token: 0x04005B55 RID: 23381
		_Loop
	}
}
