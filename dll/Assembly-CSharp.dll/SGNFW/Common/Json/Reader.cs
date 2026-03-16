using System;
using System.Globalization;

namespace SGNFW.Common.Json
{
	public class Reader
	{
		public bool IsError { get; private set; }

		public Reader(string text)
		{
			Verbose<Verbose>.Enabled = true;
			this.p = 0;
			this.text = text;
			this.length = text.Length;
			this.IsError = false;
			this.line = 1;
			if (text.Length >= 3 && text[0] == 'ï' && text[1] == '»' && text[2] == '¿')
			{
				this.p = 3;
			}
		}

		public bool TrimWhiteSpace()
		{
			while (this.p < this.length)
			{
				if (this.text[this.p] == '/' && this.p + 1 < this.length)
				{
					if (this.text[this.p + 1] == '/')
					{
						this.p += 2;
						while (this.p < this.length)
						{
							if (this.text[this.p] == '\n')
							{
								break;
							}
							this.p++;
						}
					}
					else if (this.text[this.p + 1] == '*')
					{
						this.p += 2;
						while (this.p + 1 < this.length)
						{
							if (this.text[this.p] == '*' && this.text[this.p + 1] == '/')
							{
								this.p += 2;
								break;
							}
							if (this.text[this.p] == '\n')
							{
								this.line++;
							}
							this.p++;
						}
					}
				}
				if (" \t\n\r".IndexOf(this.text[this.p]) == -1)
				{
					break;
				}
				if (this.text[this.p] == '\n')
				{
					this.line++;
				}
				this.p++;
			}
			return this.p < this.length;
		}

		public void ParseChar(ref Token token)
		{
			long num = 0L;
			if (this.text[this.p] == '\'')
			{
				this.p++;
				while (this.p < this.length)
				{
					char c = this.text[this.p];
					this.p++;
					if (c != '\0')
					{
						if (c == '\'')
						{
							token.type = Token.Type.Integer;
							token.longValue = num;
							return;
						}
						if (c != '\\')
						{
							num = (long)((ulong)c);
						}
						else
						{
							if (this.p >= this.length)
							{
								break;
							}
							char c2 = this.text[this.p];
							this.p++;
							if (c2 <= 'b')
							{
								if (c2 <= '/')
								{
									if (c2 != '"')
									{
										if (c2 != '/')
										{
											break;
										}
										num = 47L;
									}
									else
									{
										num = 34L;
									}
								}
								else if (c2 != '\\')
								{
									if (c2 != 'b')
									{
										break;
									}
									num = 8L;
								}
								else
								{
									num = 92L;
								}
							}
							else if (c2 <= 'n')
							{
								if (c2 != 'f')
								{
									if (c2 != 'n')
									{
										break;
									}
									num = 10L;
								}
								else
								{
									num = 12L;
								}
							}
							else if (c2 != 'r')
							{
								if (c2 != 't')
								{
									break;
								}
								num = 9L;
							}
							else
							{
								num = 13L;
							}
						}
					}
				}
			}
			token.type = Token.Type.Invalid;
			this.IsError = true;
		}

		public void ParseString(ref Token token)
		{
			this.bp_ = 0;
			if (this.text[this.p] == '"')
			{
				this.p++;
				while (this.p < this.length)
				{
					char c = this.text[this.p];
					this.p++;
					if (c == '"')
					{
						token.type = Token.Type.String;
						token.stringValue = new string(this.buffer_, 0, this.bp_);
						return;
					}
					this.prepareBuffer_();
					if (c != '\\')
					{
						char[] array = this.buffer_;
						int num = this.bp_;
						this.bp_ = num + 1;
						array[num] = c;
					}
					else
					{
						if (this.p >= this.length)
						{
							break;
						}
						char c2 = this.text[this.p];
						this.p++;
						if (c2 <= '\\')
						{
							if (c2 != '"')
							{
								if (c2 != '/')
								{
									if (c2 != '\\')
									{
										break;
									}
									char[] array2 = this.buffer_;
									int num = this.bp_;
									this.bp_ = num + 1;
									array2[num] = 92;
								}
								else
								{
									char[] array3 = this.buffer_;
									int num = this.bp_;
									this.bp_ = num + 1;
									array3[num] = 47;
								}
							}
							else
							{
								char[] array4 = this.buffer_;
								int num = this.bp_;
								this.bp_ = num + 1;
								array4[num] = 34;
							}
						}
						else if (c2 <= 'f')
						{
							if (c2 != 'b')
							{
								if (c2 != 'f')
								{
									break;
								}
								char[] array5 = this.buffer_;
								int num = this.bp_;
								this.bp_ = num + 1;
								array5[num] = 12;
							}
							else
							{
								char[] array6 = this.buffer_;
								int num = this.bp_;
								this.bp_ = num + 1;
								array6[num] = 8;
							}
						}
						else if (c2 != 'n')
						{
							switch (c2)
							{
							case 'r':
							{
								char[] array7 = this.buffer_;
								int num = this.bp_;
								this.bp_ = num + 1;
								array7[num] = 13;
								break;
							}
							case 's':
								goto IL_02A7;
							case 't':
							{
								char[] array8 = this.buffer_;
								int num = this.bp_;
								this.bp_ = num + 1;
								array8[num] = 9;
								break;
							}
							case 'u':
							{
								if (this.length - this.p < 4)
								{
									goto IL_02A7;
								}
								string text = this.text.Substring(this.p, 4);
								this.p += 4;
								string text2 = char.ConvertFromUtf32((int)uint.Parse(text, NumberStyles.HexNumber));
								for (int i = 0; i < text2.Length; i++)
								{
									char[] array9 = this.buffer_;
									int num = this.bp_;
									this.bp_ = num + 1;
									array9[num] = text2[i];
								}
								break;
							}
							default:
								goto IL_02A7;
							}
						}
						else
						{
							char[] array10 = this.buffer_;
							int num = this.bp_;
							this.bp_ = num + 1;
							array10[num] = 10;
						}
					}
				}
			}
			IL_02A7:
			token.type = Token.Type.Invalid;
			this.IsError = true;
		}

