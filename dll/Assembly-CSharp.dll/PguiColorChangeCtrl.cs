using System;
using UnityEngine;
using UnityEngine.UI;

public class PguiColorChangeCtrl : PguiBehaviour
{
	private void Init()
	{
		this.m_RawImage = base.gameObject.GetComponent<RawImage>();
	}

	private void Start()
	{
		this.Init();
	}

	private void Update()
	{
	}

	public void OnValidate()
	{
		this.SetEnable(base.enabled);
	}

	public void SetEnable(bool sw)
	{
		if (this.m_RawImage == null)
		{
			this.Init();
		}
		if (this.m_RawImage != null)
		{
			if (sw)
			{
				this.m_RawImage.material = new Material(Shader.Find("UI/ColorChange"));
				this.m_RawImage.material.color = this.m_Color;
				return;
			}
			this.m_RawImage.material = new Material(Shader.Find("UI/Default"));
		}
	}

	public Color m_Color;

	private RawImage m_RawImage;
}
