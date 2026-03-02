using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
[ExecuteInEditMode]
[AddComponentMenu("Colorful/White Balance")]
public class CC_WhiteBalance : CC_Base
{
	// Token: 0x06000065 RID: 101 RVA: 0x00003D60 File Offset: 0x00001F60
	private void Reset()
	{
		this.white = (CC_Base.IsLinear() ? new Color(0.72974f, 0.72974f, 0.72974f) : new Color(0.5f, 0.5f, 0.5f));
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00003D99 File Offset: 0x00001F99
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetColor("_White", this.white);
		Graphics.Blit(source, destination, base.material, this.mode);
	}

	// Token: 0x040000EF RID: 239
	public Color white = new Color(0.5f, 0.5f, 0.5f);

	// Token: 0x040000F0 RID: 240
	public int mode = 1;
}
