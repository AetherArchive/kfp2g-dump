using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AEAuth3;
using CriWare;
using SGNFW.Common;
using SGNFW.Touch;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001B5 RID: 437
public class RenderTextureChara : MonoBehaviour
{
	// Token: 0x06001D7D RID: 7549 RVA: 0x0016E6E4 File Offset: 0x0016C8E4
	public static CharaMotionDefine.ActKey StrBodyMotionId2CharaModionDefineActKey(string bodyMotionId)
	{
		Type typeFromHandle = typeof(CharaMotionDefine.ActKey);
		if (!Enum.IsDefined(typeFromHandle, bodyMotionId))
		{
			return CharaMotionDefine.ActKey.INVALID;
		}
		return (CharaMotionDefine.ActKey)Enum.Parse(typeFromHandle, bodyMotionId, true);
	}

	// Token: 0x06001D7E RID: 7550 RVA: 0x0016E714 File Offset: 0x0016C914
	public void SetupInterruptMotion(RenderTextureChara.InterruptMotion im, bool enableOverwrite = true)
	{
		if (this.interruptMotionList == null)
		{
			this.interruptMotionList = new List<RenderTextureChara.InterruptMotion>();
		}
		if (enableOverwrite)
		{
			if (this.interruptMotionList.Count > 0)
			{
				this.interruptMotionList.RemoveAt(0);
			}
			if (this.interruptMotionCtrl != null)
			{
				Singleton<SceneManager>.Instance.StopCoroutine(this.interruptMotionCtrl);
				this.interruptMotionCtrl = null;
			}
		}
		this.interruptMotionList.Add(im);
	}

	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x06001D7F RID: 7551 RVA: 0x0016E77C File Offset: 0x0016C97C
	public int DispCharaId
	{
		get
		{
			return this.charaId;
		}
	}

	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x06001D80 RID: 7552 RVA: 0x0016E784 File Offset: 0x0016C984
	public int DispClothImageId
	{
		get
		{
			return this.clothImageId;
		}
	}

	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x06001D81 RID: 7553 RVA: 0x0016E78C File Offset: 0x0016C98C
	// (set) Token: 0x06001D82 RID: 7554 RVA: 0x0016E794 File Offset: 0x0016C994
	public int TapCount { get; private set; }

	// Token: 0x06001D83 RID: 7555 RVA: 0x0016E79D File Offset: 0x0016C99D
	public Vector3 GetCharaScale()
	{
		return this.charaMH.GetCharaScale();
	}

	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x06001D84 RID: 7556 RVA: 0x0016E7AA File Offset: 0x0016C9AA
	// (set) Token: 0x06001D85 RID: 7557 RVA: 0x0016E7B2 File Offset: 0x0016C9B2
	[HideInInspector]
	public bool FinishedSetup { get; private set; }

	// Token: 0x1700040D RID: 1037
	// (get) Token: 0x06001D86 RID: 7558 RVA: 0x0016E7BB File Offset: 0x0016C9BB
	protected Material renderCharaMaterial
	{
		get
		{
			if (this.m_renderCharaMaterial == null)
			{
				this.m_renderCharaMaterial = new Material(this.renderCharaShader);
				this.m_renderCharaMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_renderCharaMaterial;
		}
	}

	// Token: 0x1700040E RID: 1038
	// (get) Token: 0x06001D87 RID: 7559 RVA: 0x0016E7F0 File Offset: 0x0016C9F0
	private Camera dispCamera
	{
		get
		{
			if (this.m_dispCamera == null)
			{
				Transform transform = base.transform.Find("Camera");
				if (transform != null)
				{
					if (!this.isAwake)
					{
						return transform.GetComponent<Camera>();
					}
					this.m_dispCamera = transform.GetComponent<Camera>();
					transform.SetParent(this.renderTextureBase, false);
				}
			}
			return this.m_dispCamera;
		}
	}

	// Token: 0x1700040F RID: 1039
	// (get) Token: 0x06001D88 RID: 7560 RVA: 0x0016E858 File Offset: 0x0016CA58
	private Camera dispCameraDirect
	{
		get
		{
			if (this.m_dispCameraDirect == null)
			{
				Transform transform = base.transform.Find("CameraDirect");
				if (transform != null)
				{
					if (!this.isAwake)
					{
						return transform.GetComponent<Camera>();
					}
					this.m_dispCameraDirect = transform.GetComponent<Camera>();
					transform.SetParent(this.renderTextureBase, false);
				}
			}
			return this.m_dispCameraDirect;
		}
	}

	// Token: 0x17000410 RID: 1040
	// (get) Token: 0x06001D89 RID: 7561 RVA: 0x0016E8C0 File Offset: 0x0016CAC0
	private GameObject dispChara
	{
		get
		{
			if (this.m_dispChara == null)
			{
				Transform transform = base.transform.Find("chara");
				if (transform != null)
				{
					if (!this.isAwake)
					{
						return transform.gameObject;
					}
					this.m_dispChara = transform.gameObject;
					transform.SetParent(this.renderTextureBase, false);
				}
			}
			return this.m_dispChara;
		}
	}

	// Token: 0x17000411 RID: 1041
	// (get) Token: 0x06001D8A RID: 7562 RVA: 0x0016E928 File Offset: 0x0016CB28
	private RawImage dispTexture
	{
		get
		{
			if (this.m_dispTexture == null)
			{
				Transform transform = base.transform.Find("Texture");
				if (transform != null)
				{
					this.m_dispTexture = transform.GetComponent<RawImage>();
				}
			}
			return this.m_dispTexture;
		}
	}

	// Token: 0x17000412 RID: 1042
	// (get) Token: 0x06001D8B RID: 7563 RVA: 0x0016E970 File Offset: 0x0016CB70
	private RenderTexture renderTexture
	{
		get
		{
			if (this.m_renderTexture == null)
			{
				this.m_renderTexture = new RenderTexture(this.width, this.height, 24, RenderTextureFormat.ARGB32);
				this.m_renderTexture.wrapMode = TextureWrapMode.Clamp;
				this.m_renderTexture.useMipMap = false;
				this.m_renderTexture.filterMode = FilterMode.Bilinear;
				this.m_renderTexture.hideFlags = HideFlags.HideAndDontSave;
				this.m_renderTexture.antiAliasing = 2;
				this.m_renderTexture.Create();
			}
			return this.m_renderTexture;
		}
	}

	// Token: 0x17000413 RID: 1043
	// (get) Token: 0x06001D8C RID: 7564 RVA: 0x0016E9F4 File Offset: 0x0016CBF4
	private GameObject dispLight
	{
		get
		{
			if (RenderTextureChara.m_dispLight == null)
			{
				RenderTextureChara.m_dispLight = new GameObject("RenderTexture Directional light", new Type[] { typeof(Light) });
				RenderTextureChara.m_dispLight.layer = LayerMask.NameToLayer("UIRenderChara");
				RenderTextureChara.m_dispLight.transform.SetParent(this.renderTextureBase, false);
				RenderTextureChara.m_dispLight.transform.localRotation = Quaternion.Euler(50f, -30f, 0f);
				Light component = RenderTextureChara.m_dispLight.GetComponent<Light>();
				if (component != null)
				{
					component.cullingMask = (1 << RenderTextureChara.m_dispLight.layer) | (1 << RenderTextureChara.m_dispLight.layer + 1) | (1 << RenderTextureChara.m_dispLight.layer + 2) | (1 << LayerMask.NameToLayer("Bloom"));
					component.type = LightType.Directional;
				}
			}
			return RenderTextureChara.m_dispLight;
		}
	}

	// Token: 0x17000414 RID: 1044
	// (get) Token: 0x06001D8D RID: 7565 RVA: 0x0016EAEC File Offset: 0x0016CCEC
	private GameObject dispLight2
	{
		get
		{
			if (RenderTextureChara.m_dispLight2 == null)
			{
				RenderTextureChara.m_dispLight2 = new GameObject("RenderTexture Directional light2", new Type[] { typeof(Light) });
				RenderTextureChara.m_dispLight2.layer = LayerMask.NameToLayer("UIRenderChara");
				RenderTextureChara.m_dispLight2.transform.SetParent(this.renderTextureBase, false);
				RenderTextureChara.m_dispLight2.transform.localRotation = Quaternion.Euler(50f, -30f, 0f);
				Light component = RenderTextureChara.m_dispLight2.GetComponent<Light>();
				if (component != null)
				{
					component.cullingMask = 1 << LayerMask.NameToLayer("Bloom2");
					component.type = LightType.Directional;
				}
			}
			return RenderTextureChara.m_dispLight2;
		}
	}

	// Token: 0x17000415 RID: 1045
	// (get) Token: 0x06001D8E RID: 7566 RVA: 0x0016EBAD File Offset: 0x0016CDAD
	private Transform renderTextureBase
	{
		get
		{
			if (RenderTextureChara.m_renderTextureBase == null)
			{
				RenderTextureChara.m_renderTextureBase = new GameObject("RenderTexture Base").transform;
			}
			return RenderTextureChara.m_renderTextureBase;
		}
	}

	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x06001D8F RID: 7567 RVA: 0x0016EBD8 File Offset: 0x0016CDD8
	private Canvas tmpBgCanvas
	{
		get
		{
			if (this.m_tmpBgCanvas == null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("prefab/PguiCanvas"));
				gameObject.name = "TmpBackgroundCanvas";
				(gameObject.transform as RectTransform).anchoredPosition = new Vector2(0f, -250f);
				gameObject.GetComponent<Canvas>().worldCamera.clearFlags = CameraClearFlags.Depth;
				gameObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
				this.m_tmpBgCanvas = gameObject.transform.GetComponent<Canvas>();
			}
			return this.m_tmpBgCanvas;
		}
	}

	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x06001D90 RID: 7568 RVA: 0x0016EC68 File Offset: 0x0016CE68
	private Texture2D transparentTex
	{
		get
		{
			if (RenderTextureChara.m_trasnsparentTex == null)
			{
				RenderTextureChara.m_trasnsparentTex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
				RenderTextureChara.m_trasnsparentTex.SetPixel(0, 0, new Color(0f, 0f, 0f, 0f));
				RenderTextureChara.m_trasnsparentTex.Apply();
			}
			return RenderTextureChara.m_trasnsparentTex;
		}
	}

	// Token: 0x06001D91 RID: 7569 RVA: 0x0016ECC4 File Offset: 0x0016CEC4
	public void SetupRenderTexture(int w, int h)
	{
		this.dispTexture.material = this.renderCharaMaterial;
		this.dispCamera.targetTexture = null;
		this.renderCharaMaterial.mainTexture = null;
		if (this.m_renderTexture != null)
		{
			this.m_renderTexture.Release();
			Object.DestroyImmediate(this.m_renderTexture);
			this.m_renderTexture = null;
		}
		this.width = w;
		this.height = h;
		this.dispCamera.targetTexture = this.renderTexture;
		this.renderCharaMaterial.mainTexture = this.renderTexture;
		this.dispTexture.SetNativeSize();
	}

	// Token: 0x06001D92 RID: 7570 RVA: 0x0016ED60 File Offset: 0x0016CF60
	private void Awake()
	{
		this.isAwake = true;
	}

	// Token: 0x06001D93 RID: 7571 RVA: 0x0016ED6C File Offset: 0x0016CF6C
	private void Start()
	{
		this.canvasCamera = base.GetComponentInParent<Canvas>().worldCamera;
		this.SetupRenderTexture(this.width, this.height);
		PguiRenderTextureCharaCtrl component = base.transform.parent.GetComponent<PguiRenderTextureCharaCtrl>();
		if (component != null)
		{
			this.postion = component.param.postion;
			this.rotation = component.param.rotation;
			this.fieldOfView = component.param.fieldOfView;
		}
	}

