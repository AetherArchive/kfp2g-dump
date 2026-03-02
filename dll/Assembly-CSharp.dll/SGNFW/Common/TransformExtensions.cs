using System;
using UnityEngine;

namespace SGNFW.Common
{
	// Token: 0x0200025E RID: 606
	public static class TransformExtensions
	{
		// Token: 0x060025D7 RID: 9687 RVA: 0x001A0A58 File Offset: 0x0019EC58
		public static void ResetLocalTransform(this Transform self)
		{
			self.localPosition = Vector3.zero;
			self.localRotation = Quaternion.identity;
			self.localScale = Vector3.one;
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x001A0A7B File Offset: 0x0019EC7B
		public static bool HasChild(this Transform self)
		{
			return self.childCount > 0;
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x001A0A86 File Offset: 0x0019EC86
		public static void SetLocalPositionX(this Transform self, float x)
		{
			self.SetLocalPosition(x, self.localPosition.y, self.localPosition.z);
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x001A0AA5 File Offset: 0x0019ECA5
		public static void SetLocalPositionY(this Transform self, float y)
		{
			self.SetLocalPosition(self.localPosition.x, y, self.localPosition.z);
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x001A0AC4 File Offset: 0x0019ECC4
		public static void SetLocalPositionZ(this Transform self, float z)
		{
			self.SetLocalPosition(self.localPosition.x, self.localPosition.y, z);
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x001A0AE3 File Offset: 0x0019ECE3
		public static void SetLocalPosition(this Transform self, float x, float y, float z)
		{
			self.localPosition = new Vector3(x, y, z);
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x001A0AF3 File Offset: 0x0019ECF3
		public static void SetLocalTransform(this Transform self, Transform transform)
		{
			self.localPosition = transform.localPosition;
			self.localScale = transform.localScale;
			self.localRotation = transform.localRotation;
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x001A0B19 File Offset: 0x0019ED19
		public static void SetPositionX(this Transform self, float x)
		{
			self.SetPosition(x, self.position.y, self.position.z);
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x001A0B38 File Offset: 0x0019ED38
		public static void SetPositionY(this Transform self, float y)
		{
			self.SetPosition(self.position.x, y, self.position.z);
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x001A0B57 File Offset: 0x0019ED57
		public static void SetPositionZ(this Transform self, float z)
		{
			self.SetPosition(self.position.x, self.position.y, z);
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x001A0B76 File Offset: 0x0019ED76
		public static void SetPosition(this Transform self, float x, float y, float z)
		{
			self.position = new Vector3(x, y, z);
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x001A0B86 File Offset: 0x0019ED86
		public static bool IsRoot(this Transform self)
		{
			return self.parent == null;
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x001A0B94 File Offset: 0x0019ED94
		public static Transform GetRootTransform(this Transform self)
		{
			if (self.IsRoot())
			{
				return self;
			}
			return self.parent.GetRootTransform();
		}
	}
}
