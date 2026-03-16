using System;
using System.Collections.Generic;

namespace Battle
{
	public class SceneBattle_Recover
	{
		public long Mask
		{
			get
			{
				return this._mask;
			}
		}

		public List<CharaDef.ActionBuffType> Type
		{
			get
			{
				return this._typ;
			}
		}

		public string Nam
		{
			get
			{
				return this._nam;
			}
		}

		public int Prm
		{
			get
			{
				return this._prm;
			}
		}

		public int Icn
		{
			get
			{
				return this._icn;
			}
		}

		public SceneBattle_Recover(CharaDef.ActionAbnormalMask m, CharaDef.ActionBuffType t, string n, int p, int i)
		{
			this._mask = CharaDef.AbnormalMask(m);
			this._typ = new List<CharaDef.ActionBuffType> { t };
			this._nam = n;
			this._prm = p;
			this._icn = i;
		}

		public SceneBattle_Recover(CharaDef.ActionAbnormalMask m, List<CharaDef.ActionBuffType> t, string n, int p, int i)
		{
			this._mask = CharaDef.AbnormalMask(m);
			this._typ = t;
			this._nam = n;
			this._prm = p;
			this._icn = i;
		}

		public SceneBattle_Recover(CharaDef.ActionAbnormalMask2 m, CharaDef.ActionBuffType t, string n, int p, int i)
		{
			this._mask = CharaDef.AbnormalMask(m);
			this._typ = new List<CharaDef.ActionBuffType> { t };
			this._nam = n;
			this._prm = p;
			this._icn = i;
		}

		public SceneBattle_Recover(CharaDef.ActionAbnormalMask2 m, List<CharaDef.ActionBuffType> t, string n, int p, int i)
		{
			this._mask = CharaDef.AbnormalMask(m);
			this._typ = t;
			this._nam = n;
			this._prm = p;
			this._icn = i;
		}

		public SceneBattle_Recover(CharaDef.ActionBuffType t, string n, int i)
		{
			this._mask = 0L;
			this._typ = new List<CharaDef.ActionBuffType> { t };
			this._nam = n;
			this._prm = 0;
			this._icn = i;
		}

		private long _mask;

		private List<CharaDef.ActionBuffType> _typ;

		private string _nam;

		private int _prm;

		private int _icn;
	}
}
