using System;
using System.Collections;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x02000496 RID: 1174
	internal abstract class Preferences : IEnumerable, ICollection<KeyValuePair<string, string>>, IDictionary<string, string>, IEnumerable<KeyValuePair<string, string>>
	{
		// Token: 0x060029E9 RID: 10729 RVA: 0x000DD234 File Offset: 0x000DB434
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060029EA RID: 10730
		protected abstract void AddCore(string key, string value);

		// Token: 0x060029EB RID: 10731
		protected abstract bool ContainsKeyCore(string key);

		// Token: 0x060029EC RID: 10732
		protected abstract void CopyToCore(KeyValuePair<string, string>[] array, int arrayIndex);

		// Token: 0x060029ED RID: 10733
		protected abstract bool RemoveCore(string key);

		// Token: 0x060029EE RID: 10734
		protected abstract bool TryGetValueCore(string key, out string value);

		// Token: 0x060029EF RID: 10735
		public abstract void Save();

		// Token: 0x060029F0 RID: 10736 RVA: 0x000DD23C File Offset: 0x000DB43C
		public void Add(string key, string value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this.AddCore(key, value);
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x000DD258 File Offset: 0x000DB458
		public bool ContainsKey(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this.ContainsKeyCore(key);
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x060029F2 RID: 10738
		public abstract ICollection<string> Keys { get; }

		// Token: 0x060029F3 RID: 10739 RVA: 0x000DD274 File Offset: 0x000DB474
		public bool Remove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this.RemoveCore(key);
		}

		// Token: 0x060029F4 RID: 10740 RVA: 0x000DD290 File Offset: 0x000DB490
		public bool TryGetValue(string key, out string value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this.TryGetValueCore(key, out value);
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x060029F5 RID: 10741
		public abstract ICollection<string> Values { get; }

		// Token: 0x17000737 RID: 1847
		public string this[string key]
		{
			get
			{
				string result;
				if (this.TryGetValue(key, out result))
				{
					return result;
				}
				throw new KeyNotFoundException();
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				this.AddCore(key, value);
			}
		}

		// Token: 0x060029F8 RID: 10744 RVA: 0x000DD2EC File Offset: 0x000DB4EC
		public void Add(KeyValuePair<string, string> item)
		{
			this.AddCore(item.Key, item.Value);
		}

		// Token: 0x060029F9 RID: 10745
		public abstract void Clear();

		// Token: 0x060029FA RID: 10746 RVA: 0x000DD304 File Offset: 0x000DB504
		public bool Contains(KeyValuePair<string, string> item)
		{
			if (item.Key == null)
			{
				throw new ArgumentException("Key is null.", "item");
			}
			string y;
			return this.TryGetValueCore(item.Key, out y) && EqualityComparer<string>.Default.Equals(item.Value, y);
		}

		// Token: 0x060029FB RID: 10747 RVA: 0x000DD358 File Offset: 0x000DB558
		public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			if (arrayIndex > array.Length)
			{
				throw new ArgumentException("Index larger than largest valid index of array.");
			}
			if (array.Length - arrayIndex < this.Count)
			{
				throw new ArgumentException("Destination array cannot hold the requested elements!");
			}
			this.CopyToCore(array, arrayIndex);
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x060029FC RID: 10748
		public abstract int Count { get; }

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x060029FD RID: 10749
		public abstract bool IsReadOnly { get; }

		// Token: 0x060029FE RID: 10750 RVA: 0x000DD3C0 File Offset: 0x000DB5C0
		public bool Remove(KeyValuePair<string, string> item)
		{
			return this.Contains(item) && this.Remove(item.Key);
		}

		// Token: 0x060029FF RID: 10751
		public abstract IEnumerator<KeyValuePair<string, string>> GetEnumerator();
	}
}
