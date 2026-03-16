using System;
using UnityEngine;
using UnityEngine.UI;

public class EventFlag : MonoBehaviour
{
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

	public void Awake()
	{
	}

	private void Start()
	{
	}

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

	[HideInInspector]
	public Shader flagShader;

	private Material m_flagMaterial;

	private Material defaultMaterial;

	private float time;

	private float pi = 6.2831855f;
}
