using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Ionic.Zlib;
using SGNFW.Common;
using UnityEngine.Networking;

namespace SGNFW.Http
{
	// Token: 0x0200024E RID: 590
	public class Connection : IDisposable
	{
		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06002515 RID: 9493 RVA: 0x0019E547 File Offset: 0x0019C747
		public static CookieContainer Cookie
		{
			get
			{
				if (Connection.cookie == null)
				{
					Connection.cookie = new CookieContainer();
				}
				return Connection.cookie;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06002517 RID: 9495 RVA: 0x0019E576 File Offset: 0x0019C776
		// (set) Token: 0x06002516 RID: 9494 RVA: 0x0019E55F File Offset: 0x0019C75F
		public static string Proxy
		{
			get
			{
				return Connection.proxy;
			}
			set
			{
				Connection.proxy = value;
				WebRequest.DefaultWebProxy = new WebProxy(Connection.proxy);
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06002519 RID: 9497 RVA: 0x0019E585 File Offset: 0x0019C785
		// (set) Token: 0x06002518 RID: 9496 RVA: 0x0019E57D File Offset: 0x0019C77D
		public static bool IsForceNoSecureRequest
		{
			get
			{
				return Connection.isForceNoSecureRequest;
			}
			set
			{
				Connection.isForceNoSecureRequest = value;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x0600251B RID: 9499 RVA: 0x0019E595 File Offset: 0x0019C795
		// (set) Token: 0x0600251A RID: 9498 RVA: 0x0019E58C File Offset: 0x0019C78C
		public bool IsPostMethod { get; set; }

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x0600251D RID: 9501 RVA: 0x0019E5A6 File Offset: 0x0019C7A6
		// (set) Token: 0x0600251C RID: 9500 RVA: 0x0019E59D File Offset: 0x0019C79D
		public string UserAgent { get; set; }

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x0600251F RID: 9503 RVA: 0x0019E5B7 File Offset: 0x0019C7B7
		// (set) Token: 0x0600251E RID: 9502 RVA: 0x0019E5AE File Offset: 0x0019C7AE
		public string EncryptKey { get; set; }

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06002521 RID: 9505 RVA: 0x0019E5C8 File Offset: 0x0019C7C8
		// (set) Token: 0x06002520 RID: 9504 RVA: 0x0019E5BF File Offset: 0x0019C7BF
		public bool IsBusy { get; private set; }

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06002523 RID: 9507 RVA: 0x0019E5D9 File Offset: 0x0019C7D9
		// (set) Token: 0x06002522 RID: 9506 RVA: 0x0019E5D0 File Offset: 0x0019C7D0
		public bool IsError { get; private set; }

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06002525 RID: 9509 RVA: 0x0019E5EA File Offset: 0x0019C7EA
		// (set) Token: 0x06002524 RID: 9508 RVA: 0x0019E5E1 File Offset: 0x0019C7E1
		public byte[] Posts { get; private set; }

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06002527 RID: 9511 RVA: 0x0019E5FB File Offset: 0x0019C7FB
		// (set) Token: 0x06002526 RID: 9510 RVA: 0x0019E5F2 File Offset: 0x0019C7F2
		public byte[] Bytes { get; private set; }

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06002529 RID: 9513 RVA: 0x0019E60C File Offset: 0x0019C80C
		// (set) Token: 0x06002528 RID: 9512 RVA: 0x0019E603 File Offset: 0x0019C803
		public int TimeoutTime
		{
			get
			{
				return this.timeoutTime;
			}
			set
			{
				this.timeoutTime = value;
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x0600252B RID: 9515 RVA: 0x0019E61D File Offset: 0x0019C81D
		// (set) Token: 0x0600252A RID: 9514 RVA: 0x0019E614 File Offset: 0x0019C814
		public HttpStatusCode StatusCode
		{
			get
			{
				return this.statusCode;
			}
			private set
			{
				this.statusCode = value;
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x0600252D RID: 9517 RVA: 0x0019E62E File Offset: 0x0019C82E
		// (set) Token: 0x0600252C RID: 9516 RVA: 0x0019E625 File Offset: 0x0019C825
		public WebExceptionStatus ExceptionStatus
		{
			get
			{
				return this.exceptionStatus;
			}
			private set
			{
				this.exceptionStatus = value;
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x0600252E RID: 9518 RVA: 0x0019E636 File Offset: 0x0019C836
		public string Url
		{
			get
			{
				return this.url;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x0600252F RID: 9519 RVA: 0x0019E63E File Offset: 0x0019C83E
		// (set) Token: 0x06002530 RID: 9520 RVA: 0x0019E646 File Offset: 0x0019C846
		public string Fields
		{
			get
			{
				return this.fields;
			}
			set
			{
				this.fields = value;
			}
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x0019E67C File Offset: 0x0019C87C
		~Connection()
		{
			this.Dispose(false);
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x0019E6AC File Offset: 0x0019C8AC
		public static void ClearCookie()
		{
			Connection.cookie = null;
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x0019E6B4 File Offset: 0x0019C8B4
		public void Request(string url, Action<bool, byte[]> finished = null)
		{
			if (ServicePointManager.ServerCertificateValidationCallback == null)
			{
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(Connection.OnRemoteCertificateValidation);
			}
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			lock (this)
			{
				this.IsBusy = true;
				this.url = url;
				this.onFinished = finished;
				string text = null;
				this.Posts = null;
				if (Connection.IsForceNoSecureRequest)
				{
					this.url = this.url.Replace("https:", "http:");
				}
				if (!string.IsNullOrEmpty(this.fields))
				{
					if (!string.IsNullOrEmpty(this.EncryptKey))
					{
						text = "param=" + this.Encrypt(this.fields);
					}
					else
					{
						text = this.fields;
					}
				}
				if (!string.IsNullOrEmpty(text) && !this.IsPostMethod)
				{
					this.uri = new Uri(this.url + "?" + text);
				}
				else
				{
					this.uri = new Uri(this.url);
				}
				try
				{
					this.request = (HttpWebRequest)WebRequest.Create(this.uri);
					this.request.ReadWriteTimeout = this.TimeoutTime;
					this.request.Timeout = this.TimeoutTime;
					this.request.KeepAlive = true;
					if (Connection.IsForceNoSecureRequest)
					{
						this.request.KeepAlive = false;
					}
					this.request.Headers.Add("Accept-Encoding:gzip");
					this.request.UserAgent = this.UserAgent;
					this.request.CookieContainer = Connection.Cookie;
					if (this.IsPostMethod && text != null)
					{
						this.Posts = Encoding.UTF8.GetBytes(text);
						this.request.Method = "POST";
						this.request.ContentType = "application/x-www-form-urlencoded";
						this.request.ContentLength = (long)this.Posts.Length;
						this.request.BeginGetRequestStream(new AsyncCallback(this.OnRequest), this.request);
					}
					else
					{
						this.request.BeginGetResponse(new AsyncCallback(this.OnResponse), this.request);
					}
				}
				catch (WebException ex)
				{
					Verbose<Verbose>.LogError(ex, null);
					this.Release();
					this.IsBusy = false;
					this.IsError = true;
					this.ExceptionStatus = ex.Status;
					if (this.onFinished != null)
					{
						this.onFinished(false, null);
					}
				}
				catch (Exception ex2)
				{
					Verbose<Verbose>.LogError(string.Format("{0}\n{1}", ex2, this.uri), null);
					this.Release();
					this.IsBusy = false;
					this.IsError = true;
					if (this.onFinished != null)
					{
						this.onFinished(false, null);
					}
				}
			}
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x0019E9A4 File Offset: 0x0019CBA4
		public void AddField(string name, string data)
		{
			if (string.IsNullOrEmpty(this.fields))
			{
				this.fields = string.Format("{0}={1}", UnityWebRequest.EscapeURL(name), UnityWebRequest.EscapeURL(data));
				return;
			}
			this.fields = string.Format("{0}&{1}={2}", this.fields, UnityWebRequest.EscapeURL(name), UnityWebRequest.EscapeURL(data));
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x0019E9FD File Offset: 0x0019CBFD
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x0019EA0C File Offset: 0x0019CC0C
		protected virtual void Dispose(bool dispose)
		{
			if (!this.isDisposed)
			{
				if (dispose)
				{
					this.Release();
				}
				this.isDisposed = true;
			}
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x0019EA28 File Offset: 0x0019CC28
		private void Release()
		{
			lock (this)
			{
				this.isCanceled = true;
				if (this.request != null)
				{
					this.request.Abort();
					this.request = null;
				}
				if (this.stream != null)
				{
					this.stream.Close();
					this.stream = null;
				}
				if (this.memoryStream != null)
				{
					this.memoryStream.Close();
					this.memoryStream = null;
				}
				if (this.response != null)
				{
					this.response.Close();
					this.response = null;
				}
			}
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x0019EACC File Offset: 0x0019CCCC
		private string Encrypt(string param)
		{
			byte[] md5Value = this.GetMD5Value(param);
			byte[] array = new byte[4];
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = md5Value[i];
				if (md5Value[i] < 16)
				{
					stringBuilder.Append('0');
				}
				stringBuilder.Append(Convert.ToString(md5Value[i], 16));
			}
			string text = stringBuilder.ToString();
			byte[] md5Value2 = this.GetMD5Value(array);
			byte[] bytes = Encoding.UTF8.GetBytes(param);
			StringBuilder stringBuilder2 = new StringBuilder();
			for (int j = 0; j < bytes.Length; j++)
			{
				byte[] array2 = bytes;
				int num = j;
				array2[num] ^= md5Value2[j % md5Value2.Length];
				if (bytes[j] < 16)
				{
					stringBuilder2.Append('0');
				}
				stringBuilder2.Append(Convert.ToString(bytes[j], 16));
			}
			string text2 = stringBuilder2.ToString();
			string text3 = string.Format("{0}ABCDEFGHIJKL", this.EncryptKey).Substring(2, 7) + " ";
			string md5String = this.GetMD5String(param + text3);
			return text + text2 + md5String;
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x0019EBE7 File Offset: 0x0019CDE7
		private byte[] GetMD5Value(string str)
		{
			return this.GetMD5Value(Encoding.UTF8.GetBytes(str));
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x0019EBFA File Offset: 0x0019CDFA
		private byte[] GetMD5Value(byte[] byteValue)
		{
			return new MD5CryptoServiceProvider().ComputeHash(byteValue);
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x0019EC08 File Offset: 0x0019CE08
		private string GetMD5String(string str)
		{
			byte[] md5Value = this.GetMD5Value(str);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < md5Value.Length; i++)
			{
				if (md5Value[i] < 16)
				{
					stringBuilder.Append('0');
				}
				stringBuilder.Append(Convert.ToString(md5Value[i], 16));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x0019EC58 File Offset: 0x0019CE58
		private void OnRequest(IAsyncResult ar)
		{
			if (!this.IsBusy)
			{
				return;
			}
			lock (this)
			{
				try
				{
					using (Stream stream = this.request.EndGetRequestStream(ar))
					{
						stream.Write(this.Posts, 0, this.Posts.Length);
					}
					this.request.BeginGetResponse(new AsyncCallback(this.OnResponse), this.request);
				}
				catch (WebException ex)
				{
					this.Release();
					this.IsBusy = false;
					this.IsError = true;
					this.ExceptionStatus = ex.Status;
					if (this.onFinished != null)
					{
						this.onFinished(false, null);
					}
				}
				catch (Exception)
				{
					this.Release();
					this.IsBusy = false;
					this.IsError = true;
					if (this.onFinished != null)
					{
						this.onFinished(false, null);
					}
				}
			}
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x0019ED6C File Offset: 0x0019CF6C
		private void OnResponse(IAsyncResult ar)
		{
			if (!this.IsBusy)
			{
				return;
			}
			lock (this)
			{
				try
				{
					this.response = (HttpWebResponse)this.request.EndGetResponse(ar);
					this.memoryStream = new MemoryStream();
					this.stream = this.response.GetResponseStream();
					this.stream.BeginRead(this.buffer, 0, 65536, new AsyncCallback(this.OnRead), this.response);
				}
				catch (WebException ex)
				{
					if (!this.isCanceled)
					{
						Verbose<Verbose>.LogError(ex, null);
						this.Release();
						this.IsBusy = false;
						this.IsError = true;
						this.ExceptionStatus = ex.Status;
						if (this.onFinished != null)
						{
							this.onFinished(false, null);
						}
					}
				}
				catch (Exception ex2)
				{
					if (!this.isCanceled)
					{
						Verbose<Verbose>.LogError(ex2, null);
						this.Release();
						this.IsBusy = false;
						this.IsError = true;
						if (this.onFinished != null)
						{
							this.onFinished(false, null);
						}
					}
				}
			}
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x0019EEA4 File Offset: 0x0019D0A4
		private void OnRead(IAsyncResult ar)
		{
			if (!this.IsBusy)
			{
				return;
			}
			lock (this)
			{
				try
				{
					int num = this.stream.EndRead(ar);
					if (num > 0)
					{
						this.memoryStream.Write(this.buffer, 0, num);
						this.stream.BeginRead(this.buffer, 0, 65536, new AsyncCallback(this.OnRead), this.response);
					}
					else
					{
						try
						{
							this.Bytes = this.memoryStream.ToArray();
							if (this.Bytes.Length >= 2 && this.Bytes[0] == 31 && this.Bytes[1] == 139)
							{
								this.memoryStream.Position = 0L;
								using (MemoryStream memoryStream = new MemoryStream())
								{
									using (GZipStream gzipStream = new GZipStream(this.memoryStream, CompressionMode.Decompress, true))
									{
										for (;;)
										{
											int num2 = gzipStream.Read(this.buffer, 0, this.buffer.Length);
											if (num2 == 0)
											{
												break;
											}
											memoryStream.Write(this.buffer, 0, num2);
										}
										this.Bytes = memoryStream.ToArray();
									}
								}
							}
						}
						catch (Exception ex)
						{
							Verbose<Verbose>.LogError(ex, null);
							this.Bytes = null;
						}
						if (this.Bytes == null)
						{
							this.IsBusy = false;
							this.IsError = true;
							this.Release();
							if (this.onFinished != null)
							{
								this.onFinished(false, null);
							}
						}
						else
						{
							if (this.onFinished != null)
							{
								this.onFinished(true, this.Bytes);
							}
							this.Release();
							this.IsBusy = false;
						}
					}
				}
				catch (WebException ex2)
				{
					if (!this.isCanceled)
					{
						Verbose<Verbose>.LogError(ex2, null);
						this.Release();
						this.IsBusy = false;
						this.IsError = true;
						this.ExceptionStatus = ex2.Status;
						if (this.onFinished != null)
						{
							this.onFinished(false, null);
						}
					}
				}
				catch (Exception ex3)
				{
					if (!this.isCanceled)
					{
						Verbose<Verbose>.LogError(ex3, null);
						this.Release();
						this.IsBusy = false;
						this.IsError = true;
						if (this.onFinished != null)
						{
							this.onFinished(false, null);
						}
					}
				}
			}
		}

		// Token: 0x06002540 RID: 9536 RVA: 0x0019F164 File Offset: 0x0019D364
		private static bool OnRemoteCertificateValidation(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors err)
		{
			return true;
		}

		// Token: 0x04001B86 RID: 7046
		private const int BUFFER_SIZE = 65536;

		// Token: 0x04001B87 RID: 7047
		private static CookieContainer cookie;

		// Token: 0x04001B88 RID: 7048
		private static string proxy;

		// Token: 0x04001B89 RID: 7049
		private static bool isForceNoSecureRequest;

		// Token: 0x04001B8A RID: 7050
		private string fields;

		// Token: 0x04001B8B RID: 7051
		private string url;

		// Token: 0x04001B8C RID: 7052
		private Uri uri;

		// Token: 0x04001B8D RID: 7053
		private HttpWebRequest request;

		// Token: 0x04001B8E RID: 7054
		private HttpWebResponse response;

		// Token: 0x04001B8F RID: 7055
		private Stream stream;

		// Token: 0x04001B90 RID: 7056
		private MemoryStream memoryStream;

		// Token: 0x04001B91 RID: 7057
		private byte[] buffer = new byte[65536];

		// Token: 0x04001B92 RID: 7058
		private WebExceptionStatus exceptionStatus;

		// Token: 0x04001B93 RID: 7059
		private HttpStatusCode statusCode = HttpStatusCode.OK;

		// Token: 0x04001B94 RID: 7060
		private bool isCanceled;

		// Token: 0x04001B95 RID: 7061
		private int timeoutTime = -1;

		// Token: 0x04001B96 RID: 7062
		private bool isDisposed;

		// Token: 0x04001B97 RID: 7063
		private Action<bool, byte[]> onFinished;

		// Token: 0x04001B98 RID: 7064
		private string tm;
	}
}
