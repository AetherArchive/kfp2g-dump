using System;
using UnityEngine;

// Token: 0x02000024 RID: 36
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Threshold")]
public class CC_Threshold : CC_Base
{
	// Token: 0x0600005F RID: 95 RVA: 0x00003BBC File Offset: 0x00001DBC
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Threshold", this.threshold / 255f);
		base.material.SetFloat("_Range", this.noiseRange / 255f);
		Graphics.Blit(source, destination, base.material, this.useNoise ? 1 : 0);
	}

	// Token: 0x040000E6 RID: 230
	[Range(1f, 255f)]
	public float threshold = 128f;

	// Token: 0x040000E7 RID: 231
	[Range(0f, 128f)]
	public float noiseRange = 48f;

	// Token: 0x040000E8 RID: 232
	public bool useNoise;
}
