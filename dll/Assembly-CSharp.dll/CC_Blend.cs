using System;
using UnityEngine;

// Token: 0x02000008 RID: 8
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Blend")]
public class CC_Blend : CC_Base
{
	// Token: 0x06000016 RID: 22 RVA: 0x00002324 File Offset: 0x00000524
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

	// Token: 0x0400004F RID: 79
	public Texture texture;

	// Token: 0x04000050 RID: 80
	[Range(0f, 1f)]
	public float amount = 1f;

	// Token: 0x04000051 RID: 81
	public int mode;
}
