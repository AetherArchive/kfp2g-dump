using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000191 RID: 401
[AddComponentMenu("UI/BezierLine")]
[ExecuteInEditMode]
[RequireComponent(typeof(CanvasRenderer))]
[RequireComponent(typeof(RectTransform))]
public class BezierLine : Graphic
{
	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x06001AB4 RID: 6836 RVA: 0x00157C09 File Offset: 0x00155E09
	// (set) Token: 0x06001AB3 RID: 6835 RVA: 0x00157BF9 File Offset: 0x00155DF9
	public float Width
	{
		get
		{
			return this.m_width;
		}
		set
		{
			this.m_width = value;
			this.m_is_mesh_changed = true;
		}
	}

	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x06001AB6 RID: 6838 RVA: 0x00157C35 File Offset: 0x00155E35
	// (set) Token: 0x06001AB5 RID: 6837 RVA: 0x00157C11 File Offset: 0x00155E11
	public float MaxRate
	{
		get
		{
			return this.m_max_rate;
		}
		set
		{
			this.m_max_rate = value;
			this.m_is_mesh_changed = true;
			if (value == 0f)
			{
				this.m_endpoint = this.m_start_v;
			}
		}
	}

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x06001AB8 RID: 6840 RVA: 0x00157C4D File Offset: 0x00155E4D
	// (set) Token: 0x06001AB7 RID: 6839 RVA: 0x00157C3D File Offset: 0x00155E3D
	public float MinRate
	{
		get
		{
			return this.m_min_rate;
		}
		set
		{
			this.m_min_rate = value;
			this.m_is_mesh_changed = true;
		}
	}

	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x06001ABA RID: 6842 RVA: 0x00157C6C File Offset: 0x00155E6C
	// (set) Token: 0x06001AB9 RID: 6841 RVA: 0x00157C55 File Offset: 0x00155E55
	public int Samples
	{
		get
		{
			return this.m_samples;
		}
		set
		{
			this.m_samples = value;
			this.m_is_samples_changed = true;
			this.m_is_mesh_changed = true;
		}
	}

	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x06001ABB RID: 6843 RVA: 0x00157C74 File Offset: 0x00155E74
	public override Texture mainTexture
	{
		get
		{
			return this.m_image.GetComponent<PguiRawImageCtrl>().m_RawImage.mainTexture;
		}
	}

	// Token: 0x06001ABC RID: 6844 RVA: 0x00157C8C File Offset: 0x00155E8C
	public static Vector2 BezierCurve(float rate, Vector2 start, Vector2 controll, Vector2 end)
	{
		Vector2 vector = Vector2.Lerp(start, controll, rate);
		Vector2 vector2 = Vector2.Lerp(controll, end, rate);
		return Vector2.Lerp(Vector2.Lerp(vector, vector2, rate), end, rate);
	}

	// Token: 0x06001ABD RID: 6845 RVA: 0x00157CBC File Offset: 0x00155EBC
	public void SetSpriteUV()
	{
		Rect uvRect = this.m_image.GetComponent<PguiRawImageCtrl>().m_RawImage.uvRect;
		this.m_sprite_uv = new Vector2[4];
		this.m_sprite_uv[0] = new Vector2(uvRect.x + uvRect.width, uvRect.y);
		this.m_sprite_uv[1] = new Vector2(uvRect.x + uvRect.width, uvRect.y + uvRect.height);
		this.m_sprite_uv[2] = new Vector2(uvRect.x, uvRect.y);
		this.m_sprite_uv[3] = new Vector2(uvRect.x, uvRect.y + uvRect.height);
	}

	// Token: 0x06001ABE RID: 6846 RVA: 0x00157D88 File Offset: 0x00155F88
	private bool IsMeshChanged()
	{
		return (!this.m_is_position_fixed && (this.m_start.transform.hasChanged || this.m_controll.transform.hasChanged || this.m_end.transform.hasChanged)) || this.m_is_mesh_changed;
	}

	// Token: 0x06001ABF RID: 6847 RVA: 0x00157DDB File Offset: 0x00155FDB
	private bool IsSamplesChanged()
	{
		return this.m_is_samples_changed;
	}

