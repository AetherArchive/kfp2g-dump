using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C5 RID: 197
public class EffectData
{
	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x060008D1 RID: 2257 RVA: 0x000388A0 File Offset: 0x00036AA0
	public GameObject effectObject
	{
		get
		{
			return this._effectObject;
		}
	}

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x060008D2 RID: 2258 RVA: 0x000388A8 File Offset: 0x00036AA8
	// (set) Token: 0x060008D3 RID: 2259 RVA: 0x000388B0 File Offset: 0x00036AB0
	public List<ParticleSystem> particleList { get; private set; }

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x060008D4 RID: 2260 RVA: 0x000388B9 File Offset: 0x00036AB9
	// (set) Token: 0x060008D5 RID: 2261 RVA: 0x000388C1 File Offset: 0x00036AC1
	public string EffectName { get; private set; }

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x060008D6 RID: 2262 RVA: 0x000388CA File Offset: 0x00036ACA
	// (set) Token: 0x060008D7 RID: 2263 RVA: 0x000388D2 File Offset: 0x00036AD2
	public string Publisher { get; private set; }

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x060008D8 RID: 2264 RVA: 0x000388DB File Offset: 0x00036ADB
	// (set) Token: 0x060008D9 RID: 2265 RVA: 0x000388E3 File Offset: 0x00036AE3
	public int NormalLayer { get; private set; }

	// Token: 0x060008DA RID: 2266 RVA: 0x000388EC File Offset: 0x00036AEC
	public void SetActive(bool a)
	{
		this.effectObject.SetActive(a);
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x000388FA File Offset: 0x00036AFA
	public void SetPublisher(string chara)
	{
		this.Publisher = chara;
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x00038903 File Offset: 0x00036B03
	public void SetNormalLayer(int layer)
	{
		this.NormalLayer = layer;
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x0003890C File Offset: 0x00036B0C
	public void PlayEffect(bool isFast = false)
	{
		this.SetActive(true);
		this.SetNormalizedTime(0f);
		if (this.soundParam != null)
		{
			SoundManager.Play(this.soundParam.seName, false, isFast);
		}
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x0003893B File Offset: 0x00036B3B
	public void UpdateEffectSESpeed(bool isFast)
	{
		SoundManager.SetSESpeed(this.soundParam.seName, isFast);
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x0003894E File Offset: 0x00036B4E
	public void PlayEffectWithoutSound()
	{
		this.SetActive(true);
		this.SetNormalizedTime(0f);
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x00038964 File Offset: 0x00036B64
	public float GetNormailzedTime()
	{
		if (this._animator == null)
		{
			return 0f;
		}
		return this._animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x0003899C File Offset: 0x00036B9C
	public void SetNormalizedTime(float normalizedTime)
	{
		if (this._animator == null)
		{
			return;
		}
		AnimatorStateInfo currentAnimatorStateInfo = this._animator.GetCurrentAnimatorStateInfo(0);
		this._animator.Play(currentAnimatorStateInfo.fullPathHash, 0, normalizedTime);
		this._animator.Update(0f);
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x000389EC File Offset: 0x00036BEC
	public void SetFrameTime(int frameTime)
	{
		if (this._animator == null)
		{
			return;
		}
		AnimatorStateInfo currentAnimatorStateInfo = this._animator.GetCurrentAnimatorStateInfo(0);
		float num = (float)frameTime / this.maxFrame / 30f;
		this._animator.Play(currentAnimatorStateInfo.fullPathHash, 0, num);
		this._animator.Update(0f);
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x00038A4C File Offset: 0x00036C4C
	public int GetFrameTime()
	{
		if (this._animator == null)
		{
			return -1;
		}
		return (int)(this._animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 30f);
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x00038A84 File Offset: 0x00036C84
	public int GetMaxFrame()
	{
		return (int)(this.maxFrame * 30f);
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x00038A93 File Offset: 0x00036C93
	public void SetSpeed(float speed)
	{
		if (this._animator == null)
		{
			return;
		}
		this._animator.speed = speed;
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x00038AB0 File Offset: 0x00036CB0
	public bool IsFinishByAnimation()
	{
		if (this._animator == null)
		{
			bool flag = false;
			for (int i = 0; i < this.particleList.Count; i++)
			{
				if (this.particleList[i].isPlaying)
				{
					flag = true;
				}
			}
			return !flag;
		}
		return !this._animator.GetCurrentAnimatorStateInfo(0).loop && this.GetNormailzedTime() >= 1f;
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x00038B25 File Offset: 0x00036D25
	public EffectData(GameObject obj, EffectSeParameter.PackData seData, string effectName)
	{
		this.setObj(obj);
		this.SetActive(false);
		this.soundParam = seData;
		this.EffectName = effectName;
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x00038B49 File Offset: 0x00036D49
	public void Destory()
	{
		if (this._effectObject != null)
		{
			Object.Destroy(this._effectObject);
			this._effectObject = null;
		}
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x00038B6C File Offset: 0x00036D6C
	private void setObj(GameObject obj)
	{
		if (obj == null)
		{
			return;
		}
		this._effectObject = obj;
		this._animator = obj.GetComponentInChildren<Animator>();
		if (this._animator != null)
		{
			AnimatorClipInfo[] currentAnimatorClipInfo = this._animator.GetCurrentAnimatorClipInfo(0);
			if (currentAnimatorClipInfo.Length != 0)
			{
				this.maxFrame = currentAnimatorClipInfo[0].clip.length;
			}
		}
		this.particleList = new List<ParticleSystem>(obj.GetComponentsInChildren<ParticleSystem>());
	}

	// Token: 0x04000751 RID: 1873
	private GameObject _effectObject;

	// Token: 0x04000752 RID: 1874
	private Animator _animator;

	// Token: 0x04000753 RID: 1875
	private float maxFrame;

	// Token: 0x04000758 RID: 1880
	private EffectSeParameter.PackData soundParam;
}
