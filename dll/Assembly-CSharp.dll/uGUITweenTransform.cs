using System;
using UnityEngine;

// Token: 0x02000202 RID: 514
public class uGUITweenTransform : MonoBehaviour
{
	// Token: 0x06002182 RID: 8578 RVA: 0x00190539 File Offset: 0x0018E739
	private void Start()
	{
		this.TweenLocalPosGet();
	}

	// Token: 0x06002183 RID: 8579 RVA: 0x00190541 File Offset: 0x0018E741
	private void TweenLocalPosGet()
	{
		if (this._target == null)
		{
			return;
		}
		this._targetPos = this._target.transform.position;
		this._selfPos = base.transform.position;
	}

	// Token: 0x06002184 RID: 8580 RVA: 0x00190579 File Offset: 0x0018E779
	private void OnEnable()
	{
		this._Time = 0f;
		this._AnimCurvepos = 0f;
	}

	// Token: 0x06002185 RID: 8581 RVA: 0x00190591 File Offset: 0x0018E791
	public void TweenPlay()
	{
		this.TweenLocalPosGet();
		this._play = true;
	}

	// Token: 0x06002186 RID: 8582 RVA: 0x001905A0 File Offset: 0x0018E7A0
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

	// Token: 0x04001859 RID: 6233
	public GameObject _target;

	// Token: 0x0400185A RID: 6234
	public AnimationCurve _AnimationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	// Token: 0x0400185B RID: 6235
	public float _Duration = 1f;

	// Token: 0x0400185C RID: 6236
	public bool _play;

	// Token: 0x0400185D RID: 6237
	private Vector3 _TweenPos;

	// Token: 0x0400185E RID: 6238
	private Vector3 _targetPos;

	// Token: 0x0400185F RID: 6239
	private float _Time;

	// Token: 0x04001860 RID: 6240
	private Vector3 _selfPos;

	// Token: 0x04001861 RID: 6241
	private float _AnimCurvepos;
}
