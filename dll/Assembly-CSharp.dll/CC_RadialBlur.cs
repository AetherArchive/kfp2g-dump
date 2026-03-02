using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Radial Blur")]
public class CC_RadialBlur : CC_Base
{
	// Token: 0x06000057 RID: 87 RVA: 0x0000391C File Offset: 0x00001B1C
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetFloat("_Amount", this.amount);
		base.material.SetVector("_Center", this.center);
		base.material.SetFloat("_Samples", (float)this.samples);
		Graphics.Blit(source, destination, base.material, this.quality);
	}

	// Token: 0x040000DB RID: 219
	[Range(0f, 1f)]
	public float amount = 0.1f;

	// Token: 0x040000DC RID: 220
	[Range(2f, 24f)]
	public int samples = 10;

	// Token: 0x040000DD RID: 221
	public Vector2 center = new Vector2(0.5f, 0.5f);

	// Token: 0x040000DE RID: 222
	public int quality = 1;
}
