using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace LitJson
{
	// Token: 0x02000141 RID: 321
	public class JsonData : IEnumerable, IList, IDictionary, ICollection, IOrderedDictionary, IJsonWrapper, IEquatable<JsonData>
	{
		// Token: 0x060009F6 RID: 2550 RVA: 0x0003A6EC File Offset: 0x000388EC
		public JsonData()
		{
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0003A6F4 File Offset: 0x000388F4
		public JsonData(bool boolean)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = boolean;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0003A70C File Offset: 0x0003890C
		public JsonData(double number)
		{
			this.type = JsonType.Double;
			this.inst_double = number;
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0003A724 File Offset: 0x00038924
		public JsonData(int number)
		{
			this.type = JsonType.Int;
			this.inst_int = number;
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0003A73C File Offset: 0x0003893C
		public JsonData(long number)
		{
			this.type = JsonType.Long;
			this.inst_long = number;
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0003A754 File Offset: 0x00038954
		public JsonData(object obj)
		{
			if (obj is bool)
			{
				this.type = JsonType.Boolean;
				this.inst_boolean = (bool)obj;
				return;
			}
			if (obj is double)
			{
				this.type = JsonType.Double;
				this.inst_double = (double)obj;
				return;
			}
			if (obj is int)
			{
				this.type = JsonType.Int;
				this.inst_int = (int)obj;
				return;
			}
			if (obj is long)
			{
				this.type = JsonType.Long;
				this.inst_long = (long)obj;
				return;
			}
			if (obj is string)
			{
				this.type = JsonType.String;
				this.inst_string = (string)obj;
				return;
			}
			throw new ArgumentException("Unable to wrap the given object with JsonData");
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0003A80C File Offset: 0x00038A0C
		public JsonData(string str)
		{
			this.type = JsonType.String;
			this.inst_string = str;
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060009FD RID: 2557 RVA: 0x0003A824 File Offset: 0x00038A24
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x0003A82C File Offset: 0x00038A2C
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.EnsureCollection().IsSynchronized;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060009FF RID: 2559 RVA: 0x0003A83C File Offset: 0x00038A3C
		object ICollection.SyncRoot
		{
			get
			{
				return this.EnsureCollection().SyncRoot;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x0003A84C File Offset: 0x00038A4C
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this.EnsureDictionary().IsFixedSize;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x0003A85C File Offset: 0x00038A5C
		bool IDictionary.IsReadOnly
		{
			get
			{
				return this.EnsureDictionary().IsReadOnly;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x0003A86C File Offset: 0x00038A6C
		ICollection IDictionary.Keys
		{
			get
			{
				this.EnsureDictionary();
				IList<string> list = new List<string>();
				foreach (KeyValuePair<string, JsonData> keyValuePair in this.object_list)
				{
					list.Add(keyValuePair.Key);
				}
				return (ICollection)list;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000A03 RID: 2563 RVA: 0x0003A8E8 File Offset: 0x00038AE8
		ICollection IDictionary.Values
		{
			get
			{
				this.EnsureDictionary();
				IList<JsonData> list = new List<JsonData>();
				foreach (KeyValuePair<string, JsonData> keyValuePair in this.object_list)
				{
					list.Add(keyValuePair.Value);
				}
				return (ICollection)list;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0003A964 File Offset: 0x00038B64
		bool IJsonWrapper.IsArray
		{
			get
			{
				return this.IsArray;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x0003A96C File Offset: 0x00038B6C
		bool IJsonWrapper.IsBoolean
		{
			get
			{
				return this.IsBoolean;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000A06 RID: 2566 RVA: 0x0003A974 File Offset: 0x00038B74
		bool IJsonWrapper.IsDouble
		{
			get
			{
				return this.IsDouble;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x0003A97C File Offset: 0x00038B7C
		bool IJsonWrapper.IsInt
		{
			get
			{
				return this.IsInt;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000A08 RID: 2568 RVA: 0x0003A984 File Offset: 0x00038B84
		bool IJsonWrapper.IsLong
		{
			get
			{
				return this.IsLong;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000A09 RID: 2569 RVA: 0x0003A98C File Offset: 0x00038B8C
		bool IJsonWrapper.IsObject
		{
			get
			{
				return this.IsObject;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000A0A RID: 2570 RVA: 0x0003A994 File Offset: 0x00038B94
		bool IJsonWrapper.IsString
		{
			get
			{
				return this.IsString;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000A0B RID: 2571 RVA: 0x0003A99C File Offset: 0x00038B9C
		bool IList.IsFixedSize
		{
			get
			{
				return this.EnsureList().IsFixedSize;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000A0C RID: 2572 RVA: 0x0003A9AC File Offset: 0x00038BAC
		bool IList.IsReadOnly
		{
			get
			{
				return this.EnsureList().IsReadOnly;
			}
		}

		// Token: 0x1700011D RID: 285
		object IDictionary.this[object key]
		{
			get
			{
				return this.EnsureDictionary()[key];
			}
			set
			{
				if (!(key is string))
				{
					throw new ArgumentException("The key has to be a string");
				}
				JsonData value2 = this.ToJsonData(value);
				this[(string)key] = value2;
			}
		}

		// Token: 0x1700011E RID: 286
		object IOrderedDictionary.this[int idx]
		{
			get
			{
				this.EnsureDictionary();
				return this.object_list[idx].Value;
			}
			set
			{
				this.EnsureDictionary();
				JsonData value2 = this.ToJsonData(value);
				KeyValuePair<string, JsonData> keyValuePair = this.object_list[idx];
				this.inst_object[keyValuePair.Key] = value2;
				KeyValuePair<string, JsonData> value3 = new KeyValuePair<string, JsonData>(keyValuePair.Key, value2);
				this.object_list[idx] = value3;
			}
		}

		// Token: 0x1700011F RID: 287
		object IList.this[int index]
		{
			get
			{
				return this.EnsureList()[index];
			}
			set
			{
				this.EnsureList();
				JsonData value2 = this.ToJsonData(value);
				this[index] = value2;
			}
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0003AAB8 File Offset: 0x00038CB8
		void ICollection.CopyTo(Array array, int index)
		{
			this.EnsureCollection().CopyTo(array, index);
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x0003AAC8 File Offset: 0x00038CC8
		void IDictionary.Add(object key, object value)
		{
			JsonData value2 = this.ToJsonData(value);
			this.EnsureDictionary().Add(key, value2);
			KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>((string)key, value2);
			this.object_list.Add(item);
			this.json = null;
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0003AB0C File Offset: 0x00038D0C
		void IDictionary.Clear()
		{
			this.EnsureDictionary().Clear();
			this.object_list.Clear();
			this.json = null;
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x0003AB2C File Offset: 0x00038D2C
		bool IDictionary.Contains(object key)
		{
			return this.EnsureDictionary().Contains(key);
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x0003AB3C File Offset: 0x00038D3C
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IOrderedDictionary)this).GetEnumerator();
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x0003AB44 File Offset: 0x00038D44
		void IDictionary.Remove(object key)
		{
			this.EnsureDictionary().Remove(key);
			for (int i = 0; i < this.object_list.Count; i++)
			{
				if (this.object_list[i].Key == (string)key)
				{
					this.object_list.RemoveAt(i);
					break;
				}
			}
			this.json = null;
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0003ABB8 File Offset: 0x00038DB8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.EnsureCollection().GetEnumerator();
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0003ABC8 File Offset: 0x00038DC8
		bool IJsonWrapper.GetBoolean()
		{
			if (this.type != JsonType.Boolean)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a boolean");
			}
			return this.inst_boolean;
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0003ABE8 File Offset: 0x00038DE8
		double IJsonWrapper.GetDouble()
		{
			if (this.type != JsonType.Double)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a double");
			}
			return this.inst_double;
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0003AC08 File Offset: 0x00038E08
		int IJsonWrapper.GetInt()
		{
			if (this.type != JsonType.Int)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold an int");
			}
			return this.inst_int;
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x0003AC28 File Offset: 0x00038E28
		long IJsonWrapper.GetLong()
		{
			if (this.type != JsonType.Long)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a long");
			}
			return this.inst_long;
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0003AC48 File Offset: 0x00038E48
		string IJsonWrapper.GetString()
		{
			if (this.type != JsonType.String)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a string");
			}
			return this.inst_string;
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0003AC68 File Offset: 0x00038E68
		void IJsonWrapper.SetBoolean(bool val)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = val;
			this.json = null;
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0003AC80 File Offset: 0x00038E80
		void IJsonWrapper.SetDouble(double val)
		{
			this.type = JsonType.Double;
			this.inst_double = val;
			this.json = null;
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0003AC98 File Offset: 0x00038E98
		void IJsonWrapper.SetInt(int val)
		{
			this.type = JsonType.Int;
			this.inst_int = val;
			this.json = null;
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0003ACB0 File Offset: 0x00038EB0
		void IJsonWrapper.SetLong(long val)
		{
			this.type = JsonType.Long;
			this.inst_long = val;
			this.json = null;
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0003ACC8 File Offset: 0x00038EC8
		void IJsonWrapper.SetString(string val)
		{
			this.type = JsonType.String;
			this.inst_string = val;
			this.json = null;
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0003ACE0 File Offset: 0x00038EE0
		string IJsonWrapper.ToJson()
		{
			return this.ToJson();
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0003ACE8 File Offset: 0x00038EE8
		void IJsonWrapper.ToJson(JsonWriter writer)
		{
			this.ToJson(writer);
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0003ACF4 File Offset: 0x00038EF4
		int IList.Add(object value)
		{
			return this.Add(value);
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0003AD00 File Offset: 0x00038F00
		void IList.Clear()
		{
			this.EnsureList().Clear();
			this.json = null;
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0003AD14 File Offset: 0x00038F14
		bool IList.Contains(object value)
		{
			return this.EnsureList().Contains(value);
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0003AD24 File Offset: 0x00038F24
		int IList.IndexOf(object value)
		{
			return this.EnsureList().IndexOf(value);
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0003AD34 File Offset: 0x00038F34
		void IList.Insert(int index, object value)
		{
			this.EnsureList().Insert(index, value);
			this.json = null;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0003AD4C File Offset: 0x00038F4C
		void IList.Remove(object value)
		{
			this.EnsureList().Remove(value);
			this.json = null;
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0003AD64 File Offset: 0x00038F64
		void IList.RemoveAt(int index)
		{
			this.EnsureList().RemoveAt(index);
			this.json = null;
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0003AD7C File Offset: 0x00038F7C
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			this.EnsureDictionary();
			return new OrderedDictionaryEnumerator(this.object_list.GetEnumerator());
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x0003AD98 File Offset: 0x00038F98
		void IOrderedDictionary.Insert(int idx, object key, object value)
		{
			string text = (string)key;
			JsonData value2 = this.ToJsonData(value);
			this[text] = value2;
			KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>(text, value2);
			this.object_list.Insert(idx, item);
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0003ADD4 File Offset: 0x00038FD4
		void IOrderedDictionary.RemoveAt(int idx)
		{
			this.EnsureDictionary();
			this.inst_object.Remove(this.object_list[idx].Key);
			this.object_list.RemoveAt(idx);
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000A30 RID: 2608 RVA: 0x0003AE14 File Offset: 0x00039014
		public int Count
		{
			get
			{
				return this.EnsureCollection().Count;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x0003AE24 File Offset: 0x00039024
		public bool IsArray
		{
			get
			{
				return this.type == JsonType.Array;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000A32 RID: 2610 RVA: 0x0003AE30 File Offset: 0x00039030
		public bool IsBoolean
		{
			get
			{
				return this.type == JsonType.Boolean;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x0003AE3C File Offset: 0x0003903C
		public bool IsDouble
		{
			get
			{
				return this.type == JsonType.Double;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000A34 RID: 2612 RVA: 0x0003AE48 File Offset: 0x00039048
		public bool IsInt
		{
			get
			{
				return this.type == JsonType.Int;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x0003AE54 File Offset: 0x00039054
		public bool IsLong
		{
			get
			{
				return this.type == JsonType.Long;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000A36 RID: 2614 RVA: 0x0003AE60 File Offset: 0x00039060
		public bool IsObject
		{
			get
			{
				return this.type == JsonType.Object;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0003AE6C File Offset: 0x0003906C
		public bool IsString
		{
			get
			{
				return this.type == JsonType.String;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x0003AE78 File Offset: 0x00039078
		public ICollection<string> Keys
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object.Keys;
			}
		}

		// Token: 0x17000129 RID: 297
		public JsonData this[string prop_name]
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object[prop_name];
			}
			set
			{
				this.EnsureDictionary();
				KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(prop_name, value);
				if (this.inst_object.ContainsKey(prop_name))
				{
					for (int i = 0; i < this.object_list.Count; i++)
					{
						if (this.object_list[i].Key == prop_name)
						{
							this.object_list[i] = keyValuePair;
							break;
						}
					}
				}
				else
				{
					this.object_list.Add(keyValuePair);
				}
				this.inst_object[prop_name] = value;
				this.json = null;
			}
		}

		// Token: 0x1700012A RID: 298
		public JsonData this[int index]
		{
			get
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					return this.inst_array[index];
				}
				return this.object_list[index].Value;
			}
			set
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					this.inst_array[index] = value;
				}
				else
				{
					KeyValuePair<string, JsonData> keyValuePair = this.object_list[index];
					KeyValuePair<string, JsonData> value2 = new KeyValuePair<string, JsonData>(keyValuePair.Key, value);
					this.object_list[index] = value2;
					this.inst_object[keyValuePair.Key] = value;
				}
				this.json = null;
			}
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0003B000 File Offset: 0x00039200
		private ICollection EnsureCollection()
		{
			if (this.type == JsonType.Array)
			{
				return (ICollection)this.inst_array;
			}
			if (this.type == JsonType.Object)
			{
				return (ICollection)this.inst_object;
			}
			throw new InvalidOperationException("The JsonData instance has to be initialized first");
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x0003B048 File Offset: 0x00039248
		private IDictionary EnsureDictionary()
		{
			if (this.type == JsonType.Object)
			{
				return (IDictionary)this.inst_object;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a dictionary");
			}
			this.type = JsonType.Object;
			this.inst_object = new Dictionary<string, JsonData>();
			this.object_list = new List<KeyValuePair<string, JsonData>>();
			return (IDictionary)this.inst_object;
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0003B0AC File Offset: 0x000392AC
		private IList EnsureList()
		{
			if (this.type == JsonType.Array)
			{
				return (IList)this.inst_array;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a list");
			}
			this.type = JsonType.Array;
			this.inst_array = new List<JsonData>();
			return (IList)this.inst_array;
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0003B104 File Offset: 0x00039304
		private JsonData ToJsonData(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is JsonData)
			{
				return (JsonData)obj;
			}
			return new JsonData(obj);
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0003B134 File Offset: 0x00039334
		private static void WriteJson(IJsonWrapper obj, JsonWriter writer)
		{
			if (obj == null)
			{
				writer.Write(null);
				return;
			}
			if (obj.IsString)
			{
				writer.Write(obj.GetString());
				return;
			}
			if (obj.IsBoolean)
			{
				writer.Write(obj.GetBoolean());
				return;
			}
			if (obj.IsDouble)
			{
				writer.Write(obj.GetDouble());
				return;
			}
			if (obj.IsInt)
			{
				writer.Write(obj.GetInt());
				return;
			}
			if (obj.IsLong)
			{
				writer.Write(obj.GetLong());
				return;
			}
			if (obj.IsArray)
			{
				writer.WriteArrayStart();
				foreach (object obj2 in obj)
				{
					JsonData.WriteJson((JsonData)obj2, writer);
				}
				writer.WriteArrayEnd();
				return;
			}
			if (obj.IsObject)
			{
				writer.WriteObjectStart();
				foreach (object obj3 in obj)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
					writer.WritePropertyName((string)dictionaryEntry.Key);
					JsonData.WriteJson((JsonData)dictionaryEntry.Value, writer);
				}
				writer.WriteObjectEnd();
				return;
			}
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0003B2D0 File Offset: 0x000394D0
		public int Add(object value)
		{
			JsonData value2 = this.ToJsonData(value);
			this.json = null;
			return this.EnsureList().Add(value2);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0003B2F8 File Offset: 0x000394F8
		public void Clear()
		{
			if (this.IsObject)
			{
				((IDictionary)this).Clear();
				return;
			}
			if (this.IsArray)
			{
				((IList)this).Clear();
				return;
			}
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0003B32C File Offset: 0x0003952C
		public bool Equals(JsonData x)
		{
			if (x == null)
			{
				return false;
			}
			if (x.type != this.type)
			{
				return false;
			}
			switch (this.type)
			{
			case JsonType.None:
				return true;
			case JsonType.Object:
				return this.inst_object.Equals(x.inst_object);
			case JsonType.Array:
				return this.inst_array.Equals(x.inst_array);
			case JsonType.String:
				return this.inst_string.Equals(x.inst_string);
			case JsonType.Int:
				return this.inst_int.Equals(x.inst_int);
			case JsonType.Long:
				return this.inst_long.Equals(x.inst_long);
			case JsonType.Double:
				return this.inst_double.Equals(x.inst_double);
			case JsonType.Boolean:
				return this.inst_boolean.Equals(x.inst_boolean);
			default:
				return false;
			}
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0003B408 File Offset: 0x00039608
		public JsonType GetJsonType()
		{
			return this.type;
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0003B410 File Offset: 0x00039610
		public void SetJsonType(JsonType type)
		{
			if (this.type == type)
			{
				return;
			}
			switch (type)
			{
			case JsonType.Object:
				this.inst_object = new Dictionary<string, JsonData>();
				this.object_list = new List<KeyValuePair<string, JsonData>>();
				break;
			case JsonType.Array:
				this.inst_array = new List<JsonData>();
				break;
			case JsonType.String:
				this.inst_string = null;
				break;
			case JsonType.Int:
				this.inst_int = 0;
				break;
			case JsonType.Long:
				this.inst_long = 0L;
				break;
			case JsonType.Double:
				this.inst_double = 0.0;
				break;
			case JsonType.Boolean:
				this.inst_boolean = false;
				break;
			}
			this.type = type;
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0003B4D4 File Offset: 0x000396D4
		public string ToJson()
		{
			if (this.json != null)
			{
				return this.json;
			}
			StringWriter stringWriter = new StringWriter();
			JsonData.WriteJson(this, new JsonWriter(stringWriter)
			{
				Validate = false
			});
			this.json = stringWriter.ToString();
			return this.json;
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0003B520 File Offset: 0x00039720
		public void ToJson(JsonWriter writer)
		{
			bool validate = writer.Validate;
			writer.Validate = false;
			JsonData.WriteJson(this, writer);
			writer.Validate = validate;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0003B54C File Offset: 0x0003974C
		public override string ToString()
		{
			switch (this.type)
			{
			case JsonType.Object:
				return "JsonData object";
			case JsonType.Array:
				return "JsonData array";
			case JsonType.String:
				return this.inst_string;
			case JsonType.Int:
				return this.inst_int.ToString();
			case JsonType.Long:
				return this.inst_long.ToString();
			case JsonType.Double:
				return this.inst_double.ToString();
			case JsonType.Boolean:
				return this.inst_boolean.ToString();
			default:
				return "Uninitialized JsonData";
			}
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0003B5D4 File Offset: 0x000397D4
		public static implicit operator JsonData(bool data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0003B5DC File Offset: 0x000397DC
		public static implicit operator JsonData(double data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0003B5E4 File Offset: 0x000397E4
		public static implicit operator JsonData(int data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0003B5EC File Offset: 0x000397EC
		public static implicit operator JsonData(long data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0003B5F4 File Offset: 0x000397F4
		public static implicit operator JsonData(string data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0003B5FC File Offset: 0x000397FC
		public static explicit operator bool(JsonData data)
		{
			if (data.type != JsonType.Boolean)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_boolean;
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0003B61C File Offset: 0x0003981C
		public static explicit operator double(JsonData data)
		{
			if (data.type != JsonType.Double)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_double;
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0003B63C File Offset: 0x0003983C
		public static explicit operator int(JsonData data)
		{
			if (data.type != JsonType.Int)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_int;
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x0003B65C File Offset: 0x0003985C
		public static explicit operator long(JsonData data)
		{
			if (data.type != JsonType.Long)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_long;
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0003B67C File Offset: 0x0003987C
		public static explicit operator string(JsonData data)
		{
			if (data.type != JsonType.String)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a string");
			}
			return data.inst_string;
		}

		// Token: 0x04000825 RID: 2085
		private IList<JsonData> inst_array;

		// Token: 0x04000826 RID: 2086
		private bool inst_boolean;

		// Token: 0x04000827 RID: 2087
		private double inst_double;

		// Token: 0x04000828 RID: 2088
		private int inst_int;

		// Token: 0x04000829 RID: 2089
		private long inst_long;

		// Token: 0x0400082A RID: 2090
		private IDictionary<string, JsonData> inst_object;

		// Token: 0x0400082B RID: 2091
		private string inst_string;

		// Token: 0x0400082C RID: 2092
		private string json;

		// Token: 0x0400082D RID: 2093
		private JsonType type;

		// Token: 0x0400082E RID: 2094
		private IList<KeyValuePair<string, JsonData>> object_list;
	}
}
