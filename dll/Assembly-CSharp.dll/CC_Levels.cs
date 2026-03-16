using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Levels")]
public class CC_Levels : CC_Base
{
	public int mode
	{
		get
		{
			if (!this.isRGB)
			{
				return 0;
			}
			return 1;
		}
		set
		{
			this.isRGB = value > 0;
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.isRGB)
		{
			base.material.SetVector("_InputMin", new Vector4(this.inputMinL / 255f, this.inputMinL / 255f, this.inputMinL / 255f, 1f));
			base.material.SetVector("_InputMax", new Vector4(this.inputMaxL / 255f, this.inputMaxL / 255f, this.inputMaxL / 255f, 1f));
			base.material.SetVector("_InputGamma", new Vector4(this.inputGammaL, this.inputGammaL, this.inputGammaL, 1f));
			base.material.SetVector("_OutputMin", new Vector4(this.outputMinL / 255f, this.outputMinL / 255f, this.outputMinL / 255f, 1f));
			base.material.SetVector("_OutputMax", new Vector4(this.outputMaxL / 255f, this.outputMaxL / 255f, this.outputMaxL / 255f, 1f));
		}
		else
		{
			base.material.SetVector("_InputMin", new Vector4(this.inputMinR / 255f, this.inputMinG / 255f, this.inputMinB / 255f, 1f));
			base.material.SetVector("_InputMax", new Vector4(this.inputMaxR / 255f, this.inputMaxG / 255f, this.inputMaxB / 255f, 1f));
			base.material.SetVector("_InputGamma", new Vector4(this.inputGammaR, this.inputGammaG, this.inputGammaB, 1f));
			base.material.SetVector("_OutputMin", new Vector4(this.outputMinR / 255f, this.outputMinG / 255f, this.outputMinB / 255f, 1f));
			base.material.SetVector("_OutputMax", new Vector4(this.outputMaxR / 255f, this.outputMaxG / 255f, this.outputMaxB / 255f, 1f));
		}
		Graphics.Blit(source, destination, base.material);
	}

	public bool isRGB;

	public float inputMinL;

	public float inputMaxL = 255f;

	public float inputGammaL = 1f;

	public float inputMinR;

	public float inputMaxR = 255f;

	public float inputGammaR = 1f;

	public float inputMinG;

	public float inputMaxG = 255f;

	public float inputGammaG = 1f;

	public float inputMinB;

	public float inputMaxB = 255f;

	public float inputGammaB = 1f;

	public float outputMinL;

	public float outputMaxL = 255f;

	public float outputMinR;

	public float outputMaxR = 255f;

	public float outputMinG;

	public float outputMaxG = 255f;

	public float outputMinB;

	public float outputMaxB = 255f;

	public int currentChannel;

	public bool logarithmic;
}
