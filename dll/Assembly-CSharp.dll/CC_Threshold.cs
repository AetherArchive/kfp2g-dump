using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Threshold")]
public class CC_Threshold : CC_Base
{
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Threshold", this.threshold / 255f);
		base.material.SetFloat("_Range", this.noiseRange / 255f);
		Graphics.Blit(source, destination, base.material, this.useNoise ? 1 : 0);
	}

	[Range(1f, 255f)]
	public float threshold = 128f;

	[Range(0f, 128f)]
	public float noiseRange = 48f;

	public bool useNoise;
}
