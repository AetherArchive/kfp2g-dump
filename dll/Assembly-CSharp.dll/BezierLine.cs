using System;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/BezierLine")]
[ExecuteInEditMode]
[RequireComponent(typeof(CanvasRenderer))]
[RequireComponent(typeof(RectTransform))]
public class BezierLine : Graphic
{
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

	public override Texture mainTexture
	{
		get
		{
			return this.m_image.GetComponent<PguiRawImageCtrl>().m_RawImage.mainTexture;
		}
	}

	public static Vector2 BezierCurve(float rate, Vector2 start, Vector2 controll, Vector2 end)
	{
		Vector2 vector = Vector2.Lerp(start, controll, rate);
		Vector2 vector2 = Vector2.Lerp(controll, end, rate);
		return Vector2.Lerp(Vector2.Lerp(vector, vector2, rate), end, rate);
	}

	public void SetSpriteUV()
	{
		Rect uvRect = this.m_image.GetComponent<PguiRawImageCtrl>().m_RawImage.uvRect;
		this.m_sprite_uv = new Vector2[4];
		this.m_sprite_uv[0] = new Vector2(uvRect.x + uvRect.width, uvRect.y);
		this.m_sprite_uv[1] = new Vector2(uvRect.x + uvRect.width, uvRect.y + uvRect.height);
		this.m_sprite_uv[2] = new Vector2(uvRect.x, uvRect.y);
		this.m_sprite_uv[3] = new Vector2(uvRect.x, uvRect.y + uvRect.height);
	}

	private bool IsMeshChanged()
	{
		return (!this.m_is_position_fixed && (this.m_start.transform.hasChanged || this.m_controll.transform.hasChanged || this.m_end.transform.hasChanged)) || this.m_is_mesh_changed;
	}

	private bool IsSamplesChanged()
	{
		return this.m_is_samples_changed;
	}

	public void MeshChanged()
	{
		this.m_is_mesh_changed = true;
	}

	public void SamplesChanged()
	{
		this.m_is_samples_changed = true;
	}

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

	private void Init()
	{
		this.m_is_mesh_changed = true;
		this.m_is_samples_changed = true;
		this.m_color_old = Color.black;
		this.SetSpriteUV();
		this.Update();
	}

	protected override void Start()
	{
		base.Start();
		this.Init();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		this.Init();
	}

	private void Update()
	{
		this.UpdateSamples();
		this.UpdateMesh();
		this.UpdateColor();
	}

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

	private Vector2[] m_sprite_uv;

	public float m_width = 20f;

	[HideInInspector]
	public float m_max_rate = 1f;

	[HideInInspector]
	public float m_min_rate;

	public int m_samples = 20;

	public GameObject m_image;

	public GameObject m_start;

	public GameObject m_end;

	public GameObject m_controll;

	public Vector2 m_start_v;

	public Vector2 m_end_v;

	public Vector2 m_controll_v;

	public bool m_is_position_fixed;

	private Color m_color_old = Color.black;

	private bool m_is_mesh_changed;

	private bool m_is_samples_changed;

	private int[] m_triangles;

	private Vector2[] m_uvs;

	private Color[] m_colors;

	private Vector3[] m_vert;

	public Vector2 m_endpoint;
}
