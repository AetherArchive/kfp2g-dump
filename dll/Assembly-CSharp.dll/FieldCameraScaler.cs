using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FieldCameraScaler : MonoBehaviour
{
	public Camera fieldCamera
	{
		get
		{
			if (this.cam == null)
			{
				this.cam = base.GetComponent<Camera>();
			}
			return this.cam;
		}
	}

	public float fieldOfView
	{
		get
		{
			if (this.fov >= 0f)
			{
				return this.fov;
			}
			return this.fieldCamera.fieldOfView;
		}
		set
		{
			this.fov = value;
			if (!this.fieldCamera.orthographic)
			{
				if ((this.rct.size.x > 0f && this.rct.size.y > 0f) || (this.rct.position.x < 0f && this.rct.position.y < 0f))
				{
					this.fieldCamera.fieldOfView = this.fov;
					this.fieldCamera.ResetProjectionMatrix();
					return;
				}
				this.ApplyFieldCamera();
				this.fieldCamera.fieldOfView = Mathf.Atan(Mathf.Tan(0.017453292f * this.fov * 0.5f) * this.ratio) * 57.29578f * 2f;
				this.fieldCamera.ResetProjectionMatrix();
				Matrix4x4 projectionMatrix = this.fieldCamera.projectionMatrix;
				projectionMatrix.m02 = this.off.x;
				projectionMatrix.m12 = this.off.y;
				this.fieldCamera.projectionMatrix = projectionMatrix;
			}
		}
	}

	public float orthographicSize
	{
		get
		{
			if (this.ogs >= 0f)
			{
				return this.ogs;
			}
			return this.fieldCamera.orthographicSize;
		}
		set
		{
			this.ogs = value;
			if (this.fieldCamera.orthographic)
			{
				this.ApplyFieldCamera();
				this.fieldCamera.orthographicSize = this.ogs / this.siz.y;
			}
		}
	}

	public Vector3 localPosition
	{
		get
		{
			Vector3 localPosition = base.transform.localPosition;
			if (this.lpy > -99990f)
			{
				localPosition.y = this.lpy;
			}
			return localPosition;
		}
		set
		{
			Vector3 vector = value;
			this.lpy = vector.y;
			if (this.fieldCamera.orthographic)
			{
				this.ApplyFieldCamera();
				vector.x = this.localPositionOrg.x - this.lpxFix;
				vector.y -= this.ogs * (this.pos.y + this.pos.y + this.siz.y - 1f);
				base.transform.localPosition = vector;
			}
		}
	}

	public Rect rect
	{
		get
		{
			return this.rct;
		}
		set
		{
			this.rct = value;
			if (this.rct.size.x > 0f && this.rct.size.y > 0f)
			{
				this.ApplyFieldCamera();
				this.fieldCamera.rect = new Rect(this.pos.x + this.rct.position.x * this.siz.x, this.pos.y + this.rct.position.y * this.siz.y, this.rct.size.x * this.siz.x, this.rct.size.y * this.siz.y);
			}
			else
			{
				this.fieldCamera.rect = new Rect(0f, 0f, 1f, 1f);
			}
			this.fieldOfView = this.fieldOfView;
			this.orthographicSize = this.orthographicSize;
			this.localPosition = this.localPosition;
		}
	}

	private void ApplyFieldCamera()
	{
		if (SafeAreaScaler.ScreenWidth == 0 || SafeAreaScaler.ScreenHeight == 0)
		{
			return;
		}
		this.width = SafeAreaScaler.ScreenWidth;
		this.height = SafeAreaScaler.ScreenHeight;
		float num = FieldCameraScaler.defaultWidth;
		float num2 = FieldCameraScaler.defaultHeight;
		if (SceneHome.nowVertView)
		{
			num = FieldCameraScaler.defaultHeight;
			num2 = FieldCameraScaler.defaultWidth;
		}
		this.og = this.fieldCamera.orthographic;
		Rect safeArea = SafeAreaScaler.GetSafeArea();
		float num3 = safeArea.size.x / num;
		float num4 = safeArea.size.y / num2;
		if ((SceneHome.nowVertView || num3 - num4 >= 1f) && (!SceneHome.nowVertView || num3 - num4 <= 1f))
		{
			if (!SceneHome.nowVertView)
			{
				num = FieldCameraScaler.defaultMaxWidth;
			}
			else
			{
				num2 = FieldCameraScaler.defaultMaxWidth;
			}
			num3 = safeArea.size.x / num;
			num4 = safeArea.size.y / num2;
			if (!SceneHome.nowVertView && num3 - num4 < 1f)
			{
				num = (float)SafeAreaScaler.ScreenWidth / (float)SafeAreaScaler.ScreenHeight * FieldCameraScaler.defaultHeight;
				num3 = safeArea.size.x / num;
			}
			else if (SceneHome.nowVertView && num3 - num4 > 1f)
			{
				num2 = (float)SafeAreaScaler.ScreenWidth / (float)SafeAreaScaler.ScreenHeight * FieldCameraScaler.defaultHeight;
				num4 = safeArea.size.y / num2;
			}
		}
		float num5 = ((num3 > num4) ? num4 : num3);
		float num6 = num5 * num;
		float num7 = num5 * num2;
		this.ratio = (float)this.height / num5 / num2;
		this.off = new Vector2(((float)this.width - safeArea.position.x - safeArea.size.x - safeArea.position.x) / (float)this.width, ((float)this.height - safeArea.position.y - safeArea.size.y - safeArea.position.y) / (float)this.height);
		this.pos = new Vector2((safeArea.position.x + (safeArea.size.x - num6) * 0.5f) / (float)this.width, (safeArea.position.y + (safeArea.size.y - num7) * 0.5f) / (float)this.height);
		this.siz = new Vector2(num6 / (float)this.width, num7 / (float)this.height);
		float x = safeArea.x;
		float num8 = (float)this.width - safeArea.x - safeArea.size.x;
		this.lpxFix = 0f;
		if (x != num8)
		{
			Vector3 vector = this.fieldCamera.ScreenToWorldPoint(new Vector2((float)this.width, 0f));
			this.lpxFix = (x - num8) / (float)this.width * vector.x;
		}
		this.scrW = Screen.width;
		this.scrH = Screen.height;
		this.m_SafeArea = safeArea;
	}

	private void Awake()
	{
		this.ApplyFieldCamera();
		this.fieldOfView = this.fieldOfView;
		this.orthographicSize = this.orthographicSize;
		this.localPosition = this.localPosition;
	}

	private void Update()
	{
		if (this.width != SafeAreaScaler.ScreenWidth || this.height != SafeAreaScaler.ScreenHeight || this.og != this.fieldCamera.orthographic || this.scrW != Screen.width || this.scrH != Screen.height || this.m_SafeArea != SafeAreaScaler.GetSafeArea())
		{
			this.ApplyFieldCamera();
			this.rect = this.rct;
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			return;
		}
		this.width = (this.height = 0);
		this.scrW = (this.scrH = 0);
	}

	public Vector4 GetRect()
	{
		if (this.rct.size.x > 0f && this.rct.size.y > 0f)
		{
			return new Vector4(0f, 0f, 1f, 1f);
		}
		return new Vector4(this.pos.x, this.pos.y, this.siz.x, this.siz.y);
	}

	private static readonly float defaultWidth = 1280f;

	private static readonly float defaultMaxWidth = 1560f;

	private static readonly float defaultHeight = 720f;

	private Camera cam;

	private float fov = -1f;

	private float ogs = -1f;

	private float lpxFix;

	private float lpy = -99999f;

	private Rect rct = Rect.zero;

	private Vector3 localPositionOrg;

	private float ratio = 1f;

	private Vector2 off = Vector2.zero;

	private Vector2 pos = Vector2.zero;

	private Vector2 siz = Vector2.one;

	private int width;

	private int height;

	private bool og;

	private int scrW;

	private int scrH;

	private Rect m_SafeArea;
}
