using System;
using System.Linq;
using System.Reflection;

namespace ExtendedEnum
{
	public static class ExtendedEnum
	{
		public static bool IsExistAttribute<TEnum>(this Enum self)
		{
			FieldInfo field = self.GetType().GetField(self.ToString());
			return ((field != null) ? field.GetCustomAttribute<EnumKeyValueAttribute>() : null) != null;
		}

		public static TEnum GetValue<TEnum>(this Enum enumValue, int key)
		{
			FieldInfo field = typeof(TEnum).GetField(enumValue.ToString());
			if (field == null)
			{
				return default(TEnum);
			}
			EnumKeyValueAttribute[] array = (field.GetCustomAttributes<EnumKeyValueAttribute>() as EnumKeyValueAttribute[]) ?? null;
			if (array == null || array.Length == 0)
			{
				return default(TEnum);
			}
			EnumKeyValueAttribute enumKeyValueAttribute = array.FirstOrDefault<EnumKeyValueAttribute>((EnumKeyValueAttribute att) => att.Key == key);
			return (TEnum)((object)((enumKeyValueAttribute != null) ? enumKeyValueAttribute.Value : null));
		}
	}
}
