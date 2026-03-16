using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SGNFW.Common.Json
{
	public class Data
	{
		public static implicit operator Dictionary<string, Data>(Data jsonData)
		{
			return jsonData.Dict;
		}

		public static implicit operator List<Data>(Data jsonData)
		{
			return jsonData.List;
		}

		public static implicit operator string(Data jsonData)
		{
			return jsonData.GetString();
		}

		public static implicit operator double(Data jsonData)
		{
			return jsonData.GetDouble();
		}

		public static implicit operator long(Data jsonData)
		{
			return jsonData.GetLong();
		}

		public static implicit operator bool(Data jsonData)
		{
			return jsonData.GetBool();
		}

		public static implicit operator Data(Dictionary<string, Data> dictionary)
		{
			return new Data(dictionary);
		}

		public static implicit operator Data(List<Data> list)
		{
			return new Data(list);
		}

		public static implicit operator Data(string stringValue)
		{
			return new Data(stringValue);
		}

		public static implicit operator Data(double doubleValue)
		{
			return new Data(doubleValue);
		}

		public static implicit operator Data(long longValue)
		{
			return new Data(longValue);
		}

		public static implicit operator Data(bool boolValue)
		{
			return new Data(boolValue);
		}

		public Type JsonType
		{
			get
			{
				return this.jsonType;
			}
		}

		public Dictionary<string, Data> Dict
		{
			get
			{
				this.AssumeJsonType(Type.Dictionary);
				return this.dict;
			}
		}

		public List<Data> List
		{
			get
			{
				this.AssumeJsonType(Type.Array);
				return this.list;
			}
		}

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

		public int Length
		{
			get
			{
				return this.Count;
			}
		}

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

		public Data()
		{
		}

		public Data(Type _jsonType)
		{
			this.AssumeJsonType(_jsonType);
		}

		public Data(Dictionary<string, Data> _dict)
		{
			this.AssumeJsonType(Type.Dictionary);
			this.dict = _dict;
		}

		public Data(List<Data> _list)
		{
			this.AssumeJsonType(Type.Array);
			this.list = _list;
		}

		public Data(string _string)
		{
			this.AssumeJsonType(Type.String);
			this.stringValue = _string;
		}

		public Data(double _double)
		{
			this.AssumeJsonType(Type.Floating);
			this.doubleValue = _double;
		}

		public Data(long _long)
		{
			this.AssumeJsonType(Type.Integer);
			this.longValue = _long;
		}

		public Data(bool _bool)
		{
			this.AssumeJsonType(Type.Boolean);
			this.boolValue = _bool;
		}

		public static T ToObject<T>(string jsonText)
		{
			return (T)((object)Data.ToObject(Data.CreateFromJsonText(jsonText), typeof(T)));
		}

		public static T ToObject<T>(Data data)
		{
			return (T)((object)Data.ToObject(data, typeof(T)));
		}

		public static Data CreateFromJsonText(string jsonText)
		{
			return new Maker().Make(jsonText);
		}

		public static Data CreateDictionary()
		{
			return new Data(Type.Dictionary);
		}

		public static Data CreateArray()
		{
			return new Data(Type.Array);
		}

		public static bool IsNullOrInvalid(Data _data)
		{
			return _data == null || _data.jsonType == Type.Invalid;
		}

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

		public Data SetString(string stringValue)
		{
			this.jsonType = Type.String;
			this.stringValue = stringValue;
			return this;
		}

		public Data SetDouble(double doubleValue)
		{
			this.jsonType = Type.Floating;
			this.doubleValue = doubleValue;
			return this;
		}

		public Data SetLong(long longValue)
		{
			this.jsonType = Type.Integer;
			this.longValue = longValue;
			return this;
		}

		public Data SetBool(bool boolValue)
		{
			this.jsonType = Type.Boolean;
			this.boolValue = boolValue;
			return this;
		}

		public Data SetNull()
		{
			this.jsonType = Type.Null;
			return this;
		}

		public Data Add(string name, Data data)
		{
			this.AssumeJsonType(Type.Dictionary);
			this.Dict[name] = data;
			return this;
		}

		public Data Add(Data data)
		{
			this.AssumeJsonType(Type.Array);
			this.list.Add(data);
			return this;
		}

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

		private Type jsonType = Type.Invalid;

		private Dictionary<string, Data> dict;

		private List<Data> list;

		private string stringValue;

		private double doubleValue;

		private long longValue;

		private bool boolValue;
	}
}
