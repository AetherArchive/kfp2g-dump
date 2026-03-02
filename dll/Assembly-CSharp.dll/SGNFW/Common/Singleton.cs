using System;
using UnityEngine;

namespace SGNFW.Common
{
	// Token: 0x0200025B RID: 603
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x060025BF RID: 9663 RVA: 0x001A053E File Offset: 0x0019E73E
		public static T Instance
		{
			get
			{
				return Singleton<T>.instance;
			}
		}

		// Token: 0x060025C0 RID: 9664 RVA: 0x001A0545 File Offset: 0x0019E745
		protected virtual void OnSingletonAwake()
		{
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x001A0547 File Offset: 0x0019E747
		protected virtual void OnSingletonDestroy()
		{
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x001A054C File Offset: 0x0019E74C
		private void Awake()
		{
			if (Singleton<T>.instance != null)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			Singleton<T>.instance = this as T;
			if (Singleton<T>.instance == null)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			this.OnSingletonAwake();
			if (base.transform.parent == null)
			{
				Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		// Token: 0x060025C3 RID: 9667 RVA: 0x001A05C9 File Offset: 0x0019E7C9
		private void OnDestroy()
		{
			if (Singleton<T>.instance == this)
			{
				this.OnSingletonDestroy();
				Singleton<T>.instance = default(T);
			}
		}

		// Token: 0x04001BD0 RID: 7120
		private static T instance;
	}
}
