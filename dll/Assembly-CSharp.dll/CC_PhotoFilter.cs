using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Photo Filter")]
public class CC_PhotoFilter : CC_Base
{
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.density == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetColor("_RGB", this.color);
		base.material.SetFloat("_Density", this.density);
		Graphics.Blit(source, destination, base.material);
	}

	public Color color = new Color(1f, 0.5f, 0.2f, 1f);

	[Range(0f, 1f)]
	public float density = 0.35f;
}
