using System;
using UnityEngine;

// Token: 0x020001D2 RID: 466
[RequireComponent(typeof(CanvasGroup))]
public class PguiFadeSwitchCtrl : MonoBehaviour
{
	// Token: 0x17000439 RID: 1081
	// (get) Token: 0x06001FAD RID: 8109 RVA: 0x001872A2 File Offset: 0x001854A2
	// (set) Token: 0x06001FAE RID: 8110 RVA: 0x001872AA File Offset: 0x001854AA
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

	// Token: 0x1700043A RID: 1082
	// (get) Token: 0x06001FAF RID: 8111 RVA: 0x001872BA File Offset: 0x001854BA
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

	// Token: 0x1700043B RID: 1083
	// (get) Token: 0x06001FB1 RID: 8113 RVA: 0x001872E5 File Offset: 0x001854E5
	// (set) Token: 0x06001FB0 RID: 8112 RVA: 0x001872DC File Offset: 0x001854DC
	public PguiFadeSwitchCtrl.OnChangeInfo onChangeInfo { get; set; }

	// Token: 0x06001FB2 RID: 8114 RVA: 0x001872ED File Offset: 0x001854ED
	private void OnEnable()
	{
		this.Reset();
	}

	// Token: 0x06001FB3 RID: 8115 RVA: 0x001872F5 File Offset: 0x001854F5
	public void Reset()
	{
		this._validate = true;
		this.Update();
	}

	// Token: 0x06001FB4 RID: 8116 RVA: 0x00187304 File Offset: 0x00185504
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

	// Token: 0x0400170C RID: 5900
	private Color _From = Color.white;

	// Token: 0x0400170D RID: 5901
	private Color _To = Color.clear;

	// Token: 0x0400170E RID: 5902
	private AnimationCurve _AnimationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	// Token: 0x0400170F RID: 5903
	public float _DispDuration = 1f;

	// Token: 0x04001710 RID: 5904
	public float _MoveDuration = 0.5f;

	// Token: 0x04001711 RID: 5905
	private int _patternNum = 1;

	// Token: 0x04001712 RID: 5906
	private bool _validate = true;

	// Token: 0x04001713 RID: 5907
	public bool _UnscaledTime = true;

	// Token: 0x04001714 RID: 5908
	private float _oldTime;

	// Token: 0x04001715 RID: 5909
	private CanvasGroup _canvasGroup;

	// Token: 0x02001015 RID: 4117
	// (Invoke) Token: 0x060051E5 RID: 20965
	public delegate void OnChangeInfo(int count, GameObject go);
}
