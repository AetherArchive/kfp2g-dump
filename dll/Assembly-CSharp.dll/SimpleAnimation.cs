using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Animator))]
public class SimpleAnimation : MonoBehaviour
{
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

	public bool isPlaying
	{
		get
		{
			return this.m_Playable.IsPlaying();
		}
	}

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

	public void AddClip(AnimationClip clip, string newName)
	{
		SimpleAnimation.LegacyClipCheck(clip);
		this.AddState(clip, newName);
	}

	public void Blend(string stateName, float targetWeight, float fadeLength)
	{
		this.m_Animator.enabled = true;
		this.Kick();
		this.m_Playable.Blend(stateName, targetWeight, fadeLength);
	}

	public void CrossFade(string stateName, float fadeLength)
	{
		this.m_Animator.enabled = true;
		this.Kick();
		this.m_Playable.Crossfade(stateName, fadeLength);
	}

	public void CrossFadeQueued(string stateName, float fadeLength, QueueMode queueMode)
	{
		this.m_Animator.enabled = true;
		this.Kick();
		this.m_Playable.CrossfadeQueued(stateName, fadeLength, queueMode);
	}

	public int GetClipCount()
	{
		return this.m_Playable.GetClipCount();
	}

	public bool IsPlaying(string stateName)
	{
		return this.m_Playable.IsPlaying(stateName);
	}

	public void Stop()
	{
		this.m_Playable.StopAll();
	}

	public void Stop(string stateName)
	{
		this.m_Playable.Stop(stateName);
	}

	public void Sample()
	{
		this.m_Graph.Evaluate();
	}

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

	public void AddState(AnimationClip clip, string name)
	{
		SimpleAnimation.LegacyClipCheck(clip);
		this.Kick();
		if (this.m_Playable.AddClip(clip, name))
		{
			this.RebuildStates();
		}
	}

	public void RemoveState(string name)
	{
		if (this.m_Playable.RemoveClip(name))
		{
			this.RebuildStates();
		}
	}

	public bool Play(string stateName)
	{
		this.m_Animator.enabled = true;
		this.Kick();
		return this.m_Playable.Play(stateName);
	}

	public void PlayQueued(string stateName, QueueMode queueMode)
	{
		this.m_Animator.enabled = true;
		this.Kick();
		this.m_Playable.PlayQueued(stateName, queueMode);
	}

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

	public void Rewind()
	{
		this.Kick();
		this.m_Playable.Rewind();
	}

	public void Rewind(string stateName)
	{
		this.Kick();
		this.m_Playable.Rewind(stateName);
	}

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

	public IEnumerable<SimpleAnimation.State> GetStates()
	{
		return new SimpleAnimation.StateEnumerable(this);
	}

	public SimpleAnimation.State this[string name]
	{
		get
		{
			return this.GetState(name);
		}
	}

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

	public void ExPlayAnimation(SimpleAnimation.ExPguiStatus uiType, SimpleAnimation.ExFinishCallback finishCb = null)
	{
		this.ExPlayAnimation(uiType.ToString(), finishCb);
	}

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

	public void ExPlayAnimationCrossFade(SimpleAnimation.ExPguiStatus uiType, float fadeLength = 0.2f, SimpleAnimation.ExFinishCallback finishCb = null)
	{
		this.ExPlayAnimationCrossFade(uiType.ToString(), fadeLength, finishCb);
	}

	public void ExPlayAnimationCrossFade(string stateName, float fadeLength = 0.2f, SimpleAnimation.ExFinishCallback finishCb = null)
	{
		this.Rewind();
		this.CrossFade(stateName, fadeLength);
		this.lastPlayStateName = stateName;
		this.currentExFinishCallback = finishCb;
	}

