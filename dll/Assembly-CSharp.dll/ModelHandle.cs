using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ModelHandle : MonoBehaviour
{
	public void Initialize(int layerParam)
	{
		this.layer = layerParam;
		this.cullingList = new List<CullingCheck>();
		this.materials = new List<Material>();
		this.alpha = 1f;
		this.NeighboringAlpha = new Vector4(0f, -1f, 0f, 1f);
		this.camouflage = false;
		this.camouflageAlpha = 0f;
		foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>(true))
		{
			if (renderer.materials.Length != 0)
			{
				this.cullingList.Add(renderer.gameObject.AddComponent<CullingCheck>());
				this.materials.AddRange(renderer.materials);
				if (renderer.material.HasProperty("_Matcap"))
				{
					this.matCap = 0f;
				}
			}
			renderer.shadowCastingMode = ShadowCastingMode.Off;
			renderer.receiveShadows = false;
		}
	}

	private void OnDestroy()
	{
	}

	private void Update()
	{
		this.UpdateInternal();
	}

	public void UpdateInternal()
	{
		if (this.modelActive && this.fade != 0f)
		{
			this.alpha += Time.deltaTime / this.fade;
		}
		float num = Time.deltaTime * 5f;
		if (this.NeighboringAlpha.y >= 0f)
		{
			Vector3 position = base.transform.position;
			position.x -= this.NeighboringAlpha.x;
			position.z -= this.NeighboringAlpha.z;
			float num2 = Mathf.Sqrt(position.x * position.x + position.z * position.z);
			if (num2 <= 1f || (num2 < 1.25f && this.NeighboringAlpha.w < 1f))
			{
				num = -num;
			}
		}
		this.NeighboringAlpha.w = Mathf.Clamp01(this.NeighboringAlpha.w + num);
		this.SetEnv();
		this.NeighboringAlpha.y = -1f;
	}

	private void SetEnv()
	{
		int num = this.layer;
		string text = LayerMask.LayerToName(this.layer);
		if (this.alpha < 0f)
		{
			this.alpha = 0f;
		}
		else if (this.alpha > 1f)
		{
			this.alpha = 1f;
		}
		float num2 = this.alpha * this.NeighboringAlpha.w;
		if (num2 <= 0f)
		{
			LayerMask.NameToLayer("Ignore Raycast");
		}
		else if (num2 < 1f)
		{
			if (this.matCap < 0f)
			{
				if (FieldAlpha.layerList.Contains(text))
				{
					LayerMask.NameToLayer(text + "Alpha");
				}
				else
				{
					LayerMask.NameToLayer("FieldPlayerAlpha");
				}
			}
			this.camouflageAlpha = 0f;
		}
		else if (this.camouflage)
		{
			LayerMask.NameToLayer("Camouflage");
			if ((this.camouflageAlpha += TimeManager.DeltaTime) > 0.9f)
			{
				this.camouflageAlpha = 0.9f;
			}
		}
		else if ((this.camouflageAlpha -= TimeManager.DeltaTime) > 0f)
		{
			LayerMask.NameToLayer("Camouflage");
			if (this.camouflageAlpha > 0.77f)
			{
				this.camouflageAlpha = 0.77f;
			}
		}
		else
		{
			this.camouflageAlpha = 0f;
		}
		foreach (Material material in this.materials)
		{
			material.SetFloat("_Alpha", num2);
			material.SetFloat("_Camouflage", this.camouflageAlpha);
			if (material.GetFloat("_MatcapValue") < this.matCap)
			{
				material.SetFloat("_MatcapValue", this.matCap);
			}
		}
	}

	public void SetMatCap(float mc)
	{
		if (this.matCap >= 0f)
		{
			this.matCap = Mathf.Clamp01(mc);
		}
	}

	public float GetMatCap()
	{
		if (this.matCap < 0f)
		{
			return 0f;
		}
		return this.matCap;
	}

	public void SetAlpha(float a)
	{
		this.alpha = a;
		this.fade = 0f;
	}

	public float GetAlpha()
	{
		return this.alpha;
	}

	public void FadeIn(float time)
	{
		this.fade = time;
		if (time < 0.0001f)
		{
			this.SetAlpha(1f);
		}
		this.SetEnv();
	}

	public void FadeOut(float time)
	{
		this.fade = time;
		if (time < 0.0001f)
		{
			this.SetAlpha(0f);
		}
		else
		{
			this.fade = -this.fade;
		}
		this.SetEnv();
	}

	public bool IsDisp()
	{
		return this.fade > 0f || (this.fade == 0f && this.alpha > 0f);
	}

	private int layer;

	private float alpha;

	private float fade;

	private Vector4 NeighboringAlpha = new Vector4(0f, -1f, 0f, 1f);

	private float matCap = -1f;

	public bool camouflage;

	private float camouflageAlpha;

	private List<CullingCheck> cullingList;

	private List<Material> materials;

	private bool modelActive = true;

	private AuthPlayer authPlayer;
}
