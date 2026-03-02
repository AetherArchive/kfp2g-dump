using System;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class SoundEventHandler : MonoBehaviour
{
	// Token: 0x06000167 RID: 359 RVA: 0x0000B662 File Offset: 0x00009862
	public void SoundPlay(AnimationEvent animEvent)
	{
		SoundManager.Play(animEvent.stringParameter, false, false);
	}

	// Token: 0x06000168 RID: 360 RVA: 0x0000B672 File Offset: 0x00009872
	public void AuthVoicePlay(AnimationEvent animEvent)
	{
		SoundManager.Play(animEvent.stringParameter, false, false);
	}

	// Token: 0x040001D0 RID: 464
	private readonly string[] SPLIT_STR = new string[] { "__" };

	// Token: 0x020005DC RID: 1500
	private enum AUTH_SOUND_PARAM_KIND
	{
		// Token: 0x04002A44 RID: 10820
		CHARA,
		// Token: 0x04002A45 RID: 10821
		CUE
	}
}
