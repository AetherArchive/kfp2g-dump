using System;

namespace Battle
{
	public class SceneBattle_ResistKind
	{
		public long typ
		{
			get
			{
				return this._typ;
			}
		}

		public int num
		{
			get
			{
				return this._num;
			}
		}

		public SceneBattle_ResistKind(long m, int j)
		{
			this._typ = m;
			this._num = j;
		}

		private long _typ;

		private int _num;
	}
}