	public void ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus uiType)
	{
		this.ExPauseAnimationLastFrame(uiType.ToString());
	}

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

	public void ExPauseAnimation(SimpleAnimation.ExPguiStatus uiType, SimpleAnimation.ExFinishCallback finishCb = null)
	{
		this.ExPauseAnimation(uiType.ToString());
	}

	public void ExPauseAnimation(string stateName)
	{
		SimpleAnimation.State state = this.GetState(stateName);
		if (state != null)
		{
			this.ExPlayAnimation(stateName, null);
			state.speed = 0f;
		}
	}

	public void ExPauseAnimation()
	{
		SimpleAnimation.State state = this.GetState(this.lastPlayStateName);
		if (state != null)
		{
			state.speed = 0f;
		}
	}

	public void ExResumeAnimation(SimpleAnimation.ExFinishCallback finishCb = null)
	{
		SimpleAnimation.State state = this.GetState(this.lastPlayStateName);
		if (state != null)
		{
			state.speed = 1f;
		}
		this.currentExFinishCallback = finishCb;
	}

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

	public bool ExIsCurrent(SimpleAnimation.ExPguiStatus uiType)
	{
		return uiType.ToString() == this.lastPlayStateName;
	}

	public bool ExIsCurrent(string stateName)
	{
		return stateName == this.lastPlayStateName;
	}

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

	public string ExGetLastPlayStateName()
	{
		return this.lastPlayStateName;
	}

	public float ExGetSpeed()
	{
		SimpleAnimation.State state = this.GetState(this.lastPlayStateName);
		if (state == null)
		{
			return 0f;
		}
		return state.speed;
	}

	public float ExGetAbsTime()
	{
		SimpleAnimation.State state = this.GetState(this.lastPlayStateName);
		if (state == null)
		{
			return 0f;
		}
		return state.time;
	}

	public void ExStop(bool isStopInternal = true)
	{
		this.lastPlayStateName = "";
		if (isStopInternal)
		{
			this.Stop();
		}
	}

	public void ExInit()
	{
		this.Initialize();
	}

	private void Update()
	{
		this.UpdateExAnimation();
	}

	protected void Kick()
	{
		if (!this.m_IsPlaying)
		{
			this.m_Graph.Play();
			this.m_IsPlaying = true;
		}
	}

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

	protected virtual void OnDisable()
	{
		if (this.m_Initialized)
		{
			this.Stop();
			this.m_Graph.Stop();
		}
	}

	private void Reset()
	{
		if (this.m_Graph.IsValid())
		{
			this.m_Graph.Destroy();
		}
		this.m_Initialized = false;
	}

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

	private void EnsureDefaultStateExists()
	{
		if (this.m_Playable != null && this.m_Clip != null && this.m_Playable.GetState(this.m_Clip.name) == null)
		{
			this.m_Playable.AddClip(this.m_Clip, this.m_Clip.name);
			this.Kick();
		}
	}

	protected virtual void Awake()
	{
		this.Initialize();
	}

	protected void OnDestroy()
	{
		if (this.m_Graph.IsValid())
		{
			this.m_Graph.Destroy();
		}
	}

	private void OnPlayableDone()
	{
		this.m_Graph.Stop();
		this.m_IsPlaying = false;
	}

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

	private SimpleAnimation.EditorState CreateDefaultEditorState()
	{
		return new SimpleAnimation.EditorState
		{
			name = "Default",
			clip = this.m_Clip,
			defaultState = true
		};
	}

	private static void LegacyClipCheck(AnimationClip clip)
	{
		if (clip && clip.legacy)
		{
			throw new ArgumentException(string.Format("Legacy clip {0} cannot be used in this component. Set .legacy property to false before using this clip", clip));
		}
	}

	private void InvalidLegacyClipError(string clipName, string stateName)
	{
	}

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

	private string lastPlayStateName = "";

	private SimpleAnimation.ExFinishCallback currentExFinishCallback;

	private float frame;

	protected PlayableGraph m_Graph;

	protected PlayableHandle m_LayerMixer;

	protected PlayableHandle m_TransitionMixer;

	protected Animator m_Animator;

	protected bool m_Initialized;

	protected bool m_IsPlaying;

	protected SimpleAnimationPlayable m_Playable;

	[SerializeField]
	protected bool m_PlayAutomatically = true;

	[SerializeField]
	protected bool m_AnimatePhysics;

	[SerializeField]
	protected AnimatorCullingMode m_CullingMode = AnimatorCullingMode.CullUpdateTransforms;

	[SerializeField]
	protected WrapMode m_WrapMode;

	[SerializeField]
	protected AnimationClip m_Clip;

	[SerializeField]
	private SimpleAnimation.EditorState[] m_States;

	public interface State
	{
		bool enabled { get; set; }

		bool isValid { get; }

		float time { get; set; }

		float normalizedTime { get; set; }

		float speed { get; set; }

		string name { get; set; }

		float weight { get; set; }

		float length { get; }

		AnimationClip clip { get; }

		WrapMode wrapMode { get; set; }
	}

	public enum ExPguiStatus
	{
		START,
		START_SUB,
		LOOP,
		LOOP_SUB,
		END,
		END_SUB,
		MAX
	}

	public delegate void ExFinishCallback();

	private class StateEnumerable : IEnumerable<SimpleAnimation.State>, IEnumerable
	{
		public StateEnumerable(SimpleAnimation owner)
		{
			this.m_Owner = owner;
		}

		public IEnumerator<SimpleAnimation.State> GetEnumerator()
		{
			return new SimpleAnimation.StateEnumerable.StateEnumerator(this.m_Owner);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SimpleAnimation.StateEnumerable.StateEnumerator(this.m_Owner);
		}

		private SimpleAnimation m_Owner;

		private class StateEnumerator : IEnumerator<SimpleAnimation.State>, IEnumerator, IDisposable
		{
			public StateEnumerator(SimpleAnimation owner)
			{
				this.m_Owner = owner;
				this.m_Impl = this.m_Owner.m_Playable.GetStates().GetEnumerator();
				this.Reset();
			}

			private SimpleAnimation.State GetCurrent()
			{
				return new SimpleAnimation.StateImpl(this.m_Impl.Current, this.m_Owner);
			}

			object IEnumerator.Current
			{
				get
				{
					return this.GetCurrent();
				}
			}

			SimpleAnimation.State IEnumerator<SimpleAnimation.State>.Current
			{
				get
				{
					return this.GetCurrent();
				}
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				return this.m_Impl.MoveNext();
			}

			public void Reset()
			{
				this.m_Impl.Reset();
			}

			private SimpleAnimation m_Owner;

			private IEnumerator<SimpleAnimationPlayable.IState> m_Impl;
		}
	}

	private class StateImpl : SimpleAnimation.State
	{
		public StateImpl(SimpleAnimationPlayable.IState handle, SimpleAnimation component)
		{
			this.m_StateHandle = handle;
			this.m_Component = component;
		}

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

		bool SimpleAnimation.State.isValid
		{
			get
			{
				return this.m_StateHandle.IsValid();
			}
		}

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

		float SimpleAnimation.State.length
		{
			get
			{
				return this.m_StateHandle.length;
			}
		}

		AnimationClip SimpleAnimation.State.clip
		{
			get
			{
				return this.m_StateHandle.clip;
			}
		}

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

		private SimpleAnimationPlayable.IState m_StateHandle;

		private SimpleAnimation m_Component;
	}

	[Serializable]
	public class EditorState
	{
		public AnimationClip clip;

		public string name;

		public bool defaultState;
	}
}
