using System;
using UnityEngine;

// Token: 0x020000CE RID: 206
[AddComponentMenu("Image Effects/SpeedLine")]
[RequireComponent(typeof(Camera))]
public class SpeedLine : MonoBehaviour
{
	// Token: 0x170001DA RID: 474
	// (get) Token: 0x06000934 RID: 2356 RVA: 0x0003A21F File Offset: 0x0003841F
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

	// Token: 0x06000935 RID: 2357 RVA: 0x0003A253 File Offset: 0x00038453
	public void Awake()
	{
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x0003A255 File Offset: 0x00038455
	private void Start()
	{
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x0003A258 File Offset: 0x00038458
	public void OnEnable()
	{
		this.pos = 0f;
		this.rot = 0f;
		this.sin = 0f;
		this.cos = 1f;
		this.rct = new Vector4(0f, 0f, 1f, 1f);
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x0003A2B0 File Offset: 0x000384B0
	public void OnDisable()
	{
		this.lineMaterial.mainTexture = null;
		if (this.m_lineMaterial)
		{
			Object.DestroyImmediate(this.m_lineMaterial);
			this.m_lineMaterial = null;
		}
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x0003A2E0 File Offset: 0x000384E0
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

	// Token: 0x0600093A RID: 2362 RVA: 0x0003A380 File Offset: 0x00038580
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

	// Token: 0x04000783 RID: 1923
	[HideInInspector]
	public Shader lineShader;

	// Token: 0x04000784 RID: 1924
	[HideInInspector]
	public Texture lineTexture;

	// Token: 0x04000785 RID: 1925
	public float lineAngle;

	// Token: 0x04000786 RID: 1926
	public float lineSpeed;

	// Token: 0x04000787 RID: 1927
	public Color lineColor = Color.white;

	// Token: 0x04000788 RID: 1928
	private Material m_lineMaterial;

	// Token: 0x04000789 RID: 1929
	private float pos;

	// Token: 0x0400078A RID: 1930
	private float rot;

	// Token: 0x0400078B RID: 1931
	private float sin;

	// Token: 0x0400078C RID: 1932
	private float cos;

	// Token: 0x0400078D RID: 1933
	private Vector4 rct;
}
