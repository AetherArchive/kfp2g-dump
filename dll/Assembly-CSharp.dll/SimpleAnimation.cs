using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x020001E9 RID: 489
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Animator))]
public class SimpleAnimation : MonoBehaviour
{
	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x06002096 RID: 8342 RVA: 0x0018C034 File Offset: 0x0018A234
	public Animator animator
	{
		get
		{
			if (this.m_Animator == null)
			{
				this.m_Animator = base.GetComponent<Animator>();
			}
			return this.m_Animator;
		}
	}

	// Token: 0x17000459 RID: 1113
	// (get) Token: 0x06002097 RID: 8343 RVA: 0x0018C056 File Offset: 0x0018A256
	// (set) Token: 0x06002098 RID: 8344 RVA: 0x0018C05E File Offset: 0x0018A25E
	public bool animatePhysics
	{
		get
		{
			return this.m_AnimatePhysics;
		}
		set
		{
			this.m_AnimatePhysics = value;
			this.animator.updateMode = (this.m_AnimatePhysics ? AnimatorUpdateMode.UnscaledTime : AnimatorUpdateMode.Normal);
		}
	}

	// Token: 0x1700045A RID: 1114
	// (get) Token: 0x06002099 RID: 8345 RVA: 0x0018C07E File Offset: 0x0018A27E
	// (set) Token: 0x0600209A RID: 8346 RVA: 0x0018C08B File Offset: 0x0018A28B
	public AnimatorCullingMode cullingMode
	{
		get
		{
			return this.animator.cullingMode;
		}
		set
		{
			this.m_CullingMode = value;
			this.animator.cullingMode = this.m_CullingMode;
		}
	}

	// Token: 0x1700045B RID: 1115
	// (get) Token: 0x0600209B RID: 8347 RVA: 0x0018C0A5 File Offset: 0x0018A2A5
	public bool isPlaying
	{
		get
		{
			return this.m_Playable.IsPlaying();
		}
	}

	// Token: 0x1700045C RID: 1116
	// (get) Token: 0x0600209C RID: 8348 RVA: 0x0018C0B2 File Offset: 0x0018A2B2
	// (set) Token: 0x0600209D RID: 8349 RVA: 0x0018C0BA File Offset: 0x0018A2BA
	public bool playAutomatically
	{
		get
		{
			return this.m_PlayAutomatically;
		}
		set
		{
			this.m_PlayAutomatically = value;
		}
	}

	// Token: 0x1700045D RID: 1117
	// (get) Token: 0x0600209E RID: 8350 RVA: 0x0018C0C3 File Offset: 0x0018A2C3
	// (set) Token: 0x0600209F RID: 8351 RVA: 0x0018C0CB File Offset: 0x0018A2CB
	public AnimationClip clip
	{
		get
		{
			return this.m_Clip;
		}
		set
		{
			SimpleAnimation.LegacyClipCheck(value);
			this.m_Clip = value;
		}
	}

	// Token: 0x1700045E RID: 1118
	// (get) Token: 0x060020A0 RID: 8352 RVA: 0x0018C0DA File Offset: 0x0018A2DA
	// (set) Token: 0x060020A1 RID: 8353 RVA: 0x0018C0E2 File Offset: 0x0018A2E2
	public WrapMode wrapMode
	{
		get
		{
			return this.m_WrapMode;
		}
		set
		{
			this.m_WrapMode = value;
		}
	}

	// Token: 0x060020A2 RID: 8354 RVA: 0x0018C0EB File Offset: 0x0018A2EB
	public void AddClip(AnimationClip clip, string newName)
	{
		SimpleAnimation.LegacyClipCheck(clip);
		this.AddState(clip, newName);
	}

	// Token: 0x060020A3 RID: 8355 RVA: 0x0018C0FB File Offset: 0x0018A2FB
	public void Blend(string stateName, float targetWeight, float fadeLength)
	{
		this.m_Animator.enabled = true;
		this.Kick();
		this.m_Playable.Blend(stateName, targetWeight, fadeLength);
	}

	// Token: 0x060020A4 RID: 8356 RVA: 0x0018C11E File Offset: 0x0018A31E
	public void CrossFade(string stateName, float fadeLength)
	{
		this.m_Animator.enabled = true;
		this.Kick();
		this.m_Playable.Crossfade(stateName, fadeLength);
	}

