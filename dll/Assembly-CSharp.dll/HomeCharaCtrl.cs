using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Touch;
using UnityEngine;

// Token: 0x0200014E RID: 334
public class HomeCharaCtrl : MonoBehaviour
{
	// Token: 0x17000377 RID: 887
	// (get) Token: 0x06001295 RID: 4757 RVA: 0x000E0547 File Offset: 0x000DE747
	// (set) Token: 0x06001296 RID: 4758 RVA: 0x000E054F File Offset: 0x000DE74F
	public bool isSetup { get; private set; }

	// Token: 0x06001297 RID: 4759 RVA: 0x000E0558 File Offset: 0x000DE758
	public void Init(GameObject stage, Camera camera, HomeFurnitureCtrl hfc)
	{
		this.fieldStage = stage;
		this.fieldCamera = camera;
		this.fieldCamera.cullingMask |= (1 << LayerMask.NameToLayer(HomeCharaCtrl.charaLayer)) | (1 << LayerMask.NameToLayer(HomeCharaCtrl.charaShadowLayer));
		this.furnitureMap = null;
		this.charaList = new Dictionary<int, CharaPackData>();
		this.contactList = new List<CharaContactStatic>();
		this.stayChara = new List<HomeCharaCtrl.CharaCtrl>();
		this.moveChara = new List<HomeCharaCtrl.CharaCtrl>();
		this.viewChara = null;
		this.viewContact = new Dictionary<CharaContactStatic.Situation, int>();
		this.viewPos = null;
		this.viewChg = false;
		this.moveInterval = -1f;
		this.furnitureCtrl = hfc;
		this.stayData = new List<Transform>();
		this.doubleData = new List<Transform>();
		this.rootData = new Dictionary<int, List<Transform>>();
		this.categoryData = new Dictionary<int, HomeFurnitureStatic.Category>();
		this.posBed = (this.posChr = null);
		this.touchId = 0;
		this.touchOut = false;
		this.noOpeTim = 0f;
		this.strokeAmount = -1f;
		this.strokeEff = -1f;
		this.strokePos = Vector2.zero;
		this.strokeOk = false;
		this.contactId = 0;
		this.isIcon = false;
		this.nightTime = false;
		this.isSetup = false;
	}

