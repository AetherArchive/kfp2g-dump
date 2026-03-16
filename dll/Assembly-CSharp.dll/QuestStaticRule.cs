using System;
using System.ComponentModel;
using SGNFW.Mst;

public class QuestStaticRule
{
	public int ruleId
	{
		get
		{
			return this.mstData.ruleId;
		}
	}

	public QuestStaticRule.RuleType ruleType
	{
		get
		{
			return (QuestStaticRule.RuleType)this.mstData.ruleType;
		}
	}

	public int target
	{
		get
		{
			return this.mstData.target;
		}
	}

	public QuestStaticRule.DetailType detail
	{
		get
		{
			return (QuestStaticRule.DetailType)this.mstData.detail;
		}
	}

	public QuestStaticRule.TargetAttribute attribute
	{
		get
		{
			return (QuestStaticRule.TargetAttribute)this.mstData.target;
		}
	}

	public QuestStaticRule(MstQuestRuleData ruleData)
	{
		this.mstData = ruleData;
	}

	private MstQuestRuleData mstData;

	public enum RuleType
	{
		[Description("同一属性")]
		SAME_ATTRIBUTE,
		[Description("{Replace}属性")]
		SPECIFIED_ATTRIBUTE,
		[Description("指定")]
		SPECIFIED_CHARA,
		[Description("はなまるチェンジ")]
		HC,
		[Description("サブ属性持ち")]
		SUB_ATTRIBUTE
	}

	public enum DetailType
	{
		[Description("編成できません")]
		DISABLE,
		[Description("編成できます")]
		ENABLE
	}

	public enum TargetAttribute
	{
		INVALID,
		[Description("ファニー(赤)")]
		RED,
		[Description("フレンドリー(緑)")]
		GREEN,
		[Description("リラックス(青)")]
		BLUE,
		[Description("ラブリー(桃)")]
		PINK,
		[Description("アクティブ(黄緑)")]
		LIME,
		[Description("マイペース(水)")]
		AQUA
	}
}
