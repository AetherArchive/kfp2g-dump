using System;
using UnityEngine;

// Token: 0x0200000B RID: 11
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Channel Mixer")]
public class CC_ChannelMixer : CC_Base
{
	// Token: 0x0600001C RID: 28 RVA: 0x00002558 File Offset: 0x00000758
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_Red", new Vector4(this.redR * 0.01f, this.greenR * 0.01f, this.blueR * 0.01f));
		base.material.SetVector("_Green", new Vector4(this.redG * 0.01f, this.greenG * 0.01f, this.blueG * 0.01f));
		base.material.SetVector("_Blue", new Vector4(this.redB * 0.01f, this.greenB * 0.01f, this.blueB * 0.01f));
		base.material.SetVector("_Constant", new Vector4(this.constantR * 0.01f, this.constantG * 0.01f, this.constantB * 0.01f));
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x0400005B RID: 91
	[Range(-200f, 200f)]
	public float redR = 100f;

	// Token: 0x0400005C RID: 92
	[Range(-200f, 200f)]
	public float redG;

	// Token: 0x0400005D RID: 93
	[Range(-200f, 200f)]
	public float redB;

	// Token: 0x0400005E RID: 94
	[Range(-200f, 200f)]
	public float greenR;

	// Token: 0x0400005F RID: 95
	[Range(-200f, 200f)]
	public float greenG = 100f;

	// Token: 0x04000060 RID: 96
	[Range(-200f, 200f)]
	public float greenB;

	// Token: 0x04000061 RID: 97
	[Range(-200f, 200f)]
	public float blueR;

	// Token: 0x04000062 RID: 98
	[Range(-200f, 200f)]
	public float blueG;

	// Token: 0x04000063 RID: 99
	[Range(-200f, 200f)]
	public float blueB = 100f;

	// Token: 0x04000064 RID: 100
	[Range(-200f, 200f)]
	public float constantR;

	// Token: 0x04000065 RID: 101
	[Range(-200f, 200f)]
	public float constantG;

	// Token: 0x04000066 RID: 102
	[Range(-200f, 200f)]
	public float constantB;

	// Token: 0x04000067 RID: 103
	public int currentChannel;
}
