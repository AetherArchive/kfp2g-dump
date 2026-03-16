using System;
using UnityEngine;

namespace SGNFW.Ab
{
	public class LoadProxy : MonoBehaviour
	{
		public void Complete(Object obj)
		{
			if (this.onLoaded != null)
			{
				this.onLoaded(obj, this);
			}
			if (this.next != null)
			{
				this.next.Complete(obj);
			}
		}

		public void Insert(LoadProxy.Wrap root)
		{
			this.Remove();
			if (root.proxy != null)
			{
				this.next = root.proxy;
				if (this.next != null)
				{
					this.next.prev = this;
				}
			}
			root.proxy = this;
			this.wrap = root;
		}

		public void ClearLoadedCallback()
		{
			this.onLoaded = null;
			if (this.next != null)
			{
				this.next.ClearLoadedCallback();
			}
		}

		protected void Remove()
		{
			if (this.prev != null)
			{
				this.prev.next = this.next;
			}
			else if (this.wrap != null)
			{
				this.wrap.proxy = this.next;
				this.wrap = null;
			}
			if (this.next != null)
			{
				this.next.prev = this.prev;
			}
			this.prev = null;
			this.next = null;
		}

		private void OnDestroy()
		{
			this.Remove();
		}

		public Action<Object, LoadProxy> onLoaded;

		public LoadProxy.Wrap wrap;

		protected LoadProxy prev;

		protected LoadProxy next;

		public enum State
		{
			None,
			Running
		}

		public class Wrap
		{
			public Object obj;

			public LoadProxy proxy;

			public LoadProxy.State state;
		}
	}
}
