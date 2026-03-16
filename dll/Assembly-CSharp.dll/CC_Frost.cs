using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Frost")]
public class CC_Frost : CC_Base
{
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.scale == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetFloat("_Scale", this.scale);
		base.material.SetFloat("_Sharpness", this.sharpness * 0.01f);
		base.material.SetFloat("_Darkness", this.darkness * 0.02f);
		Graphics.Blit(source, destination, base.material, this.enableVignette ? 1 : 0);
	}

	[Range(0f, 16f)]
	public float scale = 1.2f;

	[Range(-100f, 100f)]
	public float sharpness = 40f;

	[Range(0f, 100f)]
	public float darkness = 35f;

	public bool enableVignette = true;
}
