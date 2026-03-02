using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
[ExecuteInEditMode]
[AddComponentMenu("Colorful/RGB Split")]
public class CC_RGBSplit : CC_Base
{
	// Token: 0x06000059 RID: 89 RVA: 0x000039D0 File Offset: 0x00001BD0
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetFloat("_RGBShiftAmount", this.amount * 0.001f);
		base.material.SetFloat("_RGBShiftAngleCos", Mathf.Cos(this.angle));
		base.material.SetFloat("_RGBShiftAngleSin", Mathf.Sin(this.angle));
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040000DF RID: 223
	public float amount;

	// Token: 0x040000E0 RID: 224
	public float angle;
}
