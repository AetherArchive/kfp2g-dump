using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Convolution Matrix 3x3")]
public class CC_Convolution3x3 : CC_Base
{
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

	public Vector3 kernelTop = Vector3.zero;

	public Vector3 kernelMiddle = Vector3.up;

	public Vector3 kernelBottom = Vector3.zero;

	public float divisor = 1f;

	[Range(0f, 1f)]
	public float amount = 1f;
}
