using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Rilisoft.MiniJson
{
	// Token: 0x020006C0 RID: 1728
	public static class Json
	{
		// Token: 0x06003C37 RID: 15415 RVA: 0x00138504 File Offset: 0x00136704
		public static object Deserialize(string json)
		{
			if (string.IsNullOrEmpty(json))
			{
				return null;
			}
			return Json.Parser.Parse(json);
		}

		// Token: 0x06003C38 RID: 15416 RVA: 0x0013851C File Offset: 0x0013671C
		public static string Serialize(object obj)
		{
			return Json.Serializer.Serialize(obj);
		}

		// Token: 0x020006C1 RID: 1729
		private sealed class Parser : IDisposable
		{
			// Token: 0x06003C39 RID: 15417 RVA: 0x00138524 File Offset: 0x00136724
			private Parser(string jsonString)
			{
				this.json = new StringReader(jsonString);
			}

			// Token: 0x06003C3A RID: 15418 RVA: 0x00138538 File Offset: 0x00136738
			public static bool IsWordBreak(char c)
			{
				return char.IsWhiteSpace(c) || "{}[],:\"".IndexOf(c) != -1;
			}

			// Token: 0x06003C3B RID: 15419 RVA: 0x0013855C File Offset: 0x0013675C
			public static object Parse(string jsonString)
			{
				object result;
				using (Json.Parser parser = new Json.Parser(jsonString))
				{
					result = parser.ParseValue();
				}
				return result;
			}

			// Token: 0x06003C3C RID: 15420 RVA: 0x001385AC File Offset: 0x001367AC
			public void Dispose()
			{
				this.json.Dispose();
				this.json = null;
			}

			// Token: 0x06003C3D RID: 15421 RVA: 0x001385C0 File Offset: 0x001367C0
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

			// Token: 0x06003C3E RID: 15422 RVA: 0x0013864C File Offset: 0x0013684C
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

			// Token: 0x06003C3F RID: 15423 RVA: 0x001386C8 File Offset: 0x001368C8
			private object ParseValue()
			{
				Json.Parser.TOKEN nextToken = this.NextToken;
				return this.ParseByToken(nextToken);
			}

			// Token: 0x06003C40 RID: 15424 RVA: 0x001386E4 File Offset: 0x001368E4
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

			// Token: 0x06003C41 RID: 15425 RVA: 0x0013875C File Offset: 0x0013695C
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
					}
					else
					{
						flag = false;
					}
				}
				return stringBuilder.ToString();
			}

			// Token: 0x06003C42 RID: 15426 RVA: 0x001388F4 File Offset: 0x00136AF4
			private object ParseNumber()
			{
				string nextWord = this.NextWord;
				if (nextWord.IndexOf('.') == -1)
				{
					long num;
					long.TryParse(nextWord, NumberStyles.Number, CultureInfo.InvariantCulture, out num);
					return num;
				}
				double num2;
				double.TryParse(nextWord, NumberStyles.Number, CultureInfo.InvariantCulture, out num2);
				return num2;
			}

			// Token: 0x06003C43 RID: 15427 RVA: 0x00138944 File Offset: 0x00136B44
			private void EatWhitespace()
			{
				if (this.json.Peek() == -1)
				{
					return;
				}
				while (char.IsWhiteSpace(this.PeekChar))
				{
					this.json.Read();
					if (this.json.Peek() == -1)
					{
						break;
					}
				}
			}

			// Token: 0x170009EC RID: 2540
			// (get) Token: 0x06003C44 RID: 15428 RVA: 0x0013899C File Offset: 0x00136B9C
			private char PeekChar
			{
				get
				{
					int num = this.json.Peek();
					char result;
					try
					{
						result = Convert.ToChar(num);
					}
					catch (OverflowException ex)
					{
						ex.Data.Add("Character", num);
						throw ex;
					}
					return result;
				}
			}

			// Token: 0x170009ED RID: 2541
			// (get) Token: 0x06003C45 RID: 15429 RVA: 0x00138A04 File Offset: 0x00136C04
			private char NextChar
			{
				get
				{
					return Convert.ToChar(this.json.Read());
				}
			}

			// Token: 0x170009EE RID: 2542
			// (get) Token: 0x06003C46 RID: 15430 RVA: 0x00138A18 File Offset: 0x00136C18
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

			// Token: 0x170009EF RID: 2543
			// (get) Token: 0x06003C47 RID: 15431 RVA: 0x00138A6C File Offset: 0x00136C6C
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
					switch (peekChar)
					{
					case '"':
						return Json.Parser.TOKEN.STRING;
					default:
						switch (peekChar)
						{
						case '[':
							return Json.Parser.TOKEN.SQUARED_OPEN;
						default:
						{
							switch (peekChar)
							{
							case '{':
								return Json.Parser.TOKEN.CURLY_OPEN;
							case '}':
								this.json.Read();
								return Json.Parser.TOKEN.CURLY_CLOSE;
							}
							string nextWord = this.NextWord;
							switch (nextWord)
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

			// Token: 0x04002C77 RID: 11383
			private const string WORD_BREAK = "{}[],:\"";

			// Token: 0x04002C78 RID: 11384
			private StringReader json;

			// Token: 0x020006C2 RID: 1730
			private enum TOKEN
			{
				// Token: 0x04002C7B RID: 11387
				NONE,
				// Token: 0x04002C7C RID: 11388
				CURLY_OPEN,
				// Token: 0x04002C7D RID: 11389
				CURLY_CLOSE,
				// Token: 0x04002C7E RID: 11390
				SQUARED_OPEN,
				// Token: 0x04002C7F RID: 11391
				SQUARED_CLOSE,
				// Token: 0x04002C80 RID: 11392
				COLON,
				// Token: 0x04002C81 RID: 11393
				COMMA,
				// Token: 0x04002C82 RID: 11394
				STRING,
				// Token: 0x04002C83 RID: 11395
				NUMBER,
				// Token: 0x04002C84 RID: 11396
				TRUE,
				// Token: 0x04002C85 RID: 11397
				FALSE,
				// Token: 0x04002C86 RID: 11398
				NULL
			}
		}

		// Token: 0x020006C3 RID: 1731
		private sealed class Serializer
		{
			// Token: 0x06003C48 RID: 15432 RVA: 0x00138BE4 File Offset: 0x00136DE4
			private Serializer()
			{
				this.builder = new StringBuilder();
			}

			// Token: 0x06003C49 RID: 15433 RVA: 0x00138BF8 File Offset: 0x00136DF8
			public static string Serialize(object obj)
			{
				Json.Serializer serializer = new Json.Serializer();
				serializer.SerializeValue(obj);
				return serializer.builder.ToString();
			}

			// Token: 0x06003C4A RID: 15434 RVA: 0x00138C20 File Offset: 0x00136E20
			private void SerializeValue(object value)
			{
				string str;
				IList anArray;
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
					this.builder.Append((!(bool)value) ? "false" : "true");
				}
				else if ((anArray = (value as IList)) != null)
				{
					this.SerializeArray(anArray);
				}
				else if ((obj = (value as IDictionary)) != null)
				{
					this.SerializeObject(obj);
				}
				else if (value is char)
				{
					this.SerializeString(new string((char)value, 1));
				}
				else
				{
					this.SerializeOther(value);
				}
			}

			// Token: 0x06003C4B RID: 15435 RVA: 0x00138CF4 File Offset: 0x00136EF4
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

			// Token: 0x06003C4C RID: 15436 RVA: 0x00138DB4 File Offset: 0x00136FB4
			private void SerializeArray(IList anArray)
			{
				this.builder.Append('[');
				bool flag = true;
				foreach (object value in anArray)
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

			// Token: 0x06003C4D RID: 15437 RVA: 0x00138E50 File Offset: 0x00137050
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
									this.builder.Append("\\u");
									this.builder.Append(num.ToString("x4"));
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

			// Token: 0x06003C4E RID: 15438 RVA: 0x00138FCC File Offset: 0x001371CC
			private void SerializeOther(object value)
			{
				if (value is float)
				{
					this.builder.Append(((float)value).ToString("R", CultureInfo.InvariantCulture));
				}
				else if (value is int || value is uint || value is long || value is sbyte || value is byte || value is short || value is ushort || value is ulong)
				{
					this.builder.Append(Convert.ToInt64(value).ToString(CultureInfo.InvariantCulture));
				}
				else if (value is double || value is decimal)
				{
					this.builder.Append(Convert.ToDouble(value).ToString("R", CultureInfo.InvariantCulture));
				}
				else
				{
					this.SerializeString(value.ToString());
				}
			}

			// Token: 0x04002C87 RID: 11399
			private StringBuilder builder;
		}
	}
}
