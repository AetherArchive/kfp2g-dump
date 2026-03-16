using System;
using System.Collections.Generic;
using UnityEngine;

public class CharaStaticAction : ScriptableObject
{
	public string MakeSkillText(int skillLevel)
	{
		string text = this.actionEffect;
		for (int i = 0; i < this.damageList.Count; i++)
		{
			string text2 = "[DAMAGE" + i.ToString() + "]";
			if (text.Contains(text2))
			{
				string text3 = ((int)(this.damageList[i].damageRate * (1f + this.damageList[i].growthRate * (float)(skillLevel - 1)) * 100f + 0.01f)).ToString();
				text = text.Replace(text2, text3);
			}
		}
		for (int j = 0; j < this.buffList.Count; j++)
		{
			string text4 = "[BUFF" + j.ToString() + "]";
			if (text.Contains(text4))
			{
				string text5 = ((int)(Mathf.Abs((float)this.buffList[j].coefficient * (1f + (float)this.buffList[j].growthRate * (float)(skillLevel - 1)) - 1f) * 100f + 0.01f)).ToString();
				text = text.Replace(text4, text5);
			}
		}
		for (int k = 0; k < this.buffList.Count; k++)
		{
			string text6 = "[INCREMENT" + k.ToString() + "]";
			if (text.Contains(text6))
			{
				string text7 = ((int)(Mathf.Abs((float)this.buffList[k].increment) * (1f + (float)this.buffList[k].growthRate * (float)(skillLevel - 1)) + 0.01f)).ToString();
				text = text.Replace(text6, text7);
			}
		}
		for (int l = 0; l < this.buffList.Count; l++)
		{
			string text8 = "[HEAL" + l.ToString() + "]";
			if (text.Contains(text8))
			{
				string text9 = ((int)((float)this.buffList[l].coefficient * (1f + (float)this.buffList[l].growthRate * (float)(skillLevel - 1)) * 100f + 0.01f)).ToString();
				text = text.Replace(text8, text9);
			}
		}
		return text;
	}

	public string actionName;

	[Multiline(6)]
	public string actionEffect;

	public int actionInfoTime;

	public CharaAuthParam authParam;

	public CharaActionParam actionParam;

	public CharaMotionParam motionParam;

	public List<CharaDamageParam> damageList;

	public List<CharaBuffParam> buffList;
}
