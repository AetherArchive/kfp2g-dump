using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class SimpleAnimationPlayable : PlayableBehaviour
{
	public bool keepStoppedPlayablesConnected
	{
		get
		{
			return this.m_KeepStoppedPlayablesConnected;
		}
		set
		{
			if (value != this.m_KeepStoppedPlayablesConnected)
			{
				this.m_KeepStoppedPlayablesConnected = value;
			}
		}
	}

	private void UpdateStoppedPlayablesConnections()
	{
		for (int i = 0; i < this.m_States.Count; i++)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[i];
			if (stateInfo != null && !stateInfo.enabled)
			{
				if (this.keepStoppedPlayablesConnected)
				{
					this.ConnectInput(stateInfo.index);
				}
				else
				{
					this.DisconnectInput(stateInfo.index);
				}
			}
		}
	}

	protected Playable self
	{
		get
		{
			return this.m_ActualPlayable;
		}
	}

	public Playable playable
	{
		get
		{
			return this.self;
		}
	}

	protected PlayableGraph graph
	{
		get
		{
			return this.self.GetGraph<Playable>();
		}
	}

	public SimpleAnimationPlayable()
	{
		this.m_States = new SimpleAnimationPlayable.StateManagement();
		this.m_StateQueue = new LinkedList<SimpleAnimationPlayable.QueuedState>();
	}

	public Playable GetInput(int index)
	{
		if (index >= this.m_Mixer.GetInputCount<AnimationMixerPlayable>())
		{
			return Playable.Null;
		}
		return this.m_Mixer.GetInput(index);
	}

	public override void OnPlayableCreate(Playable playable)
	{
		this.m_ActualPlayable = playable;
		AnimationMixerPlayable animationMixerPlayable = AnimationMixerPlayable.Create(this.graph, 1, true);
		this.m_Mixer = animationMixerPlayable;
		this.self.SetInputCount(1);
		this.self.SetInputWeight(0, 1f);
		this.graph.Connect<AnimationMixerPlayable, Playable>(this.m_Mixer, 0, this.self, 0);
	}

	public IEnumerable<SimpleAnimationPlayable.IState> GetStates()
	{
		return new SimpleAnimationPlayable.StateEnumerable(this);
	}

	public SimpleAnimationPlayable.IState GetState(string name)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States.FindState(name);
		if (stateInfo == null)
		{
			return null;
		}
		return new SimpleAnimationPlayable.StateHandle(this, stateInfo.index, stateInfo.playable);
	}

	private SimpleAnimationPlayable.StateInfo DoAddClip(string name, AnimationClip clip)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States.InsertState();
		stateInfo.stateName = name;
		stateInfo.clip = clip;
		stateInfo.wrapMode = clip.wrapMode;
		int index = stateInfo.index;
		if (index == this.m_Mixer.GetInputCount<AnimationMixerPlayable>())
		{
			this.m_Mixer.SetInputCount(index + 1);
		}
		stateInfo.playable = AnimationClipPlayable.Create(this.graph, clip);
		if (!clip.isLooping || stateInfo.wrapMode == WrapMode.Once)
		{
			stateInfo.playable.SetDuration((double)clip.length);
			stateInfo.playable.SetPlayState(PlayState.Paused);
		}
		if (this.keepStoppedPlayablesConnected)
		{
			this.ConnectInput(stateInfo.index);
		}
		return stateInfo;
	}

	public bool AddClip(AnimationClip clip, string name)
	{
		if (this.m_States.FindState(name) != null)
		{
			return false;
		}
		this.DoAddClip(name, clip);
		this.UpdateDoneStatus();
		this.InvalidateStates();
		return true;
	}

	public bool RemoveClip(string name)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States.FindState(name);
		if (stateInfo == null)
		{
			return false;
		}
		this.RemoveClones(stateInfo);
		this.InvalidateStates();
		this.m_States.RemoveAtIndex(stateInfo.index);
		return true;
	}

	public bool RemoveClip(AnimationClip clip)
	{
		this.InvalidateStates();
		return this.m_States.RemoveClip(clip);
	}

	public bool Play(string name)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States.FindState(name);
		return stateInfo != null && this.Play(stateInfo.index);
	}

	private bool Play(int index)
	{
		for (int i = 0; i < this.m_States.Count; i++)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[i];
			if (stateInfo != null && stateInfo.index == index)
			{
				this.m_States.EnableState(i);
				this.m_States.SetInputWeight(i, 1f);
			}
			else
			{
				this.DoStop(i);
			}
		}
		return true;
	}

	public bool PlayQueued(string name, QueueMode queueMode)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States.FindState(name);
		return stateInfo != null && this.PlayQueued(stateInfo.index, queueMode);
	}

	private bool PlayQueued(int index, QueueMode queueMode)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.CloneState(index);
		if (queueMode == QueueMode.PlayNow)
		{
			this.Play(stateInfo.index);
			return true;
		}
		this.m_StateQueue.AddLast(new SimpleAnimationPlayable.QueuedState(this.StateInfoToHandle(stateInfo), 0f));
		return true;
	}

	public void Rewind(string name)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States.FindState(name);
		if (stateInfo == null)
		{
			return;
		}
		this.Rewind(stateInfo.index);
	}

	private void Rewind(int index)
	{
		this.m_States.SetStateTime(index, 0f);
	}

	public void Rewind()
	{
		for (int i = 0; i < this.m_States.Count; i++)
		{
			if (this.m_States[i] != null)
			{
				this.m_States.SetStateTime(i, 0f);
			}
		}
	}

	private void RemoveClones(SimpleAnimationPlayable.StateInfo state)
	{
		LinkedListNode<SimpleAnimationPlayable.QueuedState> next;
		for (LinkedListNode<SimpleAnimationPlayable.QueuedState> linkedListNode = this.m_StateQueue.First; linkedListNode != null; linkedListNode = next)
		{
			next = linkedListNode.Next;
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[linkedListNode.Value.state.index];
			if (stateInfo.parentState.index == state.index)
			{
				this.m_StateQueue.Remove(linkedListNode);
				this.DoStop(stateInfo.index);
			}
		}
	}

	public bool Stop(string name)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States.FindState(name);
		if (stateInfo == null)
		{
			return false;
		}
		this.DoStop(stateInfo.index);
		this.UpdateDoneStatus();
		return true;
	}

	private void DoStop(int index)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
		if (stateInfo == null)
		{
			return;
		}
		this.m_States.StopState(index, stateInfo.isClone);
		if (!stateInfo.isClone)
		{
			this.RemoveClones(stateInfo);
		}
	}

	public bool StopAll()
	{
		for (int i = 0; i < this.m_States.Count; i++)
		{
			this.DoStop(i);
		}
		this.playable.SetDone(true);
		return true;
	}

	public bool IsPlaying()
	{
		return this.m_States.AnyStatePlaying();
	}

	public bool IsPlaying(string stateName)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States.FindState(stateName);
		return stateInfo != null && (stateInfo.enabled || this.IsClonePlaying(stateInfo));
	}

	private bool IsClonePlaying(SimpleAnimationPlayable.StateInfo state)
	{
		for (int i = 0; i < this.m_States.Count; i++)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[i];
			if (stateInfo.isClone && stateInfo.enabled && stateInfo.parentState.index == state.index)
			{
				return true;
			}
		}
		return false;
	}

	public int GetClipCount()
	{
		int num = 0;
		for (int i = 0; i < this.m_States.Count; i++)
		{
			if (this.m_States[i] != null)
			{
				num++;
			}
		}
		return num;
	}

	private void SetupLerp(SimpleAnimationPlayable.StateInfo state, float targetWeight, float time)
	{
		float num = Mathf.Abs(state.targetWeight - targetWeight);
		state.fading = num > 0f;
		state.fadeSpeed = ((time != 0f) ? (num / time) : float.PositiveInfinity);
		state.targetWeight = targetWeight;
	}

	private bool Crossfade(int index, float time)
	{
		for (int i = 0; i < this.m_States.Count; i++)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[i];
			if (stateInfo != null)
			{
				if (stateInfo.index == index)
				{
					this.m_States.EnableState(index);
				}
				if (stateInfo.enabled)
				{
					float num = ((stateInfo.index == index) ? 1f : 0f);
					this.SetupLerp(stateInfo, num, time);
				}
			}
		}
		return true;
	}

	private SimpleAnimationPlayable.StateInfo CloneState(int index)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
		string text = stateInfo.stateName + "Queued Clone";
		SimpleAnimationPlayable.StateInfo stateInfo2 = this.DoAddClip(text, stateInfo.clip);
		stateInfo2.parentState = new SimpleAnimationPlayable.StateHandle(this, stateInfo.index, stateInfo.playable);
		stateInfo2.isClone = true;
		return stateInfo2;
	}

	public bool Crossfade(string name, float time)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States.FindState(name);
		if (stateInfo == null)
		{
			return false;
		}
		if (time == 0f)
		{
			return this.Play(stateInfo.index);
		}
		return this.Crossfade(stateInfo.index, time);
	}

	public bool CrossfadeQueued(string name, float time, QueueMode queueMode)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States.FindState(name);
		return stateInfo != null && this.CrossfadeQueued(stateInfo.index, time, queueMode);
	}

	private bool CrossfadeQueued(int index, float time, QueueMode queueMode)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.CloneState(index);
		if (queueMode == QueueMode.PlayNow)
		{
			this.Crossfade(stateInfo.index, time);
			return true;
		}
		this.m_StateQueue.AddLast(new SimpleAnimationPlayable.QueuedState(this.StateInfoToHandle(stateInfo), time));
		return true;
	}

	private bool Blend(int index, float targetWeight, float time)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
		if (!stateInfo.enabled)
		{
			this.m_States.EnableState(index);
		}
		if (time == 0f)
		{
			stateInfo.weight = targetWeight;
		}
		else
		{
			this.SetupLerp(stateInfo, targetWeight, time);
		}
		return true;
	}

	public bool Blend(string name, float targetWeight, float time)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States.FindState(name);
		return stateInfo != null && this.Blend(stateInfo.index, targetWeight, time);
	}

	public override void OnGraphStop(Playable playable)
	{
		if (!this.self.IsValid<Playable>())
		{
			return;
		}
		for (int i = 0; i < this.m_States.Count; i++)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[i];
			if (stateInfo != null && stateInfo.fadeSpeed == 0f && stateInfo.targetWeight == 0f)
			{
				Playable input = this.m_Mixer.GetInput(stateInfo.index);
				if (!input.Equals(Playable.Null))
				{
					input.SetTime(0.0);
					input.SetTime(0.0);
				}
			}
		}
	}

	private void UpdateDoneStatus()
	{
		if (!this.m_States.AnyStatePlaying())
		{
			bool flag = this.playable.IsDone<Playable>();
			this.playable.SetDone(true);
			if (!flag && this.onDone != null)
			{
				this.onDone();
			}
		}
	}

	private void DisconnectInput(int index)
	{
		if (this.keepStoppedPlayablesConnected)
		{
			this.m_States[index].playable.SetPlayState(PlayState.Paused);
		}
		this.graph.Disconnect<AnimationMixerPlayable>(this.m_Mixer, index);
	}

	private void ConnectInput(int index)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
		this.graph.Connect<Playable, AnimationMixerPlayable>(stateInfo.playable, 0, this.m_Mixer, stateInfo.index);
	}

	private void UpdateStates(float deltaTime)
	{
		for (int i = 0; i < this.m_States.Count; i++)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[i];
			if (stateInfo != null)
			{
				if (stateInfo.fading)
				{
					stateInfo.weight = Mathf.MoveTowards(stateInfo.weight, stateInfo.targetWeight, stateInfo.fadeSpeed * deltaTime);
					if (stateInfo.weight == stateInfo.targetWeight)
					{
						stateInfo.fadeSpeed = 0f;
						stateInfo.fading = false;
					}
					stateInfo.weightDirty = true;
				}
				if (stateInfo.enabledDirty)
				{
					stateInfo.playable.SetPlayState(stateInfo.enabled ? PlayState.Playing : PlayState.Paused);
					if (!this.keepStoppedPlayablesConnected)
					{
						Playable input = this.m_Mixer.GetInput(i);
						if (input.IsValid<Playable>() && !stateInfo.enabled)
						{
							this.DisconnectInput(i);
						}
						else if (stateInfo.enabled && !input.IsValid<Playable>())
						{
							this.ConnectInput(stateInfo.index);
						}
					}
					stateInfo.weightDirty = true;
					stateInfo.enabledDirty = false;
				}
				if (stateInfo.enabled && stateInfo.wrapMode == WrapMode.Once)
				{
					bool flag = stateInfo.playable.IsDone<Playable>();
					float stateSpeed = this.m_States.GetStateSpeed(stateInfo.index);
					float stateTime = this.m_States.GetStateTime(stateInfo.index);
					float statePlayableDuration = this.m_States.GetStatePlayableDuration(stateInfo.index);
					if (flag | (stateSpeed < 0f && stateTime < 0f) | (stateSpeed >= 0f && stateTime >= statePlayableDuration))
					{
						stateInfo.weight = 0f;
						stateInfo.time = 0f;
						stateInfo.playable.SetTime((double)stateInfo.time);
						stateInfo.playable.SetTime((double)stateInfo.time);
						this.m_States.DisableState(i);
						if (!this.keepStoppedPlayablesConnected)
						{
							this.DisconnectInput(stateInfo.index);
						}
						stateInfo.weightDirty = true;
					}
				}
				if (stateInfo.weightDirty)
				{
					this.m_Mixer.SetInputWeight(stateInfo.index, stateInfo.weight);
					stateInfo.weightDirty = false;
				}
			}
		}
	}

	private float CalculateQueueTimes()
	{
		float num = -1f;
		for (int i = 0; i < this.m_States.Count; i++)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[i];
			if (stateInfo != null && stateInfo.enabled && stateInfo.playable.IsValid<Playable>())
			{
				if (stateInfo.wrapMode == WrapMode.Loop)
				{
					return float.PositiveInfinity;
				}
				float num2 = (float)stateInfo.playable.GetSpeed<Playable>();
				float stateTime = this.m_States.GetStateTime(stateInfo.index);
				float num3;
				if (num2 > 0f)
				{
					num3 = (stateInfo.clip.length - stateTime) / num2;
				}
				else if (num2 < 0f)
				{
					num3 = stateTime / num2;
				}
				else
				{
					num3 = float.PositiveInfinity;
				}
				if (num3 > num)
				{
					num = num3;
				}
			}
		}
		return num;
	}

	private void ClearQueuedStates()
	{
		foreach (SimpleAnimationPlayable.QueuedState queuedState in this.m_StateQueue)
		{
			this.m_States.StopState(queuedState.state.index, true);
		}
		this.m_StateQueue.Clear();
	}

	private void UpdateQueuedStates()
	{
		bool flag = true;
		float num = -1f;
		for (LinkedListNode<SimpleAnimationPlayable.QueuedState> linkedListNode = this.m_StateQueue.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			if (flag)
			{
				num = this.CalculateQueueTimes();
				flag = false;
			}
			SimpleAnimationPlayable.QueuedState value = linkedListNode.Value;
			if (value.fadeTime >= num)
			{
				this.Crossfade(value.state.index, value.fadeTime);
				flag = true;
			}
		}
	}

	private void UpdateStateTimes()
	{
		int count = this.m_States.Count;
		for (int i = 0; i < count; i++)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[i];
			if (stateInfo != null)
			{
				stateInfo.timeIsUpToDate = false;
			}
		}
	}

	public override void PrepareFrame(Playable owner, FrameData data)
	{
		this.UpdateStateTimes();
		this.UpdateQueuedStates();
		this.UpdateStates(data.deltaTime);
		this.UpdateDoneStatus();
	}

	public bool ValidateInput(int index, Playable input)
	{
		if (!this.ValidateIndex(index))
		{
			return false;
		}
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
		return stateInfo != null && stateInfo.playable.IsValid<Playable>() && !(stateInfo.playable.GetHandle() != input.GetHandle());
	}

	public bool ValidateIndex(int index)
	{
		return index >= 0 && index < this.m_States.Count;
	}

	private void SetInputWeight(int index, float weight)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
		stateInfo.targetWeight = weight;
		stateInfo.weight = weight;
		stateInfo.fading = false;
	}

	private float GetInputWeight(int index)
	{
		return this.m_States[index].weight;
	}

	private void InvalidateStates()
	{
		this.m_StatesVersion++;
	}

	private SimpleAnimationPlayable.StateHandle StateInfoToHandle(SimpleAnimationPlayable.StateInfo info)
	{
		return new SimpleAnimationPlayable.StateHandle(this, info.index, info.playable);
	}

	private LinkedList<SimpleAnimationPlayable.QueuedState> m_StateQueue;

	private SimpleAnimationPlayable.StateManagement m_States;

	private bool m_Initialized;

	private bool m_KeepStoppedPlayablesConnected = true;

	protected Playable m_ActualPlayable;

	private AnimationMixerPlayable m_Mixer;

	public Action onDone;

	private int m_StatesVersion;

	private class StateEnumerable : IEnumerable<SimpleAnimationPlayable.IState>, IEnumerable
	{
		public StateEnumerable(SimpleAnimationPlayable owner)
		{
			this.m_Owner = owner;
		}

		public IEnumerator<SimpleAnimationPlayable.IState> GetEnumerator()
		{
			return new SimpleAnimationPlayable.StateEnumerable.StateEnumerator(this.m_Owner);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SimpleAnimationPlayable.StateEnumerable.StateEnumerator(this.m_Owner);
		}

		private SimpleAnimationPlayable m_Owner;

		private class StateEnumerator : IEnumerator<SimpleAnimationPlayable.IState>, IEnumerator, IDisposable
		{
			public StateEnumerator(SimpleAnimationPlayable owner)
			{
				this.m_Owner = owner;
				this.m_Version = this.m_Owner.m_StatesVersion;
				this.Reset();
			}

			private bool IsValid()
			{
				return this.m_Owner != null && this.m_Version == this.m_Owner.m_StatesVersion;
			}

			private SimpleAnimationPlayable.IState GetCurrentHandle(int index)
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("The collection has been modified, this Enumerator is invalid");
				}
				if (index < 0 || index >= this.m_Owner.m_States.Count)
				{
					throw new InvalidOperationException("Enumerator is invalid");
				}
				SimpleAnimationPlayable.StateInfo stateInfo = this.m_Owner.m_States[index];
				if (stateInfo == null)
				{
					throw new InvalidOperationException("Enumerator is invalid");
				}
				return new SimpleAnimationPlayable.StateHandle(this.m_Owner, stateInfo.index, stateInfo.playable);
			}

			object IEnumerator.Current
			{
				get
				{
					return this.GetCurrentHandle(this.m_Index);
				}
			}

			SimpleAnimationPlayable.IState IEnumerator<SimpleAnimationPlayable.IState>.Current
			{
				get
				{
					return this.GetCurrentHandle(this.m_Index);
				}
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("The collection has been modified, this Enumerator is invalid");
				}
				do
				{
					this.m_Index++;
				}
				while (this.m_Index < this.m_Owner.m_States.Count && this.m_Owner.m_States[this.m_Index] == null);
				return this.m_Index < this.m_Owner.m_States.Count;
			}

			public void Reset()
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("The collection has been modified, this Enumerator is invalid");
				}
				this.m_Index = -1;
			}

			private int m_Index = -1;

			private int m_Version;

			private SimpleAnimationPlayable m_Owner;
		}
	}

	public interface IState
	{
		bool IsValid();

		bool enabled { get; set; }

		float time { get; set; }

		float normalizedTime { get; set; }

		float speed { get; set; }

		string name { get; set; }

		float weight { get; set; }

		float length { get; }

		AnimationClip clip { get; }

		WrapMode wrapMode { get; }
	}

	public class StateHandle : SimpleAnimationPlayable.IState
	{
		public StateHandle(SimpleAnimationPlayable s, int index, Playable target)
		{
			this.m_Parent = s;
			this.m_Index = index;
			this.m_Target = target;
		}

		public bool IsValid()
		{
			return this.m_Parent.ValidateInput(this.m_Index, this.m_Target);
		}

		public bool enabled
		{
			get
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				return this.m_Parent.m_States[this.m_Index].enabled;
			}
			set
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				if (value)
				{
					this.m_Parent.m_States.EnableState(this.m_Index);
					return;
				}
				this.m_Parent.m_States.DisableState(this.m_Index);
			}
		}

		public float time
		{
			get
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				return this.m_Parent.m_States.GetStateTime(this.m_Index);
			}
			set
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				this.m_Parent.m_States.SetStateTime(this.m_Index, value);
			}
		}

		public float normalizedTime
		{
			get
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				float num = this.m_Parent.m_States.GetStateLength(this.m_Index);
				if (num == 0f)
				{
					num = 1f;
				}
				return this.m_Parent.m_States.GetStateTime(this.m_Index) / num;
			}
			set
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				float num = this.m_Parent.m_States.GetStateLength(this.m_Index);
				if (num == 0f)
				{
					num = 1f;
				}
				this.m_Parent.m_States.SetStateTime(this.m_Index, value *= num);
			}
		}

		public float speed
		{
			get
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				return this.m_Parent.m_States.GetStateSpeed(this.m_Index);
			}
			set
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				this.m_Parent.m_States.SetStateSpeed(this.m_Index, value);
			}
		}

		public string name
		{
			get
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				return this.m_Parent.m_States.GetStateName(this.m_Index);
			}
			set
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				if (value == null)
				{
					throw new ArgumentNullException("A null string is not a valid name");
				}
				this.m_Parent.m_States.SetStateName(this.m_Index, value);
			}
		}

		public float weight
		{
			get
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				return this.m_Parent.m_States[this.m_Index].weight;
			}
			set
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				if (value < 0f)
				{
					throw new ArgumentException("Weights cannot be negative");
				}
				this.m_Parent.m_States[this.m_Index].weight = value;
			}
		}

		public float length
		{
			get
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				return this.m_Parent.m_States.GetStateLength(this.m_Index);
			}
		}

		public AnimationClip clip
		{
			get
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				return this.m_Parent.m_States.GetStateClip(this.m_Index);
			}
		}

		public WrapMode wrapMode
		{
			get
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("This StateHandle is not valid");
				}
				return this.m_Parent.m_States.GetStateWrapMode(this.m_Index);
			}
		}

		public int index
		{
			get
			{
				return this.m_Index;
			}
		}

		private SimpleAnimationPlayable m_Parent;

		private int m_Index;

		private Playable m_Target;

		private SimpleAnimationPlayable.StateInfo state;
	}

	private class StateInfo
	{
		public bool enabled;

		public int index;

		public string stateName;

		public bool fading;

		public float time;

		public float targetWeight;

		public float weight;

		public float fadeSpeed;

		public float speed;

		public AnimationClip clip;

		public Playable playable;

		public WrapMode wrapMode;

		public bool isClone;

		public SimpleAnimationPlayable.StateHandle parentState;

		public bool weightDirty;

		public bool enabledDirty;

		public bool timeIsUpToDate;
	}

	private class StateManagement
	{
		public int Count
		{
			get
			{
				return this.m_Count;
			}
		}

		public SimpleAnimationPlayable.StateInfo this[int i]
		{
			get
			{
				return this.m_States[i];
			}
		}

		public StateManagement()
		{
			this.m_States = new List<SimpleAnimationPlayable.StateInfo>();
		}

		public SimpleAnimationPlayable.StateInfo InsertState()
		{
			SimpleAnimationPlayable.StateInfo stateInfo = new SimpleAnimationPlayable.StateInfo();
			int num = this.m_States.FindIndex((SimpleAnimationPlayable.StateInfo s) => s == null);
			if (num == -1)
			{
				num = this.m_States.Count;
				this.m_States.Add(stateInfo);
			}
			else
			{
				this.m_States.Insert(num, stateInfo);
			}
			stateInfo.index = num;
			this.m_Count++;
			return stateInfo;
		}

		public bool AnyStatePlaying()
		{
			return this.m_States.FindIndex((SimpleAnimationPlayable.StateInfo s) => s != null && s.enabled) != -1;
		}

		public bool IsStateDone(int index)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
			return stateInfo == null || !stateInfo.enabled;
		}

		public void RemoveAtIndex(int index)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
			this.m_States[index] = null;
			if (stateInfo.playable.IsValid<Playable>())
			{
				stateInfo.playable.GetGraph<Playable>().DestroyPlayable<Playable>(stateInfo.playable);
			}
			this.m_Count = this.m_States.Count;
		}

		public bool RemoveClip(AnimationClip clip)
		{
			bool flag = false;
			for (int i = 0; i < this.m_States.Count; i++)
			{
				SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[i];
				if (stateInfo != null && stateInfo.clip == clip)
				{
					this.RemoveAtIndex(i);
					flag = true;
				}
			}
			return flag;
		}

		public SimpleAnimationPlayable.StateInfo FindState(string name)
		{
			int num = this.m_States.FindIndex((SimpleAnimationPlayable.StateInfo s) => s != null && s.stateName == name);
			if (num == -1)
			{
				return null;
			}
			return this.m_States[num];
		}

		public void EnableState(int index)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
			if (stateInfo.enabled)
			{
				return;
			}
			stateInfo.enabledDirty = true;
			stateInfo.enabled = true;
		}

		public void DisableState(int index)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
			if (!stateInfo.enabled)
			{
				return;
			}
			stateInfo.enabledDirty = true;
			stateInfo.enabled = false;
		}

		public void SetInputWeight(int index, float weight)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
			stateInfo.targetWeight = weight;
			stateInfo.weight = weight;
			stateInfo.fading = false;
			stateInfo.weightDirty = true;
		}

		public void SetStateTime(int index, float time)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
			stateInfo.time = time;
			stateInfo.playable.SetTime((double)time);
			stateInfo.playable.SetDone((double)time >= stateInfo.playable.GetDuration<Playable>());
		}

		public bool IsCloneOf(int potentialCloneIndex, int originalIndex)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[potentialCloneIndex];
			return stateInfo.isClone && stateInfo.parentState.index == originalIndex;
		}

		public float GetStateTime(int index)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
			if (stateInfo.timeIsUpToDate)
			{
				return stateInfo.time;
			}
			stateInfo.time = (float)stateInfo.playable.GetTime<Playable>();
			stateInfo.timeIsUpToDate = true;
			return stateInfo.time;
		}

		public float GetStateSpeed(int index)
		{
			return (float)this.m_States[index].playable.GetSpeed<Playable>();
		}

		public void SetStateSpeed(int index, float value)
		{
			this.m_States[index].playable.SetSpeed((double)value);
		}

		public float GetInputWeight(int index)
		{
			return this.m_States[index].weight;
		}

		public float GetStateLength(int index)
		{
			AnimationClip clip = this.m_States[index].clip;
			if (clip == null)
			{
				return 0f;
			}
			float num = (float)this.m_States[index].playable.GetSpeed<Playable>();
			if (this.m_States[index].playable.GetSpeed<Playable>() == 0.0)
			{
				return float.PositiveInfinity;
			}
			return clip.length / num;
		}

		public float GetStatePlayableDuration(int index)
		{
			Playable playable = this.m_States[index].playable;
			if (!playable.IsValid<Playable>())
			{
				return 0f;
			}
			return (float)playable.GetDuration<Playable>();
		}

		public AnimationClip GetStateClip(int index)
		{
			AnimationClip clip = this.m_States[index].clip;
			if (clip == null)
			{
				return null;
			}
			return clip;
		}

		public WrapMode GetStateWrapMode(int index)
		{
			return this.m_States[index].wrapMode;
		}

		public string GetStateName(int index)
		{
			return this.m_States[index].stateName;
		}

		public void SetStateName(int index, string name)
		{
			this.m_States[index].stateName = name;
		}

		public void StopState(int index, bool cleanup)
		{
			if (cleanup)
			{
				this.RemoveAtIndex(index);
				return;
			}
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
			stateInfo.fadeSpeed = 0f;
			stateInfo.weight = 0f;
			stateInfo.targetWeight = 0f;
			stateInfo.time = 0f;
			stateInfo.enabled = false;
			stateInfo.enabledDirty = true;
			stateInfo.weightDirty = true;
			stateInfo.playable.SetTime(0.0);
			stateInfo.playable.SetDone(false);
		}

		private List<SimpleAnimationPlayable.StateInfo> m_States;

		private int m_Count;
	}

	private class QueuedState
	{
		public QueuedState(SimpleAnimationPlayable.StateHandle s, float t)
		{
			this.state = s;
			this.fadeTime = t;
		}

		public SimpleAnimationPlayable.StateHandle state;

		public float fadeTime;
	}
}
