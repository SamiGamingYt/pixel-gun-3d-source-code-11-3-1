using System;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x02000146 RID: 326
	internal struct ObjectMetadata
	{
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x0003B828 File Offset: 0x00039A28
		// (set) Token: 0x06000A69 RID: 2665 RVA: 0x0003B848 File Offset: 0x00039A48
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

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x0003B854 File Offset: 0x00039A54
		// (set) Token: 0x06000A6B RID: 2667 RVA: 0x0003B85C File Offset: 0x00039A5C
		public bool IsDictionary
		{
			get
			{
				return this.is_dictionary;
			}
			set
			{
				this.is_dictionary = value;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x0003B868 File Offset: 0x00039A68
		// (set) Token: 0x06000A6D RID: 2669 RVA: 0x0003B870 File Offset: 0x00039A70
		public IDictionary<string, PropertyMetadata> Properties
		{
			get
			{
				return this.properties;
			}
			set
			{
				this.properties = value;
			}
		}

		// Token: 0x04000836 RID: 2102
		private Type element_type;

		// Token: 0x04000837 RID: 2103
		private bool is_dictionary;

		// Token: 0x04000838 RID: 2104
		private IDictionary<string, PropertyMetadata> properties;
	}
}
