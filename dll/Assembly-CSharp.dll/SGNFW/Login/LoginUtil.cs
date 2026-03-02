using System;
using System.Security.Cryptography;
using System.Text;

namespace SGNFW.Login
{
	// Token: 0x02000321 RID: 801
	public static class LoginUtil
	{
		// Token: 0x060028A0 RID: 10400 RVA: 0x001A86D4 File Offset: 0x001A68D4
		public static string GetLoginSecureID(string account, string digestSalt)
		{
			string text = "";
			byte[] array = MD5.Create().ComputeHash(Encoding.Default.GetBytes(account + account + digestSalt));
			for (int i = 0; i < array.Length; i++)
			{
				text += string.Format("{0:X2}", array[i]);
			}
			return text;
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x001A872C File Offset: 0x001A692C
		public static string GetPasswordDigest(string password)
		{
			string text = "";
			byte[] array = MD5.Create().ComputeHash(Encoding.Default.GetBytes(password));
			for (int i = 0; i < array.Length; i++)
			{
				text += string.Format("{0:X2}", array[i]);
			}
			return text;
		}
	}
}
