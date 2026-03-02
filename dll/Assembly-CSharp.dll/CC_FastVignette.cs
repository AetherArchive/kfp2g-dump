using System;
using UnityEngine;

// Token: 0x02000011 RID: 17
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Fast Vignette")]
public class CC_FastVignette : CC_Base
{
	// Token: 0x0600002A RID: 42 RVA: 0x00002B74 File Offset: 0x00000D74
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_Data", new Vector4(this.center.x, this.center.y, this.sharpness * 0.01f, this.darkness * 0.02f));
		Graphics.Blit(source, destination, base.material, this.desaturate ? 1 : 0);
	}

	// Token: 0x04000083 RID: 131
	public Vector2 center = new Vector2(0.5f, 0.5f);

	// Token: 0x04000084 RID: 132
	[Range(-100f, 100f)]
	public float sharpness = 10f;

	// Token: 0x04000085 RID: 133
	[Range(0f, 100f)]
	public float darkness = 30f;

	// Token: 0x04000086 RID: 134
	public bool desaturate;
}
