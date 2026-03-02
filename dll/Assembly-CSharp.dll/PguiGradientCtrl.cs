using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001D3 RID: 467
public class PguiGradientCtrl : MonoBehaviour
{
	// Token: 0x06001FB6 RID: 8118 RVA: 0x001874D9 File Offset: 0x001856D9
	public void InitForce()
	{
		this.Awake();
	}

	// Token: 0x06001FB7 RID: 8119 RVA: 0x001874E4 File Offset: 0x001856E4
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

	// Token: 0x06001FB8 RID: 8120 RVA: 0x00187552 File Offset: 0x00185752
	public Gradient GetGradientById(string id)
	{
		this.InitForce();
		if (this.m_GradientMap.ContainsKey(id))
		{
			return this.m_GradientMap[id].gradient;
		}
		return null;
	}

	// Token: 0x06001FB9 RID: 8121 RVA: 0x0018757B File Offset: 0x0018577B
	public Color GetOutlineById(string id)
	{
		this.InitForce();
		if (this.m_GradientMap.ContainsKey(id))
		{
			return this.m_GradientMap[id].outline;
		}
		return Color.black;
	}

	// Token: 0x06001FBA RID: 8122 RVA: 0x001875A8 File Offset: 0x001857A8
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

	// Token: 0x06001FBB RID: 8123 RVA: 0x00187628 File Offset: 0x00185828
	private void Start()
	{
	}

	// Token: 0x06001FBC RID: 8124 RVA: 0x0018762A File Offset: 0x0018582A
	private void Update()
	{
	}

	// Token: 0x04001717 RID: 5911
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

	// Token: 0x04001718 RID: 5912
	private Dictionary<string, PguiGradientCtrl.ExGradient> m_GradientMap;

	// Token: 0x04001719 RID: 5913
	private bool isInit;

	// Token: 0x02001016 RID: 4118
	[Serializable]
	public class ExGradient
	{
		// Token: 0x04005A61 RID: 23137
		public string id;

		// Token: 0x04005A62 RID: 23138
		public Gradient gradient;

		// Token: 0x04005A63 RID: 23139
		public Color outline;
	}
}
