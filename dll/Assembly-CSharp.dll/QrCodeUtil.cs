using System;
using System.Security.Cryptography;

// Token: 0x020000D6 RID: 214
public static class QrCodeUtil
{
	// Token: 0x020007CF RID: 1999
	public static class Encription
	{
		// Token: 0x0600373F RID: 14143 RVA: 0x001C7DAC File Offset: 0x001C5FAC
		public static byte[] EncryptBuffer(byte[] sourceBuffer, string password, byte[] salt, int count, int StretchCount)
		{
			int num = (sourceBuffer.Length + QrCodeUtil.Encription.Default_Block_Byte - 1) / QrCodeUtil.Encription.Default_Block_Byte * QrCodeUtil.Encription.Default_Block_Byte;
			byte[] array = new byte[num];
			Buffer.BlockCopy(sourceBuffer, 0, array, 0, sourceBuffer.Length);
			byte[] array2 = new byte[num];
			AesManaged aesManaged = new AesManaged();
			aesManaged.Mode = CipherMode.CBC;
			aesManaged.Padding = PaddingMode.None;
			QrCodeUtil.Encription.GenerateKeyAndIV(aesManaged, password, salt, count);
			for (int i = 0; i < StretchCount; i++)
			{
				aesManaged.CreateEncryptor(aesManaged.Key, aesManaged.IV).TransformBlock(array, 0, array.Length, array2, 0);
				Buffer.BlockCopy(array2, 0, array, 0, array2.Length);
			}
			return array2;
		}

		// Token: 0x06003740 RID: 14144 RVA: 0x001C7E40 File Offset: 0x001C6040
		public static byte[] DecryptBuffer(byte[] sourceBuffer, string password, byte[] salt, int count, int StretchCount)
		{
			byte[] array = new byte[sourceBuffer.Length];
			Buffer.BlockCopy(sourceBuffer, 0, array, 0, sourceBuffer.Length);
			byte[] array2 = new byte[sourceBuffer.Length];
			AesManaged aesManaged = new AesManaged();
			aesManaged.Mode = CipherMode.CBC;
			aesManaged.Padding = PaddingMode.None;
			QrCodeUtil.Encription.GenerateKeyAndIV(aesManaged, password, salt, count);
			for (int i = 0; i < StretchCount; i++)
			{
				aesManaged.CreateDecryptor(aesManaged.Key, aesManaged.IV).TransformBlock(array, 0, array.Length, array2, 0);
				Buffer.BlockCopy(array2, 0, array, 0, array2.Length);
			}
			return array2;
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x001C7EC4 File Offset: 0x001C60C4
		private static void GenerateKeyAndIV(AesManaged aes, string password, byte[] salt, int count)
		{
			aes.KeySize = QrCodeUtil.Encription.Default_Key_Size;
			aes.BlockSize = QrCodeUtil.Encription.Default_Block_Size;
			Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt);
			rfc2898DeriveBytes.IterationCount = count;
			aes.Key = rfc2898DeriveBytes.GetBytes(QrCodeUtil.Encription.Default_Key_Byte);
			aes.IV = rfc2898DeriveBytes.GetBytes(QrCodeUtil.Encription.Default_Block_Byte);
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x001C7F18 File Offset: 0x001C6118
		public static string convertHeaderToParam(byte[] header, string Password, byte[] Salt, byte[] saltCopy, out int addIteration, out int addStretch)
		{
			int num = (int)(header[0] % 8);
			string text = Password.Substring(0, num) + Convert.ToString(header[0], 16) + Convert.ToString(header[1], 16) + Password.Substring(num);
			int num2 = (int)(header[1] % 8);
			Buffer.BlockCopy(Salt, 0, saltCopy, 0, Salt.Length);
			saltCopy[num2] = header[2];
			saltCopy[num2 + 7] = header[3];
			addIteration = (int)(header[2] % 30);
			addStretch = (int)(header[3] % 4);
			return text;
		}

		// Token: 0x040034C8 RID: 13512
		private static int Default_Key_Size = 128;

		// Token: 0x040034C9 RID: 13513
		private static int Default_Block_Size = 128;

		// Token: 0x040034CA RID: 13514
		private static int Byte_Size = 8;

		// Token: 0x040034CB RID: 13515
		private static int Default_Key_Byte = QrCodeUtil.Encription.Default_Key_Size / QrCodeUtil.Encription.Byte_Size;

		// Token: 0x040034CC RID: 13516
		private static int Default_Block_Byte = QrCodeUtil.Encription.Default_Block_Size / QrCodeUtil.Encription.Byte_Size;
	}
}