	// Token: 0x06001298 RID: 4760 RVA: 0x000E06A4 File Offset: 0x000DE8A4
	public void Setup(List<HomeFurnitureMapping> hfm, CharaPackData vc, bool night, int sf)
	{
		this.isSetup = false;
		string[] array = HomeCharaCtrl.emotionIcon;
		for (int i = 0; i < array.Length; i++)
		{
			EffectManager.ReqLoadEffect(array[i], AssetManager.OWNER.HomeChara, 0, null);
		}
		EffectManager.ReqLoadEffect(HomeCharaCtrl.strokeEffName, AssetManager.OWNER.HomeChara, 0, null);
		this.isIcon = false;
		this.touchId = 0;
		this.touchOut = false;
		this.noOpeTim = 0f;
		this.strokeAmount = -1f;
		this.strokeEff = -1f;
		this.strokePos = Vector2.zero;
		this.strokeOk = false;
		this.contactId = 0;
		this.nightTime = night;
		this.furnitureMap = hfm;
		this.charaList = DataManager.DmChara.GetUserCharaMap();
		this.contactList = ((vc == null) ? new List<CharaContactStatic>() : DataManager.DmChara.GetContactByChara(vc.id));
		if (vc != null)
		{
			this.contactList.RemoveAll((CharaContactStatic itm) => !vc.dynamicData.haveContactItemIdList.Contains(itm.GetId()));
		}
		this.stayChara = new List<HomeCharaCtrl.CharaCtrl>();
		this.moveChara = new List<HomeCharaCtrl.CharaCtrl>();
		HomeCharaCtrl.CharaCtrl charaCtrl2;
		if (vc != null)
		{
			HomeCharaCtrl.CharaCtrl charaCtrl = new HomeCharaCtrl.CharaCtrl();
			charaCtrl.id = vc.id;
			charaCtrl.chara = null;
			charaCtrl.ctrl = null;
			charaCtrl.dressup = null;
			charaCtrl2 = charaCtrl;
			charaCtrl.voiceSheet = new List<HomeCharaCtrl.CharaCtrl.VoiceSheet>();
		}
		else
		{
			charaCtrl2 = null;
		}
		this.viewChara = charaCtrl2;
		this.viewContact = new Dictionary<CharaContactStatic.Situation, int>();
		this.moveInterval = -1f;
		this.stayFriends = sf;
		List<Transform> list = new List<Transform>();
		List<Transform> list2 = new List<Transform>();
		this.posBed = (this.posChr = null);
		foreach (object obj in this.fieldStage.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name.StartsWith("pos_"))
			{
				list.Add(transform);
			}
			else if (transform.name.StartsWith("root_"))
			{
				list2.Add(transform);
			}
			if (transform.name.StartsWith("pos_bed_"))
			{
				this.posBed = transform;
			}
			else if (transform.name.StartsWith("pos_chair_"))
			{
				this.posChr = transform;
			}
		}
		this.stayData = new List<Transform>();
		char c5 = 'a';
		for (;;)
		{
			Transform transform2 = list2.Find((Transform itm) => itm.name.StartsWith("root_stay_" + c5.ToString()));
			if (transform2 == null)
			{
				break;
			}
			this.stayData.Add(transform2);
			char c4 = c5;
			c5 = c4 + '\u0001';
		}
		this.doubleData = new List<Transform>();
		char c = 'a';
		for (;;)
		{
			Transform transform3 = list2.Find((Transform itm) => itm.name.StartsWith("root_double_" + c.ToString()));
			if (transform3 == null)
			{
				break;
			}
			this.doubleData.Add(transform3);
			char c4 = c;
			c = c4 + '\u0001';
		}
		this.flyData = new List<List<Transform>>();
		char c2 = 'a';
		for (;;)
		{
			string n = "root_fly_" + c2.ToString() + "_";
			if (list2.Find((Transform itm) => itm.name.StartsWith(n)) == null)
			{
				break;
			}
			Transform transform4 = list2.Find((Transform itm) => itm.name.StartsWith(n + "start"));
			Transform transform5 = list2.Find((Transform itm) => itm.name.StartsWith(n + "end"));
			if (transform4 == null || transform5 == null)
			{
				transform4 = list2.Find((Transform itm) => itm.name.StartsWith(n + "a"));
				transform5 = list2.Find((Transform itm) => itm.name.StartsWith(n + "b"));
				if (transform4 != null || transform5 != null)
				{
					this.flyData.Add(new List<Transform> { transform4, transform5, transform4 });
				}
			}
			else
			{
				this.flyData.Add(new List<Transform> { transform4, transform5 });
				this.flyData.Add(new List<Transform> { transform5, transform4 });
			}
			c2 += '\u0001';
		}
		this.rootData = new Dictionary<int, List<Transform>>();
		this.categoryData = new Dictionary<int, HomeFurnitureStatic.Category>();
		Dictionary<int, HomePlacementStatic> homePlacementStaticMap = DataManager.DmHome.GetHomePlacementStaticMap();
		foreach (int num in homePlacementStaticMap.Keys)
		{
			string text = homePlacementStaticMap[num].locatorName;
			Transform transform6 = this.fieldStage.transform.Find(text);
			if (!(transform6 == null))
			{
				text = text.Replace("pos_", "root_") + "_";
				char c3 = 'a';
				for (;;)
				{
					transform6 = this.fieldStage.transform.Find(text + c3.ToString());
					if (transform6 == null)
					{
						break;
					}
					if (!this.rootData.ContainsKey(num))
					{
						this.rootData.Add(num, new List<Transform>());
					}
					this.rootData[num].Add(transform6);
					c3 += '\u0001';
				}
				this.categoryData.Add(num, homePlacementStaticMap[num].enableFurnitureCategory);
			}
		}
		this.Update();
	}

	// Token: 0x06001299 RID: 4761 RVA: 0x000E0C4C File Offset: 0x000DEE4C
	public void SetNight(bool night)
	{
		this.nightTime = night;
	}

	// Token: 0x0600129A RID: 4762 RVA: 0x000E0C58 File Offset: 0x000DEE58
	public void TearDown()
	{
		this.isSetup = false;
		this.furnitureMap = null;
		this.charaList = new Dictionary<int, CharaPackData>();
		this.contactList = new List<CharaContactStatic>();
		foreach (HomeCharaCtrl.CharaCtrl charaCtrl in this.stayChara)
		{
			this.DestChara(charaCtrl);
		}
		this.stayChara = new List<HomeCharaCtrl.CharaCtrl>();
		foreach (HomeCharaCtrl.CharaCtrl charaCtrl2 in this.moveChara)
		{
			this.DestChara(charaCtrl2);
		}
		this.moveChara = new List<HomeCharaCtrl.CharaCtrl>();
		if (this.viewChara != null)
		{
			this.DestChara(this.viewChara);
		}
		this.viewChara = null;
		this.viewContact = new Dictionary<CharaContactStatic.Situation, int>();
		this.stayData = new List<Transform>();
		this.doubleData = new List<Transform>();
		this.rootData = new Dictionary<int, List<Transform>>();
		this.categoryData = new Dictionary<int, HomeFurnitureStatic.Category>();
		this.isIcon = false;
		string[] array = HomeCharaCtrl.emotionIcon;
		for (int i = 0; i < array.Length; i++)
		{
			EffectManager.UnloadEffect(array[i], AssetManager.OWNER.HomeChara);
		}
		EffectManager.UnloadEffect(HomeCharaCtrl.strokeEffName, AssetManager.OWNER.HomeChara);
	}

	// Token: 0x0600129B RID: 4763 RVA: 0x000E0DB0 File Offset: 0x000DEFB0
	public int OnTap(Vector2 tap, float dist, bool outchk)
	{
		Dictionary<int, HomeCharaCtrl.CharaCtrl> dictionary = new Dictionary<int, HomeCharaCtrl.CharaCtrl>();
		if (this.viewChara != null && this.charaList.ContainsKey(this.viewChara.id) && this.viewChara.chara != null && this.viewChara.chara.IsFinishInitialize() && this.viewChara.busy >= 0)
		{
			dictionary.Add(this.viewChara.id, this.viewChara);
		}
		foreach (HomeCharaCtrl.CharaCtrl charaCtrl in this.stayChara)
		{
			if (this.charaList.ContainsKey(charaCtrl.id) && charaCtrl.chara != null && charaCtrl.chara.IsFinishInitialize() && charaCtrl.busy >= 0)
			{
				dictionary.Add(charaCtrl.id, charaCtrl);
			}
		}
		foreach (HomeCharaCtrl.CharaCtrl charaCtrl2 in this.moveChara)
		{
			if (this.charaList.ContainsKey(charaCtrl2.id) && charaCtrl2.chara != null && charaCtrl2.chara.IsFinishInitialize() && charaCtrl2.busy >= 0)
			{
				dictionary.Add(charaCtrl2.id, charaCtrl2);
			}
		}
		this.touchId = 0;
		foreach (int num in dictionary.Keys)
		{
			float z = this.CheckChara(dictionary[num].chara, tap).z;
			if (z >= 0f && z < dist)
			{
				dist = z;
				this.touchId = ((dictionary[num].busy == 0) ? num : (-num));
			}
		}
		this.touchOut = outchk && this.touchId == 0;
		return this.touchId;
	}

	// Token: 0x0600129C RID: 4764 RVA: 0x000E0FD8 File Offset: 0x000DF1D8
	public bool OnTouchStart(Info info)
	{
		if (this.viewChara != null && this.charaList.ContainsKey(this.viewChara.id) && this.viewChara.chara != null && this.viewChara.chara.IsFinishInitialize() && this.strokeEff < 0f && this.viewPos != null && !this.viewPos.name.StartsWith("pos_bed") && !this.viewPos.name.StartsWith("pos_chair") && this.CheckChara(this.viewChara.chara, info.CurrentPosition).z > 0f)
		{
			this.strokeAmount = (this.strokeOk ? (-1f) : 0f);
			this.strokeEff = 0f;
			this.strokePos = Vector2.zero;
			return true;
		}
		this.strokeAmount = -1f;
		this.strokeEff = -1f;
		return false;
	}

	// Token: 0x0600129D RID: 4765 RVA: 0x000E10F1 File Offset: 0x000DF2F1
	public bool OnTouchEnd(Info info)
	{
		this.strokeAmount = -1f;
		this.strokeEff = -1f;
		return false;
	}

	// Token: 0x0600129E RID: 4766 RVA: 0x000E110C File Offset: 0x000DF30C
	public bool OnTouchMove(Info info)
	{
		if (this.strokeAmount >= 0f)
		{
			Vector3 vector = this.CheckChara(this.viewChara.chara, info.CurrentPosition);
			if (vector.z > 0f)
			{
				this.strokeAmount += HomeCharaCtrl.deltaPosition(info.DeltaPosition).magnitude;
				this.strokePos = new Vector2(vector.x, vector.y);
			}
			return true;
		}
		return this.strokeEff >= 0f;
	}

	// Token: 0x0600129F RID: 4767 RVA: 0x000E1194 File Offset: 0x000DF394
	public static Vector2 deltaPosition(Vector2 dp)
	{
		float num = (float)Screen.width;
		float num2 = (float)Screen.height;
		if (Screen.height > Screen.width)
		{
			num2 = (float)Screen.width;
			num = (float)Screen.height;
		}
		return dp * ((num / 16f < num2 / 9f) ? (1280f / num) : (720f / num2));
	}

	// Token: 0x060012A0 RID: 4768 RVA: 0x000E11F0 File Offset: 0x000DF3F0
	private Vector3 CheckChara(CharaModelHandle cmh, Vector2 tap)
	{
		Vector3 vector = new Vector3(0f, 0f, -1f);
		if (cmh.GetAlpha() < 1f)
		{
			return vector;
		}
		Vector3 vector2 = cmh.transform.position;
		Vector3 vector3 = cmh.GetNodePos("j_head");
		if (vector2.y - vector3.y > 0.5f)
		{
			vector2.y += 0.2f;
			vector3.y -= 0.4f;
			Vector3 vector4 = vector2;
			vector2 = vector3;
			vector3 = vector4;
		}
		else
		{
			vector3.y += 0.4f;
		}
		float num = vector3.y - vector2.y;
		float num2 = 0.3f;
		if (num < 0.6f)
		{
			vector3.y += 0.6f - num;
			num = 0.6f;
			num2 = 0.6f;
		}
		Vector3 vector5 = this.fieldCamera.WorldToViewportPoint(vector2);
		if (vector5.x < 0f || vector5.x > 1f || vector5.y > 1f || vector5.z < 1f)
		{
			return vector;
		}
		vector5 = this.fieldCamera.WorldToViewportPoint(vector3);
		if (vector5.x < 0f || vector5.x > 1f || vector5.y < 0f || vector5.z < 1f)
		{
			return vector;
		}
		Vector2 vector6 = RectTransformUtility.WorldToScreenPoint(this.fieldCamera, vector2);
		if ((vector.y = tap.y - vector6.y) < 0f)
		{
			return vector;
		}
		Vector2 vector7 = RectTransformUtility.WorldToScreenPoint(this.fieldCamera, vector3);
		if (vector7.y < tap.y)
		{
			return vector;
		}
		float num3 = (vector7.y - vector6.y) * num2 / num;
		vector.x = tap.x - vector6.x;
		if (vector.x * vector.x > num3 * num3)
		{
			return vector;
		}
		num3 = num / (vector7.y - vector6.y);
		vector.x *= num3;
		vector.y *= num3;
		vector.z = Vector3.Distance((vector2 + vector3) * 0.5f, this.fieldCamera.transform.position);
		return vector;
	}

	// Token: 0x060012A1 RID: 4769 RVA: 0x000E143C File Offset: 0x000DF63C
	public int GetCharaStat(int id)
	{
		if (this.viewChara == null || this.viewChara.id != id)
		{
			foreach (HomeCharaCtrl.CharaCtrl charaCtrl in this.stayChara)
			{
				if (charaCtrl.id == id)
				{
					return (charaCtrl.chara == null || charaCtrl.busy < 0 || !this.IsViewChara(charaCtrl.chara)) ? 0 : (charaCtrl.slp ? 3 : 1);
				}
			}
			foreach (HomeCharaCtrl.CharaCtrl charaCtrl2 in this.moveChara)
			{
				if (charaCtrl2.id == id)
				{
					return (charaCtrl2.chara == null || charaCtrl2.busy < 0 || !this.IsViewChara(charaCtrl2.chara)) ? 0 : 2;
				}
			}
			return 0;
		}
		if (this.viewChara.chara == null || this.viewChara.busy < 0 || this.IsViewChara(this.viewChara.chara))
		{
			return 0;
		}
		if (!this.viewChara.slp)
		{
			return 1;
		}
		return 3;
	}

	// Token: 0x060012A2 RID: 4770 RVA: 0x000E1598 File Offset: 0x000DF798
	private bool IsViewChara(CharaModelHandle cmh)
	{
		return this.fieldCamera.WorldToViewportPoint(cmh.transform.position).z > 0.3f;
	}

	// Token: 0x060012A3 RID: 4771 RVA: 0x000E15BC File Offset: 0x000DF7BC
	public void SetViewPos(Transform vp)
	{
		this.viewPos = vp;
		this.viewChg = true;
		if (this.viewChara != null && this.viewChara.chara != null)
		{
			if (this.viewPos == null)
			{
				Vector3 vector;
				Vector3 vector2;
				float placePos = this.GetPlacePos(0, out vector, out vector2);
				this.viewChara.chara.transform.position = vector;
				this.viewChara.chara.transform.eulerAngles = new Vector3(0f, placePos, 0f);
			}
			else
			{
				this.viewChara.chara.transform.position = this.viewPos.position;
				this.viewChara.chara.transform.rotation = this.viewPos.rotation;
			}
			this.viewChara.slp = false;
			this.viewChara.sit = false;
			string text = ((this.viewPos == null) ? "" : this.viewPos.name);
			if (text.StartsWith("pos_bed"))
			{
				this.viewChara.slp = true;
				this.viewChara.chara.PlayAnimation(CharaMotionDefine.ActKey.H_ITEM_BED_LP, true, 1f, -1f, -1f, false);
				this.viewChara.chara.SetPosition(this.viewChara.chara.transform.localPosition + new Vector3(0f, 0.56f, 0f));
				this.viewChara.chara.SetRotation(this.viewChara.chara.transform.localEulerAngles.y + 180f);
				this.viewChara.chara.headFollowObj = (this.viewChara.chara.eyeFollowObj = (this.viewChara.chara.mouthFollowObj = null));
				this.viewChara.chara.shadowHeight = -20f;
				return;
			}
			if (text.StartsWith("pos_chair"))
			{
				this.viewChara.sit = true;
				this.viewChara.chara.PlayAnimation(CharaMotionDefine.ActKey.H_ITEM_CHR_LP, true, 1f, -1f, -1f, false);
				this.viewChara.chara.SetRotation(this.viewChara.chara.transform.localEulerAngles.y + 180f);
				this.viewChara.chara.headFollowObj = (this.viewChara.chara.eyeFollowObj = (this.viewChara.chara.mouthFollowObj = this.fieldCamera.transform));
				this.viewChara.chara.shadowHeight = -20f;
				return;
			}
			this.viewChara.chara.PlayAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true, 1f, -1f, -1f, false);
			this.viewChara.chara.headFollowObj = (this.viewChara.chara.eyeFollowObj = (this.viewChara.chara.mouthFollowObj = this.fieldCamera.transform));
			this.viewChara.chara.shadowHeight = this.viewChara.chara.transform.position.y + 0.03f;
		}
	}

	// Token: 0x060012A4 RID: 4772 RVA: 0x000E1928 File Offset: 0x000DFB28
	public void SetOpe()
	{
		this.noOpeTim = 0f;
	}

	// Token: 0x060012A5 RID: 4773 RVA: 0x000E1935 File Offset: 0x000DFB35
	public void CancelContact()
	{
		this.contactId = 0;
	}

	// Token: 0x060012A6 RID: 4774 RVA: 0x000E1940 File Offset: 0x000DFB40
	private void Update()
	{
		if (this.isSetup)
		{
			this.noOpeTim += TimeManager.DeltaTime;
			this.moveInterval -= TimeManager.DeltaTime;
			this.stayTime += TimeManager.DeltaTime;
		}
		this.isIcon = true;
		foreach (string text in HomeCharaCtrl.emotionIcon)
		{
			this.isIcon &= EffectManager.IsLoadFinishEffect(text);
		}
		bool flag = this.isIcon;
		List<int> list = new List<int>(this.charaList.Keys);
		List<int> list2 = new List<int>();
		if (this.furnitureMap != null)
		{
			foreach (HomeFurnitureMapping homeFurnitureMapping in this.furnitureMap)
			{
				if (this.rootData.ContainsKey(homeFurnitureMapping.placementId) && homeFurnitureMapping.furnitureId != 0)
				{
					list2.Add(homeFurnitureMapping.placementId);
				}
			}
		}
		if (this.viewChara != null)
		{
			this.CheckDressUp(this.viewChara);
			if (this.viewChara.ctrl != null && !this.viewChara.ctrl.MoveNext())
			{
				this.viewChara.ctrl = null;
			}
			foreach (HomeCharaCtrl.CharaCtrl.VoiceSheet voiceSheet in this.viewChara.voiceSheet)
			{
				if (voiceSheet.loader != null && !voiceSheet.loader.MoveNext())
				{
					voiceSheet.loader = null;
				}
			}
			this.SetAlpha(this.viewChara.chara);
		}
		List<HomeCharaCtrl.CharaCtrl> list3 = new List<HomeCharaCtrl.CharaCtrl>();
		foreach (HomeCharaCtrl.CharaCtrl charaCtrl in this.stayChara)
		{
			if (charaCtrl.typ == 0 && !charaCtrl.slp && !charaCtrl.sit && charaCtrl.chara != null && charaCtrl.colli < 0f)
			{
				list3.Add(charaCtrl);
			}
		}
		foreach (HomeCharaCtrl.CharaCtrl charaCtrl2 in this.moveChara)
		{
			if (charaCtrl2.typ == 0 && charaCtrl2.chara != null && charaCtrl2.colli < 0f)
			{
				list3.Add(charaCtrl2);
			}
		}
		foreach (HomeCharaCtrl.CharaCtrl charaCtrl3 in list3)
		{
			charaCtrl3.colli = 1f;
			float y = charaCtrl3.chara.transform.eulerAngles.y;
			foreach (HomeCharaCtrl.CharaCtrl charaCtrl4 in list3)
			{
				if (charaCtrl4 != charaCtrl3)
				{
					Vector3 vector = charaCtrl4.chara.transform.position - charaCtrl3.chara.transform.position;
					vector.y = 0f;
					float num = vector.magnitude / 2f;
					if (num > 0.15f && charaCtrl3.colli > num)
					{
						charaCtrl3.chara.transform.LookAt(charaCtrl4.chara.transform);
						if (Mathf.Abs(Mathf.DeltaAngle(y, charaCtrl3.chara.transform.eulerAngles.y)) < (1f - num) * 45f)
						{
							charaCtrl3.colli = num;
						}
					}
				}
			}
			charaCtrl3.chara.transform.eulerAngles = new Vector3(0f, y, 0f);
		}
		list3 = new List<HomeCharaCtrl.CharaCtrl>();
		foreach (HomeCharaCtrl.CharaCtrl charaCtrl5 in this.stayChara)
		{
			this.CheckDressUp(charaCtrl5);
			if (charaCtrl5.ctrl == null || !charaCtrl5.ctrl.MoveNext())
			{
				if (charaCtrl5.dressup == null)
				{
					list3.Add(charaCtrl5);
				}
			}
			else if (charaCtrl5.typ == 0)
			{
				if (list2.Contains(charaCtrl5.place))
				{
					if (!charaCtrl5.slp)
					{
						list2.Remove(charaCtrl5.place);
					}
				}
				else if (charaCtrl5.place > 0)
				{
					charaCtrl5.place = -charaCtrl5.place;
				}
			}
			foreach (HomeCharaCtrl.CharaCtrl.VoiceSheet voiceSheet2 in charaCtrl5.voiceSheet)
			{
				if (voiceSheet2.loader != null && !voiceSheet2.loader.MoveNext())
				{
					voiceSheet2.loader = null;
				}
			}
			this.SetAlpha(charaCtrl5.chara);
		}
		foreach (HomeCharaCtrl.CharaCtrl charaCtrl6 in list3)
		{
			this.DestChara(charaCtrl6);
			this.stayChara.Remove(charaCtrl6);
		}
		list3 = new List<HomeCharaCtrl.CharaCtrl>();
		foreach (HomeCharaCtrl.CharaCtrl charaCtrl7 in this.moveChara)
		{
			this.CheckDressUp(charaCtrl7);
			if (charaCtrl7.ctrl == null || !charaCtrl7.ctrl.MoveNext())
			{
				if (charaCtrl7.dressup == null)
				{
					list3.Add(charaCtrl7);
				}
			}
			else if (charaCtrl7.typ == 0)
			{
				if (list2.Contains(charaCtrl7.place))
				{
					list2.Remove(charaCtrl7.place);
				}
				else if (charaCtrl7.place > 0)
				{
					charaCtrl7.place = -charaCtrl7.place;
				}
			}
			foreach (HomeCharaCtrl.CharaCtrl.VoiceSheet voiceSheet3 in charaCtrl7.voiceSheet)
			{
				if (voiceSheet3.loader != null && !voiceSheet3.loader.MoveNext())
				{
					voiceSheet3.loader = null;
				}
			}
			this.SetAlpha(charaCtrl7.chara);
		}
		foreach (HomeCharaCtrl.CharaCtrl charaCtrl8 in list3)
		{
			this.DestChara(charaCtrl8);
			this.moveChara.Remove(charaCtrl8);
			if (this.moveInterval < 0f)
			{
				this.moveInterval = 3f;
			}
		}
		foreach (HomeCharaCtrl.CharaCtrl charaCtrl9 in this.stayChara)
		{
			if (charaCtrl9.chara == null || !charaCtrl9.chara.IsFinishInitialize())
			{
				flag = false;
			}
			HashSet<int> cid2 = DataManager.DmChara.GetSameCharaList(charaCtrl9.id, true);
			list.RemoveAll((int itm) => cid2.Contains(itm));
			if (charaCtrl9.typ == 0 && (charaCtrl9.place < 0 || (this.stayTime > 300f && !charaCtrl9.slp && !charaCtrl9.sit)))
			{
				int num2 = this.ChangePlace(Mathf.Abs(charaCtrl9.place), list2);
				if (num2 > 0)
				{
					list2.Remove(charaCtrl9.place = num2);
				}
				else if (charaCtrl9.place < 0)
				{
					charaCtrl9.place = 0;
				}
				this.stayTime = 0f;
			}
		}
		int num3 = 0;
		foreach (HomeCharaCtrl.CharaCtrl charaCtrl10 in this.moveChara)
		{
			if (charaCtrl10.direct && charaCtrl10.chara != null && !charaCtrl10.chara.IsFinishInitialize())
			{
				flag = false;
			}
			HashSet<int> cid3 = DataManager.DmChara.GetSameCharaList(charaCtrl10.id, true);
			list.RemoveAll((int itm) => cid3.Contains(itm));
			if (charaCtrl10.typ > 0)
			{
				num3 = 1;
			}
			else if (charaCtrl10.place < 0)
			{
				int num4 = ((Random.Range(0, 2) == 0) ? 0 : this.ChangePlace(Mathf.Abs(charaCtrl10.place), list2));
				if (num4 > 0)
				{
					list2.Remove(charaCtrl10.place = num4);
					charaCtrl10.ctrl = this.MoveChara(charaCtrl10);
				}
				else
				{
					charaCtrl10.place = 0;
				}
			}
		}
		if (this.viewChara != null)
		{
			if (this.viewChara.chara == null)
			{
				this.viewChara.ctrl = this.ViewChara(this.viewChara);
				this.viewChara.ctrl.MoveNext();
			}
			flag &= this.viewChara.chara.IsFinishInitialize();
		}
		else if (!this.furnitureCtrl.isSetup)
		{
			flag = false;
		}
		else if (this.stayChara.Count < 1)
		{
			this.stayTime = 0f;
			HomeCharaCtrl.CharaCtrl charaCtrl11 = new HomeCharaCtrl.CharaCtrl();
			charaCtrl11.id = ((list.Count > 0) ? list[Random.Range(0, list.Count)] : 0);
			if (list.Contains(this.stayFriends))
			{
				charaCtrl11.id = this.stayFriends;
			}
			int num5 = 0;
			foreach (int num6 in this.categoryData.Keys)
			{
				if (this.categoryData[num6] == HomeFurnitureStatic.Category.BED)
				{
					num5 = num6;
				}
			}
			if (charaCtrl11.id > 0 && (this.charaList[charaCtrl11.id].staticData.baseData.isNightTypeByHome ^ this.nightTime) && list2.Contains(num5) && Random.Range(0, 100) < 60)
			{
				charaCtrl11.typ = 0;
				charaCtrl11.place = num5;
			}
			else
			{
				if ((charaCtrl11.typ = Random.Range(0, 10)) < 3 && list2.Count > 0)
				{
					charaCtrl11.typ = 0;
				}
				else if (charaCtrl11.typ > 7 && list.Count > 1)
				{
					charaCtrl11.typ = 2;
				}
				else
				{
					charaCtrl11.typ = 1;
				}
				if (charaCtrl11.typ == 0)
				{
					charaCtrl11.place = list2[Random.Range(0, list2.Count)];
				}
				else if (charaCtrl11.typ == 1)
				{
					charaCtrl11.place = Random.Range(0, this.stayData.Count);
				}
				else
				{
					charaCtrl11.place = 0;
				}
			}
			if (charaCtrl11.id > 0 && this.charaList[charaCtrl11.id].equipLongSkirt && charaCtrl11.typ == 1 && this.stayData[charaCtrl11.place].name.EndsWith("_sit"))
			{
				charaCtrl11.id = 0;
			}
			if (charaCtrl11.id > 0)
			{
				flag = false;
				charaCtrl11.dress = this.charaList[charaCtrl11.id].equipClothImageId;
				charaCtrl11.longSkirt = this.charaList[charaCtrl11.id].equipLongSkirt;
				charaCtrl11.accessory = (this.charaList[charaCtrl11.id].dynamicData.dispAccessoryEffect ? this.charaList[charaCtrl11.id].dynamicData.accessory : null);
				charaCtrl11.root = 0;
				charaCtrl11.chara = null;
				charaCtrl11.direct = false;
				charaCtrl11.voiceSheet = new List<HomeCharaCtrl.CharaCtrl.VoiceSheet>();
				charaCtrl11.ctrl = this.StayChara(charaCtrl11);
				charaCtrl11.ctrl.MoveNext();
				this.stayChara.Add(charaCtrl11);
				if (charaCtrl11.typ > 1)
				{
					for (int j = 1; j < this.doubleData.Count; j++)
					{
						HashSet<int> cid = DataManager.DmChara.GetSameCharaList(charaCtrl11.id, true);
						list.RemoveAll((int itm) => cid.Contains(itm));
						if (list.Count <= 0)
						{
							break;
						}
						charaCtrl11 = new HomeCharaCtrl.CharaCtrl();
						charaCtrl11.id = list[Random.Range(0, list.Count)];
						charaCtrl11.dress = this.charaList[charaCtrl11.id].equipClothImageId;
						charaCtrl11.longSkirt = this.charaList[charaCtrl11.id].equipLongSkirt;
						charaCtrl11.accessory = (this.charaList[charaCtrl11.id].dynamicData.dispAccessoryEffect ? this.charaList[charaCtrl11.id].dynamicData.accessory : null);
						charaCtrl11.typ = 2;
						charaCtrl11.place = j;
						charaCtrl11.root = 0;
						charaCtrl11.chara = null;
						charaCtrl11.direct = false;
						charaCtrl11.voiceSheet = new List<HomeCharaCtrl.CharaCtrl.VoiceSheet>();
						charaCtrl11.ctrl = this.StayChara(charaCtrl11);
						charaCtrl11.ctrl.MoveNext();
						this.stayChara.Add(charaCtrl11);
					}
				}
			}
		}
		else if (this.moveChara.Count < 5 - this.stayChara.Count && this.moveInterval < 0f)
		{
			HomeCharaCtrl.CharaCtrl charaCtrl12 = new HomeCharaCtrl.CharaCtrl();
			charaCtrl12.id = ((list.Count > 0) ? list[Random.Range(0, list.Count)] : 0);
			charaCtrl12.typ = ((num3 > 0 || charaCtrl12.id <= 0 || !this.charaList[charaCtrl12.id].staticData.baseData.isFlyingTypeByHome) ? 0 : Random.Range(0, 2));
			charaCtrl12.place = ((charaCtrl12.typ == 0) ? ((list2.Count > 0) ? list2[Random.Range(0, list2.Count)] : 0) : Random.Range(0, this.flyData.Count));
			if (charaCtrl12.id > 0 && (charaCtrl12.typ > 0 || charaCtrl12.place > 0))
			{
				charaCtrl12.dress = this.charaList[charaCtrl12.id].equipClothImageId;
				charaCtrl12.longSkirt = this.charaList[charaCtrl12.id].equipLongSkirt;
				charaCtrl12.accessory = (this.charaList[charaCtrl12.id].dynamicData.dispAccessoryEffect ? this.charaList[charaCtrl12.id].dynamicData.accessory : null);
				charaCtrl12.root = 0;
				charaCtrl12.chara = null;
				charaCtrl12.direct = false;
				charaCtrl12.voiceSheet = new List<HomeCharaCtrl.CharaCtrl.VoiceSheet>();
				if (this.isSetup || this.moveChara.Count - num3 > 2 - this.stayChara.Count)
				{
					this.moveInterval = 1f;
				}
				else if (charaCtrl12.typ == 0)
				{
					CharaModelHandle.InitializeParam initializeParam = CharaModelHandle.InitializeParam.CreaateByCharaId(charaCtrl12.id, charaCtrl12.dress, charaCtrl12.longSkirt, false);
					if (AssetManager.IsNeedAssetDownload(CharaModelHandle.CHARA_MODEL_PATH + initializeParam.bodyModelName))
					{
						charaCtrl12.id = 0;
					}
					else
					{
						flag = false;
						charaCtrl12.chara = this.MakeChara(charaCtrl12.id, charaCtrl12.dress, charaCtrl12.longSkirt, charaCtrl12.accessory);
						this.SetVoiceSheet(charaCtrl12);
						Vector3 vector2;
						Vector3 vector3;
						charaCtrl12.chara.transform.eulerAngles = new Vector3(0f, this.GetPlacePos(charaCtrl12.place, out vector2, out vector3), 0f);
						charaCtrl12.chara.transform.position = vector2;
					}
				}
				if (charaCtrl12.id > 0)
				{
					charaCtrl12.ctrl = ((charaCtrl12.typ == 0) ? this.MoveChara(charaCtrl12) : this.FlyChara(charaCtrl12));
					charaCtrl12.ctrl.MoveNext();
					this.moveChara.Add(charaCtrl12);
				}
			}
		}
		if (flag)
		{
			this.isSetup = true;
		}
	}

	// Token: 0x060012A7 RID: 4775 RVA: 0x000E2B14 File Offset: 0x000E0D14
	private IEnumerator ViewChara(HomeCharaCtrl.CharaCtrl cc)
	{
		cc.busy = -1;
		cc.colli = 1f;
		cc.dress = this.charaList[cc.id].equipClothImageId;
		cc.longSkirt = this.charaList[cc.id].equipLongSkirt;
		cc.accessory = (this.charaList[cc.id].dynamicData.dispAccessoryEffect ? this.charaList[cc.id].dynamicData.accessory : null);
		cc.chara = this.MakeChara(cc.id, cc.dress, cc.longSkirt, cc.accessory);
		this.SetVoiceSheet(cc);
		this.SetViewPos(this.viewPos);
		this.viewChg = false;
		while (!cc.chara.IsFinishInitialize())
		{
			yield return null;
		}
		cc.chara.FadeIn(0.2f);
		cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
		cc.chara.SetEnableUpdateOffscreen();
		yield return null;
		IEnumerator ope = this.ViewCharaVoice(cc, VOICE_TYPE.PRF01);
		for (;;)
		{
			string text = ((this.viewPos == null) ? "" : this.viewPos.name);
			if (this.viewChg)
			{
				ope = null;
				this.viewChg = false;
				this.noOpeTim = 0f;
				this.strokeAmount = -1f;
				this.strokeOk = false;
				this.strokeEff = -1f;
			}
			else if (ope != null)
			{
				if (!ope.MoveNext())
				{
					ope = null;
					this.strokeOk = false;
				}
			}
			else if (this.strokeOk)
			{
				ope = this.ViewCharaStroke(cc);
				ope.MoveNext();
				this.strokeAmount = -1f;
			}
			else if (cc.id == this.touchId)
			{
				this.noOpeTim = 0f;
				this.contactId = this.touchId;
				this.touchId = 0;
				if (text.StartsWith("pos_bed"))
				{
					cc.busy = 1;
					ope = this.ViewCharaSleep(cc);
					ope.MoveNext();
				}
				else if (text.StartsWith("pos_chair"))
				{
					cc.busy = 1;
					ope = this.ViewCharaSitdown(cc);
					ope.MoveNext();
				}
				else
				{
					cc.busy = 1;
					ope = this.ViewCharaTouch(cc);
					ope.MoveNext();
				}
				this.strokeAmount = -1f;
				this.strokeEff = -1f;
			}
			else if (this.touchOut)
			{
				this.noOpeTim = 0f;
				this.touchOut = false;
				if (!text.StartsWith("pos_bed") && !text.StartsWith("pos_chair"))
				{
					cc.busy = 1;
					ope = this.ViewCharaOut(cc);
					ope.MoveNext();
				}
			}
			else if (this.noOpeTim > 7f)
			{
				if (text.StartsWith("pos_bed") || text.StartsWith("pos_chair"))
				{
					this.noOpeTim = 0f;
				}
				else
				{
					cc.busy = 1;
					ope = this.ViewCharaTurn(cc);
					ope.MoveNext();
				}
			}
			else
			{
				cc.busy = 0;
			}
			if (this.strokeEff >= 0f)
			{
				this.noOpeTim = 0f;
				if (this.strokeAmount > 1000f)
				{
					cc.busy = 1;
					this.strokeOk = true;
				}
				else if (this.strokeAmount >= 0f && this.strokeAmount > this.strokeEff + 150f)
				{
					this.strokeEff = this.strokeAmount;
					EffectData effectData = (EffectManager.IsLoadFinishEffect(HomeCharaCtrl.strokeEffName) ? EffectManager.InstantiateEffect(HomeCharaCtrl.strokeEffName, cc.chara.transform, LayerMask.NameToLayer(HomeCharaCtrl.charaLayer), 1f) : null);
					if (effectData != null)
					{
						effectData.effectObject.transform.localPosition = new Vector3(this.strokePos.x, this.strokePos.y, 0f);
						effectData.PlayEffect(false);
						cc.chara.SetEffect(effectData);
						SoundManager.Play("prd_se_friends_stroke", false, false);
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060012A8 RID: 4776 RVA: 0x000E2B2C File Offset: 0x000E0D2C
	private CharaContactStatic GetViewContact(CharaContactStatic.Situation st)
	{
		List<CharaContactStatic> list = this.contactList.FindAll((CharaContactStatic itm) => itm.SituationType == st);
		if (!this.viewContact.ContainsKey(st))
		{
			this.viewContact[st] = 0;
		}
		if (list.Count > 1)
		{
			list.RemoveAll((CharaContactStatic itm) => itm.GetId() == this.viewContact[st]);
		}
		CharaContactStatic charaContactStatic = ((list.Count > 0) ? list[Random.Range(0, list.Count)] : null);
		this.viewContact[st] = ((charaContactStatic == null) ? 0 : charaContactStatic.GetId());
		return charaContactStatic;
	}

	// Token: 0x060012A9 RID: 4777 RVA: 0x000E2BE3 File Offset: 0x000E0DE3
	private IEnumerator ViewCharaVoice(HomeCharaCtrl.CharaCtrl cc, VOICE_TYPE vt)
	{
		float tim = this.PlayVoice(cc, vt);
		while ((tim -= TimeManager.DeltaTime) > 0f)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060012AA RID: 4778 RVA: 0x000E2C00 File Offset: 0x000E0E00
	private IEnumerator ViewCharaSleep(HomeCharaCtrl.CharaCtrl cc)
	{
		CharaContactStatic charaContactStatic = this.GetViewContact(CharaContactStatic.Situation.SLEEP_TAP);
		string text = ((charaContactStatic == null) ? "" : charaContactStatic.MotionKey);
		string text2 = ((charaContactStatic == null) ? HomeCharaCtrl.ViewTouchSlp[Random.Range(0, HomeCharaCtrl.ViewTouchSlp.Count)].ToString() : ((charaContactStatic.VoiceKeyList.Count > 0) ? charaContactStatic.VoiceKeyList[Random.Range(0, charaContactStatic.VoiceKeyList.Count)] : ""));
		float tim = (string.IsNullOrEmpty(text2) ? 0f : this.PlayVoice(cc, text2));
		if (!string.IsNullOrEmpty(text))
		{
			cc.chara.PlayAnimation(text, false, 1f, 0.5f, 0.25f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
			yield return null;
			while (cc.chara.IsPlaying())
			{
				tim -= TimeManager.DeltaTime;
				yield return null;
			}
			cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_ITEM_BED_LP, true, 1f, 0.5f, 0.25f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = null));
		}
		while ((tim -= TimeManager.DeltaTime) > 0f)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060012AB RID: 4779 RVA: 0x000E2C16 File Offset: 0x000E0E16
	private IEnumerator ViewCharaSitdown(HomeCharaCtrl.CharaCtrl cc)
	{
		CharaContactStatic charaContactStatic = this.GetViewContact(CharaContactStatic.Situation.SITDOWN_TAP);
		string text = ((charaContactStatic == null) ? "" : charaContactStatic.MotionKey);
		string text2 = ((charaContactStatic == null) ? HomeCharaCtrl.ViewTouchsit[Random.Range(0, HomeCharaCtrl.ViewTouchsit.Count)].ToString() : ((charaContactStatic.VoiceKeyList.Count > 0) ? charaContactStatic.VoiceKeyList[Random.Range(0, charaContactStatic.VoiceKeyList.Count)] : ""));
		float tim = (string.IsNullOrEmpty(text2) ? 0f : this.PlayVoice(cc, text2));
		if (!string.IsNullOrEmpty(text))
		{
			cc.chara.PlayAnimation(text, false, 1f, 0.5f, 0.25f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
			yield return null;
			while (cc.chara.IsPlaying())
			{
				tim -= TimeManager.DeltaTime;
				yield return null;
			}
			cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_ITEM_CHR_LP, true, 1f, 0.5f, 0.25f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
		}
		while ((tim -= TimeManager.DeltaTime) > 0f)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060012AC RID: 4780 RVA: 0x000E2C2C File Offset: 0x000E0E2C
	private IEnumerator ViewCharaTouch(HomeCharaCtrl.CharaCtrl cc)
	{
		CharaContactStatic charaContactStatic = this.GetViewContact(CharaContactStatic.Situation.STAND_TAP);
		string text = ((charaContactStatic == null) ? HomeCharaCtrl.ViewTouchMot[Random.Range(0, HomeCharaCtrl.ViewTouchMot.Count)].ToString() : charaContactStatic.MotionKey);
		string text2 = ((charaContactStatic != null && charaContactStatic.VoiceKeyList.Count > 0) ? charaContactStatic.VoiceKeyList[Random.Range(0, charaContactStatic.VoiceKeyList.Count)] : "");
		float tim = (string.IsNullOrEmpty(text2) ? 0f : this.PlayVoice(cc, text2));
		if (!string.IsNullOrEmpty(text))
		{
			cc.chara.PlayAnimation(text, false, 1f, 0.5f, 0.25f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
			yield return null;
			while (cc.chara.IsPlaying())
			{
				tim -= TimeManager.DeltaTime;
				yield return null;
			}
			cc.chara.PlayAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true, 1f, 0.5f, 0.25f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
		}
		while ((tim -= TimeManager.DeltaTime) > 0f)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060012AD RID: 4781 RVA: 0x000E2C42 File Offset: 0x000E0E42
	private IEnumerator ViewCharaOut(HomeCharaCtrl.CharaCtrl cc)
	{
		CharaContactStatic charaContactStatic = this.GetViewContact(CharaContactStatic.Situation.STAND_OUT_TAP);
		string text = ((charaContactStatic == null) ? "" : charaContactStatic.MotionKey);
		string text2 = ((charaContactStatic != null && charaContactStatic.VoiceKeyList.Count > 0) ? charaContactStatic.VoiceKeyList[Random.Range(0, charaContactStatic.VoiceKeyList.Count)] : "");
		float tim = (string.IsNullOrEmpty(text2) ? 0f : this.PlayVoice(cc, text2));
		if (!string.IsNullOrEmpty(text))
		{
			cc.chara.PlayAnimation(text, false, 1f, 0.5f, 0.25f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
			yield return null;
			while (cc.chara.IsPlaying())
			{
				tim -= TimeManager.DeltaTime;
				yield return null;
			}
			cc.chara.PlayAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true, 1f, 0.5f, 0.25f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
		}
		while ((tim -= TimeManager.DeltaTime) > 0f)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060012AE RID: 4782 RVA: 0x000E2C58 File Offset: 0x000E0E58
	private IEnumerator ViewCharaStroke(HomeCharaCtrl.CharaCtrl cc)
	{
		CharaContactStatic charaContactStatic = this.GetViewContact(CharaContactStatic.Situation.STAND_STROKING);
		string text = ((charaContactStatic == null) ? CharaMotionDefine.ActKey.FRIENDSHIP_STROKE_REACTION.ToString() : charaContactStatic.MotionKey);
		string text2 = ((charaContactStatic == null) ? VOICE_TYPE.PRF02.ToString() : ((charaContactStatic.VoiceKeyList.Count > 0) ? charaContactStatic.VoiceKeyList[Random.Range(0, charaContactStatic.VoiceKeyList.Count)] : ""));
		float tim = (string.IsNullOrEmpty(text2) ? 0f : this.PlayVoice(cc, text2));
		if (!string.IsNullOrEmpty(text))
		{
			cc.chara.PlayAnimation(text, false, 1f, 0.5f, 0.25f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
			yield return null;
			while (cc.chara.IsPlaying())
			{
				tim -= TimeManager.DeltaTime;
				yield return null;
			}
			cc.chara.PlayAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true, 1f, 0.5f, 0.25f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
		}
		while ((tim -= TimeManager.DeltaTime) > 0f)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060012AF RID: 4783 RVA: 0x000E2C6E File Offset: 0x000E0E6E
	private IEnumerator ViewCharaTurn(HomeCharaCtrl.CharaCtrl cc)
	{
		CharaContactStatic ccs = this.GetViewContact(CharaContactStatic.Situation.STAND_NEGLECT);
		float yb = cc.chara.transform.eulerAngles.y;
		cc.chara.transform.LookAt(this.fieldCamera.transform);
		float yd = Mathf.DeltaAngle(yb, cc.chara.transform.eulerAngles.y);
		cc.chara.transform.eulerAngles = new Vector3(0f, yb, 0f);
		CharaMotionDefine.ActKey actKey = CharaMotionDefine.ActKey.INVALID;
		float yo = 0f;
		if (yd < -150f)
		{
			actKey = CharaMotionDefine.ActKey.H_HOM_LEFT_FULL_TURN;
			yo = -180f;
		}
		else if (yd < -50f)
		{
			actKey = CharaMotionDefine.ActKey.H_HOM_LEFT_HALF_TURN;
			yo = 90f;
		}
		else if (yd > 150f)
		{
			actKey = CharaMotionDefine.ActKey.H_HOM_RIGHT_FULL_TURN;
			yo = -180f;
		}
		else if (yd > 50f)
		{
			actKey = CharaMotionDefine.ActKey.H_HOM_RIGHT_HALF_TURN;
			yo = -90f;
		}
		bool flag = false;
		if (actKey != CharaMotionDefine.ActKey.INVALID)
		{
			cc.chara.PlayAnimation(actKey, false, 1f, 0.5f, 0.25f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
			yd = Mathf.DeltaAngle(0f, yd + yo);
			float animationTime;
			do
			{
				yield return null;
				animationTime = cc.chara.GetAnimationTime(null);
				cc.chara.transform.eulerAngles = new Vector3(0f, yb + Mathf.Lerp(0f, yd, animationTime), 0f);
			}
			while (animationTime < 1f);
			yb += yd - yo;
			yd = 0f;
			flag = true;
		}
		if (ccs == null)
		{
			cc.chara.PlayAnimation(CharaMotionDefine.ActKey.FRIENDSHIP_SEE_ST, false, 1f, 0.5f, 0.25f, flag);
			cc.chara.SetRotation(yb);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
			float animationTime2;
			do
			{
				yield return null;
				animationTime2 = cc.chara.GetAnimationTime(null);
				cc.chara.transform.eulerAngles = new Vector3(0f, yb + Mathf.Lerp(0f, yd, animationTime2), 0f);
			}
			while (animationTime2 < 1f);
			cc.chara.PlayAnimation(CharaMotionDefine.ActKey.FRIENDSHIP_SEE_LP, false, 1f, 0.2f, 0.1f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
			while (cc.chara.IsPlaying())
			{
				yield return null;
			}
			cc.chara.PlayAnimation(CharaMotionDefine.ActKey.FRIENDSHIP_SEE_EN, false, 1f, 0.2f, 0.1f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
			while (cc.chara.IsPlaying())
			{
				yield return null;
			}
			cc.chara.PlayAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true, 1f, 0.5f, 0.25f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
		}
		else
		{
			string motionKey = ccs.MotionKey;
			string text = ((ccs.VoiceKeyList.Count > 0) ? ccs.VoiceKeyList[Random.Range(0, ccs.VoiceKeyList.Count)] : "");
			float tim = (string.IsNullOrEmpty(text) ? 0f : this.PlayVoice(cc, text));
			if (string.IsNullOrEmpty(motionKey))
			{
				cc.chara.PlayAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true, 1f, 0.5f, 0.25f, flag);
				cc.chara.SetRotation(yb);
				cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
				float t = 0f;
				while ((t += TimeManager.DeltaTime * 3f) < 1f)
				{
					cc.chara.transform.eulerAngles = new Vector3(0f, yb + Mathf.Lerp(0f, yd, t), 0f);
					yield return null;
				}
				cc.chara.transform.eulerAngles = new Vector3(0f, yb + yd, 0f);
			}
			else
			{
				cc.chara.PlayAnimation(motionKey, false, 1f, 0.5f, 0.25f, flag);
				cc.chara.SetRotation(yb);
				cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
				float animationTime3;
				do
				{
					yield return null;
					tim -= TimeManager.DeltaTime;
					animationTime3 = cc.chara.GetAnimationTime(null);
					cc.chara.transform.eulerAngles = new Vector3(0f, yb + Mathf.Lerp(0f, yd, animationTime3), 0f);
				}
				while (animationTime3 < 1f);
				cc.chara.PlayAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true, 1f, 0.5f, 0.25f, false);
				cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
			}
			while ((tim -= TimeManager.DeltaTime) > 0f)
			{
				yield return null;
			}
		}
		this.noOpeTim = 0f;
		yield break;
	}

	// Token: 0x060012B0 RID: 4784 RVA: 0x000E2C84 File Offset: 0x000E0E84
	private IEnumerator StayChara(HomeCharaCtrl.CharaCtrl cc)
	{
		cc.busy = -1;
		cc.colli = 1f;
		int place = cc.place;
		cc.chara = this.MakeChara(cc.id, cc.dress, cc.longSkirt, cc.accessory);
		this.SetVoiceSheet(cc);
		cc.slp = false;
		cc.sit = false;
		bool beam = false;
		Vector3 vector;
		float num;
		if (cc.typ == 0)
		{
			Vector3 vector2;
			num = this.GetPlacePos(place, out vector, out vector2);
			if (this.rootData.ContainsKey(place) && this.rootData[place][0].name.IndexOf("_bed_") > 0)
			{
				cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_ITEM_BED_LP, true, 1f, -1f, -1f, false);
				cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = null));
				cc.slp = true;
				if (this.posBed != null)
				{
					vector = this.posBed.localPosition + new Vector3(0f, 0.56f, 0f);
					num = this.posBed.localEulerAngles.y + 180f;
				}
				cc.chara.shadowHeight = -20f;
			}
			else if (this.rootData.ContainsKey(place) && this.rootData[place][0].name.IndexOf("_chair_") > 0)
			{
				cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_ITEM_CHR_LP, true, 1f, -1f, -1f, false);
				cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = null));
				cc.sit = true;
				if (this.posChr != null)
				{
					vector = this.posChr.localPosition;
					num = this.posChr.localEulerAngles.y + 180f;
				}
				cc.chara.shadowHeight = -20f;
			}
			else
			{
				cc.chara.shadowHeight = vector.y + 0.03f;
			}
		}
		else if (cc.typ == 1)
		{
			vector = this.stayData[place].position;
			num = this.stayData[place].eulerAngles.y;
			if (this.stayData[place].name.EndsWith("_sit"))
			{
				if (this.charaList[cc.id].staticData.baseData.isTreeTypeByHome)
				{
					cc.slp = true;
				}
				else
				{
					cc.sit = true;
				}
				cc.chara.PlayAnimation(cc.slp ? CharaMotionDefine.ActKey.H_ACT_TREE_SLP_LP : CharaMotionDefine.ActKey.H_ITEM_CHR_LP, true, 1f, -1f, -1f, false);
				cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = null));
				vector.y -= (cc.slp ? 0.508f : 0.468f);
				if (cc.slp)
				{
					vector += new Vector3(0f, 0.5f, -0.167f);
				}
				cc.chara.shadowHeight = -20f;
				beam = true;
			}
			else
			{
				cc.chara.shadowHeight = vector.y + 0.03f;
			}
		}
		else
		{
			vector = this.doubleData[place].position;
			num = this.doubleData[place].eulerAngles.y;
			cc.chara.shadowHeight = vector.y + 0.03f;
		}
		cc.chara.transform.position = vector;
		cc.chara.transform.eulerAngles = new Vector3(0f, num, 0f);
		while (!cc.chara.IsFinishInitialize())
		{
			yield return null;
		}
		cc.chara.FadeIn(0.2f);
		cc.chara.SetEnableUpdateOffscreen();
		cc.root = 1;
		yield return null;
		float tim = 2f + Random.Range(0f, 2f);
		IEnumerator mov = null;
		IEnumerator tch = null;
		while (!beam || !cc.longSkirt)
		{
			cc.busy = 1;
			if (tch != null)
			{
				if (!tch.MoveNext())
				{
					tch = null;
				}
			}
			else if (cc.id == this.touchId)
			{
				this.contactId = this.touchId;
				if (!cc.slp && !cc.sit)
				{
					tch = this.CharaTouch(cc, false);
				}
				else if (cc.sit)
				{
					cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
				}
				this.touchId = 0;
			}
			else if (cc.place < 0)
			{
				if (cc.typ > 0)
				{
					cc.place = -cc.place;
				}
			}
			else if (cc.place == place)
			{
				if (mov == null)
				{
					if ((tim -= TimeManager.DeltaTime) > 0f)
					{
						if (!cc.chara.IsPlaying())
						{
							cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_HOM_WAIT0, true, 1f, 0.5f, 0.25f, false);
							cc.busy = 0;
						}
						else if (cc.chara.IsLoopAnimation())
						{
							cc.busy = 0;
						}
					}
					else if (!cc.slp && !cc.sit)
					{
						int num2 = ((cc.typ > 1) ? 0 : Random.Range(0, HomeCharaCtrl.emotionAct.Length));
						cc.chara.PlayAnimation(HomeCharaCtrl.emotionAct[num2], false, 1f, 0.5f, 0.25f, false);
						cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = null));
						EffectData effectData = (this.isIcon ? EffectManager.InstantiateEffect(HomeCharaCtrl.emotionIcon[num2], cc.chara.transform, LayerMask.NameToLayer(HomeCharaCtrl.charaLayer), 1f) : null);
						if (effectData != null)
						{
							effectData.effectObject.transform.localPosition = new Vector3(0f, 1.7f, 0f);
							effectData.PlayEffect(false);
							cc.chara.SetEffect(effectData);
						}
						tim = 8f + Random.Range(0f, 2f);
					}
					else
					{
						tim = 0f;
						if (this.contactId != cc.id)
						{
							cc.busy = 0;
							cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = null));
						}
					}
				}
				else if (!mov.MoveNext())
				{
					mov = null;
				}
			}
			else if (cc.typ == 0)
			{
				cc.slp = (cc.sit = false);
				if (cc.place <= 0)
				{
					cc.direct = false;
					mov = this.CharaMove(cc, -place);
					break;
				}
				cc.direct = true;
				mov = this.CharaMove(cc, place = cc.place);
			}
			else
			{
				place = cc.place;
			}
			cc.colli = ((mov == null) ? 1f : (-1f));
			yield return null;
		}
		if (mov != null && mov.MoveNext())
		{
			cc.colli = -1f;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060012B1 RID: 4785 RVA: 0x000E2C9A File Offset: 0x000E0E9A
	private IEnumerator MoveChara(HomeCharaCtrl.CharaCtrl cc)
	{
		cc.busy = -1;
		cc.colli = 1f;
		int place = cc.place;
		HomeFurnitureStatic.Category cat = (this.categoryData.ContainsKey(place) ? this.categoryData[place] : HomeFurnitureStatic.Category.INVALID);
		cc.direct = true;
		if (cc.chara == null)
		{
			cc.direct = false;
			cc.chara = this.MakeChara(cc.id, cc.dress, cc.longSkirt, cc.accessory);
			this.SetVoiceSheet(cc);
			Vector3 vector;
			Vector3 vector2;
			this.GetPlacePos(-place, out vector, out vector2);
			cc.chara.transform.position = vector;
			this.GetPlacePos(place, out vector, out vector2);
			cc.chara.transform.LookAt(vector);
		}
		cc.chara.SetAlpha(1f);
		while (!cc.chara.IsFinishInitialize())
		{
			yield return null;
		}
		cc.chara.SetEnableUpdateOffscreen();
		IEnumerator mov = this.CharaMove(cc, place);
		IEnumerator tch = null;
		for (;;)
		{
			cc.busy = 1;
			if (tch != null)
			{
				if (!tch.MoveNext())
				{
					tch = null;
				}
			}
			else if (cc.id == this.touchId)
			{
				this.contactId = this.touchId;
				tch = this.CharaTouch(cc, true);
				this.touchId = 0;
			}
			else
			{
				if (cc.place != place || mov == null || !mov.MoveNext())
				{
					break;
				}
				cc.colli = -1f;
				cc.busy = 0;
			}
			yield return null;
		}
		mov = null;
		cc.busy = -1;
		float tim = Random.Range(0.1f, 1f);
		while (cc.place == place && (tim -= TimeManager.DeltaTime) > 0f)
		{
			yield return null;
		}
		if (cc.place == place)
		{
			if (this.charaList[cc.id].staticData.baseData.isNightTypeByHome ^ this.nightTime)
			{
				cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_HOM_YAW, true, 1f, 0.5f, 0.25f, false);
				cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = null));
				tim = 6f;
				while (cc.place == place)
				{
					if ((tim -= TimeManager.DeltaTime) <= 0f)
					{
						break;
					}
					yield return null;
				}
			}
			else if (cat == HomeFurnitureStatic.Category.DESK || cat == HomeFurnitureStatic.Category.CHAIR || cat == HomeFurnitureStatic.Category.ORNAMENT || cat == HomeFurnitureStatic.Category.BED)
			{
				cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_ACT_SEE_ST, false, 1f, 0.5f, 0.25f, false);
				cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = null));
				while (cc.chara.IsPlaying())
				{
					yield return null;
				}
				cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_ACT_SEE_LP, true, 1f, 0.2f, 0.1f, false);
				tim = 6f;
				while (cc.place == place && (tim -= TimeManager.DeltaTime) > 0f)
				{
					yield return null;
				}
				cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_ACT_SEE_EN, false, 1f, 0.2f, 0.1f, false);
				while (cc.chara.IsPlaying())
				{
					yield return null;
				}
			}
			else if (cat == HomeFurnitureStatic.Category.STORAGE || cat == HomeFurnitureStatic.Category.ELECTRONICS)
			{
				cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_ITEM_STR, true, 1f, 0.5f, 0.25f, false);
				cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = null));
				tim = 6f;
				while (cc.place == place)
				{
					if ((tim -= TimeManager.DeltaTime) <= 0f)
					{
						break;
					}
					yield return null;
				}
			}
			else
			{
				tim = 6f;
				while (cc.place == place && (tim -= TimeManager.DeltaTime) > 0f)
				{
					yield return null;
				}
			}
		}
		if (cc.place == place)
		{
			cc.place = -place;
		}
		while (cc.place < 0)
		{
			yield return null;
		}
		cc.direct = false;
		mov = this.CharaMove(cc, -place);
		for (;;)
		{
			cc.busy = 1;
			if (tch != null)
			{
				if (!tch.MoveNext())
				{
					tch = null;
				}
			}
			else if (cc.id == this.touchId)
			{
				this.contactId = this.touchId;
				tch = this.CharaTouch(cc, true);
				this.touchId = 0;
			}
			else
			{
				if (mov == null || !mov.MoveNext())
				{
					break;
				}
				cc.colli = -1f;
				cc.busy = 0;
			}
			yield return null;
		}
		mov = null;
		this.DestChara(cc);
		yield break;
	}

	// Token: 0x060012B2 RID: 4786 RVA: 0x000E2CB0 File Offset: 0x000E0EB0
	private IEnumerator CharaMove(HomeCharaCtrl.CharaCtrl cc, int place)
	{
		Vector3 cp;
		Vector3 tp;
		float rot = this.GetPlacePos(place, out cp, out tp);
		cc.root = -1;
		Vector3 rp = cp;
		List<Transform> root;
		if (this.rootData.TryGetValue(Mathf.Abs(place), out root))
		{
			Vector3 vector = cp - cc.chara.transform.position;
			vector.y = 0f;
			float num = vector.sqrMagnitude;
			float num2 = num;
			if (cc.direct && place > 0 && root.Count > 0)
			{
				if (num > 0.5f)
				{
					rp = root[cc.root = root.Count - 1].position;
				}
				num2 = (num = -1f);
			}
			for (int i = ((place > 0) ? 0 : 1); i < root.Count; i++)
			{
				Vector3 position = root[i].position;
				vector = position - cp;
				vector.y = 0f;
				if (vector.sqrMagnitude < num)
				{
					vector = position - cc.chara.transform.position;
					vector.y = 0f;
					float sqrMagnitude = vector.sqrMagnitude;
					if (sqrMagnitude < num2)
					{
						num2 = sqrMagnitude;
						rp = position;
						cc.root = i;
					}
				}
			}
		}
		cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_HOM_MOV_ST, false, 1f, 0.5f, 0.25f, false);
		while (cc.root >= 0)
		{
			float num3 = TimeManager.DeltaTime;
			float y = cc.chara.transform.eulerAngles.y;
			cc.chara.transform.LookAt(rp);
			float num4 = Mathf.DeltaAngle(y, cc.chara.transform.eulerAngles.y);
			float num5 = Mathf.Abs(num4);
			num3 *= Mathf.Cos(0.017453292f * num5 / 4f);
			if (!cc.chara.IsLoopAnimation())
			{
				num3 *= cc.chara.GetAnimationTime(CharaMotionDefine.ActKey.H_HOM_MOV_ST.ToString());
			}
			if (cc.colli >= 0f)
			{
				num3 *= cc.colli;
			}
			Vector3 vector2 = rp - cc.chara.transform.position;
			vector2.y = 0f;
			float magnitude = vector2.magnitude;
			if (magnitude > num3)
			{
				vector2 *= num3 / magnitude;
			}
			else if (place > 0)
			{
				int num6 = cc.root + 1;
				cc.root = num6;
				if (num6 >= root.Count)
				{
					cc.root = -1;
					rp = cp;
				}
				else
				{
					rp = root[cc.root].position;
				}
			}
			else
			{
				int num6 = cc.root - 1;
				cc.root = num6;
				if (num6 < 0)
				{
					rp = cp;
				}
				else
				{
					rp = root[cc.root].position;
				}
			}
			float num7 = TimeManager.DeltaTime * 180f;
			if (num5 < 30f)
			{
				num7 /= 6f;
			}
			num4 = Mathf.Clamp(num4, -num7, num7);
			cc.chara.transform.eulerAngles = new Vector3(0f, y + num4, 0f);
			cc.chara.transform.position += vector2;
			if (num5 < 30f && cc.chara.IsLoopAnimation())
			{
				num3 = (TimeManager.DeltaTime - num3) * 0.5f;
				if (cc.colli >= 0f)
				{
					num3 *= cc.colli;
				}
				cc.chara.transform.Translate(0f, 0f, num3);
			}
			if (!cc.chara.IsPlaying())
			{
				cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_HOM_MOV_LP, true, 1f, 0f, 0f, false);
			}
			yield return null;
		}
		cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_HOM_MOV_EN, false, 1f, 0f, 0f, false);
		for (;;)
		{
			float deltaTime = TimeManager.DeltaTime;
			Vector3 vector3 = tp - cc.chara.transform.position;
			vector3.y = 0f;
			float magnitude2 = vector3.magnitude;
			float y2 = cc.chara.transform.eulerAngles.y;
			float num8 = Mathf.DeltaAngle(y2, rot);
			float num9 = TimeManager.DeltaTime * 180f;
			if (Mathf.Abs(num8) <= num9 && magnitude2 <= deltaTime)
			{
				break;
			}
			num8 = Mathf.Clamp(num8, -num9, num9);
			cc.chara.transform.eulerAngles = new Vector3(0f, y2 + num8, 0f);
			if (magnitude2 > deltaTime)
			{
				vector3 *= deltaTime / magnitude2;
			}
			cc.chara.transform.position += vector3;
			yield return null;
		}
		cc.chara.transform.position = tp;
		cc.chara.transform.eulerAngles = new Vector3(0f, rot, 0f);
		cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_HOM_WAIT0, true, 1f, 0.5f, 0.25f, false);
		yield break;
	}

	// Token: 0x060012B3 RID: 4787 RVA: 0x000E2CD0 File Offset: 0x000E0ED0
	private void SetAlpha(CharaModelHandle cmh)
	{
		if (cmh != null)
		{
			float num = cmh.transform.position.x - this.fieldCamera.transform.position.x;
			float num2 = cmh.transform.position.z - this.fieldCamera.transform.position.z;
			cmh.SetAlpha(Mathf.Clamp01((num * num + num2 * num2 - this.fieldCamera.nearClipPlane) / 0.3f));
		}
	}

	// Token: 0x060012B4 RID: 4788 RVA: 0x000E2D58 File Offset: 0x000E0F58
	private IEnumerator CharaTouch(HomeCharaCtrl.CharaCtrl cc, bool mov)
	{
		float yb = cc.chara.transform.eulerAngles.y;
		cc.chara.transform.LookAt(this.fieldCamera.transform);
		float yd = Mathf.DeltaAngle(yb, cc.chara.transform.eulerAngles.y);
		cc.chara.transform.eulerAngles = new Vector3(0f, yb, 0f);
		CharaMotionDefine.ActKey trn = CharaMotionDefine.ActKey.INVALID;
		float yo = 0f;
		if (yd < -150f)
		{
			trn = CharaMotionDefine.ActKey.H_HOM_LEFT_FULL_TURN;
			yo = -180f;
		}
		else if (yd < -50f)
		{
			trn = CharaMotionDefine.ActKey.H_HOM_LEFT_HALF_TURN;
			yo = 90f;
		}
		else if (yd > 150f)
		{
			trn = CharaMotionDefine.ActKey.H_HOM_RIGHT_FULL_TURN;
			yo = -180f;
		}
		else if (yd > 50f)
		{
			trn = CharaMotionDefine.ActKey.H_HOM_RIGHT_HALF_TURN;
			yo = -90f;
		}
		float ybb = yb;
		bool flag = false;
		if (trn != CharaMotionDefine.ActKey.INVALID)
		{
			cc.chara.PlayAnimation(trn, false, 1f, 0.5f, 0.25f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
			yd = Mathf.DeltaAngle(0f, yd + yo);
			float animationTime;
			do
			{
				yield return null;
				animationTime = cc.chara.GetAnimationTime(null);
				cc.chara.transform.eulerAngles = new Vector3(0f, yb + Mathf.Lerp(0f, yd, animationTime), 0f);
			}
			while (animationTime < 1f);
			yb += yd - yo;
			yd = 0f;
			flag = true;
		}
		int mot = 0;
		cc.chara.PlayAnimation(CharaMotionDefine.ActKey.VOICE_CREDIT, false, 1f, 0.5f, 0.25f, flag);
		cc.chara.SetRotation(yb);
		cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
		float tim = 0f;
		while ((tim += TimeManager.DeltaTime * 3f) < 1f)
		{
			cc.chara.transform.eulerAngles = new Vector3(0f, yb + Mathf.Lerp(0f, yd, tim), 0f);
			yield return null;
		}
		cc.chara.transform.eulerAngles = new Vector3(0f, yb + yd, 0f);
		while (cc.id == this.contactId)
		{
			bool flag2 = false;
			if (cc.id == Mathf.Abs(this.touchId))
			{
				this.contactId = cc.id;
				this.touchId = 0;
				flag2 = true;
			}
			if (mot == 0)
			{
				int num = mot;
				mot = num + 1;
			}
			else if (flag2)
			{
				CharaMotionDefine.ActKey actKey = CharaMotionDefine.ActKey.H_ACT_SEE_ST;
				if (mot > 1)
				{
					int num2 = Random.Range(0, 3);
					if (num2 == 0)
					{
						actKey = CharaMotionDefine.ActKey.JOY;
					}
					else if (num2 == 1)
					{
						actKey = CharaMotionDefine.ActKey.POSITIVE;
					}
					else
					{
						actKey = CharaMotionDefine.ActKey.VOICE_CREDIT;
					}
				}
				cc.chara.PlayAnimation(actKey, false, 1f, 0.5f, 0.25f, false);
				int num = mot;
				mot = num + 1;
				tim = 0f;
				do
				{
					yield return null;
				}
				while ((tim += TimeManager.DeltaTime) < 0.2f);
			}
			if (!cc.chara.IsPlaying())
			{
				cc.chara.PlayAnimation(cc.chara.IsCurrentAnimation(CharaMotionDefine.ActKey.H_ACT_SEE_ST) ? CharaMotionDefine.ActKey.H_ACT_SEE_LP : CharaMotionDefine.ActKey.H_HOM_WAIT0, true, 1f, 0.5f, 0.25f, false);
			}
			yield return null;
		}
		yb = cc.chara.transform.eulerAngles.y;
		yd = Mathf.DeltaAngle(yb, ybb);
		if (mov)
		{
			cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_HOM_MOV_ST, false, 1f, 0.5f, 0.25f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = null));
		}
		else if (trn == CharaMotionDefine.ActKey.INVALID)
		{
			cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_HOM_WAIT0, true, 1f, 0.5f, 0.25f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = null));
			tim = 0f;
			while ((tim += TimeManager.DeltaTime * 3f) < 1f)
			{
				cc.chara.transform.eulerAngles = new Vector3(0f, yb + Mathf.Lerp(0f, yd, tim), 0f);
				yield return null;
			}
			cc.chara.transform.eulerAngles = new Vector3(0f, ybb, 0f);
		}
		else
		{
			yo = -yo;
			if (trn == CharaMotionDefine.ActKey.H_HOM_LEFT_FULL_TURN)
			{
				trn = CharaMotionDefine.ActKey.H_HOM_RIGHT_FULL_TURN;
			}
			else if (trn == CharaMotionDefine.ActKey.H_HOM_LEFT_HALF_TURN)
			{
				trn = CharaMotionDefine.ActKey.H_HOM_RIGHT_HALF_TURN;
			}
			else if (trn == CharaMotionDefine.ActKey.H_HOM_RIGHT_FULL_TURN)
			{
				trn = CharaMotionDefine.ActKey.H_HOM_LEFT_FULL_TURN;
			}
			else
			{
				trn = CharaMotionDefine.ActKey.H_HOM_LEFT_HALF_TURN;
			}
			cc.chara.PlayAnimation(trn, false, 1f, 0.5f, 0.25f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = null));
			yd = Mathf.DeltaAngle(0f, yd + yo);
			float animationTime2;
			do
			{
				yield return null;
				animationTime2 = cc.chara.GetAnimationTime(null);
				cc.chara.transform.eulerAngles = new Vector3(0f, yb + Mathf.Lerp(0f, yd, animationTime2), 0f);
			}
			while (animationTime2 < 1f);
			cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_HOM_WAIT0, true, 1f, 0.5f, 0.25f, true);
			cc.chara.SetRotation(ybb);
		}
		yield break;
	}

	// Token: 0x060012B5 RID: 4789 RVA: 0x000E2D75 File Offset: 0x000E0F75
	private IEnumerator FlyChara(HomeCharaCtrl.CharaCtrl cc)
	{
		cc.busy = -1;
		cc.colli = 1f;
		cc.chara = this.MakeChara(cc.id, cc.dress, cc.longSkirt, cc.accessory);
		this.SetVoiceSheet(cc);
		cc.chara.shadowHeight = -20f;
		cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = null));
		int r = 0;
		Transform transform = cc.chara.transform;
		List<Transform> list = this.flyData[cc.place];
		int num = r;
		r = num + 1;
		Vector3 pos = (transform.position = list[num].position);
		bool ud = pos.y - this.flyData[cc.place][r].position.y > 0.03f;
		if (ud)
		{
			cc.chara.transform.eulerAngles = new Vector3(0f, 180f, 0f);
		}
		else
		{
			cc.chara.transform.LookAt(this.flyData[cc.place][r]);
			cc.chara.transform.eulerAngles = new Vector3(0f, cc.chara.transform.eulerAngles.y, 0f);
		}
		cc.chara.SetAlpha(1f);
		while (!cc.chara.IsFinishInitialize())
		{
			yield return null;
		}
		cc.chara.SetEnableUpdateOffscreen();
		cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_HOM_FLY_MOV_ST, false, 1f, 0.5f, 0.25f, false);
		IEnumerator tch = null;
		float tt = 0f;
		while (r < this.flyData[cc.place].Count)
		{
			cc.busy = 1;
			if (tch != null)
			{
				if (!tch.MoveNext())
				{
					tch = null;
				}
			}
			else if (cc.id == this.touchId)
			{
				this.contactId = this.touchId;
				tch = this.FlyCharaTouch(cc);
				this.touchId = 0;
			}
			else
			{
				float y = cc.chara.transform.eulerAngles.y;
				if (ud)
				{
					cc.chara.transform.eulerAngles = new Vector3(0f, 180f, 0f);
				}
				else
				{
					cc.chara.transform.LookAt(this.flyData[cc.place][r]);
				}
				float num2 = Mathf.DeltaAngle(y, cc.chara.transform.eulerAngles.y);
				float num3 = TimeManager.DeltaTime * 180f;
				num2 = Mathf.Clamp(num2, -num3, num3);
				cc.chara.transform.eulerAngles = new Vector3(0f, y + num2, 0f);
				num3 = Vector3.Magnitude(pos - this.flyData[cc.place][r].position);
				if (num3 < 0.03f)
				{
					num = r;
					r = num + 1;
				}
				else
				{
					pos = Vector3.Lerp(pos, this.flyData[cc.place][r].position, TimeManager.DeltaTime * 2f / num3);
					tt += TimeManager.DeltaTime;
					float num4 = Mathf.Sin(tt * 3.1415927f);
					if (ud)
					{
						cc.chara.transform.position = new Vector3(0.1f * num4, 0f, 0f);
					}
					else
					{
						cc.chara.transform.position = new Vector3(0f, 0.25f * num4, 0f);
					}
					cc.chara.transform.position += pos;
				}
				cc.busy = 0;
				if (!cc.chara.IsPlaying())
				{
					cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_HOM_FLY_MOV_LP, true, 1f, 0f, 0f, false);
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060012B6 RID: 4790 RVA: 0x000E2D8B File Offset: 0x000E0F8B
	private IEnumerator FlyCharaTouch(HomeCharaCtrl.CharaCtrl cc)
	{
		float yb = cc.chara.transform.eulerAngles.y;
		cc.chara.transform.LookAt(this.fieldCamera.transform);
		float yd = Mathf.DeltaAngle(yb, cc.chara.transform.eulerAngles.y);
		cc.chara.transform.eulerAngles = new Vector3(0f, yb, 0f);
		CharaMotionDefine.ActKey actKey = CharaMotionDefine.ActKey.INVALID;
		float yo = 0f;
		if (yd < -150f)
		{
			actKey = CharaMotionDefine.ActKey.H_HOM_FLY_LEFT_FULL_TURN;
			yo = -180f;
		}
		else if (yd < -50f)
		{
			actKey = CharaMotionDefine.ActKey.H_HOM_FLY_LEFT_HALF_TURN;
			yo = 90f;
		}
		else if (yd > 150f)
		{
			actKey = CharaMotionDefine.ActKey.H_HOM_FLY_RIGHT_FULL_TURN;
			yo = -180f;
		}
		else if (yd > 50f)
		{
			actKey = CharaMotionDefine.ActKey.H_HOM_FLY_RIGHT_HALF_TURN;
			yo = -90f;
		}
		bool flag = false;
		if (actKey != CharaMotionDefine.ActKey.INVALID)
		{
			cc.chara.PlayAnimation(actKey, false, 1f, 0.5f, 0.25f, false);
			cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
			yd = Mathf.DeltaAngle(0f, yd + yo);
			float animationTime;
			do
			{
				yield return null;
				animationTime = cc.chara.GetAnimationTime(null);
				cc.chara.transform.eulerAngles = new Vector3(0f, yb + Mathf.Lerp(0f, yd, animationTime), 0f);
			}
			while (animationTime < 1f);
			yb += yd - yo;
			yd = 0f;
			flag = true;
		}
		int mot = 0;
		cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_HOM_FLY_WAIT0, true, 1f, 0.5f, 0.25f, flag);
		cc.chara.SetRotation(yb);
		cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = this.fieldCamera.transform));
		float tim = 0f;
		while ((tim += TimeManager.DeltaTime * 3f) < 1f)
		{
			cc.chara.transform.eulerAngles = new Vector3(0f, yb + Mathf.Lerp(0f, yd, tim), 0f);
			yield return null;
		}
		cc.chara.transform.eulerAngles = new Vector3(0f, yb + yd, 0f);
		while (cc.id == this.contactId)
		{
			bool flag2 = false;
			if (cc.id == Mathf.Abs(this.touchId))
			{
				this.contactId = cc.id;
				this.touchId = 0;
				flag2 = true;
			}
			if (mot == 0)
			{
				int num = mot;
				mot = num + 1;
			}
			else if (flag2)
			{
				int num = mot;
				mot = num + 1;
				tim = 0f;
				do
				{
					yield return null;
				}
				while ((tim += TimeManager.DeltaTime) < 0.2f);
			}
			yield return null;
		}
		cc.chara.PlayAnimation(CharaMotionDefine.ActKey.H_HOM_FLY_MOV_ST, false, 1f, 0.5f, 0.25f, false);
		cc.chara.headFollowObj = (cc.chara.eyeFollowObj = (cc.chara.mouthFollowObj = null));
		yield break;
	}

	// Token: 0x060012B7 RID: 4791 RVA: 0x000E2DA4 File Offset: 0x000E0FA4
	private CharaModelHandle MakeChara(int id, int dress, bool longSkirt, DataManagerCharaAccessory.Accessory accessory)
	{
		CharaModelHandle component = new GameObject("Chara" + id.ToString(), new Type[] { typeof(CharaModelHandle) }).GetComponent<CharaModelHandle>();
		component.transform.SetParent(base.transform, false);
		component.Initialize(id, true, false, dress, longSkirt, false, true, accessory);
		component.SetLayer(HomeCharaCtrl.charaLayer);
		component.SetModelActive(true);
		component.SetWeaponActive(false);
		component.SetAlpha(0f);
		component.PlayAnimation(CharaMotionDefine.ActKey.H_HOM_WAIT0, true, 1f, 0f, 0f, false);
		component.DispAccessory(0, true, false);
		component.shadowSize = 0.6f;
		return component;
	}

	// Token: 0x060012B8 RID: 4792 RVA: 0x000E2E54 File Offset: 0x000E1054
	private void SetVoiceSheet(HomeCharaCtrl.CharaCtrl cc)
	{
		cc.voiceSheet.Add(new HomeCharaCtrl.CharaCtrl.VoiceSheet
		{
			sheet = SoundManager.CharaIdToSheet(cc.id),
			loader = null
		});
		CharaStaticBase baseData = this.charaList[cc.id].staticData.baseData;
		if (this.charaList.ContainsKey(baseData.OriginalId))
		{
			cc.voiceSheet.Add(new HomeCharaCtrl.CharaCtrl.VoiceSheet
			{
				sheet = SoundManager.CharaIdToSheet(baseData.OriginalId),
				loader = null
			});
		}
		foreach (int num in baseData.SynonymIdSet)
		{
			if (this.charaList.ContainsKey(num))
			{
				cc.voiceSheet.Add(new HomeCharaCtrl.CharaCtrl.VoiceSheet
				{
					sheet = SoundManager.CharaIdToSheet(num),
					loader = null
				});
			}
		}
		foreach (HomeCharaCtrl.CharaCtrl.VoiceSheet voiceSheet in cc.voiceSheet)
		{
			voiceSheet.loader = SoundManager.LoadCueSheetWithDownload(voiceSheet.sheet);
		}
	}

	// Token: 0x060012B9 RID: 4793 RVA: 0x000E2F9C File Offset: 0x000E119C
	private float PlayVoice(HomeCharaCtrl.CharaCtrl cc, VOICE_TYPE vt)
	{
		return this.PlayVoice(cc, vt.ToString());
	}

	// Token: 0x060012BA RID: 4794 RVA: 0x000E2FB4 File Offset: 0x000E11B4
	private float PlayVoice(HomeCharaCtrl.CharaCtrl cc, string vts)
	{
		List<string> list = new List<string>();
		foreach (HomeCharaCtrl.CharaCtrl.VoiceSheet voiceSheet in cc.voiceSheet)
		{
			if (voiceSheet.loader == null)
			{
				list.Add(voiceSheet.sheet);
			}
		}
		float num = 0.1f;
		if (list.Count > 0)
		{
			string text = list[Random.Range(0, list.Count)];
			SoundManager.PlayVoiceByTypeName(text, vts);
			num = SoundManager.GetVoiceLengthByTypeName(text, vts);
		}
		return num;
	}

	// Token: 0x060012BB RID: 4795 RVA: 0x000E304C File Offset: 0x000E124C
	private void DestChara(HomeCharaCtrl.CharaCtrl cc)
	{
		foreach (HomeCharaCtrl.CharaCtrl.VoiceSheet voiceSheet in cc.voiceSheet)
		{
			voiceSheet.loader = null;
			SoundManager.UnloadCueSheet(voiceSheet.sheet);
		}
		cc.voiceSheet = new List<HomeCharaCtrl.CharaCtrl.VoiceSheet>();
		cc.ctrl = null;
		if (cc.chara != null)
		{
			Object.Destroy(cc.chara.gameObject);
		}
		cc.chara = null;
	}

	// Token: 0x060012BC RID: 4796 RVA: 0x000E30E0 File Offset: 0x000E12E0
	private float GetPlacePos(int place, out Vector3 cp, out Vector3 tp)
	{
		tp = (cp = this.fieldStage.transform.position);
		float num = 180f;
		if (place > 0)
		{
			GameObject furnitureModel = this.furnitureCtrl.GetFurnitureModel(place);
			Transform transform = ((furnitureModel == null) ? null : furnitureModel.transform);
			if (transform == null)
			{
				HomePlacementStatic homePlacementStatic = null;
				if (DataManager.DmHome.GetHomePlacementStaticMap().TryGetValue(place, out homePlacementStatic))
				{
					transform = this.fieldStage.transform.Find(homePlacementStatic.locatorName);
				}
			}
			if (transform != null)
			{
				Transform transform2 = transform;
				Transform transform3 = transform;
				foreach (object obj in transform)
				{
					Transform transform4 = (Transform)obj;
					if (transform4.name.StartsWith("pos_obj_"))
					{
						if (transform4.name.EndsWith("_start"))
						{
							transform2 = transform4;
						}
						else if (transform4.name.EndsWith("_direction"))
						{
							transform3 = transform4;
						}
					}
				}
				tp = transform2.position;
				cp = transform3.position;
				num = transform2.eulerAngles.y;
				Vector3 vector = transform.position - tp;
				if (vector.x * vector.x + vector.z * vector.z > 0.01f)
				{
					num = 90f - Mathf.Atan2(vector.z, vector.x) * 57.29578f;
				}
			}
		}
		else if (place < 0 && this.rootData.ContainsKey(-place) && this.rootData[-place].Count > 0)
		{
			tp = (cp = this.rootData[-place][0].position);
			if (this.rootData[-place].Count > 1)
			{
				Vector3 vector2 = tp - this.rootData[-place][1].position;
				if (vector2.x * vector2.x + vector2.z * vector2.z > 0.01f)
				{
					num = 90f - Mathf.Atan2(vector2.z, vector2.x) * 57.29578f;
				}
			}
		}
		cp.y = (tp.y = 0f);
		return num;
	}

	// Token: 0x060012BD RID: 4797 RVA: 0x000E3388 File Offset: 0x000E1588
	private int ChangePlace(int place, List<int> plc)
	{
		HomeFurnitureStatic.Category category = (this.categoryData.ContainsKey(place) ? this.categoryData[place] : HomeFurnitureStatic.Category.INVALID);
		List<HomeFurnitureStatic.Category> list = (HomeCharaCtrl.ChangeCategory.ContainsKey(category) ? HomeCharaCtrl.ChangeCategory[category] : new List<HomeFurnitureStatic.Category>());
		List<int> list2 = new List<int>();
		foreach (int num in plc)
		{
			if (this.categoryData.ContainsKey(num) && list.Contains(this.categoryData[num]))
			{
				list2.Add(num);
			}
		}
		if (list2.Count <= 0)
		{
			return 0;
		}
		return list2[Random.Range(0, list2.Count)];
	}

	// Token: 0x060012BE RID: 4798 RVA: 0x000E3460 File Offset: 0x000E1660
	private void CheckDressUp(HomeCharaCtrl.CharaCtrl cc)
	{
		if (cc.dressup == null)
		{
			if (cc.chara == null)
			{
				return;
			}
			if (cc.dress != this.charaList[cc.id].equipClothImageId)
			{
				cc.dress = this.charaList[cc.id].equipClothImageId;
				cc.longSkirt = this.charaList[cc.id].equipLongSkirt;
				cc.accessory = (this.charaList[cc.id].dynamicData.dispAccessoryEffect ? this.charaList[cc.id].dynamicData.accessory : null);
				cc.dressup = this.DressUp(cc);
				cc.dressup.MoveNext();
				return;
			}
		}
		else if (!cc.dressup.MoveNext())
		{
			cc.dressup = null;
		}
	}

	// Token: 0x060012BF RID: 4799 RVA: 0x000E354F File Offset: 0x000E174F
	private IEnumerator DressUp(HomeCharaCtrl.CharaCtrl cc)
	{
		CharaModelHandle cmh = this.MakeChara(cc.id, cc.dress, cc.longSkirt, cc.accessory);
		while (!cc.chara.IsFinishInitialize())
		{
			yield return null;
		}
		cmh.transform.position = cc.chara.transform.position;
		cmh.transform.rotation = cc.chara.transform.rotation;
		cmh.SetAlpha(1f);
		while (!cmh.IsFinishInitialize())
		{
			yield return null;
		}
		cmh.SetEnableUpdateOffscreen();
		if (cc.chara != null)
		{
			cmh.PlayAnimation(cc.chara.GetCurrentAnimation(), cc.chara.IsLoopAnimation(), 1f, 0f, 0f, false);
			Object.Destroy(cc.chara.gameObject);
		}
		cc.chara = cmh;
		yield break;
	}

	// Token: 0x060012C0 RID: 4800 RVA: 0x000E3568 File Offset: 0x000E1768
	public void ReplayAnimation()
	{
		foreach (HomeCharaCtrl.CharaCtrl charaCtrl in this.moveChara)
		{
			CharaMotionDefine.ActKey currentAnimation = charaCtrl.chara.GetCurrentAnimation();
			bool flag = charaCtrl.chara.IsLoopAnimation();
			charaCtrl.chara.PlayAnimation(currentAnimation, flag, 1f, 0f, 0f, false);
		}
		foreach (HomeCharaCtrl.CharaCtrl charaCtrl2 in this.stayChara)
		{
			CharaMotionDefine.ActKey currentAnimation2 = charaCtrl2.chara.GetCurrentAnimation();
			bool flag2 = charaCtrl2.chara.IsLoopAnimation();
			charaCtrl2.chara.PlayAnimation(currentAnimation2, flag2, 1f, 0f, 0f, false);
		}
	}

	// Token: 0x04000F42 RID: 3906
	private GameObject fieldStage;

	// Token: 0x04000F43 RID: 3907
	private Camera fieldCamera;

	// Token: 0x04000F44 RID: 3908
	private List<HomeFurnitureMapping> furnitureMap;

	// Token: 0x04000F45 RID: 3909
	private Dictionary<int, CharaPackData> charaList;

	// Token: 0x04000F46 RID: 3910
	private List<CharaContactStatic> contactList;

	// Token: 0x04000F47 RID: 3911
	private List<HomeCharaCtrl.CharaCtrl> stayChara;

	// Token: 0x04000F48 RID: 3912
	private List<HomeCharaCtrl.CharaCtrl> moveChara;

	// Token: 0x04000F49 RID: 3913
	private HomeCharaCtrl.CharaCtrl viewChara;

	// Token: 0x04000F4A RID: 3914
	private Dictionary<CharaContactStatic.Situation, int> viewContact;

	// Token: 0x04000F4B RID: 3915
	private Transform viewPos;

	// Token: 0x04000F4C RID: 3916
	private bool viewChg;

	// Token: 0x04000F4D RID: 3917
	private float moveInterval;

	// Token: 0x04000F4E RID: 3918
	private int stayFriends;

	// Token: 0x04000F4F RID: 3919
	private float stayTime;

	// Token: 0x04000F50 RID: 3920
	private HomeFurnitureCtrl furnitureCtrl;

	// Token: 0x04000F51 RID: 3921
	private List<Transform> stayData;

	// Token: 0x04000F52 RID: 3922
	private List<Transform> doubleData;

	// Token: 0x04000F53 RID: 3923
	private List<List<Transform>> flyData;

	// Token: 0x04000F54 RID: 3924
	private Dictionary<int, List<Transform>> rootData;

	// Token: 0x04000F55 RID: 3925
	private Dictionary<int, HomeFurnitureStatic.Category> categoryData;

	// Token: 0x04000F56 RID: 3926
	private Transform posBed;

	// Token: 0x04000F57 RID: 3927
	private Transform posChr;

	// Token: 0x04000F58 RID: 3928
	private int touchId;

	// Token: 0x04000F59 RID: 3929
	private bool touchOut;

	// Token: 0x04000F5A RID: 3930
	private float noOpeTim;

	// Token: 0x04000F5B RID: 3931
	private float strokeAmount;

	// Token: 0x04000F5C RID: 3932
	private float strokeEff;

	// Token: 0x04000F5D RID: 3933
	private Vector2 strokePos;

	// Token: 0x04000F5E RID: 3934
	private bool strokeOk;

	// Token: 0x04000F5F RID: 3935
	private int contactId;

	// Token: 0x04000F60 RID: 3936
	private bool nightTime;

	// Token: 0x04000F61 RID: 3937
	private static readonly string charaLayer = "FieldPlayer";

	// Token: 0x04000F62 RID: 3938
	private static readonly string charaShadowLayer = "FieldPlayerShadow";

	// Token: 0x04000F63 RID: 3939
	private static readonly string[] emotionIcon = new string[] { "Ef_info_home_happy", "Ef_info_home_supprise", "Ef_info_home_question", "Ef_info_home_inspiration" };

	// Token: 0x04000F64 RID: 3940
	private bool isIcon;

	// Token: 0x04000F65 RID: 3941
	private static readonly CharaMotionDefine.ActKey[] emotionAct = new CharaMotionDefine.ActKey[]
	{
		CharaMotionDefine.ActKey.JOY,
		CharaMotionDefine.ActKey.SURPRISE,
		CharaMotionDefine.ActKey.DENIAL,
		CharaMotionDefine.ActKey.POSITIVE
	};

	// Token: 0x04000F66 RID: 3942
	private static readonly string strokeEffName = "Ef_info_home_stroke";

	// Token: 0x04000F68 RID: 3944
	private static readonly List<VOICE_TYPE> ViewTouchSlp = new List<VOICE_TYPE>
	{
		VOICE_TYPE.SLP01,
		VOICE_TYPE.SLP02,
		VOICE_TYPE.SLP03
	};

	// Token: 0x04000F69 RID: 3945
	private static readonly List<VOICE_TYPE> ViewTouchsit = new List<VOICE_TYPE>
	{
		VOICE_TYPE.HOM02,
		VOICE_TYPE.HOM03,
		VOICE_TYPE.HOM04
	};

	// Token: 0x04000F6A RID: 3946
	private static readonly List<CharaMotionDefine.ActKey> ViewTouchMot = new List<CharaMotionDefine.ActKey>
	{
		CharaMotionDefine.ActKey.POSITIVE,
		CharaMotionDefine.ActKey.JOY,
		CharaMotionDefine.ActKey.SURPRISE
	};

	// Token: 0x04000F6B RID: 3947
	private static readonly Dictionary<HomeFurnitureStatic.Category, List<HomeFurnitureStatic.Category>> ChangeCategory = new Dictionary<HomeFurnitureStatic.Category, List<HomeFurnitureStatic.Category>>
	{
		{
			HomeFurnitureStatic.Category.BED,
			new List<HomeFurnitureStatic.Category>
			{
				HomeFurnitureStatic.Category.DESK,
				HomeFurnitureStatic.Category.CHAIR,
				HomeFurnitureStatic.Category.WINDOW,
				HomeFurnitureStatic.Category.ELECTRONICS
			}
		},
		{
			HomeFurnitureStatic.Category.DESK,
			new List<HomeFurnitureStatic.Category>
			{
				HomeFurnitureStatic.Category.BED,
				HomeFurnitureStatic.Category.CHAIR,
				HomeFurnitureStatic.Category.WINDOW,
				HomeFurnitureStatic.Category.STORAGE,
				HomeFurnitureStatic.Category.ELECTRONICS
			}
		},
		{
			HomeFurnitureStatic.Category.CHAIR,
			new List<HomeFurnitureStatic.Category>
			{
				HomeFurnitureStatic.Category.BED,
				HomeFurnitureStatic.Category.DESK,
				HomeFurnitureStatic.Category.WINDOW,
				HomeFurnitureStatic.Category.ELECTRONICS
			}
		},
		{
			HomeFurnitureStatic.Category.WINDOW,
			new List<HomeFurnitureStatic.Category>
			{
				HomeFurnitureStatic.Category.BED,
				HomeFurnitureStatic.Category.DESK,
				HomeFurnitureStatic.Category.CHAIR,
				HomeFurnitureStatic.Category.ELECTRONICS
			}
		},
		{
			HomeFurnitureStatic.Category.STORAGE,
			new List<HomeFurnitureStatic.Category>
			{
				HomeFurnitureStatic.Category.DESK,
				HomeFurnitureStatic.Category.CHAIR,
				HomeFurnitureStatic.Category.INTERIOR,
				HomeFurnitureStatic.Category.ORNAMENT,
				HomeFurnitureStatic.Category.ELECTRONICS
			}
		},
		{
			HomeFurnitureStatic.Category.INTERIOR,
			new List<HomeFurnitureStatic.Category>
			{
				HomeFurnitureStatic.Category.STORAGE,
				HomeFurnitureStatic.Category.ORNAMENT
			}
		},
		{
			HomeFurnitureStatic.Category.ORNAMENT,
			new List<HomeFurnitureStatic.Category>
			{
				HomeFurnitureStatic.Category.STORAGE,
				HomeFurnitureStatic.Category.INTERIOR,
				HomeFurnitureStatic.Category.ELECTRONICS
			}
		},
		{
			HomeFurnitureStatic.Category.ELECTRONICS,
			new List<HomeFurnitureStatic.Category>
			{
				HomeFurnitureStatic.Category.BED,
				HomeFurnitureStatic.Category.DESK,
				HomeFurnitureStatic.Category.CHAIR,
				HomeFurnitureStatic.Category.STORAGE,
				HomeFurnitureStatic.Category.ORNAMENT
			}
		}
	};

	// Token: 0x02000AF1 RID: 2801
	private class CharaCtrl
	{
		// Token: 0x0400454C RID: 17740
		public int id;

		// Token: 0x0400454D RID: 17741
		public int typ;

		// Token: 0x0400454E RID: 17742
		public CharaModelHandle chara;

		// Token: 0x0400454F RID: 17743
		public int place;

		// Token: 0x04004550 RID: 17744
		public int root;

		// Token: 0x04004551 RID: 17745
		public IEnumerator ctrl;

		// Token: 0x04004552 RID: 17746
		public int busy;

		// Token: 0x04004553 RID: 17747
		public int dress;

		// Token: 0x04004554 RID: 17748
		public bool longSkirt;

		// Token: 0x04004555 RID: 17749
		public DataManagerCharaAccessory.Accessory accessory;

		// Token: 0x04004556 RID: 17750
		public IEnumerator dressup;

		// Token: 0x04004557 RID: 17751
		public bool slp;

		// Token: 0x04004558 RID: 17752
		public bool sit;

		// Token: 0x04004559 RID: 17753
		public float colli;

		// Token: 0x0400455A RID: 17754
		public bool direct;

		// Token: 0x0400455B RID: 17755
		public List<HomeCharaCtrl.CharaCtrl.VoiceSheet> voiceSheet;

		// Token: 0x02001175 RID: 4469
		public class VoiceSheet
		{
			// Token: 0x04005FD7 RID: 24535
			public string sheet;

			// Token: 0x04005FD8 RID: 24536
			public IEnumerator loader;
		}
	}
}
