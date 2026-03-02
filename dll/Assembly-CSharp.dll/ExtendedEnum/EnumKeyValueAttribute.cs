using System;

namespace ExtendedEnum
{
	// Token: 0x02000569 RID: 1385
	[AttributeUsage(AttributeTargets.Field)]
	public class EnumKeyValueAttribute : Attribute
	{
		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06002DF0 RID: 11760 RVA: 0x001B0E44 File Offset: 0x001AF044
		// (set) Token: 0x06002DF1 RID: 11761 RVA: 0x001B0E4C File Offset: 0x001AF04C
		public int Key { get; private set; }

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06002DF2 RID: 11762 RVA: 0x001B0E55 File Offset: 0x001AF055
		// (set) Token: 0x06002DF3 RID: 11763 RVA: 0x001B0E5D File Offset: 0x001AF05D
		public object Value { get; private set; }

		// Token: 0x06002DF4 RID: 11764 RVA: 0x001B0E66 File Offset: 0x001AF066
		public EnumKeyValueAttribute(int key, object value)
		{
			this.Key = key;
			this.Value = value;
		}
	}
}
