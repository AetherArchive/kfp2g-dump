using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
[ExecuteInEditMode]
[AddComponentMenu("Colorful/LED")]
public class CC_Led : CC_Base
{
	// Token: 0x06000045 RID: 69 RVA: 0x0000329E File Offset: 0x0000149E
	protected override void Start()
	{
		base.Start();
		this.m_Camera = base.GetComponent<Camera>();
	}

	// Token: 0x06000046 RID: 70 RVA: 0x000032B4 File Offset: 0x000014B4
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

	// Token: 0x040000B3 RID: 179
	[Range(1f, 255f)]
	public float scale = 80f;

	// Token: 0x040000B4 RID: 180
	[Range(0f, 10f)]
	public float brightness = 1f;

	// Token: 0x040000B5 RID: 181
	public bool automaticRatio;

	// Token: 0x040000B6 RID: 182
	public float ratio = 1f;

	// Token: 0x040000B7 RID: 183
	public int mode;

	// Token: 0x040000B8 RID: 184
	protected Camera m_Camera;
}
