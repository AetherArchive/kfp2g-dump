using System;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class MasterStaticSkill : ScriptableObject
{
	// Token: 0x1700007C RID: 124
	// (get) Token: 0x060002A6 RID: 678 RVA: 0x00015898 File Offset: 0x00013A98
	public string iconName
	{
		get
		{
			return "icon_playerskill_" + (this.id % 100).ToString("00");
		}
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x060002A7 RID: 679 RVA: 0x000158C8 File Offset: 0x00013AC8
	public string iconMiniName
	{
		get
		{
			return "icon_playerskill_mini_" + (this.id % 100).ToString("00");
		}
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x000158F5 File Offset: 0x00013AF5
	public float glowParam(float prm, int skillLevel)
	{
		return prm * (1f + this.glowRate * (float)(skillLevel - 1));
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x0001590C File Offset: 0x00013B0C
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

	// Token: 0x0400043A RID: 1082
	public int id;

	// Token: 0x0400043B RID: 1083
	public string skillName;

	// Token: 0x0400043C RID: 1084
	[Multiline]
	public string skillInfo;

	// Token: 0x0400043D RID: 1085
	public int levelTableId;

	// Token: 0x0400043E RID: 1086
	public int maxLevel;

	// Token: 0x0400043F RID: 1087
	public MasterStaticSkill.Type type;

	// Token: 0x04000440 RID: 1088
	public int paramI0;

	// Token: 0x04000441 RID: 1089
	public int paramI1;

	// Token: 0x04000442 RID: 1090
	public int paramI2;

	// Token: 0x04000443 RID: 1091
	public float paramF0;

	// Token: 0x04000444 RID: 1092
	public float paramF1;

	// Token: 0x04000445 RID: 1093
	public float paramF2;

	// Token: 0x04000446 RID: 1094
	public float glowRate;

	// Token: 0x04000447 RID: 1095
	public int maxChangeNum;

	// Token: 0x0200061F RID: 1567
	public enum Type
	{
		// Token: 0x04002D80 RID: 11648
		INVALID,
		// Token: 0x04002D81 RID: 11649
		ORDER_CARD_CHANGE,
		// Token: 0x04002D82 RID: 11650
		MOVE_POINT_ADD,
		// Token: 0x04002D83 RID: 11651
		CHARA_REINFORCE,
		// Token: 0x04002D84 RID: 11652
		RESURRECT,
		// Token: 0x04002D85 RID: 11653
		PLAZM_REINFORCE,
		// Token: 0x04002D86 RID: 11654
		ENEMY_WEAKEN,
		// Token: 0x04002D87 RID: 11655
		MYSIDE_HEAL,
		// Token: 0x04002D88 RID: 11656
		RECOVERY,
		// Token: 0x04002D89 RID: 11657
		ALL_WAIT
	}
}
