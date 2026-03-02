using System;
using System.Collections.Generic;
using System.Globalization;

namespace SGNFW.Common.Json
{
	// Token: 0x02000274 RID: 628
	public class Maker
	{
		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06002693 RID: 9875 RVA: 0x001A2085 File Offset: 0x001A0285
		// (set) Token: 0x06002692 RID: 9874 RVA: 0x001A207C File Offset: 0x001A027C
		public bool IsError { get; private set; }

		// Token: 0x06002694 RID: 9876 RVA: 0x001A2090 File Offset: 0x001A0290
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

		// Token: 0x06002695 RID: 9877 RVA: 0x001A211C File Offset: 0x001A031C
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

		// Token: 0x06002696 RID: 9878 RVA: 0x001A22D0 File Offset: 0x001A04D0
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

		// Token: 0x06002697 RID: 9879 RVA: 0x001A2390 File Offset: 0x001A0590
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

		// Token: 0x06002698 RID: 9880 RVA: 0x001A2414 File Offset: 0x001A0614
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

		// Token: 0x06002699 RID: 9881 RVA: 0x001A2544 File Offset: 0x001A0744
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

		// Token: 0x0600269A RID: 9882 RVA: 0x001A27F8 File Offset: 0x001A09F8
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

		// Token: 0x0600269B RID: 9883 RVA: 0x001A2848 File Offset: 0x001A0A48
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

		// Token: 0x0600269C RID: 9884 RVA: 0x001A290C File Offset: 0x001A0B0C
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

		// Token: 0x0600269D RID: 9885 RVA: 0x001A2990 File Offset: 0x001A0B90
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

		// Token: 0x04001C58 RID: 7256
		private int p;

		// Token: 0x04001C59 RID: 7257
		private int length;

		// Token: 0x04001C5A RID: 7258
		private string text;

		// Token: 0x04001C5B RID: 7259
		private int line;

		// Token: 0x04001C5C RID: 7260
		private char[] buffer_ = new char[128];

		// Token: 0x04001C5D RID: 7261
		private int bufferLength_ = 128;

		// Token: 0x04001C5E RID: 7262
		private int bp_;

		// Token: 0x04001C5F RID: 7263
		private const int DEFAULT_BUFFER_LENGTH = 128;

		// Token: 0x020010A4 RID: 4260
		private enum TokenType
		{
			// Token: 0x04005C4C RID: 23628
			Invalid,
			// Token: 0x04005C4D RID: 23629
			ObjectEnd,
			// Token: 0x04005C4E RID: 23630
			ObjectSeparator,
			// Token: 0x04005C4F RID: 23631
			ArrayEnd,
			// Token: 0x04005C50 RID: 23632
			Separator,
			// Token: 0x04005C51 RID: 23633
			JsonData,
			// Token: 0x04005C52 RID: 23634
			End
		}

		// Token: 0x020010A5 RID: 4261
		private struct Token
		{
			// Token: 0x04005C53 RID: 23635
			public Maker.TokenType type;

			// Token: 0x04005C54 RID: 23636
			public Data data;
		}
	}
}
