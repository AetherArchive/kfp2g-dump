using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Posterize")]
public class CC_Posterize : CC_Base
{
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Levels", (float)this.levels);
		Graphics.Blit(source, destination, base.material);
	}

	[Range(2f, 255f)]
	public int levels = 4;
}