	// Token: 0x06001D94 RID: 7572 RVA: 0x0016EDEC File Offset: 0x0016CFEC
	private void Update()
	{
		if (this.interruptMotionList != null && this.interruptMotionList.Count > 0 && this.interruptMotionCtrl == null)
		{
			this.interruptMotionCtrl = this.InterruptMotionCtrl();
			Singleton<SceneManager>.Instance.StartCoroutine(this.interruptMotionCtrl);
			if (this.leaveAloneReactionInternal != null)
			{
				Singleton<SceneManager>.Instance.StopCoroutine(this.leaveAloneReactionInternal);
				this.leaveAloneReactionInternal = null;
			}
		}
		if (this.leaveAloneReactionInternal != null && (this.isTouchedBody || this.isTouchedFace))
		{
			if (this.isSleep)
			{
				this.isSleep = false;
				Singleton<SceneManager>.Instance.StopCoroutine(this.leaveAloneReactionInternal);
				this.SetupLeaveAloneReaction(RenderTextureChara.CharaActType.WAITTOUCH);
			}
			else
			{
				Singleton<SceneManager>.Instance.StopCoroutine(this.leaveAloneReactionInternal);
				this.SetupLeaveAloneReaction(this.isTouchedBody ? RenderTextureChara.CharaActType.BODYTOUCH : RenderTextureChara.CharaActType.FACETOUCH);
			}
			this.isTouchedBody = false;
			this.isTouchedFace = false;
		}
		if (this.enableTuouchCharaModel)
		{
			if (this.isTouchedFace)
			{
				UnityAction touchedFaceCb = this.TouchedFaceCb;
				if (touchedFaceCb != null)
				{
					touchedFaceCb();
				}
				this.isTouchedFace = false;
			}
			if (this.isTouchedBody)
			{
				UnityAction touchedBodyCb = this.TouchedBodyCb;
				if (touchedBodyCb != null)
				{
					touchedBodyCb();
				}
				this.isTouchedBody = false;
			}
		}
		if (this.setupInternal != null && !this.setupInternal.MoveNext())
		{
			this.setupInternal = null;
		}
		if (this.interruptMotionCtrl != null && !this.interruptMotionCtrl.MoveNext())
		{
			Singleton<SceneManager>.Instance.StopCoroutine(this.interruptMotionCtrl);
			this.interruptMotionCtrl = null;
			if (this.interruptMotionList.Count > 0)
			{
				this.interruptMotionList.RemoveAt(0);
			}
			this.SetupLeaveAloneReaction(RenderTextureChara.CharaActType.INVALID);
		}
		if (this.isSetupInternal && this.currentFinishCallback != null && !this.charaMH.IsPlaying())
		{
			RenderTextureChara.FinishCallback finishCallback = this.currentFinishCallback;
			this.currentFinishCallback = null;
			finishCallback();
		}
		if (this.m_dispChara != null)
		{
			List<EffectData> charaEffect = this.dispChara.GetComponent<CharaModelHandle>().charaEffect;
			if (this.IsBloom())
			{
				if (this.IsAlpha())
				{
					if (!this.beforeAlpha)
					{
						this.ResetTmpBgObjects();
						this.DisableTmpBgCanvas(false);
					}
					this.UpdateCharaPosition(false);
					this.dispCameraDirect.gameObject.SetActive(false);
					if (this.FinishedSetup)
					{
						this.dispTexture.gameObject.SetActive(true);
					}
				}
				else
				{
					if (this.beforeAlpha)
					{
						this.SetupTmpBgCanvas();
					}
					this.dispTexture.gameObject.SetActive(false);
					if (charaEffect[0].effectObject.layer != this.bloomLayer)
					{
						charaEffect[0].effectObject.SetLayerRecursively(this.bloomLayer);
					}
					if (base.transform.root.GetComponent<RectTransform>().sizeDelta != this.canvasSize || this.dispTexture.transform.lossyScale != this.textureScale || (this.parentMask != null && (this.parentMaskSize != this.parentMask.GetComponent<RectTransform>().sizeDelta || this.parentMaskPos != this.parentMask.transform.position)))
					{
						this.CalcDirectCameraFov();
					}
					this.UpdateCharaPosition(true);
					this.dispCameraDirect.gameObject.SetActive(!this.IsMaskInvisible() && !this.isOutOfCameraDirect && this.FinishedSetup);
				}
				if (this.originCloneGraphics != null)
				{
					foreach (KeyValuePair<MaskableGraphic, MaskableGraphic> keyValuePair in this.originCloneGraphics)
					{
						MaskableGraphic key = keyValuePair.Key;
						MaskableGraphic value = keyValuePair.Value;
						if (!(key == null) && !(value == null) && (key.gameObject.activeInHierarchy || value.gameObject.activeInHierarchy))
						{
							if (!key.gameObject.activeInHierarchy && value.gameObject.activeInHierarchy)
							{
								value.gameObject.SetActive(false);
							}
							else
							{
								Transform transform = value.transform;
								while (transform != null && !transform.gameObject.activeInHierarchy)
								{
									transform.gameObject.SetActive(true);
									transform = transform.parent;
								}
								if (key.enabled && key.color != Color.clear && (key as AEImage == null || (key as AEImage).texture != this.transparentTex))
								{
									this.SetTransparentGraphic(key, value, null);
									break;
								}
								RawImage rawImage = key as RawImage;
								RawImage rawImage2 = value as RawImage;
								if (rawImage != null && rawImage2 != null)
								{
									rawImage2.texture = rawImage.texture;
								}
								Image image = key as Image;
								Image image2 = value as Image;
								if (image != null && image2 != null)
								{
									image2.sprite = image.sprite;
								}
								Text text = key as Text;
								Text text2 = value as Text;
								if (text != null && text2 != null)
								{
									text2.text = text.text;
								}
								if (value.transform.parent.GetComponent<LayoutGroup>() == null && value.transform.GetComponent<ILayoutSelfController>() == null)
								{
									value.transform.position = key.transform.position + this.tmpBgCanvas.transform.position - key.transform.root.position;
								}
								value.transform.localScale = new Vector3((value.transform.parent.lossyScale.x != 0f) ? (key.transform.lossyScale.x / value.transform.parent.lossyScale.x) : 0f, (value.transform.parent.lossyScale.y != 0f) ? (key.transform.lossyScale.y / value.transform.parent.lossyScale.y) : 0f, (value.transform.parent.lossyScale.z != 0f) ? (key.transform.lossyScale.z / value.transform.parent.lossyScale.z) : 0f);
								value.transform.rotation = key.transform.rotation;
								if (value.transform.parent != value.transform.root)
								{
									if (key.transform.parent.gameObject.name != value.transform.parent.gameObject.name)
									{
										Transform transform2 = key.transform;
										Transform transform3 = key.transform.parent;
										bool flag = false;
										while (transform3 != null)
										{
											Transform transform4 = base.transform;
											Transform transform5 = base.transform.parent;
											bool flag2 = false;
											while (transform5 != null)
											{
												if (transform3 == transform5)
												{
													if (transform2.GetSiblingIndex() > transform4.GetSiblingIndex())
													{
														foreach (MaskableGraphic maskableGraphic in key.GetComponentsInChildren<MaskableGraphic>(true))
														{
															MaskableGraphic maskableGraphic2 = this.originCloneGraphics.TryGetValueEx(maskableGraphic, null);
															if (!(maskableGraphic2 == null))
															{
																this.RestoreTransparentGraphic(maskableGraphic, maskableGraphic2);
																maskableGraphic2.gameObject.SetActive(false);
																maskableGraphic2.enabled = false;
																this.originCloneGraphics.Remove(maskableGraphic);
															}
														}
														flag = true;
													}
													flag2 = true;
													break;
												}
												transform4 = transform4.parent;
												transform5 = transform5.parent;
											}
											if (flag2)
											{
												break;
											}
											transform2 = transform3;
											transform3 = transform3.parent;
										}
										if (flag)
										{
											break;
										}
									}
									value.transform.SetSiblingIndex(key.transform.GetSiblingIndex());
								}
							}
						}
					}
				}
				if (this.originCloneAECtrls != null)
				{
					foreach (PguiAECtrl pguiAECtrl in this.originCloneAECtrls.Keys)
					{
						PguiAECtrl pguiAECtrl2 = this.originCloneAECtrls[pguiAECtrl];
						if (!(pguiAECtrl == null) && !(pguiAECtrl2 == null) && !(pguiAECtrl.m_AEImage == null) && !(pguiAECtrl2.m_AEImage == null) && pguiAECtrl2.gameObject.activeInHierarchy)
						{
							pguiAECtrl2.m_AEImage.sourceClip = pguiAECtrl.m_AEImage.sourceClip;
							if (pguiAECtrl.IsPlaying() && (!pguiAECtrl2.IsPlaying() || pguiAECtrl2.GetAnimeType() != pguiAECtrl.GetAnimeType()))
							{
								pguiAECtrl2.PlayAnimation(pguiAECtrl.GetAnimeType(), pguiAECtrl.GetFinishCallback());
							}
							pguiAECtrl2.m_AEImage.playTime = pguiAECtrl.m_AEImage.playTime;
							pguiAECtrl2.m_AEImage.playSpeed = pguiAECtrl.m_AEImage.playSpeed;
							if (pguiAECtrl.m_AEImage.GetLayers() != null)
							{
								foreach (global::AEAuth3.Layer layer in pguiAECtrl.m_AEImage.GetLayers())
								{
									global::AEAuth3.Layer layer2 = pguiAECtrl2.m_AEImage.GetLayer(layer.name);
									if (layer != null && layer2 != null)
									{
										layer2.layerFlags = layer.layerFlags;
									}
								}
							}
						}
					}
				}
				if (this.originCloneAnims != null)
				{
					foreach (SimpleAnimation simpleAnimation in this.originCloneAnims.Keys)
					{
						SimpleAnimation simpleAnimation2 = this.originCloneAnims[simpleAnimation];
						if (!(simpleAnimation == null) && !(simpleAnimation2 == null) && !(simpleAnimation.ExGetLastPlayStateName() == ""))
						{
							if (simpleAnimation.gameObject.activeInHierarchy && !simpleAnimation2.gameObject.activeInHierarchy)
							{
								Transform transform6 = simpleAnimation2.transform;
								while (transform6 != null && !transform6.gameObject.activeInHierarchy)
								{
									transform6.gameObject.SetActive(true);
									transform6 = transform6.parent;
								}
							}
							if (simpleAnimation.ExIsPlaying())
							{
								simpleAnimation2.ExPlayAnimation(simpleAnimation.ExGetLastPlayStateName(), simpleAnimation.ExGetAbsTime(), simpleAnimation.ExGetSpeed());
							}
						}
					}
				}
				this.beforeAlpha = this.IsAlpha();
			}
		}
	}

	// Token: 0x06001D95 RID: 7573 RVA: 0x0016F8F8 File Offset: 0x0016DAF8
	private void OnEnable()
	{
		this.dispCamera.gameObject.SetActive(true);
		this.dispChara.SetActive(true);
		if (this.charaMH != null && this.charaMH.IsFinishInitialize())
		{
			if (this.leaveAloneReactionInternal != null)
			{
				Singleton<SceneManager>.Instance.StopCoroutine(this.leaveAloneReactionInternal);
				this.SetupLeaveAloneReaction(RenderTextureChara.CharaActType.SCENE_IN);
			}
			else
			{
				this.SetAnimation(this.resetMotion, true);
			}
			bool flag = this.IsBloom();
			if (flag && !this.isUpdateModel && !this.IsAlpha())
			{
				this.SetupTmpBgCanvas();
				this.UpdateCharaPosition(true);
				this.dispTexture.gameObject.SetActive(false);
			}
			this.dispCameraDirect.gameObject.SetActive(flag && !this.isUpdateModel && !this.IsAlpha() && !this.IsMaskInvisible() && !this.isOutOfCameraDirect && this.FinishedSetup);
		}
		if ((ItemDef.Id2Kind(this.charaId) == ItemDef.Kind.CHARA || ItemDef.Id2Kind(this.charaId) == ItemDef.Kind.NPC) && this.charaId != 0)
		{
			RenderTextureChara.CharaId2CueName(this.charaId);
		}
	}

