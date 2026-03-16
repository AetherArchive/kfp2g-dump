using System;
using UnityEngine;

public class DebugCamera : MonoBehaviour
{
	private void setupFocusObject(string name)
	{
		this.focusObj = new GameObject(name);
		this.focusObj.transform.position = this.focus;
		this.focusObj.transform.LookAt(base.transform.position);
	}

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

	private void Update()
	{
		this.keyEvent();
		this.mouseEvent();
	}

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

	public void CameraReset()
	{
		this.focus = this.resetFocus;
		this.focusObj.transform.position = this.resetFocusPostion;
		base.transform.localPosition = this.resetPostion;
		this.focusObj.transform.rotation = this.resetFocusRotation;
	}

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

	private void cameraTranslate(Vector3 vec)
	{
		Transform transform = this.focusObj.transform;
		vec.x *= -1f;
		transform.Translate(Vector3.right * vec.x);
		transform.Translate(Vector3.up * vec.y);
		this.focus = transform.position;
	}

	public void cameraRotate(Vector3 eulerAngle)
	{
		Quaternion identity = Quaternion.identity;
		this.focusObj.transform.localEulerAngles += eulerAngle;
		if (this.focus != Vector3.zero)
		{
			identity.SetLookRotation(this.focus);
		}
	}

	public void modelRotate(Vector3 eulerAngle)
	{
		if (this.modelObj != null)
		{
			this.modelObj.transform.localEulerAngles += eulerAngle;
		}
	}

	[SerializeField]
	private Vector3 focus = Vector3.zero;

	[SerializeField]
	private GameObject focusObj;

	public bool showInstWindow;

	public GameObject modelObj;

	private Vector3 oldPos;

	[HideInInspector]
	public bool enableMouseButtonDownMiddle = true;

	private Vector3 resetFocus;

	private Vector3 resetFocusPostion;

	private Vector3 resetPostion;

	private Quaternion resetFocusRotation;
}
