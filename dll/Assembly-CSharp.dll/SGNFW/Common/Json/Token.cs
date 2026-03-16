using System;

namespace SGNFW.Common.Json
{
	public struct Token
	{
		public bool Bool
		{
			get
			{
				return this.GetBool();
			}
		}

		public sbyte Sbyte
		{
			get
			{
				return (sbyte)this.GetLong();
			}
		}

		public short Short
		{
			get
			{
				return (short)this.GetLong();
			}
		}

		public int Int
		{
			get
			{
				return (int)this.GetLong();
			}
		}

		public long Long
		{
			get
			{
				return this.GetLong();
			}
		}

		public float Float
		{
			get
			{
				return (float)this.GetDouble();
			}
		}

		public double Double
		{
			get
			{
				return this.GetDouble();
			}
		}

		public byte Byte
		{
			get
			{
				return (byte)this.GetLong();
			}
		}

		public ushort Ushort
		{
			get
			{
				return (ushort)this.GetLong();
			}
		}

		public uint Uint
		{
			get
			{
				return (uint)this.GetLong();
			}
		}

		public ulong Ulong
		{
			get
			{
				return (ulong)this.GetLong();
			}
		}

		public char Char
		{
			get
			{
				return (char)this.GetLong();
			}
		}

		public string String
		{
			get
			{
				return this.GetString();
			}
		}

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

		public Token.Type type;

		public double doubleValue;

		public long longValue;

		public bool boolValue;

		public string stringValue;

		public enum Type
		{
			Invalid,
			ObjectBegin,
			ObjectEnd,
			ArrayBegin,
			ArrayEnd,
			Floating,
			Integer,
			String,
			Boolean,
			Null,
			End
		}
	}
}
