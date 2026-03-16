using System;
using UnityEngine;

namespace Project.Lib
{
	[DefaultExecutionOrder(-1)]
	public class VirtualScreenUtil : MonoBehaviour
	{
		private void Awake()
		{
			VirtualScreenUtil.instance_ = this;
			float num = (float)this.activeHeight(this.constraint, this.manualWidth, this.manualHeight);
			this.ResizeTransformScale(num);
			VirtualScreenUtil.Pixel2Screen.x = (float)VirtualScreenUtil.ScreenWidth / (float)Screen.width;
			VirtualScreenUtil.Pixel2Screen.y = (float)VirtualScreenUtil.ScreenHeight / (float)Screen.height;
			VirtualScreenUtil.Screen2Pixel.x = (float)Screen.width / (float)VirtualScreenUtil.ScreenWidth;
			VirtualScreenUtil.Screen2Pixel.y = (float)Screen.height / (float)VirtualScreenUtil.ScreenHeight;
			this.enable_ = true;
		}

		private void ResizeTransformScale(float calcActiveHeight)
		{
			if (calcActiveHeight > 0f)
			{
				float num = 2f / calcActiveHeight;
				Vector3 localScale = base.transform.localScale;
				if (Mathf.Abs(localScale.x - num) > 1E-45f || Mathf.Abs(localScale.y - num) > 1E-45f || Mathf.Abs(localScale.z - num) > 1E-45f)
				{
					base.transform.localScale = new Vector3(num, num, num);
				}
			}
		}

		private int activeHeight(VirtualScreenUtil.Constraint cons, int width, int height)
		{
			if (cons == VirtualScreenUtil.Constraint.FitHeight)
			{
				return height;
			}
			Vector2 screenSize = VirtualScreenUtil.screenSize;
			float num = screenSize.x / screenSize.y;
			float num2 = (float)width / (float)height;
			switch (cons)
			{
			case VirtualScreenUtil.Constraint.Fit:
				if (num2 <= num)
				{
					return height;
				}
				return Mathf.RoundToInt((float)width / num);
			case VirtualScreenUtil.Constraint.Fill:
				if (num2 >= num)
				{
					return height;
				}
				return Mathf.RoundToInt((float)width / num);
			case VirtualScreenUtil.Constraint.FitWidth:
				return Mathf.RoundToInt((float)width / num);
			default:
				return height;
			}
		}

		private static Vector2 screenSize
		{
			get
			{
				return new Vector2((float)Screen.width, (float)Screen.height);
			}
		}

		public static bool IsValid()
		{
			return VirtualScreenUtil.instance_ != null && VirtualScreenUtil.instance_.enable_;
		}

		public static int ScreenWidth
		{
			get
			{
				return (int)((float)Screen.width * ((float)VirtualScreenUtil.ScreenHeight / (float)Screen.height));
			}
		}

		public static int ScreenHeight
		{
			get
			{
				return VirtualScreenUtil.instance_.manualHeight;
			}
		}

		public static Vector2 PixelToScreenPos(Vector2 pos)
		{
			pos.Set(pos.x * VirtualScreenUtil.Pixel2Screen.x - 0.5f * (float)VirtualScreenUtil.ScreenWidth, pos.y * VirtualScreenUtil.Pixel2Screen.y - 0.5f * (float)VirtualScreenUtil.ScreenHeight);
			return pos;
		}

		public static Vector2 PixelToScreenDelta(Vector2 delta)
		{
			delta.Set(delta.x * VirtualScreenUtil.Pixel2Screen.x, delta.y * VirtualScreenUtil.Pixel2Screen.y);
			return delta;
		}

		public static Vector2 Pixel2Screen;

		public static Vector2 Screen2Pixel;

		[SerializeField]
		private int manualWidth = 1280;

		[SerializeField]
		private int manualHeight = 1136;

		[SerializeField]
		private VirtualScreenUtil.Constraint constraint = VirtualScreenUtil.Constraint.FitHeight;

		private bool enable_;

		private static VirtualScreenUtil instance_;

		public enum Constraint
		{
			Fit,
			Fill,
			FitWidth,
			FitHeight
		}
	}
}
