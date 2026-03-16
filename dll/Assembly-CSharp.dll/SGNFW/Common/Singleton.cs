using System;
using UnityEngine;

namespace SGNFW.Common
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		public static T Instance
		{
			get
			{
				return Singleton<T>.instance;
			}
		}

		protected virtual void OnSingletonAwake()
		{
		}

		protected virtual void OnSingletonDestroy()
		{
		}

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

		private void OnDestroy()
		{
			if (Singleton<T>.instance == this)
			{
				this.OnSingletonDestroy();
				Singleton<T>.instance = default(T);
			}
		}

		private static T instance;
	}
}
