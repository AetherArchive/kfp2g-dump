using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/White Balance")]
public class CC_WhiteBalance : CC_Base
{
	private void Reset()
	{
		this.white = (CC_Base.IsLinear() ? new Color(0.72974f, 0.72974f, 0.72974f) : new Color(0.5f, 0.5f, 0.5f));
	}

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetColor("_White", this.white);
		Graphics.Blit(source, destination, base.material, this.mode);
	}

	public Color white = new Color(0.5f, 0.5f, 0.5f);

	public int mode = 1;
}
