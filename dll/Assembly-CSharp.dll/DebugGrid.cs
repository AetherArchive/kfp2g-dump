using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DebugGrid : MonoBehaviour
{
	private void Start()
	{
		this.setting = new DebugGrid.Setting();
		this.preSetting = new DebugGrid.Setting();
		base.GetComponent<MeshFilter>().mesh = (this.mesh = new Mesh());
		this.mesh = this.ReGrid(this.mesh);
	}

	private Mesh ReGrid(Mesh mesh)
	{
		if (mesh == null)
		{
			return null;
		}
		if (this.setting.back)
		{
			base.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Sprites/Default"));
		}
		else
		{
			base.GetComponent<MeshRenderer>().material = new Material(Shader.Find("GUI/Text Shader"));
		}
		mesh.Clear();
		int num = this.setting.size * 2;
		float num2 = this.setting.gridSize * (float)num / 4f;
		int num3 = (num + 2) * 2;
		float num4 = num2 / (float)num;
		Vector3[] array = new Vector3[num3];
		Vector2[] array2 = new Vector2[num3];
		int[] array3 = new int[num3];
		Color[] array4 = new Color[num3];
		Vector2 vector = new Vector2(-num2, -num2);
		Vector2 vector2 = new Vector2(num2, num2);
		for (int i = 0; i < array.Length; i += 4)
		{
			array[i] = new Vector3(vector.x + num4 * (float)i, vector.y, 0f);
			array[i + 1] = new Vector3(vector.x + num4 * (float)i, vector2.y, 0f);
			array[i + 2] = new Vector3(vector.x, vector2.y - num4 * (float)i, 0f);
			array[i + 3] = new Vector3(vector2.x, vector2.y - num4 * (float)i, 0f);
		}
		for (int j = 0; j < num3; j++)
		{
			array2[j] = Vector2.zero;
			array3[j] = j;
			array4[j] = this.setting.color;
		}
		Vector3 vector3 = Vector3.forward;
		switch (this.setting.face)
		{
		case DebugGrid.Face.xy:
			vector3 = Vector3.forward;
			break;
		case DebugGrid.Face.zx:
			vector3 = Vector3.up;
			break;
		case DebugGrid.Face.yz:
			vector3 = Vector3.right;
			break;
		default:
			vector3 = Vector3.forward;
			break;
		}
		mesh.vertices = this.RotationVertices(array, vector3);
		mesh.uv = array2;
		mesh.colors = array4;
		mesh.SetIndices(array3, MeshTopology.Lines, 0);
		this.preSetting.gridSize = this.setting.gridSize;
		this.preSetting.size = this.setting.size;
		this.preSetting.color = this.setting.color;
		this.preSetting.face = this.setting.face;
		this.preSetting.back = this.setting.back;
		return mesh;
	}

	private Vector3[] RotationVertices(Vector3[] vertices, Vector3 rotDirection)
	{
		Vector3[] array = new Vector3[vertices.Length];
		for (int i = 0; i < vertices.Length; i++)
		{
			array[i] = Quaternion.LookRotation(rotDirection) * vertices[i];
		}
		return array;
	}

	private void OnValidate()
	{
		if (this.setting.gridSize <= 0f)
		{
			this.setting.gridSize = 1E-06f;
		}
		if (this.setting.size <= 0)
		{
			this.setting.size = 1;
		}
		this.ReGrid(this.mesh);
	}

	[SerializeField]
	private DebugGrid.Setting setting;

	private DebugGrid.Setting preSetting;

	private Mesh mesh;

	public enum Face
	{
		xy,
		zx,
		yz
	}

	[Serializable]
	public class Setting
	{
		public Setting()
		{
			this.gridSize = 1f;
			this.size = 20;
			this.color = Color.white;
			this.face = DebugGrid.Face.zx;
			this.back = true;
		}

		public float gridSize;

		[SerializeField]
		[Range(1f, 80f)]
		public int size;

		[SerializeField]
		public Color color;

		public DebugGrid.Face face;

		public bool back;
	}
}
