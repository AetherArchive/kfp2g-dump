using System;
using UnityEngine;

// Token: 0x020000CD RID: 205
[AddComponentMenu("Image Effects/ParallelClip")]
[RequireComponent(typeof(Camera))]
public class ParallelClip : MonoBehaviour
{
	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x0600091D RID: 2333 RVA: 0x00039D7D File Offset: 0x00037F7D
	protected Material clipMaterial
	{
		get
		{
			if (this.m_clipMaterial == null)
			{
				this.m_clipMaterial = new Material(this.clipShader);
				this.m_clipMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_clipMaterial;
		}
	}

	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x0600091E RID: 2334 RVA: 0x00039DB1 File Offset: 0x00037FB1
	// (set) Token: 0x0600091F RID: 2335 RVA: 0x00039DB9 File Offset: 0x00037FB9
	public float clipY { get; set; }

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x06000920 RID: 2336 RVA: 0x00039DC2 File Offset: 0x00037FC2
	// (set) Token: 0x06000921 RID: 2337 RVA: 0x00039DCA File Offset: 0x00037FCA
	public float clipX { get; set; }

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x06000922 RID: 2338 RVA: 0x00039DD3 File Offset: 0x00037FD3
	// (set) Token: 0x06000923 RID: 2339 RVA: 0x00039DDB File Offset: 0x00037FDB
	public float offU { get; set; }

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x06000924 RID: 2340 RVA: 0x00039DE4 File Offset: 0x00037FE4
	// (set) Token: 0x06000925 RID: 2341 RVA: 0x00039DEC File Offset: 0x00037FEC
	public float offD { get; set; }

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x06000926 RID: 2342 RVA: 0x00039DF5 File Offset: 0x00037FF5
	// (set) Token: 0x06000927 RID: 2343 RVA: 0x00039DFD File Offset: 0x00037FFD
	public float offL { get; set; }

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x06000928 RID: 2344 RVA: 0x00039E06 File Offset: 0x00038006
	// (set) Token: 0x06000929 RID: 2345 RVA: 0x00039E0E File Offset: 0x0003800E
	public float offR { get; set; }

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x0600092A RID: 2346 RVA: 0x00039E17 File Offset: 0x00038017
	// (set) Token: 0x0600092B RID: 2347 RVA: 0x00039E1F File Offset: 0x0003801F
	public float alpha { get; set; }

	// Token: 0x0600092C RID: 2348 RVA: 0x00039E28 File Offset: 0x00038028
	public void Awake()
	{
		this.mainCamera = base.GetComponent<Camera>();
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x00039E38 File Offset: 0x00038038
	public void OnEnable()
	{
		this.clipCamera = new GameObject("Parallel Clip Camera", new Type[] { typeof(Camera) }).GetComponent<Camera>();
		this.clipCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;
		this.clipCamera.enabled = false;
		this.cullingMask = this.mainCamera.cullingMask;
		this.mainCamera.cullingMask = 0;
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x00039EA8 File Offset: 0x000380A8
	public void OnDisable()
	{
		this.mainCamera.cullingMask = this.cullingMask;
		this.clipMaterial.mainTexture = null;
		RenderTexture.active = null;
		if (null != this.clipCamera)
		{
			Object.DestroyImmediate(this.clipCamera.gameObject);
			this.clipCamera = null;
		}
		if (this.m_clipMaterial)
		{
			Object.DestroyImmediate(this.m_clipMaterial);
			this.m_clipMaterial = null;
		}
		this.DeleteClipTexture();
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x00039F24 File Offset: 0x00038124
	private void CreateClipTexture(RenderTexture source)
	{
		if (source.width < 1 || source.height < 1)
		{
			return;
		}
		if (null != this.clipRT && (this.clipRT.width != source.width || this.clipRT.height != source.height || this.clipRT.antiAliasing != source.antiAliasing))
		{
			this.DeleteClipTexture();
		}
		if (null == this.clipRT)
		{
			this.clipRT = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.ARGB32);
			this.clipRT.wrapMode = TextureWrapMode.Clamp;
			this.clipRT.useMipMap = false;
			this.clipRT.filterMode = FilterMode.Bilinear;
			this.clipRT.hideFlags = HideFlags.HideAndDontSave;
			this.clipRT.antiAliasing = source.antiAliasing;
			this.clipRT.Create();
		}
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x0003A006 File Offset: 0x00038206
	private void DeleteClipTexture()
	{
		if (null != this.clipRT)
		{
			this.clipRT.Release();
			Object.DestroyImmediate(this.clipRT);
			this.clipRT = null;
		}
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x0003A034 File Offset: 0x00038234
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.CreateClipTexture(source);
		if (null != this.clipRT)
		{
			this.clipCamera.CopyFrom(this.mainCamera);
			this.clipCamera.enabled = true;
			this.clipCamera.rect = new Rect(0f, 0f, 1f, 1f);
			this.clipCamera.targetTexture = this.clipRT;
			this.clipCamera.clearFlags = CameraClearFlags.Color;
			this.clipCamera.backgroundColor = Color.clear;
			this.clipCamera.cullingMask = 0;
			this.clipCamera.Render();
			this.clipCamera.SetTargetBuffers(this.clipRT.colorBuffer, source.depthBuffer);
			this.clipCamera.clearFlags = CameraClearFlags.Nothing;
			this.clipCamera.cullingMask = this.cullingMask;
			this.clipCamera.Render();
		}
		this.clipCamera.enabled = false;
		this.clipMaterial.SetTexture("_ClipTex", this.clipRT);
		this.clipMaterial.SetFloat("_ClipY", this.clipY);
		this.clipMaterial.SetFloat("_ClipX", this.clipX);
		this.clipMaterial.SetFloat("_OffU", this.offU);
		this.clipMaterial.SetFloat("_OffD", this.offD);
		this.clipMaterial.SetFloat("_OffL", this.offL);
		this.clipMaterial.SetFloat("_OffR", this.offR);
		this.clipMaterial.SetFloat("_Alpha", this.alpha);
		Graphics.Blit(source, destination, this.clipMaterial);
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x0003A1EB File Offset: 0x000383EB
	private void Update()
	{
		if (this.mainCamera.cullingMask != 0)
		{
			this.cullingMask = this.mainCamera.cullingMask;
			this.mainCamera.cullingMask = 0;
		}
	}

	// Token: 0x04000776 RID: 1910
	[HideInInspector]
	public Shader clipShader;

	// Token: 0x04000777 RID: 1911
	private Material m_clipMaterial;

	// Token: 0x04000778 RID: 1912
	private Camera mainCamera;

	// Token: 0x04000779 RID: 1913
	private Camera clipCamera;

	// Token: 0x0400077A RID: 1914
	private RenderTexture clipRT;

	// Token: 0x0400077B RID: 1915
	private int cullingMask;
}
