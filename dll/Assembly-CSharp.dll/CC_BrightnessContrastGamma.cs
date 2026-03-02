using System;
using UnityEngine;

// Token: 0x02000009 RID: 9
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Brightness, Contrast, Gamma")]
public class CC_BrightnessContrastGamma : CC_Base
{
	// Token: 0x06000018 RID: 24 RVA: 0x000023A8 File Offset: 0x000005A8
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

	// Token: 0x04000052 RID: 82
	[Range(-100f, 100f)]
	public float brightness;

	// Token: 0x04000053 RID: 83
	[Range(-100f, 100f)]
	public float contrast;

	// Token: 0x04000054 RID: 84
	[Range(0f, 1f)]
	public float redCoeff = 0.5f;

	// Token: 0x04000055 RID: 85
	[Range(0f, 1f)]
	public float greenCoeff = 0.5f;

	// Token: 0x04000056 RID: 86
	[Range(0f, 1f)]
	public float blueCoeff = 0.5f;

	// Token: 0x04000057 RID: 87
	[Range(0.1f, 9.9f)]
	public float gamma = 1f;
}
