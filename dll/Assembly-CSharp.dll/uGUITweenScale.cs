using System;
using UnityEngine;

public class uGUITweenScale : MonoBehaviour
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

	private void OnEnable()
	{
		this._Time = 0f;
		this._AnimCurvescale = 0f;
	}

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

	public Vector3 _From = new Vector3(1f, 1f, 1f);

	public Vector3 _To = new Vector3(1f, 1f, 1f);

	public AnimationCurve _AnimationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	public float _Duration = 1f;

	[SerializeField]
	private uGUITweenScale.PlayType _playType = uGUITweenScale.PlayType._PingPong;

	[SerializeField]
	private bool synchronize;

	private bool _ReverceTrigger;

	private float _AnimCurvescale;

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
