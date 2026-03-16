using System;
using System.Collections.Generic;

namespace Battle
{
	public class SceneBattle_Act
	{
		public SceneBattle_Act()
		{
			this.dmg = null;
			this.buf = null;
			this.tag = null;
			this.tim = 0f;
			this.idx = 0;
			this.ability = null;
			this.tactics = null;
		}

		public CharaDamageParam dmg;

		public CharaBuffParam buf;

		public List<SceneBattle_Tag> tag;

		public float tim;

		public int idx;

		public List<SceneBattle_Tag> ability;

		public List<SceneBattle_Tag> tactics;
	}
}
