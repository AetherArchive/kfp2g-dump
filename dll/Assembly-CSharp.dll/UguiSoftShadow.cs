using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001FB RID: 507
public class UguiSoftShadow : Shadow
{
	// Token: 0x06002169 RID: 8553 RVA: 0x0018F3F8 File Offset: 0x0018D5F8
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

	// Token: 0x0600216A RID: 8554 RVA: 0x0018F430 File Offset: 0x0018D630
	private void ModifyVertices(List<UIVertex> verts)
	{
		int num = verts.Count * 5;
		if (verts.Capacity < num)
		{
			verts.Capacity = num;
		}
		int num2 = 0;
		int num3 = verts.Count;
		for (int i = 0; i < this._OutLineResolution; i++)
		{
			int num4 = 0;
			while ((float)num4 < this._BlurResolution)
			{
				float num5 = this._BlurResolution - (float)(num4 / 1);
				int num6 = Mathf.RoundToInt(255f - 255f / num5);
				float num7 = Mathf.Sin(6.2831855f * (float)i / (float)this._OutLineResolution) * this._OutlineWidth * (float)num4 + base.effectDistance.x;
				float num8 = Mathf.Cos(6.2831855f * (float)i / (float)this._OutLineResolution) * this._OutlineWidth * (float)num4 + base.effectDistance.y;
				base.ApplyShadowZeroAlloc(verts, base.effectColor * new Color(1f, 1f, 1f, (float)num6 * this.BlurContrast / 255f), num2, verts.Count, num7, num8);
				num2 = num3;
				num3 = verts.Count;
				num4++;
			}
		}
	}

	// Token: 0x0400180D RID: 6157
	public float _OutlineWidth = 8f;

	// Token: 0x0400180E RID: 6158
	[SerializeField]
	[Range(0f, 12f)]
	public int _OutLineResolution = 12;

	// Token: 0x0400180F RID: 6159
	[SerializeField]
	[Range(0f, 6f)]
	public float _BlurResolution = 4f;

	// Token: 0x04001810 RID: 6160
	[SerializeField]
	[Range(0f, 1f)]
	public float BlurContrast = 0.3f;
}
