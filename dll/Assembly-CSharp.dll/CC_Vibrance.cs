using System;
using UnityEngine;

// Token: 0x02000025 RID: 37
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Vibrance")]
public class CC_Vibrance : CC_Base
{
	// Token: 0x06000061 RID: 97 RVA: 0x00003C38 File Offset: 0x00001E38
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		if (this.advanced)
		{
			base.material.SetFloat("_Amount", this.amount * 0.01f);
			base.material.SetVector("_Channels", new Vector3(this.redChannel, this.greenChannel, this.blueChannel));
			Graphics.Blit(source, destination, base.material, 1);
			return;
		}
		base.material.SetFloat("_Amount", this.amount * 0.02f);
		Graphics.Blit(source, destination, base.material, 0);
	}

	// Token: 0x040000E9 RID: 233
	[Range(-100f, 100f)]
	public float amount;

	// Token: 0x040000EA RID: 234
	[Range(-5f, 5f)]
	public float redChannel = 1f;

	// Token: 0x040000EB RID: 235
	[Range(-5f, 5f)]
	public float greenChannel = 1f;

	// Token: 0x040000EC RID: 236
	[Range(-5f, 5f)]
	public float blueChannel = 1f;

	// Token: 0x040000ED RID: 237
	public bool advanced;
}
