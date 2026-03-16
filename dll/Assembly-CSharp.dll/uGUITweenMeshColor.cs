using System;
using UnityEngine;

public class uGUITweenMeshColor : MonoBehaviour
{
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

	public Color _From = Color.white;

	public Color _To = Color.white;

	public AnimationCurve _AnimationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	public float _Duration = 1f;

	[SerializeField]
	private uGUITweenMeshColor.PlayType _playType = uGUITweenMeshColor.PlayType._PingPong;

	private bool _ReverceTrigger;

	private float _AnimCurvepos;

	private Mesh _MeshSelf;

	private SpriteRenderer _SpriteRendere;

	private bool _BoolMesh;

	private bool _BoolSR;

	private float _Time;

	public enum PlayType
	{
		_Default,
		_PingPong,
		_Loop
	}
}
