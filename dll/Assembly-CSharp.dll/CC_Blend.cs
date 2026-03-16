using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Blend")]
public class CC_Blend : CC_Base
{
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.texture == null || this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetTexture("_OverlayTex", this.texture);
		base.material.SetFloat("_Amount", this.amount);
		Graphics.Blit(source, destination, base.material, this.mode);
	}

	public Texture texture;

	[Range(0f, 1f)]
	public float amount = 1f;

	public int mode;
}
