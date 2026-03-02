using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGNFW.Common
{
	// Token: 0x02000254 RID: 596
	public static class AnimationExtensions
	{
		// Token: 0x0600255D RID: 9565 RVA: 0x0019F37A File Offset: 0x0019D57A
		public static string[] GetClipNames(this Animation self)
		{
			return self.GetClipNameList().ToArray();
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x0019F388 File Offset: 0x0019D588
		public static List<string> GetClipNameList(this Animation self)
		{
			List<string> list = new List<string>();
			foreach (object obj in self.GetComponent<Animation>())
			{
				AnimationState animationState = (AnimationState)obj;
				list.Add(animationState.name);
			}
			return list;
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x0019F3F0 File Offset: 0x0019D5F0
		public static string GetPlayingName(this Animation self)
		{
			float num = 0f;
			string text = "";
			foreach (object obj in self)
			{
				AnimationState animationState = (AnimationState)obj;
				if (animationState.enabled && animationState.weight > num)
				{
					text = animationState.name;
					num = animationState.weight;
				}
			}
			return text;
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x0019F46C File Offset: 0x0019D66C
		public static bool HasClip(this Animation self, string animationName)
		{
			using (IEnumerator enumerator = self.GetComponent<Animation>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((AnimationState)enumerator.Current).name == animationName)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
