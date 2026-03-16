using System;
using System.Collections.Generic;

namespace Battle
{
	public class SceneBattle_PlayerSkillRecover
	{
		public SceneBattle_Friends Ply
		{
			get
			{
				return this._ply;
			}
		}

		public List<SceneBattle_Buff> NewBuff
		{
			get
			{
				return this._newBuff;
			}
		}

		public List<SceneBattle_Buff> Buff
		{
			get
			{
				return this._buff;
			}
		}

		public List<int> Recover
		{
			get
			{
				return this._recover;
			}
		}

		public void SetFriends(SceneBattle_Friends ply)
		{
			this._ply = ply;
		}

		private SceneBattle_Friends _ply;

		private List<SceneBattle_Buff> _newBuff = new List<SceneBattle_Buff>();

		private List<SceneBattle_Buff> _buff = new List<SceneBattle_Buff>();

		private List<int> _recover = new List<int>();
	}
}
