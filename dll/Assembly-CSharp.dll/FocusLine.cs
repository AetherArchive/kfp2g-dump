using System;
using UnityEngine;

// Token: 0x020000CA RID: 202
[AddComponentMenu("Image Effects/FocusLine")]
[RequireComponent(typeof(Camera))]
public class FocusLine : MonoBehaviour
{
	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x0600090A RID: 2314 RVA: 0x00039568 File Offset: 0x00037768
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

	// Token: 0x0600090B RID: 2315 RVA: 0x0003959C File Offset: 0x0003779C
	public void Awake()
	{
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0003959E File Offset: 0x0003779E
	private void Start()
	{
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x000395A0 File Offset: 0x000377A0
	public void OnEnable()
	{
		this.fade = 0f;
		this.rct = new Vector4(0f, 0f, 1f, 1f);
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x000395CC File Offset: 0x000377CC
	public void OnDisable()
	{
		this.lineMaterial.mainTexture = null;
		if (this.m_lineMaterial)
		{
			Object.DestroyImmediate(this.m_lineMaterial);
			this.m_lineMaterial = null;
		}
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x000395FC File Offset: 0x000377FC
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

	// Token: 0x06000910 RID: 2320 RVA: 0x00039734 File Offset: 0x00037934
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

	// Token: 0x0400076A RID: 1898
	[HideInInspector]
	public Shader lineShader;

	// Token: 0x0400076B RID: 1899
	[HideInInspector]
	public Texture lineTexture;

	// Token: 0x0400076C RID: 1900
	public Vector2 focusPos = new Vector2(0.5f, 0.5f);

	// Token: 0x0400076D RID: 1901
	public Color lineColor = Color.white;

	// Token: 0x0400076E RID: 1902
	private Material m_lineMaterial;

	// Token: 0x0400076F RID: 1903
	private float fade;

	// Token: 0x04000770 RID: 1904
	private Vector4 rct;
}
