using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SimpleJSON
{
	// Token: 0x020002B5 RID: 693
	public class JSONNode
	{
		// Token: 0x060015AB RID: 5547 RVA: 0x00057B9C File Offset: 0x00055D9C
		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		// Token: 0x17000265 RID: 613
		public virtual JSONNode this[int aIndex]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000266 RID: 614
		public virtual JSONNode this[string aKey]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060015B0 RID: 5552 RVA: 0x00057BB0 File Offset: 0x00055DB0
		// (set) Token: 0x060015B1 RID: 5553 RVA: 0x00057BB8 File Offset: 0x00055DB8
		public virtual string Value
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060015B2 RID: 5554 RVA: 0x00057BBC File Offset: 0x00055DBC
		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x00057BC0 File Offset: 0x00055DC0
		public virtual void Add(JSONNode aItem)
		{
			this.Add(string.Empty, aItem);
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x00057BD0 File Offset: 0x00055DD0
		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x00057BD4 File Offset: 0x00055DD4
		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x00057BD8 File Offset: 0x00055DD8
		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060015B7 RID: 5559 RVA: 0x00057BDC File Offset: 0x00055DDC
		public virtual IEnumerable<JSONNode> Childs
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060015B8 RID: 5560 RVA: 0x00057BF8 File Offset: 0x00055DF8
		public IEnumerable<JSONNode> DeepChilds
		{
			get
			{
				foreach (JSONNode C in this.Childs)
				{
					foreach (JSONNode D in C.DeepChilds)
					{
						yield return D;
					}
				}
				yield break;
			}
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x00057C1C File Offset: 0x00055E1C
		public override string ToString()
		{
			return "JSONNode";
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x00057C24 File Offset: 0x00055E24
		public virtual string ToString(string aPrefix)
		{
			return "JSONNode";
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060015BB RID: 5563 RVA: 0x00057C2C File Offset: 0x00055E2C
		// (set) Token: 0x060015BC RID: 5564 RVA: 0x00057C50 File Offset: 0x00055E50
		public virtual int AsInt
		{
			get
			{
				int result = 0;
				if (int.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060015BD RID: 5565 RVA: 0x00057C60 File Offset: 0x00055E60
		// (set) Token: 0x060015BE RID: 5566 RVA: 0x00057C8C File Offset: 0x00055E8C
		public virtual float AsFloat
		{
			get
			{
				float result = 0f;
				if (float.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0f;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060015BF RID: 5567 RVA: 0x00057C9C File Offset: 0x00055E9C
		// (set) Token: 0x060015C0 RID: 5568 RVA: 0x00057CD0 File Offset: 0x00055ED0
		public virtual double AsDouble
		{
			get
			{
				double result = 0.0;
				if (double.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0.0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x060015C1 RID: 5569 RVA: 0x00057CE0 File Offset: 0x00055EE0
		// (set) Token: 0x060015C2 RID: 5570 RVA: 0x00057D14 File Offset: 0x00055F14
		public virtual bool AsBool
		{
			get
			{
				bool result = false;
				if (bool.TryParse(this.Value, out result))
				{
					return result;
				}
				return !string.IsNullOrEmpty(this.Value);
			}
			set
			{
				this.Value = ((!value) ? "false" : "true");
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x060015C3 RID: 5571 RVA: 0x00057D34 File Offset: 0x00055F34
		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x060015C4 RID: 5572 RVA: 0x00057D3C File Offset: 0x00055F3C
		public virtual JSONClass AsObject
		{
			get
			{
				return this as JSONClass;
			}
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x00057D44 File Offset: 0x00055F44
		public override bool Equals(object obj)
		{
			return object.ReferenceEquals(this, obj);
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x00057D50 File Offset: 0x00055F50
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x00057D58 File Offset: 0x00055F58
		internal static string Escape(string aText)
		{
			string text = string.Empty;
			foreach (char c in aText)
			{
				char c2 = c;
				switch (c2)
				{
				case '\b':
					text += "\\b";
					break;
				case '\t':
					text += "\\t";
					break;
				case '\n':
					text += "\\n";
					break;
				default:
					if (c2 != '"')
					{
						if (c2 != '\\')
						{
							text += c;
						}
						else
						{
							text += "\\\\";
						}
					}
					else
					{
						text += "\\\"";
					}
					break;
				case '\f':
					text += "\\f";
					break;
				case '\r':
					text += "\\r";
					break;
				}
			}
			return text;
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x00057E54 File Offset: 0x00056054
		public static JSONNode Parse(string aJSON)
		{
			Stack<JSONNode> stack = new Stack<JSONNode>();
			JSONNode jsonnode = null;
			int i = 0;
			string text = string.Empty;
			string text2 = string.Empty;
			bool flag = false;
			while (i < aJSON.Length)
			{
				char c = aJSON[i];
				switch (c)
				{
				case '\t':
					goto IL_333;
				case '\n':
				case '\r':
					break;
				default:
					switch (c)
					{
					case ' ':
						goto IL_333;
					default:
						switch (c)
						{
						case '[':
							if (flag)
							{
								text += aJSON[i];
								goto IL_467;
							}
							stack.Push(new JSONArray());
							if (jsonnode != null)
							{
								text2 = text2.Trim();
								if (jsonnode is JSONArray)
								{
									jsonnode.Add(stack.Peek());
								}
								else if (text2 != string.Empty)
								{
									jsonnode.Add(text2, stack.Peek());
								}
							}
							text2 = string.Empty;
							text = string.Empty;
							jsonnode = stack.Peek();
							goto IL_467;
						case '\\':
							i++;
							if (flag)
							{
								char c2 = aJSON[i];
								char c3 = c2;
								switch (c3)
								{
								case 'n':
									text += '\n';
									break;
								default:
									if (c3 != 'b')
									{
										if (c3 != 'f')
										{
											text += c2;
										}
										else
										{
											text += '\f';
										}
									}
									else
									{
										text += '\b';
									}
									break;
								case 'r':
									text += '\r';
									break;
								case 't':
									text += '\t';
									break;
								case 'u':
								{
									string s = aJSON.Substring(i + 1, 4);
									text += (char)int.Parse(s, NumberStyles.AllowHexSpecifier);
									i += 4;
									break;
								}
								}
							}
							goto IL_467;
						case ']':
							break;
						default:
							switch (c)
							{
							case '{':
								if (flag)
								{
									text += aJSON[i];
									goto IL_467;
								}
								stack.Push(new JSONClass());
								if (jsonnode != null)
								{
									text2 = text2.Trim();
									if (jsonnode is JSONArray)
									{
										jsonnode.Add(stack.Peek());
									}
									else if (text2 != string.Empty)
									{
										jsonnode.Add(text2, stack.Peek());
									}
								}
								text2 = string.Empty;
								text = string.Empty;
								jsonnode = stack.Peek();
								goto IL_467;
							default:
								if (c != ',')
								{
									if (c != ':')
									{
										text += aJSON[i];
										goto IL_467;
									}
									if (flag)
									{
										text += aJSON[i];
										goto IL_467;
									}
									text2 = text;
									text = string.Empty;
									goto IL_467;
								}
								else
								{
									if (flag)
									{
										text += aJSON[i];
										goto IL_467;
									}
									if (text != string.Empty)
									{
										if (jsonnode is JSONArray)
										{
											jsonnode.Add(text);
										}
										else if (text2 != string.Empty)
										{
											jsonnode.Add(text2, text);
										}
									}
									text2 = string.Empty;
									text = string.Empty;
									goto IL_467;
								}
								break;
							case '}':
								break;
							}
							break;
						}
						if (flag)
						{
							text += aJSON[i];
						}
						else
						{
							if (stack.Count == 0)
							{
								throw new Exception("JSON Parse: Too many closing brackets");
							}
							stack.Pop();
							if (text != string.Empty)
							{
								text2 = text2.Trim();
								if (jsonnode is JSONArray)
								{
									jsonnode.Add(text);
								}
								else if (text2 != string.Empty)
								{
									jsonnode.Add(text2, text);
								}
							}
							text2 = string.Empty;
							text = string.Empty;
							if (stack.Count > 0)
							{
								jsonnode = stack.Peek();
							}
						}
						break;
					case '"':
						flag ^= true;
						break;
					}
					break;
				}
				IL_467:
				i++;
				continue;
				IL_333:
				if (flag)
				{
					text += aJSON[i];
				}
				goto IL_467;
			}
			if (flag)
			{
				throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
			}
			return jsonnode;
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x000582EC File Offset: 0x000564EC
		public virtual void Serialize(BinaryWriter aWriter)
		{
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x000582F0 File Offset: 0x000564F0
		public void SaveToStream(Stream aData)
		{
			BinaryWriter aWriter = new BinaryWriter(aData);
			this.Serialize(aWriter);
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x0005830C File Offset: 0x0005650C
		public void SaveToCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x00058318 File Offset: 0x00056518
		public void SaveToCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x00058324 File Offset: 0x00056524
		public string SaveToCompressedBase64()
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x00058330 File Offset: 0x00056530
		public void SaveToFile(string aFileName)
		{
			Directory.CreateDirectory(new FileInfo(aFileName).Directory.FullName);
			using (FileStream fileStream = File.OpenWrite(aFileName))
			{
				this.SaveToStream(fileStream);
			}
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x00058390 File Offset: 0x00056590
		public string SaveToBase64()
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				this.SaveToStream(memoryStream);
				memoryStream.Position = 0L;
				result = Convert.ToBase64String(memoryStream.ToArray());
			}
			return result;
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x000583F4 File Offset: 0x000565F4
		public static JSONNode Deserialize(BinaryReader aReader)
		{
			JSONBinaryTag jsonbinaryTag = (JSONBinaryTag)aReader.ReadByte();
			switch (jsonbinaryTag)
			{
			case JSONBinaryTag.Array:
			{
				int num = aReader.ReadInt32();
				JSONArray jsonarray = new JSONArray();
				for (int i = 0; i < num; i++)
				{
					jsonarray.Add(JSONNode.Deserialize(aReader));
				}
				return jsonarray;
			}
			case JSONBinaryTag.Class:
			{
				int num2 = aReader.ReadInt32();
				JSONClass jsonclass = new JSONClass();
				for (int j = 0; j < num2; j++)
				{
					string aKey = aReader.ReadString();
					JSONNode aItem = JSONNode.Deserialize(aReader);
					jsonclass.Add(aKey, aItem);
				}
				return jsonclass;
			}
			case JSONBinaryTag.Value:
				return new JSONData(aReader.ReadString());
			case JSONBinaryTag.IntValue:
				return new JSONData(aReader.ReadInt32());
			case JSONBinaryTag.DoubleValue:
				return new JSONData(aReader.ReadDouble());
			case JSONBinaryTag.BoolValue:
				return new JSONData(aReader.ReadBoolean());
			case JSONBinaryTag.FloatValue:
				return new JSONData(aReader.ReadSingle());
			default:
				throw new Exception("Error deserializing JSON. Unknown tag: " + jsonbinaryTag);
			}
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x000584F8 File Offset: 0x000566F8
		public static JSONNode LoadFromCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x00058504 File Offset: 0x00056704
		public static JSONNode LoadFromCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x00058510 File Offset: 0x00056710
		public static JSONNode LoadFromCompressedBase64(string aBase64)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x0005851C File Offset: 0x0005671C
		public static JSONNode LoadFromStream(Stream aData)
		{
			JSONNode result;
			using (BinaryReader binaryReader = new BinaryReader(aData))
			{
				result = JSONNode.Deserialize(binaryReader);
			}
			return result;
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x0005856C File Offset: 0x0005676C
		public static JSONNode LoadFromFile(string aFileName)
		{
			JSONNode result;
			using (FileStream fileStream = File.OpenRead(aFileName))
			{
				result = JSONNode.LoadFromStream(fileStream);
			}
			return result;
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x000585BC File Offset: 0x000567BC
		public static JSONNode LoadFromBase64(string aBase64)
		{
			byte[] buffer = Convert.FromBase64String(aBase64);
			return JSONNode.LoadFromStream(new MemoryStream(buffer)
			{
				Position = 0L
			});
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x000585E8 File Offset: 0x000567E8
		public static implicit operator JSONNode(string s)
		{
			return new JSONData(s);
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x000585F0 File Offset: 0x000567F0
		public static implicit operator string(JSONNode d)
		{
			return (!(d == null)) ? d.Value : null;
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x0005860C File Offset: 0x0005680C
		public static bool operator ==(JSONNode a, object b)
		{
			return (b == null && a is JSONLazyCreator) || object.ReferenceEquals(a, b);
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x00058628 File Offset: 0x00056828
		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}
	}
}
