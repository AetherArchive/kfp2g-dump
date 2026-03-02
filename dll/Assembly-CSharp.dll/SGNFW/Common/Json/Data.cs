using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SGNFW.Common.Json
{
	// Token: 0x02000273 RID: 627
	public class Data
	{
		// Token: 0x06002644 RID: 9796 RVA: 0x001A14D3 File Offset: 0x0019F6D3
		public static implicit operator Dictionary<string, Data>(Data jsonData)
		{
			return jsonData.Dict;
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x001A14DB File Offset: 0x0019F6DB
		public static implicit operator List<Data>(Data jsonData)
		{
			return jsonData.List;
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x001A14E3 File Offset: 0x0019F6E3
		public static implicit operator string(Data jsonData)
		{
			return jsonData.GetString();
		}

		// Token: 0x06002647 RID: 9799 RVA: 0x001A14EB File Offset: 0x0019F6EB
		public static implicit operator double(Data jsonData)
		{
			return jsonData.GetDouble();
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x001A14F3 File Offset: 0x0019F6F3
		public static implicit operator long(Data jsonData)
		{
			return jsonData.GetLong();
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x001A14FB File Offset: 0x0019F6FB
		public static implicit operator bool(Data jsonData)
		{
			return jsonData.GetBool();
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x001A1503 File Offset: 0x0019F703
		public static implicit operator Data(Dictionary<string, Data> dictionary)
		{
			return new Data(dictionary);
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x001A150B File Offset: 0x0019F70B
		public static implicit operator Data(List<Data> list)
		{
			return new Data(list);
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x001A1513 File Offset: 0x0019F713
		public static implicit operator Data(string stringValue)
		{
			return new Data(stringValue);
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x001A151B File Offset: 0x0019F71B
		public static implicit operator Data(double doubleValue)
		{
			return new Data(doubleValue);
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x001A1523 File Offset: 0x0019F723
		public static implicit operator Data(long longValue)
		{
			return new Data(longValue);
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x001A152B File Offset: 0x0019F72B
		public static implicit operator Data(bool boolValue)
		{
			return new Data(boolValue);
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06002650 RID: 9808 RVA: 0x001A1533 File Offset: 0x0019F733
		public Type JsonType
		{
			get
			{
				return this.jsonType;
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06002651 RID: 9809 RVA: 0x001A153B File Offset: 0x0019F73B
		public Dictionary<string, Data> Dict
		{
			get
			{
				this.AssumeJsonType(Type.Dictionary);
				return this.dict;
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06002652 RID: 9810 RVA: 0x001A154A File Offset: 0x0019F74A
		public List<Data> List
		{
			get
			{
				this.AssumeJsonType(Type.Array);
				return this.list;
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06002653 RID: 9811 RVA: 0x001A1559 File Offset: 0x0019F759
		// (set) Token: 0x06002654 RID: 9812 RVA: 0x001A1561 File Offset: 0x0019F761
		public bool Bool
		{
			get
			{
				return this.GetBool();
			}
			set
			{
				this.SetBool(value);
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06002655 RID: 9813 RVA: 0x001A156B File Offset: 0x0019F76B
		// (set) Token: 0x06002656 RID: 9814 RVA: 0x001A1574 File Offset: 0x0019F774
		public sbyte Sbyte
		{
			get
			{
				return (sbyte)this.GetLong();
			}
			set
			{
				this.SetLong((long)value);
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06002657 RID: 9815 RVA: 0x001A157F File Offset: 0x0019F77F
		// (set) Token: 0x06002658 RID: 9816 RVA: 0x001A1588 File Offset: 0x0019F788
		public short Short
		{
			get
			{
				return (short)this.GetLong();
			}
			set
			{
				this.SetLong((long)value);
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06002659 RID: 9817 RVA: 0x001A1593 File Offset: 0x0019F793
		// (set) Token: 0x0600265A RID: 9818 RVA: 0x001A159C File Offset: 0x0019F79C
		public int Int
		{
			get
			{
				return (int)this.GetLong();
			}
			set
			{
				this.SetLong((long)value);
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x0600265B RID: 9819 RVA: 0x001A15A7 File Offset: 0x0019F7A7
		// (set) Token: 0x0600265C RID: 9820 RVA: 0x001A15AF File Offset: 0x0019F7AF
		public long Long
		{
			get
			{
				return this.GetLong();
			}
			set
			{
				this.SetLong(value);
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x0600265D RID: 9821 RVA: 0x001A15B9 File Offset: 0x0019F7B9
		// (set) Token: 0x0600265E RID: 9822 RVA: 0x001A15C2 File Offset: 0x0019F7C2
		public float Float
		{
			get
			{
				return (float)this.GetDouble();
			}
			set
			{
				this.SetDouble((double)value);
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x0600265F RID: 9823 RVA: 0x001A15CD File Offset: 0x0019F7CD
		// (set) Token: 0x06002660 RID: 9824 RVA: 0x001A15D5 File Offset: 0x0019F7D5
		public double Double
		{
			get
			{
				return this.GetDouble();
			}
			set
			{
				this.SetDouble(value);
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06002661 RID: 9825 RVA: 0x001A15DF File Offset: 0x0019F7DF
		// (set) Token: 0x06002662 RID: 9826 RVA: 0x001A15E8 File Offset: 0x0019F7E8
		public byte Byte
		{
			get
			{
				return (byte)this.GetLong();
			}
			set
			{
				this.SetLong((long)((ulong)value));
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06002663 RID: 9827 RVA: 0x001A15F3 File Offset: 0x0019F7F3
		// (set) Token: 0x06002664 RID: 9828 RVA: 0x001A15FC File Offset: 0x0019F7FC
		public ushort Ushort
		{
			get
			{
				return (ushort)this.GetLong();
			}
			set
			{
				this.SetLong((long)((ulong)value));
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06002665 RID: 9829 RVA: 0x001A1607 File Offset: 0x0019F807
		// (set) Token: 0x06002666 RID: 9830 RVA: 0x001A1610 File Offset: 0x0019F810
		public uint Uint
		{
			get
			{
				return (uint)this.GetLong();
			}
			set
			{
				this.SetLong((long)((ulong)value));
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06002667 RID: 9831 RVA: 0x001A161B File Offset: 0x0019F81B
		// (set) Token: 0x06002668 RID: 9832 RVA: 0x001A1623 File Offset: 0x0019F823
		public ulong Ulong
		{
			get
			{
				return (ulong)this.GetLong();
			}
			set
			{
				this.SetLong((long)value);
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06002669 RID: 9833 RVA: 0x001A162D File Offset: 0x0019F82D
		// (set) Token: 0x0600266A RID: 9834 RVA: 0x001A1636 File Offset: 0x0019F836
		public char Char
		{
			get
			{
				return (char)this.GetLong();
			}
			set
			{
				this.SetLong((long)((ulong)value));
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x0600266B RID: 9835 RVA: 0x001A1641 File Offset: 0x0019F841
		// (set) Token: 0x0600266C RID: 9836 RVA: 0x001A1649 File Offset: 0x0019F849
		public string String
		{
			get
			{
				return this.GetString();
			}
			set
			{
				this.SetString(value);
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x0600266D RID: 9837 RVA: 0x001A1653 File Offset: 0x0019F853
		public int Count
		{
			get
			{
				if (this.jsonType == Type.Dictionary)
				{
					return this.dict.Count;
				}
				if (this.jsonType == Type.Array)
				{
					return this.list.Count;
				}
				return 0;
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x0600266E RID: 9838 RVA: 0x001A1680 File Offset: 0x0019F880
		public int Length
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x170005B8 RID: 1464
		public Data this[string key]
		{
			get
			{
				this.AssumeJsonType(Type.Dictionary);
				Data data;
				if (!this.Dict.TryGetValue(key, out data))
				{
					data = new Data();
					this.Dict.Add(key, data);
				}
				return data;
			}
			set
			{
				this.AssumeJsonType(Type.Dictionary);
				this.Dict[key] = value;
			}
		}

		// Token: 0x170005B9 RID: 1465
		public Data this[int index]
		{
			get
			{
				this.AssumeJsonType(Type.Array);
				while (this.list.Count <= index)
				{
					this.list.Add(new Data());
				}
				return this.list[index];
			}
			set
			{
				this.AssumeJsonType(Type.Array);
				while (this.list.Count <= index)
				{
					this.list.Add(new Data());
				}
				this.list[index] = value;
			}
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x001A1741 File Offset: 0x0019F941
		public Data()
		{
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x001A1750 File Offset: 0x0019F950
		public Data(Type _jsonType)
		{
			this.AssumeJsonType(_jsonType);
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x001A1766 File Offset: 0x0019F966
		public Data(Dictionary<string, Data> _dict)
		{
			this.AssumeJsonType(Type.Dictionary);
			this.dict = _dict;
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x001A1783 File Offset: 0x0019F983
		public Data(List<Data> _list)
		{
			this.AssumeJsonType(Type.Array);
			this.list = _list;
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x001A17A0 File Offset: 0x0019F9A0
		public Data(string _string)
		{
			this.AssumeJsonType(Type.String);
			this.stringValue = _string;
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x001A17BD File Offset: 0x0019F9BD
		public Data(double _double)
		{
			this.AssumeJsonType(Type.Floating);
			this.doubleValue = _double;
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x001A17DA File Offset: 0x0019F9DA
		public Data(long _long)
		{
			this.AssumeJsonType(Type.Integer);
			this.longValue = _long;
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x001A17F7 File Offset: 0x0019F9F7
		public Data(bool _bool)
		{
			this.AssumeJsonType(Type.Boolean);
			this.boolValue = _bool;
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x001A1814 File Offset: 0x0019FA14
		public static T ToObject<T>(string jsonText)
		{
			return (T)((object)Data.ToObject(Data.CreateFromJsonText(jsonText), typeof(T)));
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x001A1830 File Offset: 0x0019FA30
		public static T ToObject<T>(Data data)
		{
			return (T)((object)Data.ToObject(data, typeof(T)));
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x001A1847 File Offset: 0x0019FA47
		public static Data CreateFromJsonText(string jsonText)
		{
			return new Maker().Make(jsonText);
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x001A1854 File Offset: 0x0019FA54
		public static Data CreateDictionary()
		{
			return new Data(Type.Dictionary);
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x001A185C File Offset: 0x0019FA5C
		public static Data CreateArray()
		{
			return new Data(Type.Array);
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x001A1864 File Offset: 0x0019FA64
		public static bool IsNullOrInvalid(Data _data)
		{
			return _data == null || _data.jsonType == Type.Invalid;
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x001A1874 File Offset: 0x0019FA74
		public Data Find(string name)
		{
			this.AssumeJsonType(Type.Dictionary);
			Data data;
			if (!this.Dict.TryGetValue(name, out data))
			{
				return null;
			}
			return data;
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x001A189C File Offset: 0x0019FA9C
		public Data Pickout(string name)
		{
			this.AssumeJsonType(Type.Dictionary);
			Data data;
			if (!this.Dict.TryGetValue(name, out data))
			{
				return null;
			}
			this.Dict.Remove(name);
			return data;
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x001A18D0 File Offset: 0x0019FAD0
		public string GetString()
		{
			switch (this.jsonType)
			{
			case Type.Invalid:
				return "";
			case Type.Null:
				return "null";
			case Type.String:
				return this.stringValue;
			case Type.Floating:
				return this.doubleValue.ToString();
			case Type.Integer:
				return this.longValue.ToString();
			case Type.Boolean:
				if (!this.boolValue)
				{
					return "false";
				}
				return "true";
			}
			throw new Exception("Json Type Mismatch");
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x001A1958 File Offset: 0x0019FB58
		public double GetDouble()
		{
			switch (this.jsonType)
			{
			case Type.Invalid:
				return 0.0;
			case Type.Null:
				return 0.0;
			case Type.String:
			{
				double num;
				if (!double.TryParse(this.stringValue, out num))
				{
					return 0.0;
				}
				return num;
			}
			case Type.Floating:
				return this.doubleValue;
			case Type.Integer:
				return (double)this.longValue;
			case Type.Boolean:
				if (!this.boolValue)
				{
					return 0.0;
				}
				return 1.0;
			}
			throw new Exception("Json Type Mismatch");
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x001A19FC File Offset: 0x0019FBFC
		public long GetLong()
		{
			switch (this.jsonType)
			{
			case Type.Invalid:
				return 0L;
			case Type.Null:
				return 0L;
			case Type.String:
			{
				long num;
				if (!long.TryParse(this.stringValue, out num))
				{
					return 0L;
				}
				return num;
			}
			case Type.Floating:
				return (long)this.doubleValue;
			case Type.Integer:
				return this.longValue;
			case Type.Boolean:
				return this.boolValue ? 1L : 0L;
			}
			throw new Exception("Json Type Mismatch");
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x001A1A7C File Offset: 0x0019FC7C
		public bool GetBool()
		{
			switch (this.jsonType)
			{
			case Type.Invalid:
				return false;
			case Type.Null:
				return false;
			case Type.String:
			{
				bool flag;
				return bool.TryParse(this.stringValue, out flag) && flag;
			}
			case Type.Floating:
				return this.doubleValue != 0.0;
			case Type.Integer:
				return this.longValue != 0L;
			case Type.Boolean:
				return this.boolValue;
			}
			throw new Exception("Json Type Mismatch");
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x001A1B04 File Offset: 0x0019FD04
		public TEnum GetEnum<TEnum>() where TEnum : struct
		{
			if (this.jsonType == Type.String)
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

		// Token: 0x06002688 RID: 9864 RVA: 0x001A1B7C File Offset: 0x0019FD7C
		public Data SetString(string stringValue)
		{
			this.jsonType = Type.String;
			this.stringValue = stringValue;
			return this;
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x001A1B8D File Offset: 0x0019FD8D
		public Data SetDouble(double doubleValue)
		{
			this.jsonType = Type.Floating;
			this.doubleValue = doubleValue;
			return this;
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x001A1B9E File Offset: 0x0019FD9E
		public Data SetLong(long longValue)
		{
			this.jsonType = Type.Integer;
			this.longValue = longValue;
			return this;
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x001A1BAF File Offset: 0x0019FDAF
		public Data SetBool(bool boolValue)
		{
			this.jsonType = Type.Boolean;
			this.boolValue = boolValue;
			return this;
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x001A1BC0 File Offset: 0x0019FDC0
		public Data SetNull()
		{
			this.jsonType = Type.Null;
			return this;
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x001A1BCA File Offset: 0x0019FDCA
		public Data Add(string name, Data data)
		{
			this.AssumeJsonType(Type.Dictionary);
			this.Dict[name] = data;
			return this;
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x001A1BE1 File Offset: 0x0019FDE1
		public Data Add(Data data)
		{
			this.AssumeJsonType(Type.Array);
			this.list.Add(data);
			return this;
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x001A1BF8 File Offset: 0x0019FDF8
		private void AssumeJsonType(Type type)
		{
			if (this.jsonType == Type.Invalid)
			{
				this.jsonType = type;
				if (this.jsonType == Type.Dictionary && this.dict == null)
				{
					this.dict = new Dictionary<string, Data>();
				}
				if (this.jsonType == Type.Array && this.list == null)
				{
					this.list = new List<Data>();
				}
				return;
			}
			if (this.jsonType != type)
			{
				throw new Exception("Json Type Mismatch");
			}
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x001A1C64 File Offset: 0x0019FE64
		private static object ToObject(Data data, Type type)
		{
			object obj = null;
			if (data != null)
			{
				switch (data.JsonType)
				{
				case Type.Dictionary:
					if (type.IsSubclassOf(typeof(ScriptableObject)))
					{
						obj = ScriptableObject.CreateInstance(type);
					}
					else
					{
						obj = Activator.CreateInstance(type);
					}
					Data.Load(obj, data);
					break;
				case Type.Array:
					if (type.IsSubclassOf(typeof(Array)))
					{
						Type elementType = type.GetElementType();
						Array array = Array.CreateInstance(elementType, data.Length);
						for (int i = 0; i < data.Length; i++)
						{
							object obj2;
							if (elementType.IsArray || elementType.IsValueType || elementType == typeof(string))
							{
								obj2 = Data.ToObject(data.list[i], elementType);
							}
							else
							{
								obj2 = Activator.CreateInstance(elementType);
								Data.Load(obj2, data.list[i]);
							}
							array.SetValue(obj2, i);
						}
						obj = array;
					}
					else if (type.IsSubclassOf(typeof(IList)))
					{
						Type elementType2 = type.GetElementType();
						IList list = Activator.CreateInstance(type) as IList;
						for (int j = 0; j < data.Length; j++)
						{
							object obj3;
							if (elementType2.IsArray || elementType2.IsValueType || elementType2 == typeof(string))
							{
								obj3 = Data.ToObject(data.list[j], elementType2);
							}
							else
							{
								obj3 = Activator.CreateInstance(elementType2);
								Data.Load(obj3, data.list[j]);
							}
							list.Add(obj3);
						}
						obj = list;
					}
					break;
				case Type.String:
					if (type.IsEnum)
					{
						obj = Enum.Parse(type, data.String);
					}
					else if (type == typeof(string))
					{
						obj = data.String;
					}
					else if (type == typeof(double))
					{
						obj = data.Double;
					}
					else if (type == typeof(float))
					{
						obj = data.Float;
					}
					else if (type == typeof(bool))
					{
						obj = data.Bool;
					}
					else if (type == typeof(long))
					{
						obj = data.Long;
					}
					else if (type.IsValueType)
					{
						obj = data.Int;
					}
					else
					{
						obj = Data.ToObject(Data.CreateFromJsonText(data.String), type);
					}
					break;
				case Type.Floating:
					if (type == typeof(double))
					{
						obj = data.Double;
					}
					else
					{
						obj = data.Float;
					}
					break;
				case Type.Integer:
					if (type == typeof(long))
					{
						obj = data.Long;
					}
					else
					{
						obj = data.Int;
					}
					break;
				case Type.Boolean:
					obj = data.Bool;
					break;
				}
			}
			return obj;
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x001A1F9C File Offset: 0x001A019C
		private static void Load(object obj, Data data)
		{
			if (data != null && data.JsonType == Type.Dictionary)
			{
				Type type = obj.GetType();
				foreach (KeyValuePair<string, Data> keyValuePair in data.Dict)
				{
					FieldInfo field = type.GetField(keyValuePair.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (field != null)
					{
						field.SetValue(obj, Data.ToObject(keyValuePair.Value, field.FieldType));
					}
					else
					{
						string[] array = new string[5];
						array[0] = "<color=red>[Json Parse Error] : <";
						int num = 1;
						Type type2 = type;
						array[num] = ((type2 != null) ? type2.ToString() : null);
						array[2] = "> Class Key Not Found -> <";
						array[3] = keyValuePair.Key;
						array[4] = "></color>";
						Verbose<Verbose>.LogError(string.Concat(array), null);
					}
				}
			}
		}

		// Token: 0x04001C51 RID: 7249
		private Type jsonType = Type.Invalid;

		// Token: 0x04001C52 RID: 7250
		private Dictionary<string, Data> dict;

		// Token: 0x04001C53 RID: 7251
		private List<Data> list;

		// Token: 0x04001C54 RID: 7252
		private string stringValue;

		// Token: 0x04001C55 RID: 7253
		private double doubleValue;

		// Token: 0x04001C56 RID: 7254
		private long longValue;

		// Token: 0x04001C57 RID: 7255
		private bool boolValue;
	}
}