	// Token: 0x060020A5 RID: 8357 RVA: 0x0018C140 File Offset: 0x0018A340
	public void CrossFadeQueued(string stateName, float fadeLength, QueueMode queueMode)
	{
		this.m_Animator.enabled = true;
		this.Kick();
		this.m_Playable.CrossfadeQueued(stateName, fadeLength, queueMode);
	}

	// Token: 0x060020A6 RID: 8358 RVA: 0x0018C163 File Offset: 0x0018A363
	public int GetClipCount()
	{
		return this.m_Playable.GetClipCount();
	}

	// Token: 0x060020A7 RID: 8359 RVA: 0x0018C170 File Offset: 0x0018A370
	public bool IsPlaying(string stateName)
	{
		return this.m_Playable.IsPlaying(stateName);
	}

	// Token: 0x060020A8 RID: 8360 RVA: 0x0018C17E File Offset: 0x0018A37E
	public void Stop()
	{
		this.m_Playable.StopAll();
	}

	// Token: 0x060020A9 RID: 8361 RVA: 0x0018C18C File Offset: 0x0018A38C
	public void Stop(string stateName)
	{
		this.m_Playable.Stop(stateName);
	}

	// Token: 0x060020AA RID: 8362 RVA: 0x0018C19B File Offset: 0x0018A39B
	public void Sample()
	{
		this.m_Graph.Evaluate();
	}

	// Token: 0x060020AB RID: 8363 RVA: 0x0018C1A8 File Offset: 0x0018A3A8
	public bool Play()
	{
		this.m_Animator.enabled = true;
		this.Kick();
		if (this.m_Clip != null)
		{
			this.m_Playable.Play(this.m_Clip.name);
		}
		return false;
	}

	// Token: 0x060020AC RID: 8364 RVA: 0x0018C1E2 File Offset: 0x0018A3E2
	public void AddState(AnimationClip clip, string name)
	{
		SimpleAnimation.LegacyClipCheck(clip);
		this.Kick();
		if (this.m_Playable.AddClip(clip, name))
		{
			this.RebuildStates();
		}
	}

	// Token: 0x060020AD RID: 8365 RVA: 0x0018C205 File Offset: 0x0018A405
	public void RemoveState(string name)
	{
		if (this.m_Playable.RemoveClip(name))
		{
			this.RebuildStates();
		}
	}

	// Token: 0x060020AE RID: 8366 RVA: 0x0018C21B File Offset: 0x0018A41B
	public bool Play(string stateName)
	{
		this.m_Animator.enabled = true;
		this.Kick();
		return this.m_Playable.Play(stateName);
	}

	// Token: 0x060020AF RID: 8367 RVA: 0x0018C23B File Offset: 0x0018A43B
	public void PlayQueued(string stateName, QueueMode queueMode)
	{
		this.m_Animator.enabled = true;
		this.Kick();
		this.m_Playable.PlayQueued(stateName, queueMode);
	}

	// Token: 0x060020B0 RID: 8368 RVA: 0x0018C25D File Offset: 0x0018A45D
	public void RemoveClip(AnimationClip clip)
	{
		if (clip == null)
		{
			throw new NullReferenceException("clip");
		}
		if (this.m_Playable.RemoveClip(clip))
		{
			this.RebuildStates();
		}
	}

	// Token: 0x060020B1 RID: 8369 RVA: 0x0018C287 File Offset: 0x0018A487
	public void Rewind()
	{
		this.Kick();
		this.m_Playable.Rewind();
	}

	// Token: 0x060020B2 RID: 8370 RVA: 0x0018C29A File Offset: 0x0018A49A
	public void Rewind(string stateName)
	{
		this.Kick();
		this.m_Playable.Rewind(stateName);
	}

	// Token: 0x060020B3 RID: 8371 RVA: 0x0018C2B0 File Offset: 0x0018A4B0
	public SimpleAnimation.State GetState(string stateName)
	{
		SimpleAnimationPlayable playable = this.m_Playable;
		SimpleAnimationPlayable.IState state = ((playable != null) ? playable.GetState(stateName) : null);
		if (state == null)
		{
			return null;
		}
		return new SimpleAnimation.StateImpl(state, this);
	}

	// Token: 0x060020B4 RID: 8372 RVA: 0x0018C2DD File Offset: 0x0018A4DD
	public IEnumerable<SimpleAnimation.State> GetStates()
	{
		return new SimpleAnimation.StateEnumerable(this);
	}

