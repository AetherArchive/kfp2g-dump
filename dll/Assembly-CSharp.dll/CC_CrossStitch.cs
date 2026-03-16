using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Cross Stitch")]
public class CC_CrossStitch : CC_Base
{
	protected override void Start()
	{
		base.Start();
		this.m_Camera = base.GetComponent<Camera>();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_StitchSize", (float)this.size);
		base.material.SetFloat("_Brightness", this.brightness);
		int num = (this.invert ? 1 : 0);
		if (this.pixelize)
		{
			num += 2;
			base.material.SetFloat("_Scale", (float)(this.m_Camera.pixelWidth / this.size));
			base.material.SetFloat("_Ratio", (float)(this.m_Camera.pixelWidth / this.m_Camera.pixelHeight));
		}
		Graphics.Blit(source, destination, base.material, num);
	}

	[Range(1f, 128f)]
	public int size = 8;

	public float brightness = 1.5f;

	public bool invert;

	public bool pixelize = true;

	protected Camera m_Camera;
}
