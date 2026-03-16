using System;
using System.Runtime.InteropServices;

public class QRCodeDef
{
	public const byte CardVersion = 0;

	public const byte Allow_Readable_CardVersion = 0;

	public const int IterationCount = 1000;

	public const int StretchCount = 3;

	public const int QRHeaderSize = 9;

	public const bool IsUseQRHeaderToSalt = true;

	public const int CardIDSize = 2;

	public const int QRCodeMaxSize = 16;

	public enum CardKind : byte
	{
		Mobile = 3,
		End
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class QRCardGameIDInfo
	{
		private const int PassAndSaltSize = 4;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] PassAndSalt = new byte[4];

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] CardID = new byte[2];

		public ushort PrintDate;

		public byte GameVersion;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class QRCardHeader
	{
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

		public const byte VersionShift = 0;

		public const byte CardTypeShift = 5;

		public const byte VersionMask = 31;

		public const byte CardTypeMask = 224;

		private byte UnionValue;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 16)]
	public class QRMobileCard
	{
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

		public QRCodeDef.QRCardHeader Header;

		public uint CardID;

		public byte SubID;
	}

	public class Util
	{
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
