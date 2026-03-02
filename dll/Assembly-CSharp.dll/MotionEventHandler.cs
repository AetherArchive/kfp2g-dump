using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class MotionEventHandler : MonoBehaviour
{
	// Token: 0x0600015E RID: 350 RVA: 0x0000AF80 File Offset: 0x00009180
	public static string AnimeEventParse(AnimationEvent animEvent, out int idx, out float start, out float end, out float speed, out bool loop)
	{
		string[] array = animEvent.stringParameter.Split(MotionEventHandler.SPLIT_STR, StringSplitOptions.None);
		string[] array2 = array[0].Split('_', StringSplitOptions.None);
		idx = int.Parse(array2[1]) - 1;
		string text = array[1];
		string text2 = array[2];
		loop = false;
		if (text2.StartsWith("_"))
		{
			text2 = text2.Substring(2);
			text += "_";
		}
		else
		{
			text2 = text2.Substring(1);
		}
		if (text.StartsWith("KEYTYPE_"))
		{
			text = text.Replace("KEYTYPE_", "");
		}
		else if (text.StartsWith("KEYTYPELP_"))
		{
			text = text.Replace("KEYTYPELP_", "");
			loop = true;
		}
		else
		{
			text = "AUTH_" + text;
		}
		string text3 = array[3].Substring(1);
		string text4 = "1";
		if (array.Length >= 5)
		{
			text4 = array[4].Substring(2);
		}
		start = float.Parse(text2) * 0.033333335f;
		end = (loop ? (-1f) : (float.Parse(text3) * 0.033333335f));
		speed = 1f / float.Parse(text4);
		return text;
	}

	// Token: 0x0600015F RID: 351 RVA: 0x0000B09C File Offset: 0x0000929C
	public void AuthMotPlay(AnimationEvent animEvent)
	{
		int num;
		float num2;
		float num3;
		float num4;
		bool flag;
		string text = MotionEventHandler.AnimeEventParse(animEvent, out num, out num2, out num3, out num4, out flag);
		foreach (string text2 in this.modelChange.Keys)
		{
			if (text.IndexOf(text2) > 0)
			{
				text = text.Replace(text2, this.modelChange[text2]);
				break;
			}
		}
		AuthCharaData authCharaData = this.charaModelList[num];
		CharaModelHandle charaModelHandle = authCharaData.charaModelHandle;
		charaModelHandle.SetModelActive(true);
		charaModelHandle.FadeIn(0f);
		if (num3 > 0f && this.animeState == null)
		{
			using (List<SimpleAnimation>.Enumerator enumerator2 = charaModelHandle.GetEnableAnimationList().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					SimpleAnimation sa = enumerator2.Current;
					if (!(sa == null))
					{
						MotionEventHandler.SimpleAnimationStopper simpleAnimationStopper = this.simpleAnimationList.Find((MotionEventHandler.SimpleAnimationStopper item) => item.anime == sa);
						if (simpleAnimationStopper == null)
						{
							simpleAnimationStopper = new MotionEventHandler.SimpleAnimationStopper();
							this.simpleAnimationList.Add(simpleAnimationStopper);
						}
						simpleAnimationStopper.anime = sa;
						simpleAnimationStopper.animeState = sa[text];
						simpleAnimationStopper.motName = text;
						simpleAnimationStopper.reqStartTime = num2;
						simpleAnimationStopper.reqMotSpeed = num4;
						simpleAnimationStopper.cmh = charaModelHandle;
						if (simpleAnimationStopper.animeState == null)
						{
							simpleAnimationStopper.animeState = sa[text.Replace("AUTH_", "")];
						}
						simpleAnimationStopper.stopTime = num3;
					}
				}
			}
		}
		authCharaData.offset = animEvent.time;
		authCharaData.start = num2;
		authCharaData.speed = num4;
		authCharaData.end = num3;
		if (this.animeState != null)
		{
			num2 += (this.animeState.time - animEvent.time) * num4;
		}
		charaModelHandle.PlayAnimationByAuth(text, num2, num4, flag);
		if (text == "GACHA_ST")
		{
			base.StartCoroutine(this.GachaMotionChange(charaModelHandle, num2, num4));
		}
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000B2E8 File Offset: 0x000094E8
	private IEnumerator GachaMotionChange(CharaModelHandle cmh, float reqStartTime, float reqMotSpeed)
	{
		while (cmh.IsPlaying())
		{
			yield return null;
		}
		cmh.PlayAnimation("GACHA_LP", true, 1f, 0f, 0f, false);
		yield break;
	}

	// Token: 0x06000161 RID: 353 RVA: 0x0000B2F8 File Offset: 0x000094F8
	private void Update()
	{
		foreach (MotionEventHandler.SimpleAnimationStopper simpleAnimationStopper in this.simpleAnimationList)
		{
			if (simpleAnimationStopper.animeState != null && simpleAnimationStopper.animeState.time >= simpleAnimationStopper.stopTime)
			{
				simpleAnimationStopper.animeState.time = simpleAnimationStopper.stopTime;
				simpleAnimationStopper.animeState.speed = 0f;
				simpleAnimationStopper.anime.ExStop(false);
				simpleAnimationStopper.finish = true;
			}
		}
		this.simpleAnimationList.RemoveAll((MotionEventHandler.SimpleAnimationStopper item) => item.finish);
		if (this.animeState != null)
		{
			foreach (AuthCharaData authCharaData in this.charaModelList)
			{
				if (!authCharaData.charaModelHandle.IsLoopAnimation())
				{
					if (authCharaData.charaModelHandle.IsCurrentAnimation("GACHA_ST") && this.animeState.normalizedTime >= 1f)
					{
						authCharaData.charaModelHandle.SetAnimationSpeed(authCharaData.speed);
					}
					else
					{
						float num = this.animeState.time - authCharaData.offset;
						num = authCharaData.start + num * authCharaData.speed;
						if (authCharaData.end > 0f && num > authCharaData.end)
						{
							num = authCharaData.end;
						}
						authCharaData.charaModelHandle.SetAnimationTime(num);
						authCharaData.charaModelHandle.SetAnimationSpeed(0f);
					}
				}
			}
		}
	}

	// Token: 0x040001BC RID: 444
	private static readonly string[] SPLIT_STR = new string[] { "__" };

	// Token: 0x040001BD RID: 445
	public List<AuthCharaData> charaModelList = new List<AuthCharaData>();

	// Token: 0x040001BE RID: 446
	public Dictionary<string, string> modelChange = new Dictionary<string, string>();

	// Token: 0x040001BF RID: 447
	public SimpleAnimation.State animeState;

	// Token: 0x040001C0 RID: 448
	private List<MotionEventHandler.SimpleAnimationStopper> simpleAnimationList = new List<MotionEventHandler.SimpleAnimationStopper>();

	// Token: 0x020005D7 RID: 1495
	private enum MOT_EVENT_PARAM_KIND
	{
		// Token: 0x04002A30 RID: 10800
		CHARA,
		// Token: 0x04002A31 RID: 10801
		MOTION,
		// Token: 0x04002A32 RID: 10802
		START,
		// Token: 0x04002A33 RID: 10803
		END,
		// Token: 0x04002A34 RID: 10804
		SPEED
	}

	// Token: 0x020005D8 RID: 1496
	private class SimpleAnimationStopper
	{
		// Token: 0x04002A35 RID: 10805
		public SimpleAnimation anime;

		// Token: 0x04002A36 RID: 10806
		public SimpleAnimation.State animeState;

		// Token: 0x04002A37 RID: 10807
		public float stopTime;

		// Token: 0x04002A38 RID: 10808
		public bool finish;

		// Token: 0x04002A39 RID: 10809
		public string motName = "";

		// Token: 0x04002A3A RID: 10810
		public float reqStartTime;

		// Token: 0x04002A3B RID: 10811
		public float reqMotSpeed;

		// Token: 0x04002A3C RID: 10812
		public CharaModelHandle cmh;
	}
}
