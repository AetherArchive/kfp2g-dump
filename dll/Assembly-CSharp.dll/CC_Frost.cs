using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Frost")]
public class CC_Frost : CC_Base
{
	// Token: 0x0600002C RID: 44 RVA: 0x00002C10 File Offset: 0x00000E10
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.scale == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetFloat("_Scale", this.scale);
		base.material.SetFloat("_Sharpness", this.sharpness * 0.01f);
		base.material.SetFloat("_Darkness", this.darkness * 0.02f);
		Graphics.Blit(source, destination, base.material, this.enableVignette ? 1 : 0);
	}

	// Token: 0x04000087 RID: 135
	[Range(0f, 16f)]
	public float scale = 1.2f;

	// Token: 0x04000088 RID: 136
	[Range(-100f, 100f)]
	public float sharpness = 40f;

	// Token: 0x04000089 RID: 137
	[Range(0f, 100f)]
	public float darkness = 35f;

	// Token: 0x0400008A RID: 138
	public bool enableVignette = true;
}
