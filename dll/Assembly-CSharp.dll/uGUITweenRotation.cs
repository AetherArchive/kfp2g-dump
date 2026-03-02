using System;
using UnityEngine;

// Token: 0x02000200 RID: 512
public class uGUITweenRotation : MonoBehaviour
{
	// Token: 0x0600217B RID: 8571 RVA: 0x0018FF2D File Offset: 0x0018E12D
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

	// Token: 0x0600217C RID: 8572 RVA: 0x0018FF64 File Offset: 0x0018E164
	private void Update()
	{
		float num = 1f / this._Duration;
		this._Time = Time.deltaTime * num;
		if (!this._ReverceTrigger)
		{
			this._Time *= -1f;
		}
		this._AnimCurverot = Mathf.Clamp(this._AnimCurverot + this._Time, 0f, 1f);
		if (this._AnimCurverot <= 0f)
		{
			this._ReverceTrigger = true;
		}
		if (this._AnimCurverot >= 1f && this._playType == uGUITweenRotation.PlayType._Default)
		{
			this._ReverceTrigger = false;
			base.GetComponent<uGUITweenRotation>().enabled = false;
		}
		if (this._AnimCurverot >= 1f && this._playType == uGUITweenRotation.PlayType._PingPong)
		{
			this._ReverceTrigger = false;
		}
		if (this._AnimCurverot >= 1f && this._playType == uGUITweenRotation.PlayType._Loop)
		{
			this._AnimCurverot -= 0.999f;
		}
		Vector3 vector = new Vector3(Mathf.Lerp(this._From.x, this._To.x, this._AnimationCurve.Evaluate(this._AnimCurverot)), Mathf.Lerp(this._From.y, this._To.y, this._AnimationCurve.Evaluate(this._AnimCurverot)), Mathf.Lerp(this._From.z, this._To.z, this._AnimationCurve.Evaluate(this._AnimCurverot)));
		if (this._SelfUIChecker)
		{
			this._RTSelf.localEulerAngles = vector;
			return;
		}
		this._Tran.transform.eulerAngles = vector;
	}

	// Token: 0x04001842 RID: 6210
	public Vector3 _From;

	// Token: 0x04001843 RID: 6211
	public Vector3 _To;

	// Token: 0x04001844 RID: 6212
	public AnimationCurve _AnimationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	// Token: 0x04001845 RID: 6213
	public float _Duration = 1f;

	// Token: 0x04001846 RID: 6214
	[SerializeField]
	private uGUITweenRotation.PlayType _playType = uGUITweenRotation.PlayType._PingPong;

	// Token: 0x04001847 RID: 6215
	private bool _ReverceTrigger;

	// Token: 0x04001848 RID: 6216
	private float _AnimCurverot;

	// Token: 0x04001849 RID: 6217
	private RectTransform _RTSelf;

	// Token: 0x0400184A RID: 6218
	private Transform _Tran;

	// Token: 0x0400184B RID: 6219
	private bool _SelfUIChecker = true;

	// Token: 0x0400184C RID: 6220
	private float _Time;

	// Token: 0x02001048 RID: 4168
	public enum PlayType
	{
		// Token: 0x04005B57 RID: 23383
		_Default,
		// Token: 0x04005B58 RID: 23384
		_PingPong,
		// Token: 0x04005B59 RID: 23385
		_Loop
	}
}
