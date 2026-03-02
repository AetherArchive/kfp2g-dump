using System;
using UnityEngine;

// Token: 0x02000007 RID: 7
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Bleach Bypass")]
public class CC_BleachBypass : CC_Base
{
	// Token: 0x06000014 RID: 20 RVA: 0x000022D5 File Offset: 0x000004D5
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetFloat("_Amount", this.amount);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x0400004E RID: 78
	[Range(0f, 1f)]
	public float amount = 1f;
}
