using System;
using System.Collections;
using System.Collections.Specialized;

namespace LitJson
{
	// Token: 0x02000140 RID: 320
	public interface IJsonWrapper : IEnumerable, IList, IDictionary, ICollection, IOrderedDictionary
	{
		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060009E1 RID: 2529
		bool IsArray { get; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060009E2 RID: 2530
		bool IsBoolean { get; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060009E3 RID: 2531
		bool IsDouble { get; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060009E4 RID: 2532
		bool IsInt { get; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060009E5 RID: 2533
		bool IsLong { get; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060009E6 RID: 2534
		bool IsObject { get; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060009E7 RID: 2535
		bool IsString { get; }

		// Token: 0x060009E8 RID: 2536
		bool GetBoolean();

		// Token: 0x060009E9 RID: 2537
		double GetDouble();

		// Token: 0x060009EA RID: 2538
		int GetInt();

		// Token: 0x060009EB RID: 2539
		JsonType GetJsonType();

		// Token: 0x060009EC RID: 2540
		long GetLong();

		// Token: 0x060009ED RID: 2541
		string GetString();

		// Token: 0x060009EE RID: 2542
		void SetBoolean(bool val);

		// Token: 0x060009EF RID: 2543
		void SetDouble(double val);

		// Token: 0x060009F0 RID: 2544
		void SetInt(int val);

		// Token: 0x060009F1 RID: 2545
		void SetJsonType(JsonType type);

		// Token: 0x060009F2 RID: 2546
		void SetLong(long val);

		// Token: 0x060009F3 RID: 2547
		void SetString(string val);

		// Token: 0x060009F4 RID: 2548
		string ToJson();

		// Token: 0x060009F5 RID: 2549
		void ToJson(JsonWriter writer);
	}
}
