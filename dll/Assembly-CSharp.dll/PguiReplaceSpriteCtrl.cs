using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PguiReplaceSpriteCtrl : MonoBehaviour
{
	public void InitForce()
	{
		this.Awake();
	}

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

	public void Replace(GameObject go, int id)
	{
		PguiImageCtrl component = go.GetComponent<PguiImageCtrl>();
		if (component != null)
		{
			component.m_Image.sprite = this.GetSpriteById(id);
		}
	}

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

	[SerializeField]
	private List<PguiReplaceSpriteCtrl.ExSprite> m_SpriteList = new List<PguiReplaceSpriteCtrl.ExSprite>();

	private Dictionary<int, PguiReplaceSpriteCtrl.ExSprite> m_SpriteMap;

	private bool isInit;

	[Serializable]
	public class ExSprite
	{
		public int id;

		public Sprite m_Sprite;
	}
}
