using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001FD RID: 509
public class uGUITweenColor : MonoBehaviour
{
	// Token: 0x0600216F RID: 8559 RVA: 0x0018F6D8 File Offset: 0x0018D8D8
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

	// Token: 0x06002170 RID: 8560 RVA: 0x0018F748 File Offset: 0x0018D948
	private void OnEnable()
	{
		this.Reset();
	}

	// Token: 0x06002171 RID: 8561 RVA: 0x0018F750 File Offset: 0x0018D950
	public void Reset()
	{
		this._Time = 0f;
		this._AnimCurvepos = 0f;
		this.Update();
	}

	// Token: 0x06002172 RID: 8562 RVA: 0x0018F770 File Offset: 0x0018D970
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

	// Token: 0x0400181B RID: 6171
	public Color _From = Color.white;

	// Token: 0x0400181C RID: 6172
	public Color _To = Color.white;

	// Token: 0x0400181D RID: 6173
	public AnimationCurve _AnimationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	// Token: 0x0400181E RID: 6174
	public float _Duration = 1f;

	// Token: 0x0400181F RID: 6175
	[SerializeField]
	private uGUITweenColor.PlayType _playType = uGUITweenColor.PlayType._PingPong;

	// Token: 0x04001820 RID: 6176
	public bool _UnscaledTime;

	// Token: 0x04001821 RID: 6177
	private bool _ReverceTrigger;

	// Token: 0x04001822 RID: 6178
	private float _AnimCurvepos;

	// Token: 0x04001823 RID: 6179
	private Image _RTSelf;

	// Token: 0x04001824 RID: 6180
	private RawImage _Tran;

	// Token: 0x04001825 RID: 6181
	private Text _text;

	// Token: 0x04001826 RID: 6182
	private bool _SelfCheckImage;

	// Token: 0x04001827 RID: 6183
	private bool _SelfCheckRawImage;

	// Token: 0x04001828 RID: 6184
	private bool _SelfCheckText;

	// Token: 0x04001829 RID: 6185
	private float _Time;

	// Token: 0x02001045 RID: 4165
	public enum PlayType
	{
		// Token: 0x04005B4B RID: 23371
		_Default,
		// Token: 0x04005B4C RID: 23372
		_PingPong,
		// Token: 0x04005B4D RID: 23373
		_Loop
	}
}
