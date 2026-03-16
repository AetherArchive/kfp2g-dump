using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGNFW.Common
{
	public static class AnimationExtensions
	{
		public static string[] GetClipNames(this Animation self)
		{
			return self.GetClipNameList().ToArray();
		}

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