	// Token: 0x06001AC0 RID: 6848 RVA: 0x00157DE3 File Offset: 0x00155FE3
	public void MeshChanged()
	{
		this.m_is_mesh_changed = true;
	}

	// Token: 0x06001AC1 RID: 6849 RVA: 0x00157DEC File Offset: 0x00155FEC
	public void SamplesChanged()
	{
		this.m_is_samples_changed = true;
	}

	// Token: 0x06001AC2 RID: 6850 RVA: 0x00157DF8 File Offset: 0x00155FF8
	private void ResetMeshChanged()
	{
		if (!this.m_is_position_fixed)
		{
			this.m_start.transform.hasChanged = false;
			this.m_controll.transform.hasChanged = false;
			this.m_end.transform.hasChanged = false;
		}
		this.m_is_mesh_changed = false;
	}

	// Token: 0x06001AC3 RID: 6851 RVA: 0x00157E48 File Offset: 0x00156048
	public void UpdateSamples()
	{
		if (!this.IsSamplesChanged())
		{
			return;
		}
		if (this.m_sprite_uv == null)
		{
			return;
		}
		this.m_triangles = new int[6 * (this.m_samples - 1)];
		for (int i = 0; i < this.m_samples - 1; i++)
		{
			int num = i * 4;
			int num2 = i * 6;
			this.m_triangles[num2] = num;
			this.m_triangles[num2 + 1] = num + 2;
			this.m_triangles[num2 + 2] = num + 1;
			this.m_triangles[num2 + 3] = num + 1;
			this.m_triangles[num2 + 4] = num + 2;
			this.m_triangles[num2 + 5] = num + 3;
		}
		this.m_uvs = new Vector2[4 * (this.m_samples - 1)];
		for (int j = 0; j < this.m_samples - 1; j++)
		{
			int num3 = j * 4;
			for (int k = 0; k < 4; k++)
			{
				this.m_uvs[num3 + k] = this.m_sprite_uv[k];
			}
		}
		this.m_vert = new Vector3[4 * (this.m_samples - 1)];
		this.m_colors = new Color[4 * (this.m_samples - 1)];
		this.m_is_samples_changed = false;
	}

	// Token: 0x06001AC4 RID: 6852 RVA: 0x00157F6C File Offset: 0x0015616C
	private void UpdateColor()
	{
		if (this.m_color_old == this.color)
		{
			return;
		}
		this.m_colors = new Color[4 * (this.m_samples - 1)];
		for (int i = 0; i < this.m_colors.Length; i++)
		{
			this.m_colors[i] = this.color;
		}
		this.m_color_old = this.color;
	}

