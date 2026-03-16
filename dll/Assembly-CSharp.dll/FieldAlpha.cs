using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Image Effects/FieldAlpha")]
[RequireComponent(typeof(Camera))]
public class FieldAlpha : MonoBehaviour
{
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

	public void Awake()
	{
		this.mainCamera = base.GetComponent<Camera>();
	}

	public void OnEnable()
	{
		this.alphaCamera = new GameObject("Field Alpha Camera", new Type[] { typeof(Camera) }).GetComponent<Camera>();
		this.alphaCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;
		this.alphaCamera.enabled = false;
	}

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

	[HideInInspector]
	public Shader alphaShader;

	private Material m_alphaMaterial;

	private Camera mainCamera;

	private Camera alphaCamera;

	private RenderTexture alphaRT;

	public static readonly List<string> layerList = new List<string> { "AuthMain", "FieldStage", "FieldPlayer", "FieldEnemy" };
}
