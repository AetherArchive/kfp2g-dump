using System;
using UnityEngine;
using UnityEngine.Profiling;

namespace SGNFW.Common.NativePlugin
{
	// Token: 0x0200026F RID: 623
	public class Memory
	{
		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x0600263B RID: 9787 RVA: 0x001A147E File Offset: 0x0019F67E
		// (set) Token: 0x0600263A RID: 9786 RVA: 0x001A1465 File Offset: 0x0019F665
		public bool disable
		{
			get
			{
				return this.m_Disable;
			}
			set
			{
				if (this.m_Disable != value)
				{
					this.m_Disable = value;
					bool disable = this.m_Disable;
				}
			}
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x001A1486 File Offset: 0x0019F686
		public static int TotalMemory()
		{
			return SystemInfo.systemMemorySize * 1024;
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x001A1493 File Offset: 0x0019F693
		public static int FreeMemory()
		{
			return Memory.TotalMemory() - Memory.UseMemory();
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x001A14A0 File Offset: 0x0019F6A0
		public static int UseMemory()
		{
			return (int)(Profiler.usedHeapSize / 1024U);
		}

		// Token: 0x04001C47 RID: 7239
		private bool m_Disable;
	}
}