	// Token: 0x06001AC5 RID: 6853 RVA: 0x00157FD4 File Offset: 0x001561D4
	public void UpdateMesh()
	{
		if (!this.IsMeshChanged())
		{
			return;
		}
		float num = (this.m_max_rate - this.m_min_rate) / (float)this.m_samples;
		float num2 = this.m_min_rate;
		float num3 = this.m_width / 2f;
		if (!this.m_is_position_fixed)
		{
			if (this.m_start == null || this.m_controll == null || this.m_end == null)
			{
				return;
			}
			this.m_start_v = this.m_start.GetComponent<RectTransform>().localPosition;
			this.m_end_v = this.m_end.GetComponent<RectTransform>().localPosition;
			this.m_controll_v = this.m_controll.GetComponent<RectTransform>().localPosition;
		}
		this.m_vert = new Vector3[4 * (this.m_samples - 1)];
		Vector2 vector = BezierLine.BezierCurve(num2, this.m_start_v, this.m_controll_v, this.m_end_v);
		num2 += num;
		for (int i = 1; i < this.m_samples; i++)
		{
			Vector2 vector2 = BezierLine.BezierCurve(num2, this.m_start_v, this.m_controll_v, this.m_end_v);
			Vector2 vector3 = new Vector2(vector.y - vector2.y, -(vector.x - vector2.x));
			vector3.Normalize();
			vector3 *= num3;
			int num4 = (i - 1) * 4;
			if (num4 == 0)
			{
				this.m_vert[num4] = new Vector3(vector.x - vector3.x, vector.y - vector3.y, 0f);
				this.m_vert[num4 + 1] = new Vector3(vector.x + vector3.x, vector.y + vector3.y, 0f);
			}
			else
			{
				this.m_vert[num4] = this.m_vert[num4 - 2];
				this.m_vert[num4 + 1] = this.m_vert[num4 - 1];
			}
			this.m_vert[num4 + 2] = new Vector3(vector2.x - vector3.x, vector2.y - vector3.y, 0f);
			this.m_vert[num4 + 3] = new Vector3(vector2.x + vector3.x, vector2.y + vector3.y, 0f);
			vector = vector2;
			num2 += num;
		}
		this.m_endpoint = vector;
		this.ResetMeshChanged();
		this.SetVerticesDirty();
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x0015826C File Offset: 0x0015646C
	private void Init()
	{
		this.m_is_mesh_changed = true;
		this.m_is_samples_changed = true;
		this.m_color_old = Color.black;
		this.SetSpriteUV();
		this.Update();
	}

	// Token: 0x06001AC7 RID: 6855 RVA: 0x00158293 File Offset: 0x00156493
	protected override void Start()
	{
		base.Start();
		this.Init();
	}

	// Token: 0x06001AC8 RID: 6856 RVA: 0x001582A1 File Offset: 0x001564A1
	protected override void OnEnable()
	{
		base.OnEnable();
		this.Init();
	}

	// Token: 0x06001AC9 RID: 6857 RVA: 0x001582AF File Offset: 0x001564AF
	private void Update()
	{
		this.UpdateSamples();
		this.UpdateMesh();
		this.UpdateColor();
	}

	// Token: 0x06001ACA RID: 6858 RVA: 0x001582C4 File Offset: 0x001564C4
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		base.OnPopulateMesh(vh);
		if (this.m_sprite_uv == null)
		{
			return;
		}
		vh.Clear();
		for (int i = 0; i < this.m_vert.Length; i++)
		{
			vh.AddVert(new UIVertex
			{
				position = this.m_vert[i],
				uv0 = this.m_uvs[i],
				color = this.m_colors[i]
			});
		}
		for (int j = 0; j < this.m_triangles.Length / 3; j++)
		{
			int num = j * 3;
			vh.AddTriangle(this.m_triangles[num], this.m_triangles[num + 1], this.m_triangles[num + 2]);
		}
	}

	// Token: 0x0400144E RID: 5198
	private Vector2[] m_sprite_uv;

	// Token: 0x0400144F RID: 5199
	public float m_width = 20f;

	// Token: 0x04001450 RID: 5200
	[HideInInspector]
	public float m_max_rate = 1f;

	// Token: 0x04001451 RID: 5201
	[HideInInspector]
	public float m_min_rate;

	// Token: 0x04001452 RID: 5202
	public int m_samples = 20;

	// Token: 0x04001453 RID: 5203
	public GameObject m_image;

	// Token: 0x04001454 RID: 5204
	public GameObject m_start;

	// Token: 0x04001455 RID: 5205
	public GameObject m_end;

	// Token: 0x04001456 RID: 5206
	public GameObject m_controll;

	// Token: 0x04001457 RID: 5207
	public Vector2 m_start_v;

	// Token: 0x04001458 RID: 5208
	public Vector2 m_end_v;

	// Token: 0x04001459 RID: 5209
	public Vector2 m_controll_v;

	// Token: 0x0400145A RID: 5210
	public bool m_is_position_fixed;

	// Token: 0x0400145B RID: 5211
	private Color m_color_old = Color.black;

	// Token: 0x0400145C RID: 5212
	private bool m_is_mesh_changed;

	// Token: 0x0400145D RID: 5213
	private bool m_is_samples_changed;

	// Token: 0x0400145E RID: 5214
	private int[] m_triangles;

	// Token: 0x0400145F RID: 5215
	private Vector2[] m_uvs;

	// Token: 0x04001460 RID: 5216
	private Color[] m_colors;

	// Token: 0x04001461 RID: 5217
	private Vector3[] m_vert;

	// Token: 0x04001462 RID: 5218
	public Vector2 m_endpoint;
}
