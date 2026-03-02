using System;
using System.Linq;
using System.Reflection;

namespace ExtendedEnum
{
	// Token: 0x02000568 RID: 1384
	public static class ExtendedEnum
	{
		// Token: 0x06002DEE RID: 11758 RVA: 0x001B0D97 File Offset: 0x001AEF97
		public static bool IsExistAttribute<TEnum>(this Enum self)
		{
			FieldInfo field = self.GetType().GetField(self.ToString());
			return ((field != null) ? field.GetCustomAttribute<EnumKeyValueAttribute>() : null) != null;
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x001B0DBC File Offset: 0x001AEFBC
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
