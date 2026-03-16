using System;
using UnityEngine;

public class uGUITweenRotation : MonoBehaviour
{
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

	public Vector3 _From;

	public Vector3 _To;

	public AnimationCurve _AnimationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	public float _Duration = 1f;

	[SerializeField]
	private uGUITweenRotation.PlayType _playType = uGUITweenRotation.PlayType._PingPong;

	private bool _ReverceTrigger;

	private float _AnimCurverot;

	private RectTransform _RTSelf;

	private Transform _Tran;

	private bool _SelfUIChecker = true;

	private float _Time;

	public enum PlayType
	{
		_Default,
		_PingPong,
		_Loop
	}
}
