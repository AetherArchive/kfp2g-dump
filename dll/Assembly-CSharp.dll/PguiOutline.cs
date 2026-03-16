using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PguiOutline : BaseMeshEffect
{
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

	[SerializeField]
	private Color m_EffectColor = new Color(0f, 0f, 0f, 0.5f);

	[SerializeField]
	private Vector2 m_EffectDistance = new Vector2(1f, 1f);

	[SerializeField]
	private bool m_UseGraphicAlpha = true;
}
