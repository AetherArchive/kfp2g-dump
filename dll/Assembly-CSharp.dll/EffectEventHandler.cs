using System;
using UnityEngine;

public class EffectEventHandler : MonoBehaviour
{
	public void AuthEffectPlay(AnimationEvent animEvent)
	{
		AuthEffectParam authEffectParam = new AuthEffectParam(animEvent.stringParameter);
		this.authPlayer.PlayEffectByEventHandler(authEffectParam.effectName);
	}

	public AuthPlayer authPlayer;
}
