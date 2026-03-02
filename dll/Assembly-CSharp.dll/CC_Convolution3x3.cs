using System;
using UnityEngine;

// Token: 0x0200000E RID: 14
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Convolution Matrix 3x3")]
public class CC_Convolution3x3 : CC_Base
{
	// Token: 0x06000023 RID: 35 RVA: 0x000028C8 File Offset: 0x00000AC8
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_PX", 1f / (float)Screen.width);
		base.material.SetFloat("_PY", 1f / (float)Screen.height);
		base.material.SetFloat("_Amount", this.amount);
		base.material.SetVector("_KernelT", this.kernelTop / this.divisor);
		base.material.SetVector("_KernelM", this.kernelMiddle / this.divisor);
		base.material.SetVector("_KernelB", this.kernelBottom / this.divisor);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04000077 RID: 119
	public Vector3 kernelTop = Vector3.zero;

	// Token: 0x04000078 RID: 120
	public Vector3 kernelMiddle = Vector3.up;

	// Token: 0x04000079 RID: 121
	public Vector3 kernelBottom = Vector3.zero;

	// Token: 0x0400007A RID: 122
	public float divisor = 1f;

	// Token: 0x0400007B RID: 123
	[Range(0f, 1f)]
	public float amount = 1f;
}
