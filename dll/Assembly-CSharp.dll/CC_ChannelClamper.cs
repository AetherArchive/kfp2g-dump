using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Channel Clamper")]
public class CC_ChannelClamper : CC_Base
{
	// Token: 0x0600001A RID: 26 RVA: 0x00002498 File Offset: 0x00000698
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_RedClamp", this.red);
		base.material.SetVector("_GreenClamp", this.green);
		base.material.SetVector("_BlueClamp", this.blue);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04000058 RID: 88
	public Vector2 red = new Vector2(0f, 1f);

	// Token: 0x04000059 RID: 89
	public Vector2 green = new Vector2(0f, 1f);

	// Token: 0x0400005A RID: 90
	public Vector2 blue = new Vector2(0f, 1f);
}
