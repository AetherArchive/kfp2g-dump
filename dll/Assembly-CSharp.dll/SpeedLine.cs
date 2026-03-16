using System;
using UnityEngine;

[AddComponentMenu("Image Effects/SpeedLine")]
[RequireComponent(typeof(Camera))]
public class SpeedLine : MonoBehaviour
{
	protected Material lineMaterial
	{
		get
		{
			if (this.m_lineMaterial == null)
			{
				this.m_lineMaterial = new Material(this.lineShader);
				this.m_lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_lineMaterial;
		}
	}

	public void Awake()
	{
	}

	private void Start()
	{
	}

	public void OnEnable()
	{
		this.pos = 0f;
		this.rot = 0f;
		this.sin = 0f;
		this.cos = 1f;
		this.rct = new Vector4(0f, 0f, 1f, 1f);
	}

	public void OnDisable()
	{
		this.lineMaterial.mainTexture = null;
		if (this.m_lineMaterial)
		{
			Object.DestroyImmediate(this.m_lineMaterial);
			this.m_lineMaterial = null;
		}
	}

	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.lineMaterial.SetTexture("_LineTex", this.lineTexture);
		this.lineMaterial.SetFloat("_Pos", this.pos);
		this.lineMaterial.SetFloat("_Sin", this.sin);
		this.lineMaterial.SetFloat("_Cos", this.cos);
		this.lineMaterial.SetColor("_Color", this.lineColor);
		this.lineMaterial.SetVector("_Rect", this.rct);
		Graphics.Blit(source, destination, this.lineMaterial);
	}

	private void Update()
	{
		this.rot = 0.017453292f * (360f - this.lineAngle);
		this.sin = Mathf.Sin(this.rot);
		this.cos = Mathf.Cos(this.rot);
		Vector2 vector = new Vector2(this.cos, this.sin * 1.7778f);
		float num = this.lineSpeed * vector.magnitude;
		if ((this.pos += Time.deltaTime * num) >= 1f)
		{
			this.pos -= 1f;
		}
		else if (this.pos < 0f)
		{
			this.pos += 1f;
		}
		FieldCameraScaler component = base.GetComponent<FieldCameraScaler>();
		if (component == null)
		{
			this.rct = new Vector4(0f, 0f, 1f, 1f);
			return;
		}
		this.rct = component.GetRect();
	}

	[HideInInspector]
	public Shader lineShader;

	[HideInInspector]
	public Texture lineTexture;

	public float lineAngle;

	public float lineSpeed;

	public Color lineColor = Color.white;

	private Material m_lineMaterial;

	private float pos;

	private float rot;

	private float sin;

	private float cos;

	private Vector4 rct;
}
