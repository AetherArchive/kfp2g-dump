using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Analog TV")]
public class CC_AnalogTV : CC_Base
{
	// Token: 0x0600000C RID: 12 RVA: 0x000020DF File Offset: 0x000002DF
	private void Update()
	{
		if (this.autoPhase)
		{
			this.phase += Time.deltaTime * 0.25f;
		}
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002104 File Offset: 0x00000304
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Phase", this.phase);
		base.material.SetFloat("_NoiseIntensity", this.noiseIntensity);
		base.material.SetFloat("_ScanlinesIntensity", this.scanlinesIntensity);
		base.material.SetFloat("_ScanlinesCount", (float)((int)this.scanlinesCount));
		base.material.SetFloat("_ScanlinesOffset", this.scanlinesOffset);
		base.material.SetFloat("_Distortion", this.distortion);
		base.material.SetFloat("_CubicDistortion", this.cubicDistortion);
		base.material.SetFloat("_Scale", this.scale);
		Graphics.Blit(source, destination, base.material, this.grayscale ? 1 : 0);
	}

	// Token: 0x04000042 RID: 66
	public bool autoPhase = true;

	// Token: 0x04000043 RID: 67
	public float phase = 0.5f;

	// Token: 0x04000044 RID: 68
	public bool grayscale;

	// Token: 0x04000045 RID: 69
	[Range(0f, 1f)]
	public float noiseIntensity = 0.5f;

	// Token: 0x04000046 RID: 70
	[Range(0f, 10f)]
	public float scanlinesIntensity = 2f;

	// Token: 0x04000047 RID: 71
	[Range(0f, 4096f)]
	public float scanlinesCount = 768f;

	// Token: 0x04000048 RID: 72
	public float scanlinesOffset;

	// Token: 0x04000049 RID: 73
	[Range(-2f, 2f)]
	public float distortion = 0.2f;

	// Token: 0x0400004A RID: 74
	[Range(-2f, 2f)]
	public float cubicDistortion = 0.6f;

	// Token: 0x0400004B RID: 75
	[Range(0.01f, 2f)]
	public float scale = 0.8f;
}
