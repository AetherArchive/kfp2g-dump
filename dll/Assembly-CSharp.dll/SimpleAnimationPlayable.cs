using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

// Token: 0x020001EA RID: 490
public class SimpleAnimationPlayable : PlayableBehaviour
{
	// Token: 0x17000460 RID: 1120
	// (get) Token: 0x060020DB RID: 8411 RVA: 0x0018CBE5 File Offset: 0x0018ADE5
	// (set) Token: 0x060020DC RID: 8412 RVA: 0x0018CBED File Offset: 0x0018ADED
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

	// Token: 0x060020DD RID: 8413 RVA: 0x0018CC00 File Offset: 0x0018AE00
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

	// Token: 0x17000461 RID: 1121
	// (get) Token: 0x060020DE RID: 8414 RVA: 0x0018CC5D File Offset: 0x0018AE5D
	protected Playable self
	{
		get
		{
			return this.m_ActualPlayable;
		}
	}

	// Token: 0x17000462 RID: 1122
	// (get) Token: 0x060020DF RID: 8415 RVA: 0x0018CC65 File Offset: 0x0018AE65
	public Playable playable
	{
		get
		{
			return this.self;
		}
	}

	// Token: 0x17000463 RID: 1123
	// (get) Token: 0x060020E0 RID: 8416 RVA: 0x0018CC6D File Offset: 0x0018AE6D
	protected PlayableGraph graph
	{
		get
		{
			return this.self.GetGraph<Playable>();
		}
	}

	// Token: 0x060020E1 RID: 8417 RVA: 0x0018CC7A File Offset: 0x0018AE7A
	public SimpleAnimationPlayable()
	{
		this.m_States = new SimpleAnimationPlayable.StateManagement();
		this.m_StateQueue = new LinkedList<SimpleAnimationPlayable.QueuedState>();
	}

	// Token: 0x060020E2 RID: 8418 RVA: 0x0018CC9F File Offset: 0x0018AE9F
	public Playable GetInput(int index)
	{
		if (index >= this.m_Mixer.GetInputCount<AnimationMixerPlayable>())
		{
			return Playable.Null;
		}
		return this.m_Mixer.GetInput(index);
	}

	// Token: 0x060020E3 RID: 8419 RVA: 0x0018CCC4 File Offset: 0x0018AEC4
	public override void OnPlayableCreate(Playable playable)
	{
		this.m_ActualPlayable = playable;
		AnimationMixerPlayable animationMixerPlayable = AnimationMixerPlayable.Create(this.graph, 1, true);
		this.m_Mixer = animationMixerPlayable;
		this.self.SetInputCount(1);
		this.self.SetInputWeight(0, 1f);
		this.graph.Connect<AnimationMixerPlayable, Playable>(this.m_Mixer, 0, this.self, 0);
	}

	// Token: 0x060020E4 RID: 8420 RVA: 0x0018CD27 File Offset: 0x0018AF27
	public IEnumerable<SimpleAnimationPlayable.IState> GetStates()
	{
		return new SimpleAnimationPlayable.StateEnumerable(this);
	}

