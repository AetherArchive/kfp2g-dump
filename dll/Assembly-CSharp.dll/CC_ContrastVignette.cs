using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Contrast Vignette")]
public class CC_ContrastVignette : CC_Base
{
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_Data", new Vector4(this.sharpness * 0.01f, this.darkness * 0.02f, this.contrast * 0.01f, this.edge * 0.01f));
		base.material.SetVector("_Coeffs", new Vector4(this.redCoeff, this.greenCoeff, this.blueCoeff, 1f));
		base.material.SetVector("_Center", this.center);
		base.material.SetVector("_Ambient", new Vector4(this.redAmbient, this.greenAmbient, this.blueAmbient, 1f));
		Graphics.Blit(source, destination, base.material);
	}

	public Vector2 center = new Vector2(0.5f, 0.5f);

	[Range(-100f, 100f)]
	public float sharpness = 32f;

	[Range(0f, 100f)]
	public float darkness = 28f;

	[Range(0f, 200f)]
	public float contrast = 20f;

	[Range(0f, 1f)]
	public float redCoeff = 0.5f;

	[Range(0f, 1f)]
	public float greenCoeff = 0.5f;

	[Range(0f, 1f)]
	public float blueCoeff = 0.5f;

	[Range(0f, 200f)]
	public float edge;

	[Range(0f, 1f)]
	public float redAmbient;

	[Range(0f, 1f)]
	public float greenAmbient;

	[Range(0f, 1f)]
	public float blueAmbient;
}
