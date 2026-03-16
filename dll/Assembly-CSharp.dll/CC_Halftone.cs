using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Halftone")]
public class CC_Halftone : CC_Base
{
	protected override void Start()
	{
		base.Start();
		this.m_Camera = base.GetComponent<Camera>();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Density", this.density);
		base.material.SetFloat("_AspectRatio", this.m_Camera.aspect);
		int num = 0;
		if (this.mode == 0)
		{
			if (this.antialiasing && this.showOriginal)
			{
				num = 3;
			}
			else if (this.antialiasing)
			{
				num = 1;
			}
			else if (this.showOriginal)
			{
				num = 2;
			}
		}
		else if (this.mode == 1)
		{
			num = (this.antialiasing ? 5 : 4);
		}
		Graphics.Blit(source, destination, base.material, num);
	}

	[Range(0f, 512f)]
	public float density = 64f;

	public int mode = 1;

	public bool antialiasing = true;

	public bool showOriginal;

	protected Camera m_Camera;
}
