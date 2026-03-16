using System;
using UnityEngine;

[AddComponentMenu("Image Effects/ParallelClip")]
[RequireComponent(typeof(Camera))]
public class ParallelClip : MonoBehaviour
{
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

	public float clipY { get; set; }

	public float clipX { get; set; }

	public float offU { get; set; }

	public float offD { get; set; }

	public float offL { get; set; }

	public float offR { get; set; }

	public float alpha { get; set; }

	public void Awake()
	{
		this.mainCamera = base.GetComponent<Camera>();
	}

	public void OnEnable()
	{
		this.clipCamera = new GameObject("Parallel Clip Camera", new Type[] { typeof(Camera) }).GetComponent<Camera>();
		this.clipCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;
		this.clipCamera.enabled = false;
		this.cullingMask = this.mainCamera.cullingMask;
		this.mainCamera.cullingMask = 0;
	}

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

	private void DeleteClipTexture()
	{
		if (null != this.clipRT)
		{
			this.clipRT.Release();
			Object.DestroyImmediate(this.clipRT);
			this.clipRT = null;
		}
	}

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

	private void Update()
	{
		if (this.mainCamera.cullingMask != 0)
		{
			this.cullingMask = this.mainCamera.cullingMask;
			this.mainCamera.cullingMask = 0;
		}
	}

	[HideInInspector]
	public Shader clipShader;

	private Material m_clipMaterial;

	private Camera mainCamera;

	private Camera clipCamera;

	private RenderTexture clipRT;

	private int cullingMask;
}
