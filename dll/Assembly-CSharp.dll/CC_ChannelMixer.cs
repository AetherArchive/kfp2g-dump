using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Channel Mixer")]
public class CC_ChannelMixer : CC_Base
{
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_Red", new Vector4(this.redR * 0.01f, this.greenR * 0.01f, this.blueR * 0.01f));
		base.material.SetVector("_Green", new Vector4(this.redG * 0.01f, this.greenG * 0.01f, this.blueG * 0.01f));
		base.material.SetVector("_Blue", new Vector4(this.redB * 0.01f, this.greenB * 0.01f, this.blueB * 0.01f));
		base.material.SetVector("_Constant", new Vector4(this.constantR * 0.01f, this.constantG * 0.01f, this.constantB * 0.01f));
		Graphics.Blit(source, destination, base.material);
	}

	[Range(-200f, 200f)]
	public float redR = 100f;

	[Range(-200f, 200f)]
	public float redG;

	[Range(-200f, 200f)]
	public float redB;

	[Range(-200f, 200f)]
	public float greenR;

	[Range(-200f, 200f)]
	public float greenG = 100f;

	[Range(-200f, 200f)]
	public float greenB;

	[Range(-200f, 200f)]
	public float blueR;

	[Range(-200f, 200f)]
	public float blueG;

	[Range(-200f, 200f)]
	public float blueB = 100f;

	[Range(-200f, 200f)]
	public float constantR;

	[Range(-200f, 200f)]
	public float constantG;

	[Range(-200f, 200f)]
	public float constantB;

	public int currentChannel;
}
