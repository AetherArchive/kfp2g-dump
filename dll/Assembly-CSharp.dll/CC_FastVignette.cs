using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Fast Vignette")]
public class CC_FastVignette : CC_Base
{
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_Data", new Vector4(this.center.x, this.center.y, this.sharpness * 0.01f, this.darkness * 0.02f));
		Graphics.Blit(source, destination, base.material, this.desaturate ? 1 : 0);
	}

	public Vector2 center = new Vector2(0.5f, 0.5f);

	[Range(-100f, 100f)]
	public float sharpness = 10f;

	[Range(0f, 100f)]
	public float darkness = 30f;

	public bool desaturate;
}
