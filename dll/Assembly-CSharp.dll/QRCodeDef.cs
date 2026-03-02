using System;
using System.Runtime.InteropServices;

// Token: 0x020000D4 RID: 212
public class QRCodeDef
{
	// Token: 0x040007C0 RID: 1984
	public const byte CardVersion = 0;

	// Token: 0x040007C1 RID: 1985
	public const byte Allow_Readable_CardVersion = 0;

	// Token: 0x040007C2 RID: 1986
	public const int IterationCount = 1000;

	// Token: 0x040007C3 RID: 1987
	public const int StretchCount = 3;

	// Token: 0x040007C4 RID: 1988
	public const int QRHeaderSize = 9;

	// Token: 0x040007C5 RID: 1989
	public const bool IsUseQRHeaderToSalt = true;

	// Token: 0x040007C6 RID: 1990
	public const int CardIDSize = 2;

	// Token: 0x040007C7 RID: 1991
	public const int QRCodeMaxSize = 16;

	// Token: 0x020007C9 RID: 1993
	public enum CardKind : byte
	{
		// Token: 0x040034AF RID: 13487
		Mobile = 3,
		// Token: 0x040034B0 RID: 13488
		End
	}

	// Token: 0x020007CA RID: 1994
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class QRCardGameIDInfo
	{
		// Token: 0x040034B1 RID: 13489
		private const int PassAndSaltSize = 4;

		// Token: 0x040034B2 RID: 13490
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] PassAndSalt = new byte[4];

		// Token: 0x040034B3 RID: 13491
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] CardID = new byte[2];

		// Token: 0x040034B4 RID: 13492
		public ushort PrintDate;

		// Token: 0x040034B5 RID: 13493
		public byte GameVersion;
	}

	// Token: 0x020007CB RID: 1995
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class QRCardHeader
	{
		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06003735 RID: 14133 RVA: 0x001C7C68 File Offset: 0x001C5E68
		// (set) Token: 0x06003736 RID: 14134 RVA: 0x001C7C79 File Offset: 0x001C5E79
		public QRCodeDef.CardKind CardType
		{
			get
			{
				return (QRCodeDef.CardKind)((this.UnionValue & 224) >> 5);
			}
			set
			{
				this.UnionValue = (byte)(((int)this.UnionValue & -225) | (int)(((int)value << 5) & (QRCodeDef.CardKind)224));
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06003737 RID: 14135 RVA: 0x001C7C98 File Offset: 0x001C5E98
		// (set) Token: 0x06003738 RID: 14136 RVA: 0x001C7CA4 File Offset: 0x001C5EA4
		public byte Version
		{
			get
			{
				return this.UnionValue & 31;
			}
			set
			{
				this.UnionValue = (byte)(((int)this.UnionValue & -32) | (int)(value & 31));
			}
		}

		// Token: 0x040034B6 RID: 13494
		public const byte VersionShift = 0;

		// Token: 0x040034B7 RID: 13495
		public const byte CardTypeShift = 5;

		// Token: 0x040034B8 RID: 13496
		public const byte VersionMask = 31;

		// Token: 0x040034B9 RID: 13497
		public const byte CardTypeMask = 224;

		// Token: 0x040034BA RID: 13498
		private byte UnionValue;
	}

	// Token: 0x020007CC RID: 1996
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 16)]
	public class QRMobileCard
	{
		// Token: 0x0600373A RID: 14138 RVA: 0x001C7CC4 File Offset: 0x001C5EC4
		public static byte[] CreateByteData(int photoId, bool isChange)
		{
			QRCodeDef.QRMobileCard qrmobileCard = new QRCodeDef.QRMobileCard
			{
				Header = new QRCodeDef.QRCardHeader
				{
					CardType = QRCodeDef.CardKind.Mobile
				},
				CardID = (uint)photoId,
				SubID = (isChange ? 1 : 0)
			};
			byte[] array = new byte[16];
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			Marshal.StructureToPtr<QRCodeDef.QRMobileCard>(qrmobileCard, gchandle.AddrOfPinnedObject(), false);
			gchandle.Free();
			return array;
		}

		// Token: 0x040034BB RID: 13499
		public QRCodeDef.QRCardHeader Header;

		// Token: 0x040034BC RID: 13500
		public uint CardID;

		// Token: 0x040034BD RID: 13501
		public byte SubID;
	}

	// Token: 0x020007CD RID: 1997
	public class Util
	{
		// Token: 0x0600373C RID: 14140 RVA: 0x001C7D2C File Offset: 0x001C5F2C
		public static byte CalcCRC7(byte[] buffer, int startPos = 0, int endPos = 0)
		{
			byte b = 0;
			if (endPos <= 0)
			{
				endPos = buffer.Length;
			}
			for (int i = startPos; i < endPos; i++)
			{
				b ^= buffer[i] + 1;
				for (int j = 0; j < 8; j++)
				{
					if ((b & 128) > 0)
					{
						b ^= 137;
					}
					b = (byte)(b << 1);
				}
			}
			return b;
		}
	}
}
