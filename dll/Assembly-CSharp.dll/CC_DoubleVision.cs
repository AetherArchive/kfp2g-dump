using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Double Vision")]
public class CC_DoubleVision : CC_Base
{
	// Token: 0x06000028 RID: 40 RVA: 0x00002AC8 File Offset: 0x00000CC8
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetVector("_Displace", new Vector2(this.displace.x / (float)Screen.width, this.displace.y / (float)Screen.height));
		base.material.SetFloat("_Amount", this.amount);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04000081 RID: 129
	public Vector2 displace = new Vector2(0.7f, 0f);

	// Token: 0x04000082 RID: 130
	[Range(0f, 1f)]
	public float amount = 1f;
}
