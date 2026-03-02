using System;
using UnityEngine;

namespace Project.Lib
{
	// Token: 0x0200056C RID: 1388
	[DefaultExecutionOrder(-1)]
	public class VirtualScreenUtil : MonoBehaviour
	{
		// Token: 0x06002DFB RID: 11771 RVA: 0x001B0F2C File Offset: 0x001AF12C
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

		// Token: 0x06002DFC RID: 11772 RVA: 0x001B0FC4 File Offset: 0x001AF1C4
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

		// Token: 0x06002DFD RID: 11773 RVA: 0x001B103C File Offset: 0x001AF23C
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

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06002DFE RID: 11774 RVA: 0x001B10A8 File Offset: 0x001AF2A8
		private static Vector2 screenSize
		{
			get
			{
				return new Vector2((float)Screen.width, (float)Screen.height);
			}
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x001B10BB File Offset: 0x001AF2BB
		public static bool IsValid()
		{
			return VirtualScreenUtil.instance_ != null && VirtualScreenUtil.instance_.enable_;
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06002E00 RID: 11776 RVA: 0x001B10D6 File Offset: 0x001AF2D6
		public static int ScreenWidth
		{
			get
			{
				return (int)((float)Screen.width * ((float)VirtualScreenUtil.ScreenHeight / (float)Screen.height));
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06002E01 RID: 11777 RVA: 0x001B10ED File Offset: 0x001AF2ED
		public static int ScreenHeight
		{
			get
			{
				return VirtualScreenUtil.instance_.manualHeight;
			}
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x001B10FC File Offset: 0x001AF2FC
		public static Vector2 PixelToScreenPos(Vector2 pos)
		{
			pos.Set(pos.x * VirtualScreenUtil.Pixel2Screen.x - 0.5f * (float)VirtualScreenUtil.ScreenWidth, pos.y * VirtualScreenUtil.Pixel2Screen.y - 0.5f * (float)VirtualScreenUtil.ScreenHeight);
			return pos;
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x001B114D File Offset: 0x001AF34D
		public static Vector2 PixelToScreenDelta(Vector2 delta)
		{
			delta.Set(delta.x * VirtualScreenUtil.Pixel2Screen.x, delta.y * VirtualScreenUtil.Pixel2Screen.y);
			return delta;
		}

		// Token: 0x04002887 RID: 10375
		public static Vector2 Pixel2Screen;

		// Token: 0x04002888 RID: 10376
		public static Vector2 Screen2Pixel;

		// Token: 0x04002889 RID: 10377
		[SerializeField]
		private int manualWidth = 1280;

		// Token: 0x0400288A RID: 10378
		[SerializeField]
		private int manualHeight = 1136;

		// Token: 0x0400288B RID: 10379
		[SerializeField]
		private VirtualScreenUtil.Constraint constraint = VirtualScreenUtil.Constraint.FitHeight;

		// Token: 0x0400288C RID: 10380
		private bool enable_;

		// Token: 0x0400288D RID: 10381
		private static VirtualScreenUtil instance_;

		// Token: 0x020010E5 RID: 4325
		public enum Constraint
		{
			// Token: 0x04005D60 RID: 23904
			Fit,
			// Token: 0x04005D61 RID: 23905
			Fill,
			// Token: 0x04005D62 RID: 23906
			FitWidth,
			// Token: 0x04005D63 RID: 23907
			FitHeight
		}
	}
}
