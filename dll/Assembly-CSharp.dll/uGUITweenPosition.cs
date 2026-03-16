using System;
using UnityEngine;

public class uGUITweenPosition : MonoBehaviour
{
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

	private void OnEnable()
	{
		this._Time = 0f;
		this._AnimCurvepos = 0f;
	}

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

	public Vector3 _From;

	public Vector3 _To;

	public AnimationCurve _AnimationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	public float _Duration = 1f;

	[SerializeField]
	private uGUITweenPosition.PlayType _playType = uGUITweenPosition.PlayType._PingPong;

	[SerializeField]
	private bool IsUnscaledTime;

	private bool _ReverceTrigger;

	private float _AnimCurvepos;

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
