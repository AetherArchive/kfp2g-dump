using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C9 RID: 201
[AddComponentMenu("Image Effects/FieldAlpha")]
[RequireComponent(typeof(Camera))]
public class FieldAlpha : MonoBehaviour
{
	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x06000901 RID: 2305 RVA: 0x0003917E File Offset: 0x0003737E
	protected Material alphaMaterial
	{
		get
		{
			if (this.m_alphaMaterial == null)
			{
				this.m_alphaMaterial = new Material(this.alphaShader);
				this.m_alphaMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_alphaMaterial;
		}
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x000391B2 File Offset: 0x000373B2
	public void Awake()
	{
		this.mainCamera = base.GetComponent<Camera>();
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x000391C0 File Offset: 0x000373C0
	public void OnEnable()
	{
		this.alphaCamera = new GameObject("Field Alpha Camera", new Type[] { typeof(Camera) }).GetComponent<Camera>();
		this.alphaCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;
		this.alphaCamera.enabled = false;
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x00039214 File Offset: 0x00037414
	public void OnDisable()
	{
		this.alphaMaterial.mainTexture = null;
		RenderTexture.active = null;
		if (null != this.alphaCamera)
		{
			Object.DestroyImmediate(this.alphaCamera.gameObject);
			this.alphaCamera = null;
		}
		if (this.m_alphaMaterial)
		{
			Object.DestroyImmediate(this.m_alphaMaterial);
			this.m_alphaMaterial = null;
		}
		this.DeleteAlphaTexture();
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x00039280 File Offset: 0x00037480
	private void CreateAlphaTexture(RenderTexture source)
	{
		if (source.width < 1 || source.height < 1)
		{
			return;
		}
		if (null != this.alphaRT && (this.alphaRT.width != source.width || this.alphaRT.height != source.height || this.alphaRT.antiAliasing != source.antiAliasing))
		{
			this.DeleteAlphaTexture();
		}
		if (null == this.alphaRT)
		{
			this.alphaRT = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.ARGB32);
			this.alphaRT.wrapMode = TextureWrapMode.Clamp;
			this.alphaRT.useMipMap = false;
			this.alphaRT.filterMode = FilterMode.Bilinear;
			this.alphaRT.hideFlags = HideFlags.HideAndDontSave;
			this.alphaRT.antiAliasing = source.antiAliasing;
			this.alphaRT.Create();
		}
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x00039362 File Offset: 0x00037562
	private void DeleteAlphaTexture()
	{
		if (null != this.alphaRT)
		{
			this.alphaRT.Release();
			Object.DestroyImmediate(this.alphaRT);
			this.alphaRT = null;
		}
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x00039390 File Offset: 0x00037590
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.CreateAlphaTexture(source);
		if (null != this.alphaRT)
		{
			this.alphaCamera.CopyFrom(this.mainCamera);
			this.alphaCamera.enabled = true;
			this.alphaCamera.rect = new Rect(0f, 0f, 1f, 1f);
			this.alphaCamera.targetTexture = this.alphaRT;
			this.alphaCamera.clearFlags = CameraClearFlags.Color;
			this.alphaCamera.backgroundColor = Color.clear;
			this.alphaCamera.cullingMask = 0;
			this.alphaCamera.Render();
			this.alphaCamera.SetTargetBuffers(this.alphaRT.colorBuffer, source.depthBuffer);
			this.alphaCamera.clearFlags = CameraClearFlags.Nothing;
			this.alphaCamera.cullingMask = 0;
			foreach (string text in FieldAlpha.layerList)
			{
				if ((this.mainCamera.cullingMask & (1 << LayerMask.NameToLayer(text))) != 0)
				{
					this.alphaCamera.cullingMask |= 1 << LayerMask.NameToLayer(text + "Alpha");
				}
			}
			this.alphaCamera.Render();
		}
		this.alphaCamera.enabled = false;
		this.alphaMaterial.SetTexture("_AlphaTex", this.alphaRT);
		Graphics.Blit(source, destination, this.alphaMaterial);
	}

	// Token: 0x04000764 RID: 1892
	[HideInInspector]
	public Shader alphaShader;

	// Token: 0x04000765 RID: 1893
	private Material m_alphaMaterial;

	// Token: 0x04000766 RID: 1894
	private Camera mainCamera;

	// Token: 0x04000767 RID: 1895
	private Camera alphaCamera;

	// Token: 0x04000768 RID: 1896
	private RenderTexture alphaRT;

	// Token: 0x04000769 RID: 1897
	public static readonly List<string> layerList = new List<string> { "AuthMain", "FieldStage", "FieldPlayer", "FieldEnemy" };
}
