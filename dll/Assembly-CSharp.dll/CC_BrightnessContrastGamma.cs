using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Brightness, Contrast, Gamma")]
public class CC_BrightnessContrastGamma : CC_Base
{
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.brightness == 0f && this.contrast == 0f && this.gamma == 1f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetVector("_BCG", new Vector4((this.brightness + 100f) * 0.01f, (this.contrast + 100f) * 0.01f, 1f / this.gamma));
		base.material.SetVector("_Coeffs", new Vector4(this.redCoeff, this.greenCoeff, this.blueCoeff, 1f));
		Graphics.Blit(source, destination, base.material);
	}

	[Range(-100f, 100f)]
	public float brightness;

	[Range(-100f, 100f)]
	public float contrast;

	[Range(0f, 1f)]
	public float redCoeff = 0.5f;

	[Range(0f, 1f)]
	public float greenCoeff = 0.5f;

	[Range(0f, 1f)]
	public float blueCoeff = 0.5f;

	[Range(0.1f, 9.9f)]
	public float gamma = 1f;
}