		private void prepareBuffer_()
		{
			if (this.bp_ + 4 >= this.bufferLength_)
			{
				int num = this.bufferLength_ * 2;
				char[] array = new char[num];
				Array.Copy(this.buffer_, 0, array, 0, this.bp_);
				this.buffer_ = array;
				this.bufferLength_ = num;
			}
		}

		public void ParseBooleanOrNull(ref Token token)
		{
			if (this.length - this.p >= 5 && string.Compare(this.text, this.p, "false", 0, 5) == 0)
			{
				this.p += 5;
				token.type = Token.Type.Boolean;
				token.boolValue = false;
				return;
			}
			if (this.length - this.p >= 4)
			{
				if (string.Compare(this.text, this.p, "true", 0, 4) == 0)
				{
					this.p += 4;
					token.type = Token.Type.Boolean;
					token.boolValue = true;
					return;
				}
				if (string.Compare(this.text, this.p, "null", 0, 4) == 0)
				{
					this.p += 4;
					token.type = Token.Type.Null;
					return;
				}
			}
			this.IsError = true;
		}

		public void ParseNumber(ref Token token)
		{
			int num = this.p;
			while (num < this.length && "0123456789+-.eE".IndexOf(this.text[num]) != -1)
			{
				num++;
			}
			string text = this.text.Substring(this.p, num - this.p);
			this.p = num;
			if (text.Contains("."))
			{
				token.type = Token.Type.Floating;
				token.doubleValue = double.Parse(text);
				return;
			}
			token.type = Token.Type.Integer;
			token.longValue = long.Parse(text);
		}

		public Token GetToken()
		{
			Token token = default(Token);
			while (this.TrimWhiteSpace())
			{
				char c = this.text[this.p];
				if (c <= 'f')
				{
					if (c <= '[')
					{
						switch (c)
						{
						case '"':
							this.ParseString(ref token);
							return token;
						case '#':
						case '$':
						case '%':
						case '&':
						case '(':
						case ')':
						case '*':
						case '+':
						case '.':
						case '/':
							goto IL_01A0;
						case '\'':
							this.ParseChar(ref token);
							return token;
						case ',':
							this.p++;
							continue;
						case '-':
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
							this.ParseNumber(ref token);
							return token;
						case ':':
							this.p++;
							continue;
						default:
							if (c != '[')
							{
								goto IL_01A0;
							}
							this.p++;
							token.type = Token.Type.ArrayBegin;
							return token;
						}
					}
					else
					{
						if (c == ']')
						{
							this.p++;
							token.type = Token.Type.ArrayEnd;
							return token;
						}
						if (c != 'f')
						{
							goto IL_01A0;
						}
					}
				}
				else if (c <= 't')
				{
					if (c != 'n' && c != 't')
					{
						goto IL_01A0;
					}
				}
				else
				{
					if (c == '{')
					{
						this.p++;
						token.type = Token.Type.ObjectBegin;
						return token;
					}
					if (c != '}')
					{
						goto IL_01A0;
					}
					this.p++;
					token.type = Token.Type.ObjectEnd;
					return token;
				}
				this.ParseBooleanOrNull(ref token);
				return token;
				IL_01A0:
				token.type = Token.Type.Invalid;
				this.IsError = true;
				return token;
			}
			token.type = Token.Type.End;
			return token;
		}

		public void SkipValue()
		{
			int num = 0;
			int num2 = 0;
			do
			{
				switch (this.GetToken().type)
				{
				case Token.Type.ObjectBegin:
					num2++;
					break;
				case Token.Type.ObjectEnd:
					num2--;
					break;
				case Token.Type.ArrayBegin:
					num++;
					break;
				case Token.Type.ArrayEnd:
					num--;
					break;
				}
			}
			while (num > 0 || num2 > 0);
		}

		private int p;

		private int length;

		private string text;

		private int line;

		private char[] buffer_ = new char[128];

		private int bufferLength_ = 128;

		private int bp_;

		private const int DEFAULT_BUFFER_LENGTH = 128;
	}
}
