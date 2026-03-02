using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001DD RID: 477
public class PguiReplaceSpriteCtrl : MonoBehaviour
{
	// Token: 0x06002020 RID: 8224 RVA: 0x0018A261 File Offset: 0x00188461
	public void InitForce()
	{
		this.Awake();
	}

	// Token: 0x06002021 RID: 8225 RVA: 0x0018A26C File Offset: 0x0018846C
	public void Awake()
	{
		if (this.isInit)
		{
			return;
		}
		this.isInit = true;
		this.m_SpriteMap = new Dictionary<int, PguiReplaceSpriteCtrl.ExSprite>();
		int num = 0;
		while (this.m_SpriteList != null && num < this.m_SpriteList.Count)
		{
			this.m_SpriteMap[this.m_SpriteList[num].id] = this.m_SpriteList[num];
			num++;
		}
	}

	// Token: 0x06002022 RID: 8226 RVA: 0x0018A2DA File Offset: 0x001884DA
	public Sprite GetSpriteById(int id)
	{
		if (this.m_SpriteMap == null)
		{
			this.InitForce();
		}
		if (this.m_SpriteMap.ContainsKey(id))
		{
			return this.m_SpriteMap[id].m_Sprite;
		}
		return null;
	}

	// Token: 0x06002023 RID: 8227 RVA: 0x0018A30C File Offset: 0x0018850C
	public void Replace(GameObject go, int id)
	{
		PguiImageCtrl component = go.GetComponent<PguiImageCtrl>();
		if (component != null)
		{
			component.m_Image.sprite = this.GetSpriteById(id);
		}
	}

	// Token: 0x06002024 RID: 8228 RVA: 0x0018A33C File Offset: 0x0018853C
	public void Replace(int id)
	{
		Image component = base.GetComponent<Image>();
		if (component != null)
		{
			component.sprite = this.GetSpriteById(id);
			return;
		}
		component.sprite = null;
	}

	// Token: 0x06002025 RID: 8229 RVA: 0x0018A370 File Offset: 0x00188570
	public void Replace(PhotoDef.Type type)
	{
		int num = -1;
		if (type != PhotoDef.Type.PARAMETER)
		{
			if (type == PhotoDef.Type.ABILITY)
			{
				num = 1;
			}
		}
		else
		{
			num = 2;
		}
		this.Replace(num);
	}

	// Token: 0x04001752 RID: 5970
	[SerializeField]
	private List<PguiReplaceSpriteCtrl.ExSprite> m_SpriteList = new List<PguiReplaceSpriteCtrl.ExSprite>();

	// Token: 0x04001753 RID: 5971
	private Dictionary<int, PguiReplaceSpriteCtrl.ExSprite> m_SpriteMap;

	// Token: 0x04001754 RID: 5972
	private bool isInit;

	// Token: 0x02001023 RID: 4131
	[Serializable]
	public class ExSprite
	{
		// Token: 0x04005AAF RID: 23215
		public int id;

		// Token: 0x04005AB0 RID: 23216
		public Sprite m_Sprite;
	}
}
