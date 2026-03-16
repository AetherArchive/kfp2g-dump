using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("")]
public class CC_Base : MonoBehaviour
{
	protected Material material
	{
		get
		{
			if (this._material == null)
			{
				this._material = new Material(this.shader);
				this._material.hideFlags = HideFlags.HideAndDontSave;
			}
			return this._material;
		}
	}

	public static bool IsLinear()
	{
		return QualitySettings.activeColorSpace == ColorSpace.Linear;
	}

	protected virtual void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
		if (!this.shader || !this.shader.isSupported)
		{
			base.enabled = false;
		}
	}

	protected virtual void OnDisable()
	{
		if (this._material)
		{
			Object.DestroyImmediate(this._material);
		}
	}

	public Shader shader;

	protected Material _material;
}
