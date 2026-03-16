using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialMaskCtrl : MonoBehaviour
{
	private void Awake()
	{
		this.maskImage.material = new Material(this.maskShader);
		this.idClicpLT = new List<int>
		{
			Shader.PropertyToID("_ClipLT0"),
			Shader.PropertyToID("_ClipLT1")
		};
		this.idClicpRT = new List<int>
		{
			Shader.PropertyToID("_ClipRT0"),
			Shader.PropertyToID("_ClipRT1")
		};
		this.idClicpLB = new List<int>
		{
			Shader.PropertyToID("_ClipLB0"),
			Shader.PropertyToID("_ClipLB1")
		};
		this.idClicpRB = new List<int>
		{
			Shader.PropertyToID("_ClipRB0"),
			Shader.PropertyToID("_ClipRB1")
		};
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < this.frameCollider.Count; i++)
		{
			list.Add(this.frameCollider[i].gameObject);
		}
		list.Add(this.maskImage.gameObject);
		this.maskImage.GetComponent<Graphic>().raycastTarget = true;
		for (int j = 0; j < this.frameCollider.Count; j++)
		{
			this.frameCollider[j].GetComponent<TutorialEventTrigger>().maskObjectList = list;
		}
		this.maskRT = new List<RectTransform>();
		this.maskScale = new List<Vector2>();
		this.maskCam = new List<FieldCameraScaler>();
		for (int k = 0; k < this.maskRect.Count; k++)
		{
			this.maskRT.Add(null);
			this.maskScale.Add(Vector2.one);
			this.maskCam.Add(null);
			this.maskBaseRect.Add(Rect.zero);
		}
		this.serifRT = Object.Instantiate<GameObject>(this.TutorialSerifPrefab, base.transform).transform as RectTransform;
		this.stickRT = Object.Instantiate<GameObject>(this.TutorialStickPrefab, base.transform).transform as RectTransform;
		this.serifRT.gameObject.SetActive(false);
		this.stickRT.gameObject.SetActive(false);
		Graphic[] array = this.serifRT.GetComponentsInChildren<Graphic>(true);
		for (int l = 0; l < array.Length; l++)
		{
			array[l].raycastTarget = false;
		}
		array = this.stickRT.GetComponentsInChildren<Graphic>(true);
		for (int l = 0; l < array.Length; l++)
		{
			array[l].raycastTarget = false;
		}
		this.tutorialSerifAnime = this.serifRT.GetComponent<SimpleAnimation>();
		this.infoCharaImage = this.serifRT.Find("Chara/Icon_Chara").GetComponent<PguiRawImageCtrl>();
		this.infoMessageText = this.serifRT.Find("SerifWindow/Txt_Serif").GetComponent<PguiTextCtrl>();
		this.touchNextObj = this.serifRT.Find("SerifWindow/Txt_Touch").gameObject;
		this.maskImage.gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			this.OnTouchMask(this.maskImage.transform);
		}, null, null, null, null);
		this.outFrame = new List<RawImage>();
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name.StartsWith("OutFrame"))
			{
				this.outFrame.Add(transform.GetComponent<RawImage>());
			}
		}
		foreach (RawImage rawImage in this.outFrame)
		{
			rawImage.gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
			{
				this.OnTouchMask(this.maskImage.transform);
			}, null, null, null, null);
		}
		this.isInit = true;
		this.m_SafeArea = SafeAreaScaler.GetSafeArea();
	}

	private void Update()
	{
		if (this.m_SafeArea != SafeAreaScaler.GetSafeArea())
		{
			base.StartCoroutine(this.UpdateMask());
			base.StartCoroutine(this.UpdateStick());
			this.m_SafeArea = SafeAreaScaler.GetSafeArea();
		}
	}

	private IEnumerator UpdateMask()
	{
		int num;
		for (int index = 0; index < this.maskRect.Count; index = num + 1)
		{
			if (this.frameCollider[index].gameObject.activeSelf)
			{
				Transform oldParent = this.frameCollider[index].transform.parent;
				Transform newParent = ((this.maskRT[index] != null && this.maskRT[index].GetComponentInParent<SafeAreaScaler>() != null) ? this.maskRT[index].GetComponentInParent<SafeAreaScaler>().transform : null);
				while ((oldParent.GetComponent<SafeAreaScaler>() != null && oldParent.GetComponent<SafeAreaScaler>().applySafeArea != SafeAreaScaler.GetSafeArea()) || (newParent != null && newParent.GetComponent<SafeAreaScaler>() != null && newParent.GetComponent<SafeAreaScaler>().applySafeArea != SafeAreaScaler.GetSafeArea()))
				{
					yield return null;
				}
				Rect frameCollRect = default(Rect);
				Rect newParentRect = default(Rect);
				while (frameCollRect != this.frameCollider[index].GetComponent<RectTransform>().rect || (newParent != null && newParentRect != newParent.GetComponent<RectTransform>().rect))
				{
					this.SetFrame(index);
					frameCollRect = this.frameCollider[index].GetComponent<RectTransform>().rect;
					if (newParent != null)
					{
						newParentRect = newParent.GetComponent<RectTransform>().rect;
					}
					yield return null;
				}
				oldParent = null;
				newParent = null;
				frameCollRect = default(Rect);
				newParentRect = default(Rect);
			}
			num = index;
		}
		yield break;
	}

	private IEnumerator UpdateStick()
	{
		if (this.stickRT.gameObject.activeSelf)
		{
			Transform oldParent = this.stickRT.transform.parent;
			Transform newParent = ((this.stickParentRT != null && this.stickParentRT.GetComponentInParent<SafeAreaScaler>() != null) ? this.stickParentRT.GetComponentInParent<SafeAreaScaler>().transform : null);
			while ((oldParent.GetComponent<SafeAreaScaler>() != null && oldParent.GetComponent<SafeAreaScaler>().applySafeArea != SafeAreaScaler.GetSafeArea()) || (newParent != null && newParent.GetComponent<SafeAreaScaler>() != null && newParent.GetComponent<SafeAreaScaler>().applySafeArea != SafeAreaScaler.GetSafeArea()))
			{
				yield return null;
			}
			Rect stickRect = default(Rect);
			Rect newParentRect = default(Rect);
			while (stickRect != this.stickRT.GetComponent<RectTransform>().rect || (newParent != null && newParentRect != newParent.GetComponent<RectTransform>().rect))
			{
				this.SetStickCursor();
				this.SetStickCursor(true, this.stickPosition, this.stickAngle, this.stickOffset);
				stickRect = this.stickRT.GetComponent<RectTransform>().rect;
				if (newParent != null)
				{
					newParentRect = newParent.GetComponent<RectTransform>().rect;
				}
				yield return null;
			}
			oldParent = null;
			newParent = null;
			stickRect = default(Rect);
			newParentRect = default(Rect);
		}
		yield break;
	}

	private void SetFrame(int index)
	{
		if (this.maskRT[index] != null)
		{
			Transform parent = this.frameCollider[index].transform.parent;
			Transform transform = this.maskRT[index].transform;
			this.frameCollider[index].transform.SetParent(transform, true);
			(this.frameCollider[index].transform as RectTransform).anchoredPosition = Vector2.zero;
			Vector2 vector = Vector2.zero;
			while (!(transform == null) && !(transform.GetComponent<SafeAreaScaler>() != null) && !(transform.GetComponent<Canvas>() != null))
			{
				transform = transform.parent;
			}
			this.frameCollider[index].transform.SetParent(transform, true);
			vector = (this.frameCollider[index].transform as RectTransform).anchoredPosition;
			this.frameCollider[index].transform.SetParent(parent, true);
			Rect rect = this.maskRT[index].rect;
			Vector2 vector2 = this.maskScale[index];
			this.maskRect[index] = new Rect((vector2.x < 0f) ? (vector.x - rect.width * Math.Abs(vector2.x)) : vector.x, (vector2.y < 0f) ? (vector.y - rect.height * Math.Abs(vector2.y)) : vector.y, rect.width * Math.Abs(vector2.x), rect.height * Math.Abs(vector2.y));
			this.serifRT.SetAsLastSibling();
			this.stickRT.SetAsLastSibling();
		}
		else if (this.maskCam[index] == null)
		{
			Rect rect2 = this.maskBaseRect[index];
			this.maskRect[index] = new Rect(rect2.x + base.GetComponent<RectTransform>().offsetMax.x, rect2.y, rect2.width, rect2.height);
		}
		Rect safeArea = SafeAreaScaler.GetSafeArea();
		float num = safeArea.width;
		if (SafeAreaScaler.IsLongDevice())
		{
			num = (((float)SafeAreaScaler.ScreenHeight / 9f * 19.5f >= safeArea.size.x) ? safeArea.size.x : ((float)SafeAreaScaler.ScreenHeight / 9f * 19.5f));
		}
		float num2 = 1280f;
		float num3 = 720f;
		float num4 = 0f;
		if ((float)SafeAreaScaler.ScreenWidth - num > 1f && SafeAreaScaler.CompareToFloat((float)SafeAreaScaler.ScreenWidth / (float)SafeAreaScaler.ScreenHeight, 1.7777778f) >= 0)
		{
			num2 = num * num3 / safeArea.size.y;
			if (this.maskCam[index] != null)
			{
				num4 = this.maskCam[index].GetRect().x;
			}
		}
		float num5 = this.maskRect[index].x / num2 + 0.5f * num4;
		this.maskImage.material.SetFloat(this.idClicpLT[index], num5);
		this.maskImage.material.SetFloat(this.idClicpRT[index], num5 + this.maskRect[index].width / num2);
		float num6 = this.maskRect[index].y / num3;
		this.maskImage.material.SetFloat(this.idClicpLB[index], num6);
		this.maskImage.material.SetFloat(this.idClicpRB[index], num6 + this.maskRect[index].height / num3);
		RectTransform rectTransform = this.frameCollider[index].transform as RectTransform;
		if (index == 0 && this.maskRT[index] == CanvasManager.HdlSelCharaDeck.GetCharaDeckPhotoRadioBtnRectTransform())
		{
			rectTransform.anchoredPosition = new Vector2(num5 * num2 + 6f, num6 * num3 + 4f);
			rectTransform.sizeDelta = new Vector2(this.maskRect[index].width * 0.92f, this.maskRect[index].height * 0.92f);
			return;
		}
		rectTransform.anchoredPosition = new Vector2(num5 * num2, num6 * num3);
		rectTransform.sizeDelta = new Vector2(this.maskRect[index].width, this.maskRect[index].height);
	}

	public void OnTouchMask(Transform tr)
	{
		if (this.charaInfoParameter != null && this.charaInfoParameter.messageList.Count > 0 && this.touchNextObj.activeSelf && TimeManager.Now.Ticks > this.invalidMessageSkipTick)
		{
			this.messageStep++;
			if (this.charaInfoParameter.messageList.Count > this.messageStep)
			{
				this.infoMessageText.text = this.charaInfoParameter.messageList[this.messageStep];
				this.invalidMessageSkipTick = TimeManager.Now.Ticks + 2000000L;
				if (this.charaInfoParameter.messageList.Count == this.messageStep + 1 && !this.charaInfoParameter.enableTouchNext)
				{
					this.touchNextObj.SetActive(false);
					if (this.charaInfoParameter.finishCallBack != null)
					{
						this.charaInfoParameter.finishCallBack();
					}
					this.charaInfoParameter = null;
				}
			}
			else
			{
				if (this.charaInfoParameter.finishCallBack != null)
				{
					this.charaInfoParameter.finishCallBack();
				}
				this.charaInfoParameter = null;
			}
			SoundManager.Play("prd_se_click", false, false);
		}
	}

	public void SetEnable(bool e)
	{
		this.SetMaskEnable(true);
		base.gameObject.SetActive(e);
	}

	public void SetFrame(int index, bool isDisp, Rect? rect = null, bool isCollThrough = true, FieldCameraScaler cam = null)
	{
		if (index < 0 || index >= this.maskRect.Count)
		{
			return;
		}
		if (!isDisp || rect == null)
		{
			rect = new Rect?(Rect.zero);
		}
		this.maskBaseRect[index] = rect.Value;
		float num = ((cam == null) ? base.GetComponent<RectTransform>().offsetMax.x : 0f);
		this.maskRect[index] = new Rect(rect.Value.x + num, rect.Value.y, rect.Value.width, rect.Value.height);
		this.maskRT[index] = null;
		this.maskScale[index] = Vector2.one;
		this.maskCam[index] = cam;
		if (this.maskRect[index].height > 0f)
		{
			this.frameCollider[index].gameObject.SetActive(true);
			this.frameCollider[index].raycastTarget = isCollThrough;
			this.SetFrame(index);
		}
		else
		{
			this.frameCollider[index].gameObject.SetActive(false);
			this.maskImage.material.SetFloat(this.idClicpLT[index], 0f);
			this.maskImage.material.SetFloat(this.idClicpRT[index], 0f);
			this.maskImage.material.SetFloat(this.idClicpLB[index], 0f);
			this.maskImage.material.SetFloat(this.idClicpRB[index], 0f);
		}
		if (index == 0 && isDisp)
		{
			SoundManager.Play("prd_se_tutorial_emphasis", false, false);
		}
	}

	public void SetFrame(int index, RectTransform rectTransform, bool isCollThrough, float scaleX = 1f, float scaleY = 1f)
	{
		if (index < 0 || index >= this.maskRect.Count)
		{
			return;
		}
		this.maskBaseRect[index] = Rect.zero;
		this.maskRect[index] = Rect.zero;
		this.maskRT[index] = rectTransform;
		this.maskScale[index] = new Vector2(scaleX, scaleY);
		this.maskCam[index] = null;
		this.frameCollider[index].gameObject.SetActive(true);
		this.frameCollider[index].raycastTarget = isCollThrough;
		this.SetFrame(index);
	}

	public void SetCharaInfo(TutorialMaskCtrl.CharaInfoParameter param)
	{
		if (param.charaImagePath != string.Empty)
		{
			this.infoCharaImage.SetRawImage(param.charaImagePath, true, false, null);
		}
		if (param.postion != null)
		{
			this.serifRT.anchoredPosition = param.postion.Value + new Vector2(360f, 72f);
		}
		this.infoCharaImage.transform.parent.gameObject.SetActive(param.dispInfoChara);
		switch (param.dispType)
		{
		case TutorialMaskCtrl.CharaDispType.IN:
			this.touchNextObj.GetComponent<uGUITweenColor>().Reset();
			this.touchNextObj.SetActive(false);
			this.serifRT.gameObject.SetActive(true);
			this.infoMessageText.text = param.messageList[0];
			this.tutorialSerifAnime.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
			{
				if (param.messageList.Count > 1)
				{
					this.touchNextObj.SetActive(true);
					return;
				}
				if (param.messageList.Count == 1)
				{
					if (param.enableTouchNext)
					{
						this.touchNextObj.SetActive(true);
						return;
					}
					if (param.finishCallBack != null)
					{
						param.finishCallBack();
					}
				}
			});
			this.charaInfoParameter = param;
			this.messageStep = 0;
			this.invalidMessageSkipTick = TimeManager.Now.Ticks + 2000000L;
			return;
		case TutorialMaskCtrl.CharaDispType.IN_QUICK:
			this.touchNextObj.SetActive(false);
			this.serifRT.gameObject.SetActive(true);
			this.tutorialSerifAnime.ExPauseAnimation(SimpleAnimation.ExPguiStatus.END, null);
			this.infoMessageText.text = param.messageList[0];
			if (param.messageList.Count > 1)
			{
				this.touchNextObj.SetActive(true);
			}
			else if (param.messageList.Count == 1)
			{
				if (param.enableTouchNext)
				{
					this.touchNextObj.SetActive(true);
				}
				else if (param.finishCallBack != null)
				{
					param.finishCallBack();
				}
			}
			this.charaInfoParameter = param;
			this.messageStep = 0;
			this.invalidMessageSkipTick = TimeManager.Now.Ticks + 2000000L;
			return;
		case TutorialMaskCtrl.CharaDispType.OUT:
			this.tutorialSerifAnime.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
			{
				this.serifRT.gameObject.SetActive(false);
				if (param.finishCallBack != null)
				{
					param.finishCallBack();
				}
			});
			return;
		case TutorialMaskCtrl.CharaDispType.OUT_QUICK:
			this.serifRT.gameObject.SetActive(false);
			if (param.finishCallBack != null)
			{
				param.finishCallBack();
			}
			return;
		default:
			return;
		}
	}

	private void SetStickCursor()
	{
		if (this.stickParentRT == null)
		{
			return;
		}
		Transform parent = this.stickRT.transform.parent;
		this.stickRT.transform.SetParent(this.stickParentRT.transform, true);
		(this.stickRT.transform as RectTransform).anchoredPosition = new Vector2(0f, 0f);
		Transform transform = this.stickParentRT.transform;
		while (!(transform == null) && !(transform.GetComponent<SafeAreaScaler>() != null) && !(transform.GetComponent<Canvas>() != null))
		{
			transform = transform.parent;
		}
		this.stickRT.transform.SetParent(transform, true);
		Vector2 anchoredPosition = (this.stickRT.transform as RectTransform).anchoredPosition;
		this.stickRT.transform.SetParent(parent, true);
		(this.stickRT.transform as RectTransform).anchoredPosition = anchoredPosition + new Vector2((this.stickScaleX < 0f) ? (this.stickParentRT.rect.width * this.stickScaleX) : (this.stickParentRT.rect.width / 4f), this.stickParentRT.rect.height / 1.5f);
	}

	public void SetStickCursor(bool isDisp, Vector2? postion = null, Vector3? angle = null, Vector2? offset = null)
	{
		this.stickRT.gameObject.SetActive(isDisp);
		this.stickPosition = postion;
		this.stickAngle = angle;
		this.stickOffset = offset;
		if (postion != null)
		{
			this.stickRT.anchoredPosition = postion.Value;
		}
		if (angle != null)
		{
			this.stickRT.localEulerAngles = angle.Value;
		}
		if (offset != null)
		{
			this.stickRT.anchoredPosition += offset.Value;
		}
	}

	public bool IsEnableStick()
	{
		return this.stickRT.gameObject.activeSelf;
	}

	public void SetStickCursor(RectTransform rectTransform, float scaleX = 1f)
	{
		this.stickRT.gameObject.SetActive(true);
		this.stickRT.localEulerAngles = new Vector3(0f, 0f, 50f);
		this.stickPosition = null;
		this.stickAngle = null;
		this.stickOffset = null;
		this.stickParentRT = rectTransform;
		this.stickScaleX = scaleX;
		this.SetStickCursor();
	}

	public void SetBlackMask(bool isDisp)
	{
		this.SetMaskEnable(true);
		this.maskImage.color = (isDisp ? new Color(0f, 0f, 0f, 0.5f) : Color.clear);
		foreach (RawImage rawImage in this.outFrame)
		{
			rawImage.color = this.maskImage.color;
		}
	}

	public void SetMaskEnable(bool sw)
	{
		this.maskImage.gameObject.SetActive(sw);
		foreach (RawImage rawImage in this.outFrame)
		{
			rawImage.gameObject.SetActive(sw);
		}
	}

	[SerializeField]
	private Shader maskShader;

	[SerializeField]
	private RawImage maskImage;

	[SerializeField]
	private List<PguiCollider> frameCollider;

	[SerializeField]
	private List<Rect> maskRect;

	[SerializeField]
	private GameObject TutorialSerifPrefab;

	[SerializeField]
	private GameObject TutorialStickPrefab;

	private List<int> idClicpLT;

	private List<int> idClicpRT;

	private List<int> idClicpLB;

	private List<int> idClicpRB;

	private const float MASK_WIDTH = 1280f;

	private const float MASK_HEIGHT = 720f;

	private List<RectTransform> maskRT;

	private List<Vector2> maskScale;

	private List<FieldCameraScaler> maskCam;

	private List<Rect> maskBaseRect = new List<Rect>();

	private RectTransform serifRT;

	private RectTransform stickRT;

	private RectTransform stickParentRT;

	private float stickScaleX = 1f;

	private Vector2? stickPosition;

	private Vector3? stickAngle;

	private Vector2? stickOffset;

	private SimpleAnimation tutorialSerifAnime;

	private PguiRawImageCtrl infoCharaImage;

	private PguiTextCtrl infoMessageText;

	private GameObject touchNextObj;

	private TutorialMaskCtrl.CharaInfoParameter charaInfoParameter;

	private int messageStep;

	private long invalidMessageSkipTick;

	private const long InvalidMessageSkipTickInterval = 2000000L;

	private List<RawImage> outFrame;

	private bool isInit;

	private Rect m_SafeArea;

	public enum CharaDispType
	{
		INVALID,
		IN,
		IN_QUICK,
		OUT,
		OUT_QUICK
	}

	public class CharaInfoParameter
	{
		public TutorialMaskCtrl.CharaDispType dispType;

		public Vector2? postion;

		public bool dispInfoChara = true;

		public bool enableTouchNext = true;

		public string charaImagePath = string.Empty;

		public List<string> messageList = new List<string>();

		public UnityAction finishCallBack;
	}
}
