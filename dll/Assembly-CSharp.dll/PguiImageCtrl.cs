using System;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PguiImageCtrl : PguiBehaviour
{
	public void SetImageByName(string spriteName)
	{
		this.m_Image.sprite = this.atlas.GetSprite(spriteName);
	}

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

	public Image m_Image;

	public SpriteAtlas atlas;

	private PguiColorCtrl m_PguiColorCtrl;

	public bool customize;
}
