using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Channel Clamper")]
public class CC_ChannelClamper : CC_Base
{
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_RedClamp", this.red);
		base.material.SetVector("_GreenClamp", this.green);
		base.material.SetVector("_BlueClamp", this.blue);
		Graphics.Blit(source, destination, base.material);
	}

	public Vector2 red = new Vector2(0f, 1f);

	public Vector2 green = new Vector2(0f, 1f);

	public Vector2 blue = new Vector2(0f, 1f);
}
