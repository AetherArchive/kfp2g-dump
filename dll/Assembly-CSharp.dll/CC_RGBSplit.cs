using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/RGB Split")]
public class CC_RGBSplit : CC_Base
{
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetFloat("_RGBShiftAmount", this.amount * 0.001f);
		base.material.SetFloat("_RGBShiftAngleCos", Mathf.Cos(this.angle));
		base.material.SetFloat("_RGBShiftAngleSin", Mathf.Sin(this.angle));
		Graphics.Blit(source, destination, base.material);
	}

	public float amount;

	public float angle;
}
