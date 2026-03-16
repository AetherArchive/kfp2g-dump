using System;
using UnityEngine;

public class MasterStaticSkill : ScriptableObject
{
	public string iconName
	{
		get
		{
			return "icon_playerskill_" + (this.id % 100).ToString("00");
		}
	}

	public string iconMiniName
	{
		get
		{
			return "icon_playerskill_mini_" + (this.id % 100).ToString("00");
		}
	}

	public float glowParam(float prm, int skillLevel)
	{
		return prm * (1f + this.glowRate * (float)(skillLevel - 1));
	}

	public string MakeSkillText(int skillLevel, bool isGrow)
	{
		string text = this.skillInfo;
		string text2 = "[paramI0]";
		if (text.Contains(text2))
		{
			string text3 = ((int)(this.glowParam((float)this.paramI0, skillLevel) + 0.01f)).ToString();
			if (isGrow)
			{
				text3 = "<color=#FF7B16FF>" + text3 + "</color>";
			}
			text = text.Replace(text2, text3);
		}
		text2 = "[paramI1]";
		if (text.Contains(text2))
		{
			string text4 = ((int)(this.glowParam((float)this.paramI1, skillLevel) + 0.01f)).ToString();
			if (isGrow)
			{
				text4 = "<color=#FF7B16FF>" + text4 + "</color>";
			}
			text = text.Replace(text2, text4);
		}
		text2 = "[paramI2]";
		if (text.Contains(text2))
		{
			string text5 = ((int)(this.glowParam((float)this.paramI2, skillLevel) + 0.01f)).ToString();
			if (isGrow)
			{
				text5 = "<color=#FF7B16FF>" + text5 + "</color>";
			}
			text = text.Replace(text2, text5);
		}
		text2 = "[paramF0]";
		if (text.Contains(text2))
		{
			int num = (int)(this.glowParam(this.paramF0, skillLevel) * 10f + 0.01f);
			string text6 = (num / 10).ToString();
			num %= 10;
			if (num > 0)
			{
				text6 = text6 + "." + num.ToString();
			}
			if (isGrow)
			{
				text6 = "<color=#FF7B16FF>" + text6 + "</color>";
			}
			text = text.Replace(text2, text6);
		}
		text2 = "[paramF1]";
		if (text.Contains(text2))
		{
			float num2 = this.glowParam(this.paramF1, skillLevel);
			string text7 = Mathf.Abs((int)((num2 - 1f) * 100f + ((num2 < 1f) ? (-0.01f) : 0.01f))).ToString();
			if (isGrow)
			{
				text7 = "<color=#FF7B16FF>" + text7 + "</color>";
			}
			text = text.Replace(text2, text7);
		}
		text2 = "[paramF2]";
		if (text.Contains(text2))
		{
			string text8 = ((int)(this.glowParam(this.paramF2, skillLevel) * 100f + 0.01f)).ToString();
			if (isGrow)
			{
				text8 = "<color=#FF7B16FF>" + text8 + "</color>";
			}
			text = text.Replace(text2, text8);
		}
		return text;
	}

	public int id;

	public string skillName;

	[Multiline]
	public string skillInfo;

	public int levelTableId;

	public int maxLevel;

	public MasterStaticSkill.Type type;

	public int paramI0;

	public int paramI1;

	public int paramI2;

	public float paramF0;

	public float paramF1;

	public float paramF2;

	public float glowRate;

	public int maxChangeNum;

	public enum Type
	{
		INVALID,
		ORDER_CARD_CHANGE,
		MOVE_POINT_ADD,
		CHARA_REINFORCE,
		RESURRECT,
		PLAZM_REINFORCE,
		ENEMY_WEAKEN,
		MYSIDE_HEAL,
		RECOVERY,
		ALL_WAIT
	}
}
