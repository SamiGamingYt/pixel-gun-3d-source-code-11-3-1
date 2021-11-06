using System;
using System.Collections;
using Rilisoft;

// Token: 0x02000565 RID: 1381
public class ArrayListWrapper
{
	// Token: 0x17000841 RID: 2113
	// (get) Token: 0x06002FDA RID: 12250 RVA: 0x000F9EAC File Offset: 0x000F80AC
	public int Count
	{
		get
		{
			int count;
			using (new ArrayListChecker(this._list, "_list"))
			{
				count = this._list.Count;
			}
			return count;
		}
	}

	// Token: 0x17000842 RID: 2114
	public object this[int index]
	{
		get
		{
			object result;
			using (new ArrayListChecker(this._list, "_list"))
			{
				result = this._list[index];
			}
			return result;
		}
		set
		{
			using (new ArrayListChecker(this._list, "_list"))
			{
				this._list[index] = value;
			}
		}
	}

	// Token: 0x06002FDD RID: 12253 RVA: 0x000F9FC8 File Offset: 0x000F81C8
	public void AddRange(ICollection c)
	{
		using (new ArrayListChecker(this._list, "_list"))
		{
			this._list.AddRange(c);
		}
	}

	// Token: 0x06002FDE RID: 12254 RVA: 0x000FA020 File Offset: 0x000F8220
	public int Add(object item)
	{
		int result;
		using (new ArrayListChecker(this._list, "_list"))
		{
			result = this._list.Add(item);
		}
		return result;
	}

	// Token: 0x06002FDF RID: 12255 RVA: 0x000FA080 File Offset: 0x000F8280
	public bool Contains(object item)
	{
		bool result;
		using (new ArrayListChecker(this._list, "_list"))
		{
			result = this._list.Contains(item);
		}
		return result;
	}

	// Token: 0x06002FE0 RID: 12256 RVA: 0x000FA0E0 File Offset: 0x000F82E0
	public Array ToArray(Type type)
	{
		Array result;
		using (new ArrayListChecker(this._list, "_list"))
		{
			result = this._list.ToArray(type);
		}
		return result;
	}

	// Token: 0x06002FE1 RID: 12257 RVA: 0x000FA140 File Offset: 0x000F8340
	public void RemoveAt(int index)
	{
		using (new ArrayListChecker(this._list, "_list"))
		{
			this._list.RemoveAt(index);
		}
	}

	// Token: 0x06002FE2 RID: 12258 RVA: 0x000FA198 File Offset: 0x000F8398
	public void Insert(int index, object obj)
	{
		using (new ArrayListChecker(this._list, "_list"))
		{
			this._list.Insert(index, obj);
		}
	}

	// Token: 0x04002331 RID: 9009
	private ArrayList _list = new ArrayList();
}
