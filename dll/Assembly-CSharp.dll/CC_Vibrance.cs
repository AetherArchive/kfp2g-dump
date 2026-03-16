using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Vibrance")]
public class CC_Vibrance : CC_Base
{
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

	[Range(-100f, 100f)]
	public float amount;

	[Range(-5f, 5f)]
	public float redChannel = 1f;

	[Range(-5f, 5f)]
	public float greenChannel = 1f;

	[Range(-5f, 5f)]
	public float blueChannel = 1f;

	public bool advanced;
}
