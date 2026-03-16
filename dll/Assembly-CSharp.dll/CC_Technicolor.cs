using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Technicolor")]
public class CC_Technicolor : CC_Base
{
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Exposure", 8f - this.exposure);
		base.material.SetVector("_Balance", Vector3.one - this.balance);
		base.material.SetFloat("_Amount", this.amount);
		Graphics.Blit(source, destination, base.material);
	}

	[Range(0f, 8f)]
	public float exposure = 4f;

	public Vector3 balance = new Vector3(0.25f, 0.25f, 0.25f);

	[Range(0f, 1f)]
	public float amount = 0.5f;
}
