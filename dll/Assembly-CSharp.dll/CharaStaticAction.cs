using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000061 RID: 97
public class CharaStaticAction : ScriptableObject
{
	// Token: 0x06000292 RID: 658 RVA: 0x00015440 File Offset: 0x00013640
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

	// Token: 0x040003B6 RID: 950
	public string actionName;

	// Token: 0x040003B7 RID: 951
	[Multiline(6)]
	public string actionEffect;

	// Token: 0x040003B8 RID: 952
	public int actionInfoTime;

	// Token: 0x040003B9 RID: 953
	public CharaAuthParam authParam;

	// Token: 0x040003BA RID: 954
	public CharaActionParam actionParam;

	// Token: 0x040003BB RID: 955
	public CharaMotionParam motionParam;

	// Token: 0x040003BC RID: 956
	public List<CharaDamageParam> damageList;

	// Token: 0x040003BD RID: 957
	public List<CharaBuffParam> buffList;
}
