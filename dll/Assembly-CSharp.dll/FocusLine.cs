using System;
using UnityEngine;

[AddComponentMenu("Image Effects/FocusLine")]
[RequireComponent(typeof(Camera))]
public class FocusLine : MonoBehaviour
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
		this.fade = 0f;
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
		this.lineMaterial.SetFloat("_PosX", this.focusPos.x);
		this.lineMaterial.SetFloat("_PosY", this.focusPos.y);
		float num = Mathf.Abs(this.focusPos.x - 0.5f) + 0.5f;
		float num2 = Mathf.Abs(this.focusPos.y - 0.5f) + 0.5f;
		this.lineMaterial.SetFloat("_Scl", this.fade / Mathf.Sqrt(num * num * 4f + num2 * num2 * 4f));
		float num3 = Random.Range(0f, 6.2831855f);
		this.lineMaterial.SetFloat("_Sin", Mathf.Sin(num3));
		this.lineMaterial.SetFloat("_Cos", Mathf.Cos(num3));
		this.lineMaterial.SetColor("_Color", this.lineColor);
		this.lineMaterial.SetVector("_Rect", this.rct);
		Graphics.Blit(source, destination, this.lineMaterial);
	}

	private void Update()
	{
		if ((this.fade += Time.deltaTime * 10f) > 1f)
		{
			this.fade = 1f;
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

	public Vector2 focusPos = new Vector2(0.5f, 0.5f);

	public Color lineColor = Color.white;

	private Material m_lineMaterial;

	private float fade;

	private Vector4 rct;
}
