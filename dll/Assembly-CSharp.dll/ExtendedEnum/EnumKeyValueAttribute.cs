using System;

namespace ExtendedEnum
{
	[AttributeUsage(AttributeTargets.Field)]
	public class EnumKeyValueAttribute : Attribute
	{
		public int Key { get; private set; }

		public object Value { get; private set; }

		public EnumKeyValueAttribute(int key, object value)
		{
			this.Key = key;
			this.Value = value;
		}
	}
}
