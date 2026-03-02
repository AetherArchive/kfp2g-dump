using System;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class EffectEventHandler : MonoBehaviour
{
	// Token: 0x0600015C RID: 348 RVA: 0x0000AF4C File Offset: 0x0000914C
	public void AuthEffectPlay(AnimationEvent animEvent)
	{
		AuthEffectParam authEffectParam = new AuthEffectParam(animEvent.stringParameter);
		this.authPlayer.PlayEffectByEventHandler(authEffectParam.effectName);
	}

	// Token: 0x040001BB RID: 443
	public AuthPlayer authPlayer;
}
