using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Gradient Ramp")]
public class CC_GradientRamp : CC_Base
{
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.rampTexture == null || this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetTexture("_RampTex", this.rampTexture);
		base.material.SetFloat("_Amount", this.amount);
		Graphics.Blit(source, destination, base.material);
	}

	public Texture rampTexture;

	[Range(0f, 1f)]
	public float amount = 1f;
}
