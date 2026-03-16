using System;

namespace Battle
{
	public class SceneBattle_InfoBuff
	{
		public CharaDef.ActionBuffType Type
		{
			get
			{
				return this._type;
			}
		}

		public long Mask
		{
			get
			{
				return this._mask;
			}
		}

		public CharaDef.AttributeMask Attribute
		{
			get
			{
				return this._attribute;
			}
		}

		public CharaDef.HealthMask Health
		{
			get
			{
				return this._health;
			}
		}

		public CharaDef.EnemyMask Enemy
		{
			get
			{
				return this._enemy;
			}
		}

		public int Updw
		{
			get
			{
				return this._updw;
			}
		}

		public int Turn
		{
			get
			{
				return this._turn;
			}
		}

		public bool Giveup
		{
			get
			{
				return this._giveup;
			}
		}

		public int Arts
		{
			get
			{
				return this._arts;
			}
		}

		public string AdditionalInfo
		{
			get
			{
				return this._additionalInfo;
			}
		}

		public SceneBattle_InfoBuff(CharaDef.ActionBuffType type, long mask, CharaDef.AttributeMask attribute, CharaDef.HealthMask healthMask, CharaDef.EnemyMask enemyMask, int updown, int turnCount, bool giveUp, int ar, string info = "")
		{
			this._type = type;
			this._mask = mask;
			this._attribute = attribute;
			this._health = healthMask;
			this._enemy = enemyMask;
			this._updw = updown;
			this._turn = turnCount;
			this._giveup = giveUp;
			this._arts = ar;
			this._additionalInfo = info;
		}

		public void SetArts(int arts)
		{
			this._arts = arts;
		}

		private CharaDef.ActionBuffType _type;

		private long _mask;

		private CharaDef.AttributeMask _attribute;

		private CharaDef.HealthMask _health;

		private CharaDef.EnemyMask _enemy;

		private int _updw;

		private int _turn;

		private bool _giveup;

		private int _arts;

		private string _additionalInfo;
	}
}