	// Token: 0x06001D96 RID: 7574 RVA: 0x0016FA18 File Offset: 0x0016DC18
	private void OnDisable()
	{
		if (this.m_dispCamera != null)
		{
			this.m_dispCamera.gameObject.SetActive(false);
		}
		if (this.m_dispCameraDirect != null)
		{
			this.m_dispCameraDirect.gameObject.SetActive(false);
		}
		if (this.m_dispChara != null)
		{
			this.m_dispChara.SetActive(false);
		}
		if (this.charaMH != null && this.charaMH.IsFinishInitialize())
		{
			this.charaMH.PlayAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true, 1f, 0f, 0f, false);
			if (this.checkPlayVoice1stAction.GetStatus() == CriAtomExPlayback.Status.Playing)
			{
				SoundManager.Stop(this.charaMH.loadVoiceCueSheetName);
			}
		}
		if (this.leaveAloneReactionInternal != null && Singleton<SceneManager>.Instance != null)
		{
			Singleton<SceneManager>.Instance.StopCoroutine(this.leaveAloneReactionInternal);
		}
		SGNFW.Touch.Manager.UnRegisterTap(new SGNFW.Touch.Manager.SingleAction(this.OnTouchTap));
		this.TouchedFaceCb = null;
		this.TouchedBodyCb = null;
		this.enableTuouchCharaModel = false;
		this.isUpdateModel = false;
		this.ResetTmpBgObjects();
		this.DisableTmpBgCanvas(false);
	}

	// Token: 0x06001D97 RID: 7575 RVA: 0x0016FB34 File Offset: 0x0016DD34
	private void OnDestroy()
	{
		this.ResetTmpBgObjects();
		this.DisableTmpBgCanvas(true);
		if (this.m_dispCamera != null)
		{
			this.m_dispCamera.targetTexture = null;
			Object.Destroy(this.m_dispCamera.gameObject);
			this.m_dispCamera = null;
		}
		if (this.m_dispCameraDirect != null)
		{
			this.m_dispCameraDirect.targetTexture = null;
			Object.Destroy(this.m_dispCameraDirect.gameObject);
			this.m_dispCameraDirect = null;
		}
		this.charaMH = null;
		if (this.m_dispChara != null)
		{
			Object.Destroy(this.m_dispChara);
			this.m_dispChara = null;
		}
		if (this.m_dispTexture != null)
		{
			this.m_dispTexture.material = null;
			Object.Destroy(this.m_dispTexture.gameObject);
			this.m_dispTexture = null;
		}
		if (this.m_renderCharaMaterial != null)
		{
			this.m_renderCharaMaterial.mainTexture = null;
			Object.DestroyImmediate(this.m_renderCharaMaterial);
			this.m_renderCharaMaterial = null;
		}
		if (this.m_renderTexture != null)
		{
			this.m_renderTexture.Release();
			Object.DestroyImmediate(this.m_renderTexture);
			this.m_renderTexture = null;
		}
		if (this.luckyEyeEffect != null)
		{
			EffectManager.DestroyEffect(this.luckyEyeEffect);
			EffectManager.UnloadEffect("Ef_info_lb_eye_green", AssetManager.OWNER.CharaModel);
			this.luckyEyeEffect = null;
		}
	}

	// Token: 0x06001D98 RID: 7576 RVA: 0x0016FC84 File Offset: 0x0016DE84
	private IEnumerator SetupInternal(bool updateModel, bool updateMotion, bool updateFace, bool loop, bool enableTouch, FacePackData fpd, float startMotion, UnityAction setupAfterCb, bool forceSetupCollision, bool isDisableVoice)
	{
		if (this.charaId == 0)
		{
			yield break;
		}
		this.isSetupInternal = false;
		this.TapCount = 0;
		if (this.luckyEyeEffect != null)
		{
			EffectManager.DestroyEffect(this.luckyEyeEffect);
			this.luckyEyeEffect = null;
		}
		this.charaMH = this.dispChara.GetComponent<CharaModelHandle>();
		bool isBloom = this.IsBloom();
		if (updateModel)
		{
			CharaModelHandle.InitializeParam initializeParam = CharaModelHandle.InitializeParam.CreaateByCharaId(this.charaId, this.clothImageId, this.longSkirt, false);
			initializeParam.isShadowModel = false;
			initializeParam.layer = this.charaMH.gameObject.layer;
			initializeParam.isDisableVoice = isDisableVoice;
			string tmpCueName = this.charaMH.loadVoiceCueSheetName;
			if (!string.IsNullOrEmpty(tmpCueName))
			{
				SoundManager.LoadCueSheet(tmpCueName);
			}
			this.charaMH.Initialize(initializeParam);
			this.dispTexture.gameObject.SetActive(false);
			this.dispCameraDirect.gameObject.SetActive(false);
			while (this.charaMH != null && !this.charaMH.IsFinishInitialize())
			{
				yield return null;
			}
			isBloom = this.IsBloom();
			if (isBloom)
			{
				this.ResetTmpBgObjects();
				this.dispChara.transform.localPosition = this.defaultCharaLocalPos;
				this.dispTexture.GetComponent<RectTransform>().anchoredPosition = this.postion;
				if (this.dispChara.GetComponent<CharaModelHandle>().charaEffect != null)
				{
					this.dispChara.GetComponent<CharaModelHandle>().charaEffect[0].effectObject.SetLayerRecursively(this.bloomLayer);
				}
				this.GetParentInfo();
				this.dispCameraDirect.depth = this.canvasCamera.depth - 1f;
				this.CalcDirectCameraFov();
				foreach (Camera camera in this.renderTextureBase.GetComponentsInChildren<Camera>())
				{
					if (camera.name == "CameraDirect" && camera != this.dispCameraDirect && camera.gameObject.activeSelf && camera.depth == this.dispCameraDirect.depth)
					{
						camera.depth = this.dispCameraDirect.depth - 1.5f;
						break;
					}
				}
				if (!this.IsAlpha())
				{
					this.SetupTmpBgCanvas();
				}
			}
			if (!string.IsNullOrEmpty(tmpCueName))
			{
				SoundManager.UnloadCueSheet(tmpCueName);
			}
			Transform transform = base.transform;
			int j = 0;
			while (j < 10)
			{
				SafeAreaScaler component = transform.GetComponent<SafeAreaScaler>();
				if (component)
				{
					this.safeAreaScaler = component;
				}
				else
				{
					transform = transform.parent;
					if (!(transform == null))
					{
						j++;
						continue;
					}
				}
				IL_0305:
				while (this.charaMH != null && !this.charaMH.IsFinishInitialize())
				{
					yield return null;
				}
				this.OnValidate();
				tmpCueName = null;
				goto IL_032D;
			}
			goto IL_0305;
		}
		IL_032D:
		if (enableTouch || forceSetupCollision)
		{
			this.SetupCollision();
		}
		this.UpdateCharaPosition(isBloom && !this.IsAlpha());
		yield return null;
		this.dispTexture.gameObject.SetActive(!isBloom || this.IsAlpha());
		this.dispCameraDirect.gameObject.SetActive(isBloom && !this.IsAlpha() && !this.IsMaskInvisible() && !this.isOutOfCameraDirect);
		if (!isBloom || this.IsAlpha())
		{
			this.ResetTmpBgObjects();
			this.DisableTmpBgCanvas(false);
		}
		this.FinishedSetup = (this.isSetupInternal = true);
		if (updateMotion)
		{
			if (enableTouch)
			{
				if (this.leaveAloneReactionInternal != null)
				{
					Singleton<SceneManager>.Instance.StopCoroutine(this.leaveAloneReactionInternal);
				}
				this.SetupLeaveAloneReaction(RenderTextureChara.CharaActType.SCENE_IN);
				this.canvasCamera = base.transform.GetComponentInParent<Canvas>().worldCamera;
			}
			else if (startMotion > 0f)
			{
				this.charaMH.PlayAnimationByAuth(this.currentMotion.ToString(), startMotion, 1f, loop);
			}
			else
			{
				this.charaMH.PlayAnimation(this.currentMotion, loop, 1f, 0f, 0f, false);
			}
		}
		if (updateFace)
		{
			this.SetFacePack(fpd);
		}
		if (setupAfterCb != null)
		{
			setupAfterCb();
		}
		yield break;
	}

	// Token: 0x06001D99 RID: 7577 RVA: 0x0016FCEC File Offset: 0x0016DEEC
	private void UpdateCharaPosition(bool useDirectCamera)
	{
		if (this.isFirstUpdateCharaPosition)
		{
			this.defaultCharaLocalPos = this.dispChara.transform.localPosition;
			this.isFirstUpdateCharaPosition = false;
		}
		if (!useDirectCamera)
		{
			this.dispChara.transform.localPosition = this.defaultCharaLocalPos;
			this.dispChara.transform.localRotation = Quaternion.Euler(this.rotation);
			if (this.bloomLayer == LayerMask.NameToLayer("Bloom"))
			{
				this.dispLight.transform.localRotation = Quaternion.Euler(50f, -30f, 0f);
				return;
			}
			this.dispLight2.transform.localRotation = Quaternion.Euler(50f, -30f, 0f);
			return;
		}
		else
		{
			Vector3 vector = this.canvasCamera.WorldToScreenPoint(this.dispTexture.transform.position);
			Vector3 vector2 = this.dispCameraDirect.ScreenToViewportPoint(vector);
			vector2.z = this.dispCameraDirect.transform.position.z;
			if (float.IsNaN(vector2.x) || float.IsNaN(vector2.y) || vector2.x < 0f || vector2.x > 1f || vector2.y < 0f || vector2.y > 1f)
			{
				this.isOutOfCameraDirect = true;
				return;
			}
			this.isOutOfCameraDirect = false;
			Vector3 vector3 = this.dispCameraDirect.ViewportToWorldPoint(vector2);
			this.dispChara.transform.position = vector3 - new Vector3(this.dispCameraDirect.transform.position.x, this.dispCameraDirect.transform.position.y, 0f);
			Vector3 normalized = (vector3 - this.dispCameraDirect.transform.position).normalized;
			this.dispChara.transform.rotation = Quaternion.LookRotation(normalized) * this.dispCameraDirect.transform.rotation * Quaternion.Euler(this.rotation);
			if (this.bloomLayer == LayerMask.NameToLayer("Bloom"))
			{
				this.dispLight.transform.rotation = Quaternion.LookRotation(normalized) * this.dispCameraDirect.transform.rotation * Quaternion.Euler(50f, -30f, 0f);
				return;
			}
			this.dispLight2.transform.rotation = Quaternion.LookRotation(normalized) * this.dispCameraDirect.transform.rotation * Quaternion.Euler(50f, -30f, 0f);
			return;
		}
	}

	// Token: 0x06001D9A RID: 7578 RVA: 0x0016FFA0 File Offset: 0x0016E1A0
	private void CalcDirectCameraFov()
	{
		this.canvasSize = base.transform.root.GetComponent<RectTransform>().sizeDelta;
		this.textureScale = this.dispTexture.transform.lossyScale;
		float num = this.canvasSize.y * base.transform.root.lossyScale.y;
		float num2 = this.canvasSize.x * base.transform.root.lossyScale.x;
		float num3 = this.dispTexture.GetComponent<RectTransform>().sizeDelta.y * this.dispTexture.transform.lossyScale.y;
		float num4 = this.dispCamera.transform.position.z - this.defaultCharaLocalPos.z;
		float num5 = Mathf.Tan(this.fieldOfView * 0.017453292f * 0.5f) * num4 * 2f;
		if (this.parentMask != null)
		{
			this.parentMaskSize = this.parentMask.GetComponent<RectTransform>().sizeDelta;
			this.parentMaskPos = this.parentMask.transform.position;
			RectTransform component = this.parentMask.GetComponent<RectTransform>();
			if (component.rect.width == 0f || component.rect.height == 0f)
			{
				this.dispCameraDirect.gameObject.SetActive(false);
				return;
			}
			Vector3 vector = component.TransformPoint(component.rect.center);
			Vector3 vector2 = this.canvasCamera.WorldToViewportPoint(vector);
			float num6 = component.rect.width * this.parentMask.transform.lossyScale.x;
			float num7 = component.rect.height * this.parentMask.transform.lossyScale.y;
			this.dispCameraDirect.rect = new Rect(vector2.x - num6 * 0.5f / num2, vector2.y - num7 * 0.5f / num, num6 / num2, num7 / num);
			float num8 = Mathf.Atan(num5 / num3 * num7 * 0.5f / num4) * 57.29578f * 2f;
			if (float.IsNaN(num8))
			{
				this.isOutOfCameraDirect = true;
				return;
			}
			this.dispCameraDirect.fieldOfView = num8;
			return;
		}
		else
		{
			this.dispCameraDirect.rect = new Rect(0f, 0f, 1f, 1f);
			float num9 = Mathf.Atan(num5 / num3 * num * 0.5f / num4) * 57.29578f * 2f;
			if (float.IsNaN(num9))
			{
				this.isOutOfCameraDirect = true;
				return;
			}
			this.dispCameraDirect.fieldOfView = num9;
			return;
		}
	}

	// Token: 0x06001D9B RID: 7579 RVA: 0x00170278 File Offset: 0x0016E478
	private IEnumerator LeaveAloneReactionInternal(RenderTextureChara.CharaActType firstAction)
	{
		string cueName = RenderTextureChara.CharaId2CueName(this.charaId);
		bool gotoNext = false;
		if (firstAction != RenderTextureChara.CharaActType.INVALID)
		{
			this.SetAnimation(this.currentActKeyList[(int)firstAction], false, delegate
			{
				gotoNext = true;
			});
			if (this.checkPlayVoice1stAction.GetStatus() != CriAtomExPlayback.Status.Playing)
			{
				this.checkPlayVoice1stAction = SoundManager.PlayVoice(cueName, this.currentVoiceList[(int)firstAction]);
			}
			while (!gotoNext)
			{
				yield return 0;
			}
		}
		this.SetAnimation(this.currentActKeyList[2], true);
		yield return new WaitForSeconds(15f);
		SoundManager.PlayVoice(cueName, this.currentVoiceList[3]);
		this.SetAnimation(this.currentActKeyList[3], false, delegate
		{
			this.SetAnimation(this.currentActKeyList[2], true);
		});
		yield return new WaitForSeconds(15f);
		SoundManager.PlayVoice(cueName, this.currentVoiceList[4]);
		this.SetAnimation(this.currentActKeyList[4], false, delegate
		{
			this.SetAnimation(this.currentActKeyList[2], true);
		});
		yield return new WaitForSeconds(15f);
		SoundManager.PlayVoice(cueName, this.currentVoiceList[5]);
		this.SetAnimation(this.currentActKeyList[5], true);
		this.isSleep = true;
		yield break;
	}

	// Token: 0x06001D9C RID: 7580 RVA: 0x0017028E File Offset: 0x0016E48E
	private IEnumerator InterruptMotionCtrl()
	{
		this.nextStepinterruptMotion = false;
		RenderTextureChara.InterruptMotion interruptMotion = this.interruptMotionList[0];
		this.SetAnimation(interruptMotion.key, false, delegate
		{
			this.nextStepinterruptMotion = true;
			this.SetAnimation(this.resetMotion, true);
		});
		while (!this.nextStepinterruptMotion)
		{
			yield return null;
		}
		this.interruptMotionList.RemoveAt(0);
		yield break;
	}

	// Token: 0x06001D9D RID: 7581 RVA: 0x001702A0 File Offset: 0x0016E4A0
	private void SetupCollision()
	{
		if (DataManager.DmChara != null)
		{
			float num = 1.8f;
			if (ItemDef.Id2Kind(this.charaId) == ItemDef.Kind.CHARA)
			{
				CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(this.charaId);
				float num2 = charaStaticData.baseData.width;
				num = charaStaticData.baseData.height;
			}
			Transform transform = this.dispChara.transform;
			if (transform.Find("Head") == null)
			{
				GameObject gameObject = new GameObject("Head");
				gameObject.transform.SetParent(transform, true);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			}
			if (transform.Find("Head").GetComponent<CapsuleCollider>() == null)
			{
				transform.Find("Head").gameObject.AddComponent<CapsuleCollider>();
			}
			CapsuleCollider component = transform.Find("Head").GetComponent<CapsuleCollider>();
			if (component != null)
			{
				component.center = ((this.charaMH != null) ? new Vector3(0f, num * 0.6666667f + num * 0.33333334f * 0.5f, 0f) : default(Vector3));
				component.radius = num * 0.33333334f * 0.5f;
				component.height = num * 0.33333334f;
			}
			if (transform.Find("Body") == null)
			{
				GameObject gameObject2 = new GameObject("Body");
				gameObject2.transform.SetParent(transform, true);
				gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
			}
			if (transform.Find("Body").GetComponent<CapsuleCollider>() == null)
			{
				transform.Find("Body").gameObject.AddComponent<CapsuleCollider>();
			}
			CapsuleCollider component2 = transform.Find("Body").GetComponent<CapsuleCollider>();
			if (component2 != null)
			{
				component2.center = ((this.charaMH != null) ? new Vector3(0f, num * 0.33333334f, 0f) : default(Vector3));
				component2.radius = num * 0.33333334f * 0.5f;
				component2.height = num * 0.6666667f;
			}
		}
	}

	// Token: 0x06001D9E RID: 7582 RVA: 0x0017051C File Offset: 0x0016E71C
	private void SetupLeaveAloneReaction(RenderTextureChara.CharaActType firstAction)
	{
		SGNFW.Touch.Manager.UnRegisterTap(new SGNFW.Touch.Manager.SingleAction(this.OnTouchTap));
		this.leaveAloneReactionInternal = Singleton<SceneManager>.Instance.StartCoroutine(this.LeaveAloneReactionInternal(firstAction));
		SGNFW.Touch.Manager.RegisterTap(new SGNFW.Touch.Manager.SingleAction(this.OnTouchTap));
		this.isSleep = false;
		this.isTouchedFace = (this.isTouchedBody = false);
	}

	// Token: 0x06001D9F RID: 7583 RVA: 0x0017057C File Offset: 0x0016E77C
	public void SetupEnableTouch(int charaId, int layer, bool isShop = false, int equipClothImageId = 0, bool equipLongSkirt = false)
	{
		this.currentActKeyList = new List<CharaMotionDefine.ActKey>(isShop ? RenderTextureChara.ActKeyListShop : RenderTextureChara.ActKeyListDefault);
		this.currentVoiceList = new List<VOICE_TYPE>(isShop ? RenderTextureChara.VoiceTypeListShop : RenderTextureChara.VoiceTypeListDefault);
		this.Setup(charaId, layer, CharaMotionDefine.ActKey.INVALID, equipClothImageId, equipLongSkirt, true, null, true, null, 0f, null, false, false, false);
	}

	// Token: 0x06001DA0 RID: 7584 RVA: 0x001705D8 File Offset: 0x0016E7D8
	public void SetupFace(int charaId, int layer, CharaMotionDefine.ActKey actKey = CharaMotionDefine.ActKey.INVALID, FacePackData fpd = null, int equipClothImageId = 0, bool equipLongSkirt = false, bool loop = true, RenderTextureChara.FinishCallback cb = null, bool enableTouch = false)
	{
		this.Setup(charaId, layer, actKey, equipClothImageId, equipLongSkirt, loop, cb, enableTouch, fpd, 0f, null, false, false, false);
	}

	// Token: 0x06001DA1 RID: 7585 RVA: 0x00170604 File Offset: 0x0016E804
	public void Setup(CharaPackData cpd, int layer, CharaMotionDefine.ActKey actKey = CharaMotionDefine.ActKey.INVALID, bool loop = true, RenderTextureChara.FinishCallback cb = null, bool enableTouch = false, FacePackData fpd = null, float startMotion = 0f, UnityAction setupAfterCb = null, bool isDisableVoice = false)
	{
		int num = 0;
		bool flag = false;
		if (cpd.dynamicData.OwnerType == CharaDynamicData.CharaOwnerType.User || DataManager.DmUserInfo.optionData.ViewClothesAffect)
		{
			num = cpd.equipClothImageId;
			flag = cpd.equipLongSkirt;
		}
		this.Setup(cpd.id, layer, actKey, num, flag, loop, cb, enableTouch, fpd, startMotion, setupAfterCb, isDisableVoice, false, false);
	}

	// Token: 0x06001DA2 RID: 7586 RVA: 0x00170664 File Offset: 0x0016E864
	public void Setup(int _charaId, int layer, CharaMotionDefine.ActKey actKey = CharaMotionDefine.ActKey.INVALID, int equipClothImageId = 0, bool equipLongSkirt = false, bool loop = true, RenderTextureChara.FinishCallback cb = null, bool enableTouch = false, FacePackData fpd = null, float startMotion = 0f, UnityAction setupAfterCb = null, bool forceUpdate = false, bool forceSetupCollision = false, bool isDisableVoice = false)
	{
		this.dispChara.layer = this.dispLight.layer + layer;
		int num = LayerMask.NameToLayer("Bloom");
		int num2 = LayerMask.NameToLayer("Bloom2");
		int num3 = num;
		foreach (Camera camera in this.renderTextureBase.GetComponentsInChildren<Camera>())
		{
			if (camera.name == "Camera" && camera != this.dispCamera && camera.gameObject.activeSelf && (camera.cullingMask & (1 << num)) != 0)
			{
				num3 = num2;
				break;
			}
		}
		if (num3 != num2)
		{
			foreach (Camera camera2 in Singleton<SceneManager>.Instance.BaseField.GetComponentsInChildren<Camera>())
			{
				if (camera2.gameObject.activeSelf && (camera2.cullingMask & (1 << LayerMask.NameToLayer("Bloom"))) != 0)
				{
					num3 = LayerMask.NameToLayer("Bloom2");
					break;
				}
			}
		}
		this.dispCamera.cullingMask = (1 << this.dispChara.layer) | (1 << num3);
		this.dispCameraDirect.cullingMask = (1 << this.dispChara.layer) | (1 << num3);
		if (num3 == num)
		{
			this.dispLight.GetComponent<Light>().cullingMask |= (1 << this.dispChara.layer) | (1 << num3);
			this.dispLight2.GetComponent<Light>().cullingMask &= ~(1 << this.dispChara.layer) & ~(1 << num3);
		}
		else
		{
			this.dispLight.GetComponent<Light>().cullingMask &= ~(1 << this.dispChara.layer) & ~(1 << num3);
			this.dispLight2.GetComponent<Light>().cullingMask |= (1 << this.dispChara.layer) | (1 << num3);
		}
		this.bloomLayer = num3;
		this.currentFinishCallback = cb;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		if (this.charaId != _charaId || this.clothImageId != equipClothImageId || forceUpdate)
		{
			flag = true;
			this.charaId = _charaId;
			this.clothImageId = equipClothImageId;
			this.longSkirt = equipLongSkirt;
		}
		this.isUpdateModel = flag;
		if (flag || this.currentMotion != actKey)
		{
			flag2 = true;
			this.currentMotion = actKey;
			this.resetMotion = actKey;
		}
		if (fpd != null && (flag || fpd.id != this.facePackDataId))
		{
			flag3 = true;
			this.facePackDataId = fpd.id;
		}
		if (flag || flag2 || flag3)
		{
			this.FinishedSetup = (this.isSetupInternal = false);
			this.dispTexture.gameObject.SetActive(false);
			this.dispCameraDirect.gameObject.SetActive(false);
			this.setupInternal = this.SetupInternal(flag, flag2, flag3, loop, enableTouch, fpd, startMotion, setupAfterCb, forceSetupCollision, isDisableVoice);
		}
	}

	// Token: 0x06001DA3 RID: 7587 RVA: 0x0017097C File Offset: 0x0016EB7C
	public void SetAnimation(CharaMotionDefine.ActKey key, bool loop = true)
	{
		if (this.isSetupInternal)
		{
			this.currentMotion = key;
			if (this.charaMH.GetCurrentAnimation() == CharaMotionDefine.ActKey.INVALID)
			{
				this.charaMH.PlayAnimation(this.currentMotion, loop, 1f, 0f, 0f, false);
				return;
			}
			this.charaMH.PlayAnimation(this.currentMotion, loop, 1f, 0.5f, 0.25f, false);
		}
	}

	// Token: 0x06001DA4 RID: 7588 RVA: 0x001709EA File Offset: 0x0016EBEA
	public void SetAnimation(CharaMotionDefine.ActKey key, bool loop, RenderTextureChara.FinishCallback cb = null)
	{
		this.SetAnimation(key, loop);
		if (this.isSetupInternal)
		{
			this.currentFinishCallback = cb;
		}
	}

	// Token: 0x06001DA5 RID: 7589 RVA: 0x00170A03 File Offset: 0x0016EC03
	public void SetAnimation(CharaMotionDefine.ActKey key, bool loop, float startTime, RenderTextureChara.FinishCallback cb = null)
	{
		if (this.isSetupInternal)
		{
			this.currentMotion = key;
			this.charaMH.PlayAnimationByAuth(key.ToString(), startTime, 1f, loop);
		}
		if (this.isSetupInternal)
		{
			this.currentFinishCallback = cb;
		}
	}

	// Token: 0x06001DA6 RID: 7590 RVA: 0x00170A43 File Offset: 0x0016EC43
	public void SetAnimation(string key, bool loop, float startTime, RenderTextureChara.FinishCallback cb = null)
	{
		if (this.isSetupInternal)
		{
			this.charaMH.PlayAnimationByAuth(key, startTime, 1f, loop);
		}
		if (this.isSetupInternal)
		{
			this.currentFinishCallback = cb;
		}
	}

	// Token: 0x06001DA7 RID: 7591 RVA: 0x00170A70 File Offset: 0x0016EC70
	public void StopAnimation()
	{
		if (this.isSetupInternal)
		{
			this.charaMH.SetAnimationSpeed(0f);
		}
	}

	// Token: 0x06001DA8 RID: 7592 RVA: 0x00170A8A File Offset: 0x0016EC8A
	public void RestartAnimation()
	{
		if (this.isSetupInternal)
		{
			this.charaMH.SetAnimationSpeed(1f);
		}
	}

	// Token: 0x06001DA9 RID: 7593 RVA: 0x00170AA4 File Offset: 0x0016ECA4
	public bool IsCurrentAnimation(CharaMotionDefine.ActKey key)
	{
		return this.isSetupInternal && this.charaMH.IsCurrentAnimation(key);
	}

	// Token: 0x06001DAA RID: 7594 RVA: 0x00170ABC File Offset: 0x0016ECBC
	public float AnimationLength()
	{
		if (!this.isSetupInternal)
		{
			return 0f;
		}
		return this.charaMH.GetAnimationLength(this.charaMH.GetCurrentAnimation().ToString());
	}

	// Token: 0x06001DAB RID: 7595 RVA: 0x00170AFC File Offset: 0x0016ECFC
	public float AnimationTime()
	{
		if (!this.isSetupInternal)
		{
			return 0f;
		}
		return this.charaMH.GetAnimationTime(this.charaMH.GetCurrentAnimation().ToString());
	}

	// Token: 0x06001DAC RID: 7596 RVA: 0x00170B3B File Offset: 0x0016ED3B
	public Vector3 GetNodePos(string node)
	{
		if (!this.isSetupInternal)
		{
			return Vector3.zero;
		}
		return this.charaMH.GetNodePos(node) - this.charaMH.transform.position;
	}

	// Token: 0x06001DAD RID: 7597 RVA: 0x00170B6C File Offset: 0x0016ED6C
	public void PlayVoice(VOICE_TYPE vt)
	{
		if (this.isSetupInternal)
		{
			SoundManager.PlayVoice(this.charaMH.loadVoiceCueSheetName, vt);
		}
	}

	// Token: 0x06001DAE RID: 7598 RVA: 0x00170B88 File Offset: 0x0016ED88
	public void StopVoice()
	{
		if (this.isSetupInternal)
		{
			SoundManager.Stop(this.charaMH.loadVoiceCueSheetName);
		}
	}

	// Token: 0x06001DAF RID: 7599 RVA: 0x00170BA2 File Offset: 0x0016EDA2
	public void SetFacePack(FacePackData packData)
	{
		if (this.isSetupInternal && packData != null)
		{
			this.charaMH.enabledFaceMotion = false;
			this.charaMH.SetFacePackData(packData, null, null);
		}
	}

	// Token: 0x06001DB0 RID: 7600 RVA: 0x00170BCF File Offset: 0x0016EDCF
	public void SetCameraPosition(Vector3 pos)
	{
		this.dispCamera.transform.localPosition = pos;
		this.dispCameraDirect.transform.localPosition = pos;
	}

	// Token: 0x06001DB1 RID: 7601 RVA: 0x00170BF3 File Offset: 0x0016EDF3
	public Transform GetChara()
	{
		return this.dispChara.transform;
	}

	// Token: 0x06001DB2 RID: 7602 RVA: 0x00170C00 File Offset: 0x0016EE00
	public void DispLuckyEyeEffect(bool isDisp, int colorIndex)
	{
		if (isDisp && this.dispLuckyEyeEffect == null)
		{
			this.dispLuckyEyeEffect = base.StartCoroutine(this.DispLuckyEyeEffectInternal(colorIndex));
		}
		if (this.luckyEyeEffect != null)
		{
			this.luckyEyeEffect.SetActive(isDisp);
		}
	}

	// Token: 0x06001DB3 RID: 7603 RVA: 0x00170C34 File Offset: 0x0016EE34
	private IEnumerator DispLuckyEyeEffectInternal(int colorIndex)
	{
		if (this.luckyEyeEffect != null)
		{
			string effectName2 = this.luckyEyeEffect.EffectName;
			EffectManager.DestroyEffect(this.luckyEyeEffect);
			EffectManager.UnloadEffect(effectName2, AssetManager.OWNER.CharaModel);
			this.luckyEyeEffect = null;
		}
		string effectName = "Ef_info_lb_eye_black";
		switch (colorIndex)
		{
		case 0:
			effectName = "Ef_info_lb_eye_black";
			break;
		case 1:
			effectName = "Ef_info_lb_eye_blue";
			break;
		case 2:
			effectName = "Ef_info_lb_eye_red";
			break;
		case 3:
			effectName = "Ef_info_lb_eye_white";
			break;
		case 4:
			effectName = "Ef_info_lb_eye_green";
			break;
		}
		EffectManager.ReqLoadEffect(effectName, AssetManager.OWNER.CharaModel, 0, null);
		while (!EffectManager.IsLoadFinishEffect(effectName))
		{
			yield return null;
		}
		while (!this.isSetupInternal)
		{
			yield return null;
		}
		this.luckyEyeEffect = EffectManager.InstantiateEffect(effectName, this.charaMH.GetNodeTransform("j_spine_c"), this.charaMH.gameObject.layer, 1f);
		this.luckyEyeEffect.SetActive(true);
		this.dispLuckyEyeEffect = null;
		yield break;
	}

	// Token: 0x06001DB4 RID: 7604 RVA: 0x00170C4C File Offset: 0x0016EE4C
	public void AddOnTouchCharaModelListener(UnityAction touchedFaceCb, UnityAction touchedBodyCb)
	{
		SGNFW.Touch.Manager.UnRegisterTap(new SGNFW.Touch.Manager.SingleAction(this.OnTouchTap));
		this.TouchedFaceCb = touchedFaceCb;
		this.TouchedBodyCb = touchedBodyCb;
		SGNFW.Touch.Manager.RegisterTap(new SGNFW.Touch.Manager.SingleAction(this.OnTouchTap));
		this.isTouchedFace = (this.isTouchedBody = false);
		this.enableTuouchCharaModel = true;
	}

	// Token: 0x06001DB5 RID: 7605 RVA: 0x00170CA0 File Offset: 0x0016EEA0
	public void OnValidate()
	{
		if (this.charaMH != null && this.charaMH.IsFinishInitialize())
		{
			this.dispTexture.GetComponent<RectTransform>().anchoredPosition = this.postion;
			this.dispCamera.fieldOfView = this.fieldOfView;
			bool flag = this.IsBloom();
			if (flag)
			{
				this.CalcDirectCameraFov();
			}
			this.UpdateCharaPosition(flag && !this.IsAlpha());
		}
	}

	// Token: 0x06001DB6 RID: 7606 RVA: 0x00170D14 File Offset: 0x0016EF14
	private void OnTouchTap(Info info)
	{
		this.dispTexture.raycastTarget = true;
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
		{
			position = info.CurrentPosition
		};
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, list);
		this.dispTexture.raycastTarget = false;
		if (list.Count <= 0 || !(list[0].gameObject == this.dispTexture.gameObject))
		{
			return;
		}
		if (this.safeAreaScaler == null)
		{
			return;
		}
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		RenderTexture targetTexture = this.dispCamera.targetTexture;
		Vector2 vector = new Vector2(info.CurrentPosition.x * ((float)targetTexture.width / (float)Screen.width), info.CurrentPosition.y * ((float)targetTexture.height / (float)Screen.height));
		float num = ((info.CurrentPosition.x - (float)Screen.width / 2f < 0f) ? (-(float)targetTexture.width / (float)Screen.width * 100f * this.safeAreaScaler.transform.localScale.x) : ((float)targetTexture.width / (float)Screen.width * 100f * this.safeAreaScaler.transform.localScale.x));
		float num2 = 0f;
		Vector2 anchoredPosition = this.dispTexture.GetComponent<RectTransform>().anchoredPosition;
		Vector2 vector2 = new Vector2(vector.x - anchoredPosition.x * this.safeAreaScaler.transform.localScale.x + num, vector.y - anchoredPosition.y * this.safeAreaScaler.transform.localScale.y - num2);
		Ray ray = this.dispCamera.ScreenPointToRay(vector2);
		RaycastHit raycastHit = default(RaycastHit);
		if (Physics.Raycast(ray, out raycastHit))
		{
			if (raycastHit.collider.name == "Head")
			{
				this.isTouchedFace = true;
				int num3 = this.TapCount;
				this.TapCount = num3 + 1;
				return;
			}
			if (raycastHit.collider.name == "Body")
			{
				this.isTouchedBody = true;
				int num3 = this.TapCount;
				this.TapCount = num3 + 1;
			}
		}
	}

	// Token: 0x06001DB7 RID: 7607 RVA: 0x00170F60 File Offset: 0x0016F160
	public static string CharaId2CueName(int charaId)
	{
		return "cv_" + charaId.ToString("0000");
	}

	// Token: 0x06001DB8 RID: 7608 RVA: 0x00170F78 File Offset: 0x0016F178
	public GameObject GetCloneCharaMovieObject()
	{
		return this.charaMovieObject;
	}

	// Token: 0x06001DB9 RID: 7609 RVA: 0x00170F80 File Offset: 0x0016F180
	private void SetupTmpBgCanvas()
	{
		this.beforeAlpha = this.IsAlpha();
		this.CloneBgObjects();
		if (this.originCloneBgObjs == null || this.originCloneBgObjs.Count == 0)
		{
			return;
		}
		this.tmpBgCanvas.gameObject.SetActive(true);
		this.tmpBgCanvas.worldCamera.depth = this.dispCameraDirect.depth - 1f;
	}

	// Token: 0x06001DBA RID: 7610 RVA: 0x00170FE8 File Offset: 0x0016F1E8
	private void GetParentInfo()
	{
		if (this.parentMask != null && this.parentAlpha != null)
		{
			return;
		}
		Transform transform = base.transform.parent;
		while (transform != null)
		{
			if (this.parentMask == null && transform.GetComponent<RectMask2D>() != null && transform.GetComponent<Canvas>() == null)
			{
				this.parentMask = transform.GetComponent<RectMask2D>();
				this.parentMaskSize = this.parentMask.GetComponent<RectTransform>().sizeDelta;
				this.parentMaskPos = this.parentMask.transform.position;
			}
			if (this.parentAlpha == null && transform.GetComponent<CanvasGroup>() != null)
			{
				this.parentAlpha = transform.GetComponent<CanvasGroup>();
			}
			if (this.parentMask != null && this.parentAlpha != null)
			{
				break;
			}
			transform = transform.parent;
		}
	}

	// Token: 0x06001DBB RID: 7611 RVA: 0x001710DC File Offset: 0x0016F2DC
	private void CloneBgObjects()
	{
		Vector3[] array = new Vector3[4];
		this.dispTexture.GetComponent<RectTransform>().GetWorldCorners(array);
		for (int i = 0; i < 4; i++)
		{
			array[i] = this.canvasCamera.WorldToScreenPoint(array[i]);
		}
		Transform transform = base.transform.parent;
		Transform transform2 = base.transform;
		this.originCloneBgObjs = new Dictionary<GameObject, GameObject>();
		this.originCloneGraphics = new Dictionary<MaskableGraphic, MaskableGraphic>();
		this.originCloneAECtrls = new Dictionary<PguiAECtrl, PguiAECtrl>();
		this.originCloneScrolls = new Dictionary<ReuseScroll, ReuseScroll>();
		this.originCloneScrollbars = new Dictionary<PguiScrollbar, PguiScrollbar>();
		this.originCloneAnims = new Dictionary<SimpleAnimation, SimpleAnimation>();
		this.originCloneAELayerConsts = new Dictionary<AELayerConstraint, AELayerConstraint>();
		while (transform != null)
		{
			if (transform == transform.root)
			{
				using (IEnumerator enumerator = transform.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Transform transform3 = (Transform)obj;
						if (transform3.gameObject.activeInHierarchy && transform3.GetSiblingIndex() < transform2.GetSiblingIndex())
						{
							this.CloneBgObjectsSub(transform3, -1, array);
						}
					}
					goto IL_011D;
				}
				goto IL_010F;
			}
			goto IL_010F;
			IL_011D:
			transform2 = transform;
			transform = transform.parent;
			continue;
			IL_010F:
			this.CloneBgObjectsSub(transform, transform2.GetSiblingIndex(), array);
			goto IL_011D;
		}
	}

	// Token: 0x06001DBC RID: 7612 RVA: 0x0017122C File Offset: 0x0016F42C
	private void CloneBgObjectsSub(Transform parent, int myselfIdx, Vector3[] targetRectScPos)
	{
		foreach (Transform transform in from Transform t in parent
			orderby t.GetSiblingIndex() descending
			select t)
		{
			if ((myselfIdx < 0 || transform.GetSiblingIndex() <= myselfIdx) && !(transform == base.transform) && ((transform.GetComponent<MaskableGraphic>() != null && this.IsIntersect(transform.GetComponent<RectTransform>(), targetRectScPos)) || (transform.GetSiblingIndex() != myselfIdx && this.HasBgObjectsRecursively(transform, targetRectScPos))))
			{
				GameObject gameObject = transform.gameObject;
				bool activeSelf = gameObject.activeSelf;
				Dictionary<GameObject, bool> dictionary = new Dictionary<GameObject, bool>();
				List<MaskableGraphic> list = new List<MaskableGraphic>();
				Dictionary<AELayerConstraint, bool[]> dictionary2 = new Dictionary<AELayerConstraint, bool[]>();
				MaskableGraphic[] array = gameObject.GetComponentsInChildren<MaskableGraphic>(true);
				int i = 0;
				while (i < array.Length)
				{
					MaskableGraphic maskableGraphic = array[i];
					dictionary[maskableGraphic.gameObject] = maskableGraphic.gameObject.activeInHierarchy;
					if (!maskableGraphic.enabled || maskableGraphic.color == Color.clear)
					{
						goto IL_0124;
					}
					AEImage aeimage = maskableGraphic as AEImage;
					if (((aeimage != null) ? aeimage.texture : null) == this.transparentTex)
					{
						goto IL_0124;
					}
					IL_012D:
					if (maskableGraphic.GetComponent<AELayerConstraint>() != null)
					{
						AELayerConstraint component = maskableGraphic.GetComponent<AELayerConstraint>();
						dictionary2[component] = new bool[] { component.lockVisibility, component.lockColor };
					}
					i++;
					continue;
					IL_0124:
					list.Add(maskableGraphic);
					goto IL_012D;
				}
				Dictionary<SimpleAnimation, object[]> dictionary3 = new Dictionary<SimpleAnimation, object[]>();
				foreach (SimpleAnimation simpleAnimation in gameObject.GetComponentsInChildren<SimpleAnimation>(true))
				{
					if (!dictionary.ContainsKey(simpleAnimation.gameObject) && simpleAnimation.ExIsPlaying())
					{
						dictionary[simpleAnimation.gameObject] = simpleAnimation.gameObject.activeInHierarchy;
						dictionary3[simpleAnimation] = new object[]
						{
							simpleAnimation.ExGetLastPlayStateName(),
							simpleAnimation.ExGetAbsTime(),
							simpleAnimation.ExGetSpeed()
						};
					}
				}
				GameObject gameObject2;
				if (transform.GetSiblingIndex() != myselfIdx)
				{
					gameObject.SetActive(false);
					gameObject2 = Object.Instantiate<GameObject>(gameObject, this.tmpBgCanvas.transform);
				}
				else
				{
					gameObject2 = new GameObject();
					gameObject2.transform.SetParent(this.tmpBgCanvas.transform);
					MaskableGraphic maskableGraphic2;
					if (gameObject.TryGetComponent<MaskableGraphic>(out maskableGraphic2))
					{
						MaskableGraphic maskableGraphic3;
						if (maskableGraphic2 as AEImage != null)
						{
							maskableGraphic3 = gameObject2.AddComponent<AEImage>();
						}
						else if (maskableGraphic2 as Image != null)
						{
							maskableGraphic3 = gameObject2.AddComponent<Image>();
							(maskableGraphic3 as Image).type = (maskableGraphic2 as Image).type;
						}
						else if (maskableGraphic2 as RawImage != null)
						{
							maskableGraphic3 = gameObject2.AddComponent<RawImage>();
						}
						else if (maskableGraphic2 as Text != null)
						{
							maskableGraphic3 = gameObject2.AddComponent<Text>();
						}
						else
						{
							maskableGraphic3 = gameObject2.AddComponent<MaskableGraphic>();
						}
						maskableGraphic3.enabled = maskableGraphic2.enabled;
						maskableGraphic3.raycastTarget = maskableGraphic2.raycastTarget;
						maskableGraphic3.color = maskableGraphic2.color;
					}
				}
				gameObject2.name = gameObject.name;
				gameObject2.transform.position = gameObject.transform.position + this.tmpBgCanvas.transform.position - gameObject.transform.root.position;
				gameObject2.transform.localScale = gameObject.transform.lossyScale / gameObject.transform.root.localScale.x;
				gameObject2.transform.rotation = gameObject.transform.rotation;
				gameObject2.transform.SetSiblingIndex(1);
				this.originCloneBgObjs[gameObject] = gameObject2;
				gameObject.SetActive(activeSelf);
				Transform[] componentsInChildren = gameObject2.GetComponentsInChildren<Transform>();
				for (i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].gameObject.SetActive(false);
				}
				foreach (MaskableGraphic maskableGraphic4 in gameObject2.GetComponentsInChildren<MaskableGraphic>(true))
				{
					if (!(maskableGraphic4.gameObject.GetComponentInParent<RenderTextureChara>(true) != null))
					{
						GameObject gameObject3 = maskableGraphic4.gameObject;
						bool flag = gameObject3.name == "Chara_MovieImage";
						if (flag)
						{
							this.charaMovieObject = gameObject3;
							string text = "Gacha/Gacha_movie_" + this.charaId.ToString("0000");
							if (AssetManager.IsExsistAssetData(AssetManager.PREFIX_PATH_MOVIE + text))
							{
								gameObject3.SetActive(true);
								MoviePlayer.Play(gameObject3, text, false);
							}
							else
							{
								gameObject3.SetActive(false);
							}
						}
						string relativePath = this.GetRelativePath(maskableGraphic4.gameObject.transform, gameObject2.transform);
						MaskableGraphic maskableGraphic5;
						if (relativePath == "")
						{
							maskableGraphic5 = gameObject.GetComponent<MaskableGraphic>();
						}
						else
						{
							if (gameObject.transform.Find(relativePath) == null)
							{
								goto IL_066C;
							}
							maskableGraphic5 = gameObject.transform.Find(relativePath).GetComponent<MaskableGraphic>();
						}
						if (flag)
						{
							CriManaMovieControllerForUI component2 = maskableGraphic4.GetComponent<CriManaMovieControllerForUI>();
							CriManaMovieControllerForUI component3 = maskableGraphic5.GetComponent<CriManaMovieControllerForUI>();
							if (component3 != null)
							{
								Material material = new Material(component3.material);
								component2.material = material;
								component2.enabled = true;
							}
						}
						if (dictionary.TryGetValueEx(maskableGraphic5.gameObject, false))
						{
							Transform transform2 = maskableGraphic4.transform;
							while (transform2 != null && transform2 != gameObject2 && !transform2.gameObject.activeInHierarchy)
							{
								transform2.gameObject.SetActive(true);
								transform2 = transform2.parent;
							}
						}
						if (maskableGraphic5.GetComponent<AELayerConstraint>() != null && maskableGraphic4.GetComponent<AELayerConstraint>() != null)
						{
							AELayerConstraint component4 = maskableGraphic5.GetComponent<AELayerConstraint>();
							AELayerConstraint component5 = maskableGraphic4.GetComponent<AELayerConstraint>();
							component5.lockPosition = false;
							bool[] array3 = dictionary2.TryGetValueEx(component4, new bool[2]);
							component5.lockVisibility = array3[0];
							component5.lockColor = array3[1];
							if (component4.lockVisibility || component4.lockColor)
							{
								component4.lockVisibility = false;
								component4.lockColor = false;
								this.originCloneAELayerConsts[component4] = component5;
							}
						}
						this.SetTransparentGraphic(maskableGraphic5, maskableGraphic4, list);
						if (maskableGraphic5.GetComponent<PguiAECtrl>() != null && maskableGraphic4.GetComponent<PguiAECtrl>() != null)
						{
							PguiAECtrl component6 = maskableGraphic5.GetComponent<PguiAECtrl>();
							PguiAECtrl component7 = maskableGraphic4.GetComponent<PguiAECtrl>();
							this.originCloneAECtrls[component6] = component7;
						}
					}
					IL_066C:;
				}
				foreach (ReuseScroll reuseScroll in this.originCloneScrolls.Keys)
				{
					ReuseScroll reuseScroll2 = this.originCloneScrolls[reuseScroll];
					foreach (PguiButtonCtrl pguiButtonCtrl in reuseScroll2.GetComponentsInChildren<PguiButtonCtrl>())
					{
						string relativePath2 = this.GetRelativePath(pguiButtonCtrl.transform, reuseScroll2.transform);
						PguiButtonCtrl component8 = reuseScroll.transform.Find(relativePath2).GetComponent<PguiButtonCtrl>();
						pguiButtonCtrl.AddOnClickListener(component8.GetOnClickListener(), component8.GetSoundType());
					}
					reuseScroll.gameObject.SetActive(false);
				}
				foreach (PguiScrollbar pguiScrollbar in this.originCloneScrollbars.Keys)
				{
					pguiScrollbar.gameObject.SetActive(false);
				}
				foreach (SimpleAnimation simpleAnimation2 in gameObject2.GetComponentsInChildren<SimpleAnimation>(true))
				{
					if (simpleAnimation2.enabled)
					{
						GameObject gameObject4 = simpleAnimation2.gameObject;
						string relativePath3 = this.GetRelativePath(gameObject4.transform, gameObject2.transform);
						GameObject gameObject5;
						if (relativePath3 == "")
						{
							gameObject5 = gameObject;
						}
						else
						{
							if (gameObject.transform.Find(relativePath3) == null)
							{
								goto IL_08DA;
							}
							gameObject5 = gameObject.transform.Find(relativePath3).gameObject;
						}
						SimpleAnimation component9 = gameObject5.GetComponent<SimpleAnimation>();
						if (dictionary.TryGetValueEx(gameObject5, false))
						{
							Transform transform3 = gameObject4.transform;
							while (transform3 != null && transform3 != gameObject2 && !transform3.gameObject.activeInHierarchy)
							{
								transform3.gameObject.SetActive(true);
								transform3 = transform3.parent;
							}
						}
						this.originCloneAnims[component9] = simpleAnimation2;
						if (gameObject5.activeInHierarchy && dictionary3.ContainsKey(component9))
						{
							object[] array4 = dictionary3[component9];
							if ((string)array4[0] != "")
							{
								component9.ExPlayAnimation((string)array4[0], (float)array4[1], (float)array4[2]);
								simpleAnimation2.ExPlayAnimation((string)array4[0], (float)array4[1], (float)array4[2]);
							}
						}
					}
					IL_08DA:;
				}
			}
		}
	}

	// Token: 0x06001DBD RID: 7613 RVA: 0x00171B88 File Offset: 0x0016FD88
	private void SetTransparentGraphic(MaskableGraphic originMg, MaskableGraphic cloneMg, List<MaskableGraphic> originTransparentList)
	{
		if (cloneMg.GetComponentInParent<ReuseScroll>() != null)
		{
			ReuseScroll componentInParent = originMg.GetComponentInParent<ReuseScroll>();
			ReuseScroll componentInParent2 = cloneMg.GetComponentInParent<ReuseScroll>();
			this.originCloneScrolls[componentInParent] = componentInParent2;
			return;
		}
		if (cloneMg.GetComponentInParent<PguiScrollbar>() != null)
		{
			PguiScrollbar componentInParent3 = originMg.GetComponentInParent<PguiScrollbar>();
			PguiScrollbar componentInParent4 = cloneMg.GetComponentInParent<PguiScrollbar>();
			this.originCloneScrollbars[componentInParent3] = componentInParent4;
			return;
		}
		if (originMg as AEImage != null)
		{
			if (originTransparentList != null && originTransparentList.Contains(originMg) && originMg.enabled && originMg.color != Color.clear)
			{
				(cloneMg as AEImage).texture = this.transparentTex;
			}
			else
			{
				(cloneMg as AEImage).texture = (originMg as AEImage).texture;
			}
			(originMg as AEImage).texture = this.transparentTex;
			this.originCloneGraphics[originMg] = cloneMg;
			this.SyncRectSize(originMg, cloneMg);
			return;
		}
		if (originMg as Text != null || !originMg.raycastTarget)
		{
			if (originTransparentList != null && originTransparentList.Contains(originMg) && originMg.color != Color.clear)
			{
				cloneMg.enabled = false;
			}
			else
			{
				cloneMg.enabled = originMg.enabled;
			}
			originMg.enabled = false;
			this.originCloneGraphics[originMg] = cloneMg;
			this.SyncRectSize(originMg, cloneMg);
			return;
		}
		if (originTransparentList != null && originTransparentList.Contains(originMg) && originMg.enabled)
		{
			cloneMg.color = Color.clear;
		}
		else
		{
			cloneMg.color = originMg.color;
		}
		originMg.color = Color.clear;
		this.originCloneGraphics[originMg] = cloneMg;
		this.SyncRectSize(originMg, cloneMg);
	}

	// Token: 0x06001DBE RID: 7614 RVA: 0x00171D20 File Offset: 0x0016FF20
	private void SyncRectSize(MaskableGraphic originMg, MaskableGraphic cloneMg)
	{
		RectTransform component = originMg.GetComponent<RectTransform>();
		RectTransform component2 = cloneMg.GetComponent<RectTransform>();
		component2.pivot = component.pivot;
		Vector2 sizeDelta = component.sizeDelta;
		if (component.anchorMin != component.anchorMax)
		{
			component2.anchorMax = (component2.anchorMin = component.anchorMin);
			if (component.anchorMin.x != component.anchorMax.x)
			{
				sizeDelta.x = component.parent.GetComponent<RectTransform>().rect.width * (component.anchorMax.x - component.anchorMin.x) + component.offsetMax.x - component.offsetMin.x;
			}
			if (component.anchorMin.y != component.anchorMax.y)
			{
				sizeDelta.y = component.parent.GetComponent<RectTransform>().rect.height * (component.anchorMax.y - component.anchorMin.y) + component.offsetMax.y - component.offsetMin.y;
			}
		}
		component2.sizeDelta = sizeDelta;
	}

	// Token: 0x06001DBF RID: 7615 RVA: 0x00171E50 File Offset: 0x00170050
	private void RestoreTransparentGraphic(MaskableGraphic originMg, MaskableGraphic cloneMg)
	{
		if (originMg == null || cloneMg == null)
		{
			return;
		}
		if (originMg as AEImage != null)
		{
			(originMg as AEImage).texture = (cloneMg as AEImage).texture;
			return;
		}
		if (originMg as Text != null || !originMg.raycastTarget)
		{
			originMg.enabled = cloneMg.enabled;
			return;
		}
		originMg.color = cloneMg.color;
	}

	// Token: 0x06001DC0 RID: 7616 RVA: 0x00171EC4 File Offset: 0x001700C4
	private bool HasBgObjectsRecursively(Transform parent, Vector3[] targetRectScPos)
	{
		bool flag = false;
		foreach (Transform transform in from Transform t in parent
			orderby t.GetSiblingIndex() descending
			select t)
		{
			if (transform.gameObject.activeInHierarchy)
			{
				if (transform.GetComponent<MaskableGraphic>() == null || !transform.GetComponent<MaskableGraphic>().enabled || transform.GetComponent<MaskableGraphic>().color == Color.clear)
				{
					if (this.HasBgObjectsRecursively(transform, targetRectScPos))
					{
						flag = true;
					}
				}
				else if (this.IsIntersect(transform.GetComponent<RectTransform>(), targetRectScPos))
				{
					flag = true;
				}
			}
		}
		return flag;
	}

	// Token: 0x06001DC1 RID: 7617 RVA: 0x00171F90 File Offset: 0x00170190
	private bool IsIntersect(RectTransform rect, Vector3[] targetRectScPos)
	{
		if (rect == null)
		{
			return false;
		}
		Vector3[] array = new Vector3[4];
		rect.GetWorldCorners(array);
		for (int i = 0; i < 4; i++)
		{
			array[i] = this.canvasCamera.WorldToScreenPoint(array[i]);
		}
		float num = Mathf.Min(new float[]
		{
			array[0].x,
			array[1].x,
			array[2].x,
			array[3].x
		});
		float num2 = Mathf.Max(new float[]
		{
			array[0].x,
			array[1].x,
			array[2].x,
			array[3].x
		});
		float num3 = Mathf.Min(new float[]
		{
			array[0].y,
			array[1].y,
			array[2].y,
			array[3].y
		});
		float num4 = Mathf.Max(new float[]
		{
			array[0].y,
			array[1].y,
			array[2].y,
			array[3].y
		});
		float num5 = Mathf.Min(new float[]
		{
			targetRectScPos[0].x,
			targetRectScPos[1].x,
			targetRectScPos[2].x,
			targetRectScPos[3].x
		});
		float num6 = Mathf.Max(new float[]
		{
			targetRectScPos[0].x,
			targetRectScPos[1].x,
			targetRectScPos[2].x,
			targetRectScPos[3].x
		});
		float num7 = Mathf.Min(new float[]
		{
			targetRectScPos[0].y,
			targetRectScPos[1].y,
			targetRectScPos[2].y,
			targetRectScPos[3].y
		});
		float num8 = Mathf.Max(new float[]
		{
			targetRectScPos[0].y,
			targetRectScPos[1].y,
			targetRectScPos[2].y,
			targetRectScPos[3].y
		});
		bool flag = num <= num6 && num2 >= num5;
		bool flag2 = num3 <= num8 && num4 >= num7;
		if (num < num5)
		{
			targetRectScPos[0].x = (targetRectScPos[1].x = num);
		}
		if (num2 > num6)
		{
			targetRectScPos[2].x = (targetRectScPos[3].x = num2);
		}
		if (num3 < num7)
		{
			targetRectScPos[0].y = (targetRectScPos[3].y = num3);
		}
		if (num4 > num8)
		{
			targetRectScPos[1].y = (targetRectScPos[2].y = num4);
		}
		return flag && flag2;
	}

	// Token: 0x06001DC2 RID: 7618 RVA: 0x001722DC File Offset: 0x001704DC
	private void DisableTmpBgCanvas(bool isCanvasDestroy)
	{
		if (this.m_tmpBgCanvas != null)
		{
			bool flag = false;
			using (IEnumerator enumerator = this.tmpBgCanvas.transform.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((Transform)enumerator.Current).GetComponent<Camera>() == null)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				this.tmpBgCanvas.gameObject.SetActive(false);
				if (isCanvasDestroy)
				{
					CanvasManager.RecursiveDestroy(this.tmpBgCanvas.gameObject);
					Object.Destroy(this.tmpBgCanvas.gameObject);
				}
			}
		}
	}

	// Token: 0x06001DC3 RID: 7619 RVA: 0x00172390 File Offset: 0x00170590
	private void ResetTmpBgObjects()
	{
		if (this.originCloneScrolls != null)
		{
			foreach (ReuseScroll reuseScroll in this.originCloneScrolls.Keys)
			{
				ReuseScroll reuseScroll2 = this.originCloneScrolls[reuseScroll];
				if (!(reuseScroll == null) && !(reuseScroll2 == null))
				{
					reuseScroll.gameObject.SetActive(reuseScroll2.gameObject.activeSelf);
				}
			}
		}
		if (this.originCloneScrollbars != null)
		{
			foreach (PguiScrollbar pguiScrollbar in this.originCloneScrollbars.Keys)
			{
				PguiScrollbar pguiScrollbar2 = this.originCloneScrollbars[pguiScrollbar];
				if (!(pguiScrollbar == null) && !(pguiScrollbar2 == null))
				{
					pguiScrollbar.gameObject.SetActive(pguiScrollbar2.gameObject.activeSelf);
				}
			}
		}
		if (this.originCloneAELayerConsts != null)
		{
			foreach (AELayerConstraint aelayerConstraint in this.originCloneAELayerConsts.Keys)
			{
				AELayerConstraint aelayerConstraint2 = this.originCloneAELayerConsts[aelayerConstraint];
				if (!(aelayerConstraint == null) && !(aelayerConstraint2 == null))
				{
					aelayerConstraint.lockVisibility = aelayerConstraint2.lockVisibility;
					aelayerConstraint.lockColor = aelayerConstraint2.lockColor;
				}
			}
		}
		if (this.originCloneGraphics != null)
		{
			foreach (MaskableGraphic maskableGraphic in this.originCloneGraphics.Keys)
			{
				MaskableGraphic maskableGraphic2 = this.originCloneGraphics[maskableGraphic];
				if (!(maskableGraphic == null) && !(maskableGraphic2 == null))
				{
					this.RestoreTransparentGraphic(maskableGraphic, maskableGraphic2);
				}
			}
		}
		foreach (object obj in this.tmpBgCanvas.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.GetComponent<Camera>() == null)
			{
				Object.Destroy(transform.gameObject);
			}
		}
		this.originCloneBgObjs = null;
		this.originCloneGraphics = null;
		this.originCloneAECtrls = null;
		this.originCloneScrolls = null;
		this.originCloneScrollbars = null;
		this.originCloneAnims = null;
		this.originCloneAELayerConsts = null;
	}

	// Token: 0x06001DC4 RID: 7620 RVA: 0x0017263C File Offset: 0x0017083C
	public bool IsBloom()
	{
		if (this.m_dispChara == null)
		{
			return false;
		}
		List<EffectData> charaEffect = this.dispChara.GetComponent<CharaModelHandle>().charaEffect;
		return charaEffect != null && charaEffect.Count > 0 && charaEffect[0] != null;
	}

	// Token: 0x06001DC5 RID: 7621 RVA: 0x00172682 File Offset: 0x00170882
	private bool IsAlpha()
	{
		return this.parentAlpha != null && this.parentAlpha.alpha < 1f;
	}

	// Token: 0x06001DC6 RID: 7622 RVA: 0x001726A8 File Offset: 0x001708A8
	private bool IsMaskInvisible()
	{
		return this.parentMask != null && (this.parentMask.GetComponent<RectTransform>().rect.width == 0f || this.parentMask.GetComponent<RectTransform>().rect.height == 0f);
	}

	// Token: 0x06001DC7 RID: 7623 RVA: 0x00172708 File Offset: 0x00170908
	private string GetRelativePath(Transform target, Transform parent)
	{
		string text = "";
		Transform transform = target;
		while (transform != parent)
		{
			if (text != "")
			{
				text = "/" + text;
			}
			text = transform.name + text;
			transform = transform.parent;
		}
		return text;
	}

	// Token: 0x040015AB RID: 5547
	[HideInInspector]
	public Shader renderCharaShader;

	// Token: 0x040015AC RID: 5548
	private static readonly List<CharaMotionDefine.ActKey> ActKeyListDefault = new List<CharaMotionDefine.ActKey>
	{
		CharaMotionDefine.ActKey.INVALID,
		CharaMotionDefine.ActKey.S_IN,
		CharaMotionDefine.ActKey.SCENARIO_STAND_BY,
		CharaMotionDefine.ActKey.S_WAIT1,
		CharaMotionDefine.ActKey.S_WAIT2,
		CharaMotionDefine.ActKey.S_WAIT3,
		CharaMotionDefine.ActKey.S_FACETOUCH,
		CharaMotionDefine.ActKey.S_BODY,
		CharaMotionDefine.ActKey.S_WAKEUP
	};

	// Token: 0x040015AD RID: 5549
	private static readonly List<VOICE_TYPE> VoiceTypeListDefault = new List<VOICE_TYPE>
	{
		VOICE_TYPE.NONE,
		VOICE_TYPE.CAL01,
		VOICE_TYPE.NONE,
		VOICE_TYPE.SWT01,
		VOICE_TYPE.SWT02,
		VOICE_TYPE.SWT03,
		VOICE_TYPE.SFA01,
		VOICE_TYPE.SBD01,
		VOICE_TYPE.SWA01
	};

	// Token: 0x040015AE RID: 5550
	private static readonly List<CharaMotionDefine.ActKey> ActKeyListShop = new List<CharaMotionDefine.ActKey>
	{
		CharaMotionDefine.ActKey.INVALID,
		CharaMotionDefine.ActKey.SHOP_IN,
		CharaMotionDefine.ActKey.SHOP_IDLING,
		CharaMotionDefine.ActKey.S_WAIT1,
		CharaMotionDefine.ActKey.S_WAIT2,
		CharaMotionDefine.ActKey.S_WAIT3,
		CharaMotionDefine.ActKey.S_FACETOUCH,
		CharaMotionDefine.ActKey.S_BODY,
		CharaMotionDefine.ActKey.S_WAKEUP
	};

	// Token: 0x040015AF RID: 5551
	private static readonly List<VOICE_TYPE> VoiceTypeListShop = new List<VOICE_TYPE>
	{
		VOICE_TYPE.NONE,
		VOICE_TYPE.SHP01,
		VOICE_TYPE.NONE,
		VOICE_TYPE.SWT01,
		VOICE_TYPE.SWT02,
		VOICE_TYPE.SWT03,
		VOICE_TYPE.SFA01,
		VOICE_TYPE.SBD01,
		VOICE_TYPE.SWA01
	};

	// Token: 0x040015B0 RID: 5552
	private List<RenderTextureChara.InterruptMotion> interruptMotionList = new List<RenderTextureChara.InterruptMotion>();

	// Token: 0x040015B1 RID: 5553
	private int charaId;

	// Token: 0x040015B2 RID: 5554
	private int clothImageId;

	// Token: 0x040015B3 RID: 5555
	private bool longSkirt;

	// Token: 0x040015B4 RID: 5556
	private string facePackDataId = "";

	// Token: 0x040015B6 RID: 5558
	public Vector2 postion;

	// Token: 0x040015B7 RID: 5559
	public Vector3 rotation;

	// Token: 0x040015B8 RID: 5560
	public float fieldOfView = 20f;

	// Token: 0x040015B9 RID: 5561
	public SafeAreaScaler safeAreaScaler;

	// Token: 0x040015BA RID: 5562
	private CharaModelHandle charaMH;

	// Token: 0x040015BC RID: 5564
	private bool isSetupInternal;

	// Token: 0x040015BD RID: 5565
	private bool isUpdateModel;

	// Token: 0x040015BE RID: 5566
	private CharaMotionDefine.ActKey currentMotion;

	// Token: 0x040015BF RID: 5567
	private CharaMotionDefine.ActKey resetMotion;

	// Token: 0x040015C0 RID: 5568
	private List<CharaMotionDefine.ActKey> currentActKeyList = new List<CharaMotionDefine.ActKey>(RenderTextureChara.ActKeyListDefault);

	// Token: 0x040015C1 RID: 5569
	private List<VOICE_TYPE> currentVoiceList = new List<VOICE_TYPE>(RenderTextureChara.VoiceTypeListDefault);

	// Token: 0x040015C2 RID: 5570
	private RenderTextureChara.FinishCallback currentFinishCallback;

	// Token: 0x040015C3 RID: 5571
	private bool currentLoop;

	// Token: 0x040015C4 RID: 5572
	private IEnumerator setupInternal;

	// Token: 0x040015C5 RID: 5573
	private Coroutine leaveAloneReactionInternal;

	// Token: 0x040015C6 RID: 5574
	private IEnumerator interruptMotionCtrl;

	// Token: 0x040015C7 RID: 5575
	private bool isSleep;

	// Token: 0x040015C8 RID: 5576
	private bool isTouchedFace;

	// Token: 0x040015C9 RID: 5577
	private bool isTouchedBody;

	// Token: 0x040015CA RID: 5578
	private bool enableTuouchCharaModel;

	// Token: 0x040015CB RID: 5579
	private UnityAction TouchedFaceCb;

	// Token: 0x040015CC RID: 5580
	private UnityAction TouchedBodyCb;

	// Token: 0x040015CD RID: 5581
	private Material m_renderCharaMaterial;

	// Token: 0x040015CE RID: 5582
	private Camera m_dispCamera;

	// Token: 0x040015CF RID: 5583
	private Camera m_dispCameraDirect;

	// Token: 0x040015D0 RID: 5584
	private GameObject m_dispChara;

	// Token: 0x040015D1 RID: 5585
	private RawImage m_dispTexture;

	// Token: 0x040015D2 RID: 5586
	private int width = 1024;

	// Token: 0x040015D3 RID: 5587
	private int height = 1024;

	// Token: 0x040015D4 RID: 5588
	private RenderTexture m_renderTexture;

	// Token: 0x040015D5 RID: 5589
	private CriAtomExPlayback checkPlayVoiceSceneIn;

	// Token: 0x040015D6 RID: 5590
	private CriAtomExPlayback checkPlayVoice1stAction;

	// Token: 0x040015D7 RID: 5591
	private static GameObject m_dispLight = null;

	// Token: 0x040015D8 RID: 5592
	private static GameObject m_dispLight2 = null;

	// Token: 0x040015D9 RID: 5593
	private Camera canvasCamera;

	// Token: 0x040015DA RID: 5594
	private EffectData luckyEyeEffect;

	// Token: 0x040015DB RID: 5595
	private Coroutine dispLuckyEyeEffect;

	// Token: 0x040015DC RID: 5596
	private static Transform m_renderTextureBase = null;

	// Token: 0x040015DD RID: 5597
	private int bloomLayer;

	// Token: 0x040015DE RID: 5598
	private Canvas m_tmpBgCanvas;

	// Token: 0x040015DF RID: 5599
	private Dictionary<GameObject, GameObject> originCloneBgObjs;

	// Token: 0x040015E0 RID: 5600
	private Dictionary<MaskableGraphic, MaskableGraphic> originCloneGraphics;

	// Token: 0x040015E1 RID: 5601
	private Dictionary<PguiAECtrl, PguiAECtrl> originCloneAECtrls;

	// Token: 0x040015E2 RID: 5602
	private Dictionary<ReuseScroll, ReuseScroll> originCloneScrolls;

	// Token: 0x040015E3 RID: 5603
	private Dictionary<PguiScrollbar, PguiScrollbar> originCloneScrollbars;

	// Token: 0x040015E4 RID: 5604
	private Dictionary<SimpleAnimation, SimpleAnimation> originCloneAnims;

	// Token: 0x040015E5 RID: 5605
	private Dictionary<AELayerConstraint, AELayerConstraint> originCloneAELayerConsts;

	// Token: 0x040015E6 RID: 5606
	private RectMask2D parentMask;

	// Token: 0x040015E7 RID: 5607
	private CanvasGroup parentAlpha;

	// Token: 0x040015E8 RID: 5608
	private static Texture2D m_trasnsparentTex = null;

	// Token: 0x040015E9 RID: 5609
	private Vector3 defaultCharaLocalPos = Vector3.zero;

	// Token: 0x040015EA RID: 5610
	private bool isFirstUpdateCharaPosition = true;

	// Token: 0x040015EB RID: 5611
	private bool isAwake;

	// Token: 0x040015EC RID: 5612
	private Vector2 parentMaskSize = Vector2.zero;

	// Token: 0x040015ED RID: 5613
	private Vector3 parentMaskPos = Vector3.zero;

	// Token: 0x040015EE RID: 5614
	private Vector2 canvasSize = Vector2.zero;

	// Token: 0x040015EF RID: 5615
	private Vector3 textureScale = Vector3.one;

	// Token: 0x040015F0 RID: 5616
	private bool beforeAlpha;

	// Token: 0x040015F1 RID: 5617
	private bool isOutOfCameraDirect;

	// Token: 0x040015F2 RID: 5618
	private GameObject charaMovieObject;

	// Token: 0x040015F3 RID: 5619
	private bool nextStepinterruptMotion;

	// Token: 0x02000F5A RID: 3930
	private enum CharaActType
	{
		// Token: 0x040056EF RID: 22255
		INVALID,
		// Token: 0x040056F0 RID: 22256
		SCENE_IN,
		// Token: 0x040056F1 RID: 22257
		NEUTRAL,
		// Token: 0x040056F2 RID: 22258
		WAIT1,
		// Token: 0x040056F3 RID: 22259
		WAIT2,
		// Token: 0x040056F4 RID: 22260
		WAIT3,
		// Token: 0x040056F5 RID: 22261
		FACETOUCH,
		// Token: 0x040056F6 RID: 22262
		BODYTOUCH,
		// Token: 0x040056F7 RID: 22263
		WAITTOUCH
	}

	// Token: 0x02000F5B RID: 3931
	public class InterruptMotion
	{
		// Token: 0x040056F8 RID: 22264
		public CharaMotionDefine.ActKey key;

		// Token: 0x040056F9 RID: 22265
		public string faceId;
	}

	// Token: 0x02000F5C RID: 3932
	// (Invoke) Token: 0x06004F4E RID: 20302
	public delegate void FinishCallback();
}
