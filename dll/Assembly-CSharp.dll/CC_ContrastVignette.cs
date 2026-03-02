using System;
using UnityEngine;

// Token: 0x0200000D RID: 13
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Contrast Vignette")]
public class CC_ContrastVignette : CC_Base
{
	// Token: 0x06000021 RID: 33 RVA: 0x00002788 File Offset: 0x00000988
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_Data", new Vector4(this.sharpness * 0.01f, this.darkness * 0.02f, this.contrast * 0.01f, this.edge * 0.01f));
		base.material.SetVector("_Coeffs", new Vector4(this.redCoeff, this.greenCoeff, this.blueCoeff, 1f));
		base.material.SetVector("_Center", this.center);
		base.material.SetVector("_Ambient", new Vector4(this.redAmbient, this.greenAmbient, this.blueAmbient, 1f));
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x0400006C RID: 108
	public Vector2 center = new Vector2(0.5f, 0.5f);

	// Token: 0x0400006D RID: 109
	[Range(-100f, 100f)]
	public float sharpness = 32f;

	// Token: 0x0400006E RID: 110
	[Range(0f, 100f)]
	public float darkness = 28f;

	// Token: 0x0400006F RID: 111
	[Range(0f, 200f)]
	public float contrast = 20f;

	// Token: 0x04000070 RID: 112
	[Range(0f, 1f)]
	public float redCoeff = 0.5f;

	// Token: 0x04000071 RID: 113
	[Range(0f, 1f)]
	public float greenCoeff = 0.5f;

	// Token: 0x04000072 RID: 114
	[Range(0f, 1f)]
	public float blueCoeff = 0.5f;

	// Token: 0x04000073 RID: 115
	[Range(0f, 200f)]
	public float edge;

	// Token: 0x04000074 RID: 116
	[Range(0f, 1f)]
	public float redAmbient;

	// Token: 0x04000075 RID: 117
	[Range(0f, 1f)]
	public float greenAmbient;

	// Token: 0x04000076 RID: 118
	[Range(0f, 1f)]
	public float blueAmbient;
}
