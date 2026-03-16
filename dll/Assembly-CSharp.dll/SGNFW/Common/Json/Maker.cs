using System;
using System.Collections.Generic;
using System.Globalization;

namespace SGNFW.Common.Json
{
	public class Maker
	{
		public bool IsError { get; private set; }

		public Data Make(string text)
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
			Maker.Token token = this.GetToken();
			if (this.IsError)
			{
				return null;
			}
			return token.data;
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

		public Data ParseObject()
		{
			Dictionary<string, Data> dictionary = new Dictionary<string, Data>();
			if (this.text[this.p] == '{')
			{
				this.p++;
				Maker.Token token3;
				do
				{
					Maker.Token token = this.GetToken();
					if (token.type == Maker.TokenType.ObjectEnd)
					{
						goto IL_00A1;
					}
					if (token.type != Maker.TokenType.JsonData || token.data.JsonType != Type.String || this.GetToken().type != Maker.TokenType.ObjectSeparator)
					{
						goto IL_00A8;
					}
					Maker.Token token2 = this.GetToken();
					if (token2.type != Maker.TokenType.JsonData)
					{
						goto IL_00A8;
					}
					dictionary[token.data.GetString()] = token2.data;
					token3 = this.GetToken();
				}
				while (token3.type == Maker.TokenType.Separator);
				if (token3.type != Maker.TokenType.ObjectEnd)
				{
					goto IL_00A8;
				}
				IL_00A1:
				return new Data(dictionary);
			}
			IL_00A8:
			this.IsError = true;
			return null;
		}

		public Data ParseArray()
		{
			List<Data> list = new List<Data>();
			if (this.text[this.p] == '[')
			{
				this.p++;
				Maker.Token token2;
				do
				{
					Maker.Token token = this.GetToken();
					if (token.type == Maker.TokenType.ArrayEnd)
					{
						goto IL_0067;
					}
					if (token.type != Maker.TokenType.JsonData)
					{
						goto IL_006E;
					}
					list.Add(token.data);
					token2 = this.GetToken();
				}
				while (token2.type == Maker.TokenType.Separator);
				if (token2.type != Maker.TokenType.ArrayEnd)
				{
					goto IL_006E;
				}
				IL_0067:
				return new Data(list);
			}
			IL_006E:
			this.IsError = true;
			return null;
		}

		public Data ParseChar()
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
							return new Data(num);
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
			this.IsError = true;
			return null;
		}

		public Data ParseString()
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
						return new Data(new string(this.buffer_, 0, this.bp_));
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
								goto IL_029F;
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
									goto IL_029F;
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
								goto IL_029F;
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
			IL_029F:
			this.IsError = true;
			return null;
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

		public Data ParseBooleanOrNull()
		{
			if (this.length - this.p >= 5 && string.Compare(this.text, this.p, "false", 0, 5) == 0)
			{
				this.p += 5;
				return new Data(false);
			}
			if (this.length - this.p >= 4)
			{
				if (string.Compare(this.text, this.p, "true", 0, 4) == 0)
				{
					this.p += 4;
					return new Data(true);
				}
				if (string.Compare(this.text, this.p, "null", 0, 4) == 0)
				{
					this.p += 4;
					return new Data();
				}
			}
			this.IsError = true;
			return null;
		}

		public Data ParseNumber()
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
				return new Data(double.Parse(text));
			}
			return new Data(long.Parse(text));
		}

		private Maker.Token GetToken()
		{
			Maker.Token token = default(Maker.Token);
			token.type = Maker.TokenType.Invalid;
			token.data = null;
			if (!this.TrimWhiteSpace())
			{
				token.type = Maker.TokenType.End;
				return token;
			}
			char c = this.text[this.p];
			if (c <= 'f')
			{
				if (c <= '[')
				{
					switch (c)
					{
					case '"':
						token.data = this.ParseString();
						if (token.data != null)
						{
							token.type = Maker.TokenType.JsonData;
						}
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
						goto IL_021B;
					case '\'':
						token.data = this.ParseChar();
						if (token.data != null)
						{
							token.type = Maker.TokenType.JsonData;
						}
						return token;
					case ',':
						this.p++;
						token.type = Maker.TokenType.Separator;
						return token;
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
						token.data = this.ParseNumber();
						if (token.data != null)
						{
							token.type = Maker.TokenType.JsonData;
						}
						return token;
					case ':':
						this.p++;
						token.type = Maker.TokenType.ObjectSeparator;
						return token;
					default:
						if (c != '[')
						{
							goto IL_021B;
						}
						token.data = this.ParseArray();
						if (token.data != null)
						{
							token.type = Maker.TokenType.JsonData;
						}
						return token;
					}
				}
				else
				{
					if (c == ']')
					{
						this.p++;
						token.type = Maker.TokenType.ArrayEnd;
						return token;
					}
					if (c != 'f')
					{
						goto IL_021B;
					}
				}
			}
			else if (c <= 't')
			{
				if (c != 'n' && c != 't')
				{
					goto IL_021B;
				}
			}
			else
			{
				if (c == '{')
				{
					token.data = this.ParseObject();
					if (token.data != null)
					{
						token.type = Maker.TokenType.JsonData;
					}
					return token;
				}
				if (c != '}')
				{
					goto IL_021B;
				}
				this.p++;
				token.type = Maker.TokenType.ObjectEnd;
				return token;
			}
			token.data = this.ParseBooleanOrNull();
			if (token.data != null)
			{
				token.type = Maker.TokenType.JsonData;
			}
			return token;
			IL_021B:
			token.type = Maker.TokenType.Invalid;
			this.IsError = true;
			return token;
		}

		private int p;

		private int length;

		private string text;

		private int line;

		private char[] buffer_ = new char[128];

		private int bufferLength_ = 128;

		private int bp_;

		private const int DEFAULT_BUFFER_LENGTH = 128;

		private enum TokenType
		{
			Invalid,
			ObjectEnd,
			ObjectSeparator,
			ArrayEnd,
			Separator,
			JsonData,
			End
		}

		private struct Token
		{
			public Maker.TokenType type;

			public Data data;
		}
	}
}
