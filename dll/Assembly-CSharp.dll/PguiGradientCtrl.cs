using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PguiGradientCtrl : MonoBehaviour
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
		this.m_GradientMap = new Dictionary<string, PguiGradientCtrl.ExGradient>();
		int num = 0;
		while (this.m_GradientList != null && num < this.m_GradientList.Count)
		{
			this.m_GradientMap[this.m_GradientList[num].id] = this.m_GradientList[num];
			num++;
		}
	}

	public Gradient GetGradientById(string id)
	{
		this.InitForce();
		if (this.m_GradientMap.ContainsKey(id))
		{
			return this.m_GradientMap[id].gradient;
		}
		return null;
	}

	public Color GetOutlineById(string id)
	{
		this.InitForce();
		if (this.m_GradientMap.ContainsKey(id))
		{
			return this.m_GradientMap[id].outline;
		}
		return Color.black;
	}

	public void SetGameObjectById(string id)
	{
		GradationText component = base.GetComponent<GradationText>();
		if (component != null)
		{
			Gradient gradientById = this.GetGradientById(id);
			if (gradientById != null)
			{
				component.EffectGradient = gradientById;
			}
		}
		Color outlineById = this.GetOutlineById(id);
		PguiOutline[] components = base.GetComponents<PguiOutline>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].effectColor = outlineById;
		}
		Outline[] components2 = base.GetComponents<Outline>();
		for (int i = 0; i < components2.Length; i++)
		{
			components2[i].effectColor = outlineById;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	[SerializeField]
	private List<PguiGradientCtrl.ExGradient> m_GradientList = new List<PguiGradientCtrl.ExGradient>
	{
		new PguiGradientCtrl.ExGradient
		{
			gradient = new Gradient
			{
				colorKeys = new GradientColorKey[]
				{
					new GradientColorKey(Color.black, 0f),
					new GradientColorKey(Color.white, 1f)
				}
			},
			outline = Color.black
		}
	};

	private Dictionary<string, PguiGradientCtrl.ExGradient> m_GradientMap;

	private bool isInit;

	[Serializable]
	public class ExGradient
	{
		public string id;

		public Gradient gradient;

		public Color outline;
	}
}
