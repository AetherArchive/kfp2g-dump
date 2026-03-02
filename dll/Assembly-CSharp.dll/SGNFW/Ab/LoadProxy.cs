using System;
using UnityEngine;

namespace SGNFW.Ab
{
	// Token: 0x02000285 RID: 645
	public class LoadProxy : MonoBehaviour
	{
		// Token: 0x06002723 RID: 10019 RVA: 0x001A4D36 File Offset: 0x001A2F36
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

		// Token: 0x06002724 RID: 10020 RVA: 0x001A4D68 File Offset: 0x001A2F68
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

		// Token: 0x06002725 RID: 10021 RVA: 0x001A4DBD File Offset: 0x001A2FBD
		public void ClearLoadedCallback()
		{
			this.onLoaded = null;
			if (this.next != null)
			{
				this.next.ClearLoadedCallback();
			}
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x001A4DE0 File Offset: 0x001A2FE0
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

		// Token: 0x06002727 RID: 10023 RVA: 0x001A4E5B File Offset: 0x001A305B
		private void OnDestroy()
		{
			this.Remove();
		}

		// Token: 0x04001CA7 RID: 7335
		public Action<Object, LoadProxy> onLoaded;

		// Token: 0x04001CA8 RID: 7336
		public LoadProxy.Wrap wrap;

		// Token: 0x04001CA9 RID: 7337
		protected LoadProxy prev;

		// Token: 0x04001CAA RID: 7338
		protected LoadProxy next;

		// Token: 0x020010B1 RID: 4273
		public enum State
		{
			// Token: 0x04005C9B RID: 23707
			None,
			// Token: 0x04005C9C RID: 23708
			Running
		}

		// Token: 0x020010B2 RID: 4274
		public class Wrap
		{
			// Token: 0x04005C9D RID: 23709
			public Object obj;

			// Token: 0x04005C9E RID: 23710
			public LoadProxy proxy;

			// Token: 0x04005C9F RID: 23711
			public LoadProxy.State state;
		}
	}
}
