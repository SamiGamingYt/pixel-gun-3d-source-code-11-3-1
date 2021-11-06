using System;

namespace LitJson
{
	// Token: 0x02000145 RID: 325
	internal struct ArrayMetadata
	{
		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x0003B7D4 File Offset: 0x000399D4
		// (set) Token: 0x06000A63 RID: 2659 RVA: 0x0003B7F4 File Offset: 0x000399F4
		public Type ElementType
		{
			get
			{
				if (this.element_type == null)
				{
					return typeof(JsonData);
				}
				return this.element_type;
			}
			set
			{
				this.element_type = value;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000A64 RID: 2660 RVA: 0x0003B800 File Offset: 0x00039A00
		// (set) Token: 0x06000A65 RID: 2661 RVA: 0x0003B808 File Offset: 0x00039A08
		public bool IsArray
		{
			get
			{
				return this.is_array;
			}
			set
			{
				this.is_array = value;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000A66 RID: 2662 RVA: 0x0003B814 File Offset: 0x00039A14
		// (set) Token: 0x06000A67 RID: 2663 RVA: 0x0003B81C File Offset: 0x00039A1C
		public bool IsList
		{
			get
			{
				return this.is_list;
			}
			set
			{
				this.is_list = value;
			}
		}

		// Token: 0x04000833 RID: 2099
		private Type element_type;

		// Token: 0x04000834 RID: 2100
		private bool is_array;

		// Token: 0x04000835 RID: 2101
		private bool is_list;
	}
}
