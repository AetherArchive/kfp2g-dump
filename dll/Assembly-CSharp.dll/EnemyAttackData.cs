using System;
using System.Collections.Generic;

public class EnemyAttackData
{
	public CharaDef.EnemyActionPattern actionPattern;

	public List<EnemyAttackData.Param> attackList = new List<EnemyAttackData.Param>();

	public class Param
	{
		public int point;

		public CharaStaticAction param;

		public List<int> death;

		public List<int> alive;
	}
}
