using System;
using UnityEngine;

// Token: 0x0200001C RID: 28
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Negative")]
public class CC_Negative : CC_Base
{
	// Token: 0x0600004E RID: 78 RVA: 0x00003736 File Offset: 0x00001936
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

	// Token: 0x040000D2 RID: 210
	[Range(0f, 1f)]
	public float amount = 1f;
}
