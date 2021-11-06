using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200035E RID: 862
public class BetterList<T>
{
	// Token: 0x06001D81 RID: 7553 RVA: 0x0007EC68 File Offset: 0x0007CE68
	public IEnumerator<T> GetEnumerator()
	{
		if (this.buffer != null)
		{
			for (int i = 0; i < this.size; i++)
			{
				yield return this.buffer[i];
			}
		}
		yield break;
	}

	// Token: 0x1700050A RID: 1290
	[DebuggerHidden]
	public T this[int i]
	{
		get
		{
			return this.buffer[i];
		}
		set
		{
			this.buffer[i] = value;
		}
	}

	// Token: 0x06001D84 RID: 7556 RVA: 0x0007ECA4 File Offset: 0x0007CEA4
	private void AllocateMore()
	{
		T[] array = (this.buffer == null) ? new T[32] : new T[Mathf.Max(this.buffer.Length << 1, 32)];
		if (this.buffer != null && this.size > 0)
		{
			this.buffer.CopyTo(array, 0);
		}
		this.buffer = array;
	}

	// Token: 0x06001D85 RID: 7557 RVA: 0x0007ED0C File Offset: 0x0007CF0C
	private void Trim()
	{
		if (this.size > 0)
		{
			if (this.size < this.buffer.Length)
			{
				T[] array = new T[this.size];
				for (int i = 0; i < this.size; i++)
				{
					array[i] = this.buffer[i];
				}
				this.buffer = array;
			}
		}
		else
		{
			this.buffer = null;
		}
	}

	// Token: 0x06001D86 RID: 7558 RVA: 0x0007ED84 File Offset: 0x0007CF84
	public void Clear()
	{
		this.size = 0;
	}

	// Token: 0x06001D87 RID: 7559 RVA: 0x0007ED90 File Offset: 0x0007CF90
	public void Release()
	{
		this.size = 0;
		this.buffer = null;
	}

	// Token: 0x06001D88 RID: 7560 RVA: 0x0007EDA0 File Offset: 0x0007CFA0
	public void Add(T item)
	{
		if (this.buffer == null || this.size == this.buffer.Length)
		{
			this.AllocateMore();
		}
		this.buffer[this.size++] = item;
	}

	// Token: 0x06001D89 RID: 7561 RVA: 0x0007EDF0 File Offset: 0x0007CFF0
	public void Insert(int index, T item)
	{
		if (this.buffer == null || this.size == this.buffer.Length)
		{
			this.AllocateMore();
		}
		if (index > -1 && index < this.size)
		{
			for (int i = this.size; i > index; i--)
			{
				this.buffer[i] = this.buffer[i - 1];
			}
			this.buffer[index] = item;
			this.size++;
		}
		else
		{
			this.Add(item);
		}
	}

	// Token: 0x06001D8A RID: 7562 RVA: 0x0007EE8C File Offset: 0x0007D08C
	public bool Contains(T item)
	{
		if (this.buffer == null)
		{
			return false;
		}
		for (int i = 0; i < this.size; i++)
		{
			if (this.buffer[i].Equals(item))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001D8B RID: 7563 RVA: 0x0007EEE4 File Offset: 0x0007D0E4
	public int IndexOf(T item)
	{
		if (this.buffer == null)
		{
			return -1;
		}
		for (int i = 0; i < this.size; i++)
		{
			if (this.buffer[i].Equals(item))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06001D8C RID: 7564 RVA: 0x0007EF3C File Offset: 0x0007D13C
	public bool Remove(T item)
	{
		if (this.buffer != null)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < this.size; i++)
			{
				if (@default.Equals(this.buffer[i], item))
				{
					this.size--;
					this.buffer[i] = default(T);
					for (int j = i; j < this.size; j++)
					{
						this.buffer[j] = this.buffer[j + 1];
					}
					this.buffer[this.size] = default(T);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001D8D RID: 7565 RVA: 0x0007EFFC File Offset: 0x0007D1FC
	public void RemoveAt(int index)
	{
		if (this.buffer != null && index > -1 && index < this.size)
		{
			this.size--;
			this.buffer[index] = default(T);
			for (int i = index; i < this.size; i++)
			{
				this.buffer[i] = this.buffer[i + 1];
			}
			this.buffer[this.size] = default(T);
		}
	}

	// Token: 0x06001D8E RID: 7566 RVA: 0x0007F098 File Offset: 0x0007D298
	public T Pop()
	{
		if (this.buffer != null && this.size != 0)
		{
			T result = this.buffer[--this.size];
			this.buffer[this.size] = default(T);
			return result;
		}
		return default(T);
	}

	// Token: 0x06001D8F RID: 7567 RVA: 0x0007F100 File Offset: 0x0007D300
	public T[] ToArray()
	{
		this.Trim();
		return this.buffer;
	}

	// Token: 0x06001D90 RID: 7568 RVA: 0x0007F110 File Offset: 0x0007D310
	[DebuggerHidden]
	[DebuggerStepThrough]
	public void Sort(BetterList<T>.CompareFunc comparer)
	{
		int num = 0;
		int num2 = this.size - 1;
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int i = num; i < num2; i++)
			{
				if (comparer(this.buffer[i], this.buffer[i + 1]) > 0)
				{
					T t = this.buffer[i];
					this.buffer[i] = this.buffer[i + 1];
					this.buffer[i + 1] = t;
					flag = true;
				}
				else if (!flag)
				{
					num = ((i != 0) ? (i - 1) : 0);
				}
			}
		}
	}

	// Token: 0x040012D4 RID: 4820
	public T[] buffer;

	// Token: 0x040012D5 RID: 4821
	public int size;

	// Token: 0x020008F7 RID: 2295
	// (Invoke) Token: 0x0600507C RID: 20604
	public delegate int CompareFunc(T left, T right);
}
