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
	public class Connection : IDisposable
	{
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

		public bool IsPostMethod { get; set; }

		public string UserAgent { get; set; }

		public string EncryptKey { get; set; }

		public bool IsBusy { get; private set; }

		public bool IsError { get; private set; }

		public byte[] Posts { get; private set; }

		public byte[] Bytes { get; private set; }

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

		public string Url
		{
			get
			{
				return this.url;
			}
		}

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

		~Connection()
		{
			this.Dispose(false);
		}

		public static void ClearCookie()
		{
			Connection.cookie = null;
		}

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

		public void AddField(string name, string data)
		{
			if (string.IsNullOrEmpty(this.fields))
			{
				this.fields = string.Format("{0}={1}", UnityWebRequest.EscapeURL(name), UnityWebRequest.EscapeURL(data));
				return;
			}
			this.fields = string.Format("{0}&{1}={2}", this.fields, UnityWebRequest.EscapeURL(name), UnityWebRequest.EscapeURL(data));
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

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

		private byte[] GetMD5Value(string str)
		{
			return this.GetMD5Value(Encoding.UTF8.GetBytes(str));
		}

		private byte[] GetMD5Value(byte[] byteValue)
		{
			return new MD5CryptoServiceProvider().ComputeHash(byteValue);
		}

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

		private static bool OnRemoteCertificateValidation(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors err)
		{
			return true;
		}

		private const int BUFFER_SIZE = 65536;

		private static CookieContainer cookie;

		private static string proxy;

		private static bool isForceNoSecureRequest;

		private string fields;

		private string url;

		private Uri uri;

		private HttpWebRequest request;

		private HttpWebResponse response;

		private Stream stream;

		private MemoryStream memoryStream;

		private byte[] buffer = new byte[65536];

		private WebExceptionStatus exceptionStatus;

		private HttpStatusCode statusCode = HttpStatusCode.OK;

		private bool isCanceled;

		private int timeoutTime = -1;

		private bool isDisposed;

		private Action<bool, byte[]> onFinished;

		private string tm;
	}
}
