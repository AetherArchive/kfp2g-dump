using System;
using UnityEngine;

// Token: 0x02000015 RID: 21
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Grayscale")]
public class CC_Grayscale : CC_Base
{
	// Token: 0x06000035 RID: 53 RVA: 0x00002E88 File Offset: 0x00001088
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetVector("_Data", new Vector4(this.redLuminance, this.greenLuminance, this.blueLuminance, this.amount));
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04000091 RID: 145
	[Range(0f, 1f)]
	public float redLuminance = 0.299f;

	// Token: 0x04000092 RID: 146
	[Range(0f, 1f)]
	public float greenLuminance = 0.587f;

	// Token: 0x04000093 RID: 147
	[Range(0f, 1f)]
	public float blueLuminance = 0.114f;

	// Token: 0x04000094 RID: 148
	[Range(0f, 1f)]
	public float amount = 1f;
}
