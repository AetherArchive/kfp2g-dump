using System;
using UnityEngine;

// Token: 0x020001F0 RID: 496
public class DebugCamera : MonoBehaviour
{
	// Token: 0x06002137 RID: 8503 RVA: 0x0018E027 File Offset: 0x0018C227
	private void setupFocusObject(string name)
	{
		this.focusObj = new GameObject(name);
		this.focusObj.transform.position = this.focus;
		this.focusObj.transform.LookAt(base.transform.position);
	}

	// Token: 0x06002138 RID: 8504 RVA: 0x0018E068 File Offset: 0x0018C268
	private void Start()
	{
		if (this.focusObj == null)
		{
			this.setupFocusObject("CameraFocusObject");
		}
		Transform transform = base.transform;
		transform.SetParent(this.focusObj.transform, true);
		transform.LookAt(this.focus);
		this.resetFocus = this.focus;
		this.resetFocusPostion = this.focusObj.transform.position;
		this.resetPostion = base.transform.localPosition;
		this.resetFocusRotation = this.focusObj.transform.rotation;
	}

	// Token: 0x06002139 RID: 8505 RVA: 0x0018E0FA File Offset: 0x0018C2FA
	private void Update()
	{
		this.keyEvent();
		this.mouseEvent();
	}

	// Token: 0x0600213A RID: 8506 RVA: 0x0018E108 File Offset: 0x0018C308
	private void OnGUI()
	{
		if (this.showInstWindow)
		{
			GUI.Box(new Rect((float)(Screen.width - 310), (float)(Screen.height - 180), 300f, 160f), "Camera Operations");
			GUI.Label(new Rect((float)(Screen.width - 300), (float)(Screen.height - 160), 300f, 30f), "Iキー / ヘルプ表示非表示");
			GUI.Label(new Rect((float)(Screen.width - 300), (float)(Screen.height - 140), 300f, 30f), "ホイールドラッグ / 平行移動");
			GUI.Label(new Rect((float)(Screen.width - 300), (float)(Screen.height - 120), 300f, 30f), "ホイール回転     / 拡大縮小");
			GUI.Label(new Rect((float)(Screen.width - 300), (float)(Screen.height - 100), 300f, 30f), "右ドラッグ\u3000\u3000\u3000 / 回転");
			GUI.Label(new Rect((float)(Screen.width - 300), (float)(Screen.height - 80), 300f, 30f), "Rキー\u3000\u3000\u3000 / カメラリセット");
			if (this.modelObj != null)
			{
				GUI.Label(new Rect((float)(Screen.width - 300), (float)(Screen.height - 60), 400f, 30f), "L Shift + 右ドラッグ / モデル横回転");
			}
			if (this.modelObj != null)
			{
				GUI.Label(new Rect((float)(Screen.width - 300), (float)(Screen.height - 40), 400f, 30f), "R Shift + 右ドラッグ / モデル縦回転");
			}
		}
	}

