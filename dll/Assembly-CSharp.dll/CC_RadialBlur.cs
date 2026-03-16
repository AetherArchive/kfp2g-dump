using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Radial Blur")]
public class CC_RadialBlur : CC_Base
{
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetFloat("_Amount", this.amount);
		base.material.SetVector("_Center", this.center);
		base.material.SetFloat("_Samples", (float)this.samples);
		Graphics.Blit(source, destination, base.material, this.quality);
	}

	[Range(0f, 1f)]
	public float amount = 0.1f;

	[Range(2f, 24f)]
	public int samples = 10;

	public Vector2 center = new Vector2(0.5f, 0.5f);

	public int quality = 1;
}
