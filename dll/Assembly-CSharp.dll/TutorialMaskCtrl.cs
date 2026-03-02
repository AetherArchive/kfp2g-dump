using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200010A RID: 266
public class TutorialMaskCtrl : MonoBehaviour
{
	// Token: 0x06000CDA RID: 3290 RVA: 0x0004E764 File Offset: 0x0004C964
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

	// Token: 0x06000CDB RID: 3291 RVA: 0x0004EB50 File Offset: 0x0004CD50
	private void Update()
	{
		if (this.m_SafeArea != SafeAreaScaler.GetSafeArea())
		{
			base.StartCoroutine(this.UpdateMask());
			base.StartCoroutine(this.UpdateStick());
			this.m_SafeArea = SafeAreaScaler.GetSafeArea();
		}
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x0004EB89 File Offset: 0x0004CD89
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

	// Token: 0x06000CDD RID: 3293 RVA: 0x0004EB98 File Offset: 0x0004CD98
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

	// Token: 0x06000CDE RID: 3294 RVA: 0x0004EBA8 File Offset: 0x0004CDA8
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

	// Token: 0x06000CDF RID: 3295 RVA: 0x0004F090 File Offset: 0x0004D290
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

	// Token: 0x06000CE0 RID: 3296 RVA: 0x0004F1D4 File Offset: 0x0004D3D4
	public void SetEnable(bool e)
	{
		this.SetMaskEnable(true);
		base.gameObject.SetActive(e);
	}

	// Token: 0x06000CE1 RID: 3297 RVA: 0x0004F1EC File Offset: 0x0004D3EC
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

	// Token: 0x06000CE2 RID: 3298 RVA: 0x0004F3D8 File Offset: 0x0004D5D8
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

	// Token: 0x06000CE3 RID: 3299 RVA: 0x0004F47C File Offset: 0x0004D67C
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

	// Token: 0x06000CE4 RID: 3300 RVA: 0x0004F710 File Offset: 0x0004D910
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

	// Token: 0x06000CE5 RID: 3301 RVA: 0x0004F870 File Offset: 0x0004DA70
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

	// Token: 0x06000CE6 RID: 3302 RVA: 0x0004F900 File Offset: 0x0004DB00
	public bool IsEnableStick()
	{
		return this.stickRT.gameObject.activeSelf;
	}

	// Token: 0x06000CE7 RID: 3303 RVA: 0x0004F914 File Offset: 0x0004DB14
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

	// Token: 0x06000CE8 RID: 3304 RVA: 0x0004F98C File Offset: 0x0004DB8C
	public void SetBlackMask(bool isDisp)
	{
		this.SetMaskEnable(true);
		this.maskImage.color = (isDisp ? new Color(0f, 0f, 0f, 0.5f) : Color.clear);
		foreach (RawImage rawImage in this.outFrame)
		{
			rawImage.color = this.maskImage.color;
		}
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x0004FA1C File Offset: 0x0004DC1C
	public void SetMaskEnable(bool sw)
	{
		this.maskImage.gameObject.SetActive(sw);
		foreach (RawImage rawImage in this.outFrame)
		{
			rawImage.gameObject.SetActive(sw);
		}
	}

	// Token: 0x04000A31 RID: 2609
	[SerializeField]
	private Shader maskShader;

	// Token: 0x04000A32 RID: 2610
	[SerializeField]
	private RawImage maskImage;

	// Token: 0x04000A33 RID: 2611
	[SerializeField]
	private List<PguiCollider> frameCollider;

	// Token: 0x04000A34 RID: 2612
	[SerializeField]
	private List<Rect> maskRect;

	// Token: 0x04000A35 RID: 2613
	[SerializeField]
	private GameObject TutorialSerifPrefab;

	// Token: 0x04000A36 RID: 2614
	[SerializeField]
	private GameObject TutorialStickPrefab;

	// Token: 0x04000A37 RID: 2615
	private List<int> idClicpLT;

	// Token: 0x04000A38 RID: 2616
	private List<int> idClicpRT;

	// Token: 0x04000A39 RID: 2617
	private List<int> idClicpLB;

	// Token: 0x04000A3A RID: 2618
	private List<int> idClicpRB;

	// Token: 0x04000A3B RID: 2619
	private const float MASK_WIDTH = 1280f;

	// Token: 0x04000A3C RID: 2620
	private const float MASK_HEIGHT = 720f;

	// Token: 0x04000A3D RID: 2621
	private List<RectTransform> maskRT;

	// Token: 0x04000A3E RID: 2622
	private List<Vector2> maskScale;

	// Token: 0x04000A3F RID: 2623
	private List<FieldCameraScaler> maskCam;

	// Token: 0x04000A40 RID: 2624
	private List<Rect> maskBaseRect = new List<Rect>();

	// Token: 0x04000A41 RID: 2625
	private RectTransform serifRT;

	// Token: 0x04000A42 RID: 2626
	private RectTransform stickRT;

	// Token: 0x04000A43 RID: 2627
	private RectTransform stickParentRT;

	// Token: 0x04000A44 RID: 2628
	private float stickScaleX = 1f;

	// Token: 0x04000A45 RID: 2629
	private Vector2? stickPosition;

	// Token: 0x04000A46 RID: 2630
	private Vector3? stickAngle;

	// Token: 0x04000A47 RID: 2631
	private Vector2? stickOffset;

	// Token: 0x04000A48 RID: 2632
	private SimpleAnimation tutorialSerifAnime;

	// Token: 0x04000A49 RID: 2633
	private PguiRawImageCtrl infoCharaImage;

	// Token: 0x04000A4A RID: 2634
	private PguiTextCtrl infoMessageText;

	// Token: 0x04000A4B RID: 2635
	private GameObject touchNextObj;

	// Token: 0x04000A4C RID: 2636
	private TutorialMaskCtrl.CharaInfoParameter charaInfoParameter;

	// Token: 0x04000A4D RID: 2637
	private int messageStep;

	// Token: 0x04000A4E RID: 2638
	private long invalidMessageSkipTick;

	// Token: 0x04000A4F RID: 2639
	private const long InvalidMessageSkipTickInterval = 2000000L;

	// Token: 0x04000A50 RID: 2640
	private List<RawImage> outFrame;

	// Token: 0x04000A51 RID: 2641
	private bool isInit;

	// Token: 0x04000A52 RID: 2642
	private Rect m_SafeArea;

	// Token: 0x02000846 RID: 2118
	public enum CharaDispType
	{
		// Token: 0x04003729 RID: 14121
		INVALID,
		// Token: 0x0400372A RID: 14122
		IN,
		// Token: 0x0400372B RID: 14123
		IN_QUICK,
		// Token: 0x0400372C RID: 14124
		OUT,
		// Token: 0x0400372D RID: 14125
		OUT_QUICK
	}

	// Token: 0x02000847 RID: 2119
	public class CharaInfoParameter
	{
		// Token: 0x0400372E RID: 14126
		public TutorialMaskCtrl.CharaDispType dispType;

		// Token: 0x0400372F RID: 14127
		public Vector2? postion;

		// Token: 0x04003730 RID: 14128
		public bool dispInfoChara = true;

		// Token: 0x04003731 RID: 14129
		public bool enableTouchNext = true;

		// Token: 0x04003732 RID: 14130
		public string charaImagePath = string.Empty;

		// Token: 0x04003733 RID: 14131
		public List<string> messageList = new List<string>();

		// Token: 0x04003734 RID: 14132
		public UnityAction finishCallBack;
	}
}
