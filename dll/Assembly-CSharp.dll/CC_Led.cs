using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/LED")]
public class CC_Led : CC_Base
{
	protected override void Start()
	{
		base.Start();
		this.m_Camera = base.GetComponent<Camera>();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		int num = this.mode;
		if (num != 0)
		{
			if (num != 1)
			{
			}
			base.material.SetFloat("_Scale", (float)this.m_Camera.pixelWidth / this.scale);
		}
		else
		{
			base.material.SetFloat("_Scale", this.scale);
		}
		base.material.SetFloat("_Ratio", this.automaticRatio ? ((float)this.m_Camera.pixelWidth / (float)this.m_Camera.pixelHeight) : this.ratio);
		base.material.SetFloat("_Brightness", this.brightness);
		Graphics.Blit(source, destination, base.material);
	}

	[Range(1f, 255f)]
	public float scale = 80f;

	[Range(0f, 10f)]
	public float brightness = 1f;

	public bool automaticRatio;

	public float ratio = 1f;

	public int mode;

	protected Camera m_Camera;
}
