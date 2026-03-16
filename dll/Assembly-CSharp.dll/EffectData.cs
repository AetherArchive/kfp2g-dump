using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectData
{
	public GameObject effectObject
	{
		get
		{
			return this._effectObject;
		}
	}

	public List<ParticleSystem> particleList { get; private set; }

	public string EffectName { get; private set; }

	public string Publisher { get; private set; }

	public int NormalLayer { get; private set; }

	public void SetActive(bool a)
	{
		this.effectObject.SetActive(a);
	}

	public void SetPublisher(string chara)
	{
		this.Publisher = chara;
	}

	public void SetNormalLayer(int layer)
	{
		this.NormalLayer = layer;
	}

	public void PlayEffect(bool isFast = false)
	{
		this.SetActive(true);
		this.SetNormalizedTime(0f);
		if (this.soundParam != null)
		{
			SoundManager.Play(this.soundParam.seName, false, isFast);
		}
	}

	public void UpdateEffectSESpeed(bool isFast)
	{
		SoundManager.SetSESpeed(this.soundParam.seName, isFast);
	}

	public void PlayEffectWithoutSound()
	{
		this.SetActive(true);
		this.SetNormalizedTime(0f);
	}

	public float GetNormailzedTime()
	{
		if (this._animator == null)
		{
			return 0f;
		}
		return this._animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
	}

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

	public int GetFrameTime()
	{
		if (this._animator == null)
		{
			return -1;
		}
		return (int)(this._animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 30f);
	}

	public int GetMaxFrame()
	{
		return (int)(this.maxFrame * 30f);
	}

	public void SetSpeed(float speed)
	{
		if (this._animator == null)
		{
			return;
		}
		this._animator.speed = speed;
	}

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

	public EffectData(GameObject obj, EffectSeParameter.PackData seData, string effectName)
	{
		this.setObj(obj);
		this.SetActive(false);
		this.soundParam = seData;
		this.EffectName = effectName;
	}

	public void Destory()
	{
		if (this._effectObject != null)
		{
			Object.Destroy(this._effectObject);
			this._effectObject = null;
		}
	}

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

	private GameObject _effectObject;

	private Animator _animator;

	private float maxFrame;

	private EffectSeParameter.PackData soundParam;
}
