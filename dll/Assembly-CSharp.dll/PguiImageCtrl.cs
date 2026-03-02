using System;
using UnityEngine.U2D;
using UnityEngine.UI;

// Token: 0x020001D4 RID: 468
public class PguiImageCtrl : PguiBehaviour
{
	// Token: 0x06001FBE RID: 8126 RVA: 0x001876A9 File Offset: 0x001858A9
	public void SetImageByName(string spriteName)
	{
		this.m_Image.sprite = this.atlas.GetSprite(spriteName);
	}

	// Token: 0x06001FBF RID: 8127 RVA: 0x001876C4 File Offset: 0x001858C4
	public void ReplaceColorByPguiColorCtrl(string key)
	{
		if (this.m_PguiColorCtrl == null)
		{
			this.m_PguiColorCtrl = base.GetComponent<PguiColorCtrl>();
		}
		if (this.m_PguiColorCtrl != null)
		{
			this.m_Image.color = this.m_PguiColorCtrl.GetGameObjectById(key);
		}
	}

	// Token: 0x0400171A RID: 5914
	public Image m_Image;

	// Token: 0x0400171B RID: 5915
	public SpriteAtlas atlas;

	// Token: 0x0400171C RID: 5916
	private PguiColorCtrl m_PguiColorCtrl;

	// Token: 0x0400171D RID: 5917
	public bool customize;
}
