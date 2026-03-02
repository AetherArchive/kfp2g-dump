using System;
using UnityEngine;

// Token: 0x02000016 RID: 22
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Halftone")]
public class CC_Halftone : CC_Base
{
	// Token: 0x06000037 RID: 55 RVA: 0x00002F18 File Offset: 0x00001118
	protected override void Start()
	{
		base.Start();
		this.m_Camera = base.GetComponent<Camera>();
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00002F2C File Offset: 0x0000112C
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

	// Token: 0x04000095 RID: 149
	[Range(0f, 512f)]
	public float density = 64f;

	// Token: 0x04000096 RID: 150
	public int mode = 1;

	// Token: 0x04000097 RID: 151
	public bool antialiasing = true;

	// Token: 0x04000098 RID: 152
	public bool showOriginal;

	// Token: 0x04000099 RID: 153
	protected Camera m_Camera;
}
