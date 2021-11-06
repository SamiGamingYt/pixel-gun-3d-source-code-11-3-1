using System;
using System.Collections;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x02000520 RID: 1312
	public class ObservableList<T> : List<T>, IEnumerable, IEnumerable<T>, IList<T>, ICollection<T>
	{
		// Token: 0x06002DB4 RID: 11700 RVA: 0x000F0654 File Offset: 0x000EE854
		public ObservableList()
		{
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x000F065C File Offset: 0x000EE85C
		public ObservableList(IEnumerable<T> collection)
		{
			base.AddRange(collection);
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x000F066C File Offset: 0x000EE86C
		public void Add(T item, bool silent = false)
		{
			base.Add(item);
			if (this.OnItemInserted != null && !silent)
			{
				this.OnItemInserted(this.Count - 1, item);
			}
		}

		// Token: 0x06002DB7 RID: 11703 RVA: 0x000F06A8 File Offset: 0x000EE8A8
		public void AddRange(IEnumerable<T> collection, bool silent = false)
		{
			base.AddRange(collection);
			if (this.OnItemInserted != null && !silent)
			{
				int num = 0;
				foreach (T arg in collection)
				{
					this.OnItemInserted(num, arg);
					num++;
				}
			}
		}

		// Token: 0x06002DB8 RID: 11704 RVA: 0x000F072C File Offset: 0x000EE92C
		public void Insert(int index, T item, bool silent = false)
		{
			base.Insert(index, item);
			if (this.OnItemInserted != null && !silent)
			{
				this.OnItemInserted(index, item);
			}
		}

		// Token: 0x06002DB9 RID: 11705 RVA: 0x000F0760 File Offset: 0x000EE960
		public void RemoveAt(int index, bool silent = false)
		{
			T arg = this[index];
			base.RemoveAt(index);
			if (this.OnItemRemoved != null && !silent)
			{
				this.OnItemRemoved(index, arg);
			}
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x000F079C File Offset: 0x000EE99C
		public void Remove(T item, bool silent = false)
		{
			int arg = base.IndexOf(item);
			base.Remove(item);
			if (this.OnItemRemoved != null && !silent)
			{
				this.OnItemRemoved(arg, item);
			}
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x000F07D8 File Offset: 0x000EE9D8
		public void RemoveRange(int idx, int count, bool silent = false)
		{
			if (this.OnItemRemoved != null && !silent)
			{
				List<T> list = new List<T>();
				for (int i = 0; i <= count; i++)
				{
					list.Add(base[idx + i]);
				}
				base.RemoveRange(idx, count);
				int num = 0;
				foreach (T arg in list)
				{
					this.OnItemRemoved(idx + num, arg);
					num++;
				}
			}
			else
			{
				base.RemoveRange(idx, count);
			}
		}

		// Token: 0x04002228 RID: 8744
		public Action<int, T> OnItemInserted;

		// Token: 0x04002229 RID: 8745
		public Action<int, T> OnItemRemoved;
	}
}
