using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Analog TV")]
public class CC_AnalogTV : CC_Base
{
	private void Update()
	{
		if (this.autoPhase)
		{
			this.phase += Time.deltaTime * 0.25f;
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Phase", this.phase);
		base.material.SetFloat("_NoiseIntensity", this.noiseIntensity);
		base.material.SetFloat("_ScanlinesIntensity", this.scanlinesIntensity);
		base.material.SetFloat("_ScanlinesCount", (float)((int)this.scanlinesCount));
		base.material.SetFloat("_ScanlinesOffset", this.scanlinesOffset);
		base.material.SetFloat("_Distortion", this.distortion);
		base.material.SetFloat("_CubicDistortion", this.cubicDistortion);
		base.material.SetFloat("_Scale", this.scale);
		Graphics.Blit(source, destination, base.material, this.grayscale ? 1 : 0);
	}

	public bool autoPhase = true;

	public float phase = 0.5f;

	public bool grayscale;

	[Range(0f, 1f)]
	public float noiseIntensity = 0.5f;

	[Range(0f, 10f)]
	public float scanlinesIntensity = 2f;

	[Range(0f, 4096f)]
	public float scanlinesCount = 768f;

	public float scanlinesOffset;

	[Range(-2f, 2f)]
	public float distortion = 0.2f;

	[Range(-2f, 2f)]
	public float cubicDistortion = 0.6f;

	[Range(0.01f, 2f)]
	public float scale = 0.8f;
}
