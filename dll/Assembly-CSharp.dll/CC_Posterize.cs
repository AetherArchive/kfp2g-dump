using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Posterize")]
public class CC_Posterize : CC_Base
{
	// Token: 0x06000055 RID: 85 RVA: 0x000038E4 File Offset: 0x00001AE4
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Levels", (float)this.levels);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040000DA RID: 218
	[Range(2f, 255f)]
	public int levels = 4;
}
