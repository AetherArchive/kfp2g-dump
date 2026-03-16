using System;
using UnityEngine;

public class uGUITweenTransform : MonoBehaviour
{
	private void Start()
	{
		this.TweenLocalPosGet();
	}

	private void TweenLocalPosGet()
	{
		if (this._target == null)
		{
			return;
		}
		this._targetPos = this._target.transform.position;
		this._selfPos = base.transform.position;
	}

	private void OnEnable()
	{
		this._Time = 0f;
		this._AnimCurvepos = 0f;
	}

	public void TweenPlay()
	{
		this.TweenLocalPosGet();
		this._play = true;
	}

	private void Update()
	{
		if (this._play)
		{
			float num = 1f / this._Duration;
			this._Time = Time.deltaTime * num;
			this._AnimCurvepos = Mathf.Clamp(this._AnimCurvepos + this._Time, 0f, 1f);
			this._TweenPos = Vector3.Lerp(this._selfPos, this._targetPos, this._AnimationCurve.Evaluate(this._AnimCurvepos));
			base.transform.position = this._TweenPos;
		}
		else
		{
			this._Time = 0f;
			this._AnimCurvepos = 0f;
		}
		if (1f <= this._AnimCurvepos)
		{
			this._play = false;
		}
	}

	public GameObject _target;

	public AnimationCurve _AnimationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	public float _Duration = 1f;

	public bool _play;

	private Vector3 _TweenPos;

	private Vector3 _targetPos;

	private float _Time;

	private Vector3 _selfPos;

	private float _AnimCurvepos;
}