	// Token: 0x1700045F RID: 1119
	public SimpleAnimation.State this[string name]
	{
		get
		{
			return this.GetState(name);
		}
	}

	// Token: 0x060020B6 RID: 8374 RVA: 0x0018C2F0 File Offset: 0x0018A4F0
	private void UpdateExAnimation()
	{
		if (this.currentExFinishCallback != null && !this.ExIsPlaying())
		{
			SimpleAnimation.ExFinishCallback exFinishCallback = this.currentExFinishCallback;
			this.currentExFinishCallback = null;
			exFinishCallback();
		}
		if (this.currentExFinishCallback != null && (this.lastPlayStateName == SimpleAnimation.ExPguiStatus.LOOP.ToString() || this.lastPlayStateName == SimpleAnimation.ExPguiStatus.LOOP_SUB.ToString()))
		{
			if (this.frame > this.ExGetTime())
			{
				this.currentExFinishCallback();
			}
			this.frame = this.ExGetTime();
		}
	}

	// Token: 0x060020B7 RID: 8375 RVA: 0x0018C384 File Offset: 0x0018A584
	public void ExPlayAnimation(SimpleAnimation.ExPguiStatus uiType, SimpleAnimation.ExFinishCallback finishCb = null)
	{
		this.ExPlayAnimation(uiType.ToString(), finishCb);
	}

	// Token: 0x060020B8 RID: 8376 RVA: 0x0018C39A File Offset: 0x0018A59A
	public void ExPlayAnimation(string stateName, SimpleAnimation.ExFinishCallback finishCb = null)
	{
		if (this.m_Playable == null)
		{
			return;
		}
		this.Stop();
		this.Rewind();
		this[stateName].speed = 1f;
		this.Play(stateName);
		this.lastPlayStateName = stateName;
		this.currentExFinishCallback = finishCb;
	}

	// Token: 0x060020B9 RID: 8377 RVA: 0x0018C3D8 File Offset: 0x0018A5D8
	public void ExPlayAnimation(string stateName, float startTime, float speed)
	{
		if (this.m_Playable == null)
		{
			return;
		}
		this.Stop();
		this[stateName].time = startTime;
		this[stateName].speed = speed;
		this.Play(stateName);
		this.lastPlayStateName = stateName;
	}

	// Token: 0x060020BA RID: 8378 RVA: 0x0018C412 File Offset: 0x0018A612
	public void ExPlayAnimationCrossFade(SimpleAnimation.ExPguiStatus uiType, float fadeLength = 0.2f, SimpleAnimation.ExFinishCallback finishCb = null)
	{
		this.ExPlayAnimationCrossFade(uiType.ToString(), fadeLength, finishCb);
	}

	// Token: 0x060020BB RID: 8379 RVA: 0x0018C429 File Offset: 0x0018A629
	public void ExPlayAnimationCrossFade(string stateName, float fadeLength = 0.2f, SimpleAnimation.ExFinishCallback finishCb = null)
	{
		this.Rewind();
		this.CrossFade(stateName, fadeLength);
		this.lastPlayStateName = stateName;
		this.currentExFinishCallback = finishCb;
	}

