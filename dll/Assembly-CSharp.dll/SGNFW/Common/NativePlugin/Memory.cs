using System;
using UnityEngine;
using UnityEngine.Profiling;

namespace SGNFW.Common.NativePlugin
{
	public class Memory
	{
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

		public static int TotalMemory()
		{
			return SystemInfo.systemMemorySize * 1024;
		}

		public static int FreeMemory()
		{
			return Memory.TotalMemory() - Memory.UseMemory();
		}

		public static int UseMemory()
		{
			return (int)(Profiler.usedHeapSize / 1024U);
		}

		private bool m_Disable;
	}
}