	// Token: 0x060020E5 RID: 8421 RVA: 0x0018CD30 File Offset: 0x0018AF30
	public SimpleAnimationPlayable.IState GetState(string name)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States.FindState(name);
		if (stateInfo == null)
		{
			return null;
		}
		return new SimpleAnimationPlayable.StateHandle(this, stateInfo.index, stateInfo.playable);
	}

	// Token: 0x060020E6 RID: 8422 RVA: 0x0018CD64 File Offset: 0x0018AF64
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

	// Token: 0x060020E7 RID: 8423 RVA: 0x0018CE15 File Offset: 0x0018B015
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

	// Token: 0x060020E8 RID: 8424 RVA: 0x0018CE40 File Offset: 0x0018B040
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

	// Token: 0x060020E9 RID: 8425 RVA: 0x0018CE7E File Offset: 0x0018B07E
	public bool RemoveClip(AnimationClip clip)
	{
		this.InvalidateStates();
		return this.m_States.RemoveClip(clip);
	}

	// Token: 0x060020EA RID: 8426 RVA: 0x0018CE94 File Offset: 0x0018B094
	public bool Play(string name)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States.FindState(name);
		return stateInfo != null && this.Play(stateInfo.index);
	}

	// Token: 0x060020EB RID: 8427 RVA: 0x0018CEC0 File Offset: 0x0018B0C0
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

	// Token: 0x060020EC RID: 8428 RVA: 0x0018CF24 File Offset: 0x0018B124
	public bool PlayQueued(string name, QueueMode queueMode)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States.FindState(name);
		return stateInfo != null && this.PlayQueued(stateInfo.index, queueMode);
	}

	// Token: 0x060020ED RID: 8429 RVA: 0x0018CF50 File Offset: 0x0018B150
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

	// Token: 0x060020EE RID: 8430 RVA: 0x0018CF98 File Offset: 0x0018B198
	public void Rewind(string name)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States.FindState(name);
		if (stateInfo == null)
		{
			return;
		}
		this.Rewind(stateInfo.index);
	}

	// Token: 0x060020EF RID: 8431 RVA: 0x0018CFC2 File Offset: 0x0018B1C2
	private void Rewind(int index)
	{
		this.m_States.SetStateTime(index, 0f);
	}

	// Token: 0x060020F0 RID: 8432 RVA: 0x0018CFD8 File Offset: 0x0018B1D8
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

	// Token: 0x060020F1 RID: 8433 RVA: 0x0018D01C File Offset: 0x0018B21C
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

	// Token: 0x060020F2 RID: 8434 RVA: 0x0018D088 File Offset: 0x0018B288
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

	// Token: 0x060020F3 RID: 8435 RVA: 0x0018D0BC File Offset: 0x0018B2BC
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

	// Token: 0x060020F4 RID: 8436 RVA: 0x0018D0FC File Offset: 0x0018B2FC
	public bool StopAll()
	{
		for (int i = 0; i < this.m_States.Count; i++)
		{
			this.DoStop(i);
		}
		this.playable.SetDone(true);
		return true;
	}

	// Token: 0x060020F5 RID: 8437 RVA: 0x0018D133 File Offset: 0x0018B333
	public bool IsPlaying()
	{
		return this.m_States.AnyStatePlaying();
	}

	// Token: 0x060020F6 RID: 8438 RVA: 0x0018D140 File Offset: 0x0018B340
	public bool IsPlaying(string stateName)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States.FindState(stateName);
		return stateInfo != null && (stateInfo.enabled || this.IsClonePlaying(stateInfo));
	}

	// Token: 0x060020F7 RID: 8439 RVA: 0x0018D170 File Offset: 0x0018B370
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

	// Token: 0x060020F8 RID: 8440 RVA: 0x0018D1C8 File Offset: 0x0018B3C8
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

	// Token: 0x060020F9 RID: 8441 RVA: 0x0018D200 File Offset: 0x0018B400
	private void SetupLerp(SimpleAnimationPlayable.StateInfo state, float targetWeight, float time)
	{
		float num = Mathf.Abs(state.targetWeight - targetWeight);
		state.fading = num > 0f;
		state.fadeSpeed = ((time != 0f) ? (num / time) : float.PositiveInfinity);
		state.targetWeight = targetWeight;
	}

	// Token: 0x060020FA RID: 8442 RVA: 0x0018D248 File Offset: 0x0018B448
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

	// Token: 0x060020FB RID: 8443 RVA: 0x0018D2B8 File Offset: 0x0018B4B8
	private SimpleAnimationPlayable.StateInfo CloneState(int index)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
		string text = stateInfo.stateName + "Queued Clone";
		SimpleAnimationPlayable.StateInfo stateInfo2 = this.DoAddClip(text, stateInfo.clip);
		stateInfo2.parentState = new SimpleAnimationPlayable.StateHandle(this, stateInfo.index, stateInfo.playable);
		stateInfo2.isClone = true;
		return stateInfo2;
	}

	// Token: 0x060020FC RID: 8444 RVA: 0x0018D310 File Offset: 0x0018B510
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

	// Token: 0x060020FD RID: 8445 RVA: 0x0018D354 File Offset: 0x0018B554
	public bool CrossfadeQueued(string name, float time, QueueMode queueMode)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States.FindState(name);
		return stateInfo != null && this.CrossfadeQueued(stateInfo.index, time, queueMode);
	}

	// Token: 0x060020FE RID: 8446 RVA: 0x0018D384 File Offset: 0x0018B584
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

	// Token: 0x060020FF RID: 8447 RVA: 0x0018D3C8 File Offset: 0x0018B5C8
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

	// Token: 0x06002100 RID: 8448 RVA: 0x0018D414 File Offset: 0x0018B614
	public bool Blend(string name, float targetWeight, float time)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States.FindState(name);
		return stateInfo != null && this.Blend(stateInfo.index, targetWeight, time);
	}

	// Token: 0x06002101 RID: 8449 RVA: 0x0018D444 File Offset: 0x0018B644
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

	// Token: 0x06002102 RID: 8450 RVA: 0x0018D4DD File Offset: 0x0018B6DD
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

	// Token: 0x06002103 RID: 8451 RVA: 0x0018D518 File Offset: 0x0018B718
	private void DisconnectInput(int index)
	{
		if (this.keepStoppedPlayablesConnected)
		{
			this.m_States[index].playable.SetPlayState(PlayState.Paused);
		}
		this.graph.Disconnect<AnimationMixerPlayable>(this.m_Mixer, index);
	}

	// Token: 0x06002104 RID: 8452 RVA: 0x0018D55C File Offset: 0x0018B75C
	private void ConnectInput(int index)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
		this.graph.Connect<Playable, AnimationMixerPlayable>(stateInfo.playable, 0, this.m_Mixer, stateInfo.index);
	}

	// Token: 0x06002105 RID: 8453 RVA: 0x0018D598 File Offset: 0x0018B798
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

	// Token: 0x06002106 RID: 8454 RVA: 0x0018D7A8 File Offset: 0x0018B9A8
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

	// Token: 0x06002107 RID: 8455 RVA: 0x0018D868 File Offset: 0x0018BA68
	private void ClearQueuedStates()
	{
		foreach (SimpleAnimationPlayable.QueuedState queuedState in this.m_StateQueue)
		{
			this.m_States.StopState(queuedState.state.index, true);
		}
		this.m_StateQueue.Clear();
	}

	// Token: 0x06002108 RID: 8456 RVA: 0x0018D8BC File Offset: 0x0018BABC
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

	// Token: 0x06002109 RID: 8457 RVA: 0x0018D920 File Offset: 0x0018BB20
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

	// Token: 0x0600210A RID: 8458 RVA: 0x0018D95C File Offset: 0x0018BB5C
	public override void PrepareFrame(Playable owner, FrameData data)
	{
		this.UpdateStateTimes();
		this.UpdateQueuedStates();
		this.UpdateStates(data.deltaTime);
		this.UpdateDoneStatus();
	}

	// Token: 0x0600210B RID: 8459 RVA: 0x0018D980 File Offset: 0x0018BB80
	public bool ValidateInput(int index, Playable input)
	{
		if (!this.ValidateIndex(index))
		{
			return false;
		}
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
		return stateInfo != null && stateInfo.playable.IsValid<Playable>() && !(stateInfo.playable.GetHandle() != input.GetHandle());
	}

	// Token: 0x0600210C RID: 8460 RVA: 0x0018D9D1 File Offset: 0x0018BBD1
	public bool ValidateIndex(int index)
	{
		return index >= 0 && index < this.m_States.Count;
	}

	// Token: 0x0600210D RID: 8461 RVA: 0x0018D9E7 File Offset: 0x0018BBE7
	private void SetInputWeight(int index, float weight)
	{
		SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
		stateInfo.targetWeight = weight;
		stateInfo.weight = weight;
		stateInfo.fading = false;
	}

	// Token: 0x0600210E RID: 8462 RVA: 0x0018DA09 File Offset: 0x0018BC09
	private float GetInputWeight(int index)
	{
		return this.m_States[index].weight;
	}

	// Token: 0x0600210F RID: 8463 RVA: 0x0018DA1C File Offset: 0x0018BC1C
	private void InvalidateStates()
	{
		this.m_StatesVersion++;
	}

	// Token: 0x06002110 RID: 8464 RVA: 0x0018DA2C File Offset: 0x0018BC2C
	private SimpleAnimationPlayable.StateHandle StateInfoToHandle(SimpleAnimationPlayable.StateInfo info)
	{
		return new SimpleAnimationPlayable.StateHandle(this, info.index, info.playable);
	}

	// Token: 0x040017BD RID: 6077
	private LinkedList<SimpleAnimationPlayable.QueuedState> m_StateQueue;

	// Token: 0x040017BE RID: 6078
	private SimpleAnimationPlayable.StateManagement m_States;

	// Token: 0x040017BF RID: 6079
	private bool m_Initialized;

	// Token: 0x040017C0 RID: 6080
	private bool m_KeepStoppedPlayablesConnected = true;

	// Token: 0x040017C1 RID: 6081
	protected Playable m_ActualPlayable;

	// Token: 0x040017C2 RID: 6082
	private AnimationMixerPlayable m_Mixer;

	// Token: 0x040017C3 RID: 6083
	public Action onDone;

	// Token: 0x040017C4 RID: 6084
	private int m_StatesVersion;

	// Token: 0x02001036 RID: 4150
	private class StateEnumerable : IEnumerable<SimpleAnimationPlayable.IState>, IEnumerable
	{
		// Token: 0x06005244 RID: 21060 RVA: 0x002486A6 File Offset: 0x002468A6
		public StateEnumerable(SimpleAnimationPlayable owner)
		{
			this.m_Owner = owner;
		}

		// Token: 0x06005245 RID: 21061 RVA: 0x002486B5 File Offset: 0x002468B5
		public IEnumerator<SimpleAnimationPlayable.IState> GetEnumerator()
		{
			return new SimpleAnimationPlayable.StateEnumerable.StateEnumerator(this.m_Owner);
		}

		// Token: 0x06005246 RID: 21062 RVA: 0x002486C2 File Offset: 0x002468C2
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SimpleAnimationPlayable.StateEnumerable.StateEnumerator(this.m_Owner);
		}

		// Token: 0x04005AE0 RID: 23264
		private SimpleAnimationPlayable m_Owner;

		// Token: 0x02001229 RID: 4649
		private class StateEnumerator : IEnumerator<SimpleAnimationPlayable.IState>, IEnumerator, IDisposable
		{
			// Token: 0x060057F8 RID: 22520 RVA: 0x00259FBB File Offset: 0x002581BB
			public StateEnumerator(SimpleAnimationPlayable owner)
			{
				this.m_Owner = owner;
				this.m_Version = this.m_Owner.m_StatesVersion;
				this.Reset();
			}

			// Token: 0x060057F9 RID: 22521 RVA: 0x00259FE8 File Offset: 0x002581E8
			private bool IsValid()
			{
				return this.m_Owner != null && this.m_Version == this.m_Owner.m_StatesVersion;
			}

			// Token: 0x060057FA RID: 22522 RVA: 0x0025A008 File Offset: 0x00258208
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

			// Token: 0x17000D0B RID: 3339
			// (get) Token: 0x060057FB RID: 22523 RVA: 0x0025A081 File Offset: 0x00258281
			object IEnumerator.Current
			{
				get
				{
					return this.GetCurrentHandle(this.m_Index);
				}
			}

			// Token: 0x17000D0C RID: 3340
			// (get) Token: 0x060057FC RID: 22524 RVA: 0x0025A08F File Offset: 0x0025828F
			SimpleAnimationPlayable.IState IEnumerator<SimpleAnimationPlayable.IState>.Current
			{
				get
				{
					return this.GetCurrentHandle(this.m_Index);
				}
			}

			// Token: 0x060057FD RID: 22525 RVA: 0x0025A09D File Offset: 0x0025829D
			public void Dispose()
			{
			}

			// Token: 0x060057FE RID: 22526 RVA: 0x0025A0A0 File Offset: 0x002582A0
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

			// Token: 0x060057FF RID: 22527 RVA: 0x0025A116 File Offset: 0x00258316
			public void Reset()
			{
				if (!this.IsValid())
				{
					throw new InvalidOperationException("The collection has been modified, this Enumerator is invalid");
				}
				this.m_Index = -1;
			}

			// Token: 0x04006375 RID: 25461
			private int m_Index = -1;

			// Token: 0x04006376 RID: 25462
			private int m_Version;

			// Token: 0x04006377 RID: 25463
			private SimpleAnimationPlayable m_Owner;
		}
	}

	// Token: 0x02001037 RID: 4151
	public interface IState
	{
		// Token: 0x06005247 RID: 21063
		bool IsValid();

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x06005248 RID: 21064
		// (set) Token: 0x06005249 RID: 21065
		bool enabled { get; set; }

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x0600524A RID: 21066
		// (set) Token: 0x0600524B RID: 21067
		float time { get; set; }

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x0600524C RID: 21068
		// (set) Token: 0x0600524D RID: 21069
		float normalizedTime { get; set; }

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x0600524E RID: 21070
		// (set) Token: 0x0600524F RID: 21071
		float speed { get; set; }

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06005250 RID: 21072
		// (set) Token: 0x06005251 RID: 21073
		string name { get; set; }

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x06005252 RID: 21074
		// (set) Token: 0x06005253 RID: 21075
		float weight { get; set; }

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x06005254 RID: 21076
		float length { get; }

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x06005255 RID: 21077
		AnimationClip clip { get; }

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x06005256 RID: 21078
		WrapMode wrapMode { get; }
	}

	// Token: 0x02001038 RID: 4152
	public class StateHandle : SimpleAnimationPlayable.IState
	{
		// Token: 0x06005257 RID: 21079 RVA: 0x002486CF File Offset: 0x002468CF
		public StateHandle(SimpleAnimationPlayable s, int index, Playable target)
		{
			this.m_Parent = s;
			this.m_Index = index;
			this.m_Target = target;
		}

		// Token: 0x06005258 RID: 21080 RVA: 0x002486EC File Offset: 0x002468EC
		public bool IsValid()
		{
			return this.m_Parent.ValidateInput(this.m_Index, this.m_Target);
		}

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x06005259 RID: 21081 RVA: 0x00248705 File Offset: 0x00246905
		// (set) Token: 0x0600525A RID: 21082 RVA: 0x00248738 File Offset: 0x00246938
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

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x0600525B RID: 21083 RVA: 0x00248788 File Offset: 0x00246988
		// (set) Token: 0x0600525C RID: 21084 RVA: 0x002487B4 File Offset: 0x002469B4
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

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x0600525D RID: 21085 RVA: 0x002487E0 File Offset: 0x002469E0
		// (set) Token: 0x0600525E RID: 21086 RVA: 0x00248840 File Offset: 0x00246A40
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

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x0600525F RID: 21087 RVA: 0x002488A1 File Offset: 0x00246AA1
		// (set) Token: 0x06005260 RID: 21088 RVA: 0x002488CC File Offset: 0x00246ACC
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

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06005261 RID: 21089 RVA: 0x002488F8 File Offset: 0x00246AF8
		// (set) Token: 0x06005262 RID: 21090 RVA: 0x00248923 File Offset: 0x00246B23
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

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x06005263 RID: 21091 RVA: 0x0024895D File Offset: 0x00246B5D
		// (set) Token: 0x06005264 RID: 21092 RVA: 0x00248990 File Offset: 0x00246B90
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

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x06005265 RID: 21093 RVA: 0x002489DF File Offset: 0x00246BDF
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

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x06005266 RID: 21094 RVA: 0x00248A0A File Offset: 0x00246C0A
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

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x06005267 RID: 21095 RVA: 0x00248A35 File Offset: 0x00246C35
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

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x06005268 RID: 21096 RVA: 0x00248A60 File Offset: 0x00246C60
		public int index
		{
			get
			{
				return this.m_Index;
			}
		}

		// Token: 0x04005AE1 RID: 23265
		private SimpleAnimationPlayable m_Parent;

		// Token: 0x04005AE2 RID: 23266
		private int m_Index;

		// Token: 0x04005AE3 RID: 23267
		private Playable m_Target;

		// Token: 0x04005AE4 RID: 23268
		private SimpleAnimationPlayable.StateInfo state;
	}

	// Token: 0x02001039 RID: 4153
	private class StateInfo
	{
		// Token: 0x04005AE5 RID: 23269
		public bool enabled;

		// Token: 0x04005AE6 RID: 23270
		public int index;

		// Token: 0x04005AE7 RID: 23271
		public string stateName;

		// Token: 0x04005AE8 RID: 23272
		public bool fading;

		// Token: 0x04005AE9 RID: 23273
		public float time;

		// Token: 0x04005AEA RID: 23274
		public float targetWeight;

		// Token: 0x04005AEB RID: 23275
		public float weight;

		// Token: 0x04005AEC RID: 23276
		public float fadeSpeed;

		// Token: 0x04005AED RID: 23277
		public float speed;

		// Token: 0x04005AEE RID: 23278
		public AnimationClip clip;

		// Token: 0x04005AEF RID: 23279
		public Playable playable;

		// Token: 0x04005AF0 RID: 23280
		public WrapMode wrapMode;

		// Token: 0x04005AF1 RID: 23281
		public bool isClone;

		// Token: 0x04005AF2 RID: 23282
		public SimpleAnimationPlayable.StateHandle parentState;

		// Token: 0x04005AF3 RID: 23283
		public bool weightDirty;

		// Token: 0x04005AF4 RID: 23284
		public bool enabledDirty;

		// Token: 0x04005AF5 RID: 23285
		public bool timeIsUpToDate;
	}

	// Token: 0x0200103A RID: 4154
	private class StateManagement
	{
		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x0600526A RID: 21098 RVA: 0x00248A70 File Offset: 0x00246C70
		public int Count
		{
			get
			{
				return this.m_Count;
			}
		}

		// Token: 0x17000BE5 RID: 3045
		public SimpleAnimationPlayable.StateInfo this[int i]
		{
			get
			{
				return this.m_States[i];
			}
		}

		// Token: 0x0600526C RID: 21100 RVA: 0x00248A86 File Offset: 0x00246C86
		public StateManagement()
		{
			this.m_States = new List<SimpleAnimationPlayable.StateInfo>();
		}

		// Token: 0x0600526D RID: 21101 RVA: 0x00248A9C File Offset: 0x00246C9C
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

		// Token: 0x0600526E RID: 21102 RVA: 0x00248B1B File Offset: 0x00246D1B
		public bool AnyStatePlaying()
		{
			return this.m_States.FindIndex((SimpleAnimationPlayable.StateInfo s) => s != null && s.enabled) != -1;
		}

		// Token: 0x0600526F RID: 21103 RVA: 0x00248B50 File Offset: 0x00246D50
		public bool IsStateDone(int index)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
			return stateInfo == null || !stateInfo.enabled;
		}

		// Token: 0x06005270 RID: 21104 RVA: 0x00248B78 File Offset: 0x00246D78
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

		// Token: 0x06005271 RID: 21105 RVA: 0x00248BD8 File Offset: 0x00246DD8
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

		// Token: 0x06005272 RID: 21106 RVA: 0x00248C28 File Offset: 0x00246E28
		public SimpleAnimationPlayable.StateInfo FindState(string name)
		{
			int num = this.m_States.FindIndex((SimpleAnimationPlayable.StateInfo s) => s != null && s.stateName == name);
			if (num == -1)
			{
				return null;
			}
			return this.m_States[num];
		}

		// Token: 0x06005273 RID: 21107 RVA: 0x00248C6C File Offset: 0x00246E6C
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

		// Token: 0x06005274 RID: 21108 RVA: 0x00248CA0 File Offset: 0x00246EA0
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

		// Token: 0x06005275 RID: 21109 RVA: 0x00248CD1 File Offset: 0x00246ED1
		public void SetInputWeight(int index, float weight)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
			stateInfo.targetWeight = weight;
			stateInfo.weight = weight;
			stateInfo.fading = false;
			stateInfo.weightDirty = true;
		}

		// Token: 0x06005276 RID: 21110 RVA: 0x00248CFC File Offset: 0x00246EFC
		public void SetStateTime(int index, float time)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[index];
			stateInfo.time = time;
			stateInfo.playable.SetTime((double)time);
			stateInfo.playable.SetDone((double)time >= stateInfo.playable.GetDuration<Playable>());
		}

		// Token: 0x06005277 RID: 21111 RVA: 0x00248D48 File Offset: 0x00246F48
		public bool IsCloneOf(int potentialCloneIndex, int originalIndex)
		{
			SimpleAnimationPlayable.StateInfo stateInfo = this.m_States[potentialCloneIndex];
			return stateInfo.isClone && stateInfo.parentState.index == originalIndex;
		}

		// Token: 0x06005278 RID: 21112 RVA: 0x00248D7C File Offset: 0x00246F7C
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

		// Token: 0x06005279 RID: 21113 RVA: 0x00248DC4 File Offset: 0x00246FC4
		public float GetStateSpeed(int index)
		{
			return (float)this.m_States[index].playable.GetSpeed<Playable>();
		}

		// Token: 0x0600527A RID: 21114 RVA: 0x00248DDD File Offset: 0x00246FDD
		public void SetStateSpeed(int index, float value)
		{
			this.m_States[index].playable.SetSpeed((double)value);
		}

		// Token: 0x0600527B RID: 21115 RVA: 0x00248DF7 File Offset: 0x00246FF7
		public float GetInputWeight(int index)
		{
			return this.m_States[index].weight;
		}

		// Token: 0x0600527C RID: 21116 RVA: 0x00248E0C File Offset: 0x0024700C
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

		// Token: 0x0600527D RID: 21117 RVA: 0x00248E84 File Offset: 0x00247084
		public float GetStatePlayableDuration(int index)
		{
			Playable playable = this.m_States[index].playable;
			if (!playable.IsValid<Playable>())
			{
				return 0f;
			}
			return (float)playable.GetDuration<Playable>();
		}

		// Token: 0x0600527E RID: 21118 RVA: 0x00248EB8 File Offset: 0x002470B8
		public AnimationClip GetStateClip(int index)
		{
			AnimationClip clip = this.m_States[index].clip;
			if (clip == null)
			{
				return null;
			}
			return clip;
		}

		// Token: 0x0600527F RID: 21119 RVA: 0x00248EE3 File Offset: 0x002470E3
		public WrapMode GetStateWrapMode(int index)
		{
			return this.m_States[index].wrapMode;
		}

		// Token: 0x06005280 RID: 21120 RVA: 0x00248EF6 File Offset: 0x002470F6
		public string GetStateName(int index)
		{
			return this.m_States[index].stateName;
		}

		// Token: 0x06005281 RID: 21121 RVA: 0x00248F09 File Offset: 0x00247109
		public void SetStateName(int index, string name)
		{
			this.m_States[index].stateName = name;
		}

		// Token: 0x06005282 RID: 21122 RVA: 0x00248F20 File Offset: 0x00247120
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

		// Token: 0x04005AF6 RID: 23286
		private List<SimpleAnimationPlayable.StateInfo> m_States;

		// Token: 0x04005AF7 RID: 23287
		private int m_Count;
	}

	// Token: 0x0200103B RID: 4155
	private class QueuedState
	{
		// Token: 0x06005283 RID: 21123 RVA: 0x00248FA4 File Offset: 0x002471A4
		public QueuedState(SimpleAnimationPlayable.StateHandle s, float t)
		{
			this.state = s;
			this.fadeTime = t;
		}

		// Token: 0x04005AF8 RID: 23288
		public SimpleAnimationPlayable.StateHandle state;

		// Token: 0x04005AF9 RID: 23289
		public float fadeTime;
	}
}
