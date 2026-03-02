using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Wiggle")]
public class CC_Wiggle : CC_Base
{
	// Token: 0x06000068 RID: 104 RVA: 0x00003DED File Offset: 0x00001FED
	private void Update()
	{
		if (this.autoTimer)
		{
			this.timer += this.speed * Time.deltaTime;
		}
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00003E10 File Offset: 0x00002010
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Timer", this.timer);
		base.material.SetFloat("_Scale", this.scale);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040000F1 RID: 241
	public float timer;

	// Token: 0x040000F2 RID: 242
	public float speed = 1f;

	// Token: 0x040000F3 RID: 243
	public float scale = 12f;

	// Token: 0x040000F4 RID: 244
	public bool autoTimer = true;
}
