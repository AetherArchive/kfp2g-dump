using System;
using UnityEngine;

namespace SGNFW.Common
{
	public static class TransformExtensions
	{
		public static void ResetLocalTransform(this Transform self)
		{
			self.localPosition = Vector3.zero;
			self.localRotation = Quaternion.identity;
			self.localScale = Vector3.one;
		}

		public static bool HasChild(this Transform self)
		{
			return self.childCount > 0;
		}

		public static void SetLocalPositionX(this Transform self, float x)
		{
			self.SetLocalPosition(x, self.localPosition.y, self.localPosition.z);
		}

		public static void SetLocalPositionY(this Transform self, float y)
		{
			self.SetLocalPosition(self.localPosition.x, y, self.localPosition.z);
		}

		public static void SetLocalPositionZ(this Transform self, float z)
		{
			self.SetLocalPosition(self.localPosition.x, self.localPosition.y, z);
		}

		public static void SetLocalPosition(this Transform self, float x, float y, float z)
		{
			self.localPosition = new Vector3(x, y, z);
		}

		public static void SetLocalTransform(this Transform self, Transform transform)
		{
			self.localPosition = transform.localPosition;
			self.localScale = transform.localScale;
			self.localRotation = transform.localRotation;
		}

		public static void SetPositionX(this Transform self, float x)
		{
			self.SetPosition(x, self.position.y, self.position.z);
		}

		public static void SetPositionY(this Transform self, float y)
		{
			self.SetPosition(self.position.x, y, self.position.z);
		}

		public static void SetPositionZ(this Transform self, float z)
		{
			self.SetPosition(self.position.x, self.position.y, z);
		}

		public static void SetPosition(this Transform self, float x, float y, float z)
		{
			self.position = new Vector3(x, y, z);
		}

		public static bool IsRoot(this Transform self)
		{
			return self.parent == null;
		}

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
