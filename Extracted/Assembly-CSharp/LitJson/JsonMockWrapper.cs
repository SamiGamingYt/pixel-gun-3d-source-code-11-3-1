using System;
using System.Collections;
using System.Collections.Specialized;

namespace LitJson
{
	// Token: 0x02000148 RID: 328
	public class JsonMockWrapper : IEnumerable, IList, IDictionary, ICollection, IOrderedDictionary, IJsonWrapper
	{
		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x0003D144 File Offset: 0x0003B344
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x0003D148 File Offset: 0x0003B348
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000137 RID: 311
		object IList.this[int index]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0003D154 File Offset: 0x0003B354
		int IList.Add(object value)
		{
			return 0;
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0003D158 File Offset: 0x0003B358
		void IList.Clear()
		{
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0003D15C File Offset: 0x0003B35C
		bool IList.Contains(object value)
		{
			return false;
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0003D160 File Offset: 0x0003B360
		int IList.IndexOf(object value)
		{
			return -1;
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0003D164 File Offset: 0x0003B364
		void IList.Insert(int i, object v)
		{
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0003D168 File Offset: 0x0003B368
		void IList.Remove(object value)
		{
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x0003D16C File Offset: 0x0003B36C
		void IList.RemoveAt(int index)
		{
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x0003D170 File Offset: 0x0003B370
		int ICollection.Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000AAF RID: 2735 RVA: 0x0003D174 File Offset: 0x0003B374
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x0003D178 File Offset: 0x0003B378
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0003D17C File Offset: 0x0003B37C
		void ICollection.CopyTo(Array array, int index)
		{
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x0003D180 File Offset: 0x0003B380
		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x0003D184 File Offset: 0x0003B384
		bool IDictionary.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x0003D188 File Offset: 0x0003B388
		bool IDictionary.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x0003D18C File Offset: 0x0003B38C
		ICollection IDictionary.Keys
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x0003D190 File Offset: 0x0003B390
		ICollection IDictionary.Values
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700013F RID: 319
		object IDictionary.this[object key]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0003D19C File Offset: 0x0003B39C
		void IDictionary.Add(object k, object v)
		{
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0003D1A0 File Offset: 0x0003B3A0
		void IDictionary.Clear()
		{
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0003D1A4 File Offset: 0x0003B3A4
		bool IDictionary.Contains(object key)
		{
			return false;
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0003D1A8 File Offset: 0x0003B3A8
		void IDictionary.Remove(object key)
		{
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x0003D1AC File Offset: 0x0003B3AC
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return null;
		}

		// Token: 0x17000140 RID: 320
		object IOrderedDictionary.this[int idx]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0003D1B8 File Offset: 0x0003B3B8
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			return null;
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x0003D1BC File Offset: 0x0003B3BC
		void IOrderedDictionary.Insert(int i, object k, object v)
		{
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0003D1C0 File Offset: 0x0003B3C0
		void IOrderedDictionary.RemoveAt(int i)
		{
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x0003D1C4 File Offset: 0x0003B3C4
		public bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x0003D1C8 File Offset: 0x0003B3C8
		public bool IsBoolean
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x0003D1CC File Offset: 0x0003B3CC
		public bool IsDouble
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x0003D1D0 File Offset: 0x0003B3D0
		public bool IsInt
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x0003D1D4 File Offset: 0x0003B3D4
		public bool IsLong
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x0003D1D8 File Offset: 0x0003B3D8
		public bool IsObject
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x0003D1DC File Offset: 0x0003B3DC
		public bool IsString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x0003D1E0 File Offset: 0x0003B3E0
		public bool GetBoolean()
		{
			return false;
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0003D1E4 File Offset: 0x0003B3E4
		public double GetDouble()
		{
			return 0.0;
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0003D1F0 File Offset: 0x0003B3F0
		public int GetInt()
		{
			return 0;
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0003D1F4 File Offset: 0x0003B3F4
		public JsonType GetJsonType()
		{
			return JsonType.None;
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x0003D1F8 File Offset: 0x0003B3F8
		public long GetLong()
		{
			return 0L;
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x0003D1FC File Offset: 0x0003B3FC
		public string GetString()
		{
			return string.Empty;
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x0003D204 File Offset: 0x0003B404
		public void SetBoolean(bool val)
		{
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0003D208 File Offset: 0x0003B408
		public void SetDouble(double val)
		{
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0003D20C File Offset: 0x0003B40C
		public void SetInt(int val)
		{
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0003D210 File Offset: 0x0003B410
		public void SetJsonType(JsonType type)
		{
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x0003D214 File Offset: 0x0003B414
		public void SetLong(long val)
		{
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x0003D218 File Offset: 0x0003B418
		public void SetString(string val)
		{
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0003D21C File Offset: 0x0003B41C
		public string ToJson()
		{
			return string.Empty;
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0003D224 File Offset: 0x0003B424
		public void ToJson(JsonWriter writer)
		{
		}
	}
}
