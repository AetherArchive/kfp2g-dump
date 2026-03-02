using System;

namespace Battle
{
	// Token: 0x02000219 RID: 537
	public class SceneBattle_InfoBuff
	{
		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x060022CC RID: 8908 RVA: 0x001948B8 File Offset: 0x00192AB8
		public CharaDef.ActionBuffType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x060022CD RID: 8909 RVA: 0x001948C0 File Offset: 0x00192AC0
		public long Mask
		{
			get
			{
				return this._mask;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x060022CE RID: 8910 RVA: 0x001948C8 File Offset: 0x00192AC8
		public CharaDef.AttributeMask Attribute
		{
			get
			{
				return this._attribute;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x060022CF RID: 8911 RVA: 0x001948D0 File Offset: 0x00192AD0
		public CharaDef.HealthMask Health
		{
			get
			{
				return this._health;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x060022D0 RID: 8912 RVA: 0x001948D8 File Offset: 0x00192AD8
		public CharaDef.EnemyMask Enemy
		{
			get
			{
				return this._enemy;
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x060022D1 RID: 8913 RVA: 0x001948E0 File Offset: 0x00192AE0
		public int Updw
		{
			get
			{
				return this._updw;
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x060022D2 RID: 8914 RVA: 0x001948E8 File Offset: 0x00192AE8
		public int Turn
		{
			get
			{
				return this._turn;
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x060022D3 RID: 8915 RVA: 0x001948F0 File Offset: 0x00192AF0
		public bool Giveup
		{
			get
			{
				return this._giveup;
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x060022D4 RID: 8916 RVA: 0x001948F8 File Offset: 0x00192AF8
		public int Arts
		{
			get
			{
				return this._arts;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x060022D5 RID: 8917 RVA: 0x00194900 File Offset: 0x00192B00
		public string AdditionalInfo
		{
			get
			{
				return this._additionalInfo;
			}
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x00194908 File Offset: 0x00192B08
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

		// Token: 0x060022D7 RID: 8919 RVA: 0x00194968 File Offset: 0x00192B68
		public void SetArts(int arts)
		{
			this._arts = arts;
		}

		// Token: 0x040019F0 RID: 6640
		private CharaDef.ActionBuffType _type;

		// Token: 0x040019F1 RID: 6641
		private long _mask;

		// Token: 0x040019F2 RID: 6642
		private CharaDef.AttributeMask _attribute;

		// Token: 0x040019F3 RID: 6643
		private CharaDef.HealthMask _health;

		// Token: 0x040019F4 RID: 6644
		private CharaDef.EnemyMask _enemy;

		// Token: 0x040019F5 RID: 6645
		private int _updw;

		// Token: 0x040019F6 RID: 6646
		private int _turn;

		// Token: 0x040019F7 RID: 6647
		private bool _giveup;

		// Token: 0x040019F8 RID: 6648
		private int _arts;

		// Token: 0x040019F9 RID: 6649
		private string _additionalInfo;
	}
}
