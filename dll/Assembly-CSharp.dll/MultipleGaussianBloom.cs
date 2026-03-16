using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Bloom and Glow/MultipleGaussianBloom")]
public class MultipleGaussianBloom : MonoBehaviour
{
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

	public bool camouflageEnable { get; set; }

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

	public void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
	}

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

	private void DeleteRenderTexture()
	{
		if (null != this.extractRT)
		{
			this.extractRT.Release();
			Object.DestroyImmediate(this.extractRT);
			this.extractRT = null;
		}
	}

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

	private void DeleteAlphaTexture()
	{
		if (null != this.alphaRT)
		{
			this.alphaRT.Release();
			Object.DestroyImmediate(this.alphaRT);
			this.alphaRT = null;
		}
	}

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

	private void DeleteCamouflageTexture()
	{
		if (null != this.camouflageRT)
		{
			this.camouflageRT.Release();
			Object.DestroyImmediate(this.camouflageRT);
			this.camouflageRT = null;
		}
	}

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

	private const int numDownsampleImages = 4;

	public Shader replacementShader;

	public string replacementTag = "BloomType";

	public float bloomIntensity = 1f;

	[Range(-1f, 1f)]
	public float bloomDistribution;

	public MultipleGaussianBloom.FilterTaps gaussianFilter = MultipleGaussianBloom.FilterTaps._7Taps;

	public MultipleGaussianBloom.RTFormat renderingFormat = MultipleGaussianBloom.RTFormat._32bpp;

	private RenderTextureFormat internalFormat;

	[HideInInspector]
	public Shader bloomShader;

	private Material m_bloomMaterial;

	private Camera mainCamera;

	private Camera subCamera;

	private RenderTexture extractRT;

	private int gaussianFilterPrev = -1;

	private int renderingFormatPrev = -1;

	private Camera alphaCamera;

	private RenderTexture alphaRT;

	public static readonly List<string> layerList = new List<string> { "AuthMain", "FieldStage", "FieldPlayer", "FieldEnemy" };

	private Camera camouflageCamera;

	private RenderTexture camouflageRT;

	public enum FilterTaps
	{
		_3Taps,
		_5Taps,
		_7Taps,
		_9Taps
	}

	public enum RTFormat
	{
		_16bpp,
		_32bpp,
		_64bppHDR
	}
}
