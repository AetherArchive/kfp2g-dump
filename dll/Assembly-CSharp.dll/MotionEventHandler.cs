using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionEventHandler : MonoBehaviour
{
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

	private IEnumerator GachaMotionChange(CharaModelHandle cmh, float reqStartTime, float reqMotSpeed)
	{
		while (cmh.IsPlaying())
		{
			yield return null;
		}
		cmh.PlayAnimation("GACHA_LP", true, 1f, 0f, 0f, false);
		yield break;
	}

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

	private static readonly string[] SPLIT_STR = new string[] { "__" };

	public List<AuthCharaData> charaModelList = new List<AuthCharaData>();

	public Dictionary<string, string> modelChange = new Dictionary<string, string>();

	public SimpleAnimation.State animeState;

	private List<MotionEventHandler.SimpleAnimationStopper> simpleAnimationList = new List<MotionEventHandler.SimpleAnimationStopper>();

	private enum MOT_EVENT_PARAM_KIND
	{
		CHARA,
		MOTION,
		START,
		END,
		SPEED
	}

	private class SimpleAnimationStopper
	{
		public SimpleAnimation anime;

		public SimpleAnimation.State animeState;

		public float stopTime;

		public bool finish;

		public string motName = "";

		public float reqStartTime;

		public float reqMotSpeed;

		public CharaModelHandle cmh;
	}
}
