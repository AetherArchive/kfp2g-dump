using System;
using UnityEngine;

// Token: 0x02000014 RID: 20
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Gradient Ramp")]
public class CC_GradientRamp : CC_Base
{
	// Token: 0x06000033 RID: 51 RVA: 0x00002E0C File Offset: 0x0000100C
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

	// Token: 0x0400008F RID: 143
	public Texture rampTexture;

	// Token: 0x04000090 RID: 144
	[Range(0f, 1f)]
	public float amount = 1f;
}
