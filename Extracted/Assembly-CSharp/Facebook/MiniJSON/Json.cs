using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Facebook.MiniJSON
{
	// Token: 0x02000110 RID: 272
	public static class Json
	{
		// Token: 0x060007E7 RID: 2023 RVA: 0x0002FD30 File Offset: 0x0002DF30
		public static object Deserialize(string json)
		{
			if (json == null)
			{
				return null;
			}
			return Json.Parser.Parse(json);
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0002FD40 File Offset: 0x0002DF40
		public static string Serialize(object obj)
		{
			return Json.Serializer.Serialize(obj);
		}

		// Token: 0x04000691 RID: 1681
		private static NumberFormatInfo numberFormat = new CultureInfo("en-US").NumberFormat;

		// Token: 0x02000111 RID: 273
		private sealed class Parser : IDisposable
		{
			// Token: 0x060007E9 RID: 2025 RVA: 0x0002FD48 File Offset: 0x0002DF48
			private Parser(string jsonString)
			{
				this.json = new StringReader(jsonString);
			}

			// Token: 0x170000E1 RID: 225
			// (get) Token: 0x060007EA RID: 2026 RVA: 0x0002FD5C File Offset: 0x0002DF5C
			private char PeekChar
			{
				get
				{
					return Convert.ToChar(this.json.Peek());
				}
			}

			// Token: 0x170000E2 RID: 226
			// (get) Token: 0x060007EB RID: 2027 RVA: 0x0002FD70 File Offset: 0x0002DF70
			private char NextChar
			{
				get
				{
					return Convert.ToChar(this.json.Read());
				}
			}

			// Token: 0x170000E3 RID: 227
			// (get) Token: 0x060007EC RID: 2028 RVA: 0x0002FD84 File Offset: 0x0002DF84
			private string NextWord
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					while (" \t\n\r{}[],:\"".IndexOf(this.PeekChar) == -1)
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

			// Token: 0x170000E4 RID: 228
			// (get) Token: 0x060007ED RID: 2029 RVA: 0x0002FDDC File Offset: 0x0002DFDC
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
					char c = peekChar;
					switch (c)
					{
					case '"':
						return Json.Parser.TOKEN.STRING;
					default:
						switch (c)
						{
						case '[':
							return Json.Parser.TOKEN.SQUARED_OPEN;
						default:
						{
							switch (c)
							{
							case '{':
								return Json.Parser.TOKEN.CURLY_OPEN;
							case '}':
								this.json.Read();
								return Json.Parser.TOKEN.CURLY_CLOSE;
							}
							string nextWord = this.NextWord;
							string text = nextWord;
							switch (text)
							{
							case "false":
								return Json.Parser.TOKEN.FALSE;
							case "true":
								return Json.Parser.TOKEN.TRUE;
							case "null":
								return Json.Parser.TOKEN.NULL;
							}
							return Json.Parser.TOKEN.NONE;
						}
						case ']':
							this.json.Read();
							return Json.Parser.TOKEN.SQUARED_CLOSE;
						}
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
					}
				}
			}

			// Token: 0x060007EE RID: 2030 RVA: 0x0002FF60 File Offset: 0x0002E160
			public static object Parse(string jsonString)
			{
				object result;
				using (Json.Parser parser = new Json.Parser(jsonString))
				{
					result = parser.ParseValue();
				}
				return result;
			}

			// Token: 0x060007EF RID: 2031 RVA: 0x0002FFB0 File Offset: 0x0002E1B0
			public void Dispose()
			{
				this.json.Dispose();
				this.json = null;
			}

			// Token: 0x060007F0 RID: 2032 RVA: 0x0002FFC4 File Offset: 0x0002E1C4
			private Dictionary<string, object> ParseObject()
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				this.json.Read();
				for (;;)
				{
					Json.Parser.TOKEN nextToken = this.NextToken;
					switch (nextToken)
					{
					case Json.Parser.TOKEN.NONE:
						goto IL_37;
					default:
						if (nextToken != Json.Parser.TOKEN.COMMA)
						{
							string text = this.ParseString();
							if (text == null)
							{
								goto Block_2;
							}
							if (this.NextToken != Json.Parser.TOKEN.COLON)
							{
								goto Block_3;
							}
							this.json.Read();
							dictionary[text] = this.ParseValue();
						}
						break;
					case Json.Parser.TOKEN.CURLY_CLOSE:
						return dictionary;
					}
				}
				IL_37:
				return null;
				Block_2:
				return null;
				Block_3:
				return null;
			}

			// Token: 0x060007F1 RID: 2033 RVA: 0x00030050 File Offset: 0x0002E250
			private List<object> ParseArray()
			{
				List<object> list = new List<object>();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					Json.Parser.TOKEN nextToken = this.NextToken;
					Json.Parser.TOKEN token = nextToken;
					switch (token)
					{
					case Json.Parser.TOKEN.SQUARED_CLOSE:
						flag = false;
						break;
					default:
					{
						if (token == Json.Parser.TOKEN.NONE)
						{
							return null;
						}
						object item = this.ParseByToken(nextToken);
						list.Add(item);
						break;
					}
					case Json.Parser.TOKEN.COMMA:
						break;
					}
				}
				return list;
			}

			// Token: 0x060007F2 RID: 2034 RVA: 0x000300CC File Offset: 0x0002E2CC
			private object ParseValue()
			{
				Json.Parser.TOKEN nextToken = this.NextToken;
				return this.ParseByToken(nextToken);
			}

			// Token: 0x060007F3 RID: 2035 RVA: 0x000300E8 File Offset: 0x0002E2E8
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

			// Token: 0x060007F4 RID: 2036 RVA: 0x00030160 File Offset: 0x0002E360
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
					char nextChar = this.NextChar;
					char c = nextChar;
					if (c != '"')
					{
						if (c != '\\')
						{
							stringBuilder.Append(nextChar);
						}
						else if (this.json.Peek() == -1)
						{
							flag = false;
						}
						else
						{
							nextChar = this.NextChar;
							char c2 = nextChar;
							switch (c2)
							{
							case 'n':
								stringBuilder.Append('\n');
								break;
							default:
								if (c2 != '"' && c2 != '/' && c2 != '\\')
								{
									if (c2 != 'b')
									{
										if (c2 == 'f')
										{
											stringBuilder.Append('\f');
										}
									}
									else
									{
										stringBuilder.Append('\b');
									}
								}
								else
								{
									stringBuilder.Append(nextChar);
								}
								break;
							case 'r':
								stringBuilder.Append('\r');
								break;
							case 't':
								stringBuilder.Append('\t');
								break;
							case 'u':
							{
								StringBuilder stringBuilder2 = new StringBuilder();
								for (int i = 0; i < 4; i++)
								{
									stringBuilder2.Append(this.NextChar);
								}
								stringBuilder.Append((char)Convert.ToInt32(stringBuilder2.ToString(), 16));
								break;
							}
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

			// Token: 0x060007F5 RID: 2037 RVA: 0x000302F8 File Offset: 0x0002E4F8
			private object ParseNumber()
			{
				string nextWord = this.NextWord;
				if (nextWord.IndexOf('.') == -1)
				{
					return long.Parse(nextWord, Json.numberFormat);
				}
				return double.Parse(nextWord, Json.numberFormat);
			}

			// Token: 0x060007F6 RID: 2038 RVA: 0x0003033C File Offset: 0x0002E53C
			private void EatWhitespace()
			{
				while (" \t\n\r".IndexOf(this.PeekChar) != -1)
				{
					this.json.Read();
					if (this.json.Peek() == -1)
					{
						break;
					}
				}
			}

			// Token: 0x04000692 RID: 1682
			private const string WhiteSpace = " \t\n\r";

			// Token: 0x04000693 RID: 1683
			private const string WordBreak = " \t\n\r{}[],:\"";

			// Token: 0x04000694 RID: 1684
			private StringReader json;

			// Token: 0x02000112 RID: 274
			private enum TOKEN
			{
				// Token: 0x04000697 RID: 1687
				NONE,
				// Token: 0x04000698 RID: 1688
				CURLY_OPEN,
				// Token: 0x04000699 RID: 1689
				CURLY_CLOSE,
				// Token: 0x0400069A RID: 1690
				SQUARED_OPEN,
				// Token: 0x0400069B RID: 1691
				SQUARED_CLOSE,
				// Token: 0x0400069C RID: 1692
				COLON,
				// Token: 0x0400069D RID: 1693
				COMMA,
				// Token: 0x0400069E RID: 1694
				STRING,
				// Token: 0x0400069F RID: 1695
				NUMBER,
				// Token: 0x040006A0 RID: 1696
				TRUE,
				// Token: 0x040006A1 RID: 1697
				FALSE,
				// Token: 0x040006A2 RID: 1698
				NULL
			}
		}

		// Token: 0x02000113 RID: 275
		private sealed class Serializer
		{
			// Token: 0x060007F7 RID: 2039 RVA: 0x00030388 File Offset: 0x0002E588
			private Serializer()
			{
				this.builder = new StringBuilder();
			}

			// Token: 0x060007F8 RID: 2040 RVA: 0x0003039C File Offset: 0x0002E59C
			public static string Serialize(object obj)
			{
				Json.Serializer serializer = new Json.Serializer();
				serializer.SerializeValue(obj);
				return serializer.builder.ToString();
			}

			// Token: 0x060007F9 RID: 2041 RVA: 0x000303C4 File Offset: 0x0002E5C4
			private void SerializeValue(object value)
			{
				string str;
				IList array;
				IDictionary obj;
				if (value == null)
				{
					this.builder.Append("null");
				}
				else if ((str = (value as string)) != null)
				{
					this.SerializeString(str);
				}
				else if (value is bool)
				{
					this.builder.Append(value.ToString().ToLower());
				}
				else if ((array = (value as IList)) != null)
				{
					this.SerializeArray(array);
				}
				else if ((obj = (value as IDictionary)) != null)
				{
					this.SerializeObject(obj);
				}
				else if (value is char)
				{
					this.SerializeString(value.ToString());
				}
				else
				{
					this.SerializeOther(value);
				}
			}

			// Token: 0x060007FA RID: 2042 RVA: 0x00030484 File Offset: 0x0002E684
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

			// Token: 0x060007FB RID: 2043 RVA: 0x00030544 File Offset: 0x0002E744
			private void SerializeArray(IList array)
			{
				this.builder.Append('[');
				bool flag = true;
				foreach (object value in array)
				{
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeValue(value);
					flag = false;
				}
				this.builder.Append(']');
			}

			// Token: 0x060007FC RID: 2044 RVA: 0x000305E0 File Offset: 0x0002E7E0
			private void SerializeString(string str)
			{
				this.builder.Append('"');
				char[] array = str.ToCharArray();
				foreach (char c in array)
				{
					char c2 = c;
					switch (c2)
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
					default:
						if (c2 != '"')
						{
							if (c2 != '\\')
							{
								int num = Convert.ToInt32(c);
								if (num >= 32 && num <= 126)
								{
									this.builder.Append(c);
								}
								else
								{
									this.builder.Append("\\u" + Convert.ToString(num, 16).PadLeft(4, '0'));
								}
							}
							else
							{
								this.builder.Append("\\\\");
							}
						}
						else
						{
							this.builder.Append("\\\"");
						}
						break;
					case '\f':
						this.builder.Append("\\f");
						break;
					case '\r':
						this.builder.Append("\\r");
						break;
					}
				}
				this.builder.Append('"');
			}

			// Token: 0x060007FD RID: 2045 RVA: 0x00030758 File Offset: 0x0002E958
			private void SerializeOther(object value)
			{
				if (value is float || value is int || value is uint || value is long || value is double || value is sbyte || value is byte || value is short || value is ushort || value is ulong || value is decimal)
				{
					this.builder.Append(value.ToString());
				}
				else
				{
					this.SerializeString(value.ToString());
				}
			}

			// Token: 0x040006A3 RID: 1699
			private StringBuilder builder;
		}
	}
}
