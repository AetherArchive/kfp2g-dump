using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Channel Swapper")]
public class CC_ChannelSwapper : CC_Base
{
	// Token: 0x0600001E RID: 30 RVA: 0x00002680 File Offset: 0x00000880
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_Red", CC_ChannelSwapper.m_Channels[this.red]);
		base.material.SetVector("_Green", CC_ChannelSwapper.m_Channels[this.green]);
		base.material.SetVector("_Blue", CC_ChannelSwapper.m_Channels[this.blue]);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04000068 RID: 104
	public int red;

	// Token: 0x04000069 RID: 105
	public int green = 1;

	// Token: 0x0400006A RID: 106
	public int blue = 2;

	// Token: 0x0400006B RID: 107
	private static Vector4[] m_Channels = new Vector4[]
	{
		new Vector4(1f, 0f, 0f, 0f),
		new Vector4(0f, 1f, 0f, 0f),
		new Vector4(0f, 0f, 1f, 0f)
	};
}
