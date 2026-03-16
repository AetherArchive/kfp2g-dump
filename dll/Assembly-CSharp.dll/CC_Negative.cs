using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Negative")]
public class CC_Negative : CC_Base
{
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetFloat("_Amount", this.amount);
		Graphics.Blit(source, destination, base.material);
	}

	[Range(0f, 1f)]
	public float amount = 1f;
}