	// Token: 0x0600213B RID: 8507 RVA: 0x0018E2B5 File Offset: 0x0018C4B5
	private void keyEvent()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			this.CameraReset();
		}
		if (Input.GetKeyDown(KeyCode.I))
		{
			this.showInstWindow = !this.showInstWindow;
		}
	}

	// Token: 0x0600213C RID: 8508 RVA: 0x0018E2E0 File Offset: 0x0018C4E0
	public void CameraReset()
	{
		this.focus = this.resetFocus;
		this.focusObj.transform.position = this.resetFocusPostion;
		base.transform.localPosition = this.resetPostion;
		this.focusObj.transform.rotation = this.resetFocusRotation;
	}

	// Token: 0x0600213D RID: 8509 RVA: 0x0018E338 File Offset: 0x0018C538
	private void mouseEvent()
	{
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis != 0f)
		{
			this.mouseWheelEvent(axis);
		}
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(1))
		{
			this.oldPos = Input.mousePosition;
		}
		this.mouseDragEvent(Input.mousePosition);
	}

	// Token: 0x0600213E RID: 8510 RVA: 0x0018E390 File Offset: 0x0018C590
	private void mouseDragEvent(Vector3 mousePos)
	{
		Vector3 vector = mousePos - this.oldPos;
		if (Input.GetMouseButton(0))
		{
			if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftMeta))
			{
				if (vector.magnitude > 1E-05f)
				{
					this.cameraTranslate(-vector / 100f);
				}
			}
			else if (Input.GetKey(KeyCode.LeftAlt) && vector.magnitude > 1E-05f)
			{
				this.cameraRotate(new Vector3(vector.y, vector.x, 0f));
			}
		}
		else if (Input.GetMouseButton(2))
		{
			if (this.enableMouseButtonDownMiddle && vector.magnitude > 1E-05f)
			{
				this.cameraTranslate(-vector / 100f);
			}
		}
		else if (Input.GetMouseButton(1))
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				this.modelRotate(new Vector3(0f, vector.x, 0f));
			}
			else if (Input.GetKey(KeyCode.RightShift))
			{
				this.modelRotate(new Vector3(vector.y, 0f, 0f));
			}
			else if (vector.magnitude > 1E-05f)
			{
				this.cameraRotate(new Vector3(vector.y, vector.x, 0f));
			}
		}
		this.oldPos = mousePos;
	}

	// Token: 0x0600213F RID: 8511 RVA: 0x0018E504 File Offset: 0x0018C704
	public void mouseWheelEvent(float delta)
	{
		if (!this.enableMouseButtonDownMiddle)
		{
			return;
		}
		Vector3 vector = (base.transform.position - this.focus) * (1f + delta);
		if ((double)vector.magnitude > 0.01)
		{
			base.transform.position = this.focus + vector;
		}
	}

	// Token: 0x06002140 RID: 8512 RVA: 0x0018E568 File Offset: 0x0018C768
	private void cameraTranslate(Vector3 vec)
	{
		Transform transform = this.focusObj.transform;
		vec.x *= -1f;
		transform.Translate(Vector3.right * vec.x);
		transform.Translate(Vector3.up * vec.y);
		this.focus = transform.position;
	}

	// Token: 0x06002141 RID: 8513 RVA: 0x0018E5CC File Offset: 0x0018C7CC
	public void cameraRotate(Vector3 eulerAngle)
	{
		Quaternion identity = Quaternion.identity;
		this.focusObj.transform.localEulerAngles += eulerAngle;
		if (this.focus != Vector3.zero)
		{
			identity.SetLookRotation(this.focus);
		}
	}

	// Token: 0x06002142 RID: 8514 RVA: 0x0018E61A File Offset: 0x0018C81A
	public void modelRotate(Vector3 eulerAngle)
	{
		if (this.modelObj != null)
		{
			this.modelObj.transform.localEulerAngles += eulerAngle;
		}
	}

	// Token: 0x040017DE RID: 6110
	[SerializeField]
	private Vector3 focus = Vector3.zero;

	// Token: 0x040017DF RID: 6111
	[SerializeField]
	private GameObject focusObj;

	// Token: 0x040017E0 RID: 6112
	public bool showInstWindow;

	// Token: 0x040017E1 RID: 6113
	public GameObject modelObj;

	// Token: 0x040017E2 RID: 6114
	private Vector3 oldPos;

	// Token: 0x040017E3 RID: 6115
	[HideInInspector]
	public bool enableMouseButtonDownMiddle = true;

	// Token: 0x040017E4 RID: 6116
	private Vector3 resetFocus;

	// Token: 0x040017E5 RID: 6117
	private Vector3 resetFocusPostion;

	// Token: 0x040017E6 RID: 6118
	private Vector3 resetPostion;

	// Token: 0x040017E7 RID: 6119
	private Quaternion resetFocusRotation;
}
