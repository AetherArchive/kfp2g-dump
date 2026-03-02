using System;
using UnityEngine;

// Token: 0x020001F3 RID: 499
public class ImageCapture : MonoBehaviour
{
	// Token: 0x06002146 RID: 8518 RVA: 0x0018E670 File Offset: 0x0018C870
	private void Start()
	{
	}

	// Token: 0x06002147 RID: 8519 RVA: 0x0018E672 File Offset: 0x0018C872
	private void Update()
	{
	}

	// Token: 0x06002148 RID: 8520 RVA: 0x0018E674 File Offset: 0x0018C874
	public void capture(Rect field, int width, int height)
	{
		this.data = null;
		this.cap = field;
		base.GetComponent<Camera>().targetTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
		this.tex = new Texture2D((int)this.cap.width, (int)this.cap.height, TextureFormat.ARGB32, false);
	}

	// Token: 0x06002149 RID: 8521 RVA: 0x0018E6CC File Offset: 0x0018C8CC
	private void OnPostRender()
	{
		if (this.tex != null)
		{
			this.tex.ReadPixels(this.cap, 0, 0);
			this.tex.Apply();
			this.data = this.tex.EncodeToPNG();
			this.tex = null;
			base.GetComponent<Camera>().targetTexture = null;
		}
	}

	// Token: 0x040017E8 RID: 6120
	public byte[] data;

	// Token: 0x040017E9 RID: 6121
	private Texture2D tex;

	// Token: 0x040017EA RID: 6122
	private Rect cap;
}
