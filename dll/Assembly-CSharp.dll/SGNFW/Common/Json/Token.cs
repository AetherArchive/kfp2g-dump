using System;

namespace SGNFW.Common.Json
{
	// Token: 0x02000278 RID: 632
	public struct Token
	{
		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x060026AF RID: 9903 RVA: 0x001A3655 File Offset: 0x001A1855
		public bool Bool
		{
			get
			{
				return this.GetBool();
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x060026B0 RID: 9904 RVA: 0x001A365D File Offset: 0x001A185D
		public sbyte Sbyte
		{
			get
			{
				return (sbyte)this.GetLong();
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x060026B1 RID: 9905 RVA: 0x001A3666 File Offset: 0x001A1866
		public short Short
		{
			get
			{
				return (short)this.GetLong();
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x060026B2 RID: 9906 RVA: 0x001A366F File Offset: 0x001A186F
		public int Int
		{
			get
			{
				return (int)this.GetLong();
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x060026B3 RID: 9907 RVA: 0x001A3678 File Offset: 0x001A1878
		public long Long
		{
			get
			{
				return this.GetLong();
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x060026B4 RID: 9908 RVA: 0x001A3680 File Offset: 0x001A1880
		public float Float
		{
			get
			{
				return (float)this.GetDouble();
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x060026B5 RID: 9909 RVA: 0x001A3689 File Offset: 0x001A1889
		public double Double
		{
			get
			{
				return this.GetDouble();
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x060026B6 RID: 9910 RVA: 0x001A3691 File Offset: 0x001A1891
		public byte Byte
		{
			get
			{
				return (byte)this.GetLong();
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x060026B7 RID: 9911 RVA: 0x001A369A File Offset: 0x001A189A
		public ushort Ushort
		{
			get
			{
				return (ushort)this.GetLong();
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x060026B8 RID: 9912 RVA: 0x001A36A3 File Offset: 0x001A18A3
		public uint Uint
		{
			get
			{
				return (uint)this.GetLong();
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x060026B9 RID: 9913 RVA: 0x001A36AC File Offset: 0x001A18AC
		public ulong Ulong
		{
			get
			{
				return (ulong)this.GetLong();
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x060026BA RID: 9914 RVA: 0x001A36B4 File Offset: 0x001A18B4
		public char Char
		{
			get
			{
				return (char)this.GetLong();
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x060026BB RID: 9915 RVA: 0x001A36BD File Offset: 0x001A18BD
		public string String
		{
			get
			{
				return this.GetString();
			}
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x001A36C8 File Offset: 0x001A18C8
		public string GetString()
		{
			switch (this.type)
			{
			case Token.Type.Invalid:
				return "";
			case Token.Type.Floating:
				return this.doubleValue.ToString();
			case Token.Type.Integer:
				return this.longValue.ToString();
			case Token.Type.String:
				return this.stringValue;
			case Token.Type.Boolean:
				if (!this.boolValue)
				{
					return "false";
				}
				return "true";
			case Token.Type.Null:
				return "null";
			}
			throw new Exception("Json Type Mismatch");
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x001A3758 File Offset: 0x001A1958
		public double GetDouble()
		{
			switch (this.type)
			{
			case Token.Type.Invalid:
				return 0.0;
			case Token.Type.Floating:
				return this.doubleValue;
			case Token.Type.Integer:
				return (double)this.longValue;
			case Token.Type.String:
			{
				double num;
				if (!double.TryParse(this.stringValue, out num))
				{
					return 0.0;
				}
				return num;
			}
			case Token.Type.Boolean:
				if (!this.boolValue)
				{
					return 0.0;
				}
				return 1.0;
			case Token.Type.Null:
				return 0.0;
			}
			throw new Exception("Json Type Mismatch");
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x001A3800 File Offset: 0x001A1A00
		public long GetLong()
		{
			switch (this.type)
			{
			case Token.Type.Invalid:
				return 0L;
			case Token.Type.Floating:
				return (long)this.doubleValue;
			case Token.Type.Integer:
				return this.longValue;
			case Token.Type.String:
			{
				long num;
				if (!long.TryParse(this.stringValue, out num))
				{
					return 0L;
				}
				return num;
			}
			case Token.Type.Boolean:
				return this.boolValue ? 1L : 0L;
			case Token.Type.Null:
				return 0L;
			}
			throw new Exception("Json Type Mismatch");
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x001A3888 File Offset: 0x001A1A88
		public bool GetBool()
		{
			switch (this.type)
			{
			case Token.Type.Invalid:
				return false;
			case Token.Type.Floating:
				return this.doubleValue != 0.0;
			case Token.Type.Integer:
				return this.longValue != 0L;
			case Token.Type.String:
			{
				bool flag;
				return bool.TryParse(this.stringValue, out flag) && flag;
			}
			case Token.Type.Boolean:
				return this.boolValue;
			case Token.Type.Null:
				return false;
			}
			throw new Exception("Json Type Mismatch");
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x001A3914 File Offset: 0x001A1B14
		public TEnum GetEnum<TEnum>() where TEnum : struct
		{
			if (this.type == Token.Type.String)
			{
				try
				{
					return (TEnum)((object)Enum.Parse(typeof(TEnum), this.stringValue));
				}
				catch (Exception)
				{
					return (TEnum)((object)Enum.ToObject(typeof(TEnum), 0));
				}
			}
			return (TEnum)((object)Enum.ToObject(typeof(TEnum), this.GetLong()));
		}

		// Token: 0x04001C6B RID: 7275
		public Token.Type type;

		// Token: 0x04001C6C RID: 7276
		public double doubleValue;

		// Token: 0x04001C6D RID: 7277
		public long longValue;

		// Token: 0x04001C6E RID: 7278
		public bool boolValue;

		// Token: 0x04001C6F RID: 7279
		public string stringValue;

		// Token: 0x020010A6 RID: 4262
		public enum Type
		{
			// Token: 0x04005C56 RID: 23638
			Invalid,
			// Token: 0x04005C57 RID: 23639
			ObjectBegin,
			// Token: 0x04005C58 RID: 23640
			ObjectEnd,
			// Token: 0x04005C59 RID: 23641
			ArrayBegin,
			// Token: 0x04005C5A RID: 23642
			ArrayEnd,
			// Token: 0x04005C5B RID: 23643
			Floating,
			// Token: 0x04005C5C RID: 23644
			Integer,
			// Token: 0x04005C5D RID: 23645
			String,
			// Token: 0x04005C5E RID: 23646
			Boolean,
			// Token: 0x04005C5F RID: 23647
			Null,
			// Token: 0x04005C60 RID: 23648
			End
		}
	}
}
