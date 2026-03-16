using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Grayscale")]
public class CC_Grayscale : CC_Base
{
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

	[Range(0f, 1f)]
	public float redLuminance = 0.299f;

	[Range(0f, 1f)]
	public float greenLuminance = 0.587f;

	[Range(0f, 1f)]
	public float blueLuminance = 0.114f;

	[Range(0f, 1f)]
	public float amount = 1f;
}
