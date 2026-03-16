using System;
using System.Security.Cryptography;
using System.Text;

namespace SGNFW.Login
{
	public static class LoginUtil
	{
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
