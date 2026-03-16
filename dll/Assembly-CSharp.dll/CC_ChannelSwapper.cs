using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Channel Swapper")]
public class CC_ChannelSwapper : CC_Base
{
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_Red", CC_ChannelSwapper.m_Channels[this.red]);
		base.material.SetVector("_Green", CC_ChannelSwapper.m_Channels[this.green]);
		base.material.SetVector("_Blue", CC_ChannelSwapper.m_Channels[this.blue]);
		Graphics.Blit(source, destination, base.material);
	}

	public int red;

	public int green = 1;

	public int blue = 2;

	private static Vector4[] m_Channels = new Vector4[]
	{
		new Vector4(1f, 0f, 0f, 0f),
		new Vector4(0f, 1f, 0f, 0f),
		new Vector4(0f, 0f, 1f, 0f)
	};
}
