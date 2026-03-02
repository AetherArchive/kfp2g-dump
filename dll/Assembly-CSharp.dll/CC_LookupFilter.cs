using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Lookup Filter (Color Grading)")]
public class CC_LookupFilter : CC_Base
{
	// Token: 0x0600004C RID: 76 RVA: 0x000036BC File Offset: 0x000018BC
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

	// Token: 0x040000D0 RID: 208
	public Texture lookupTexture;

	// Token: 0x040000D1 RID: 209
	[Range(0f, 1f)]
	public float amount = 1f;
}
