using System;
using UnityEngine;

// Token: 0x020001FE RID: 510
public class uGUITweenMeshColor : MonoBehaviour
{
	// Token: 0x06002174 RID: 8564 RVA: 0x0018F9D8 File Offset: 0x0018DBD8
	private void Start()
	{
		if (!base.GetComponent<MeshFilter>() && !base.GetComponent<SpriteRenderer>())
		{
			return;
		}
		if (base.GetComponent<MeshFilter>())
		{
			this._MeshSelf = base.GetComponent<MeshFilter>().mesh;
			this._BoolMesh = true;
		}
		if (base.GetComponent<SpriteRenderer>())
		{
			this._SpriteRendere = base.GetComponent<SpriteRenderer>();
			this._BoolSR = true;
			return;
		}
	}

	// Token: 0x06002175 RID: 8565 RVA: 0x0018FA48 File Offset: 0x0018DC48
	private void Update()
	{
		float num = 1f / this._Duration;
		this._Time = Time.deltaTime * num;
		if (!this._ReverceTrigger)
		{
			this._Time *= -1f;
		}
		this._AnimCurvepos = Mathf.Clamp(this._AnimCurvepos + this._Time, 0f, 1f);
		if (this._AnimCurvepos <= 0f)
		{
			this._ReverceTrigger = true;
		}
		if (this._AnimCurvepos >= 1f && this._playType == uGUITweenMeshColor.PlayType._Default)
		{
			this._ReverceTrigger = false;
			base.GetComponent<uGUITweenPosition>().enabled = false;
		}
		if (this._AnimCurvepos >= 1f && this._playType == uGUITweenMeshColor.PlayType._PingPong)
		{
			this._ReverceTrigger = false;
		}
		if (this._AnimCurvepos >= 1f && this._playType == uGUITweenMeshColor.PlayType._Loop)
		{
			this._AnimCurvepos -= 0.999f;
		}
		Color color = new Color(Mathf.Lerp(this._From.r, this._To.r, this._AnimationCurve.Evaluate(this._AnimCurvepos)), Mathf.Lerp(this._From.g, this._To.g, this._AnimationCurve.Evaluate(this._AnimCurvepos)), Mathf.Lerp(this._From.b, this._To.b, this._AnimationCurve.Evaluate(this._AnimCurvepos)), Mathf.Lerp(this._From.a, this._To.a, this._AnimationCurve.Evaluate(this._AnimCurvepos)));
		if (this._BoolMesh)
		{
			Vector3[] vertices = this._MeshSelf.vertices;
			Color[] array = new Color[vertices.Length];
			for (int i = 0; i < vertices.Length; i++)
			{
				array[i] = color;
			}
			this._MeshSelf.colors = array;
		}
		if (this._BoolSR)
		{
			this._SpriteRendere.color = color;
		}
	}

	// Token: 0x0400182A RID: 6186
	public Color _From = Color.white;

	// Token: 0x0400182B RID: 6187
	public Color _To = Color.white;

	// Token: 0x0400182C RID: 6188
	public AnimationCurve _AnimationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	// Token: 0x0400182D RID: 6189
	public float _Duration = 1f;

	// Token: 0x0400182E RID: 6190
	[SerializeField]
	private uGUITweenMeshColor.PlayType _playType = uGUITweenMeshColor.PlayType._PingPong;

	// Token: 0x0400182F RID: 6191
	private bool _ReverceTrigger;

	// Token: 0x04001830 RID: 6192
	private float _AnimCurvepos;

	// Token: 0x04001831 RID: 6193
	private Mesh _MeshSelf;

	// Token: 0x04001832 RID: 6194
	private SpriteRenderer _SpriteRendere;

	// Token: 0x04001833 RID: 6195
	private bool _BoolMesh;

	// Token: 0x04001834 RID: 6196
	private bool _BoolSR;

	// Token: 0x04001835 RID: 6197
	private float _Time;

	// Token: 0x02001046 RID: 4166
	public enum PlayType
	{
		// Token: 0x04005B4F RID: 23375
		_Default,
		// Token: 0x04005B50 RID: 23376
		_PingPong,
		// Token: 0x04005B51 RID: 23377
		_Loop
	}
}
