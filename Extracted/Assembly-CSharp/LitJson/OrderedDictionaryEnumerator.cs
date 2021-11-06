using System;
using System.Collections;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x02000142 RID: 322
	internal class OrderedDictionaryEnumerator : IEnumerator, IDictionaryEnumerator
	{
		// Token: 0x06000A54 RID: 2644 RVA: 0x0003B69C File Offset: 0x0003989C
		public OrderedDictionaryEnumerator(IEnumerator<KeyValuePair<string, JsonData>> enumerator)
		{
			this.list_enumerator = enumerator;
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x0003B6AC File Offset: 0x000398AC
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x0003B6BC File Offset: 0x000398BC
		public DictionaryEntry Entry
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return new DictionaryEntry(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000A57 RID: 2647 RVA: 0x0003B6E8 File Offset: 0x000398E8
		public object Key
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Key;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000A58 RID: 2648 RVA: 0x0003B708 File Offset: 0x00039908
		public object Value
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Value;
			}
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0003B728 File Offset: 0x00039928
		public bool MoveNext()
		{
			return this.list_enumerator.MoveNext();
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x0003B738 File Offset: 0x00039938
		public void Reset()
		{
			this.list_enumerator.Reset();
		}

		// Token: 0x0400082F RID: 2095
		private IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;
	}
}
