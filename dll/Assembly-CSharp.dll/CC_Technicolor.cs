using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Technicolor")]
public class CC_Technicolor : CC_Base
{
	// Token: 0x0600005D RID: 93 RVA: 0x00003B10 File Offset: 0x00001D10
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Exposure", 8f - this.exposure);
		base.material.SetVector("_Balance", Vector3.one - this.balance);
		base.material.SetFloat("_Amount", this.amount);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040000E3 RID: 227
	[Range(0f, 8f)]
	public float exposure = 4f;

	// Token: 0x040000E4 RID: 228
	public Vector3 balance = new Vector3(0.25f, 0.25f, 0.25f);

	// Token: 0x040000E5 RID: 229
	[Range(0f, 1f)]
	public float amount = 0.5f;
}
