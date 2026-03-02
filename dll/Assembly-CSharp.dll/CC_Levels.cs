using System;
using UnityEngine;

// Token: 0x0200001A RID: 26
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Levels")]
public class CC_Levels : CC_Base
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000048 RID: 72 RVA: 0x00003391 File Offset: 0x00001591
	// (set) Token: 0x06000049 RID: 73 RVA: 0x0000339E File Offset: 0x0000159E
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

	// Token: 0x0600004A RID: 74 RVA: 0x000033B0 File Offset: 0x000015B0
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

	// Token: 0x040000B9 RID: 185
	public bool isRGB;

	// Token: 0x040000BA RID: 186
	public float inputMinL;

	// Token: 0x040000BB RID: 187
	public float inputMaxL = 255f;

	// Token: 0x040000BC RID: 188
	public float inputGammaL = 1f;

	// Token: 0x040000BD RID: 189
	public float inputMinR;

	// Token: 0x040000BE RID: 190
	public float inputMaxR = 255f;

	// Token: 0x040000BF RID: 191
	public float inputGammaR = 1f;

	// Token: 0x040000C0 RID: 192
	public float inputMinG;

	// Token: 0x040000C1 RID: 193
	public float inputMaxG = 255f;

	// Token: 0x040000C2 RID: 194
	public float inputGammaG = 1f;

	// Token: 0x040000C3 RID: 195
	public float inputMinB;

	// Token: 0x040000C4 RID: 196
	public float inputMaxB = 255f;

	// Token: 0x040000C5 RID: 197
	public float inputGammaB = 1f;

	// Token: 0x040000C6 RID: 198
	public float outputMinL;

	// Token: 0x040000C7 RID: 199
	public float outputMaxL = 255f;

	// Token: 0x040000C8 RID: 200
	public float outputMinR;

	// Token: 0x040000C9 RID: 201
	public float outputMaxR = 255f;

	// Token: 0x040000CA RID: 202
	public float outputMinG;

	// Token: 0x040000CB RID: 203
	public float outputMaxG = 255f;

	// Token: 0x040000CC RID: 204
	public float outputMinB;

	// Token: 0x040000CD RID: 205
	public float outputMaxB = 255f;

	// Token: 0x040000CE RID: 206
	public int currentChannel;

	// Token: 0x040000CF RID: 207
	public bool logarithmic;
}
