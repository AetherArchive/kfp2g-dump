using System;
using UnityEngine;

// Token: 0x02000018 RID: 24
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Kuwahara")]
public class CC_Kuwahara : CC_Base
{
	// Token: 0x06000042 RID: 66 RVA: 0x00003207 File Offset: 0x00001407
	protected override void Start()
	{
		base.Start();
		this.m_Camera = base.GetComponent<Camera>();
	}

	// Token: 0x06000043 RID: 67 RVA: 0x0000321C File Offset: 0x0000141C
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.radius = Mathf.Clamp(this.radius, 1, 4);
		base.material.SetVector("_TexelSize", new Vector2(1f / (float)this.m_Camera.pixelWidth, 1f / (float)this.m_Camera.pixelHeight));
		Graphics.Blit(source, destination, base.material, this.radius - 1);
	}

	// Token: 0x040000B1 RID: 177
	[Range(1f, 4f)]
	public int radius = 3;

	// Token: 0x040000B2 RID: 178
	protected Camera m_Camera;
}
