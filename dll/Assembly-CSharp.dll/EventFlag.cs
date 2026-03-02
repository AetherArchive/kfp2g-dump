using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C8 RID: 200
public class EventFlag : MonoBehaviour
{
	// Token: 0x170001CF RID: 463
	// (get) Token: 0x060008FA RID: 2298 RVA: 0x00038FE3 File Offset: 0x000371E3
	protected Material flagMaterial
	{
		get
		{
			if (this.m_flagMaterial == null)
			{
				this.m_flagMaterial = new Material(this.flagShader);
				this.m_flagMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_flagMaterial;
		}
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x00039017 File Offset: 0x00037217
	public void Awake()
	{
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x00039019 File Offset: 0x00037219
	private void Start()
	{
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x0003901C File Offset: 0x0003721C
	public void OnEnable()
	{
		Graphic component = base.gameObject.GetComponent<Graphic>();
		if (component != null)
		{
			this.defaultMaterial = component.material;
			Renderer component2 = base.gameObject.GetComponent<Renderer>();
			if (component2 != null)
			{
				component2.sharedMaterial = this.flagMaterial;
			}
			component.material = this.flagMaterial;
		}
		this.time = 0f;
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x00039084 File Offset: 0x00037284
	private void Update()
	{
		if ((this.time += TimeManager.DeltaTime * Random.Range(1f, 10f)) > this.pi)
		{
			this.time -= this.pi;
		}
		Graphic component = base.gameObject.GetComponent<Graphic>();
		if (component != null)
		{
			component.material.SetFloat("_TimeY", this.time);
		}
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x000390FC File Offset: 0x000372FC
	public void OnDisable()
	{
		Graphic component = base.gameObject.GetComponent<Graphic>();
		if (component != null)
		{
			Renderer component2 = base.gameObject.GetComponent<Renderer>();
			if (component2 != null)
			{
				component2.sharedMaterial = this.defaultMaterial;
			}
			component.material = this.defaultMaterial;
		}
		if (this.m_flagMaterial != null)
		{
			Object.DestroyImmediate(this.m_flagMaterial);
			this.m_flagMaterial = null;
		}
	}

	// Token: 0x0400075F RID: 1887
	[HideInInspector]
	public Shader flagShader;

	// Token: 0x04000760 RID: 1888
	private Material m_flagMaterial;

	// Token: 0x04000761 RID: 1889
	private Material defaultMaterial;

	// Token: 0x04000762 RID: 1890
	private float time;

	// Token: 0x04000763 RID: 1891
	private float pi = 6.2831855f;
}
