using System;
using System.Collections.Generic;
using Facebook.MiniJSON;

namespace Facebook.Unity
{
	// Token: 0x02000108 RID: 264
	internal abstract class ResultBase : IInternalResult, IResult
	{
		// Token: 0x060007AE RID: 1966 RVA: 0x0002F70C File Offset: 0x0002D90C
		internal ResultBase(string result)
		{
			string error = null;
			bool cancelled = false;
			string callbackId = null;
			if (!string.IsNullOrEmpty(result))
			{
				Dictionary<string, object> dictionary = Json.Deserialize(result) as Dictionary<string, object>;
				if (dictionary != null)
				{
					this.ResultDictionary = dictionary;
					error = ResultBase.GetErrorValue(dictionary);
					cancelled = ResultBase.GetCancelledValue(dictionary);
					callbackId = ResultBase.GetCallbackId(dictionary);
				}
			}
			this.Init(result, error, cancelled, callbackId);
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0002F768 File Offset: 0x0002D968
		internal ResultBase(string result, string error, bool cancelled)
		{
			this.Init(result, error, cancelled, null);
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x0002F77C File Offset: 0x0002D97C
		// (set) Token: 0x060007B1 RID: 1969 RVA: 0x0002F784 File Offset: 0x0002D984
		public virtual string Error { get; protected set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x0002F790 File Offset: 0x0002D990
		// (set) Token: 0x060007B3 RID: 1971 RVA: 0x0002F798 File Offset: 0x0002D998
		public virtual IDictionary<string, object> ResultDictionary { get; protected set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x0002F7A4 File Offset: 0x0002D9A4
		// (set) Token: 0x060007B5 RID: 1973 RVA: 0x0002F7AC File Offset: 0x0002D9AC
		public virtual string RawResult { get; protected set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0002F7B8 File Offset: 0x0002D9B8
		// (set) Token: 0x060007B7 RID: 1975 RVA: 0x0002F7C0 File Offset: 0x0002D9C0
		public virtual bool Cancelled { get; protected set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x0002F7CC File Offset: 0x0002D9CC
		// (set) Token: 0x060007B9 RID: 1977 RVA: 0x0002F7D4 File Offset: 0x0002D9D4
		public virtual string CallbackId { get; protected set; }

		// Token: 0x060007BA RID: 1978 RVA: 0x0002F7E0 File Offset: 0x0002D9E0
		public override string ToString()
		{
			return string.Format("[BaseResult: Error={0}, Result={1}, RawResult={2}, Cancelled={3}]", new object[]
			{
				this.Error,
				this.ResultDictionary,
				this.RawResult,
				this.Cancelled
			});
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0002F828 File Offset: 0x0002DA28
		protected void Init(string result, string error, bool cancelled, string callbackId)
		{
			this.RawResult = result;
			this.Cancelled = cancelled;
			this.Error = error;
			this.CallbackId = callbackId;
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0002F854 File Offset: 0x0002DA54
		private static string GetErrorValue(IDictionary<string, object> result)
		{
			if (result == null)
			{
				return null;
			}
			string result2;
			if (result.TryGetValue("error", out result2))
			{
				return result2;
			}
			return null;
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0002F880 File Offset: 0x0002DA80
		private static bool GetCancelledValue(IDictionary<string, object> result)
		{
			if (result == null)
			{
				return false;
			}
			object obj;
			if (result.TryGetValue("cancelled", out obj))
			{
				bool? flag = obj as bool?;
				if (flag != null)
				{
					return flag != null && flag.Value;
				}
				string text = obj as string;
				if (text != null)
				{
					return Convert.ToBoolean(text);
				}
				int? num = obj as int?;
				if (num != null)
				{
					return num != null && num.Value != 0;
				}
			}
			return false;
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0002F920 File Offset: 0x0002DB20
		private static string GetCallbackId(IDictionary<string, object> result)
		{
			if (result == null)
			{
				return null;
			}
			string result2;
			if (result.TryGetValue("callback_id", out result2))
			{
				return result2;
			}
			return null;
		}
	}
}
