using System;
using UnityEngine;

// Token: 0x020000F1 RID: 241
[RequireComponent(typeof(Camera))]
public class FieldCameraScaler : MonoBehaviour
{
	// Token: 0x170002FD RID: 765
	// (get) Token: 0x06000B9C RID: 2972 RVA: 0x00044731 File Offset: 0x00042931
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

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x06000B9D RID: 2973 RVA: 0x00044753 File Offset: 0x00042953
	// (set) Token: 0x06000B9E RID: 2974 RVA: 0x00044774 File Offset: 0x00042974
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

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x06000B9F RID: 2975 RVA: 0x00044898 File Offset: 0x00042A98
	// (set) Token: 0x06000BA0 RID: 2976 RVA: 0x000448B9 File Offset: 0x00042AB9
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

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x000448F4 File Offset: 0x00042AF4
	// (set) Token: 0x06000BA2 RID: 2978 RVA: 0x00044928 File Offset: 0x00042B28
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

	// Token: 0x17000301 RID: 769
	// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x000449B6 File Offset: 0x00042BB6
	// (set) Token: 0x06000BA4 RID: 2980 RVA: 0x000449C0 File Offset: 0x00042BC0
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

	// Token: 0x06000BA5 RID: 2981 RVA: 0x00044AF0 File Offset: 0x00042CF0
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

	// Token: 0x06000BA6 RID: 2982 RVA: 0x00044DF4 File Offset: 0x00042FF4
	private void Awake()
	{
		this.ApplyFieldCamera();
		this.fieldOfView = this.fieldOfView;
		this.orthographicSize = this.orthographicSize;
		this.localPosition = this.localPosition;
	}

	// Token: 0x06000BA7 RID: 2983 RVA: 0x00044E20 File Offset: 0x00043020
	private void Update()
	{
		if (this.width != SafeAreaScaler.ScreenWidth || this.height != SafeAreaScaler.ScreenHeight || this.og != this.fieldCamera.orthographic || this.scrW != Screen.width || this.scrH != Screen.height || this.m_SafeArea != SafeAreaScaler.GetSafeArea())
		{
			this.ApplyFieldCamera();
			this.rect = this.rct;
		}
	}

	// Token: 0x06000BA8 RID: 2984 RVA: 0x00044E98 File Offset: 0x00043098
	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			return;
		}
		this.width = (this.height = 0);
		this.scrW = (this.scrH = 0);
	}

	// Token: 0x06000BA9 RID: 2985 RVA: 0x00044ECC File Offset: 0x000430CC
	public Vector4 GetRect()
	{
		if (this.rct.size.x > 0f && this.rct.size.y > 0f)
		{
			return new Vector4(0f, 0f, 1f, 1f);
		}
		return new Vector4(this.pos.x, this.pos.y, this.siz.x, this.siz.y);
	}

	// Token: 0x04000911 RID: 2321
	private static readonly float defaultWidth = 1280f;

	// Token: 0x04000912 RID: 2322
	private static readonly float defaultMaxWidth = 1560f;

	// Token: 0x04000913 RID: 2323
	private static readonly float defaultHeight = 720f;

	// Token: 0x04000914 RID: 2324
	private Camera cam;

	// Token: 0x04000915 RID: 2325
	private float fov = -1f;

	// Token: 0x04000916 RID: 2326
	private float ogs = -1f;

	// Token: 0x04000917 RID: 2327
	private float lpxFix;

	// Token: 0x04000918 RID: 2328
	private float lpy = -99999f;

	// Token: 0x04000919 RID: 2329
	private Rect rct = Rect.zero;

	// Token: 0x0400091A RID: 2330
	private Vector3 localPositionOrg;

	// Token: 0x0400091B RID: 2331
	private float ratio = 1f;

	// Token: 0x0400091C RID: 2332
	private Vector2 off = Vector2.zero;

	// Token: 0x0400091D RID: 2333
	private Vector2 pos = Vector2.zero;

	// Token: 0x0400091E RID: 2334
	private Vector2 siz = Vector2.one;

	// Token: 0x0400091F RID: 2335
	private int width;

	// Token: 0x04000920 RID: 2336
	private int height;

	// Token: 0x04000921 RID: 2337
	private bool og;

	// Token: 0x04000922 RID: 2338
	private int scrW;

	// Token: 0x04000923 RID: 2339
	private int scrH;

	// Token: 0x04000924 RID: 2340
	private Rect m_SafeArea;
}
