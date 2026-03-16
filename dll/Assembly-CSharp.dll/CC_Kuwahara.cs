using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Kuwahara")]
public class CC_Kuwahara : CC_Base
{
	protected override void Start()
	{
		base.Start();
		this.m_Camera = base.GetComponent<Camera>();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.radius = Mathf.Clamp(this.radius, 1, 4);
		base.material.SetVector("_TexelSize", new Vector2(1f / (float)this.m_Camera.pixelWidth, 1f / (float)this.m_Camera.pixelHeight));
		Graphics.Blit(source, destination, base.material, this.radius - 1);
	}

	[Range(1f, 4f)]
	public int radius = 3;

	protected Camera m_Camera;
}
