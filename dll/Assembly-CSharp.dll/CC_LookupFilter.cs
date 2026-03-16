using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Lookup Filter (Color Grading)")]
public class CC_LookupFilter : CC_Base
{
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.lookupTexture == null)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetTexture("_LookupTex", this.lookupTexture);
		base.material.SetFloat("_Amount", this.amount);
		Graphics.Blit(source, destination, base.material, CC_Base.IsLinear() ? 1 : 0);
	}

	public Texture lookupTexture;

	[Range(0f, 1f)]
	public float amount = 1f;
}
