using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
[RequireComponent(typeof(Camera))]
[AddComponentMenu("")]
public class CC_Base : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x0600000F RID: 15 RVA: 0x00002243 File Offset: 0x00000443
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

	// Token: 0x06000010 RID: 16 RVA: 0x00002277 File Offset: 0x00000477
	public static bool IsLinear()
	{
		return QualitySettings.activeColorSpace == ColorSpace.Linear;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002281 File Offset: 0x00000481
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

	// Token: 0x06000012 RID: 18 RVA: 0x000022B3 File Offset: 0x000004B3
	protected virtual void OnDisable()
	{
		if (this._material)
		{
			Object.DestroyImmediate(this._material);
		}
	}

	// Token: 0x0400004C RID: 76
	public Shader shader;

	// Token: 0x0400004D RID: 77
	protected Material _material;
}
