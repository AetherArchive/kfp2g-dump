using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x0200002A RID: 42
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Bloom and Glow/MultipleGaussianBloom")]
public class MultipleGaussianBloom : MonoBehaviour
{
	// Token: 0x17000006 RID: 6
	// (get) Token: 0x0600006B RID: 107 RVA: 0x00003E70 File Offset: 0x00002070
	protected Material bloomMaterial
	{
		get
		{
			if (this.m_bloomMaterial == null)
			{
				this.m_bloomMaterial = new Material(this.bloomShader);
				this.m_bloomMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_bloomMaterial;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x0600006C RID: 108 RVA: 0x00003EA4 File Offset: 0x000020A4
	// (set) Token: 0x0600006D RID: 109 RVA: 0x00003EAC File Offset: 0x000020AC
	public bool camouflageEnable { get; set; }

	// Token: 0x0600006E RID: 110 RVA: 0x00003EB8 File Offset: 0x000020B8
	public void Awake()
	{
		this.mainCamera = base.GetComponent<Camera>();
		if (this.replacementShader == null)
		{
			this.replacementShader = Shader.Find("Hidden/ForReplacement");
		}
		if (this.bloomShader == null)
		{
			this.bloomShader = Shader.Find("Hidden/MultipleGaussianBloom");
		}
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00003F0D File Offset: 0x0000210D
	public void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00003F20 File Offset: 0x00002120
	public void OnEnable()
	{
		this.subCamera = new GameObject("Glow Effect2", new Type[] { typeof(Camera) }).GetComponent<Camera>();
		this.subCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;
		this.subCamera.enabled = false;
		this.alphaCamera = new GameObject("Field Alpha Camera", new Type[] { typeof(Camera) }).GetComponent<Camera>();
		this.alphaCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;
		this.alphaCamera.enabled = false;
		this.camouflageCamera = new GameObject("Camouflage Camera", new Type[] { typeof(Camera) }).GetComponent<Camera>();
		this.camouflageCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;
		this.camouflageCamera.enabled = false;
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00004000 File Offset: 0x00002200
	public void OnDisable()
	{
		this.bloomMaterial.mainTexture = null;
		RenderTexture.active = null;
		if (null != this.subCamera)
		{
			Object.DestroyImmediate(this.subCamera.gameObject);
			this.subCamera = null;
		}
		if (this.m_bloomMaterial)
		{
			Object.DestroyImmediate(this.m_bloomMaterial);
			this.m_bloomMaterial = null;
		}
		this.DeleteRenderTexture();
		if (null != this.alphaCamera)
		{
			Object.DestroyImmediate(this.alphaCamera.gameObject);
			this.alphaCamera = null;
		}
		this.DeleteAlphaTexture();
		if (null != this.camouflageCamera)
		{
			Object.DestroyImmediate(this.camouflageCamera.gameObject);
			this.camouflageCamera = null;
		}
		this.DeleteCamouflageTexture();
	}

	// Token: 0x06000072 RID: 114 RVA: 0x000040C0 File Offset: 0x000022C0
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.CreateAlphaTexture(source);
		if (this.mainCamera.targetTexture != null)
		{
			Graphics.Blit(source, this.mainCamera.targetTexture);
		}
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
			foreach (string text in MultipleGaussianBloom.layerList)
			{
				if ((this.mainCamera.cullingMask & (1 << LayerMask.NameToLayer(text))) != 0)
				{
					this.alphaCamera.cullingMask |= 1 << LayerMask.NameToLayer(text + "Alpha");
				}
			}
			this.alphaCamera.Render();
		}
		this.alphaCamera.enabled = false;
		this.bloomMaterial.SetTexture("_AlphaTex", this.alphaRT);
		this.CreateCamouflageTexture(source);
		if (null != this.camouflageRT)
		{
			this.camouflageCamera.CopyFrom(this.mainCamera);
			this.camouflageCamera.enabled = true;
			this.camouflageCamera.rect = new Rect(0f, 0f, 1f, 1f);
			this.camouflageCamera.targetTexture = this.camouflageRT;
			this.camouflageCamera.clearFlags = CameraClearFlags.Color;
			this.camouflageCamera.backgroundColor = Color.clear;
			this.camouflageCamera.cullingMask = 0;
			this.camouflageCamera.Render();
			if (this.camouflageEnable)
			{
				this.camouflageCamera.SetTargetBuffers(this.camouflageRT.colorBuffer, source.depthBuffer);
				this.camouflageCamera.clearFlags = CameraClearFlags.Nothing;
				this.camouflageCamera.cullingMask = 1 << LayerMask.NameToLayer("Camouflage");
				this.camouflageCamera.Render();
			}
		}
		this.camouflageCamera.enabled = false;
		this.bloomMaterial.SetTexture("_CamouflageTex", this.camouflageRT);
		this.CreateRenderTexture(source);
		if (null != this.extractRT)
		{
			this.subCamera.CopyFrom(this.mainCamera);
			this.subCamera.renderingPath = RenderingPath.Forward;
			this.subCamera.enabled = true;
			this.subCamera.rect = new Rect(0f, 0f, 1f, 1f);
			this.subCamera.targetTexture = this.extractRT;
			this.subCamera.cullingMask = 0;
			this.subCamera.backgroundColor = Color.black;
			this.subCamera.clearFlags = CameraClearFlags.Color;
			this.subCamera.Render();
			this.subCamera.SetTargetBuffers(this.extractRT.colorBuffer, source.depthBuffer);
			if (base.transform.gameObject.name == "Result Camera" || base.transform.parent.name == "RenderTexture Base" || (base.transform.parent.name == "FieldSceneBattle(Clone)" && Regex.IsMatch(base.transform.gameObject.name, "^Camera[0-9]$")) || (base.transform.parent.name == "FieldSceneTreeHouse(Clone)" && (base.transform.gameObject.name == "CamL" || base.transform.gameObject.name == "CamR")) || base.transform.gameObject.name == "ScenarioCamera")
			{
				int num = 0;
				if ((this.mainCamera.cullingMask & (1 << LayerMask.NameToLayer("Bloom"))) != 0)
				{
					num |= 1 << LayerMask.NameToLayer("Bloom");
				}
				if ((this.mainCamera.cullingMask & (1 << LayerMask.NameToLayer("Bloom2"))) != 0)
				{
					num |= 1 << LayerMask.NameToLayer("Bloom2");
				}
				this.subCamera.cullingMask = num;
			}
			else
			{
				this.subCamera.cullingMask = this.mainCamera.cullingMask;
			}
			this.subCamera.clearFlags = CameraClearFlags.Nothing;
			this.subCamera.RenderWithShader(this.replacementShader, this.replacementTag);
			this.EnableKeywords();
			this.KawsseBloomEffect(source, destination);
		}
		this.subCamera.enabled = false;
	}

	// Token: 0x06000073 RID: 115 RVA: 0x000045F0 File Offset: 0x000027F0
	private void CreateRenderTexture(RenderTexture source)
	{
		if (source.width < 1 || source.height < 1)
		{
			return;
		}
		if (this.UpdateInternalFormat(this.renderingFormat))
		{
			this.DeleteRenderTexture();
		}
		if (null != this.extractRT && (this.extractRT.width != source.width || this.extractRT.height != source.height || this.extractRT.antiAliasing != source.antiAliasing))
		{
			this.DeleteRenderTexture();
		}
		if (null == this.extractRT)
		{
			this.extractRT = new RenderTexture(source.width, source.height, 0, this.internalFormat);
			this.extractRT.wrapMode = TextureWrapMode.Clamp;
			this.extractRT.useMipMap = false;
			this.extractRT.filterMode = FilterMode.Bilinear;
			this.extractRT.hideFlags = HideFlags.HideAndDontSave;
			this.extractRT.antiAliasing = source.antiAliasing;
			this.extractRT.Create();
		}
	}

	// Token: 0x06000074 RID: 116 RVA: 0x000046EB File Offset: 0x000028EB
	private void DeleteRenderTexture()
	{
		if (null != this.extractRT)
		{
			this.extractRT.Release();
			Object.DestroyImmediate(this.extractRT);
			this.extractRT = null;
		}
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00004718 File Offset: 0x00002918
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

	// Token: 0x06000076 RID: 118 RVA: 0x000047FA File Offset: 0x000029FA
	private void DeleteAlphaTexture()
	{
		if (null != this.alphaRT)
		{
			this.alphaRT.Release();
			Object.DestroyImmediate(this.alphaRT);
			this.alphaRT = null;
		}
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00004828 File Offset: 0x00002A28
	private void CreateCamouflageTexture(RenderTexture source)
	{
		if (source.width < 1 || source.height < 1)
		{
			return;
		}
		if (null != this.camouflageRT && (this.camouflageRT.width != source.width || this.camouflageRT.height != source.height || this.camouflageRT.antiAliasing != source.antiAliasing))
		{
			this.DeleteCamouflageTexture();
		}
		if (null == this.camouflageRT)
		{
			this.camouflageRT = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.ARGB32);
			this.camouflageRT.wrapMode = TextureWrapMode.Clamp;
			this.camouflageRT.useMipMap = false;
			this.camouflageRT.filterMode = FilterMode.Bilinear;
			this.camouflageRT.hideFlags = HideFlags.HideAndDontSave;
			this.camouflageRT.antiAliasing = source.antiAliasing;
			this.camouflageRT.Create();
		}
	}

	// Token: 0x06000078 RID: 120 RVA: 0x0000490A File Offset: 0x00002B0A
	private void DeleteCamouflageTexture()
	{
		if (null != this.camouflageRT)
		{
			this.camouflageRT.Release();
			Object.DestroyImmediate(this.camouflageRT);
			this.camouflageRT = null;
		}
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00004937 File Offset: 0x00002B37
	private bool UpdateInternalFormat(MultipleGaussianBloom.RTFormat in_format)
	{
		if (this.renderingFormatPrev == (int)in_format)
		{
			return false;
		}
		this.renderingFormatPrev = (int)in_format;
		switch (in_format)
		{
		default:
			this.internalFormat = RenderTextureFormat.RGB565;
			break;
		case MultipleGaussianBloom.RTFormat._32bpp:
			this.internalFormat = RenderTextureFormat.ARGB32;
			break;
		case MultipleGaussianBloom.RTFormat._64bppHDR:
			this.internalFormat = RenderTextureFormat.ARGBHalf;
			break;
		}
		return true;
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00004978 File Offset: 0x00002B78
	private void KawsseBloomEffect(RenderTexture source, RenderTexture destination)
	{
		int num = this.extractRT.width;
		int num2 = this.extractRT.height;
		while (num > 700 || num2 > 370)
		{
			num /= 2;
			num2 /= 2;
		}
		int num3 = 4;
		RenderTexture[] array = new RenderTexture[num3];
		for (int i = 0; i < num3; i++)
		{
			array[i] = RenderTexture.GetTemporary(num, num2, 0, this.internalFormat, RenderTextureReadWrite.Default, this.extractRT.antiAliasing);
			array[i].filterMode = FilterMode.Bilinear;
			num /= 2;
			num2 /= 2;
			if (num <= 1 || num2 <= 1)
			{
				num3 = i + 1;
				break;
			}
		}
		float[] array2 = new float[num3];
		for (int j = 0; j < num3; j++)
		{
			float num4 = 2f * (float)j / (float)(num3 - 1) - 1f;
			array2[j] = 1f - 2f * (num4 * this.bloomDistribution);
			if (array2[j] < 0f)
			{
				array2[j] = 0f;
			}
			else if (array2[j] > 2f)
			{
				array2[j] = 2f;
			}
			array2[j] *= this.bloomIntensity;
		}
		RenderTexture renderTexture = this.extractRT;
		for (int k = 0; k < num3; k++)
		{
			Graphics.Blit(renderTexture, array[k]);
			renderTexture = array[k];
		}
		for (int l = 0; l < num3; l++)
		{
			this.GaussianFilter(array[l]);
		}
		this.bloomMaterial.SetFloat("_SrcIntensity", array2[num3 - 1]);
		for (int m = num3 - 2; m >= 0; m--)
		{
			this.bloomMaterial.SetFloat("_DstIntensity", array2[m]);
			Graphics.Blit(array[m + 1], array[m], this.bloomMaterial, 1);
			this.bloomMaterial.SetFloat("_SrcIntensity", 1f);
		}
		this.bloomMaterial.SetTexture("_BloomTex", array[0]);
		Graphics.Blit(source, destination, this.bloomMaterial, 2);
		for (int n = 0; n < num3; n++)
		{
			RenderTexture.ReleaseTemporary(array[n]);
		}
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00004B80 File Offset: 0x00002D80
	private void GaussianFilter(RenderTexture blurTarget)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(blurTarget.width, blurTarget.height, 0, blurTarget.format, RenderTextureReadWrite.Default, blurTarget.antiAliasing);
		temporary.filterMode = FilterMode.Bilinear;
		this.bloomMaterial.SetVector("_SamplingDir", new Vector4(1f, 0f, 0f, 0f));
		Graphics.Blit(blurTarget, temporary, this.bloomMaterial, 0);
		this.bloomMaterial.SetVector("_SamplingDir", new Vector4(0f, 1f, 0f, 0f));
		Graphics.Blit(temporary, blurTarget, this.bloomMaterial, 0);
		RenderTexture.ReleaseTemporary(temporary);
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00004C28 File Offset: 0x00002E28
	private void EnableKeywords()
	{
		if (this.gaussianFilterPrev != (int)this.gaussianFilter)
		{
			this.DisableKeywords();
			this.gaussianFilterPrev = (int)this.gaussianFilter;
			switch (this.gaussianFilter)
			{
			case MultipleGaussianBloom.FilterTaps._3Taps:
				this.bloomMaterial.EnableKeyword("GAUSSFLT_3TAPS");
				return;
			case MultipleGaussianBloom.FilterTaps._5Taps:
				this.bloomMaterial.EnableKeyword("GAUSSFLT_5TAPS");
				return;
			case MultipleGaussianBloom.FilterTaps._7Taps:
				this.bloomMaterial.EnableKeyword("GAUSSFLT_7TAPS");
				return;
			case MultipleGaussianBloom.FilterTaps._9Taps:
				this.bloomMaterial.EnableKeyword("GAUSSFLT_9TAPS");
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00004CB8 File Offset: 0x00002EB8
	private void DisableKeywords()
	{
		if (this.gaussianFilterPrev != (int)this.gaussianFilter)
		{
			this.bloomMaterial.DisableKeyword("GAUSSFLT_3TAPS");
			this.bloomMaterial.DisableKeyword("GAUSSFLT_5TAPS");
			this.bloomMaterial.DisableKeyword("GAUSSFLT_7TAPS");
			this.bloomMaterial.DisableKeyword("GAUSSFLT_9TAPS");
		}
	}

	// Token: 0x040000F5 RID: 245
	private const int numDownsampleImages = 4;

	// Token: 0x040000F6 RID: 246
	public Shader replacementShader;

	// Token: 0x040000F7 RID: 247
	public string replacementTag = "BloomType";

	// Token: 0x040000F8 RID: 248
	public float bloomIntensity = 1f;

	// Token: 0x040000F9 RID: 249
	[Range(-1f, 1f)]
	public float bloomDistribution;

	// Token: 0x040000FA RID: 250
	public MultipleGaussianBloom.FilterTaps gaussianFilter = MultipleGaussianBloom.FilterTaps._7Taps;

	// Token: 0x040000FB RID: 251
	public MultipleGaussianBloom.RTFormat renderingFormat = MultipleGaussianBloom.RTFormat._32bpp;

	// Token: 0x040000FC RID: 252
	private RenderTextureFormat internalFormat;

	// Token: 0x040000FD RID: 253
	[HideInInspector]
	public Shader bloomShader;

	// Token: 0x040000FE RID: 254
	private Material m_bloomMaterial;

	// Token: 0x040000FF RID: 255
	private Camera mainCamera;

	// Token: 0x04000100 RID: 256
	private Camera subCamera;

	// Token: 0x04000101 RID: 257
	private RenderTexture extractRT;

	// Token: 0x04000102 RID: 258
	private int gaussianFilterPrev = -1;

	// Token: 0x04000103 RID: 259
	private int renderingFormatPrev = -1;

	// Token: 0x04000104 RID: 260
	private Camera alphaCamera;

	// Token: 0x04000105 RID: 261
	private RenderTexture alphaRT;

	// Token: 0x04000106 RID: 262
	public static readonly List<string> layerList = new List<string> { "AuthMain", "FieldStage", "FieldPlayer", "FieldEnemy" };

	// Token: 0x04000107 RID: 263
	private Camera camouflageCamera;

	// Token: 0x04000108 RID: 264
	private RenderTexture camouflageRT;

	// Token: 0x0200058F RID: 1423
	public enum FilterTaps
	{
		// Token: 0x04002940 RID: 10560
		_3Taps,
		// Token: 0x04002941 RID: 10561
		_5Taps,
		// Token: 0x04002942 RID: 10562
		_7Taps,
		// Token: 0x04002943 RID: 10563
		_9Taps
	}

	// Token: 0x02000590 RID: 1424
	public enum RTFormat
	{
		// Token: 0x04002945 RID: 10565
		_16bpp,
		// Token: 0x04002946 RID: 10566
		_32bpp,
		// Token: 0x04002947 RID: 10567
		_64bppHDR
	}
}
