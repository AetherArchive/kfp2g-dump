using System;
using System.ComponentModel;
using SGNFW.Mst;

// Token: 0x020000E6 RID: 230
public class QuestStaticRule
{
	// Token: 0x1700028F RID: 655
	// (get) Token: 0x06000A6C RID: 2668 RVA: 0x0003C90C File Offset: 0x0003AB0C
	public int ruleId
	{
		get
		{
			return this.mstData.ruleId;
		}
	}

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06000A6D RID: 2669 RVA: 0x0003C919 File Offset: 0x0003AB19
	public QuestStaticRule.RuleType ruleType
	{
		get
		{
			return (QuestStaticRule.RuleType)this.mstData.ruleType;
		}
	}

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06000A6E RID: 2670 RVA: 0x0003C926 File Offset: 0x0003AB26
	public int target
	{
		get
		{
			return this.mstData.target;
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06000A6F RID: 2671 RVA: 0x0003C933 File Offset: 0x0003AB33
	public QuestStaticRule.DetailType detail
	{
		get
		{
			return (QuestStaticRule.DetailType)this.mstData.detail;
		}
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06000A70 RID: 2672 RVA: 0x0003C940 File Offset: 0x0003AB40
	public QuestStaticRule.TargetAttribute attribute
	{
		get
		{
			return (QuestStaticRule.TargetAttribute)this.mstData.target;
		}
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x0003C94D File Offset: 0x0003AB4D
	public QuestStaticRule(MstQuestRuleData ruleData)
	{
		this.mstData = ruleData;
	}

	// Token: 0x04000839 RID: 2105
	private MstQuestRuleData mstData;

	// Token: 0x020007E2 RID: 2018
	public enum RuleType
	{
		// Token: 0x04003514 RID: 13588
		[Description("同一属性")]
		SAME_ATTRIBUTE,
		// Token: 0x04003515 RID: 13589
		[Description("{Replace}属性")]
		SPECIFIED_ATTRIBUTE,
		// Token: 0x04003516 RID: 13590
		[Description("指定")]
		SPECIFIED_CHARA,
		// Token: 0x04003517 RID: 13591
		[Description("はなまるチェンジ")]
		HC,
		// Token: 0x04003518 RID: 13592
		[Description("サブ属性持ち")]
		SUB_ATTRIBUTE
	}

	// Token: 0x020007E3 RID: 2019
	public enum DetailType
	{
		// Token: 0x0400351A RID: 13594
		[Description("編成できません")]
		DISABLE,
		// Token: 0x0400351B RID: 13595
		[Description("編成できます")]
		ENABLE
	}

	// Token: 0x020007E4 RID: 2020
	public enum TargetAttribute
	{
		// Token: 0x0400351D RID: 13597
		INVALID,
		// Token: 0x0400351E RID: 13598
		[Description("ファニー(赤)")]
		RED,
		// Token: 0x0400351F RID: 13599
		[Description("フレンドリー(緑)")]
		GREEN,
		// Token: 0x04003520 RID: 13600
		[Description("リラックス(青)")]
		BLUE,
		// Token: 0x04003521 RID: 13601
		[Description("ラブリー(桃)")]
		PINK,
		// Token: 0x04003522 RID: 13602
		[Description("アクティブ(黄緑)")]
		LIME,
		// Token: 0x04003523 RID: 13603
		[Description("マイペース(水)")]
		AQUA
	}
}
