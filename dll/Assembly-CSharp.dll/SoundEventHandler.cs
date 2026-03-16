using System;
using UnityEngine;

public class SoundEventHandler : MonoBehaviour
{
	public void SoundPlay(AnimationEvent animEvent)
	{
		SoundManager.Play(animEvent.stringParameter, false, false);
	}

	public void AuthVoicePlay(AnimationEvent animEvent)
	{
		SoundManager.Play(animEvent.stringParameter, false, false);
	}

	private readonly string[] SPLIT_STR = new string[] { "__" };

	private enum AUTH_SOUND_PARAM_KIND
	{
		CHARA,
		CUE
	}
}
