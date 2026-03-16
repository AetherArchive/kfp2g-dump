using System;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PguiFadeSwitchCtrl : MonoBehaviour
{
	public int _PatternNum
	{
		get
		{
			return this._patternNum;
		}
		set
		{
			this._validate = true;
			this._patternNum = value;
		}
	}

	private CanvasGroup canvasGroup
	{
		get
		{
			if (this._canvasGroup == null)
			{
				this._canvasGroup = base.GetComponent<CanvasGroup>();
			}
			return this._canvasGroup;
		}
	}

	public PguiFadeSwitchCtrl.OnChangeInfo onChangeInfo { get; set; }

	private void OnEnable()
	{
		this.Reset();
	}

	public void Reset()
	{
		this._validate = true;
		this.Update();
	}

	private void Update()
	{
		float num = (this._DispDuration + this._MoveDuration) * (float)this._patternNum;
		float num2 = 1f / (float)this._patternNum;
		float num3 = Time.realtimeSinceStartup % num / num;
		int num4 = (int)(num3 / num2);
		num3 = num3 % num2 / num2;
		if (this._validate)
		{
			if (this.onChangeInfo != null && this._patternNum > 0)
			{
				this.onChangeInfo(num4, base.gameObject);
			}
			this._validate = false;
			return;
		}
		if (this._patternNum <= 1)
		{
			this.canvasGroup.alpha = 1f;
			return;
		}
		if (num3 < 0.5f)
		{
			this.canvasGroup.alpha = 1f;
		}
		else if (num3 < 0.75f)
		{
			this.canvasGroup.alpha = 1f - (num3 - 0.5f) / 0.25f;
		}
		else if (this._oldTime < 0.75f)
		{
			if (this.onChangeInfo != null)
			{
				this.onChangeInfo(num4, base.gameObject);
			}
			this.canvasGroup.alpha = 0f;
		}
		else
		{
			this.canvasGroup.alpha = (num3 - 0.75f) / 0.25f;
		}
		this._oldTime = num3;
	}

	private Color _From = Color.white;

	private Color _To = Color.clear;

	private AnimationCurve _AnimationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	public float _DispDuration = 1f;

	public float _MoveDuration = 0.5f;

	private int _patternNum = 1;

	private bool _validate = true;

	public bool _UnscaledTime = true;

	private float _oldTime;

	private CanvasGroup _canvasGroup;

	public delegate void OnChangeInfo(int count, GameObject go);
}
