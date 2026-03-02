using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000069 RID: 105
public class ModelHandle : MonoBehaviour
{
	// Token: 0x060002AB RID: 683 RVA: 0x00015B54 File Offset: 0x00013D54
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

	// Token: 0x060002AC RID: 684 RVA: 0x00015C2F File Offset: 0x00013E2F
	private void OnDestroy()
	{
	}

	// Token: 0x060002AD RID: 685 RVA: 0x00015C31 File Offset: 0x00013E31
	private void Update()
	{
		this.UpdateInternal();
	}

	// Token: 0x060002AE RID: 686 RVA: 0x00015C3C File Offset: 0x00013E3C
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

	// Token: 0x060002AF RID: 687 RVA: 0x00015D48 File Offset: 0x00013F48
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

	// Token: 0x060002B0 RID: 688 RVA: 0x00015F24 File Offset: 0x00014124
	public void SetMatCap(float mc)
	{
		if (this.matCap >= 0f)
		{
			this.matCap = Mathf.Clamp01(mc);
		}
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x00015F3F File Offset: 0x0001413F
	public float GetMatCap()
	{
		if (this.matCap < 0f)
		{
			return 0f;
		}
		return this.matCap;
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x00015F5A File Offset: 0x0001415A
	public void SetAlpha(float a)
	{
		this.alpha = a;
		this.fade = 0f;
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x00015F6E File Offset: 0x0001416E
	public float GetAlpha()
	{
		return this.alpha;
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x00015F78 File Offset: 0x00014178
	public void FadeIn(float time)
	{
		this.fade = time;
		if (time < 0.0001f)
		{
			this.SetAlpha(1f);
		}
		this.SetEnv();
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x00015FA8 File Offset: 0x000141A8
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

	// Token: 0x060002B6 RID: 694 RVA: 0x00015FE6 File Offset: 0x000141E6
	public bool IsDisp()
	{
		return this.fade > 0f || (this.fade == 0f && this.alpha > 0f);
	}

	// Token: 0x04000448 RID: 1096
	private int layer;

	// Token: 0x04000449 RID: 1097
	private float alpha;

	// Token: 0x0400044A RID: 1098
	private float fade;

	// Token: 0x0400044B RID: 1099
	private Vector4 NeighboringAlpha = new Vector4(0f, -1f, 0f, 1f);

	// Token: 0x0400044C RID: 1100
	private float matCap = -1f;

	// Token: 0x0400044D RID: 1101
	public bool camouflage;

	// Token: 0x0400044E RID: 1102
	private float camouflageAlpha;

	// Token: 0x0400044F RID: 1103
	private List<CullingCheck> cullingList;

	// Token: 0x04000450 RID: 1104
	private List<Material> materials;

	// Token: 0x04000451 RID: 1105
	private bool modelActive = true;

	// Token: 0x04000452 RID: 1106
	private AuthPlayer authPlayer;
}
