using System;
using System.Collections.Generic;

namespace Battle
{
	public class SceneBattle_Tag
	{
		public SceneBattle_Tag(SceneBattle_Chara chr)
		{
			this.tag = chr;
			this.cover = null;
			this.flg = 0;
			this.hp = 0;
			this.kp = 0;
			this.buf = null;
			this.recover = null;
			this.infKey = 0;
			this.noExe = 0;
			this.dup = 1f;
		}

		public SceneBattle_Chara tag;

		public SceneBattle_Chara cover;

		public int flg;

		public int hp;

		public int kp;

		public SceneBattle_Buff buf;

		public List<int> recover;

		public int infKey;

		public int noExe;

		public float dup;
	}
}
