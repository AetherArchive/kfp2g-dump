using System;
using UnityEngine;

// Token: 0x0200001D RID: 29
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Photo Filter")]
public class CC_PhotoFilter : CC_Base
{
	// Token: 0x06000050 RID: 80 RVA: 0x00003784 File Offset: 0x00001984
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.density == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetColor("_RGB", this.color);
		base.material.SetFloat("_Density", this.density);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040000D3 RID: 211
	public Color color = new Color(1f, 0.5f, 0.2f, 1f);

	// Token: 0x040000D4 RID: 212
	[Range(0f, 1f)]
	public float density = 0.35f;
}