	// Token: 0x060020BC RID: 8380 RVA: 0x0018C447 File Offset: 0x0018A647
	public void ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus uiType)
	{
		this.ExPauseAnimationLastFrame(uiType.ToString());
	}

	// Token: 0x060020BD RID: 8381 RVA: 0x0018C45C File Offset: 0x0018A65C
	public void ExPauseAnimationLastFrame(string stateName)
	{
		SimpleAnimation.State state = this.GetState(stateName);
		if (state != null)
		{
			this.ExPlayAnimation(stateName, null);
			state.speed = 0f;
			state.time = state.length;
		}
	}

	// Token: 0x060020BE RID: 8382 RVA: 0x0018C493 File Offset: 0x0018A693
	public void ExPauseAnimation(SimpleAnimation.ExPguiStatus uiType, SimpleAnimation.ExFinishCallback finishCb = null)
	{
		this.ExPauseAnimation(uiType.ToString());
	}

	// Token: 0x060020BF RID: 8383 RVA: 0x0018C4A8 File Offset: 0x0018A6A8
	public void ExPauseAnimation(string stateName)
	{
		SimpleAnimation.State state = this.GetState(stateName);
		if (state != null)
		{
			this.ExPlayAnimation(stateName, null);
			state.speed = 0f;
		}
	}

	// Token: 0x060020C0 RID: 8384 RVA: 0x0018C4D4 File Offset: 0x0018A6D4
	public void ExPauseAnimation()
	{
		SimpleAnimation.State state = this.GetState(this.lastPlayStateName);
		if (state != null)
		{
			state.speed = 0f;
		}
	}

	// Token: 0x060020C1 RID: 8385 RVA: 0x0018C4FC File Offset: 0x0018A6FC
	public void ExResumeAnimation(SimpleAnimation.ExFinishCallback finishCb = null)
	{
		SimpleAnimation.State state = this.GetState(this.lastPlayStateName);
		if (state != null)
		{
			state.speed = 1f;
		}
		this.currentExFinishCallback = finishCb;
	}

	// Token: 0x060020C2 RID: 8386 RVA: 0x0018C52C File Offset: 0x0018A72C
	public bool ExIsPlaying()
	{
		if (this.m_Playable == null)
		{
			return false;
		}
		SimpleAnimation.State state = this.GetState(this.lastPlayStateName);
		if (state != null)
		{
			if (!state.enabled)
			{
				state.enabled = true;
			}
			return state.clip.isLooping || state.normalizedTime < 1f;
		}
		return false;
	}

	// Token: 0x060020C3 RID: 8387 RVA: 0x0018C580 File Offset: 0x0018A780
	public bool ExIsCurrent(SimpleAnimation.ExPguiStatus uiType)
	{
		return uiType.ToString() == this.lastPlayStateName;
	}

	// Token: 0x060020C4 RID: 8388 RVA: 0x0018C59A File Offset: 0x0018A79A
	public bool ExIsCurrent(string stateName)
	{
		return stateName == this.lastPlayStateName;
	}

	// Token: 0x060020C5 RID: 8389 RVA: 0x0018C5A8 File Offset: 0x0018A7A8
	public float ExGetTime()
	{
		SimpleAnimation.State state = this.GetState(this.lastPlayStateName);
		if (state == null)
		{
			return 0f;
		}
		float num = state.normalizedTime;
		if (num > 1f)
		{
			num %= 1f;
		}
		return num;
	}

	// Token: 0x060020C6 RID: 8390 RVA: 0x0018C5E3 File Offset: 0x0018A7E3
	public string ExGetLastPlayStateName()
	{
		return this.lastPlayStateName;
	}

	// Token: 0x060020C7 RID: 8391 RVA: 0x0018C5EC File Offset: 0x0018A7EC
	public float ExGetSpeed()
	{
		SimpleAnimation.State state = this.GetState(this.lastPlayStateName);
		if (state == null)
		{
			return 0f;
		}
		return state.speed;
	}

	// Token: 0x060020C8 RID: 8392 RVA: 0x0018C618 File Offset: 0x0018A818
	public float ExGetAbsTime()
	{
		SimpleAnimation.State state = this.GetState(this.lastPlayStateName);
		if (state == null)
		{
			return 0f;
		}
		return state.time;
	}

	// Token: 0x060020C9 RID: 8393 RVA: 0x0018C641 File Offset: 0x0018A841
	public void ExStop(bool isStopInternal = true)
	{
		this.lastPlayStateName = "";
		if (isStopInternal)
		{
			this.Stop();
		}
	}

	// Token: 0x060020CA RID: 8394 RVA: 0x0018C657 File Offset: 0x0018A857
	public void ExInit()
	{
		this.Initialize();
	}

	// Token: 0x060020CB RID: 8395 RVA: 0x0018C65F File Offset: 0x0018A85F
	private void Update()
	{
		this.UpdateExAnimation();
	}

	// Token: 0x060020CC RID: 8396 RVA: 0x0018C667 File Offset: 0x0018A867
	protected void Kick()
	{
		if (!this.m_IsPlaying)
		{
			this.m_Graph.Play();
			this.m_IsPlaying = true;
		}
	}

	// Token: 0x060020CD RID: 8397 RVA: 0x0018C683 File Offset: 0x0018A883
	protected virtual void OnEnable()
	{
		this.Initialize();
		this.m_Graph.Play();
		if (this.m_PlayAutomatically)
		{
			this.Stop();
			this.Play();
		}
	}

	// Token: 0x060020CE RID: 8398 RVA: 0x0018C6AB File Offset: 0x0018A8AB
	protected virtual void OnDisable()
	{
		if (this.m_Initialized)
		{
			this.Stop();
			this.m_Graph.Stop();
		}
	}

	// Token: 0x060020CF RID: 8399 RVA: 0x0018C6C6 File Offset: 0x0018A8C6
	private void Reset()
	{
		if (this.m_Graph.IsValid())
		{
			this.m_Graph.Destroy();
		}
		this.m_Initialized = false;
	}

	// Token: 0x060020D0 RID: 8400 RVA: 0x0018C6E8 File Offset: 0x0018A8E8
	private void Initialize()
	{
		if (this.m_Initialized)
		{
			return;
		}
		this.m_Animator = base.GetComponent<Animator>();
		this.m_Animator.updateMode = (this.m_AnimatePhysics ? AnimatorUpdateMode.UnscaledTime : AnimatorUpdateMode.Normal);
		this.m_Animator.cullingMode = this.m_CullingMode;
		this.m_Animator.runtimeAnimatorController = null;
		this.m_Graph = PlayableGraph.Create();
		this.m_Graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
		SimpleAnimationPlayable simpleAnimationPlayable = new SimpleAnimationPlayable();
		this.m_Playable = ScriptPlayable<SimpleAnimationPlayable>.Create(this.m_Graph, simpleAnimationPlayable, 1).GetBehaviour();
		SimpleAnimationPlayable playable = this.m_Playable;
		playable.onDone = (Action)Delegate.Combine(playable.onDone, new Action(this.OnPlayableDone));
		if (this.m_States == null)
		{
			this.m_States = new SimpleAnimation.EditorState[1];
			this.m_States[0] = new SimpleAnimation.EditorState();
			this.m_States[0].defaultState = true;
			this.m_States[0].name = "Default";
		}
		if (this.m_States != null)
		{
			foreach (SimpleAnimation.EditorState editorState in this.m_States)
			{
				if (editorState.clip)
				{
					this.m_Playable.AddClip(editorState.clip, editorState.name);
				}
			}
		}
		this.EnsureDefaultStateExists();
		AnimationPlayableUtilities.Play(this.m_Animator, this.m_Playable.playable, this.m_Graph);
		this.Play();
		this.Kick();
		this.m_Initialized = true;
	}

	// Token: 0x060020D1 RID: 8401 RVA: 0x0018C860 File Offset: 0x0018AA60
	private void EnsureDefaultStateExists()
	{
		if (this.m_Playable != null && this.m_Clip != null && this.m_Playable.GetState(this.m_Clip.name) == null)
		{
			this.m_Playable.AddClip(this.m_Clip, this.m_Clip.name);
			this.Kick();
		}
	}

	// Token: 0x060020D2 RID: 8402 RVA: 0x0018C8BE File Offset: 0x0018AABE
	protected virtual void Awake()
	{
		this.Initialize();
	}

	// Token: 0x060020D3 RID: 8403 RVA: 0x0018C8C6 File Offset: 0x0018AAC6
	protected void OnDestroy()
	{
		if (this.m_Graph.IsValid())
		{
			this.m_Graph.Destroy();
		}
	}

	// Token: 0x060020D4 RID: 8404 RVA: 0x0018C8E0 File Offset: 0x0018AAE0
	private void OnPlayableDone()
	{
		this.m_Graph.Stop();
		this.m_IsPlaying = false;
	}

	// Token: 0x060020D5 RID: 8405 RVA: 0x0018C8F4 File Offset: 0x0018AAF4
	private void RebuildStates()
	{
		IEnumerable<SimpleAnimation.State> states = this.GetStates();
		List<SimpleAnimation.EditorState> list = new List<SimpleAnimation.EditorState>();
		foreach (SimpleAnimation.State state in states)
		{
			list.Add(new SimpleAnimation.EditorState
			{
				clip = state.clip,
				name = state.name
			});
		}
		this.m_States = list.ToArray();
	}

	// Token: 0x060020D6 RID: 8406 RVA: 0x0018C974 File Offset: 0x0018AB74
	private SimpleAnimation.EditorState CreateDefaultEditorState()
	{
		return new SimpleAnimation.EditorState
		{
			name = "Default",
			clip = this.m_Clip,
			defaultState = true
		};
	}

	// Token: 0x060020D7 RID: 8407 RVA: 0x0018C999 File Offset: 0x0018AB99
	private static void LegacyClipCheck(AnimationClip clip)
	{
		if (clip && clip.legacy)
		{
			throw new ArgumentException(string.Format("Legacy clip {0} cannot be used in this component. Set .legacy property to false before using this clip", clip));
		}
	}

	// Token: 0x060020D8 RID: 8408 RVA: 0x0018C9BC File Offset: 0x0018ABBC
	private void InvalidLegacyClipError(string clipName, string stateName)
	{
	}

	// Token: 0x060020D9 RID: 8409 RVA: 0x0018C9C0 File Offset: 0x0018ABC0
	private void OnValidate()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (this.m_Clip && this.m_Clip.legacy)
		{
			this.m_Clip = null;
		}
		if (this.m_States == null || this.m_States.Length == 0)
		{
			this.m_States = new SimpleAnimation.EditorState[1];
		}
		if (this.m_States[0] == null)
		{
			this.m_States[0] = this.CreateDefaultEditorState();
		}
		if (!this.m_States[0].defaultState || this.m_States[0].name != "Default")
		{
			SimpleAnimation.EditorState[] states = this.m_States;
			this.m_States = new SimpleAnimation.EditorState[states.Length + 1];
			this.CreateDefaultEditorState();
			states.CopyTo(this.m_States, 1);
		}
		if (this.m_States[0].clip != this.m_Clip)
		{
			this.m_States[0].clip = this.m_Clip;
		}
		for (int i = 1; i < this.m_States.Length; i++)
		{
			if (this.m_States[i] == null)
			{
				this.m_States[i] = new SimpleAnimation.EditorState();
			}
			this.m_States[i].defaultState = false;
		}
		int num = this.m_States.Length;
		string[] array = new string[num];
		for (int j = 0; j < num; j++)
		{
			SimpleAnimation.EditorState editorState = this.m_States[j];
			if (editorState.name == "" && editorState.clip)
			{
				editorState.name = editorState.clip.name;
			}
			array[j] = editorState.name;
			if (editorState.clip && editorState.clip.legacy)
			{
				this.InvalidLegacyClipError(editorState.clip.name, editorState.name);
				editorState.clip = null;
			}
		}
		this.m_Animator = base.GetComponent<Animator>();
		this.m_Animator.updateMode = (this.m_AnimatePhysics ? AnimatorUpdateMode.UnscaledTime : AnimatorUpdateMode.Normal);
		this.m_Animator.cullingMode = this.m_CullingMode;
	}

	// Token: 0x040017AD RID: 6061
	private string lastPlayStateName = "";

	// Token: 0x040017AE RID: 6062
	private SimpleAnimation.ExFinishCallback currentExFinishCallback;

	// Token: 0x040017AF RID: 6063
	private float frame;

	// Token: 0x040017B0 RID: 6064
	protected PlayableGraph m_Graph;

	// Token: 0x040017B1 RID: 6065
	protected PlayableHandle m_LayerMixer;

	// Token: 0x040017B2 RID: 6066
	protected PlayableHandle m_TransitionMixer;

	// Token: 0x040017B3 RID: 6067
	protected Animator m_Animator;

	// Token: 0x040017B4 RID: 6068
	protected bool m_Initialized;

	// Token: 0x040017B5 RID: 6069
	protected bool m_IsPlaying;

	// Token: 0x040017B6 RID: 6070
	protected SimpleAnimationPlayable m_Playable;

	// Token: 0x040017B7 RID: 6071
	[SerializeField]
	protected bool m_PlayAutomatically = true;

	// Token: 0x040017B8 RID: 6072
	[SerializeField]
	protected bool m_AnimatePhysics;

	// Token: 0x040017B9 RID: 6073
	[SerializeField]
	protected AnimatorCullingMode m_CullingMode = AnimatorCullingMode.CullUpdateTransforms;

	// Token: 0x040017BA RID: 6074
	[SerializeField]
	protected WrapMode m_WrapMode;

	// Token: 0x040017BB RID: 6075
	[SerializeField]
	protected AnimationClip m_Clip;

	// Token: 0x040017BC RID: 6076
	[SerializeField]
	private SimpleAnimation.EditorState[] m_States;

	// Token: 0x02001030 RID: 4144
	public interface State
	{
		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x06005219 RID: 21017
		// (set) Token: 0x0600521A RID: 21018
		bool enabled { get; set; }

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x0600521B RID: 21019
		bool isValid { get; }

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x0600521C RID: 21020
		// (set) Token: 0x0600521D RID: 21021
		float time { get; set; }

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x0600521E RID: 21022
		// (set) Token: 0x0600521F RID: 21023
		float normalizedTime { get; set; }

		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x06005220 RID: 21024
		// (set) Token: 0x06005221 RID: 21025
		float speed { get; set; }

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x06005222 RID: 21026
		// (set) Token: 0x06005223 RID: 21027
		string name { get; set; }

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x06005224 RID: 21028
		// (set) Token: 0x06005225 RID: 21029
		float weight { get; set; }

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x06005226 RID: 21030
		float length { get; }

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x06005227 RID: 21031
		AnimationClip clip { get; }

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x06005228 RID: 21032
		// (set) Token: 0x06005229 RID: 21033
		WrapMode wrapMode { get; set; }
	}

	// Token: 0x02001031 RID: 4145
	public enum ExPguiStatus
	{
		// Token: 0x04005AD3 RID: 23251
		START,
		// Token: 0x04005AD4 RID: 23252
		START_SUB,
		// Token: 0x04005AD5 RID: 23253
		LOOP,
		// Token: 0x04005AD6 RID: 23254
		LOOP_SUB,
		// Token: 0x04005AD7 RID: 23255
		END,
		// Token: 0x04005AD8 RID: 23256
		END_SUB,
		// Token: 0x04005AD9 RID: 23257
		MAX
	}

	// Token: 0x02001032 RID: 4146
	// (Invoke) Token: 0x0600522B RID: 21035
	public delegate void ExFinishCallback();

	// Token: 0x02001033 RID: 4147
	private class StateEnumerable : IEnumerable<SimpleAnimation.State>, IEnumerable
	{
		// Token: 0x0600522E RID: 21038 RVA: 0x0024854D File Offset: 0x0024674D
		public StateEnumerable(SimpleAnimation owner)
		{
			this.m_Owner = owner;
		}

		// Token: 0x0600522F RID: 21039 RVA: 0x0024855C File Offset: 0x0024675C
		public IEnumerator<SimpleAnimation.State> GetEnumerator()
		{
			return new SimpleAnimation.StateEnumerable.StateEnumerator(this.m_Owner);
		}

		// Token: 0x06005230 RID: 21040 RVA: 0x00248569 File Offset: 0x00246769
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SimpleAnimation.StateEnumerable.StateEnumerator(this.m_Owner);
		}

		// Token: 0x04005ADA RID: 23258
		private SimpleAnimation m_Owner;

		// Token: 0x02001228 RID: 4648
		private class StateEnumerator : IEnumerator<SimpleAnimation.State>, IEnumerator, IDisposable
		{
			// Token: 0x060057F1 RID: 22513 RVA: 0x00259F47 File Offset: 0x00258147
			public StateEnumerator(SimpleAnimation owner)
			{
				this.m_Owner = owner;
				this.m_Impl = this.m_Owner.m_Playable.GetStates().GetEnumerator();
				this.Reset();
			}

			// Token: 0x060057F2 RID: 22514 RVA: 0x00259F77 File Offset: 0x00258177
			private SimpleAnimation.State GetCurrent()
			{
				return new SimpleAnimation.StateImpl(this.m_Impl.Current, this.m_Owner);
			}

			// Token: 0x17000D09 RID: 3337
			// (get) Token: 0x060057F3 RID: 22515 RVA: 0x00259F8F File Offset: 0x0025818F
			object IEnumerator.Current
			{
				get
				{
					return this.GetCurrent();
				}
			}

			// Token: 0x17000D0A RID: 3338
			// (get) Token: 0x060057F4 RID: 22516 RVA: 0x00259F97 File Offset: 0x00258197
			SimpleAnimation.State IEnumerator<SimpleAnimation.State>.Current
			{
				get
				{
					return this.GetCurrent();
				}
			}

			// Token: 0x060057F5 RID: 22517 RVA: 0x00259F9F File Offset: 0x0025819F
			public void Dispose()
			{
			}

			// Token: 0x060057F6 RID: 22518 RVA: 0x00259FA1 File Offset: 0x002581A1
			public bool MoveNext()
			{
				return this.m_Impl.MoveNext();
			}

			// Token: 0x060057F7 RID: 22519 RVA: 0x00259FAE File Offset: 0x002581AE
			public void Reset()
			{
				this.m_Impl.Reset();
			}

			// Token: 0x04006373 RID: 25459
			private SimpleAnimation m_Owner;

			// Token: 0x04006374 RID: 25460
			private IEnumerator<SimpleAnimationPlayable.IState> m_Impl;
		}
	}

	// Token: 0x02001034 RID: 4148
	private class StateImpl : SimpleAnimation.State
	{
		// Token: 0x06005231 RID: 21041 RVA: 0x00248576 File Offset: 0x00246776
		public StateImpl(SimpleAnimationPlayable.IState handle, SimpleAnimation component)
		{
			this.m_StateHandle = handle;
			this.m_Component = component;
		}

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x06005232 RID: 21042 RVA: 0x0024858C File Offset: 0x0024678C
		// (set) Token: 0x06005233 RID: 21043 RVA: 0x00248599 File Offset: 0x00246799
		bool SimpleAnimation.State.enabled
		{
			get
			{
				return this.m_StateHandle.enabled;
			}
			set
			{
				this.m_StateHandle.enabled = value;
				if (value)
				{
					this.m_Component.Kick();
				}
			}
		}

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06005234 RID: 21044 RVA: 0x002485B5 File Offset: 0x002467B5
		bool SimpleAnimation.State.isValid
		{
			get
			{
				return this.m_StateHandle.IsValid();
			}
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06005235 RID: 21045 RVA: 0x002485C2 File Offset: 0x002467C2
		// (set) Token: 0x06005236 RID: 21046 RVA: 0x002485CF File Offset: 0x002467CF
		float SimpleAnimation.State.time
		{
			get
			{
				return this.m_StateHandle.time;
			}
			set
			{
				this.m_StateHandle.time = value;
				this.m_Component.Kick();
			}
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06005237 RID: 21047 RVA: 0x002485E8 File Offset: 0x002467E8
		// (set) Token: 0x06005238 RID: 21048 RVA: 0x002485F5 File Offset: 0x002467F5
		float SimpleAnimation.State.normalizedTime
		{
			get
			{
				return this.m_StateHandle.normalizedTime;
			}
			set
			{
				this.m_StateHandle.normalizedTime = value;
				this.m_Component.Kick();
			}
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06005239 RID: 21049 RVA: 0x0024860E File Offset: 0x0024680E
		// (set) Token: 0x0600523A RID: 21050 RVA: 0x0024861B File Offset: 0x0024681B
		float SimpleAnimation.State.speed
		{
			get
			{
				return this.m_StateHandle.speed;
			}
			set
			{
				this.m_StateHandle.speed = value;
				this.m_Component.Kick();
			}
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x0600523B RID: 21051 RVA: 0x00248634 File Offset: 0x00246834
		// (set) Token: 0x0600523C RID: 21052 RVA: 0x00248641 File Offset: 0x00246841
		string SimpleAnimation.State.name
		{
			get
			{
				return this.m_StateHandle.name;
			}
			set
			{
				this.m_StateHandle.name = value;
			}
		}

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x0600523D RID: 21053 RVA: 0x0024864F File Offset: 0x0024684F
		// (set) Token: 0x0600523E RID: 21054 RVA: 0x0024865C File Offset: 0x0024685C
		float SimpleAnimation.State.weight
		{
			get
			{
				return this.m_StateHandle.weight;
			}
			set
			{
				this.m_StateHandle.weight = value;
				this.m_Component.Kick();
			}
		}

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x0600523F RID: 21055 RVA: 0x00248675 File Offset: 0x00246875
		float SimpleAnimation.State.length
		{
			get
			{
				return this.m_StateHandle.length;
			}
		}

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x06005240 RID: 21056 RVA: 0x00248682 File Offset: 0x00246882
		AnimationClip SimpleAnimation.State.clip
		{
			get
			{
				return this.m_StateHandle.clip;
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x06005241 RID: 21057 RVA: 0x0024868F File Offset: 0x0024688F
		// (set) Token: 0x06005242 RID: 21058 RVA: 0x0024869C File Offset: 0x0024689C
		WrapMode SimpleAnimation.State.wrapMode
		{
			get
			{
				return this.m_StateHandle.wrapMode;
			}
			set
			{
			}
		}

		// Token: 0x04005ADB RID: 23259
		private SimpleAnimationPlayable.IState m_StateHandle;

		// Token: 0x04005ADC RID: 23260
		private SimpleAnimation m_Component;
	}

	// Token: 0x02001035 RID: 4149
	[Serializable]
	public class EditorState
	{
		// Token: 0x04005ADD RID: 23261
		public AnimationClip clip;

		// Token: 0x04005ADE RID: 23262
		public string name;

		// Token: 0x04005ADF RID: 23263
		public bool defaultState;
	}
}
