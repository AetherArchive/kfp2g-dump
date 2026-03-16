using System;
using UnityEngine;
using UnityEngine.UI;

public class uGUITweenColor : MonoBehaviour
{
	private void Start()
	{
		if (base.GetComponent<Image>())
		{
			this._RTSelf = base.GetComponent<Image>();
			this._SelfCheckImage = true;
			return;
		}
		if (base.GetComponent<RawImage>())
		{
			this._Tran = base.GetComponent<RawImage>();
			this._SelfCheckRawImage = true;
			return;
		}
		if (!base.GetComponent<Text>())
		{
			return;
		}
		this._text = base.GetComponent<Text>();
		this._SelfCheckText = true;
	}

	private void OnEnable()
	{
		this.Reset();
	}

	public void Reset()
	{
		this._Time = 0f;
		this._AnimCurvepos = 0f;
		this.Update();
	}

	private void Update()
	{
		float num = 1f / this._Duration;
		if (this._UnscaledTime)
		{
			this._Time = Time.unscaledDeltaTime * num;
		}
		else
		{
			this._Time = Time.deltaTime * num;
		}
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
			uGUITweenColor.PlayType playType = this._playType;
		}
		if (this._AnimCurvepos >= 1f && this._playType == uGUITweenColor.PlayType._PingPong)
		{
			this._ReverceTrigger = false;
		}
		if (this._AnimCurvepos >= 1f && this._playType == uGUITweenColor.PlayType._Loop)
		{
			this._AnimCurvepos -= 0.999f;
		}
		Color color = new Color(Mathf.Lerp(this._From.r, this._To.r, this._AnimationCurve.Evaluate(this._AnimCurvepos)), Mathf.Lerp(this._From.g, this._To.g, this._AnimationCurve.Evaluate(this._AnimCurvepos)), Mathf.Lerp(this._From.b, this._To.b, this._AnimationCurve.Evaluate(this._AnimCurvepos)), Mathf.Lerp(this._From.a, this._To.a, this._AnimationCurve.Evaluate(this._AnimCurvepos)));
		if (this._SelfCheckImage)
		{
			this._RTSelf.color = color;
		}
		if (this._SelfCheckRawImage)
		{
			this._Tran.color = color;
		}
		if (this._SelfCheckText)
		{
			this._text.color = color;
		}
	}

	public Color _From = Color.white;

	public Color _To = Color.white;

	public AnimationCurve _AnimationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	public float _Duration = 1f;

	[SerializeField]
	private uGUITweenColor.PlayType _playType = uGUITweenColor.PlayType._PingPong;

	public bool _UnscaledTime;

	private bool _ReverceTrigger;

	private float _AnimCurvepos;

	private Image _RTSelf;

	private RawImage _Tran;

	private Text _text;

	private bool _SelfCheckImage;

	private bool _SelfCheckRawImage;

	private bool _SelfCheckText;

	private float _Time;

	public enum PlayType
	{
		_Default,
		_PingPong,
		_Loop
	}
}
