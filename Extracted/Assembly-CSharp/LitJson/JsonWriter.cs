using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace LitJson
{
	// Token: 0x0200014D RID: 333
	public class JsonWriter
	{
		// Token: 0x06000AEF RID: 2799 RVA: 0x0003DC24 File Offset: 0x0003BE24
		public JsonWriter()
		{
			this.inst_string_builder = new StringBuilder();
			this.writer = new StringWriter(this.inst_string_builder);
			this.Init();
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0003DC5C File Offset: 0x0003BE5C
		public JsonWriter(StringBuilder sb) : this(new StringWriter(sb))
		{
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0003DC6C File Offset: 0x0003BE6C
		public JsonWriter(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
			this.Init();
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x0003DCAC File Offset: 0x0003BEAC
		// (set) Token: 0x06000AF4 RID: 2804 RVA: 0x0003DCB4 File Offset: 0x0003BEB4
		public int IndentValue
		{
			get
			{
				return this.indent_value;
			}
			set
			{
				this.indentation = this.indentation / this.indent_value * value;
				this.indent_value = value;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x0003DCD4 File Offset: 0x0003BED4
		// (set) Token: 0x06000AF6 RID: 2806 RVA: 0x0003DCDC File Offset: 0x0003BEDC
		public bool PrettyPrint
		{
			get
			{
				return this.pretty_print;
			}
			set
			{
				this.pretty_print = value;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x0003DCE8 File Offset: 0x0003BEE8
		public TextWriter TextWriter
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x0003DCF0 File Offset: 0x0003BEF0
		// (set) Token: 0x06000AF9 RID: 2809 RVA: 0x0003DCF8 File Offset: 0x0003BEF8
		public bool Validate
		{
			get
			{
				return this.validate;
			}
			set
			{
				this.validate = value;
			}
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0003DD04 File Offset: 0x0003BF04
		private void DoValidation(Condition cond)
		{
			if (!this.context.ExpectingValue)
			{
				this.context.Count++;
			}
			if (!this.validate)
			{
				return;
			}
			if (this.has_reached_end)
			{
				throw new JsonException("A complete JSON symbol has already been written");
			}
			switch (cond)
			{
			case Condition.InArray:
				if (!this.context.InArray)
				{
					throw new JsonException("Can't close an array here");
				}
				break;
			case Condition.InObject:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't close an object here");
				}
				break;
			case Condition.NotAProperty:
				if (this.context.InObject && !this.context.ExpectingValue)
				{
					throw new JsonException("Expected a property");
				}
				break;
			case Condition.Property:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't add a property here");
				}
				break;
			case Condition.Value:
				if (!this.context.InArray && (!this.context.InObject || !this.context.ExpectingValue))
				{
					throw new JsonException("Can't add a value here");
				}
				break;
			}
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0003DE68 File Offset: 0x0003C068
		private void Init()
		{
			this.has_reached_end = false;
			this.hex_seq = new char[4];
			this.indentation = 0;
			this.indent_value = 4;
			this.pretty_print = false;
			this.validate = true;
			this.ctx_stack = new Stack<WriterContext>();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0003DECC File Offset: 0x0003C0CC
		private static void IntToHex(int n, char[] hex)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = n % 16;
				if (num < 10)
				{
					hex[3 - i] = (char)(48 + num);
				}
				else
				{
					hex[3 - i] = (char)(65 + (num - 10));
				}
				n >>= 4;
			}
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0003DF1C File Offset: 0x0003C11C
		private void Indent()
		{
			if (this.pretty_print)
			{
				this.indentation += this.indent_value;
			}
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0003DF3C File Offset: 0x0003C13C
		private void Put(string str)
		{
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				for (int i = 0; i < this.indentation; i++)
				{
					this.writer.Write(' ');
				}
			}
			this.writer.Write(str);
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0003DF94 File Offset: 0x0003C194
		private void PutNewline()
		{
			this.PutNewline(true);
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x0003DFA0 File Offset: 0x0003C1A0
		private void PutNewline(bool add_comma)
		{
			if (add_comma && !this.context.ExpectingValue && this.context.Count > 1)
			{
				this.writer.Write(',');
			}
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				this.writer.Write('\n');
			}
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0003E00C File Offset: 0x0003C20C
		private void PutString(string str)
		{
			this.Put(string.Empty);
			this.writer.Write('"');
			int length = str.Length;
			for (int i = 0; i < length; i++)
			{
				char c = str[i];
				switch (c)
				{
				case '\b':
					this.writer.Write("\\b");
					break;
				case '\t':
					this.writer.Write("\\t");
					break;
				case '\n':
					this.writer.Write("\\n");
					break;
				default:
					if (c != '"' && c != '\\')
					{
						if (str[i] >= ' ' && str[i] <= '~')
						{
							this.writer.Write(str[i]);
						}
						else
						{
							JsonWriter.IntToHex((int)str[i], this.hex_seq);
							this.writer.Write("\\u");
							this.writer.Write(this.hex_seq);
						}
					}
					else
					{
						this.writer.Write('\\');
						this.writer.Write(str[i]);
					}
					break;
				case '\f':
					this.writer.Write("\\f");
					break;
				case '\r':
					this.writer.Write("\\r");
					break;
				}
			}
			this.writer.Write('"');
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x0003E188 File Offset: 0x0003C388
		private void Unindent()
		{
			if (this.pretty_print)
			{
				this.indentation -= this.indent_value;
			}
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0003E1A8 File Offset: 0x0003C3A8
		public override string ToString()
		{
			if (this.inst_string_builder == null)
			{
				return string.Empty;
			}
			return this.inst_string_builder.ToString();
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0003E1C8 File Offset: 0x0003C3C8
		public void Reset()
		{
			this.has_reached_end = false;
			this.ctx_stack.Clear();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
			if (this.inst_string_builder != null)
			{
				this.inst_string_builder.Remove(0, this.inst_string_builder.Length);
			}
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0003E228 File Offset: 0x0003C428
		public void Write(bool boolean)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put((!boolean) ? "false" : "true");
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x0003E26C File Offset: 0x0003C46C
		public void Write(decimal number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0003E2A4 File Offset: 0x0003C4A4
		public void Write(double number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			string text = Convert.ToString(number, JsonWriter.number_format);
			this.Put(text);
			if (text.IndexOf('.') == -1 && text.IndexOf('E') == -1)
			{
				this.writer.Write(".0");
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x0003E30C File Offset: 0x0003C50C
		public void Write(int number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x0003E344 File Offset: 0x0003C544
		public void Write(long number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0003E37C File Offset: 0x0003C57C
		public void Write(string str)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			if (str == null)
			{
				this.Put("null");
			}
			else
			{
				this.PutString(str);
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x0003E3C0 File Offset: 0x0003C5C0
		public void Write(ulong number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x0003E3F8 File Offset: 0x0003C5F8
		public void WriteArrayEnd()
		{
			this.DoValidation(Condition.InArray);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("]");
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0003E46C File Offset: 0x0003C66C
		public void WriteArrayStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("[");
			this.context = new WriterContext();
			this.context.InArray = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x0003E4C0 File Offset: 0x0003C6C0
		public void WriteObjectEnd()
		{
			this.DoValidation(Condition.InObject);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("}");
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x0003E534 File Offset: 0x0003C734
		public void WriteObjectStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("{");
			this.context = new WriterContext();
			this.context.InObject = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0003E588 File Offset: 0x0003C788
		public void WritePropertyName(string property_name)
		{
			this.DoValidation(Condition.Property);
			this.PutNewline();
			this.PutString(property_name);
			if (this.pretty_print)
			{
				if (property_name.Length > this.context.Padding)
				{
					this.context.Padding = property_name.Length;
				}
				for (int i = this.context.Padding - property_name.Length; i >= 0; i--)
				{
					this.writer.Write(' ');
				}
				this.writer.Write(": ");
			}
			else
			{
				this.writer.Write(':');
			}
			this.context.ExpectingValue = true;
		}

		// Token: 0x04000889 RID: 2185
		private static NumberFormatInfo number_format = NumberFormatInfo.InvariantInfo;

		// Token: 0x0400088A RID: 2186
		private WriterContext context;

		// Token: 0x0400088B RID: 2187
		private Stack<WriterContext> ctx_stack;

		// Token: 0x0400088C RID: 2188
		private bool has_reached_end;

		// Token: 0x0400088D RID: 2189
		private char[] hex_seq;

		// Token: 0x0400088E RID: 2190
		private int indentation;

		// Token: 0x0400088F RID: 2191
		private int indent_value;

		// Token: 0x04000890 RID: 2192
		private StringBuilder inst_string_builder;

		// Token: 0x04000891 RID: 2193
		private bool pretty_print;

		// Token: 0x04000892 RID: 2194
		private bool validate;

		// Token: 0x04000893 RID: 2195
		private TextWriter writer;
	}
}
