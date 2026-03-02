using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001D8 RID: 472
public class PguiOutline : BaseMeshEffect
{
	// Token: 0x17000440 RID: 1088
	// (get) Token: 0x06001FF9 RID: 8185 RVA: 0x00189AA2 File Offset: 0x00187CA2
	// (set) Token: 0x06001FF8 RID: 8184 RVA: 0x00189A99 File Offset: 0x00187C99
	public Color effectColor
	{
		get
		{
			return this.m_EffectColor;
		}
		set
		{
			this.m_EffectColor = value;
		}
	}

	// Token: 0x17000441 RID: 1089
	// (get) Token: 0x06001FFB RID: 8187 RVA: 0x00189AB3 File Offset: 0x00187CB3
	// (set) Token: 0x06001FFA RID: 8186 RVA: 0x00189AAA File Offset: 0x00187CAA
	public Vector2 effectDistance
	{
		get
		{
			return this.m_EffectDistance;
		}
		set
		{
			this.m_EffectDistance = value;
		}
	}

	// Token: 0x17000442 RID: 1090
	// (get) Token: 0x06001FFD RID: 8189 RVA: 0x00189AC4 File Offset: 0x00187CC4
	// (set) Token: 0x06001FFC RID: 8188 RVA: 0x00189ABB File Offset: 0x00187CBB
	public bool UseGraphicAlpha
	{
		get
		{
			return this.m_UseGraphicAlpha;
		}
		set
		{
			this.m_UseGraphicAlpha = value;
		}
	}

	// Token: 0x06001FFE RID: 8190 RVA: 0x00189ACC File Offset: 0x00187CCC
	public override void ModifyMesh(VertexHelper vh)
	{
		if (!this.IsActive())
		{
			return;
		}
		List<UIVertex> list = new List<UIVertex>();
		vh.GetUIVertexStream(list);
		this.ModifyVertices(list);
		vh.Clear();
		vh.AddUIVertexTriangleStream(list);
	}

	// Token: 0x06001FFF RID: 8191 RVA: 0x00189B04 File Offset: 0x00187D04
	private void ModifyVertices(List<UIVertex> verts)
	{
		int num = 0;
		int num2 = verts.Count;
		this.ApplyShadow(verts, num, num2, this.m_EffectDistance.x, this.m_EffectDistance.y);
		num = num2;
		num2 = verts.Count;
		this.ApplyShadow(verts, num, num2, -this.m_EffectDistance.x, this.m_EffectDistance.y);
		num = num2;
		num2 = verts.Count;
		this.ApplyShadow(verts, num, num2, this.m_EffectDistance.x, -this.m_EffectDistance.y);
		num = num2;
		num2 = verts.Count;
		this.ApplyShadow(verts, num, num2, -this.m_EffectDistance.x, -this.m_EffectDistance.y);
		num2 = verts.Count;
	}

	// Token: 0x06002000 RID: 8192 RVA: 0x00189BC0 File Offset: 0x00187DC0
	private void ApplyShadow(List<UIVertex> verts, int start, int end, float x, float y)
	{
		for (int i = start; i < end; i++)
		{
			UIVertex uivertex = verts[i];
			verts.Add(uivertex);
			Vector3 position = uivertex.position;
			position.x += x;
			position.y += y;
			uivertex.position = position;
			Color32 color = this.m_EffectColor;
			if (this.m_UseGraphicAlpha && verts[i].color.a < 255)
			{
				color.a = (byte)((float)(color.a * verts[i].color.a) / 255f);
				color.a = color.a * color.a / byte.MaxValue;
			}
			uivertex.color = color;
			verts[i] = uivertex;
		}
	}

	// Token: 0x06002001 RID: 8193 RVA: 0x00189C98 File Offset: 0x00187E98
	public static Color ApplyPMA(Color c)
	{
		if (c.a != 1f)
		{
			c.r *= c.a;
			c.g *= c.a;
			c.b *= c.a;
		}
		return c;
	}

	// Token: 0x0400173E RID: 5950
	[SerializeField]
	private Color m_EffectColor = new Color(0f, 0f, 0f, 0.5f);

	// Token: 0x0400173F RID: 5951
	[SerializeField]
	private Vector2 m_EffectDistance = new Vector2(1f, 1f);

	// Token: 0x04001740 RID: 5952
	[SerializeField]
	private bool m_UseGraphicAlpha = true;
}
