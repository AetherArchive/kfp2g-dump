using System;
using UnityEngine;

// Token: 0x0200001E RID: 30
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Pixelate")]
public class CC_Pixelate : CC_Base
{
	// Token: 0x06000052 RID: 82 RVA: 0x00003811 File Offset: 0x00001A11
	protected override void Start()
	{
		base.Start();
		this.m_Camera = base.GetComponent<Camera>();
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00003828 File Offset: 0x00001A28
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
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040000D5 RID: 213
	[Range(1f, 1024f)]
	public float scale = 80f;

	// Token: 0x040000D6 RID: 214
	public bool automaticRatio;

	// Token: 0x040000D7 RID: 215
	public float ratio = 1f;

	// Token: 0x040000D8 RID: 216
	public int mode;

	// Token: 0x040000D9 RID: 217
	protected Camera m_Camera;
}
