using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AFMiniJSON
{
	// Token: 0x02000589 RID: 1417
	public static class Json
	{
		// Token: 0x06002EDC RID: 11996 RVA: 0x001B2E3D File Offset: 0x001B103D
		public static object Deserialize(string json)
		{
			if (json == null)
			{
				return null;
			}
			return Json.Parser.Parse(json);
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x001B2E4A File Offset: 0x001B104A
		public static string Serialize(object obj)
		{
			return Json.Serializer.Serialize(obj);
		}

		// Token: 0x020010F4 RID: 4340
		private sealed class Parser : IDisposable
		{
			// Token: 0x06005442 RID: 21570 RVA: 0x0024CF30 File Offset: 0x0024B130
			public static bool IsWordBreak(char c)
			{
				return char.IsWhiteSpace(c) || "{}[],:\"".IndexOf(c) != -1;
			}

			// Token: 0x06005443 RID: 21571 RVA: 0x0024CF4D File Offset: 0x0024B14D
			private Parser(string jsonString)
			{
				this.json = new StringReader(jsonString);
			}

			// Token: 0x06005444 RID: 21572 RVA: 0x0024CF64 File Offset: 0x0024B164
			public static object Parse(string jsonString)
			{
				object obj;
				using (Json.Parser parser = new Json.Parser(jsonString))
				{
					obj = parser.ParseValue();
				}
				return obj;
			}

			// Token: 0x06005445 RID: 21573 RVA: 0x0024CF9C File Offset: 0x0024B19C
			public void Dispose()
			{
				this.json.Dispose();
				this.json = null;
			}

			// Token: 0x06005446 RID: 21574 RVA: 0x0024CFB0 File Offset: 0x0024B1B0
			private Dictionary<string, object> ParseObject()
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				this.json.Read();
				for (;;)
				{
					Json.Parser.TOKEN nextToken = this.NextToken;
					if (nextToken == Json.Parser.TOKEN.NONE)
					{
						break;
					}
					if (nextToken == Json.Parser.TOKEN.CURLY_CLOSE)
					{
						return dictionary;
					}
					if (nextToken != Json.Parser.TOKEN.COMMA)
					{
						string text = this.ParseString();
						if (text == null)
						{
							goto Block_4;
						}
						if (this.NextToken != Json.Parser.TOKEN.COLON)
						{
							goto Block_5;
						}
						this.json.Read();
						dictionary[text] = this.ParseValue();
					}
				}
				return null;
				Block_4:
				return null;
				Block_5:
				return null;
			}

			// Token: 0x06005447 RID: 21575 RVA: 0x0024D018 File Offset: 0x0024B218
			private List<object> ParseArray()
			{
				List<object> list = new List<object>();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					Json.Parser.TOKEN nextToken = this.NextToken;
					if (nextToken == Json.Parser.TOKEN.NONE)
					{
						return null;
					}
					if (nextToken != Json.Parser.TOKEN.SQUARED_CLOSE)
					{
						if (nextToken != Json.Parser.TOKEN.COMMA)
						{
							object obj = this.ParseByToken(nextToken);
							list.Add(obj);
						}
					}
					else
					{
						flag = false;
					}
				}
				return list;
			}

			// Token: 0x06005448 RID: 21576 RVA: 0x0024D068 File Offset: 0x0024B268
			private object ParseValue()
			{
				Json.Parser.TOKEN nextToken = this.NextToken;
				return this.ParseByToken(nextToken);
			}

			// Token: 0x06005449 RID: 21577 RVA: 0x0024D084 File Offset: 0x0024B284
			private object ParseByToken(Json.Parser.TOKEN token)
			{
				switch (token)
				{
				case Json.Parser.TOKEN.CURLY_OPEN:
					return this.ParseObject();
				case Json.Parser.TOKEN.SQUARED_OPEN:
					return this.ParseArray();
				case Json.Parser.TOKEN.STRING:
					return this.ParseString();
				case Json.Parser.TOKEN.NUMBER:
					return this.ParseNumber();
				case Json.Parser.TOKEN.TRUE:
					return true;
				case Json.Parser.TOKEN.FALSE:
					return false;
				case Json.Parser.TOKEN.NULL:
					return null;
				}
				return null;
			}

			// Token: 0x0600544A RID: 21578 RVA: 0x0024D0F4 File Offset: 0x0024B2F4
			private string ParseString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					if (this.json.Peek() == -1)
					{
						break;
					}
					char c = this.NextChar;
					if (c != '"')
					{
						if (c != '\\')
						{
							stringBuilder.Append(c);
						}
						else if (this.json.Peek() == -1)
						{
							flag = false;
						}
						else
						{
							c = this.NextChar;
							if (c <= '\\')
							{
								if (c == '"' || c == '/' || c == '\\')
								{
									stringBuilder.Append(c);
								}
							}
							else if (c <= 'f')
							{
								if (c != 'b')
								{
									if (c == 'f')
									{
										stringBuilder.Append('\f');
									}
								}
								else
								{
									stringBuilder.Append('\b');
								}
							}
							else if (c != 'n')
							{
								switch (c)
								{
								case 'r':
									stringBuilder.Append('\r');
									break;
								case 't':
									stringBuilder.Append('\t');
									break;
								case 'u':
								{
									char[] array = new char[4];
									for (int i = 0; i < 4; i++)
									{
										array[i] = this.NextChar;
									}
									stringBuilder.Append((char)Convert.ToInt32(new string(array), 16));
									break;
								}
								}
							}
							else
							{
								stringBuilder.Append('\n');
							}
						}
					}
					else
					{
						flag = false;
					}
				}
				return stringBuilder.ToString();
			}

			// Token: 0x0600544B RID: 21579 RVA: 0x0024D244 File Offset: 0x0024B444
			private object ParseNumber()
			{
				string nextWord = this.NextWord;
				if (nextWord.IndexOf('.') == -1)
				{
					long num;
					long.TryParse(nextWord, out num);
					return num;
				}
				double num2;
				double.TryParse(nextWord, out num2);
				return num2;
			}

			// Token: 0x0600544C RID: 21580 RVA: 0x0024D282 File Offset: 0x0024B482
			private void EatWhitespace()
			{
				while (char.IsWhiteSpace(this.PeekChar))
				{
					this.json.Read();
					if (this.json.Peek() == -1)
					{
						break;
					}
				}
			}

			// Token: 0x17000C11 RID: 3089
			// (get) Token: 0x0600544D RID: 21581 RVA: 0x0024D2AD File Offset: 0x0024B4AD
			private char PeekChar
			{
				get
				{
					return Convert.ToChar(this.json.Peek());
				}
			}

			// Token: 0x17000C12 RID: 3090
			// (get) Token: 0x0600544E RID: 21582 RVA: 0x0024D2BF File Offset: 0x0024B4BF
			private char NextChar
			{
				get
				{
					return Convert.ToChar(this.json.Read());
				}
			}

			// Token: 0x17000C13 RID: 3091
			// (get) Token: 0x0600544F RID: 21583 RVA: 0x0024D2D4 File Offset: 0x0024B4D4
			private string NextWord
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					while (!Json.Parser.IsWordBreak(this.PeekChar))
					{
						stringBuilder.Append(this.NextChar);
						if (this.json.Peek() == -1)
						{
							break;
						}
					}
					return stringBuilder.ToString();
				}
			}

			// Token: 0x17000C14 RID: 3092
			// (get) Token: 0x06005450 RID: 21584 RVA: 0x0024D318 File Offset: 0x0024B518
			private Json.Parser.TOKEN NextToken
			{
				get
				{
					this.EatWhitespace();
					if (this.json.Peek() == -1)
					{
						return Json.Parser.TOKEN.NONE;
					}
					char peekChar = this.PeekChar;
					if (peekChar <= '[')
					{
						switch (peekChar)
						{
						case '"':
							return Json.Parser.TOKEN.STRING;
						case '#':
						case '$':
						case '%':
						case '&':
						case '\'':
						case '(':
						case ')':
						case '*':
						case '+':
						case '.':
						case '/':
							break;
						case ',':
							this.json.Read();
							return Json.Parser.TOKEN.COMMA;
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
							return Json.Parser.TOKEN.NUMBER;
						case ':':
							return Json.Parser.TOKEN.COLON;
						default:
							if (peekChar == '[')
							{
								return Json.Parser.TOKEN.SQUARED_OPEN;
							}
							break;
						}
					}
					else
					{
						if (peekChar == ']')
						{
							this.json.Read();
							return Json.Parser.TOKEN.SQUARED_CLOSE;
						}
						if (peekChar == '{')
						{
							return Json.Parser.TOKEN.CURLY_OPEN;
						}
						if (peekChar == '}')
						{
							this.json.Read();
							return Json.Parser.TOKEN.CURLY_CLOSE;
						}
					}
					string nextWord = this.NextWord;
					if (nextWord == "false")
					{
						return Json.Parser.TOKEN.FALSE;
					}
					if (nextWord == "true")
					{
						return Json.Parser.TOKEN.TRUE;
					}
					if (!(nextWord == "null"))
					{
						return Json.Parser.TOKEN.NONE;
					}
					return Json.Parser.TOKEN.NULL;
				}
			}

			// Token: 0x04005DB2 RID: 23986
			private const string WORD_BREAK = "{}[],:\"";

			// Token: 0x04005DB3 RID: 23987
			private StringReader json;

			// Token: 0x0200122D RID: 4653
			private enum TOKEN
			{
				// Token: 0x04006383 RID: 25475
				NONE,
				// Token: 0x04006384 RID: 25476
				CURLY_OPEN,
				// Token: 0x04006385 RID: 25477
				CURLY_CLOSE,
				// Token: 0x04006386 RID: 25478
				SQUARED_OPEN,
				// Token: 0x04006387 RID: 25479
				SQUARED_CLOSE,
				// Token: 0x04006388 RID: 25480
				COLON,
				// Token: 0x04006389 RID: 25481
				COMMA,
				// Token: 0x0400638A RID: 25482
				STRING,
				// Token: 0x0400638B RID: 25483
				NUMBER,
				// Token: 0x0400638C RID: 25484
				TRUE,
				// Token: 0x0400638D RID: 25485
				FALSE,
				// Token: 0x0400638E RID: 25486
				NULL
			}
		}

		// Token: 0x020010F5 RID: 4341
		private sealed class Serializer
		{
			// Token: 0x06005451 RID: 21585 RVA: 0x0024D43A File Offset: 0x0024B63A
			private Serializer()
			{
				this.builder = new StringBuilder();
			}

			// Token: 0x06005452 RID: 21586 RVA: 0x0024D44D File Offset: 0x0024B64D
			public static string Serialize(object obj)
			{
				Json.Serializer serializer = new Json.Serializer();
				serializer.SerializeValue(obj);
				return serializer.builder.ToString();
			}

			// Token: 0x06005453 RID: 21587 RVA: 0x0024D468 File Offset: 0x0024B668
			private void SerializeValue(object value)
			{
				if (value == null)
				{
					this.builder.Append("null");
					return;
				}
				string text;
				if ((text = value as string) != null)
				{
					this.SerializeString(text);
					return;
				}
				if (value is bool)
				{
					this.builder.Append(((bool)value) ? "true" : "false");
					return;
				}
				IList list;
				if ((list = value as IList) != null)
				{
					this.SerializeArray(list);
					return;
				}
				IDictionary dictionary;
				if ((dictionary = value as IDictionary) != null)
				{
					this.SerializeObject(dictionary);
					return;
				}
				if (value is char)
				{
					this.SerializeString(new string((char)value, 1));
					return;
				}
				this.SerializeOther(value);
			}

			// Token: 0x06005454 RID: 21588 RVA: 0x0024D50C File Offset: 0x0024B70C
			private void SerializeObject(IDictionary obj)
			{
				bool flag = true;
				this.builder.Append('{');
				foreach (object obj2 in obj.Keys)
				{
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeString(obj2.ToString());
					this.builder.Append(':');
					this.SerializeValue(obj[obj2]);
					flag = false;
				}
				this.builder.Append('}');
			}

			// Token: 0x06005455 RID: 21589 RVA: 0x0024D5B4 File Offset: 0x0024B7B4
			private void SerializeArray(IList anArray)
			{
				this.builder.Append('[');
				bool flag = true;
				foreach (object obj in anArray)
				{
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeValue(obj);
					flag = false;
				}
				this.builder.Append(']');
			}

			// Token: 0x06005456 RID: 21590 RVA: 0x0024D634 File Offset: 0x0024B834
			private void SerializeString(string str)
			{
				this.builder.Append('"');
				char[] array = str.ToCharArray();
				int i = 0;
				while (i < array.Length)
				{
					char c = array[i];
					switch (c)
					{
					case '\b':
						this.builder.Append("\\b");
						break;
					case '\t':
						this.builder.Append("\\t");
						break;
					case '\n':
						this.builder.Append("\\n");
						break;
					case '\v':
						goto IL_00E0;
					case '\f':
						this.builder.Append("\\f");
						break;
					case '\r':
						this.builder.Append("\\r");
						break;
					default:
						if (c != '"')
						{
							if (c != '\\')
							{
								goto IL_00E0;
							}
							this.builder.Append("\\\\");
						}
						else
						{
							this.builder.Append("\\\"");
						}
						break;
					}
					IL_0129:
					i++;
					continue;
					IL_00E0:
					int num = Convert.ToInt32(c);
					if (num >= 32 && num <= 126)
					{
						this.builder.Append(c);
						goto IL_0129;
					}
					this.builder.Append("\\u");
					this.builder.Append(num.ToString("x4"));
					goto IL_0129;
				}
				this.builder.Append('"');
			}

			// Token: 0x06005457 RID: 21591 RVA: 0x0024D788 File Offset: 0x0024B988
			private void SerializeOther(object value)
			{
				if (value is float)
				{
					this.builder.Append(((float)value).ToString("R"));
					return;
				}
				if (value is int || value is uint || value is long || value is sbyte || value is byte || value is short || value is ushort || value is ulong)
				{
					this.builder.Append(value);
					return;
				}
				if (value is double || value is decimal)
				{
					this.builder.Append(Convert.ToDouble(value).ToString("R"));
					return;
				}
				this.SerializeString(value.ToString());
			}

			// Token: 0x04005DB4 RID: 23988
			private StringBuilder builder;
		}
	}
}
