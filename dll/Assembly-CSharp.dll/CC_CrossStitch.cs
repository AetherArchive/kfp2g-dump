using System;
using UnityEngine;

// Token: 0x0200000F RID: 15
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Cross Stitch")]
public class CC_CrossStitch : CC_Base
{
	// Token: 0x06000025 RID: 37 RVA: 0x000029E1 File Offset: 0x00000BE1
	protected override void Start()
	{
		base.Start();
		this.m_Camera = base.GetComponent<Camera>();
	}

	// Token: 0x06000026 RID: 38 RVA: 0x000029F8 File Offset: 0x00000BF8
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

	// Token: 0x0400007C RID: 124
	[Range(1f, 128f)]
	public int size = 8;

	// Token: 0x0400007D RID: 125
	public float brightness = 1.5f;

	// Token: 0x0400007E RID: 126
	public bool invert;

	// Token: 0x0400007F RID: 127
	public bool pixelize = true;

	// Token: 0x04000080 RID: 128
	protected Camera m_Camera;
}
